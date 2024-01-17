using Contracts.ApiContracts.Queue.Requests;
using Contracts.QueueContracts;
using MudBlazor;
using System.Net.Http.Json;

namespace Client.Queue;

public class QueueManager : IQueueManager
{
    private readonly HttpClient _httpClient;
    private readonly ISnackbar _snackbar;
    private QueueStatus _status = QueueStatus.LeftQueue;
    public int Minutes
    => TimerSeconds / 60;
    public int TimerSeconds { get; private set; } = RequestLifetime.LifetimeSeconds;
    public Delegate? StateHasChangedDelegate { get; set; }

    public QueueStatus queueStatus 
        => _status;

    public QueueManager(IHttpClientFactory httpClientFactory, ISnackbar snackbar)
    {
        _httpClient = httpClientFactory.CreateClient("Auth");
        _snackbar = snackbar;
    }

    public async Task<bool> JoinQueue(QueueRocketLeagueRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Queue/SendRequest", request);
        return response.IsSuccessStatusCode;
    }    

    private async void LunchTimer()
    {
        while (TimerSeconds > 0)
        {
            StateHasChangedDelegate?.DynamicInvoke();
            TimerSeconds -= 1;           
            await Task.Delay(1000);
        }
    }


    private void ResetTimer()
        => TimerSeconds = RequestLifetime.LifetimeSeconds;

    public void SetStatusRemovedFromQueue()
    {
        _status = QueueStatus.LeftQueue;
        ResetTimer();
    }

    public void SetStatusJoinedQueue()
    {
        _status = QueueStatus.JoinedQueue;
        LunchTimer();
    }

    public void SetStatusJoinedLobby()
    {
        _status = QueueStatus.JoinedLobby;
        ResetTimer();
    }

    public void SetQueueStatus(QueueStatus queueStatus)
    {
        _status = queueStatus;
    }
}
