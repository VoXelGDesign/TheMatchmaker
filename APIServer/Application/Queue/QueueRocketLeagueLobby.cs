using Application.Exceptions.CustomExceptions;
using Contracts.QueueContracts.RocketLeague;
using Domain.Games.RocketLeague.Ranks;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using System.Security.Claims;
using Domain.Users.User;
using Infrastructure;
using MediatR;

namespace Application.Queue;

public record QueueRequestCommand(
    string Mode,
    RocketLeagueRankDto lowerBoundRank,
    RocketLeagueRankDto upperBoundRank) : IRequest;

internal class QueueRocketLeagueLobby : IRequestHandler<QueueRequestCommand>
{

    private readonly IQueueRequestPublisher _publisher;
    private readonly ClaimsPrincipal _user;
    private readonly ApplicationDbContext _dbContext;

    public QueueRocketLeagueLobby(IQueueRequestPublisher publisher, ClaimsPrincipal user, ApplicationDbContext dbContext)
    {
        _publisher = publisher;    
        _dbContext = dbContext;
        _user = user;
    }

    public async Task Handle(QueueRequestCommand request, CancellationToken cancellationToken)
    {
        Guid claimidentity;
        var id = _user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id is null)
            throw new IdClaimNotFoundException();

        claimidentity = Guid.Parse(id);
        var userId = new UserId(claimidentity);

        var queueRequest = new Contracts.QueueContracts.RocketLeague.QueueRocketLeagueLobby();

        var userRanks = await _dbContext.UserGameRanks.SingleOrDefaultAsync(x => x.UserId == userId);

        if (userRanks is null)
            throw new ResourceMissingException();

        RocketLeagueRank? userRank;

        switch (request.Mode) 
        {
            case "2VS2":

                userRank = userRanks.RocketLeague2vs2Rank;
                break;
            case "3VS3":
                userRank = userRanks.RocketLeague3vs3Rank;
                break;
            default:
                throw new NotImplementedException();
        }

        if (userRank is null)
            throw new ResourceMissingException();

        queueRequest.Mode = request.Mode;

        queueRequest.UserRank = userRank;

        var lowerBound = RocketLeagueRank.Create(
            request.lowerBoundRank.Name,
            request.lowerBoundRank.Number,
            request.lowerBoundRank.Name);

        var upperBound = RocketLeagueRank.Create(
            request.upperBoundRank.Name,
            request.upperBoundRank.Number,
            request.upperBoundRank.Name);

        if (lowerBound is null || upperBound is null)
            throw new ResourceMissingException();

        queueRequest.LowerBoundRank = lowerBound;
        queueRequest.UpperBoundRank = upperBound;

        await _publisher.PublishAsync(queueRequest);
    }
}

