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
    /// Approve all transactions from address.
    /// </summary>
    [HttpPost("SetApprovalForAll")]
    public async Task<IActionResult> SetApprovalForAllAsync(SetApprovalForAllRequest request)
    {
        var account = new Account(request.SecretKey);

        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);

        var web3 = new Web3(account,web3Client);

        var tokenAService = web3.Eth.ERC721.GetContractService(data["AddressTokenA"]);
        await tokenAService.SetApprovalForAllRequestAsync(data["AddressDEX"], true);

        return Ok();
    }
    
    /// <summary>
    /// Increase token B allowance to maximum for address.
    /// </summary>
    [HttpPost("IncreaseAllowanceToMax")]
    public async Task<IActionResult> IncreaseAllowanceToMaxAsync(IncreaseAllowanceToMaxRequest request)
    {
        var account = new Account(request.SecretKey);

        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);

        var web3 = new Web3(account,web3Client);
        var contractHandler = web3.Eth.GetContractTransactionHandler<IncreaseAllowanceToMaxFunction>();
        var contractMessage = new IncreaseAllowanceToMaxFunction
        {
            To = data["AddressDEX"]
        };

        var transaction = await contractHandler.SendRequestAndWaitForReceiptAsync(data["AddressTokenC"],contractMessage);
        if (transaction.Status.Value != 1)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
    }
}
