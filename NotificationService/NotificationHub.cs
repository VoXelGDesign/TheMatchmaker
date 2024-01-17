using Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace NotyficationService
{

    public class NotificationHub : Hub<INotificationClient>
    {
        internal Dictionary<UserIdDto, ConnectionId> RegisteredClients { get; private set; } = new Dictionary<UserIdDto, ConnectionId>();

        public void RegisterClient(string idValue)
        {
            var userId = new UserIdDto(idValue);
            var connectionId = new ConnectionId(Context.ConnectionId);
            RegisteredClients.Add(userId, connectionId);
        }

        internal void NotifyUserJoinedLobby(UserIdDto userId)
        {
            var connectionId = RegisteredClients.GetValueOrDefault(userId);
            if (connectionId is not null)
                Clients.Client(connectionId.value).RecieveJoinedLobbyNotyfication();
        }

        internal void NotifyUserJoinedQueue(UserIdDto userId)
        {
            var connectionId = RegisteredClients.GetValueOrDefault(userId);
            if (connectionId is not null)
                Clients.Client(connectionId.value).RecieveJoinedQueueNotification();
        }

        internal void NotifyUserLeftQueue(UserIdDto userId)
        {
            var connectionId = RegisteredClients.GetValueOrDefault(userId);
            if (connectionId is not null)
                Clients.Client(connectionId.value).RecieveLeftQueueNotification();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = new ConnectionId(Context.ConnectionId);

            foreach (var conn in RegisteredClients)
                if (conn.Value == connectionId)
                    RegisteredClients.Remove(conn.Key);

            return base.OnDisconnectedAsync(exception);
        }
    }

    internal record ConnectionId(string value);


    public interface INotificationClient
    {
        public Task RecieveJoinedQueueNotification();
        public Task RecieveJoinedLobbyNotyfication();
        public Task RecieveLeftQueueNotification();
    }

}