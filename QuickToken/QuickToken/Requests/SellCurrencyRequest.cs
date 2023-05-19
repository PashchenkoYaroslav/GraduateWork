using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class SellCurrencyRequest
{
    [JsonPropertyName("secret_key")]
    public string? SecretKey { get; set; }
    
    [JsonPropertyName("YdProvided")]
    [Range(1,int.MaxValue)]
    public int YdProvided { get; set; }
}