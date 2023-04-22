using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace QuickToken.Contracts;

[Function("buyToken")]
public class BuyFunction: FunctionMessage
{
    [Parameter("address", "from", 1)]
    public string From { get; set; }
    
    [Parameter("address", "to", 2)]
    public string To { get; set; }
    
    [Parameter("uint256", "amount", 3)]
    public BigInteger Amount { get; set; }
}