using Domain.Users.UserAccounts.Properties;

namespace Domain.Users.UserAccounts;

public sealed record UserEpicName
{
    public string Name { get; private set; }

    public static UserEpicName? Create(string? name)
    {
        if (!IsValidName(name)) return null;

        return new UserEpicName
        {
            Name = name!
        };

    }

    private UserEpicName()
    {
        Name = UserAccountGeneralProperties.StringPlaceholder;
    }

    public static UserEpicName Default()
        => new UserEpicName();

    private static bool IsValidName(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        if (!UserAccountValidationProperties.EpicNameRegex.IsMatch(name))
        {
            return false;
        }

        return true;
    }
}
