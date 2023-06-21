namespace EventSourcing.Demo.Models.Response;

public class BaseCommandResponse
{
    public string Id { get; set; } = string.Empty;
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
}
