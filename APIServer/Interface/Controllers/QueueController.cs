using Application.Queue;
using Application.Users.UserGameRanks.RocketLeague.Queries;
using Contracts.ApiContracts.Queue.Requests;
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
        => await _mediator.Send(new QueueRequestCommand(request.Mode, request.LowerBound, request.UpperBound));
    }
}
