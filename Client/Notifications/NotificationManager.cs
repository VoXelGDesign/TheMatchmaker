using Client.Queue;
using Contracts.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Client.Notifications
{
    public class NotificationManager : INotificationManager
    {

        private const string _connectionString = "https://localhost:7005/notyfications";

        private HubConnection _hubConnection;

        private readonly ISnackbar _snackbar;

        private readonly IQueueManager _queueManager;

        private readonly NavigationManager _navigationManager;

        public bool IsConnected()
            => _hubConnection.State == HubConnectionState.Connected;

        public NotificationManager(ISnackbar snackbar, IQueueManager queueManager, NavigationManager navigationManager)
        {
            _snackbar = snackbar;
            _queueManager = queueManager;
            _navigationManager = navigationManager;

            _hubConnection = new HubConnectionBuilder()
            .WithUrl(_connectionString)
            .Build();

            RegisterMethods();

        }
        public async Task ConnectToNotificationService()
        {
            await _hubConnection.StartAsync();
        }

        public async Task DisconnectFromNotificationService()
        {
            await _hubConnection.StopAsync();
        }

        public async Task SubscribeToNotificationService(UserIdDto userId)
        {
            if (!IsConnected()) return;

            await _hubConnection.InvokeAsync("RegisterClient", userId.UserId);

        }

        public void RegisterMethods()
        {
            _hubConnection.On("RecieveJoinedQueueNotification", () =>
            {
                _snackbar.Add("Joined Queue!", MudBlazor.Severity.Success);
                _queueManager.SetStatusJoinedQueue();
                _navigationManager.NavigateTo("queue-waiting");
            });

            _hubConnection.On("RecieveLeftQueueNotification", () =>
            {
                _snackbar.Add("Left The Queue!", MudBlazor.Severity.Error);
                _queueManager.SetStatusRemovedFromQueue();
                _navigationManager.NavigateTo("queue-page");
            });

            _snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;
            _hubConnection.On("RecieveJoinedLobbyNotyfication", () =>
            {
                _queueManager.SetStatusJoinedLobby();
                _snackbar.Add("Match Found!", MudBlazor.Severity.Success, config =>
                    {        
                        config.Action = "ACCEPT";
                        config.ActionColor = Color.Primary;
                        config.Onclick = snackbar =>
                        {
                            AcceptLobby();
                            return Task.CompletedTask;
                        };
                    });
            });

            
        }

        public void AcceptLobby()
        {
            _snackbar.Add("Not implemented");
        }
    }

}
