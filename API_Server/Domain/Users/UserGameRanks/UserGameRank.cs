using Domain.Games.RocketLeague.Ranks;
using Domain.Users.User;

namespace Domain.Users.UserGamesRanks;
public sealed class UserGameRank
{
    public UserId UserId { get; private set; } = null!;

    public RocketLeagueRank? RocketLeagueRank { get; private set; }

    public static UserGameRank Create(UserId userId, RocketLeagueRank rocketLeagueRank)
        => new UserGameRank
        {
            UserId = userId,
            RocketLeagueRank = rocketLeagueRank
        };

    public void UpdateRocketLeagueRank(RocketLeagueRank rocketLeagueRank)
        => RocketLeagueRank = rocketLeagueRank;
}

