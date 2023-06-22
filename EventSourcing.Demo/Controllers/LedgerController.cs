using EventSourcing.Demo.Models;
using EventSourcing.Demo.Models.DTOs;
using EventSourcing.Demo.Models.Events.V1;
using EventSourcing.Demo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.Demo.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LedgerController : ControllerBase
{
    private readonly ILedgerAggregateRepository _ledgerRepository;

    public LedgerController(ILedgerAggregateRepository ledgerRepository)
    {
        _ledgerRepository = ledgerRepository;
    }

    [HttpGet("{ledgerId}")]
    public async Task<IActionResult> Get(string ledgerId)
    {
        var ledger = await _ledgerRepository.GetAggregateAsync(ledgerId);

        return Ok(ledger);
    }

    [HttpPost]
    public async Task<IActionResult> Post(LedgerDTO request)
    {
        Ledger ledger = new Ledger { Id = Guid.NewGuid().ToString("N") };

        //inmemory aggregation 
        ledger.Initialise(ledger.Id,
            request.CustomerId,
            request.Begin,
            request.End,
            request.OrderId,
            request.RequiresProcessing);

        // save
        await _ledgerRepository.SaveAggregateAsync(ledger, Guid.NewGuid().ToString("N"));

        return Ok(ledger);
    }

    [HttpPost("addJourney/{ledgerId}")]
    public async Task<IActionResult> SingleJourneyAdded(string ledgerId, Journey request)
    {
        var ledger = await _ledgerRepository.GetAggregateAsync(ledgerId);

        var ev = new SingleJourneyAdded()
        {
            Journey = request
        };

        //inmemory aggregation
        ledger.AddSingleJorney(ev);
        // save
        await _ledgerRepository.SaveAggregateAsync(ledger, Guid.NewGuid().ToString("N"));

        return Ok(ledger);
    }

}
