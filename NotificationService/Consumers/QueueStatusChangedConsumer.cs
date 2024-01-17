using Contracts.QueueContracts;
using MassTransit;
using NotyficationService;

namespace NotificationService.Consumers;

public class QueueStatusChangedConsumer : IConsumer<QueueStatusChanged>
{
    private readonly NotificationHub _hub;
    public QueueStatusChangedConsumer(NotificationHub hub)
    {       
        _hub = hub;
    }
    public Task Consume(ConsumeContext<QueueStatusChanged> context)
    {
        var userId = context.Message.UserIdDto;

        if (userId is not null)
        {
            switch (context.Message.QueueStatus)
            {
                case QueueStatus.JoinedQueue:
                _hub.NotifyUserJoinedQueue(userId);
                    break;

                case QueueStatus.LeftQueue:
                _hub.NotifyUserLeftQueue(userId);
                    break;

                case QueueStatus.JoinedLobby:
                _hub.NotifyUserJoinedLobby(userId);
                    break;

                default: 
                    return Task.CompletedTask;
            }
        }
            

        return Task.CompletedTask;
    }
}
