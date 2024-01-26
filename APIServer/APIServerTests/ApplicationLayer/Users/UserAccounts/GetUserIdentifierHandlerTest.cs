using Application.Exceptions.CustomExceptions;
using Application.Users.UserAccount.Queries;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Users.UserAccounts;

public class GetUserIdentifierHandlerTest
{
    private TestUtils _utils = new TestUtils();

    [Fact]
    public async Task Handler_ShouldReturnUserId_WhenClaimIdIsPersent()
    {
        
        var getUserIdentifier = new GetUserIdentifierHandler(_utils.ClaimsPrincipal);
        var command = new GetUserIdentifierQuery();
        var result = await getUserIdentifier.Handle(command, CancellationToken.None);

        Assert.Equal(_utils.UserIdDtoFromClaimsPrincipal().UserId, result.UserId);
    }

    [Fact]

    public async Task Handler_ShouldThrowIdClaimNotFoundException_WhenClaimIdentityIsNull()
    {
        var principal = new ClaimsPrincipal();
        var getUserIdentifier = new GetUserIdentifierHandler(principal);
        var command = new GetUserIdentifierQuery();        

        await Assert.ThrowsAsync<IdClaimNotFoundException>(async () => await getUserIdentifier.Handle(command, CancellationToken.None));
    }
}
