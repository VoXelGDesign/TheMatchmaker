

namespace Client.MyAccount.Models
{
    public record RocketLeagueRank
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public string Division { get; set; }

        public RocketLeagueRank()
        {
            Name = "BRONZE";
            Number = "I";
            Division = "I";
        }
    }
}
