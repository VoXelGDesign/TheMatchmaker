using Domain.Users.User;
using Domain.Users.UserAccounts;

namespace Domain.Games.RocketLeague.Players;

public class RocketLeaguePlayer
{
    public UserId UserId { get; private set; } = null!;
    public UserAccountName UserAccountName { get; private set; } = null!;
    public UserAccountSteamProfileLink UserAccountSteamProfileLink { get; private set; } = null!;
    public UserDiscordName DiscordName { get; private set; } = null!;
    public bool IsReady { get; private set; } = false;

    public static RocketLeaguePlayer Create(UserAccount userAccount)
    => new RocketLeaguePlayer
    {
        UserId = userAccount.Id,
        UserAccountName = userAccount.Name,
        UserAccountSteamProfileLink = userAccount.SteamProfileLink,
        DiscordName = userAccount.DiscordName,
    };
    public void SetReady() 
        => IsReady = true;

}
