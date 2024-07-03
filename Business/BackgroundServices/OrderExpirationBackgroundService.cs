using DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BackgroundServices
{
    public class OrderExpirationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;
        private TimeSpan _checkInterval = TimeSpan.FromMinutes(20);

        public OrderExpirationBackgroundService(IServiceProvider provider, ILogger logger)
        {
            _provider = provider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _provider.CreateScope())
                    {
                        var dbContext = _provider.GetRequiredService<MilkStoreContext>();
                        var now = DateTime.Now;
                        var expiredOrders = dbContext.Orders.Where(o => o.ExpireDate <= now && o.Status == 0)
                            .ToList();
                        foreach (var expiredOrder in expiredOrders)
                        {
                            expiredOrder.Status = 2;
                        }
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while checking for expired orders.");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
