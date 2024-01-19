using Contracts.LobbyContracts;
using Contracts.QueueContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundTasks;

public class ResetOutdatedQueueStatuses : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public ResetOutdatedQueueStatuses(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(500, stoppingToken);

            var timestamp = DateTime.UtcNow;

            using var scope = _serviceProvider.CreateScope();

            var dbScopedContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var expiredStatuses = await dbScopedContext.UserQueueInfos
                .Where(x => x.LastChangeDate.AddMinutes(RequestLifetime.LifetimeMinutes) < DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            if (expiredStatuses.Any())
            {
                expiredStatuses.ForEach(x => x.SetStatusNotInQueue(timestamp));

                await dbScopedContext.SaveChangesAsync();
            }


        }
    }
}
