using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HowTo.Common
{
    /// <summary>
    /// An example of how to use defer async
    /// </summary>
    public class DeferByAsync
    {
        public IObservable<long> Obs {get;}

        public DeferByAsync()
        {
            Obs = Observable.Interval(TimeSpan.FromSeconds(3))
                .Select(item => Observable.DeferAsync(async token => {
                    return await ExpensiveTask(token, item);
                }))
                .Switch();
        }

        private async Task<IObservable<long>> ExpensiveTask(CancellationToken token, long item)
        {
            var t = await Task.Run(() => {
                        Task.Delay(3000, token);
                        return item;
                    });
            return Observable.Return(t);
        }
    }
}