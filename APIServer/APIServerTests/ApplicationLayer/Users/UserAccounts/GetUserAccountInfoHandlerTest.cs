using Application.Exceptions.CustomExceptions;
using Application.Users.UserAccount.Queries;
using Domain.Users.UserAccounts;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Users.UserAccounts;

public class GetUserAccountInfoHandlerTest
{
    private TestUtils _utils = new TestUtils();

    [Fact]
    public async Task Handler_ShouldThrowIdClaimNotFoundException_WhenClaimIdentityIsNull()
    {
        var command = new GetUserAccountInfoQuery();

        using (var contextMock = _utils.dbContext())
        {
            var getUserAccountInfo = 
                new GetUserAccountInfoHandler(contextMock, new ClaimsPrincipal());

            await Assert.ThrowsAsync<IdClaimNotFoundException>(async () 
                => await getUserAccountInfo.Handle(command, CancellationToken.None));
        }
    }

    [Fact]
    public async Task Handler_ReturnsCorrectAccountInfoResponse_WhenAccountInfoIsPresent()
    {
        var command = new GetUserAccountInfoQuery();
        var userId = _utils.UserIdFromClaimsPrincipal();

        var name = UserAccountName
            .Create("DUMMY");

        var link = UserAccountSteamProfileLink
            .Create("https://steamcommunity.com/id/dummy/");

        var discordName = UserDiscordName
            .Create("dummy");

        var epicName = UserEpicName
            .Create("dummy");

        if (name is null || link is null || discordName is null || epicName is null)
            Assert.Fail("Failed to create resources for user account");

        var userAccount = UserAccount.Create(userId, name, link, discordName, epicName);

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserAccounts.Add(userAccount);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var getUserAccountInfo = 
                new GetUserAccountInfoHandler(contextMock, _utils.ClaimsPrincipal);

            var result = await getUserAccountInfo
                .Handle(command, CancellationToken.None);

            Assert.Equal("DUMMY", result.Name);
            Assert.Equal("https://steamcommunity.com/id/dummy/", result.SteamProfileLink);
            Assert.Equal("dummy", result.DiscordName);
        }
    }

    [Fact]
    public async Task Handler_ReturnsEmptyAccountInfoResponse_WhenAccountInfoIsNotPresent()
    {
        var command = new GetUserAccountInfoQuery();
        using (var contextMock = _utils.dbContext())
        {
            var getUserAccountInfo = 
                new GetUserAccountInfoHandler(contextMock, _utils.ClaimsPrincipal);

            var result = await getUserAccountInfo
                .Handle(command, CancellationToken.None);

            Assert.Equal((string?)null, result.Name);
            Assert.Equal((string?)null, result.SteamProfileLink);
            Assert.Equal((string?)null, result.DiscordName);
        }
    }
}