using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace QuickToken.Controllers;

[ApiController]
[Route("[controller]")]
public class QuickTokenBaseController:ControllerBase
{
    private static readonly string dataLocation = "smartContractsData.json";
    public Dictionary<string, string> data;
    public QuickTokenBaseController()
    {
        var text = System.IO.File.ReadAllText(dataLocation);
        data = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
    }
}