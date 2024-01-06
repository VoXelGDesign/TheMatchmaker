using Domain.Users.UserAccounts;

namespace Domain.Games.RocketLeague.Palyers;

public class RocketLeaguePlayer
{
    public RocketLeaguePalyerId PlayerId { get; private set; } = null!;
    public RocketLeaguePlayerInfo PlayerInfo { get; private set; } = null!;
    public bool IsReady { get; private set; } = false;
    public DateTime CreationTime { get; private set; }

    public RocketLeaguePlayer Create(UserAccount userAccount)
    => new RocketLeaguePlayer
    {
        PlayerId = new RocketLeaguePalyerId(Guid.NewGuid()),
        PlayerInfo = new RocketLeaguePlayerInfo(userAccount),
        CreationTime = DateTime.Now
    };
    public void SetReady() 
        => IsReady = true;

}
