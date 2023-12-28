namespace Contracts.ApiContracts.UserAccountInfo.Requests;

public record UpdateUserAccountInfoRequest(
    string? Name = null,
    string? SteamProfileLink = null,
    string? DiscordName = null);

