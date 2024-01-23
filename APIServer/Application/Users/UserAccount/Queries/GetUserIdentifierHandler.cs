using Application.Exceptions.CustomExceptions;
using MediatR;
using System.Security.Claims;
using Contracts.Common;
namespace Application.Users.UserAccount.Queries;


public record GetUserIdentifierQuery() : IRequest<UserIdDto>;

internal class GetUserIdentifierHandler : IRequestHandler<GetUserIdentifierQuery, UserIdDto>
{
    private readonly ClaimsPrincipal _user;
    public GetUserIdentifierHandler(ClaimsPrincipal user)
    {
        _user = user;
    }

    public async Task<UserIdDto> Handle(GetUserIdentifierQuery request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null)
            throw new IdClaimNotFoundException();

        return new UserIdDto(claimidentity);
    }
}

