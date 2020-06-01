using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HowTo.WorkerService
{

    public class WorkerWithImmediateException : CopyOfBackgroundService
    {
        private readonly ILogger<WorkerWithImmediateException> _logger;

        public WorkerWithImmediateException(ILogger<WorkerWithImmediateException> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This exception will be caught as the exception will be returned to the calling task
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                throw new Exception("This will be caught");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
