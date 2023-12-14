using Application.Exceptions.CustomExceptions;
using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;
using Domain.Users.UserGamesRanks;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Application.Users.UserGameRanks.RocketLeague.Commands;

public record RocketLeagueUpdateRankDto(RocketLeagueRankName Name, RocketLeagueRankNumber Number, RocketLeagueDivision Division);

public record UpdateRocketLeagueRankCommand(RocketLeagueUpdateRankDto dto) : IRequest<RocketLeagueUpdateRankDto>;

public class UpdateUserAccountInfoHandler : IRequestHandler<UpdateRocketLeagueRankCommand, RocketLeagueUpdateRankDto>
{

    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;

    public UpdateUserAccountInfoHandler(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }

    public async Task<RocketLeagueUpdateRankDto> Handle(UpdateRocketLeagueRankCommand request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null) throw new IdClaimNotFoundException();

        var userId = new UserId(Guid.Parse(claimidentity));

        var userGameRank = await _applicationDbContext.UserGameRanks.SingleOrDefaultAsync(x => x.UserId == userId);

        var rocketLeagueRank = RocketLeagueRank.Create(request.dto.Name, request.dto.Number, request.dto.Division);

        if(rocketLeagueRank is null)
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

        userGameRank.UpdateRocketLeagueRank(rocketLeagueRank);

        if (userGameRank.RocketLeagueRank is null)
        {
            throw new ResourceCreationFailedException();
        }

        await _applicationDbContext.SaveChangesAsync();

        return new RocketLeagueUpdateRankDto(
            userGameRank.RocketLeagueRank.RocketLeagueRankName,
            userGameRank.RocketLeagueRank.RocketLeagueRankNumber,
            userGameRank.RocketLeagueRank.RocketLeagueDivision);
    }
}
