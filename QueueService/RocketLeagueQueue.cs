using Contracts.Common;
using Contracts.QueueContracts;
using Contracts.QueueContracts.RocketLeague;
using Contracts.QueueContracts.RocketLeague.Ranks;
using QueueService.Publishers.CreateRocketLeagueLobby;
using QueueService.Publishers.JoinedQueue;
using QueueService.Publishers.RemovedFromQueue;


namespace QueueService;

public class RocketLeagueQueue : BackgroundService
{
    private List<QueueRocketLeagueLobbyRequest> QueueRequests = new();   
    private readonly ICreateRocketLeagueLobbyPublisher _createRocketLeagueLobbyPublisher;
    private readonly IRemovedFromQueuePublisher _removedFromQueuePublisher;
    private readonly IJoinedQueuePublisher _joinedQueuePublisher;

    public RocketLeagueQueue(
        ICreateRocketLeagueLobbyPublisher lobbyPublisher, 
        IRemovedFromQueuePublisher removedPublisher,
        IJoinedQueuePublisher joinedQueuePublisher
        )
    {
        _createRocketLeagueLobbyPublisher = lobbyPublisher;
        _removedFromQueuePublisher = removedPublisher;
        _joinedQueuePublisher = joinedQueuePublisher;
    }

    public async Task AddToQueue(QueueRocketLeagueLobbyRequest request)
    {
        if (QueueRequests.Select(x => x.UserId).Contains(request.UserId))
            return;

        QueueRequests.Add(request);

        await _joinedQueuePublisher
            .PublishAsync(new UserJoinedQueue(request.UserId, DateTime.UtcNow));                
    }

    public async Task RemoveFromQueue(UserIdDto userId)
    {
        var requestToRemove = QueueRequests.FirstOrDefault(x => x.UserId == userId);

        if (requestToRemove is null)
            return;

        QueueRequests.Remove(requestToRemove);

        await _removedFromQueuePublisher
            .PublishAsync(new UserRemovedFromQueue(userId, DateTime.UtcNow));

    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(30, stoppingToken);
            Console.WriteLine(QueueRequests.Count);

            if (QueueRequests.Count < 1)
                continue;

            var checkedRequest = QueueRequests[0];
            QueueRequests.Remove(checkedRequest);

            if (checkedRequest.Mode != RocketLeagueQueueMode.TwoVSTwo && 
                checkedRequest.Mode != RocketLeagueQueueMode.ThreeVSThree)
                throw new ArgumentOutOfRangeException(nameof(checkedRequest) + "Mode: " + checkedRequest.Mode);

            if (DateTime.UtcNow >= checkedRequest.DateTime.AddMinutes(RequestLifetime.LifetimeMinutes) )
            {
                await _removedFromQueuePublisher
                    .PublishAsync(new UserRemovedFromQueue(checkedRequest.UserId, DateTime.UtcNow));
                continue;
            }
                

            var foundMatches = FindMatches(checkedRequest);

            var matchedRequests = new List<QueueRocketLeagueLobbyRequest>();


            if (Is2VS2WithEnoughMatches(foundMatches.Count(), checkedRequest.Mode))
            {
                var firstMatch = foundMatches.ElementAt(0);

                matchedRequests.Add(checkedRequest);
                matchedRequests.Add(firstMatch);

                QueueRequests.Remove(firstMatch);

                await NotifyMachedUsers(matchedRequests);
                continue;
            }


            if (Is3VS3WithEnoughMatches(foundMatches.Count(), checkedRequest.Mode))
            {
                var firstMatch = foundMatches.ElementAt(0);
                var secondMatch = foundMatches.ElementAt(1);

                matchedRequests.Add(checkedRequest);
                matchedRequests.Add(firstMatch);
                matchedRequests.Add(secondMatch);

                QueueRequests.Remove(firstMatch);
                QueueRequests.Remove(secondMatch);

                await NotifyMachedUsers(matchedRequests);
                continue;
            }

                QueueRequests.Add(checkedRequest);

        }

    }
    private IEnumerable<QueueRocketLeagueLobbyRequest> FindMatches(QueueRocketLeagueLobbyRequest checkedRequest)
            => QueueRequests.Where(x =>
                    x.Mode == checkedRequest.Mode &&
                    x.Region == checkedRequest.Region &&
                    x.Platform == checkedRequest.Platform &&
                    IsRankWithinBounds(checkedRequest.UserRank, x.LowerBoundRank, x.UpperBoundRank) &&
                    IsRankWithinBounds(x.UserRank, checkedRequest.LowerBoundRank, checkedRequest.UpperBoundRank)
                );

    private async Task NotifyMachedUsers(List<QueueRocketLeagueLobbyRequest> matchedRequests)
    {
        List<UserIdDto> userIds = matchedRequests
            .Select(x => x.UserId)
            .ToList();

        await _createRocketLeagueLobbyPublisher
            .PublishAsync(new CreateRocketLeagueLobbyRequest(userIds, DateTime.UtcNow));
    }

    private bool IsRankWithinBounds(
        QueueRocketLeagueRank checkedRank,
        QueueRocketLeagueRank lowerBound,
        QueueRocketLeagueRank upperBound)
    {
        if(!IsRankNameAboveOrEqual(checkedRank,lowerBound))
            return false;

        if (!IsRankNameBelowOrEqual(checkedRank, upperBound))
            return false;

        if (!IsNumberAndDivisionAboveOrEqual(checkedRank, lowerBound))
            return false;

        if(!IsNumberAndDivisioBelowOrEqual(checkedRank, upperBound))
            return false;

        return true;
    }

    private bool IsRankNameAboveOrEqual(
        QueueRocketLeagueRank checkedRank,
        QueueRocketLeagueRank lowerBound)
        => checkedRank.RocketLeagueRankName >= lowerBound.RocketLeagueRankName;

    private bool IsRankNameBelowOrEqual(
        QueueRocketLeagueRank checkedRank,
        QueueRocketLeagueRank upperBound)
        => checkedRank.RocketLeagueRankName <= upperBound.RocketLeagueRankName;

    private bool IsNumberAndDivisionAboveOrEqual(
        QueueRocketLeagueRank checkedRank,
        QueueRocketLeagueRank lowerBound)
        => checkedRank.RocketLeagueRankNumber >= lowerBound.RocketLeagueRankNumber &&
           checkedRank.RocketLeagueDivision >= lowerBound.RocketLeagueDivision;

    private bool IsNumberAndDivisioBelowOrEqual(
        QueueRocketLeagueRank checkedRank,
        QueueRocketLeagueRank upperBound)
        => checkedRank.RocketLeagueRankNumber <= upperBound.RocketLeagueRankNumber &&
           checkedRank.RocketLeagueDivision <= upperBound.RocketLeagueDivision;

    private bool Is2VS2WithEnoughMatches(int numberOfMatches, RocketLeagueQueueMode mode)
        => numberOfMatches >= 1 && mode == RocketLeagueQueueMode.TwoVSTwo;

    private bool Is3VS3WithEnoughMatches(int numberOfMatches, RocketLeagueQueueMode mode)
        => numberOfMatches >= 2 && mode == RocketLeagueQueueMode.ThreeVSThree;
}

