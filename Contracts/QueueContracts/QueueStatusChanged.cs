using Contracts.Common;

namespace Contracts.QueueContracts;

public record QueueStatusChanged(UserIdDto UserIdDto, QueueStatus QueueStatus);
