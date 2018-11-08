using System;
using HowTo.Common;

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
        }

        private static void MaxReached(object obj, MyEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
