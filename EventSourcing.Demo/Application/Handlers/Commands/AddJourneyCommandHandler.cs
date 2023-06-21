using EventSourcing.Demo.Application.Requests.Commands;
using EventSourcing.Demo.Models;
using EventSourcing.Demo.Models.Events.V1;
using EventSourcing.Demo.Models.Response;
using EventSourcing.Demo.Repositories;
using EventSourcing.Demo.Services;
using MediatR;

namespace EventSourcing.Demo.Application.Handlers.Commands;

public class AddJourneyCommandHandler : IRequestHandler<AddJourneyCommand, BaseCommandResponse>
{
    private readonly ILedgerAggregateRepository _ledgerRepository;

    private readonly ILedgerProjectionService _ledgerProjectionService;

    public AddJourneyCommandHandler(
        ILedgerAggregateRepository ledgerRepository,
        ILedgerProjectionService ledgerProjectionService)
    {
        _ledgerRepository = ledgerRepository;
        _ledgerProjectionService = ledgerProjectionService;
    }

    public async Task<BaseCommandResponse> Handle(AddJourneyCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        try
        {
            var ledger = await _ledgerRepository.GetAggregateAsync(request.Id);

            var ev = new SingleJourneyAdded()
            {
                Journey = request.Journey
            };

            //inmemory aggregation
            ledger.AddSingleJorney(ev);
            // save
            await _ledgerRepository.SaveAggregateAsync(ledger, Guid.NewGuid().ToString("N"));

            response.Id = ledger.Id;
            response.Message = "Under control";

            // instead of direct save we need to publish command into eventbus
            // and handler saves data into projection database
            await UpdateProjectionLedger(request.Id, request.Journey);
        }
        catch(Exception ex)
        {
            response.Success = false;
            response.Errors.Add(ex.Message);
            response.Message = "smth happened";
        }

        return response;
    }

    private async Task UpdateProjectionLedger(string ledgerId, Journey journey)
    {
        var ledger = await _ledgerProjectionService.GetAsync(ledgerId);

        if(ledger != null)
        {
            ledger.Journeys.Add(journey);
            await _ledgerProjectionService.UpdateAsync(ledger);
        }
    }

}
