using EventSourcing.Demo.Application.Requests.Queries;
using EventSourcing.Demo.Models.Response;
using EventSourcing.Demo.Repositories;
using MediatR;

namespace EventSourcing.Demo.Application.Handlers.Queries;

public class GetLedgerRequestHandler : IRequestHandler<GetLedgerRequest, LedgerResponse>
{
    private readonly ILedgerProjectionRepository _ledgerProjectionRepository;

    public GetLedgerRequestHandler(ILedgerProjectionRepository ledgerProjectionRepository)
    {
        _ledgerProjectionRepository = ledgerProjectionRepository;
    }

    public async Task<LedgerResponse> Handle(GetLedgerRequest request, CancellationToken cancellationToken)
    {
        var ledger = await _ledgerProjectionRepository.GetAsync(request.Id);

        return new LedgerResponse
        {
            Begin = ledger.Begin,
            End = ledger.End,
            CustomerId = ledger.CustomerId,
            Journeys = ledger.Journeys,
            OrderId = ledger.OrderId,
            PartitionKey = ledger.PartitionKey,
            RequiresProcessing = ledger.RequiresProcessing
        };
    }
}