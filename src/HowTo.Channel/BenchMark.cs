using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using System;
using System.Buffers;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HowTo.Channel
{
    [Config(typeof(Config))]
    public class BenchMark
    {
        [Params(10, 100, 1000)]
        public int Capacity { get; set; }
        private int ProducerCount = 1;

        [Benchmark]
        public async Task<int> ChannelPerf()
        {
            var channel = CreateChannel();
            var itemsToProduce = Capacity / ProducerCount;

            var producerFactory = new ProducerFactory(channel.Writer, itemsToProduce);
            var subscriberFactory = new ConsumerFactory(channel.Reader);

            var prodThread = producerFactory.Start();
            var subsThread = subscriberFactory.Start();

            await Task.WhenAll(prodThread, subsThread);

            return subsThread.Result;
        }

        private Channel<byte[]> CreateChannel()
        {
            return System.Threading.Channels.Channel.CreateBounded<byte[]>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.DropWrite,
                SingleReader = true,
                SingleWriter = true
            });

        }

        private class Config : ManualConfig
        {
            public Config()
            {
                AddColumn(StatisticColumn.OperationsPerSecond);
                AddJob(Job.Default.WithGcMode(new GcMode(){ Force = false }));
            }
        }
    }
}

public class ByteProducer
{
    private readonly int _itemsToProduce;
    private readonly ChannelWriter<byte[]> _writer;

    public ByteProducer(ChannelWriter<byte[]> writer, int itemsToProduce)
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
            
            while (i < itemsToProduce && writer.TryWrite(ArrayPool<byte>.Shared.Rent(1400)))
            {
                i++;
            }
        }
    }
}

public class ByteSubscriber
{
    private readonly ChannelReader<byte[]> _reader;

    public ByteSubscriber(ChannelReader<byte[]> reader)
    {
        _reader = reader;
    }

    public async Task<int> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        var reader = _reader;
        var i = 0;
        while (await reader.WaitToReadAsync(cancellationToken))
        {
            while (reader.TryRead(out var data))
            {
                i++;

                //Some sort of conversion in here

                ArrayPool<byte>.Shared.Return(data, true);
            }
        }

        return i;
    }
}

public class ProducerFactory
{
    private readonly ChannelWriter<byte[]> _writer;
    private readonly int _numberOfItems;

    public ProducerFactory(ChannelWriter<byte[]> writer, int numberOfItems)
    {
        _writer = writer;
        _numberOfItems = numberOfItems;
    }

    public async Task Start(CancellationToken ct = default)
    {
        var producer = new ByteProducer(_writer, _numberOfItems);
        var t = Task.Run(async () => await producer.ProduceAsync());

        await t;
        _writer.Complete();
    }
}

public class ConsumerFactory
{
    private readonly ChannelReader<byte[]> _reader;
    private readonly int _numberOfItems;

    public ConsumerFactory(ChannelReader<byte[]> reader)
    {
        _reader = reader;
    }

    public async Task<int> Start(CancellationToken ct = default)
    {
        var consumer = new ByteSubscriber(_reader);
        var t = Task.Run(async () => await consumer.SubscribeAsync());

        return await t;
    }

}
