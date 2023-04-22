using System.Text.Json.Serialization;

namespace QuickToken.Responses;

public class GetBalanceResponse
{
    [JsonPropertyName("eth")]
    public string Eth { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
    
    [JsonPropertyName("assets_token_ids")]
    public int[] AssetTokenIds { get; set; }
}