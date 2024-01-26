using Application.Exceptions.CustomExceptions;
using Domain.Users.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Contracts.ApiContracts.UserAccountInfo.Responses;

namespace Application.Users.UserAccount.Queries;


public record GetUserAccountInfoQuery() : IRequest<GetUserAccountInfoResponse>;


public class GetUserAccountInfoHandler : IRequestHandler<GetUserAccountInfoQuery, GetUserAccountInfoResponse>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;
    public GetUserAccountInfoHandler(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }
    public async Task<GetUserAccountInfoResponse> Handle(GetUserAccountInfoQuery request, CancellationToken cancellationToken)
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
            return new GetUserAccountInfoResponse(null, null, null, null);
        }

        return new GetUserAccountInfoResponse(userAccountInfo.Name.Name, userAccountInfo.SteamProfileLink.Link, userAccountInfo.DiscordName.Name, userAccountInfo.EpicName.Name);
    }
}
