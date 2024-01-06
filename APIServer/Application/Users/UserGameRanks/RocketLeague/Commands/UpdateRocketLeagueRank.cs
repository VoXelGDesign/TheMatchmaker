using Application.Exceptions.CustomExceptions;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Requests;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;
using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;
using Domain.Users.UserGamesRanks;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Application.Users.UserGameRanks.RocketLeague.Commands;



public record UpdateRocketLeagueRankCommand(UpdateRocketLeagueRankRequest dto) : IRequest<UpdateRocketLeagueRankResponse>;

public class UpdateUserAccountInfoHandler : IRequestHandler<UpdateRocketLeagueRankCommand, UpdateRocketLeagueRankResponse>
{

    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;

    public UpdateUserAccountInfoHandler(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }

    public async Task<UpdateRocketLeagueRankResponse> Handle(UpdateRocketLeagueRankCommand request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null) 
            throw new IdClaimNotFoundException();

        var userId = new UserId(Guid.Parse(claimidentity));

        var userGameRank =
            await _applicationDbContext.UserGameRanks
            .SingleOrDefaultAsync(x => x.UserId == userId);

        var rocketLeagueRank = RocketLeagueRank.Create(
                request.dto.Name,
                request.dto.Number,
                request.dto.Division
                );

        if (rocketLeagueRank is null)
        {
            throw new ResourceCreationFailedException();
        }

        if (userGameRank is null)
        {
            userGameRank = UserGameRank.Create(userId, rocketLeagueRank)
                ?? throw new ResourceCreationFailedException();

            _applicationDbContext.UserGameRanks.Add(userGameRank);
            await _applicationDbContext.SaveChangesAsync();
        }


        userGameRank.UpdateRocketLeagueRank(rocketLeagueRank, request.dto.Mode);
        await _applicationDbContext.SaveChangesAsync();

        switch (request.dto.Mode)
        {
            case "2VS2":

                if (userGameRank.RocketLeague2vs2Rank is null)
                    throw new ResourceCreationFailedException();

                return new UpdateRocketLeagueRankResponse(
                   userGameRank.RocketLeague2vs2Rank.RocketLeagueRankName.ToString(),
                   userGameRank.RocketLeague2vs2Rank.RocketLeagueRankNumber.ToString(),
                   userGameRank.RocketLeague2vs2Rank.RocketLeagueDivision.ToString());

            case "3VS3":

                if (userGameRank.RocketLeague3vs3Rank is null)
                    throw new ResourceCreationFailedException();

                return new UpdateRocketLeagueRankResponse(
                   userGameRank.RocketLeague3vs3Rank.RocketLeagueRankName.ToString(),
                   userGameRank.RocketLeague3vs3Rank.RocketLeagueRankNumber.ToString(),
                   userGameRank.RocketLeague3vs3Rank.RocketLeagueDivision.ToString());

            default:
                throw new ResourceCreationFailedException();
        }

    }
}
