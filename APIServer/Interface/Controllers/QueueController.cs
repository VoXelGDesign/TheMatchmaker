using Application.Queue;
using Application.Users.UserGameRanks.RocketLeague.Queries;
using Application.Users.UserQueueInfos.Queries;
using Contracts.ApiContracts.Queue.Requests;
using Contracts.ApiContracts.Queue.Responses;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;
using Contracts.QueueContracts.RocketLeague;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interface.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueueController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QueueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SendRequest")]
        public async Task RequestQueue([FromBody] QueueRocketLeagueRequest request)
        => await _mediator.Send(new QueueRequestCommand(request.Mode, request.LowerBound, request.UpperBound, request.QueueRegion));


        [HttpGet("Info")]
        public async Task<ActionResult<UserQueueInfoStatus>> GetRocketLeagueRank()
        => await _mediator.Send(new GetUserQueueInfoQuery());
    }
}
