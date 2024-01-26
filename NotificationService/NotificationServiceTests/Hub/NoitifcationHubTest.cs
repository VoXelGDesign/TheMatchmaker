
using Contracts.Common;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NotyficationService;

namespace NotificationServiceTests.Hub;

public class NoitifcationHubTest
{

    [Fact]
    public void RegisterClient_Adds_Client_To_RegisteredClients()
    {
        var userId = new UserIdDto("testUserId");
        var connectionId = new ConnectionId("testConnectionId");

        var hub = new NotificationHub();
        var clients = new Mock<IHubCallerClients<INotificationClient>>();
        var context = new Mock<HubCallerContext>();
        var signals = new Mock<INotificationClient>();
        hub.Clients = clients.Object;
        hub.Context = context.Object;


        clients.Setup(hub => hub.Client(It.IsAny<string>())).Returns(signals.Object);
        signals.Setup(hub => hub.RecieveLeftQueueNotification()).Verifiable();
        context.Setup(context => context.ConnectionId).Returns(connectionId.value);


        hub.RegisterClient(userId.UserId);

        Assert.Single(hub.RegisteredClients);
    }

    [Fact]
    public void NotifyUserJoinedLobby_Sends_Notification_To_Client()
    {
        var userId = new UserIdDto("testUserId");
        var connectionId = new ConnectionId("testConnectionId");

        var hub = new NotificationHub();
        var clients = new Mock<IHubCallerClients<INotificationClient>>();
        var context = new Mock<HubCallerContext>();
        var signals = new Mock<INotificationClient>();
        hub.Clients = clients.Object;
        hub.Context = context.Object;


        clients.Setup(hub => hub.Client(It.IsAny<string>())).Returns(signals.Object);
        signals.Setup(hub => hub.RecieveLeftQueueNotification()).Verifiable();
        context.Setup(context => context.ConnectionId).Returns(connectionId.value);


        hub.RegisterClient(userId.UserId);
        hub.NotifyUserJoinedLobby(userId);

        signals.Verify(client => client.RecieveJoinedLobbyNotyfication(), Times.Once);
    }

    [Fact]
    public void NotifyUserJoinedQueue_Sends_Notification_To_Client()
    {
        var userId = new UserIdDto("testUserId");
        var connectionId = new ConnectionId("testConnectionId");

        var hub = new NotificationHub();
        var clients = new Mock<IHubCallerClients<INotificationClient>>();
        var context = new Mock<HubCallerContext>();
        var signals = new Mock<INotificationClient>();
        hub.Clients = clients.Object;
        hub.Context = context.Object;


        clients.Setup(hub => hub.Client(It.IsAny<string>())).Returns(signals.Object);
        signals.Setup(hub => hub.RecieveLeftQueueNotification()).Verifiable();
        context.Setup(context => context.ConnectionId).Returns(connectionId.value);


        hub.RegisterClient(userId.UserId);
        hub.NotifyUserJoinedQueue(userId);

        signals.Verify(client => client.RecieveJoinedQueueNotification(), Times.Once);
    }

    [Fact]
    public void NotifyUserLeftQueue_Sends_Notification_To_Client()
    {

        var userId = new UserIdDto("testUserId");
        var connectionId = new ConnectionId("testConnectionId");

        var hub = new NotificationHub();
        var clients = new Mock<IHubCallerClients<INotificationClient>>();
        var context = new Mock<HubCallerContext>();
        var signals = new Mock<INotificationClient>();
        hub.Clients = clients.Object;
        hub.Context = context.Object;


        clients.Setup(hub => hub.Client(It.IsAny<string>())).Returns(signals.Object);
        signals.Setup(hub => hub.RecieveLeftQueueNotification()).Verifiable();
        context.Setup(context => context.ConnectionId).Returns(connectionId.value);


        hub.RegisterClient(userId.UserId);
        hub.NotifyUserLeftQueue(userId);


        signals.Verify(client => client.RecieveLeftQueueNotification(), Times.Once);
    }

    [Fact]
    public async Task OnDisconnectedAsync_Removes_Client_From_RegisteredClients()
    {
        var userId = new UserIdDto("testUserId");
        var connectionId = new ConnectionId("testConnectionId");

        var hub = new NotificationHub();
        var clients = new Mock<IHubCallerClients<INotificationClient>>();
        var context = new Mock<HubCallerContext>();
        var signals = new Mock<INotificationClient>();
        hub.Clients = clients.Object;
        hub.Context = context.Object;


        clients.Setup(hub => hub.Client(It.IsAny<string>())).Returns(signals.Object);
        signals.Setup(hub => hub.RecieveLeftQueueNotification()).Verifiable();
        context.Setup(context => context.ConnectionId).Returns(connectionId.value);


        hub.RegisterClient(userId.UserId);
        await hub.OnDisconnectedAsync(new Exception());

        Assert.Empty(hub.RegisteredClients);
    }
}
