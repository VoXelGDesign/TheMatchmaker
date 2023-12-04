﻿using Application.Exceptions.CustomExceptions;
using Domain.Users.UserAccounts;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Users.UserAccount.Commands;

public record UpdateUserAccountInfoDto(string? Name = null, string? SteamProfileLink = null);

public record UpdateUserAccountInfoCommand(UpdateUserAccountInfoDto dto) : IRequest<UpdateUserAccountInfoDto>;

public class UpdateUserAccountInfoHandler : IRequestHandler<UpdateUserAccountInfoCommand, UpdateUserAccountInfoDto>
{

    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ClaimsPrincipal _user;

    public UpdateUserAccountInfoHandler(ApplicationDbContext applicationDbContext, ClaimsPrincipal user)
    {
        _applicationDbContext = applicationDbContext;
        _user = user;
    }

    public async Task<UpdateUserAccountInfoDto> Handle(UpdateUserAccountInfoCommand request, CancellationToken cancellationToken)
    {
        var claimidentity = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimidentity == null) throw new IdClaimNotFound();
        
        var userAccountId = new UserAccountId(Guid.Parse(claimidentity));

        var userAccountInfo = await _applicationDbContext.UserAccounts.FirstOrDefaultAsync(x => x.Id == userAccountId);

        var name = UserAccountName.Create(request.dto.Name);

        var link = UserAccountSteamProfileLink.Create(request.dto.SteamProfileLink);

        if (userAccountInfo == null)
        {
            userAccountInfo = Domain.Users.UserAccounts.UserAccount.Create(
                userAccountId,
                name ?? UserAccountName.Default(),
                link ?? UserAccountSteamProfileLink.Default())
                ?? throw new ResourceCreationFailed();

            _applicationDbContext.UserAccounts.Add(userAccountInfo);
            await _applicationDbContext.SaveChangesAsync();
        }

        userAccountInfo.UpdateUserAccount(name, link);

        await _applicationDbContext.SaveChangesAsync();

        return new UpdateUserAccountInfoDto(userAccountInfo.Name.Name, userAccountInfo.SteamProfileLink.Link);
    }
}

