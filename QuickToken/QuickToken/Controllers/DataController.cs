using Microsoft.AspNetCore.Mvc;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using QuickToken.Contracts;
using QuickToken.DTO;
using QuickToken.Requests;
using QuickToken.Responses;

namespace QuickToken.Controllers;

public class DataController : QuickTokenBaseController
{
    private readonly ILogger<DataController> _logger;
    private readonly int mathDivider = 18;
    public DataController(ILogger<DataController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get user balances.
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
        var tokenCService = web3.Eth.ERC20.GetContractService(data["AddressTokenC"]);

        var ethBalance = await web3.Eth.GetBalance.SendRequestAsync(request.Address);
        var tokenAIds =  await tokenAService.GetAllTokenIdsOfOwnerUsingTokenOfOwnerByIndexAndMultiCallAsync(request.Address);
        var tokenCBalance = await tokenCService.BalanceOfQueryAsync(request.Address);
        string currencyBalance;
        try
        {
            currencyBalance = tokenCBalance.ToString().Insert(tokenCBalance.ToString().Length - mathDivider, ".");
        }
        catch
        {
            currencyBalance = tokenCBalance.ToString();
        }
        var response = new GetBalanceResponse
        {
            Eth = ethBalance.ToString(),
            Currency = currencyBalance,
            AssetTokenIds = tokenAIds.Select(p => (int)p).ToArray()
        };
        return Ok(response);
    }
    
    /// <summary>
    /// Get DEX pools.
    /// </summary>
    [HttpGet("GetPools")]
    public async Task<IActionResult> GetPoolsAsync()
    {
        var account = new Account(data["PrivateKey"]);

        var web3Client = new RpcClient(baseUrl: new Uri(data["API-URL"]), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);

        var web3 = new Web3(account, web3Client);

        var contractHandler = web3.Eth.GetContractQueryHandler<GetPoolFunction>();
        var contractMessage = new GetPoolFunction
        {
        };

        var assetPool = await contractHandler.QueryDeserializingToObjectAsync<GetPoolDTO>(contractMessage,data["AddressTokenA"]);
        var currencyPool = await contractHandler.QueryDeserializingToObjectAsync<GetPoolDTO>(contractMessage,data["AddressTokenC"]);
        string currencyBalance;
        try
        {
            currencyBalance = currencyPool.Pool.ToString().Insert
                (currencyPool.Pool.ToString().Length - mathDivider, ".");
        }
        catch
        {
            currencyBalance = currencyPool.Pool.ToString();
        }
        var response = new GetPoolsResponse
        {
            AssetPool = assetPool.Pool.ToString(),
            CurrencyPool = currencyBalance
        };
        return Ok(response);
    }
}