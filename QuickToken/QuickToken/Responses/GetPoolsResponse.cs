using System.Text.Json.Serialization;

namespace QuickToken.Responses;

public class GetPoolsResponse
{
    [JsonPropertyName("asset_pool")]
    public string AssetPool { get; set; }
    
    [JsonPropertyName("currency_pool")]
    public string CurrencyPool { get; set; }
}