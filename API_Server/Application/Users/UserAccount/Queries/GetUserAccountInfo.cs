using Domain.Users.UserAccounts;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Users.UserAccount.Commands;

public record UserAccountInfoDto(string? Name, string? SteamProfileLink);

public record GetUserAccountInfoCommand() : IRequest<UserAccountInfoDto>;


public class GetUserAccountInfoHandler : IRequestHandler<GetUserAccountInfoCommand, UserAccountInfoDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;
    public GetUserAccountInfoHandler(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }
    public async Task<UserAccountInfoDto> Handle(GetUserAccountInfoCommand request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null)
        {
            throw new Exception("Identity does not exist");
        }

        var userAccountId = new UserAccountId(Guid.Parse(claimidentity));

        var userAccountInfo = await _applicationDbContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == userAccountId);

        if (userAccountInfo is null)
        {
            return new UserAccountInfoDto(null, null);
        }

        return new UserAccountInfoDto(userAccountInfo.Name.Name, userAccountInfo.SteamProfileLink.Link);
    }
}
