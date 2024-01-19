using Contracts.Common;
using Contracts.LobbyContracts;
using Contracts.QueueContracts;
using Infrastructure.Publishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundTasks;

public class RemoveOutdatedLobby : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public RemoveOutdatedLobby(IServiceProvider serviceProvider)
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
            var queueStatusChangeScopedPublisher = scope.ServiceProvider.GetRequiredService<IQueueStatusChangedPublisher>();

            var expiredLobbies = await dbScopedContext.RocketLeague2vs2Lobbies
                .Where(x => x.CreationDate.AddMinutes(LobbyLifetime.LifetimeMinutes) < DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            var expiredStatuses = await dbScopedContext.UserQueueInfos
                .Where(x => x.LastChangeDate.AddMinutes(RequestLifetime.LifetimeMinutes) < DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            if (expiredStatuses.Any())
            {
                expiredStatuses.ForEach(x => x.SetStatusNotInQueue(timestamp));

                await dbScopedContext.SaveChangesAsync();

                foreach(var status in expiredStatuses)
                {
                    var userIdDto = new UserIdDto(status.UserId.Id.ToString());
                   await queueStatusChangeScopedPublisher.PublishAsync(new(userIdDto, QueueStatus.LeftQueue));
                }

            }

            if (expiredLobbies.Any()) 
            {
                dbScopedContext.RocketLeague2vs2Lobbies
                    .RemoveRange(expiredLobbies);

                await dbScopedContext.SaveChangesAsync();
            }


            
        }
    }
}