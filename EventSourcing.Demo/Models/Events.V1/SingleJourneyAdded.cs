namespace EventSourcing.Demo.Models.Events.V1;

public class SingleJourneyAdded
{
    public Journey Journey { get; set; } = new();
}