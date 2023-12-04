namespace Domain.Users.UserAccounts;

public sealed class UserAccount
{

    public UserAccountId Id { get; private set; } = null!;
    public UserAccountName Name { get; private set; } = null!;
    public UserAccountSteamProfileLink SteamProfileLink { get; private set; } = null!;

    public static UserAccount? Create(UserAccountId id, UserAccountName name, UserAccountSteamProfileLink steamProfileLink)
        => new UserAccount
        {
            Id = id,
            Name = name,
            SteamProfileLink = steamProfileLink
        };

    public void UpdateUserAccount(UserAccountName? name = null, UserAccountSteamProfileLink? steamProfileLink = null)
    {
        if (name is not null)
        {
            Name = name;
        }

        if (steamProfileLink is not null)
        {
            SteamProfileLink = steamProfileLink;
        }

    }
}

