using LearnFrameworkApi.Module.Datas;
using nc_backend.module.BusinessLogic;

namespace LearnFrameworkApi.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public Worker(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                int interval = _configuration.GetValue<int>("Interval");
                // Membuat scope manual
                using (var scope = _serviceProvider.CreateScope())
                {
                    var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    await BackgroundJobService.ExecuteQueue(appDbContext, interval, stoppingToken);
                }
            }
        }
    }
}
