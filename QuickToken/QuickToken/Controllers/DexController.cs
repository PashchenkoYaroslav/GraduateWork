using Microsoft.AspNetCore.Mvc;
using Nethereum.JsonRpc.Client;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using QuickToken.Contracts;
using QuickToken.Requests;

namespace QuickToken.Controllers;

public class DexController: QuickTokenBaseController
{
    private readonly ILogger<DexController> _logger;

    public DexController(ILogger<DexController> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Swap token A for a token B.
    /// </summary>
    [HttpPost("swapToTokenA")]
    public async Task<IActionResult> SwapToTokenAAsync(SwapToTokenARequest request)
    {
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<SwapToTokenAFunction>();
        var contractMessage = new SwapToTokenAFunction
        {
            YdProvided = request.YdProvided
        };
        contractMessage.Gas = 10000000;
        var transaction = await contractHandler.SendRequestAsync(data["AddressTokenB"],contractMessage);
        return Ok(transaction);
    }
    
    
    /// <summary>
    /// Swap token B for a token A.
    /// </summary>
    [HttpPost("swapToTokenB")]
    public async Task<IActionResult> SwapToTokenBAsync(SwapToTokenBRequest request)
    {
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<SwapToTokenBFunction>();
        var contractMessage = new SwapToTokenBFunction
        {
            Xd = request.Xd
        };
        contractMessage.Gas = 10000000;
        
        var transaction = await contractHandler.SendRequestAsync(data["AddressTokenB"],contractMessage);
        return Ok(transaction);
    }
}