using Microsoft.AspNetCore.Mvc;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using QuickToken.Contracts;
using QuickToken.Requests;

namespace QuickToken.Controllers;

public class MintController: QuickTokenBaseController
{
    private readonly ILogger<MintController> _logger;

    public MintController(ILogger<MintController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Mint a batch of Asset to the user.
    /// </summary>
    [HttpPost("MintAssetToUser")]
    public async Task<IActionResult> MintAssetAsync(MintBatchRequest request)
    {
        if (request.AdminSecretKey != data["PrivateKey"])
        {
            return new ObjectResult("Данный метод доступен только для администрации!"){StatusCode = 403};
        }
        
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<MintBatchFunction>();
        var contractMessage = new MintBatchFunction
        {
            To = request.To,
            Amount = request.Amount
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenA"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
    
    /// <summary>
    /// Mint a batch of Asset to the DEX.
    /// </summary>
    [HttpPost("MintAssetToDEX")]
    public async Task<IActionResult> MintAssetToDexAsync(MintBatchToDexRequest request)
    {
        if (request.AdminSecretKey != data["PrivateKey"])
        {
            return new ObjectResult("Данный метод доступен только для администрации!"){StatusCode = 403};
        }
        
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<MintBatchToDexFunction>();
        var contractMessage = new MintBatchToDexFunction
        {
            To = data["AddressDEX"],
            Amount = request.Amount
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenA"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
    

    /// <summary>
    /// Mint a batch of Currency to the user.
    /// </summary>
    [HttpPost("MintCurrencyToUser")]
    public async Task<IActionResult> MintCurrencyAsync(MintRequest request)
    {
        if (request.AdminSecretKey != data["PrivateKey"])
        {
            return new ObjectResult("Данный метод доступен только для администрации!"){StatusCode = 403};
        }
        
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<MintFunction>();
        var contractMessage = new MintFunction
        {
            To = request.To,
            Amount = request.Amount
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenC"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
    
    /// <summary>
    /// Mint a batch of Currency to DEX.
    /// </summary>
    [HttpPost("MintCurrencyToDEX")]
    public async Task<IActionResult> MintCurrencyToDexAsync(MintToDexRequest request)
    {
        if (request.AdminSecretKey != data["PrivateKey"])
        {
            return new ObjectResult("Данный метод доступен только для администрации!"){StatusCode = 403};
        }
        
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<MintToDexFunction>();
        var contractMessage = new MintToDexFunction{
            To = data["AddressDEX"],
            Amount = request.Amount
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenC"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
}