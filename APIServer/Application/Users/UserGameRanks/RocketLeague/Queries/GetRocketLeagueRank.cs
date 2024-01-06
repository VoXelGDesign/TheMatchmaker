using Application.Exceptions.CustomExceptions;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;
using Contracts.QueueContracts.RocketLeague;
using Domain.Users.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Users.UserGameRanks.RocketLeague.Queries;


public record GetUserRocketLeagueRankCommand() : IRequest<GetRocketLeagueRankResponse>;


public class GetUserAccountInfoHandler : IRequestHandler<GetUserRocketLeagueRankCommand, GetRocketLeagueRankResponse>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;
    public GetUserAccountInfoHandler(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }
    public async Task<GetRocketLeagueRankResponse> Handle(
        GetUserRocketLeagueRankCommand request, 
        CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        var response = new GetRocketLeagueRankResponse
        {
            Rank2vs2 = null,
            Rank3vs3 = null
        };

        if (claimidentity == null)
            throw new IdClaimNotFoundException();

        var userId = new UserId(Guid.Parse(claimidentity));

        var userGameRank = await _applicationDbContext.UserGameRanks
            .SingleOrDefaultAsync(x => x.UserId == userId);

        if (userGameRank is null)
            return response;

        if(userGameRank.RocketLeague2vs2Rank is null 
            && userGameRank.RocketLeague3vs3Rank is null)
            return response;



        if (userGameRank.RocketLeague2vs2Rank is not null)
            response.Rank2vs2 = RocketLeagueRankDto
                .ParseRocketLeagueRankToDto(userGameRank.RocketLeague2vs2Rank);



        if (userGameRank.RocketLeague3vs3Rank is not null)
            response.Rank3vs3 = RocketLeagueRankDto
                .ParseRocketLeagueRankToDto(userGameRank.RocketLeague3vs3Rank);


        return response;

    }
}
