using Application.Users.UserAccount.Commands;
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
    public async Task<ActionResult<UserAccountInfoDto>> GetUserAccountInfo()
        => await _mediator.Send(new GetUserAccountInfoCommand());

    [HttpPut]
    public async Task<ActionResult<UpdateUserAccountInfoDto>> UpdateUserAccountInfo(UpdateUserAccountInfoDto dto)
        => await _mediator.Send(new UpdateUserAccountInfoCommand(dto));
}

