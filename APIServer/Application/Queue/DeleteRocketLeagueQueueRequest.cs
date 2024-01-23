using Application.Exceptions.CustomExceptions;
using Contracts.Common;
using Contracts.QueueContracts.RocketLeague;
using Domain.Users.User;
using Domain.Users.UserQueueInfos;
using Infrastructure;
using Infrastructure.Publishers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Queue;

public record LeaveRocketLeagueQueueRequestCommand() : IRequest;
public class DeleteRocketLeagueQueueRequest : IRequestHandler<LeaveRocketLeagueQueueRequestCommand>
{
    private readonly ClaimsPrincipal _user;
    private readonly IDeleteRocketLeagueQueueRequestPublisher _publisher;
    private readonly ApplicationDbContext _applicationDbContext;
    public DeleteRocketLeagueQueueRequest(ClaimsPrincipal user, IDeleteRocketLeagueQueueRequestPublisher publisher, ApplicationDbContext applicationDbContext)
    {
        _user = user;
        _publisher = publisher;
        _applicationDbContext = applicationDbContext;
    }
    public async Task Handle(LeaveRocketLeagueQueueRequestCommand request, CancellationToken cancellationToken)
    {
        Guid claimidentity;
        var id = _user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id is null)
            throw new IdClaimNotFoundException();

        claimidentity = Guid.Parse(id);
        var userId = new UserId(claimidentity);

        var queueInfo = await _applicationDbContext.UserQueueInfos.SingleOrDefaultAsync(x => x.UserId == userId);

        if (queueInfo is null)
        {
            queueInfo = new UserQueueInfo(userId);
            _applicationDbContext.UserQueueInfos.Add(queueInfo);
            _applicationDbContext.SaveChanges();
        }

        if (queueInfo.Status != UserQueueStatus.InQueue)
            throw new ResourceCreationFailedException();
       
        var userIdDto = new UserIdDto(id);

        await _publisher.PublishAsync(new DeleteRocketLeagueQueueRequestRequest(userIdDto));
    }
}
