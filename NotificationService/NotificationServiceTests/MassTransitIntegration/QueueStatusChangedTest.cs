using Contracts.Common;
using Contracts.QueueContracts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Consumers;
using NotyficationService;


namespace NotificationServiceTests.MassTransitIntegration;

public class QueueStatusChangedTest
{
    [Fact]
    public async Task Consmuer_ShouldRecieveMessage_Always()
    {
        var builder = new ConfigurationBuilder()
                .AddUserSecrets<QueueStatusChangedTest>();

        var configuration = builder.Build();

        await using var provider = new ServiceCollection()
            .AddSingleton<NotificationHub, NotificationHub>()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<QueueStatusChangedConsumer>();
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

        await harness.Bus.Publish(new QueueStatusChanged(new UserIdDto("1111"), QueueStatus.JoinedQueue));

        var consumerHarness = harness.GetConsumerHarness<QueueStatusChangedConsumer>();

        Assert.True(await consumerHarness.Consumed.Any<QueueStatusChanged>());
    }
}
