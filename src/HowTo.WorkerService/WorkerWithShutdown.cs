using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HowTo.WorkerService
{

    public class WorkerWithShutdown : CopyOfBackgroundService
    {
        private readonly ILogger<WorkerWithShutdown> _logger;
        private readonly IHostApplicationLifetime _lifeTime;

        public WorkerWithShutdown(ILogger<WorkerWithShutdown> logger, IHostApplicationLifetime lifeTime)
        {
            _logger = logger;
            _lifeTime = lifeTime;
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
                _lifeTime.StopApplication();
            }
        });
    }
}
