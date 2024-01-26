using Application.Exceptions.CustomExceptions;
using Application.Users.UserGameRanks.RocketLeague.Commands;
using Application.Users.UserGameRanks.RocketLeague.Queries;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Requests;
using Domain.Games.RocketLeague.Ranks;
using Domain.Users.UserGamesRanks;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Users.UserGameRanks;

public class GetRocketLeagueRankTest
{

    private TestUtils _utils = new TestUtils();

    [Fact]
    public async Task Handler_ShouldThrowIdClaimNotFoundException_WhenClaimIdentityIsNull()
    {
        var userGameRank = UserGameRank.Create(_utils.UserIdFromClaimsPrincipal());

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserGameRanks.Add(userGameRank);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var getRocketLeagueRank =
                new GetRocketLeagueRank(contextMock, new ClaimsPrincipal());

            await Assert.ThrowsAsync<IdClaimNotFoundException>(
                async () => await getRocketLeagueRank.Handle(new GetUserRocketLeagueRankCommand(), CancellationToken.None));
        }
    }
    [Fact]
    public async Task Handler_ShouldGetRocketLeagueRank_SuccessfullyFor2VS2()
    {
        var rocketLeagueRank = RocketLeagueRank
            .Create("CHAMPION", "II", "I");

        var userGameRank = UserGameRank
            .Create(_utils.UserIdFromClaimsPrincipal(),rocketLeague2vs2Rank: rocketLeagueRank);

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserGameRanks.Add(userGameRank);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var getRocketLeagueRank =
                new GetRocketLeagueRank(contextMock, _utils.ClaimsPrincipal);

           var response = await getRocketLeagueRank
                .Handle(new GetUserRocketLeagueRankCommand(), CancellationToken.None);

            Assert.Equal("CHAMPION", response.Rank2vs2?.Name);
            Assert.Equal("II", response.Rank2vs2?.Number);
            Assert.Equal("I", response.Rank2vs2?.Division);
        }
    }

    [Fact]
    public async Task Handler_ShouldGetRocketLeagueRank_SuccessfullyFor3VS3()
    {
        var rocketLeagueRank = RocketLeagueRank
            .Create("CHAMPION", "II", "I");

        var userGameRank = UserGameRank
            .Create(_utils.UserIdFromClaimsPrincipal(), rocketLeague3vs3Rank: rocketLeagueRank);

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserGameRanks.Add(userGameRank);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var getRocketLeagueRank =
                new GetRocketLeagueRank(contextMock, _utils.ClaimsPrincipal);

            var response = await getRocketLeagueRank
                 .Handle(new GetUserRocketLeagueRankCommand(), CancellationToken.None);

            Assert.Equal("CHAMPION", response.Rank3vs3?.Name);
            Assert.Equal("II", response.Rank3vs3?.Number);
            Assert.Equal("I", response.Rank3vs3?.Division);
        }
    }
}
