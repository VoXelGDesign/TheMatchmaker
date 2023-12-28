using Application.Exceptions.CustomExceptions;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;
using Domain.Users.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Users.UserGameRanks.RocketLeague.Queries;


public record GetUserRocketLeagueRankCommand() : IRequest<GetRocketLeagueRankResponse>;


public class GetUserAccountInfoHandler : IRequestHandler<GetUserRocketLeagueRankCommand, GetRocketLeagueRankResponse?>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;
    public GetUserAccountInfoHandler(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }
    public async Task<GetRocketLeagueRankResponse?> Handle(GetUserRocketLeagueRankCommand request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null)
        {
            throw new IdClaimNotFoundException();
        }

        var userId = new UserId(Guid.Parse(claimidentity));

        var userGameRank = await _applicationDbContext.UserGameRanks.SingleOrDefaultAsync(x => x.UserId == userId);

        if (userGameRank is null)
        {
            return null;
        }

        if(userGameRank.RocketLeagueRank is null)
        {
            return null;
        }

        return new GetRocketLeagueRankResponse(
            userGameRank.RocketLeagueRank.RocketLeagueRankName.ToString(),
            userGameRank.RocketLeagueRank.RocketLeagueRankNumber.ToString(),
            userGameRank.RocketLeagueRank.RocketLeagueDivision.ToString());

    }
}
