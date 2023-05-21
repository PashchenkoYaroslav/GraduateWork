using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class SetApprovalForAllRequest
{
    [JsonPropertyName("secret_key")]
    public string? SecretKey { get; set; }
}