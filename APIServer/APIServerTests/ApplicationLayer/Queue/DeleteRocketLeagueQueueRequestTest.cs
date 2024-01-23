using Application.Queue;
using Contracts.Common;
using Contracts.QueueContracts.RocketLeague;
using Infrastructure.Publishers;
using System.Security.Claims;
using Application.Exceptions.CustomExceptions;
using Domain.Users.UserQueueInfos;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Domain.Users.User;
using System.Reflection.Metadata;
namespace APIServerTests.ApplicationLayer.Queue;

public class DeleteRocketLeagueQueueRequestTest
{
    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) }));

    public static DbContextOptions<ApplicationDbContext> Options()
        => new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

    [Fact]
    public async Task Handle_WhenUserIsInQueue_ShouldPublishDeleteRocketLeagueQueueRequestRequest()
    {

        var publisherMock = new Mock<IDeleteRocketLeagueQueueRequestPublisher>();
        publisherMock.Setup(x => x.PublishAsync(It.IsAny<DeleteRocketLeagueQueueRequestRequest>()));
        var command = new LeaveRocketLeagueQueueRequestCommand();

        using (var contextMock = new ApplicationDbContext(Options()))
        {

            var userId = new UserId(Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)));
            var userQueueInfo = new UserQueueInfo(userId);
            userQueueInfo.SetStatusInQueue(DateTime.UtcNow);
            contextMock.UserQueueInfos.Add(userQueueInfo);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            var deleteRocketLeagueQueueRequest = new DeleteRocketLeagueQueueRequest(claimsPrincipal, publisherMock.Object, contextMock);
            await deleteRocketLeagueQueueRequest.Handle(command, CancellationToken.None);
        }

        publisherMock.Verify(x => x.PublishAsync(It.IsAny<DeleteRocketLeagueQueueRequestRequest>()));

    }

    [Fact]
    public async Task Handle_WhenUserIsNotInQueue_ShouldThrowResourceCreationFailedException()
    {

        var publisherMock = new Mock<IDeleteRocketLeagueQueueRequestPublisher>();
        publisherMock.Setup(x => x.PublishAsync(It.IsAny<DeleteRocketLeagueQueueRequestRequest>()));
        var command = new LeaveRocketLeagueQueueRequestCommand();

        using (var contextMock = new ApplicationDbContext(Options()))
        {
            var deleteRocketLeagueQueueRequest = new DeleteRocketLeagueQueueRequest(claimsPrincipal, publisherMock.Object, contextMock);
            await Assert.ThrowsAsync<ResourceCreationFailedException>(() => deleteRocketLeagueQueueRequest.Handle(command, CancellationToken.None));
        }




    }
}
