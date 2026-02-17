using Elderly_System.BLL.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elderly_System.BLL.Service.Classes
{
    public class StayAutoFinishHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public StayAutoFinishHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                var nextRun = DateTime.Today.AddDays(1).AddMinutes(5);
                var delay = nextRun - now;
                if (delay < TimeSpan.Zero) delay = TimeSpan.FromMinutes(1);

                await Task.Delay(delay, stoppingToken);

                using var scope = _scopeFactory.CreateScope();
                var autoFinish = scope.ServiceProvider.GetRequiredService<IAutoFinishStaysService>();

                await autoFinish.RunAsync(DateTime.Today);
            }
        }
    }
}
