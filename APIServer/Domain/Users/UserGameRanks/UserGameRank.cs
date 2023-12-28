using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;

namespace Domain.Users.UserGamesRanks;
public sealed class UserGameRank
{
    public UserId UserId { get; private set; } = null!;

    public RocketLeagueRank? RocketLeague1vs1Rank { get; private set; }
    public RocketLeagueRank? RocketLeague2vs2Rank { get; private set; }
    public RocketLeagueRank? RocketLeague3vs3Rank { get; private set; }

    public static UserGameRank Create(
        UserId userId,
        RocketLeagueRank? rocketLeague1vs1Rank = null,
        RocketLeagueRank? rocketLeague2vs2Rank = null,
        RocketLeagueRank? rocketLeague3vs3Rank = null
        ) =>
        new UserGameRank
        {
            UserId = userId,
            RocketLeague1vs1Rank = rocketLeague1vs1Rank,
            RocketLeague2vs2Rank = rocketLeague2vs2Rank,
            RocketLeague3vs3Rank = rocketLeague3vs3Rank
        };

    public void UpdateRocketLeague1vs1Rank(RocketLeagueRank rocketLeagueRank)
        => RocketLeague1vs1Rank = rocketLeagueRank;

    public void UpdateRocketLeague2vs2Rank(RocketLeagueRank rocketLeagueRank)
        => RocketLeague2vs2Rank = rocketLeagueRank;

    public void UpdateRocketLeague3vs3Rank(RocketLeagueRank rocketLeagueRank)
        => RocketLeague3vs3Rank = rocketLeagueRank;
}

