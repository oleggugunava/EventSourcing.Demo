using EventSourcing.Demo.Models.Events.V1;
using System.Text.RegularExpressions;
using Trainline.EventSourcing.Model;

namespace EventSourcing.Demo.Models;

public class Ledger : Aggregate
{
    private readonly List<Journey> _journeys = new List<Journey>();
    public string CustomerId { get; private set; } = string.Empty;
    public string Begin { get; private set; } = string.Empty;
    public string End { get; private set; } = string.Empty;
    public string OrderId { get; private set; } = string.Empty;
    public bool RequiresProcessing { get; private set; }
    public List<Journey> Journeys  => _journeys;

    public void Initialise(
           string id,
           string customerId,
           string begin,
           string end,
           string orderId,
           bool requiresProcessing)
    {
        var ev = new LedgerCreated
        {
            Id = id,
            CustomerId= customerId,
            Begin=begin,
            End=end,
            OrderId=orderId,
            RequiresProcessing=requiresProcessing
        };

        Apply(ev);
    }

    public void AddSingleJorney(SingleJourneyAdded @event)
    {
        Apply(@event);
    }

    public void Transition(LedgerCreated ledgerCreated)
    {
        Id = ledgerCreated.Id;
        CustomerId = ledgerCreated.CustomerId;
        Begin = ledgerCreated.Begin;
        End = ledgerCreated.End;
        OrderId = ledgerCreated.OrderId;
        RequiresProcessing = ledgerCreated.RequiresProcessing;
    }

    public void Transition(SingleJourneyAdded @event)
    {
        _journeys.Add(@event.Journey);
    }
}
