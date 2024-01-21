using Application.Exceptions.CustomExceptions;
using Contracts.ApiContracts.Queue.Responses;
using Domain.Users.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Domain.Users.UserQueueInfos;
using Contracts.QueueContracts;

namespace Application.Users.UserQueueInfos.Queries;

public record GetUserQueueInfoQuery() : IRequest<UserQueueInfoStatus>;
internal class GetUserQueueInfo : IRequestHandler<GetUserQueueInfoQuery, UserQueueInfoStatus>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;
    public GetUserQueueInfo(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }
    public async Task<UserQueueInfoStatus> Handle(GetUserQueueInfoQuery request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null)
        {
            throw new IdClaimNotFoundException();
        }

        var userAccountId = new UserId(Guid.Parse(claimidentity));

        var userQueueInfo = await _applicationDbContext.UserQueueInfos.SingleOrDefaultAsync(x => x.UserId == userAccountId);

        if (userQueueInfo == null)
        {
            userQueueInfo = new UserQueueInfo(userAccountId);
            await _applicationDbContext.UserQueueInfos.AddAsync(userQueueInfo);
            await _applicationDbContext.SaveChangesAsync();
        }
       
        QueueStatus status = QueueStatus.LeftQueue;

        switch (userQueueInfo.Status)
        {
            case UserQueueStatus.NotInQueue:
                status = QueueStatus.LeftQueue;
                break;
            case UserQueueStatus.InQueue:
                status = QueueStatus.JoinedQueue; 
                break;
            case UserQueueStatus.InLobby: 
                status = QueueStatus.JoinedLobby;
                break;
            default:
                throw new ResourceMissingException();
        }

        return new UserQueueInfoStatus(status.ToString(), userQueueInfo.LastChangeDate);
    }
}
    
