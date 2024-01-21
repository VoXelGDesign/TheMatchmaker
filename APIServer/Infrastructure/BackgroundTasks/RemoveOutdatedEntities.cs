using Contracts.Common;
using Contracts.LobbyContracts;
using Contracts.QueueContracts;
using Domain.Users.UserQueueInfos;
using Infrastructure.Publishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundTasks;

public class RemoveOutdatedEntities : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public RemoveOutdatedEntities(IServiceProvider serviceProvider)
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

            var expired2vs2Lobbies = await dbScopedContext.RocketLeague2vs2Lobbies
                .Where(x => x.CreationDate.AddMinutes(LobbyLifetime.LifetimeMinutes) < DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            var expired3vs3Lobbies = await dbScopedContext.RocketLeague3vs3Lobbies
                .Where(x => x.CreationDate.AddMinutes(LobbyLifetime.LifetimeMinutes) < DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            var expiredInQueueStatuses = await dbScopedContext.UserQueueInfos
                .Where(x => x.LastChangeDate
                .AddMinutes(RequestLifetime.LifetimeMinutes ) < DateTime.UtcNow 
                && x.Status == UserQueueStatus.InQueue)
                .ToListAsync(stoppingToken);

            var expiredInLobbyStatuses = await dbScopedContext.UserQueueInfos
                 .Where(x => x.LastChangeDate
                 .AddMinutes(LobbyLifetime.LifetimeMinutes) < DateTime.UtcNow
                 && x.Status == UserQueueStatus.InLobby)
                 .ToListAsync(stoppingToken);

            if (expiredInQueueStatuses.Any())
            {
                expiredInQueueStatuses.ForEach(x => x.SetStatusNotInQueue(timestamp));

                await dbScopedContext.SaveChangesAsync();

                foreach(var status in expiredInQueueStatuses)
                {
                    var userIdDto = new UserIdDto(status.UserId.Id.ToString());
                   await queueStatusChangeScopedPublisher.PublishAsync(new(userIdDto, QueueStatus.LeftQueue));
                }

            }

            if (expiredInLobbyStatuses.Any())
            {
                expiredInLobbyStatuses.ForEach(x => x.SetStatusNotInQueue(timestamp));

                await dbScopedContext.SaveChangesAsync();

                foreach (var status in expiredInLobbyStatuses)
                {
                    var userIdDto = new UserIdDto(status.UserId.Id.ToString());
                    await queueStatusChangeScopedPublisher.PublishAsync(new(userIdDto, QueueStatus.LeftQueue));
                }

            }

            if (expired2vs2Lobbies.Any()) 
            {
                dbScopedContext.RocketLeague2vs2Lobbies
                    .RemoveRange(expired2vs2Lobbies);

                await dbScopedContext.SaveChangesAsync();
            }

            if (expired3vs3Lobbies.Any())
            {
                dbScopedContext.RocketLeague3vs3Lobbies
                    .RemoveRange(expired3vs3Lobbies);

                await dbScopedContext.SaveChangesAsync();
            }

        }
    }
}