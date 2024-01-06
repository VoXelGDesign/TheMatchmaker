using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;

namespace Domain.Users.UserGamesRanks;
public sealed class UserGameRank
{
    public UserId UserId { get; private set; } = null!;
    public RocketLeagueRank? RocketLeague2vs2Rank { get; private set; }
    public RocketLeagueRank? RocketLeague3vs3Rank { get; private set; }

    public static UserGameRank Create(
        UserId userId,
        RocketLeagueRank? rocketLeague2vs2Rank = null,
        RocketLeagueRank? rocketLeague3vs3Rank = null
        ) =>
        new UserGameRank
        {
            UserId = userId,
            RocketLeague2vs2Rank = rocketLeague2vs2Rank,
            RocketLeague3vs3Rank = rocketLeague3vs3Rank
        };

    public void UpdateRocketLeagueRank(RocketLeagueRank rocketLeagueRank, string mode)
    {
        if (mode == "2VS2")
            RocketLeague2vs2Rank = rocketLeagueRank;

        if (mode == "3VS3")
            RocketLeague3vs3Rank = rocketLeagueRank;
    }
}

