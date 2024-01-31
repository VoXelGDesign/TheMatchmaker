using Domain.Users.UserAccounts.Properties;

namespace Domain.Users.UserAccounts;

public sealed record UserAccountSteamProfileLink
{

    public string Link { get; private set; } = null!;

    private UserAccountSteamProfileLink()
    {
        Link = UserAccountGeneralProperties.StringPlaceholder;
    }

    public static UserAccountSteamProfileLink? Create(string? link)
    {
        if (!IsValidLink(link)) return null;

        return new UserAccountSteamProfileLink
        {
            Link = link!
        };
    }


    public static UserAccountSteamProfileLink Default()
        => new UserAccountSteamProfileLink();

    private static bool IsValidLink(string? link)
    {
        if(link == "EMPTY")
            return true;

        if (string.IsNullOrEmpty(link))
        {
            return false;
        }

        if (!UserAccountValidationProperties.SteamLinkRegex.IsMatch(link))
        {
            return false;
        }

        return true;
    }

}

