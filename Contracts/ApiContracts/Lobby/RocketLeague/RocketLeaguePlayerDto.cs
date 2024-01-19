using Contracts.Common;

namespace Contracts.ApiContracts.Lobby.RocketLeague;

public record RocketLeaguePlayerDto
{
    public UserIdDto UserIdDto { get; set; }
    public string UserAccountName { get; set; }

    public string UserAccountSteamProfileLink { get; set; }

    public string DiscordName { get; set; }
    public bool IsReady { get; set; }

    public RocketLeaguePlayerDto(
    UserIdDto userIdDto,
    string userAccountName,
    string userAccountSteamProfileLink,
    string discordName,
    bool isReady)
    {
        UserIdDto = userIdDto;
        UserAccountName = userAccountName;
        UserAccountSteamProfileLink = userAccountSteamProfileLink;
        DiscordName = discordName;
        IsReady = isReady;
    }
    
}
