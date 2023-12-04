using System.Text.RegularExpressions;

namespace Domain.Users.UserAccounts.Properties
{
    public static class UserAccountValidationProperties
    {
        public static readonly Regex SteamLinkRegex = new Regex(@"^https:\/\/steamcommunity\.com\/id\/[a-zA-Z0-9]+\/?$");

        public static readonly Regex NameRegex = new Regex(@"^[a-zA-Z0-9_.+-]+$");

        public const int MaxNameLength = 25;

        public const int MinNameLength = 4;
    }
}
