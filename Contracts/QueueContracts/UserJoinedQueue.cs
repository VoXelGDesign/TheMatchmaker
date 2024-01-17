using Contracts.Common;

namespace Contracts.QueueContracts;

public record UserJoinedQueue(UserIdDto UserIdDto, DateTime TimeStamp);
