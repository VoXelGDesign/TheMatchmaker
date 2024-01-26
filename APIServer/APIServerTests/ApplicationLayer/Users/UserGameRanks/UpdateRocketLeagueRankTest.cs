using Application.Exceptions.CustomExceptions;
using Application.Users.UserGameRanks.RocketLeague.Commands;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Requests;
using Domain.Users.User;
using Domain.Users.UserGamesRanks;
using Infrastructure;
using System.Security.Claims;

namespace APIServerTests.ApplicationLayer.Users.UserGameRanks;

public class UpdateRocketLeagueRankTest
{

    private TestUtils _utils = new TestUtils();

    [Fact]
    public async Task Handler_ShouldThrowIdClaimNotFoundException_WhenClaimIdentityIsNull()
    {
        var userGameRank = UserGameRank.Create(_utils.UserIdFromClaimsPrincipal());

        var updateRocketLeagueRankCommand =
            new UpdateRocketLeagueRankCommand(
                new UpdateRocketLeagueRankRequest("3VS3", "DIAMOND", "II", "III"));

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserGameRanks.Add(userGameRank);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var updateRocketLeagueRank =
                new UpdateRocketLeagueRankHandler(contextMock, new ClaimsPrincipal());

            await Assert.ThrowsAsync<IdClaimNotFoundException>(
                async () => await updateRocketLeagueRank.Handle(updateRocketLeagueRankCommand, CancellationToken.None));
        }
    }

        [Fact]
    public async Task Handler_ShouldUpdateRocketLeagueRank_SuccessfullyFor2VS2()
    {
        var userGameRank = UserGameRank.Create(_utils.UserIdFromClaimsPrincipal());

        var updateRocketLeagueRankCommand = 
            new UpdateRocketLeagueRankCommand(
                new UpdateRocketLeagueRankRequest("2VS2","BRONZE","II","III"));

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserGameRanks.Add(userGameRank);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var updateRocketLeagueRank = new UpdateRocketLeagueRankHandler(contextMock, _utils.ClaimsPrincipal);
            var response = await updateRocketLeagueRank.Handle(updateRocketLeagueRankCommand, CancellationToken.None);

            Assert.Equal("BRONZE", response.Name);
            Assert.Equal("II", response.Number);
            Assert.Equal("III", response.Division);
        }
            
    }

    [Fact]
    public async Task Handler_ShouldUpdateRocketLeagueRank_SuccessfullyFor3VS3()
    {
        var userGameRank = UserGameRank.Create(_utils.UserIdFromClaimsPrincipal());

        var updateRocketLeagueRankCommand =
            new UpdateRocketLeagueRankCommand(
                new UpdateRocketLeagueRankRequest("3VS3", "DIAMOND", "II", "III"));

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserGameRanks.Add(userGameRank);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var updateRocketLeagueRank = new UpdateRocketLeagueRankHandler(contextMock, _utils.ClaimsPrincipal);
            var response = await updateRocketLeagueRank.Handle(updateRocketLeagueRankCommand, CancellationToken.None);

            Assert.Equal("DIAMOND", response.Name);
            Assert.Equal("II", response.Number);
            Assert.Equal("III", response.Division);
        }
    }

    [Fact]
    public async Task Handler_ShouldThrowResourceCreationFailedException_WhenRequestModeIsInvalid()
    {
        var userGameRank = UserGameRank.Create(_utils.UserIdFromClaimsPrincipal());

        var updateRocketLeagueRankCommand =
            new UpdateRocketLeagueRankCommand(
                new UpdateRocketLeagueRankRequest("4VS4", "DIAMOND", "II", "III"));

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserGameRanks.Add(userGameRank);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var updateRocketLeagueRank = 
                new UpdateRocketLeagueRankHandler(contextMock, _utils.ClaimsPrincipal);
            
            await Assert.ThrowsAsync<ResourceCreationFailedException>(
                async () => await updateRocketLeagueRank.Handle(updateRocketLeagueRankCommand, CancellationToken.None));
        }
    }

    [Fact]
    public async Task Handler_ShouldThrowResourceCreationFailedException_WhenRequestRankIsInvalid()
    {
        var userGameRank = UserGameRank.Create(_utils.UserIdFromClaimsPrincipal());

        var updateRocketLeagueRankCommand =
            new UpdateRocketLeagueRankCommand(
                new UpdateRocketLeagueRankRequest("2VS2", "SAPCEX", "II", "III"));

        using (var contextMock = _utils.dbContext())
        {
            contextMock.UserGameRanks.Add(userGameRank);
            await contextMock.SaveChangesAsync();
        }

        using (var contextMock = _utils.dbContext())
        {
            var updateRocketLeagueRank =
                new UpdateRocketLeagueRankHandler(contextMock, _utils.ClaimsPrincipal);

            await Assert.ThrowsAsync<ResourceCreationFailedException>(
                async () => await updateRocketLeagueRank.Handle(updateRocketLeagueRankCommand, CancellationToken.None));
        }
    }
}