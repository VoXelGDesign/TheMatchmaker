using Application.Exceptions.CustomExceptions;
using Application.Queue;
using Application.Users.UserAccount.Commands;
using Contracts.ApiContracts.UserAccountInfo.Requests;
using Contracts.ApiContracts.UserAccountInfo.Responses;
using Domain.Users.User;
using Domain.Users.UserAccounts;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Users.UserAccounts;

public class UpdateUserAccountInfoHandlerTest
{
    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) }));

    public static DbContextOptions<ApplicationDbContext> Options()
        => new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

    [Fact]
    public async Task Handler_ShouldUpdateUserAccountInfo_Successfully()
    {
        UpdateUserAccountInfoResponse updatedUser;
        var userId = new UserId(Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)));

        var name = UserAccountName.Create("DUMMY");
        var link = UserAccountSteamProfileLink.Create("https://steamcommunity.com/id/test/");
        var discordName = UserDiscordName.Create("dummy");

        if (name is null || link is null || discordName is null)
            Assert.Fail("Failed to create resources for user account");

        var userAccount = UserAccount.Create(userId,name,link,discordName);
        var dto = new UpdateUserAccountInfoRequest("TEST", "https://steamcommunity.com/id/test/", "test");
        var command = new UpdateUserAccountInfoCommand(dto);

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            contextMock.UserAccounts.Add(userAccount);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            var updateUserAccount = new UpdateUserAccountInfoHandler(contextMock, claimsPrincipal);
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

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            var updateUserAccount = new UpdateUserAccountInfoHandler(contextMock, new ClaimsPrincipal());
            await Assert.ThrowsAsync<IdClaimNotFoundException>(async () => await updateUserAccount.Handle(command, CancellationToken.None));
        }
    }

    [Fact]
    public async Task Handler_ShouldCreateUserAccount_IfUserAccountInfoIsNull()
    {
        UpdateUserAccountInfoResponse updatedUser;
        var userId = new UserId(Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)));
      
        var dto = new UpdateUserAccountInfoRequest("TEST", "https://steamcommunity.com/id/test/", "test");
        var command = new UpdateUserAccountInfoCommand(dto);

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            var updateUserAccount = new UpdateUserAccountInfoHandler(contextMock, claimsPrincipal);
            updatedUser = await updateUserAccount.Handle(command, CancellationToken.None);
        }

        Assert.Equal("TEST", updatedUser.Name);
        Assert.Equal("https://steamcommunity.com/id/test/", updatedUser.SteamProfileLink);
        Assert.Equal("test", updatedUser.DiscordName);
    }
}
