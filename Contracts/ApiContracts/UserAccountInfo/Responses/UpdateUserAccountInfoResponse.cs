namespace Contracts.ApiContracts.UserAccountInfo.Responses;
public record UpdateUserAccountInfoResponse
{
    public string? Name { get; set; }
    public string? SteamProfileLink { get; set; }
    public string? DiscordName { get; set; }

    public UpdateUserAccountInfoResponse(
    string? name = null,
    string? steamProfileLink = null,
    string? discordName = null)
    {
        Name = name;
        SteamProfileLink = steamProfileLink;
        DiscordName = discordName;
    }
}
