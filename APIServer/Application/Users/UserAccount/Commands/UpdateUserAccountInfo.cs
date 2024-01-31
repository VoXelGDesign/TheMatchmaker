using Application.Exceptions.CustomExceptions;
using Domain.Users.UserAccounts;
using Domain.Users.User;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Contracts.ApiContracts.UserAccountInfo.Requests;
using Contracts.ApiContracts.UserAccountInfo.Responses;

namespace Application.Users.UserAccount.Commands;



public record UpdateUserAccountInfoCommand(UpdateUserAccountInfoRequest dto) : IRequest<UpdateUserAccountInfoResponse>;

public class UpdateUserAccountInfoHandler : IRequestHandler<UpdateUserAccountInfoCommand, UpdateUserAccountInfoResponse>
{

    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;

    public UpdateUserAccountInfoHandler(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }

    public async Task<UpdateUserAccountInfoResponse> Handle(UpdateUserAccountInfoCommand request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null) throw new IdClaimNotFoundException();
        
        var userAccountId = new UserId(Guid.Parse(claimidentity));

        var userAccountInfo = await _applicationDbContext.UserAccounts.SingleOrDefaultAsync(x => x.Id == userAccountId);

        var name = UserAccountName.Create(request.dto.Name);

        var link = UserAccountSteamProfileLink.Create(request.dto.SteamProfileLink);

        var discordName = UserDiscordName.Create(request.dto.DiscordName);

        var epicName = UserEpicName.Create(request.dto.EpicName);

        if (userAccountInfo == null)
        {
            userAccountInfo = Domain.Users.UserAccounts.UserAccount.Create(
                userAccountId,
                name ?? UserAccountName.Default(),
                link ?? UserAccountSteamProfileLink.Default(),
                discordName ?? UserDiscordName.Default(),
                epicName ?? UserEpicName.Default())
                ?? throw new ResourceCreationFailedException();

            _applicationDbContext.UserAccounts.Add(userAccountInfo);
            await _applicationDbContext.SaveChangesAsync();

            return new UpdateUserAccountInfoResponse(
                userAccountInfo.Name.Name, 
                userAccountInfo.SteamProfileLink.Link, 
                userAccountInfo.DiscordName.Name, 
                userAccountInfo.EpicName.Name);
        }

        userAccountInfo.UpdateUserAccount(name, link, discordName, epicName);

        await _applicationDbContext.SaveChangesAsync();

        return new UpdateUserAccountInfoResponse(
            userAccountInfo.Name.Name, 
            userAccountInfo.SteamProfileLink.Link, 
            userAccountInfo.DiscordName.Name, 
            userAccountInfo.EpicName.Name);
    }
}

