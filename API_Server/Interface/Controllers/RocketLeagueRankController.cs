using Application.Users.UserAccount.Commands;
using Application.Users.UserGameRanks.RocketLeague.Commands;
using Application.Users.UserGameRanks.RocketLeague.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interface.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RocketLeagueRankController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RocketLeagueRankController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<RocketLeagueRankDto>> GetRocketLeagueRank()
        => await _mediator.Send(new GetUserRocketLeagueRankCommand());

        [HttpPut]
        public async Task<ActionResult<RocketLeagueUpdateRankDto>> UpdateRocketLeagueRank(RocketLeagueUpdateRankDto dto)
            => await _mediator.Send(new UpdateRocketLeagueRankCommand(dto));

    }
}
