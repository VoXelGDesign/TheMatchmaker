using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Client.Notifications
{
    public class NotificationManager : INotificationManager
    {

        private const string _connectionString = "https://localhost:7276/notyfications";
        
        private HubConnection _hubConnection;

        private readonly ISnackbar _snackbar;

        public bool IsConnected(HubConnectionState state)
            => state == HubConnectionState.Connected;

        public NotificationManager(ISnackbar snackbar)
        {
            _snackbar = snackbar;

            _hubConnection = new HubConnectionBuilder()
            .WithUrl(_connectionString)
            .Build();

            RegisterMethods();

        }
        public async Task ConnectToNotificationService()
        {
            await _hubConnection.StartAsync();
        } 

        public async Task SubscribeToNotificationService(UserIdDto userId)
        {
            if (!IsConnected(_hubConnection.State)) return;

            await _hubConnection.InvokeAsync("RegisterClient", userId);

        }

        public void RegisterMethods()
        {

            _hubConnection.On("RecieveNotification", () =>
            {
                _snackbar.Add("Recieved notyfication!", MudBlazor.Severity.Info);
            });
        }
    }

    public record UserIdDto(string UserId);
}
