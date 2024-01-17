using Contracts.QueueContracts.RocketLeague;
using MassTransit;

namespace QueueService.Publishers.CreateRocketLeagueLobby
{
    public class CreateRocketLeagueLobbyPublisher : ICreateRocketLeagueLobbyPublisher
    {
        private readonly IBus _publishEndpoint;

        public CreateRocketLeagueLobbyPublisher(IBus publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task PublishAsync(CreateRocketLeagueLobbyRequest request)
        {
            await _publishEndpoint.Publish(request);
        }
    }
}
