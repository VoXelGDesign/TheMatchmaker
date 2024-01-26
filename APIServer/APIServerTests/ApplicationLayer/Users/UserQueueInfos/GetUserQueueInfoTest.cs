using Application.Exceptions.CustomExceptions;
using Application.Users.UserGameRanks.RocketLeague.Queries;
using Application.Users.UserQueueInfos.Queries;
using Domain.Users.UserGamesRanks;
using Domain.Users.UserQueueInfos;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Users.UserQueueInfos;

public class GetUserQueueInfoTest
{
    private TestUtils _utils = new TestUtils();
    [Fact]
    public async Task Handler_ShouldThrowIdClaimNotFoundException_WhenClaimIdentityIsNull()
    {
        var userQueueInfo = new UserQueueInfo(_utils.UserIdFromClaimsPrincipal());

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserQueueInfos.Add(userQueueInfo);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var getRocketLeagueRank =
                new GetUserQueueInfo(contextMock, new ClaimsPrincipal());

            await Assert.ThrowsAsync<IdClaimNotFoundException>(
                async () => await getRocketLeagueRank.Handle(new GetUserQueueInfoQuery(), CancellationToken.None));
        }
    }

    [Fact]
    public async Task Handler_ReturnsCorrectQueueInfoResponse_WhenPresent()
    {
        var userQueueInfo = new UserQueueInfo(_utils.UserIdFromClaimsPrincipal());
        userQueueInfo.SetStatusInQueue(DateTime.UtcNow);

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserQueueInfos.Add(userQueueInfo);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var getQueueInfo =
                new GetUserQueueInfo(contextMock, _utils.ClaimsPrincipal);
            var response = await getQueueInfo.Handle(new GetUserQueueInfoQuery(), CancellationToken.None);

            Assert.Equal("JoinedQueue", response.QueueStatus);
        }

        using (var contextMock = _utils.dbContext())
        {
            userQueueInfo.SetStatusNotInQueue(DateTime.UtcNow.AddMinutes(5));
            contextMock.UserQueueInfos.Update(userQueueInfo);
            await contextMock.SaveChangesAsync();

        }

        using (var contextMock = _utils.dbContext())
        {           
            var getQueueInfo =
                new GetUserQueueInfo(contextMock, _utils.ClaimsPrincipal);
            var response = await getQueueInfo.Handle(new GetUserQueueInfoQuery(), CancellationToken.None);

            Assert.Equal("LeftQueue", response.QueueStatus);
        }

        using (var contextMock = _utils.dbContext())
        {
            userQueueInfo.SetStatusInLobby(DateTime.UtcNow.AddMinutes(10));
            contextMock.UserQueueInfos.Update(userQueueInfo);
            await contextMock.SaveChangesAsync();

        }

        using (var contextMock = _utils.dbContext())
        {
            var getQueueInfo =
                new GetUserQueueInfo(contextMock, _utils.ClaimsPrincipal);
            var response = await getQueueInfo.Handle(new GetUserQueueInfoQuery(), CancellationToken.None);

            Assert.Equal("JoinedLobby", response.QueueStatus);
        }
    }
}