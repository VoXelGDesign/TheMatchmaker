using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Domain.Users;

public sealed class User
{

    public UserId Id { get; private set; } = null!;
    //public UserName Name { get; private set; }
    public UserSteamProfileLink SteamProfileLink { get; private set; } = null!;


}

