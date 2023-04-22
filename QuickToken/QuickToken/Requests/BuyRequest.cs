using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class BuyRequest
{
    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("to")]
    public string To { get; set; }
    
    [JsonPropertyName("amount")]
    [Range(1,int.MaxValue)]
    public int Amount { get; set; }
}