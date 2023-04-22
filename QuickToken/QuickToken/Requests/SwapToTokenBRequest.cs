using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Requests;

public class SwapToTokenBRequest
{
    [JsonPropertyName("Xd")]
    [Range(1,int.MaxValue)]
    public int Xd { get; set; }
}