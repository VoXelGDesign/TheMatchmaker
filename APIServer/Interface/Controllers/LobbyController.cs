using Application.Lobby.RocketLeague2vs2Lobby.Queries;
using Application.Lobby.RocketLeague3vs3Lobby.Queries;
using Contracts.ApiContracts.Lobby.RocketLeague;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interface.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LobbyController : ControllerBase
{

    private readonly IMediator _mediator;
    public LobbyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("2vs2")]
    public async Task<ActionResult<RocketLeague2vs2LobbyResponse>> GetRocketLeague2vs2Rank()
    => await _mediator.Send(new GetRocketLeague2vs2LobbyQuery());

    [HttpGet("3vs3")]
    public async Task<ActionResult<RocketLeague3vs3LobbyResponse>> GetRocketLeague3vs3Rank()
    => await _mediator.Send(new GetRocketLeague3vs3LobbyQuery());
}
