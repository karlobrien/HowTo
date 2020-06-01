using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HowTo.WorkerService
{

    /// <summary>
    /// 
    /// https://blog.stephencleary.com/2020/05/backgroundservice-gotcha-silent-failure.html
    /// </summary>
    public class WorkerWithTaskAndException : CopyOfBackgroundService
    {
        private readonly ILogger<WorkerWithTaskAndException> _logger;

        public WorkerWithTaskAndException(ILogger<WorkerWithTaskAndException> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    await Task.Delay(1000, stoppingToken);

                    throw new Exception("This will be caught");
                }
            }
            catch (Exception ex) when (False(() => _logger.LogCritical(ex, "Fatal error")))
            {
                _logger.LogError($"{ex.Message}");
            }
        });

        private bool False(Action action)
        {
            action();
            return false;
        }
    }
}
