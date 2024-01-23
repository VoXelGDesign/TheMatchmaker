using Contracts.QueueContracts;
using Contracts.QueueContracts.RocketLeague;
using Domain.Games.RocketLeague.Lobbies;
using Domain.Games.RocketLeague.Players;
using Domain.Users.User;
using Infrastructure.Exceptions.CustomExceptions;
using Infrastructure.Publishers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Consumers
{
    public class CreateRocketLeagueLobbyConsumer : IConsumer<CreateRocketLeagueLobbyRequest>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IQueueStatusChangedPublisher _queueStatusChangedPublisher;
        public CreateRocketLeagueLobbyConsumer(ApplicationDbContext dbContext, IQueueStatusChangedPublisher queueStatusChangedPublisher)
        {
            _dbContext = dbContext;
            _queueStatusChangedPublisher = queueStatusChangedPublisher;
        }
        public async Task Consume(ConsumeContext<CreateRocketLeagueLobbyRequest> context)
        {
            var timeStamp = context.Message.TimeStamp;

            var firstId = context.Message.userIds[0].UserId;
            var firstUserId = new UserId(Guid.Parse(firstId));

            if (context.Message.userIds.Count < 2)
            {
                var queueInfo = await _dbContext.UserQueueInfos
                    .FirstOrDefaultAsync(x => x.UserId == firstUserId);
                queueInfo?.SetStatusNotInQueue(timeStamp);
                return;
            }

            var secondId = context.Message.userIds[1].UserId;
            var secondUserId = new UserId(Guid.Parse(secondId));

            var firstUserAccount = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(x => x.Id == firstUserId);

            var secondUserAccount = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(x => x.Id == secondUserId);

            var firstQueueInfo = await _dbContext.UserQueueInfos
                .FirstOrDefaultAsync(x => x.UserId == firstUserId);

            var secondQueueInfo = await _dbContext.UserQueueInfos
                .FirstOrDefaultAsync(x => x.UserId == secondUserId);

            if (firstUserAccount is null || secondUserAccount is null)
            {
                firstQueueInfo?.SetStatusNotInQueue(timeStamp);
                secondQueueInfo?.SetStatusNotInQueue(timeStamp);
                return;
            }

            if (firstQueueInfo is null || secondQueueInfo is null)
                throw new InconsistencyInDatabaseException();

            var player1 = RocketLeaguePlayer.Create(firstUserAccount);
            var player2 = RocketLeaguePlayer.Create(secondUserAccount);

            switch (context.Message.userIds.Count)
            {
                case 2:

                    var lobby2vs2 = RocketLeague2vs2Lobby.Create(player1, player2);

                    _dbContext.RocketLeague2vs2Lobbies.Add(lobby2vs2);
                    

                    firstQueueInfo.SetStatusInLobby(timeStamp);
                    secondQueueInfo.SetStatusInLobby(timeStamp);

                    await _dbContext.SaveChangesAsync();
                    await _queueStatusChangedPublisher
                        .PublishAsync(new QueueStatusChanged(context.Message.userIds[0], QueueStatus.JoinedLobby));

                    await _queueStatusChangedPublisher
                        .PublishAsync(new QueueStatusChanged(context.Message.userIds[1], QueueStatus.JoinedLobby));

                    break;
                case 3:
                    var thirdUserId = new UserId(Guid.Parse(context.Message.userIds[2].UserId));
                    var thirdUserAccount = await _dbContext.UserAccounts
                        .FirstOrDefaultAsync(x => x.Id == thirdUserId);

                    var thirdQueueInfo = await _dbContext.UserQueueInfos
                        .FirstOrDefaultAsync(x => x.UserId == thirdUserId);

                    if (thirdQueueInfo is null)
                        throw new InconsistencyInDatabaseException();

                    if (thirdUserAccount is null)
                    {
                        firstQueueInfo?.SetStatusNotInQueue(timeStamp);
                        secondQueueInfo?.SetStatusNotInQueue(timeStamp);
                        thirdQueueInfo?.SetStatusNotInQueue(timeStamp);
                        return;
                    }

                    var player3 = RocketLeaguePlayer.Create(thirdUserAccount);

                    var lobby3vs3 = RocketLeague3vs3Lobby.Create(player1, player2, player3);


                    _dbContext.RocketLeague3vs3Lobbies.Add(lobby3vs3);

                    firstQueueInfo.SetStatusInLobby(timeStamp);
                    secondQueueInfo.SetStatusInLobby(timeStamp);
                    thirdQueueInfo?.SetStatusInLobby(timeStamp);

                    await _dbContext.SaveChangesAsync();

                    await _queueStatusChangedPublisher
                        .PublishAsync(new QueueStatusChanged(context.Message.userIds[0], QueueStatus.JoinedLobby));

                    await _queueStatusChangedPublisher
                        .PublishAsync(new QueueStatusChanged(context.Message.userIds[1], QueueStatus.JoinedLobby));

                    await _queueStatusChangedPublisher
                        .PublishAsync(new QueueStatusChanged(context.Message.userIds[2], QueueStatus.JoinedLobby));

                    break;
            }       
        }
    }
}
