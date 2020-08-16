using BenchmarkDotNet.Running;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HowTo.Channel
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting!");
            BenchmarkRunner.Run<BenchmarkInts>();
            //TestOutChannelDropWrite();
            //TestOutChannelWait();

            //var channel = System.Threading.Channels.Channel.CreateBounded<long>(new BoundedChannelOptions(1)
            //{
            //    SingleReader = true,
            //    SingleWriter = true,
            //    FullMode = BoundedChannelFullMode.DropWrite
            //});

            //var producer = new Producer(channel.Writer, 1);
            //var consumer = new Consumer(channel.Reader, 2);

            //CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            //var prodTask = producer.Start(cts.Token);
            //var consumeTask = consumer.Consume(cts.Token);

            //await Task.WhenAll(prodTask, consumeTask);


        }

        public static void TestOutChannelDropWrite()
        {
            Console.WriteLine("Bounded Channel Drop Write, Size 1");
            var channel = System.Threading.Channels.Channel.CreateBounded<long>(new BoundedChannelOptions(1)
            {
                SingleReader = true,
                SingleWriter = true,
                FullMode = BoundedChannelFullMode.DropWrite //DropNewest, DropOldest behave the same
            });

            var first = channel.Writer.TryWrite(1); //will return true
            var second = channel.Writer.TryWrite(2); //will return true

            Console.WriteLine($"First: {first}");
            Console.WriteLine($"Second: {second}");
        }

        public static void TestOutChannelWait()
        {
            Console.WriteLine("Bounded Channel Wait, Size 1");
            var channel = System.Threading.Channels.Channel.CreateBounded<long>(new BoundedChannelOptions(1)
            {
                SingleReader = true,
                SingleWriter = true,
                FullMode = BoundedChannelFullMode.Wait
            });

            var first = channel.Writer.TryWrite(1); //will return true
            var second = channel.Writer.TryWrite(2); //will return false

            Console.WriteLine($"First: {first}");
            Console.WriteLine($"Second: {second}");

        }
    }

    public class Producer
    {
        private readonly ChannelWriter<long> _writer;
        private readonly int _delay;

        public Producer(ChannelWriter<long> writer, int delay)
        {
            _writer = writer;
            _delay = delay;
        }

        public async Task Start(CancellationToken ct)
        {
            Console.WriteLine($"Starting the producer");
            long i = 0;
            while (await _writer.WaitToWriteAsync(ct))
            {
                await Task.Delay(TimeSpan.FromSeconds(_delay));
                if (_writer.TryWrite(i))
                    i++;
            }
        }
    }

    public class Consumer
    {
        private readonly ChannelReader<long> _reader;
        private readonly int _delay;

        public Consumer(ChannelReader<long> reader, int delay)
        {
            _reader = reader;
            _delay = delay;
        }

        public async Task Consume(CancellationToken ct)
        {
            while(await _reader.WaitToReadAsync(ct))
            {
                await Task.Delay(TimeSpan.FromSeconds(_delay), ct);
                if(_reader.TryRead(out var item))
                {
                    Console.WriteLine($"Reading: {item}");
                }

                //var result = await _reader.ReadAsync(ct);
            }
        }
    }
}
