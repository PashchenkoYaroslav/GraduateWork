using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class SwapToTokenARequest
{
    [JsonPropertyName("YdProvided")]
    [Range(1,int.MaxValue)]
    public int YdProvided { get; set; }
}