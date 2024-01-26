namespace Contracts.ApiContracts.UserAccountInfo.Responses;

public record GetUserAccountInfoResponse(
    string? Name = null,
    string? SteamProfileLink = null,
    string? DiscordName = null,
    string? EpicName = null);

