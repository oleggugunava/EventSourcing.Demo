using EventSourcing.Demo.Models.DTOs;
using EventSourcing.Demo.Models.Response;
using MediatR;

namespace EventSourcing.Demo.Application.Requests.Commands;

public class CreateLedgerCommand : IRequest<BaseCommandResponse>
{
    public LedgerDTO ledgerDTO { get; set; } = new();
}
