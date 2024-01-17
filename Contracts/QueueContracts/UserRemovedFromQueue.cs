using Contracts.Common;

namespace Contracts.QueueContracts;

public record UserRemovedFromQueue(UserIdDto UserIdDto, DateTime TimeStamp);

