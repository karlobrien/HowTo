using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HowTo.Channel
{
    [MemoryDiagnoser]
    [Config(typeof(Config))]
    public class BenchmarkInts
    {
        [Params(10, 100, 1000)]
        public int Capacity { get; set; }
        private int ProducerCount = 1;

        [Benchmark]
        public async Task<int> ChannelPerf()
        {
            var channel = CreateUnboundedChannel();//CreateChannel();
            var itemsToProduce = Capacity / ProducerCount;

            var producerFactory = new ProducerIntFactory(channel.Writer, itemsToProduce);
            var subscriberFactory = new ConsumerIntFactory(channel.Reader);

            var prodThread = producerFactory.Start();
            var subsThread = subscriberFactory.Start();

            await Task.WhenAll(prodThread, subsThread);

            return subsThread.Result;
        }

        private Channel<int> CreateChannel()
        {
            return System.Threading.Channels.Channel.CreateBounded<int>(new BoundedChannelOptions(Capacity)
            {
                FullMode = BoundedChannelFullMode.DropWrite,
                SingleReader = true,
                SingleWriter = true
            });
        }

        private Channel<int> CreateUnboundedChannel()
        {
            return System.Threading.Channels.Channel.CreateUnbounded<int>(new UnboundedChannelOptions()
            {
                SingleReader = true,
                SingleWriter = true
            });
        }

        private class Config : ManualConfig
        {
            public Config()
            {
                AddColumn(StatisticColumn.OperationsPerSecond);
                AddJob(Job.Default.WithGcMode(new GcMode() { Force = false }));
            }
        }
    }
}

public class IntProducer
{
    private readonly int _itemsToProduce;
    private readonly ChannelWriter<int> _writer;

    public IntProducer(ChannelWriter<int> writer, int itemsToProduce)
    {
        _writer = writer;
        _itemsToProduce = itemsToProduce;
    }

    public async Task ProduceAsync(CancellationToken cancellationToken = default)
    {
        var itemsToProduce = _itemsToProduce;
        var writer = _writer;

        var i = 0;

        while (i < itemsToProduce && await writer.WaitToWriteAsync(cancellationToken))
        {
            while (i < itemsToProduce && writer.TryWrite(i))
            {
                i++;
            }
        }
    }
}

public class IntSubscriber
{
    private readonly ChannelReader<int> _reader;

    public IntSubscriber(ChannelReader<int> reader)
    {
        _reader = reader;
    }

    public async Task<int> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        var reader = _reader;
        var i = 0;
        while (await reader.WaitToReadAsync(cancellationToken))
        {
            while (reader.TryRead(out _))
            {
                i++;

                //Some sort of conversion in here
            }
        }

        return i;
    }
}

public class ProducerIntFactory
{
    private readonly ChannelWriter<int> _writer;
    private readonly int _numberOfItems;

    public ProducerIntFactory(ChannelWriter<int> writer, int numberOfItems)
    {
        _writer = writer;
        _numberOfItems = numberOfItems;
    }

    public async Task Start(CancellationToken ct = default)
    {
        var producer = new IntProducer(_writer, _numberOfItems);
        var t = Task.Run(async () => await producer.ProduceAsync());

        await t;
        _writer.Complete();
    }
}

public class ConsumerIntFactory
{
    private readonly ChannelReader<int> _reader;
    private readonly int _numberOfItems;

    public ConsumerIntFactory(ChannelReader<int> reader)
    {
        _reader = reader;
    }

    public async Task<int> Start(CancellationToken ct = default)
    {
        var consumer = new IntSubscriber(_reader);
        var result = await Task.Factory.StartNew(async () => await consumer.SubscribeAsync(),
            CancellationToken.None, 
            TaskCreationOptions.DenyChildAttach, 
            TaskScheduler.Default);

        return result.Result;
    }
}
