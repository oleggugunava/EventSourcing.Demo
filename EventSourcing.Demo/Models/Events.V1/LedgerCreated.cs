namespace EventSourcing.Demo.Models.Events.V1;

public class LedgerCreated
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string Begin { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public bool RequiresProcessing { get; set; }
}
