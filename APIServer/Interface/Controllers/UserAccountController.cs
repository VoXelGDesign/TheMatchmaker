using Application.Users.UserAccount.Commands;
using Application.Users.UserAccount.Queries;
using Contracts.ApiContracts.UserAccountInfo.Requests;
using Contracts.ApiContracts.UserAccountInfo.Responses;
using Contracts.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interface.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserAccountController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<GetUserAccountInfoResponse>> GetUserAccountInfo()
        => await _mediator.Send(new GetUserAccountInfoQuery());

    [HttpGet("/Identifier")]
    public async Task<ActionResult<UserIdDto>> GetUserId()
        => await _mediator.Send(new GetUserIdentifierQuery());

    [HttpPut]
    public async Task<ActionResult<UpdateUserAccountInfoResponse>> UpdateUserAccountInfo(UpdateUserAccountInfoRequest dto)
        => await _mediator.Send(new UpdateUserAccountInfoCommand(dto));


}

