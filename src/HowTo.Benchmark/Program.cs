using System;
using HowTo.Common;
using HowTo.GarbageCollection;

namespace HowTo.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SampleForEvent se = new SampleForEvent();
            se.OnEvent += new MyEventHandler(MaxReached);

            se.AddToNumber(2);
            se.AddToNumber(9);
            se.AddToNumber(4);

            int[] items = {1,2,3,4,5};
            UnderstandingValueType uvt = new UnderstandingValueType();
            uvt.RefLocalsIn(in items);
            Console.WriteLine(items[0]);
        }

        private static void MaxReached(object obj, MyEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
