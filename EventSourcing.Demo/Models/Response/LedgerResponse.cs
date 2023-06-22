namespace EventSourcing.Demo.Models.Response;

public class LedgerResponse
{
    public string CustomerId { get; set; } = string.Empty;
    public string Begin { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public bool RequiresProcessing { get; set; }
    public List<Journey> Journeys { get; set; } = new();
}
