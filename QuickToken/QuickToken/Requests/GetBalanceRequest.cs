using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class GetBalanceRequest
{
    [JsonPropertyName("address")]
    public string Address { get; set; }
}