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
    /// Sell user asset for currency.
    /// </summary>
    [HttpPost("sellAsset")]
    public async Task<IActionResult> SellAssetAsync(SellAssetRequest request)
    {
        var account = new Account(request.SecretKey);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<SellAssetFunction>();
        var contractMessage = new SellAssetFunction
        {
            Xd = request.Xd
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressDEX"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
    
    
    /// <summary>
    /// Sell user currency for asset.
    /// </summary>
    [HttpPost("sellCurrency")]
    public async Task<IActionResult> SellCurrencyAsync(SellCurrencyRequest request)
    {
        var account = new Account(request.SecretKey);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<SellCurrencyFunction>();
        var contractMessage = new SellCurrencyFunction
        {
            YdProvided = request.YdProvided
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressDEX"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
}