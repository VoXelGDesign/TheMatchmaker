using Contracts.ApiContracts.Queue.Requests;
using Contracts.QueueContracts;

namespace Client.Queue;

public interface IQueueManager
{
    public QueueStatus queueStatus { get; }


    public int TimerSeconds { get; }
    public Delegate? StateHasChangedDelegate { get; set; }
    public int TimerMinutes { get; }
    public Task UpdateQueueStatus();
    public Task<bool> JoinQueue(QueueRocketLeagueRequest request);
}
