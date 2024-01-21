using Contracts.ApiContracts.Queue.Requests;
using Contracts.ApiContracts.Queue.Responses;
using Contracts.ApiContracts.UserAccountInfo.Responses;
using Contracts.QueueContracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace Client.Queue;

public class QueueManager : IQueueManager
{
    private readonly HttpClient _httpClient;
    private readonly ISnackbar _snackbar;
    private readonly NavigationManager _navigationManager;
    private QueueStatus _status = QueueStatus.LeftQueue;    
    public DateTime? JoinedQueueDate { get; private set;}

    public QueueStatus queueStatus 
        => _status;

    public QueueManager(IHttpClientFactory httpClientFactory, ISnackbar snackbar, NavigationManager navigationManager) 
    {
        _httpClient = httpClientFactory.CreateClient("Auth");
        _snackbar = snackbar;
        _navigationManager = navigationManager;
    }

    public async Task<bool> JoinQueue(QueueRocketLeagueRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Queue/SendRequest", request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task UpdateQueueStatus()
    {        
        var result = await _httpClient.GetAsync("api/Queue/Info");
        var queueInfo = await result.Content.ReadFromJsonAsync<UserQueueInfoStatus>();

        if (queueInfo is null) 
            return;

        var status = (QueueStatus)Enum.Parse(typeof(QueueStatus), queueInfo.QueueStatus);

        if (_status == status)
            return;

        JoinedQueueDate = null;
        _status = status;      

        switch (_status)
        {
            case QueueStatus.LeftQueue:
                _navigationManager.NavigateTo("queue-page");
                break;
            case QueueStatus.JoinedQueue:
                JoinedQueueDate = queueInfo.ChangeTime;
                _navigationManager.NavigateTo("queue-waiting");
                break;
            case QueueStatus.JoinedLobby:
                _navigationManager.NavigateTo("lobby");
                break;
        }

        
    }

    

    
    
}
