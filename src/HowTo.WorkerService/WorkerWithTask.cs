using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HowTo.WorkerService
{

    public class WorkerWithTask : CopyOfBackgroundService
    {
        private readonly ILogger<WorkerWithTask> _logger;

        public WorkerWithTask(ILogger<WorkerWithTask> logger)
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
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
            }
        });
    }
}
