namespace Contracts.ApiContracts.UserAccountInfo.Requests;

public record UpdateUserAccountInfoRequest
{
    public string? Name { get; set; }
    public string? SteamProfileLink { get; set; }
    public string? DiscordName { get; set; }

    public UpdateUserAccountInfoRequest(
    string? name = null,
    string? steamProfileLink = null,
    string? discordName = null)
    {
        Name = name;
        SteamProfileLink = steamProfileLink;
        DiscordName = discordName;
    }
}

