using Domain.Users.User;
using Domain.Users.UserAccounts;

namespace Domain.Games.RocketLeague.Palyers;

public record RocketLeaguePlayerInfo
{
    public UserId UserId { get; private set; }
    public UserAccountName UserAccountName { get; private set; }
    public UserAccountSteamProfileLink UserAccountSteamProfileLink { get; private set; }
    public UserDiscordName DiscordName { get; private set; }

    public RocketLeaguePlayerInfo(UserAccount userAccount)
    {
        UserId = userAccount.Id;
        UserAccountName = userAccount.Name;
        UserAccountSteamProfileLink = userAccount.SteamProfileLink;
        DiscordName = userAccount.DiscordName;
    }
}