using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class SellAssetRequest
{
    [JsonPropertyName("secret_key")]
    public string? SecretKey { get; set; }
    
    [JsonPropertyName("Xd")]
    [Range(1,int.MaxValue)]
    public int Xd { get; set; }
}