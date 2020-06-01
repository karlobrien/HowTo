using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HowTo.WorkerService
{
    public class WorkerWithException : CopyOfBackgroundService
    {
        private readonly ILogger<WorkerWithException> _logger;

        public WorkerWithException(ILogger<WorkerWithException> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This fails silently because the Task is not captured by the caller
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                throw new Exception("This exception will fail silently");
            }
        }
    }
}
