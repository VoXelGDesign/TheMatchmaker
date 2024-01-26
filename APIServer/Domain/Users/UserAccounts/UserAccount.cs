using Domain.Users.User;
using System.ComponentModel.DataAnnotations;

namespace Domain.Users.UserAccounts;

public sealed class UserAccount
{
    [Key]
    public UserId Id { get; private set; } = null!;
    public UserAccountName Name { get; private set; } = null!;
    public UserAccountSteamProfileLink SteamProfileLink { get; private set; } = null!;
    public UserDiscordName DiscordName { get; private set; } = null!;
    public UserEpicName EpicName { get; private set; } = null!;

    public static UserAccount? Create(UserId id, UserAccountName name, UserAccountSteamProfileLink steamProfileLink, UserDiscordName userDiscordName, UserEpicName userEpicName)
        => new UserAccount
        {
            Id = id,
            Name = name,
            SteamProfileLink = steamProfileLink,
            DiscordName = userDiscordName,
            EpicName = userEpicName
        };

    public void UpdateUserAccount(
        UserAccountName? name = null, 
        UserAccountSteamProfileLink? steamProfileLink = null, 
        UserDiscordName? userDiscordName = null,
        UserEpicName? userEpicName = null)
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

        if(userEpicName is not null)
        {
            EpicName = userEpicName;
        }

    }
}

