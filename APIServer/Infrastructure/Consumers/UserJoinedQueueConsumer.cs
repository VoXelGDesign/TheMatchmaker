using Contracts.QueueContracts;
using Domain.Users.User;
using Domain.Users.UserQueueInfos;
using Infrastructure.Publishers;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Consumers
{
    public class UserJoinedQueueConsumer : IConsumer<UserJoinedQueue>
    {
        private readonly ApplicationDbContext _dbContext; 
        private readonly IQueueStatusChangedPublisher _queueStatusChangedPublisher;
        public UserJoinedQueueConsumer(ApplicationDbContext dbContext,IQueueStatusChangedPublisher queueStatusChangedPublisher)
        {

            _dbContext = dbContext;
            _queueStatusChangedPublisher  = queueStatusChangedPublisher;
        }
        public async Task Consume(ConsumeContext<UserJoinedQueue> context)
        {
            var guid = Guid.Parse(context.Message.UserIdDto.UserId);
            var userId = new UserId(guid);

            var userQueueInfo = await _dbContext.UserQueueInfos.FirstOrDefaultAsync(x => x.UserId == userId);

            if(userQueueInfo is null)
            {
                userQueueInfo = new UserQueueInfo(userId);
                _dbContext.UserQueueInfos.Add(userQueueInfo);               
            }

            userQueueInfo.SetStatusInQueue(context.Message.TimeStamp);

           await _dbContext.SaveChangesAsync();

           await _queueStatusChangedPublisher
                .PublishAsync(new QueueStatusChanged(context.Message.UserIdDto, QueueStatus.JoinedQueue));
        }
    }
}
