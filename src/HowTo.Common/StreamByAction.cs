using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace HowTo.Common
{
     /// <summary>
    /// Build a stream of T and apply a Func to each item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StreamByAction<T>
    {
        private Func<T, long,T> _generator;
        private IObservable<T> _myStream;
        public StreamByAction(IScheduler scheduler, int ms, Func<T,long,T> generator, T seed)
        {
            _generator = generator;
            _myStream = Observable.Interval(TimeSpan.FromMilliseconds(ms), scheduler)
                            .Scan(seed, _generator);
        }
        public IObservable<T> GetStream()
        {
            return _myStream;
        }
    }
}