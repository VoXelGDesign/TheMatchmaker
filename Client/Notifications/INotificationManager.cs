using Contracts.Common;

namespace Client.Notifications
{
    public interface INotificationManager
    {
        public Task ConnectToNotificationService();
        public Task SubscribeToNotificationService(UserIdDto userId);

        public Task DisconnectFromNotificationService();
        public bool IsConnected();
    }
}
