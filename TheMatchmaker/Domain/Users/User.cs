using System.Text.RegularExpressions;

namespace Domain.Users;

public sealed class User
{
    public UserId Id { get; private set; } = null!;
    public UserName Name { get; private set; } = null!;
    public UserEmail Email { get; private set; } = null!;
    public UserHashedPassword HashedPassword { get; private set; } = null!;
}

public record UserSteamProfileLink
{
    
    public string Link { get; private set; } = null!;

    public UserSteamProfileLink()
    {
        
    }
}

