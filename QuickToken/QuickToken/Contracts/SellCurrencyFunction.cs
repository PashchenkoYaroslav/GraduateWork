using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace QuickToken.Contracts;

[Function("sellCurrency")]
public class SellCurrencyFunction : FunctionMessage
{
    [Parameter("uint256", "YdProvided", 1)]
    public BigInteger YdProvided { get; set; }
}