using Contracts.QueueContracts.RocketLeague.Ranks;
using Application.Exceptions.CustomExceptions;
using Contracts.QueueContracts.RocketLeague;
using Domain.Games.RocketLeague.Ranks;
using Microsoft.EntityFrameworkCore;
using Domain.Users.UserQueueInfos;
using Infrastructure.Publishers;
using Contracts.QueueContracts;
using System.Security.Claims;
using Domain.Users.User;
using Contracts.Common;
using Infrastructure;
using MediatR;


namespace Application.Queue;

public record QueueRequestCommand(
    string Mode,
    RocketLeagueRankDto lowerBoundRank,
    RocketLeagueRankDto upperBoundRank,
    QueueRegion Region) : IRequest;

public class QueueRocketLeagueLobby : IRequestHandler<QueueRequestCommand>
{

    private readonly IQueueRocketLeagueLobbyRequestPublisher _publisher;
    private readonly ClaimsPrincipal _user;
    private readonly ApplicationDbContext _dbContext;

    public QueueRocketLeagueLobby(IQueueRocketLeagueLobbyRequestPublisher publisher, ClaimsPrincipal user, ApplicationDbContext dbContext)
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

        var queueRequest = new QueueRocketLeagueLobbyRequestDto();

        var userRanks = await _dbContext.UserGameRanks.SingleOrDefaultAsync(x => x.UserId == userId);

        var queueInfo = await _dbContext.UserQueueInfos.SingleOrDefaultAsync(x => x.UserId == userId);

        if(queueInfo is null)
        {
            queueInfo = new UserQueueInfo(userId);
            _dbContext.UserQueueInfos.Add(queueInfo);
            _dbContext.SaveChanges();
        }
            

        var mode = (RocketLeagueQueueMode)Enum.Parse(typeof(RocketLeagueQueueMode), request.Mode);
        

        if (userRanks is null || queueInfo is null)
            throw new ResourceMissingException();

        RocketLeagueRank? userRank;

        switch (mode) 
        {
            case RocketLeagueQueueMode.TwoVSTwo:

                userRank = userRanks.RocketLeague2vs2Rank;
                break;
            case RocketLeagueQueueMode.ThreeVSThree:
                userRank = userRanks.RocketLeague3vs3Rank;
                break;
            default:
                throw new ResourceCreationFailedException();
        }

        if (userRank is null)
            throw new ResourceMissingException();

        if (queueInfo.Status != UserQueueStatus.NotInQueue)
            throw new ResourceCreationFailedException();

        queueRequest.UserId = new UserIdDto(id);

        queueRequest.Mode = mode;

        queueRequest.UserRank = new QueueRocketLeagueRankDto(
            userRank.RocketLeagueRankName.ToString(),
            userRank.RocketLeagueRankNumber.ToString(),
            userRank.RocketLeagueDivision.ToString()
            ) ?? throw new ResourceCreationFailedException();

        var lowerBound = new QueueRocketLeagueRankDto(
            request.lowerBoundRank.Name,
            request.lowerBoundRank.Number,
            request.lowerBoundRank.Division);

        var upperBound = new QueueRocketLeagueRankDto(
            request.upperBoundRank.Name,
            request.upperBoundRank.Number,
            request.upperBoundRank.Division);

        if (lowerBound is null || upperBound is null)
            throw new ResourceMissingException();

        queueRequest.LowerBoundRank = lowerBound;
        queueRequest.UpperBoundRank = upperBound;

        queueRequest.Region = request.Region;

        await _publisher.PublishAsync(queueRequest);
    }
}

