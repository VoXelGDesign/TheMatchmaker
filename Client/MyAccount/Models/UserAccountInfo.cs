namespace Client.MyAccount.Models
{
    public record UserAccountInfo
    {
        public string? Name { get; set; }
        public string? SteamProfileLink { get; set; }   
        public string? DiscordName { get; set;}
    }
}
