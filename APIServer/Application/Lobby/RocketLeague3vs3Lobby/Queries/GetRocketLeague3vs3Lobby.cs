using Application.Exceptions.CustomExceptions;
using Contracts.ApiContracts.Lobby.RocketLeague;
using Contracts.Common;
using Domain.Users.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Lobby.RocketLeague3vs3Lobby.Queries;

public record GetRocketLeague3vs3LobbyQuery() : IRequest<RocketLeague3vs3LobbyResponse>;
public class GetRocketLeague3vs3Lobby : IRequestHandler<GetRocketLeague3vs3LobbyQuery, RocketLeague3vs3LobbyResponse>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;

    public GetRocketLeague3vs3Lobby(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }

    public async Task<RocketLeague3vs3LobbyResponse> Handle(GetRocketLeague3vs3LobbyQuery request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null)
        {
            throw new IdClaimNotFoundException();
        }

        var userAccountId = new UserId(Guid.Parse(claimidentity));

        var lobby = await _applicationDbContext.RocketLeague3vs3Lobbies
            .SingleOrDefaultAsync(x => 
            x.Player1.UserId == userAccountId ||
            x.Player2.UserId == userAccountId ||
            x.Player3.UserId == userAccountId);

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

        var player3Dto = new RocketLeaguePlayerDto(
            new UserIdDto(lobby.Player3.UserId.Id.ToString()),
            lobby.Player3.UserAccountName.Name.ToString(),
            lobby.Player3.UserAccountSteamProfileLink.Link.ToString(),
            lobby.Player3.DiscordName.Name.ToString(),
            lobby.Player3.IsReady
            );

        return new RocketLeague3vs3LobbyResponse(player1Dto, player2Dto, player3Dto, lobby.CreationDate);
    }
}
