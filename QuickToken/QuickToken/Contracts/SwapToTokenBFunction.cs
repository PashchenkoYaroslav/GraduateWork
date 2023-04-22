using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace QuickToken.Contracts;

[Function("swapToTokenB")]
public class SwapToTokenBFunction : FunctionMessage
{
    [Parameter("uint256", "Xd", 1)]
    public BigInteger Xd { get; set; }
}