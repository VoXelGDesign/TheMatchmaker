using Contracts.Common;
using Contracts.QueueContracts;
using Contracts.QueueContracts.RocketLeague;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueueService.Consumers;

namespace QueueSerivceTests.MassTransitIntegration;

public class DeleteRocketLeagueQueueRequestTest
{
    public class UserRemovedFromQueueTest
    {
        [Fact]
        public async Task Consmuer_ShouldRecieveMessage_Always()
        {
            var builder = new ConfigurationBuilder()
                    .AddUserSecrets<UserRemovedFromQueueTest>();

            var configuration = builder.Build();

            await using var provider = new ServiceCollection()                
                .AddMassTransitTestHarness(x =>
                {
                    x.AddConsumer<DeleteRocketLeagueQueueRequestConsumer>();
                    x.SetKebabCaseEndpointNameFormatter();
                    x.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(configuration["AzureServiceBusConnectionString"]);
                        cfg.ConfigureEndpoints(context);
                    });
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            await harness.Bus.Publish(new DeleteRocketLeagueQueueRequestRequest(new UserIdDto("1111")));

            var consumerHarness = harness.GetConsumerHarness<DeleteRocketLeagueQueueRequestConsumer>();

            Assert.True(await consumerHarness.Consumed.Any<DeleteRocketLeagueQueueRequestRequest>());
        }
    }
}
