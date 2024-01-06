using Domain.Users.UserAccounts.Properties;

namespace Domain.Users.UserAccounts;
public sealed record UserDiscordName
{
    public string Name { get; private set; }

    public static UserDiscordName? Create(string? name)
    {
        if (!IsValidName(name)) return null;

        return new UserDiscordName
        {
            Name = name!
        };

    }

    private UserDiscordName()
    {
        Name = UserAccountGeneralProperties.StringPlaceholder;
    }

    public static UserDiscordName Default()
        => new UserDiscordName();

    private static bool IsValidName(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        if (!UserAccountValidationProperties.DiscordNameRegex.IsMatch(name))
        {
            return false;
        }

        return true;
    }
}
