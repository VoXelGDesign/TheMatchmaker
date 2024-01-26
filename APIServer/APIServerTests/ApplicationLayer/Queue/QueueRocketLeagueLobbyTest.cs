using Application.Exceptions.CustomExceptions;
using Application.Queue;
using Contracts.QueueContracts;
using Contracts.QueueContracts.RocketLeague;
using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;
using Domain.Users.UserAccounts;
using Domain.Users.UserGamesRanks;
using Domain.Users.UserQueueInfos;
using Infrastructure.Publishers;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Queue;

public class QueueRocketLeagueLobbyTest
{
    private TestUtils _utils = new TestUtils();


    [Fact]
    public async Task QueueRocketLeagueLobby_ValidRequest_SuccessfullyPublishesQueueRequest()
    {
        var publisherMock = new Mock<IQueueRocketLeagueLobbyRequestPublisher>();
        publisherMock.Setup(x => x.PublishAsync(It.IsAny<QueueRocketLeagueLobbyRequestDto>()));

        var userId = _utils.UserIdFromClaimsPrincipal();
        var name = UserAccountName.Create("DUMMY");
        var link = UserAccountSteamProfileLink.Create("https://steamcommunity.com/id/dummy/");
        var discordName = UserDiscordName.Create("dummy");
        var epicName = UserEpicName.Create("dummy");

        if (name is null || link is null || discordName is null || epicName is null)
            Assert.Fail("Failed to create resources for user account");

        var userAccount = UserAccount.Create(userId, name, link, discordName, epicName);

        
        var userQueueInfo = new UserQueueInfo(userId);
        userQueueInfo.SetStatusNotInQueue(DateTime.UtcNow);
        var rocketLeagueRank = RocketLeagueRank.Create("BRONZE", "I", "III");
        var userRank = UserGameRank.Create(userId, rocketLeague2vs2Rank: rocketLeagueRank);

        

        if (userAccount is null)
            Assert.Fail("Failed to create user account");

        var command = new QueueRequestCommand(
            "TwoVSTwo",
            new RocketLeagueRankDto("BRONZE", "I", "I"),
            new RocketLeagueRankDto("BRONZE", "III", "III"),
            QueueRegion.ENG,
            RocketLeaguePlatform.STEAM);


        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserAccounts.Add(userAccount);
            contextMock.UserGameRanks.Add(userRank);
            contextMock.UserQueueInfos.Add(userQueueInfo);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, _utils.ClaimsPrincipal, contextMock);
            await queueRocketLeagueLobby.Handle(command, CancellationToken.None);
        }

        publisherMock.Verify(x => x.PublishAsync(It.IsAny<QueueRocketLeagueLobbyRequestDto>()));
    }

    [Fact]
    public async Task QueueRocketLeagueLobby_InvalidRequest_ThrowsAppropriateException()
    {
        var publisherMock = new Mock<IQueueRocketLeagueLobbyRequestPublisher>();
        publisherMock.Setup(x => x.PublishAsync(It.IsAny<QueueRocketLeagueLobbyRequestDto>()));
        var userId = _utils.UserIdFromClaimsPrincipal();
        var name = UserAccountName.Create("DUMMY");
        var link = UserAccountSteamProfileLink.Create("https://steamcommunity.com/id/dummy/");
        var discordName = UserDiscordName.Create("dummy");
        var epicName = UserEpicName.Create("EMPTY");

        if (name is null || link is null || discordName is null || epicName is null)
            Assert.Fail("Failed to create resources for user account");

        var userAccount = UserAccount.Create(userId, name, link, discordName, epicName);



        if (userAccount is null)
            Assert.Fail("Failed to create user account");

        var userQueueInfo = new UserQueueInfo(userId);
        userQueueInfo
            .SetStatusNotInQueue(DateTime.UtcNow);

        var rocketLeagueRank = RocketLeagueRank
            .Create("BRONZE", "I", "III");
        var userRank = UserGameRank
            .Create(userId, rocketLeague2vs2Rank: rocketLeagueRank);

        var invalidModeRequest = new QueueRequestCommand(
                "invalid",
                new RocketLeagueRankDto("BRONZE", "I", "I"),
                new RocketLeagueRankDto("BRONZE", "III", "III"),
                QueueRegion.ENG,
                RocketLeaguePlatform.STEAM);
      

        var differentPlatform = new QueueRequestCommand(
                "TwoVSTwo",
                new RocketLeagueRankDto("BRONZE", "I", "I"),
                new RocketLeagueRankDto("BRONZE", "III", "III"),
                QueueRegion.ENG,
                RocketLeaguePlatform.EPIC);

        var command = new QueueRequestCommand(
                "TwoVSTwo",
                new RocketLeagueRankDto("BRONZE", "I", "I"),
                new RocketLeagueRankDto("BRONZE", "III", "III"),
                QueueRegion.ENG,
                RocketLeaguePlatform.STEAM);


        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserAccounts.Add(userAccount);
            contextMock.UserGameRanks.Add(userRank);
            contextMock.UserQueueInfos.Add(userQueueInfo);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, new ClaimsPrincipal(), contextMock);
            await Assert.ThrowsAsync<IdClaimNotFoundException>(async () => await queueRocketLeagueLobby.Handle(command, CancellationToken.None));
        }

        using (var contextMock = _utils.dbContext())
        {
            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, _utils.ClaimsPrincipal, contextMock);

            await Assert.ThrowsAsync<ResourceMissingException>(async () => await queueRocketLeagueLobby.Handle(differentPlatform, CancellationToken.None));
        }


        using (var contextMock = _utils.dbContext())
        {
            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, _utils.ClaimsPrincipal, contextMock);

            await Assert.ThrowsAsync<ArgumentException>(async () => await queueRocketLeagueLobby.Handle(invalidModeRequest, CancellationToken.None));
        }


        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserGameRanks.Remove(userRank);
            await contextMock.SaveChangesAsync();

            var queueRocketLeagueLobby = new QueueRocketLeagueLobby(publisherMock.Object, _utils.ClaimsPrincipal, contextMock);
            await Assert.ThrowsAsync<ResourceMissingException>(async () => await queueRocketLeagueLobby.Handle(command, CancellationToken.None));
        }

    }
}
