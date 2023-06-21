using EventSourcing.Demo.Models.Response;
using MediatR;

namespace EventSourcing.Demo.Application.Requests.Queries;

public class GetLedgerRequest : IRequest<LedgerResponse>
{
    public string Id { get; set; } = string.Empty;
}
