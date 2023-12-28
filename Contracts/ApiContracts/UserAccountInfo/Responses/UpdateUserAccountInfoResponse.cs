namespace Contracts.ApiContracts.UserAccountInfo.Responses;
public record UpdateUserAccountInfoResponse(
    string? Name = null,
    string? SteamProfileLink = null,
    string? DiscordName = null);
