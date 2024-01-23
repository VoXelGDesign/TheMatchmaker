using Application.Exceptions.CustomExceptions;
using Application.Queue;
using Contracts.QueueContracts;
using Contracts.QueueContracts.RocketLeague;
using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;
using Domain.Users.UserGamesRanks;
using Domain.Users.UserQueueInfos;
using Infrastructure.Publishers;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Queue;

public class QueueRocketLeagueLobbyTest
{

    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) }));

    public static DbContextOptions<ApplicationDbContext> Options()
        => new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

    [Fact]
    public async Task QueueRocketLeagueLobby_ValidRequest_SuccessfullyPublishesQueueRequest()
    {
        var publisherMock = new Mock<IQueueRocketLeagueLobbyRequestPublisher>();
        publisherMock.Setup(x => x.PublishAsync(It.IsAny<QueueRocketLeagueLobbyRequestDto>()));
        var userId = new UserId(Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)));
        var userQueueInfo = new UserQueueInfo(userId);
        userQueueInfo.SetStatusNotInQueue(DateTime.UtcNow);
        var rocketLeagueRank = RocketLeagueRank.Create("BRONZE", "I", "III");
        var userRank = UserGameRank.Create(userId, rocketLeague2vs2Rank: rocketLeagueRank);


        var command = new QueueRequestCommand(
            "TwoVSTwo",
            new RocketLeagueRankDto("BRONZE", "I", "I"),
            new RocketLeagueRankDto("BRONZE", "III", "III"),
            QueueRegion.ENG);


        using (var contextMock = new ApplicationDbContext(Options()))
        {
            contextMock.UserGameRanks.Add(userRank);
            contextMock.UserQueueInfos.Add(userQueueInfo);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, claimsPrincipal, contextMock);
            await queueRocketLeagueLobby.Handle(command, CancellationToken.None);
        }

        publisherMock.Verify(x => x.PublishAsync(It.IsAny<QueueRocketLeagueLobbyRequestDto>()));
    }

    [Fact]
    public async Task QueueRocketLeagueLobby_InvalidRequest_ThrowsAppropriateException()
    {
        var publisherMock = new Mock<IQueueRocketLeagueLobbyRequestPublisher>();
        publisherMock.Setup(x => x.PublishAsync(It.IsAny<QueueRocketLeagueLobbyRequestDto>()));
        var userId = new UserId(Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)));
        var userQueueInfo = new UserQueueInfo(userId);
        userQueueInfo.SetStatusNotInQueue(DateTime.UtcNow);
        var rocketLeagueRank = RocketLeagueRank.Create("BRONZE", "I", "III");
        var userRank = UserGameRank.Create(userId, rocketLeague2vs2Rank: rocketLeagueRank);

        var invalidModeRequest = new QueueRequestCommand(
                "invalid",
                new RocketLeagueRankDto("BRONZE", "I", "I"),
                new RocketLeagueRankDto("BRONZE", "III", "III"),
                QueueRegion.ENG);

        var invalidQueueRegion = new QueueRequestCommand(
                "invalid",
                new RocketLeagueRankDto("BRONZE", "I", "I"),
                new RocketLeagueRankDto("BRONZE", "III", "III"),
                (QueueRegion)10);

        var command = new QueueRequestCommand(
            "TwoVSTwo",
            new RocketLeagueRankDto("BRONZE", "I", "I"),
            new RocketLeagueRankDto("BRONZE", "III", "III"),
            QueueRegion.ENG);


        using (var contextMock = new ApplicationDbContext(Options()))
        {
            contextMock.UserGameRanks.Add(userRank);
            contextMock.UserQueueInfos.Add(userQueueInfo);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, new ClaimsPrincipal(), contextMock);
            await Assert.ThrowsAsync<IdClaimNotFoundException>(async () => await queueRocketLeagueLobby.Handle(command, CancellationToken.None));
        }

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, claimsPrincipal, contextMock);

            await Assert.ThrowsAsync<ArgumentException>(async () => await queueRocketLeagueLobby.Handle(invalidModeRequest, CancellationToken.None));
        }

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, claimsPrincipal, contextMock);

            await Assert.ThrowsAsync<ArgumentException>(async () => await queueRocketLeagueLobby.Handle(invalidQueueRegion, CancellationToken.None));
        }

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            contextMock.UserGameRanks.Remove(userRank);
            await contextMock.SaveChangesAsync();

            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, claimsPrincipal, contextMock);
            await Assert.ThrowsAsync<ResourceMissingException>(async () => await queueRocketLeagueLobby.Handle(command, CancellationToken.None));
        }

    }
}
