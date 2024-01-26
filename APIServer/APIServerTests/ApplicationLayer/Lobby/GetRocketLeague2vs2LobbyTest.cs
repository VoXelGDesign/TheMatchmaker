using Application.Exceptions.CustomExceptions;
using Application.Lobby.RocketLeague2vs2Lobby.Queries;
using Application.Lobby.RocketLeague3vs3Lobby.Queries;
using Domain.Games.RocketLeague.Lobbies;
using Domain.Games.RocketLeague.Players;
using Domain.Users.User;
using Domain.Users.UserAccounts;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Lobby;

public class GetRocketLeague2vs2LobbyTest
{
    private TestUtils _utils = new TestUtils();

    [Fact]
    public async Task Handler_ShouldThrowIdClaimNotFoundException_WhenClaimIdentityIsNull()
    {
        var command = new GetRocketLeague2vs2LobbyQuery();

        using (var contextMock = _utils.dbContext())
        {
            var getRocketLeague2vs2Lobby =
                new GetRocketLeague2vs2Lobby(contextMock, new ClaimsPrincipal());

            await Assert.ThrowsAsync<IdClaimNotFoundException>(async ()
                => await getRocketLeague2vs2Lobby.Handle(command, CancellationToken.None));
        }
    }

    [Fact]
    public async Task Handler_ShouldGetRocketLeague2vs2LobbySuccessfully_WhenFound()
    {
        var name1 = UserAccountName.Create("DUMMY");
        var link1 = UserAccountSteamProfileLink.Create("https://steamcommunity.com/id/dummy/");
        var discordName1 = UserDiscordName.Create("dummy");
        var epicName1 = UserEpicName.Create("dummy");

        var name2 = UserAccountName.Create("TEST");
        var link2 = UserAccountSteamProfileLink.Create("https://steamcommunity.com/id/test/");
        var discordName2 = UserDiscordName.Create("test");
        var epicName2 = UserEpicName.Create("test");

        if (name1 is null || link1 is null || discordName1 is null)
            Assert.Fail("Failed to create resources for user account 1");

        if (name2 is null || link2 is null || discordName2 is null)
            Assert.Fail("Failed to create resources for user account 1");

        var userAccount1 = UserAccount.Create(_utils.UserIdFromClaimsPrincipal(), name1, link1, discordName1, epicName1);
        var userAccount2 = UserAccount.Create(new UserId(Guid.NewGuid()), name2, link2, discordName2, epicName2);

        if (userAccount1 is null)
            Assert.Fail("Failed to create user account 1");

        if (userAccount2 is null)
            Assert.Fail("Failed to create user account 2");

        var player1 = RocketLeaguePlayer.Create(userAccount1);
        var player2 = RocketLeaguePlayer.Create(userAccount2);

        var lobby = RocketLeague2vs2Lobby.Create(player1, player2);

        var command = new GetRocketLeague2vs2LobbyQuery();

        using (var contextMock = _utils.dbContext())
        {
            contextMock.RocketLeague2vs2Lobbies.Add(lobby);
            await contextMock.SaveChangesAsync();

            var getRocketLeague2vs2Lobby =
                new GetRocketLeague2vs2Lobby(contextMock, _utils.ClaimsPrincipal);

            var response = await getRocketLeague2vs2Lobby.Handle(command, CancellationToken.None);

            Assert.Equal("DUMMY", response.Player1.UserAccountName);
            Assert.Equal("dummy", response.Player1.DiscordName);
            Assert.Equal("https://steamcommunity.com/id/dummy/", response.Player1.UserAccountSteamProfileLink);
            Assert.Equal("dummy", response.Player1.EpicName);

            Assert.Equal("TEST", response.Player2.UserAccountName);
            Assert.Equal("test", response.Player2.DiscordName);
            Assert.Equal("https://steamcommunity.com/id/test/", response.Player2.UserAccountSteamProfileLink);
            Assert.Equal("test", response.Player2.EpicName);
        }
    }

    [Fact]
    public async Task Handler_ShouldThrowResourceMissingException_WhenLeague2vs2LobbyNotFound()
    {
        var command = new GetRocketLeague2vs2LobbyQuery();

        using (var contextMock = _utils.dbContext())
        {
            var getRocketLeague2vs2Lobby =
                new GetRocketLeague2vs2Lobby(contextMock, _utils.ClaimsPrincipal);

            await Assert.ThrowsAsync<ResourceMissingException>(async ()
                => await getRocketLeague2vs2Lobby.Handle(command, CancellationToken.None));
        }
    }
}
