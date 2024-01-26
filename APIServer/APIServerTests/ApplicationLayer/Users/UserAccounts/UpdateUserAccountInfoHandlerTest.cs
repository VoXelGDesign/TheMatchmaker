using Application.Exceptions.CustomExceptions;
using Application.Users.UserAccount.Commands;
using Contracts.ApiContracts.UserAccountInfo.Requests;
using Contracts.ApiContracts.UserAccountInfo.Responses;
using Domain.Users.UserAccounts;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Users.UserAccounts;

public class UpdateUserAccountInfoHandlerTest
{
    private TestUtils _utils = new TestUtils();

    [Fact]
    public async Task Handler_ShouldUpdateUserAccountInfo_Successfully()
    {
        UpdateUserAccountInfoResponse updatedUser;      
        var name = UserAccountName.Create("DUMMY");
        var link = UserAccountSteamProfileLink.Create("https://steamcommunity.com/id/dummy/");
        var discordName = UserDiscordName.Create("dummy");
        var epicName = UserEpicName.Create("dummy");

        if (name is null || link is null || discordName is null)
            Assert.Fail("Failed to create resources for user account");

        var userAccount = UserAccount.Create(_utils.UserIdFromClaimsPrincipal(),name,link,discordName, epicName);
        var dto = new UpdateUserAccountInfoRequest("TEST", "https://steamcommunity.com/id/test/", "test");
        var command = new UpdateUserAccountInfoCommand(dto);

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserAccounts.Add(userAccount);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var updateUserAccount = new UpdateUserAccountInfoHandler(contextMock, _utils.ClaimsPrincipal);
            updatedUser = await updateUserAccount.Handle(command, CancellationToken.None);
        }

        Assert.Equal("TEST", updatedUser.Name);
        Assert.Equal("https://steamcommunity.com/id/test/", updatedUser.SteamProfileLink);
        Assert.Equal("test", updatedUser.DiscordName);
    }

    [Fact]
    public async Task Handler_ShouldThrowIdClaimNotFoundException_WhenClaimIdentityIsNull()
    {
        var dto = new UpdateUserAccountInfoRequest("", "", "");
        var command = new UpdateUserAccountInfoCommand(dto);       

        using (var contextMock = _utils.dbContext())
        {
            var updateUserAccount = 
                new UpdateUserAccountInfoHandler(contextMock, new ClaimsPrincipal());

            await Assert.ThrowsAsync<IdClaimNotFoundException>(async () 
                => await updateUserAccount.Handle(command, CancellationToken.None));
        }
    }

    [Fact]
    public async Task Handler_ShouldCreateUserAccount_IfUserAccountInfoIsNull()
    {
        UpdateUserAccountInfoResponse updatedUser;        
        var dto = new UpdateUserAccountInfoRequest("TEST", "https://steamcommunity.com/id/test/", "test");
        var command = new UpdateUserAccountInfoCommand(dto);

        using (var contextMock = _utils.dbContext())
        {
            var updateUserAccount = new UpdateUserAccountInfoHandler(contextMock, _utils.ClaimsPrincipal);
            updatedUser = await updateUserAccount.Handle(command, CancellationToken.None);
        }

        Assert.Equal("TEST", updatedUser.Name);
        Assert.Equal("https://steamcommunity.com/id/test/", updatedUser.SteamProfileLink);
        Assert.Equal("test", updatedUser.DiscordName);
    }
}
