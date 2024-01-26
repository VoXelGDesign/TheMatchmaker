using Contracts.Common;
using Contracts.QueueContracts.RocketLeague.Ranks;
using System.Reflection.Metadata.Ecma335;

namespace Contracts.QueueContracts.RocketLeague;
public record QueueRocketLeagueLobbyRequest
{
    public UserIdDto UserId { get; set; }
    public RocketLeagueQueueMode Mode { get; set; }
    public QueueRocketLeagueRank UserRank { get; set; }
    public QueueRocketLeagueRank LowerBoundRank { get; set; }
    public QueueRocketLeagueRank UpperBoundRank { get; set; }
    public QueueRegion Region { get; set; }
    public RocketLeaguePlatform Platform { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow;

    public static QueueRocketLeagueLobbyRequest? CreateFromDto(QueueRocketLeagueLobbyRequestDto dto)
    {
        var lowerBound = QueueRocketLeagueRank.Create(dto.LowerBoundRank);
        var upperBound = QueueRocketLeagueRank.Create(dto.UpperBoundRank);
        var userRank = QueueRocketLeagueRank.Create(dto.UserRank);      

        if (lowerBound is null || upperBound is null || userRank is null) 
            return null;

        return new()
        {
            UserId = dto.UserId,
            Mode = dto.Mode,
            LowerBoundRank = lowerBound,
            UpperBoundRank = upperBound,
            UserRank = userRank,
            DateTime = dto.DateTime,
            Region = dto.Region,
            Platform = dto.Platform
        };
    }
}
