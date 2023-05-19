using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class MintRequest
{
    [JsonPropertyName("address")]
    public string To { get; set; }
    
    [JsonPropertyName("amount")]
    [Range(1,int.MaxValue)]
    public int Amount { get; set; }
    
    [JsonPropertyName("admin_secret_key")]
    public string? AdminSecretKey { get; set; }
}