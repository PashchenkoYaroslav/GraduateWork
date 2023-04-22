using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class IncreaseAllowanceToMaxRequest
{
    [JsonPropertyName("to")]
    public string? To { get; set; }
}