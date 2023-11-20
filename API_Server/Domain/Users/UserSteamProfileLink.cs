using Domain.Users.Properties;

namespace Domain.Users;

public record UserSteamProfileLink
{
    
    public string Link { get; private set; } = null!;

    public UserSteamProfileLink(string link)
    {
        if (string.IsNullOrEmpty(link))
        {
            throw new ArgumentException("Link cannot be null or empty.");
        }

        if (!UserValidationProperties.SteamLinkRegex.IsMatch(link))
        {
            throw new ArgumentException("Link must be a valid steam profile link.");
        }
    }
}

