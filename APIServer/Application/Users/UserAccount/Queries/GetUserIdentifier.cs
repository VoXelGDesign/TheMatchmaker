using Application.Exceptions.CustomExceptions;
using MediatR;
using System.Security.Claims;

namespace Application.Users.UserAccount.Queries;

public record UserIdDto(string UserId);

public record GetUserIdentifierQuery() : IRequest<UserIdDto>;

internal class GetUserIdentifier : IRequestHandler<GetUserIdentifierQuery, UserIdDto>
{
    private readonly ClaimsPrincipal _user;
    public GetUserIdentifier(ClaimsPrincipal user)
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

