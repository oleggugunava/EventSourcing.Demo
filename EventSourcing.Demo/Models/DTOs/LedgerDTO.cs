namespace EventSourcing.Demo.Models.DTOs;

public class LedgerDTO
{
    public string CustomerId { get; set; } = string.Empty;
    public string Begin { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public bool RequiresProcessing { get; set; }
}
