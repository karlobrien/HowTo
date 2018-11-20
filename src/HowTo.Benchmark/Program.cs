using System;
using HowTo.Common;
using HowTo.GarbageCollection;
using System.Threading;

namespace HowTo.Benchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            // SampleSubject ss = new SampleSubject();
            // ss.Source.OnNext(1);
            // ss.Source.OnNext(1);
            // ss.Source.OnNext(1);
            // ss.Source.OnNext(1);

            // var item = Console.ReadLine();
            // if (item == "q")
            //     {
            //         ss.Source.Dispose();
            //     }

            PollingConsumer pc = new PollingConsumer();
            CancellationToken ct = new CancellationToken();
            var obs = pc.StartConsuming(ct, TimeSpan.FromSeconds(1));
            var dis = obs.Subscribe(sub => {
                Console.WriteLine(sub);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(Thread.CurrentThread.Name); //Consumer
                Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread); //False
            }, () => {
                Console.WriteLine("Complete");
            });

            var stop = Console.ReadLine();
            if (stop == "q")
                {
                    dis.Dispose();
                    Console.WriteLine(ct.IsCancellationRequested); //false as token is not cancelled on sidpose
                }



            // SampleForEvent se = new SampleForEvent();
            // se.OnEvent += new MyEventHandler(MaxReached);

            // se.AddToNumber(2);
            // se.AddToNumber(9);
            // se.AddToNumber(4);

            // int[] items = {1,2,3,4,5};
            // UnderstandingValueType uvt = new UnderstandingValueType();
            // Console.WriteLine(items[0]);
        }

        private static void MaxReached(object obj, MyEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }


}
