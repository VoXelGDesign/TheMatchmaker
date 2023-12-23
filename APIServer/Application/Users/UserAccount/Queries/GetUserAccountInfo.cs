using Application.Exceptions.CustomExceptions;
using Domain.Users.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Users.UserAccount.Queries;

public record UserAccountInfoDto(string? Name = null, string? SteamProfileLink = null, string? DiscordName = null);
public record GetUserAccountInfoQuery() : IRequest<UserAccountInfoDto>;


public class GetUserAccountInfoHandler : IRequestHandler<GetUserAccountInfoQuery, UserAccountInfoDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;
    public GetUserAccountInfoHandler(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }
    public async Task<UserAccountInfoDto> Handle(GetUserAccountInfoQuery request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null)
        {
            throw new IdClaimNotFoundException();
        }

        var userAccountId = new UserId(Guid.Parse(claimidentity));

        var userAccountInfo = await _applicationDbContext.UserAccounts.SingleOrDefaultAsync(x => x.Id == userAccountId);

        if (userAccountInfo is null)
        {
            return new UserAccountInfoDto(null, null);
        }

        return new UserAccountInfoDto(userAccountInfo.Name.Name, userAccountInfo.SteamProfileLink.Link, userAccountInfo.DiscordName.Name);
    }
}
