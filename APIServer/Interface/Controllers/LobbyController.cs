using Application.Lobby.RocketLeague2vs2Lobby.Queries;
using Contracts.ApiContracts.Lobby.RocketLeague;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Interface.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LobbyController : ControllerBase
{

    private readonly IMediator _mediator;
    public LobbyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<RocketLeague2vs2LobbyResponse>> GetRocketLeagueRank()
    => await _mediator.Send(new GetRocketLeague2vs2LobbyQuery());
}
