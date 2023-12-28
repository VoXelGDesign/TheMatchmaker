using Application.Interfaces;
using MediatR;
using Contracts.QueueContracts;
using System.Security.Claims;
using Application.Exceptions.CustomExceptions;

namespace Application.Queue;

public record QueueRequestCommand() : IRequest;

internal class SendQueueRequest : IRequestHandler<QueueRequestCommand>
{

    private readonly IQueueRequestPublisher _publisher;
    private readonly ClaimsPrincipal _user;

    public SendQueueRequest(IQueueRequestPublisher publisher, ClaimsPrincipal user)
    {
        _publisher = publisher;
        _user = user;
    }

    public async Task Handle(QueueRequestCommand request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null) 
            throw new IdClaimNotFoundException();

        await _publisher.PublishAsync(new QueueRequest(claimidentity));
    }
}

