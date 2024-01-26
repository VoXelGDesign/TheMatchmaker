using Contracts.Common;
using Contracts.QueueContracts;
using Contracts.QueueContracts.RocketLeague;
using Contracts.QueueContracts.RocketLeague.Ranks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using QueueService;
using QueueService.Publishers.CreateRocketLeagueLobby;
using QueueService.Publishers.JoinedQueue;
using QueueService.Publishers.RemovedFromQueue;

namespace QueueSerivceTests.Queue;

public class RocketLeagueQueueTest
{
    [Fact]
    public async Task Queue_ShouldHaveOnlyOneRequest_WhenItIsFromOneUser()
    {
        var rank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("BRONZE", "I", "I"));

        if (rank is null)
            Assert.Fail("Failed to create rank");

        var request = new QueueRocketLeagueLobbyRequest
        {
            UserId = new UserIdDto("123123"),
            Mode = RocketLeagueQueueMode.TwoVSTwo,
            Platform = RocketLeaguePlatform.EPIC,
            Region = QueueRegion.ENG,
            LowerBoundRank = rank,
            UpperBoundRank = rank,
            DateTime = DateTime.Now,
            UserRank = rank
        };

        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IHostedService, RocketLeagueQueue>();

        services.AddSingleton(Mock.Of<ICreateRocketLeagueLobbyPublisher>(x =>
        x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()) == Task.CompletedTask));

        services.AddSingleton(Mock.Of<IJoinedQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserJoinedQueue>()) == Task.CompletedTask));

        services.AddSingleton(
            Mock.Of<IRemovedFromQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserRemovedFromQueue>()) == Task.CompletedTask));

        var serviceProvider = services.BuildServiceProvider();
        var hostedService = serviceProvider.GetServices<IHostedService>()
            .OfType<RocketLeagueQueue>()
            .SingleOrDefault();
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;

        new Thread(async () =>
        {
            Thread.CurrentThread.IsBackground = true;
            await Task.Delay(1000);
            hostedService.StopAsync(token);
        }).Start();

        await hostedService.StartAsync(token);

        await hostedService.AddToQueue(request);
        await hostedService.AddToQueue(request);

        var service = serviceProvider
        .GetRequiredService<IJoinedQueuePublisher>();

        var mock = Mock.Get(service);

        mock.Verify(x => x.PublishAsync(It.IsAny<UserJoinedQueue>()), Times.AtMostOnce);

    }

    [Fact]
    public async Task Queue_ShouldNotPublishRemoveRequest_WhenRequestWasntRemoved()
    {
        var rank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("BRONZE", "I", "I"));

        if (rank is null)
            Assert.Fail("Failed to create rank");

        var request = new QueueRocketLeagueLobbyRequest
        {
            UserId = new UserIdDto("123123"),
            Mode = RocketLeagueQueueMode.TwoVSTwo,
            Platform = RocketLeaguePlatform.EPIC,
            Region = QueueRegion.ENG,
            LowerBoundRank = rank,
            UpperBoundRank = rank,
            DateTime = DateTime.Now,
            UserRank = rank
        };

        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IHostedService, RocketLeagueQueue>();

        services.AddSingleton(Mock.Of<ICreateRocketLeagueLobbyPublisher>(x =>
        x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()) == Task.CompletedTask));

        services.AddSingleton(Mock.Of<IJoinedQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserJoinedQueue>()) == Task.CompletedTask));

        services.AddSingleton(
            Mock.Of<IRemovedFromQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserRemovedFromQueue>()) == Task.CompletedTask));

        var serviceProvider = services.BuildServiceProvider();
        var hostedService = serviceProvider.GetServices<IHostedService>()
            .OfType<RocketLeagueQueue>()
            .SingleOrDefault();
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;

        new Thread(async () =>
        {
            Thread.CurrentThread.IsBackground = true;
            await Task.Delay(1000);
            hostedService.StopAsync(token);
        }).Start();

        await hostedService.StartAsync(token);

        await hostedService.AddToQueue(request);
        await hostedService.RemoveFromQueue(new UserIdDto("123123"));
        await hostedService.RemoveFromQueue(new UserIdDto("123123"));


        var service = serviceProvider
        .GetRequiredService<IRemovedFromQueuePublisher>();

        var mock = Mock.Get(service);

        mock.Verify(x => x.PublishAsync(It.IsAny<UserRemovedFromQueue>()), Times.AtMostOnce);

    }

    [Fact]
    public async Task Queue_ShouldMatchAtLeast600RequestsUnder10Seconds_WhenAllPosibbleToMatch()
    {
        var rank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("GOLD", "I", "I"));
        var loweRank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("BRONZE", "I", "I"));
        var uppeRank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("DIAMOND", "I", "I"));
        if (rank is null || loweRank is null || uppeRank is null)
            Assert.Fail("Failed to create rank");

        var listOfRequests = new List<QueueRocketLeagueLobbyRequest>();

        for (int i = 0; i < 600; i++)
        {
            var request = new QueueRocketLeagueLobbyRequest
            {
                UserId = new UserIdDto("123123" + i),
                Mode = RocketLeagueQueueMode.TwoVSTwo,
                Platform = RocketLeaguePlatform.EPIC,
                Region = QueueRegion.ENG,
                UserRank = rank,
                LowerBoundRank = rank,
                UpperBoundRank = rank,
                DateTime = DateTime.Now
            };
            listOfRequests.Add(request);
        }


        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IHostedService, RocketLeagueQueue>();

        services.AddSingleton(Mock.Of<ICreateRocketLeagueLobbyPublisher>(x =>
        x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()) == Task.CompletedTask));

        services.AddSingleton(Mock.Of<IJoinedQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserJoinedQueue>()) == Task.CompletedTask));

        services.AddSingleton(
            Mock.Of<IRemovedFromQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserRemovedFromQueue>()) == Task.CompletedTask));

        var serviceProvider = services.BuildServiceProvider();
        var hostedService = serviceProvider.GetServices<IHostedService>()
            .OfType<RocketLeagueQueue>()
            .SingleOrDefault();
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;

        await hostedService.StartAsync(token);

        foreach (var request in listOfRequests)
        {
            await hostedService.AddToQueue(request);
        }

        await Task.Delay(10000);
        var service = serviceProvider
        .GetRequiredService<ICreateRocketLeagueLobbyPublisher>();

        var mock = Mock.Get(service);

        mock.Verify(x => x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()), Times.Exactly(300));

        await hostedService.StopAsync(token);
    }

    [Fact]
    public async Task Queue_ShoudlNotMatchRequests_WhenRequestOutsideBounds()
    {
        var rank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("BRONZE", "I", "I"));
        var loweRank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("GOLD", "I", "I"));
        var uppeRank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("DIAMOND", "I", "I"));
        if (rank is null || loweRank is null || uppeRank is null)
            Assert.Fail("Failed to create rank");


        var request1 = new QueueRocketLeagueLobbyRequest
        {
            UserId = new UserIdDto("123123"),
            Mode = RocketLeagueQueueMode.TwoVSTwo,
            Platform = RocketLeaguePlatform.EPIC,
            Region = QueueRegion.ENG,
            UserRank = rank,
            LowerBoundRank = loweRank,
            UpperBoundRank = uppeRank,
            DateTime = DateTime.Now
        };

        var request2 = new QueueRocketLeagueLobbyRequest
        {
            UserId = new UserIdDto("456456"),
            Mode = RocketLeagueQueueMode.TwoVSTwo,
            Platform = RocketLeaguePlatform.EPIC,
            Region = QueueRegion.ENG,
            UserRank = rank,
            LowerBoundRank = loweRank,
            UpperBoundRank = uppeRank,
            DateTime = DateTime.Now
        };


        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IHostedService, RocketLeagueQueue>();

        services.AddSingleton(Mock.Of<ICreateRocketLeagueLobbyPublisher>(x =>
        x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()) == Task.CompletedTask));

        services.AddSingleton(Mock.Of<IJoinedQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserJoinedQueue>()) == Task.CompletedTask));

        services.AddSingleton(
            Mock.Of<IRemovedFromQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserRemovedFromQueue>()) == Task.CompletedTask));

        var serviceProvider = services.BuildServiceProvider();
        var hostedService = serviceProvider.GetServices<IHostedService>()
            .OfType<RocketLeagueQueue>()
            .SingleOrDefault();
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;

        await hostedService.StartAsync(token);

        
        await hostedService.AddToQueue(request1);
        await hostedService.AddToQueue(request2);


        await Task.Delay(1000);
        var service = serviceProvider
        .GetRequiredService<ICreateRocketLeagueLobbyPublisher>();

        var mock = Mock.Get(service);

        mock.Verify(x => x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()), Times.Never);

        await hostedService.StopAsync(token);
    }

    [Fact]
    public async Task Queue_ShoudlMatchRequests_WhenRequestWithinBounds()
    {
        var rank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("GOLD", "I", "I"));
        var loweRank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("GOLD", "I", "I"));
        var uppeRank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("DIAMOND", "I", "I"));
        if (rank is null || loweRank is null || uppeRank is null)
            Assert.Fail("Failed to create rank");


        var request1 = new QueueRocketLeagueLobbyRequest
        {
            UserId = new UserIdDto("123123"),
            Mode = RocketLeagueQueueMode.TwoVSTwo,
            Platform = RocketLeaguePlatform.EPIC,
            Region = QueueRegion.ENG,
            UserRank = rank,
            LowerBoundRank = loweRank,
            UpperBoundRank = uppeRank,
            DateTime = DateTime.Now
        };

        var request2 = new QueueRocketLeagueLobbyRequest
        {
            UserId = new UserIdDto("456456"),
            Mode = RocketLeagueQueueMode.TwoVSTwo,
            Platform = RocketLeaguePlatform.EPIC,
            Region = QueueRegion.ENG,
            UserRank = rank,
            LowerBoundRank = loweRank,
            UpperBoundRank = uppeRank,
            DateTime = DateTime.Now
        };


        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IHostedService, RocketLeagueQueue>();

        services.AddSingleton(Mock.Of<ICreateRocketLeagueLobbyPublisher>(x =>
        x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()) == Task.CompletedTask));

        services.AddSingleton(Mock.Of<IJoinedQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserJoinedQueue>()) == Task.CompletedTask));

        services.AddSingleton(
            Mock.Of<IRemovedFromQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserRemovedFromQueue>()) == Task.CompletedTask));

        var serviceProvider = services.BuildServiceProvider();
        var hostedService = serviceProvider.GetServices<IHostedService>()
            .OfType<RocketLeagueQueue>()
            .SingleOrDefault();
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;

        await hostedService.StartAsync(token);


        await hostedService.AddToQueue(request1);
        await hostedService.AddToQueue(request2);


        await Task.Delay(1000);
        var service = serviceProvider
        .GetRequiredService<ICreateRocketLeagueLobbyPublisher>();

        var mock = Mock.Get(service);

        mock.Verify(x => x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()), Times.Once);

        await hostedService.StopAsync(token);
    }

    [Fact]
    public async Task Queue_ShoudlMatchRequests_WhenModeIs3vs3()
    {
        var rank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("GOLD", "I", "I"));
        var loweRank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("GOLD", "I", "I"));
        var uppeRank = QueueRocketLeagueRank.Create(new QueueRocketLeagueRankDto("DIAMOND", "I", "I"));
        if (rank is null || loweRank is null || uppeRank is null)
            Assert.Fail("Failed to create rank");


        var request1 = new QueueRocketLeagueLobbyRequest
        {
            UserId = new UserIdDto("123123"),
            Mode = RocketLeagueQueueMode.ThreeVSThree,
            Platform = RocketLeaguePlatform.EPIC,
            Region = QueueRegion.ENG,
            UserRank = rank,
            LowerBoundRank = loweRank,
            UpperBoundRank = uppeRank,
            DateTime = DateTime.Now
        };

        var request2 = new QueueRocketLeagueLobbyRequest
        {
            UserId = new UserIdDto("456456"),
            Mode = RocketLeagueQueueMode.ThreeVSThree,
            Platform = RocketLeaguePlatform.EPIC,
            Region = QueueRegion.ENG,
            UserRank = rank,
            LowerBoundRank = loweRank,
            UpperBoundRank = uppeRank,
            DateTime = DateTime.Now
        };

        var request3 = new QueueRocketLeagueLobbyRequest
        {
            UserId = new UserIdDto("789789"),
            Mode = RocketLeagueQueueMode.ThreeVSThree,
            Platform = RocketLeaguePlatform.EPIC,
            Region = QueueRegion.ENG,
            UserRank = rank,
            LowerBoundRank = loweRank,
            UpperBoundRank = uppeRank,
            DateTime = DateTime.Now
        };


        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IHostedService, RocketLeagueQueue>();

        services.AddSingleton(Mock.Of<ICreateRocketLeagueLobbyPublisher>(x =>
        x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()) == Task.CompletedTask));

        services.AddSingleton(Mock.Of<IJoinedQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserJoinedQueue>()) == Task.CompletedTask));

        services.AddSingleton(
            Mock.Of<IRemovedFromQueuePublisher>(x =>
        x.PublishAsync(It.IsAny<UserRemovedFromQueue>()) == Task.CompletedTask));

        var serviceProvider = services.BuildServiceProvider();
        var hostedService = serviceProvider.GetServices<IHostedService>()
            .OfType<RocketLeagueQueue>()
            .SingleOrDefault();
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;

        await hostedService.StartAsync(token);


        await hostedService.AddToQueue(request1);
        await hostedService.AddToQueue(request2);
        await hostedService.AddToQueue(request3);

        await Task.Delay(1000);
        var service = serviceProvider
        .GetRequiredService<ICreateRocketLeagueLobbyPublisher>();

        var mock = Mock.Get(service);

        mock.Verify(x => x.PublishAsync(It.IsAny<CreateRocketLeagueLobbyRequest>()), Times.Once);

        await hostedService.StopAsync(token);
    }


}
