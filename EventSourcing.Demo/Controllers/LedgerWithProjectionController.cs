using EventSourcing.Demo.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using EventSourcing.Demo.Application.Requests.Commands;
using EventSourcing.Demo.Models.Response;
using EventSourcing.Demo.Application.Requests.Queries;
using EventSourcing.Demo.Models.Events.V1;
using EventSourcing.Demo.Models;

namespace EventSourcing.Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LedgerWithProjectionController : ControllerBase
{
    private readonly IMediator _mediator;

    public LedgerWithProjectionController(IMediator mediator)
    {
        _mediator= mediator;
    }

    [HttpGet("{ledgerId}")]
    public async Task<ActionResult<LedgerResponse>> Get(string ledgerId)
    {
        var response = await _mediator.Send(new GetLedgerRequest() { Id = ledgerId });
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<BaseCommandResponse>> Post(LedgerDTO request)
    {
        var response = await _mediator.Send(new CreateLedgerCommand() { ledgerDTO = request });
        return Ok(response);
    }

    [HttpPost("addJourney/{ledgerId}")]
    public async Task<IActionResult> SingleJourneyAdded(string ledgerId, Journey request)
    {
        var response = await _mediator.Send(new AddJourneyCommand() { Id = ledgerId, Journey = request});
        return Ok(response);
    }
}
