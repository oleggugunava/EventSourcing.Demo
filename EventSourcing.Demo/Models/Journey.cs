namespace EventSourcing.Demo.Models;

public class Journey
{
    public List<Leg>? Legs { get; set; }
    public string ConversationId { get; set; } = string.Empty;
    public string TrackingId { get; set; } = string.Empty;
}

public class Leg
{
    public string TimetableId { get; set; } = string.Empty;
    public int LegNumber { get; set; }
}