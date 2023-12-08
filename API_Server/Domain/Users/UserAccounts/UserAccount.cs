namespace Domain.Users.UserAccounts;

public sealed class UserAccount
{

    public UserAccountId Id { get; private set; } = null!;
    public UserAccountName Name { get; private set; } = null!;
    public UserAccountSteamProfileLink SteamProfileLink { get; private set; } = null!;
    public UserDiscordName DiscordName { get; private set; } = null!;

    public static UserAccount? Create(UserAccountId id, UserAccountName name, UserAccountSteamProfileLink steamProfileLink, UserDiscordName userDiscordName)
        => new UserAccount
        {
            Id = id,
            Name = name,
            SteamProfileLink = steamProfileLink,
            DiscordName = userDiscordName
        };

    public void UpdateUserAccount(
        UserAccountName? name = null, 
        UserAccountSteamProfileLink? steamProfileLink = null, 
        UserDiscordName? userDiscordName = null)
    {
        if (name is not null)
        {
            Name = name;
        }

        if (steamProfileLink is not null)
        {
            SteamProfileLink = steamProfileLink;
        }

        if(userDiscordName is not null)
        {
            DiscordName = userDiscordName;
        }

    }
}

