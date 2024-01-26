using Contracts.Common;
using Contracts.QueueContracts;
using Infrastructure.Consumers;
using Infrastructure.Publishers;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIServerTests.MassTransitIntegraion;

public class UserRemovedFromQueueTest
{
    [Fact]
    public async Task Consmuer_ShouldRecieveMessage_Always()
    {

        var builder = new ConfigurationBuilder()
                .AddUserSecrets<UserRemovedFromQueueTest>();

        var configuration = builder.Build();

        await using var provider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: "TestDatabase"))
            .AddScoped<IQueueStatusChangedPublisher, QueueStatusChangedPublisher>()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<UserRemovedFromQueueConsumer>();
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

        await harness.Bus.Publish(new UserRemovedFromQueue(new UserIdDto("1111"), DateTime.UtcNow));

        var consumerHarness = harness.GetConsumerHarness<UserRemovedFromQueueConsumer>();

        Assert.True(await consumerHarness.Consumed.Any<UserRemovedFromQueue>());
    }
}
