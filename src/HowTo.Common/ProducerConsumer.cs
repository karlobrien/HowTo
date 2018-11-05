using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace HowTo.Common
{
    public class ProducerConsumer<T>
    {
        private BlockingCollection<T> _queue;
        private int _workers;

        public ProducerConsumer(int workers, Action<T> action, int capacity=-1)
        {
            _queue = new BlockingCollection<T>();
            _workers = workers;

            Complete = Setup(action);
        }

        public Task<bool> Complete { get; }

        private Task<bool> Setup(Action<T> action)
        {
            TaskScheduler ts = TaskScheduler.Default;
            var workerTasks = new Task[_workers];
            for(var i = 0; i<_workers; i++)
            {
                workerTasks[i] = Task.Factory.StartNew(() => {
                    foreach(var item in _queue.GetConsumingEnumerable())
                    {
                        try
                        {
                            action(item);
                        }
                        catch (Exception ex)
                        {
                            OnException?.Invoke(this, ex);
                        }
                    }
                }, CancellationToken.None, TaskCreationOptions.LongRunning, ts);
            }

            var tcs = new TaskCompletionSource<bool>();
            var completeMe = Task.WhenAll(workerTasks);

            completeMe.ContinueWith(t => {
                tcs.SetResult(false);
            }, TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);

            completeMe.ContinueWith(t => {
                tcs.SetResult(true);
            }, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public event EventHandler<Exception> OnException;

        public void Add(T item)
        {
            Add(item, CancellationToken.None);
        }

        public void Add(T item, CancellationToken token)
        {
            try
            {
                _queue.Add(item);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
            }
        }

        public void CompletedForAdd()
        {
            _queue?.CompleteAdding();
        }
    }
}
