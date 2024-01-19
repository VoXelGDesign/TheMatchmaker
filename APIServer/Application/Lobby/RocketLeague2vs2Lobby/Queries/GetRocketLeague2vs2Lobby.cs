using Application.Exceptions.CustomExceptions;
using Contracts.ApiContracts.Lobby.RocketLeague;
using Contracts.Common;
using Domain.Users.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Lobby.RocketLeague2vs2Lobby.Queries;

public record GetRocketLeague2vs2LobbyQuery() : IRequest<RocketLeague2vs2LobbyResponse>;
public class GetRocketLeague2vs2Lobby : IRequestHandler<GetRocketLeague2vs2LobbyQuery, RocketLeague2vs2LobbyResponse>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;

    public GetRocketLeague2vs2Lobby(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }

    public async  Task<RocketLeague2vs2LobbyResponse> Handle(GetRocketLeague2vs2LobbyQuery request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null)
        {
            throw new IdClaimNotFoundException();
        }

        var userAccountId = new UserId(Guid.Parse(claimidentity));

        var lobby = await _applicationDbContext.RocketLeague2vs2Lobbies
            .SingleOrDefaultAsync(x => x.Player1.UserId == userAccountId || x.Player2.UserId == userAccountId);

        if (lobby is null)
            throw new ResourceMissingException();

        var player1Dto = new RocketLeaguePlayerDto(
            new UserIdDto(lobby.Player1.UserId.Id.ToString()),
            lobby.Player1.UserAccountName.Name.ToString(),
            lobby.Player1.UserAccountSteamProfileLink.Link.ToString(),
            lobby.Player1.DiscordName.Name.ToString(),
            lobby.Player1.IsReady
            );

        var player2Dto = new RocketLeaguePlayerDto(
            new UserIdDto(lobby.Player2.UserId.Id.ToString()),
            lobby.Player2.UserAccountName.Name.ToString(),
            lobby.Player2.UserAccountSteamProfileLink.Link.ToString(),
            lobby.Player2.DiscordName.Name.ToString(),
            lobby.Player2.IsReady
            );       

        return new RocketLeague2vs2LobbyResponse(player1Dto, player2Dto);
    }
}
