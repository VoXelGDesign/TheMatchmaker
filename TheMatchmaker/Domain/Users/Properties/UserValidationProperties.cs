using System.Text.RegularExpressions;

namespace Domain.Users.Properties
{
    public static class UserValidationProperties
    {
        public static readonly Regex SteamLinkRegex = new Regex(@"^https:\/\/steamcommunity\.com\/id\/[a-zA-Z0-9]+\/?$");

        public static readonly Regex PasswordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");

        public static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
        
        public static readonly Regex NameRegex = new Regex(@"^[a-zA-Z0-9_.+-]+$");

        public const int MaxNameLength = 25;

        public const int MinNameLength = 4;
    }
}
