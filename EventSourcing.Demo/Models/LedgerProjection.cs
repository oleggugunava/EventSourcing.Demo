using System.Text.Json.Serialization;

namespace EventSourcing.Demo.Models;

public class LedgerProjection
{
    [JsonPropertyName("pk")]
    public string Pk => Id;

    [JsonPropertyName("sk")]
    public string SK => Id;


    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string Begin { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public bool RequiresProcessing { get; set; }
    public List<Journey> Journeys { get; set; } = new();

    public DateTime UpdatedAt { get; set; }
}
