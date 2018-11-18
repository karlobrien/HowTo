using System.Reactive;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace HowTo.Common
{
    public class SampleSubject
    {
        public Subject<int> Source { get; }

        public SampleSubject()
        {
            Source = new Subject<int>();
            var dis = Source.Select(obs => {
                var t = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"Select Thread: {t}");
                return obs * 2;
            }).Subscribe(sub =>
            {
                Console.WriteLine(sub);
                var t = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"Subscribe Thread: {t}");
            });
        }
    }
}