using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HowTo.Common
{
    public class DeferByAsync
    {
        public IObservable<long> Me {get;}

        public DeferByAsync()
        {
            var d = Observable.Interval(TimeSpan.FromSeconds(3))
                .Select(item => Observable.DeferAsync(async token => {
                    return await StarterAsync(token, item);
                }));
        }

        private async Task<IObservable<long>> StarterAsync(CancellationToken token, long item)
        {
            var t = await Task.Run(() => {
                        Task.Delay(3000, token);
                        return item;
                    });
            return Observable.Return(t);
        }
    }
}