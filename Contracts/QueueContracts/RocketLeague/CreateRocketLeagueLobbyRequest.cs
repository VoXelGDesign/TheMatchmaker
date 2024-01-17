using Contracts.Common;

namespace Contracts.QueueContracts.RocketLeague;

public record CreateRocketLeagueLobbyRequest(List<UserIdDto> userIds, DateTime TimeStamp);
