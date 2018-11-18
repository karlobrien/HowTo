using System;
using System.Reactive.Linq;
using System.Threading;
using System.Reactive.Concurrency;

namespace HowTo.Common
{
    public class PollingConsumer
    {
        private long _counter = 0;
        public IObservable<long> StartConsuming(CancellationToken ct, TimeSpan gap)
        {
            var scheduler = new NewThreadScheduler(ts => new Thread(ts) {Name = "Consumer"});

            var obs = Observable.Generate(Read(),
                x => !ct.IsCancellationRequested,
                x => Read(),
                x => x,
                x => gap,
                scheduler);

            return obs;
        }

        private long Read()
        {
            _counter++;
            return _counter + 10;
        }
    }
}