using Contracts.QueueContracts.RocketLeague;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueueService.Consumers;

namespace QueueSerivceTests.MassTransitIntegration;

public class RocketLeagueQueueRequestTest
{
    [Fact]
    public async Task Consmuer_ShouldRecieveMessage_Always()
    {
        var builder = new ConfigurationBuilder()
                .AddUserSecrets<RocketLeagueQueueRequestTest>();

        var configuration = builder.Build();

        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<RocketLeagueQueueRequestConsumer>();
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

        await harness.Bus.Publish(new QueueRocketLeagueLobbyRequestDto());

        var consumerHarness = harness.GetConsumerHarness<RocketLeagueQueueRequestConsumer>();

        Assert.True(await consumerHarness.Consumed.Any<QueueRocketLeagueLobbyRequestDto>());
    }
}
