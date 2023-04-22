using Microsoft.AspNetCore.Mvc;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using QuickToken.Requests;
using QuickToken.Responses;

namespace QuickToken.Controllers;

public class DataController : QuickTokenBaseController
{
    private readonly ILogger<DataController> _logger;

    public DataController(ILogger<DataController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Mint a batch of type A token to the user.
    /// </summary>
    [HttpGet("GetBalances")]
    public async Task<IActionResult> GetBalanceAsync([FromQuery] GetBalanceRequest request)
    {
        var account = new Account(data["PrivateKey"]);

        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);

        var web3 = new Web3(account, web3Client);

        var tokenAService = web3.Eth.ERC721.GetContractService(data["AddressTokenA"]);
        var tokenBService = web3.Eth.ERC20.GetContractService(data["AddressTokenB"]);

        var ethBalance = await web3.Eth.GetBalance.SendRequestAsync(request.Address);
        var tokenAIds =  await tokenAService.GetAllTokenIdsOfOwnerUsingTokenOfOwnerByIndexAndMultiCallAsync(request.Address);
        var tokenBBalance = await tokenBService.BalanceOfQueryAsync(request.Address);

        var response = new GetBalanceResponse
        {
            Eth = ethBalance.ToString(),
            Currency = tokenBBalance.ToString(),
            AssetTokenIds = tokenAIds.Select(p => (int)p).ToArray()
        };
        return Ok(response);
    }
}