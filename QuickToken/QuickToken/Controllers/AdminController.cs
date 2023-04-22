using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.JsonRpc.Client;
using QuickToken.Contracts;
using QuickToken.Requests;

namespace QuickToken.Controllers;

public class AdminController : QuickTokenBaseController
{
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Increase token B allowance to maximum for address.
    /// </summary>
    [HttpPost("IncreaseAllowanceToMax")]
    public async Task<IActionResult> IncreaseAllowanceToMaxAsync(IncreaseAllowanceToMaxRequest request)
    {
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<IncreaseAllowanceToMaxFunction>();
        var contractMessage = new IncreaseAllowanceToMaxFunction
        {
            To = request.To
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenB"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
    
    /// <summary>
    /// Approve all transactions from address.
    /// </summary>
    [HttpPost("SetApprovalForAll")]
    public async Task<IActionResult> SetApprovalForAllAsync(SetApprovalForAllRequest request)
    {
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        
        var tokenAService = web3.Eth.ERC721.GetContractService(data["AddressTokenA"]);
        await tokenAService.SetApprovalForAllRequestAsync(request.Operator, request.Approved);
        
        return Ok();
    }
    
    /// <summary>
    /// Transfer token A from pool to address.
    /// </summary>
    [HttpPost("Buy")]
    public async Task<IActionResult> BuyAsync(BuyRequest request)
    {
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<BuyFunction>();
        var contractMessage = new BuyFunction
        {
            From = request.From,
            To = request.To,
            Amount = request.Amount
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenA"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
    
    /// <summary>
    /// Transfer token A from pool to address.
    /// </summary>
    [HttpPost("Sell")]
    public async Task<IActionResult> SellAsync(SellRequest request)
    {
        var account = new Account(data["PrivateKey"]);
        
        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        
        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<SellFunction>();
        var contractMessage = new SellFunction
        {
            To = request.To,
            Amount = request.Amount
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenA"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
}