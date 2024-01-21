using Contracts.ApiContracts.Queue.Requests;
using Contracts.QueueContracts;

namespace Client.Queue;

public interface IQueueManager
{
    public QueueStatus queueStatus { get; }
    public DateTime? JoinedQueueDate { get;}
    public Task UpdateQueueStatus();
    public Task<bool> JoinQueue(QueueRocketLeagueRequest request);
}
