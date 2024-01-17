using Contracts.QueueContracts;
using Contracts.QueueContracts.RocketLeague;
using Domain.Games.RocketLeague.Lobbies;
using Domain.Games.RocketLeague.Players;
using Domain.Users.User;
using Infrastructure.Publishers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Consumers
{
    internal class CreateRocketLeagueLobbyConsumer : IConsumer<CreateRocketLeagueLobbyRequest>
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


            if (context.Message.userIds.Count < 2)
            {
                var queueInfo = await _dbContext.UserQueueInfos
                    .FirstOrDefaultAsync(x => x.UserId.Id.ToString() == firstId);
                queueInfo?.SetStatusNotInQueue(timeStamp);
                return;
            }

            var secondId = context.Message.userIds[1].UserId;

            var firstUserAccount = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(x => x.Id.Id.ToString() == firstId);

            var secondUserAccount = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(x => x.Id.Id.ToString() == secondId);

            var firstQueueInfo = await _dbContext.UserQueueInfos
                .FirstOrDefaultAsync(x => x.UserId.Id.ToString() == firstId);

            var secondQueueInfo = await _dbContext.UserQueueInfos
                .FirstOrDefaultAsync(x => x.UserId.Id.ToString() == secondId);

            if (firstUserAccount is null || secondUserAccount is null)
            {
                firstQueueInfo?.SetStatusNotInQueue(timeStamp);
                secondQueueInfo?.SetStatusNotInQueue(timeStamp);
                return;
            }

            if (firstQueueInfo is null || secondQueueInfo is null)
                throw new Exception("INTERFACE_INCOSISTENCY_IN_DATABASE");

            var player1 = RocketLeaguePlayer.Create(firstUserAccount);
            var player2 = RocketLeaguePlayer.Create(secondUserAccount);

            switch (context.Message.userIds.Count)
            {
                case 2:

                    RocketLeague2vs2Lobby.Create(player1, player2);

                    firstQueueInfo.SetStatusInLobby(timeStamp);
                    secondQueueInfo.SetStatusInLobby(timeStamp);

                    await _queueStatusChangedPublisher
                        .PublishAsync(new QueueStatusChanged(context.Message.userIds[0], QueueStatus.JoinedLobby));

                    await _queueStatusChangedPublisher
                        .PublishAsync(new QueueStatusChanged(context.Message.userIds[1], QueueStatus.JoinedLobby));

                    break;
                case 3:

                    var thirdUserAccount = await _dbContext.UserAccounts
                        .FirstOrDefaultAsync(x => x.Id.Id.ToString() == context.Message.userIds[2].UserId);

                    var thirdQueueInfo = await _dbContext.UserQueueInfos
                        .FirstOrDefaultAsync(x => x.UserId.Id.ToString() == context.Message.userIds[2].UserId);

                    if (thirdQueueInfo is null)
                        throw new Exception("INTERFACE_INCOSISTENCY_IN_DATABASE");

                    if (thirdUserAccount is null)
                    {
                        firstQueueInfo?.SetStatusNotInQueue(timeStamp);
                        secondQueueInfo?.SetStatusNotInQueue(timeStamp);
                        thirdQueueInfo?.SetStatusNotInQueue(timeStamp);
                        return;
                    }

                    var player3 = RocketLeaguePlayer.Create(thirdUserAccount);

                    RocketLeague3vs3Lobby.Create(player1, player2, player3);

                    firstQueueInfo.SetStatusInLobby(timeStamp);
                    secondQueueInfo.SetStatusInLobby(timeStamp);
                    thirdQueueInfo?.SetStatusInLobby(timeStamp);

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
