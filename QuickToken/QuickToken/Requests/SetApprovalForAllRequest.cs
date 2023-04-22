using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class SetApprovalForAllRequest
{
    [JsonPropertyName("operator")]
    public string? Operator { get; set; }
    
    [JsonPropertyName("approved")]
    public bool Approved { get; set; }
}