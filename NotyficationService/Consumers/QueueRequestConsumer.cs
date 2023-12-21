using Infrastructure.Publishers.Contracts;
using MassTransit;

namespace NotyficationService.Consumers
{
    public class QueueRequestConsumer : IConsumer<QueueRequest>
    {
        private readonly NotificationHub _hub;

        public QueueRequestConsumer(NotificationHub hub)
        {
            _hub = hub;
        }

        public Task Consume(ConsumeContext<QueueRequest> context)
        {
            var userId = context.Message.UserId;

            if (userId is not null) 
            _hub.NotifyUser(new UserIdDto(userId));

            return Task.CompletedTask;
        }
    }
}
