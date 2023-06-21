using EventSourcing.Demo.Models;
using EventSourcing.Demo.Models.DTOs;
using EventSourcing.Demo.Models.Response;
using MediatR;

namespace EventSourcing.Demo.Application.Requests.Commands;

public class AddJourneyCommand : IRequest<BaseCommandResponse>
{
    public string Id { get; set; } = string.Empty;
    public Journey Journey { get; set; } = new();
}
