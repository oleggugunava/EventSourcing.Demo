using EventSourcing.Demo.Application.Requests.Commands;
using EventSourcing.Demo.Models;
using EventSourcing.Demo.Models.Response;
using EventSourcing.Demo.Repositories;
using EventSourcing.Demo.Services;
using MediatR;

namespace EventSourcing.Demo.Application.Handlers.Commands;

public class CreateLedgerCommandHandler : IRequestHandler<CreateLedgerCommand, BaseCommandResponse>
{
    private readonly ILedgerAggregateRepository _ledgerRepository;

    private readonly ILedgerProjectionService _ledgerProjectionService;

    public CreateLedgerCommandHandler(
        ILedgerAggregateRepository ledgerRepository,
        ILedgerProjectionService ledgerProjectionService)
    {
        _ledgerRepository = ledgerRepository;
        _ledgerProjectionService = ledgerProjectionService;
    }

    public async Task<BaseCommandResponse> Handle(CreateLedgerCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        try
        {
            Ledger ledger = new Ledger { Id = Guid.NewGuid().ToString("N") };

            var ledgerDTO = request.ledgerDTO;
            ledger.Initialise(ledger.Id,
                ledgerDTO.CustomerId,
                ledgerDTO.Begin,
                ledgerDTO.End,
                ledgerDTO.OrderId,
                ledgerDTO.RequiresProcessing);

            await _ledgerRepository.SaveAggregateAsync(ledger, Guid.NewGuid().ToString("N"));

            response.Id = ledger.Id;
            response.Message = "Under control";

            // instead of direct save we need to publish command into eventbus
            // and handler saves data into projection database
            await _ledgerProjectionService.CreateAsync(MapLedgerIntoProjection(ledger));
        }
        catch(Exception ex)
        {
            response.Success = false;
            response.Errors.Add(ex.Message);
            response.Message = "smth happened";
        }

        return response;
    }


    private LedgerProjection MapLedgerIntoProjection(Ledger ledger)
    {
        return new LedgerProjection
        {
            Begin = ledger.Begin,
            End = ledger.End,
            OrderId = ledger.OrderId,
            RequiresProcessing = ledger.RequiresProcessing,
            Id = ledger.Id,
            CustomerId = ledger.CustomerId,
            Journeys = ledger.Journeys
        };
    }
}
