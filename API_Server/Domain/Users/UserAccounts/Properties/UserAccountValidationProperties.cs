using System.Text.RegularExpressions;

namespace Domain.Users.UserAccounts.Properties
{
    internal static class UserAccountValidationProperties
    {
        internal static readonly Regex SteamLinkRegex = new Regex(@"^https:\/\/steamcommunity\.com\/id\/[a-zA-Z0-9]+\/?$");

        internal static readonly Regex NameRegex = new Regex(@"^[a-zA-Z0-9_.+-]+$");

        internal static readonly Regex DiscordNameRegex = new Regex(@"^[a-zA-Z0-9]{2,32}$");

        internal const int MaxNameLength = 25;

        internal const int MinNameLength = 4;

    }
}
