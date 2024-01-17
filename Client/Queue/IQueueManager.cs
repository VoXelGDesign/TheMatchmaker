using Contracts.ApiContracts.Queue.Requests;
using Contracts.QueueContracts;

namespace Client.Queue;

public interface IQueueManager
{
    public QueueStatus queueStatus { get; }

    public void SetStatusRemovedFromQueue();
    public void SetStatusJoinedQueue();
    public void SetStatusJoinedLobby();

    public int TimerSeconds { get; }
    public Delegate? StateHasChangedDelegate { get; set; }
    public int Minutes { get; }
    public void SetQueueStatus(QueueStatus queueStatus);
    public Task<bool> JoinQueue(QueueRocketLeagueRequest request);
}
