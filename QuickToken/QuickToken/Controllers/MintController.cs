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
    /// Mint a batch of type A token to the user.
    /// </summary>
    [HttpPost("MintTokenABatch")]
    public async Task<IActionResult> MintTokenABatchAsync(MintBatchRequest request)
    {
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
    /// Mint a batch of type A token to the DEX.
    /// </summary>
    [HttpPost("MintTokenABatchToDEX")]
    public async Task<IActionResult> MintTokenABatchToDexAsync(MintBatchToDexRequest request)
    {
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<MintBatchToDexFunction>();
        var contractMessage = new MintBatchToDexFunction
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
    /// Mint token B to address.
    /// </summary>
    [HttpPost("MintTokenB")]
    public async Task<IActionResult> MintTokenBAsync(MintRequest request)
    {
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

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenB"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
    
    /// <summary>
    /// Mint token B to DEX.
    /// </summary>
    [HttpPost("MintTokenBToDEX")]
    public async Task<IActionResult> MintTokenBToDexAsync(MintToDexRequest request)
    {
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<MintToDexFunction>();
        var contractMessage = new MintToDexFunction{
            To = request.To,
            Amount = request.Amount
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenB"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
}