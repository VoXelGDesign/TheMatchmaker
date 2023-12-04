using Domain.Users.UserAccounts.Properties;

namespace Domain.Users.UserAccounts;

public record UserAccountName
{
    public string Name { get; private set; }



    public static UserAccountName? Create(string? name)
    {
        if (!IsValidName(name)) return null;

        return new UserAccountName
        {
            Name = name!
        };

    }


    private UserAccountName()
    {
        Name = "EMPTY";
    }

    public static UserAccountName Default()
        => new UserAccountName();


    private static bool IsValidName(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        if (UserAccountValidationProperties.MaxNameLength < name.Length)
        {
            return false;
        }

        if (UserAccountValidationProperties.MinNameLength > name.Length)
        {
            return false;
        }

        if (!UserAccountValidationProperties.NameRegex.IsMatch(name))
        {
            return false;
        }

        return true;
    }
}
