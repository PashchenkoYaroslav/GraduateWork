using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace QuickToken.Contracts;

[Function("mintBatch")]
public class MintBatchFunction: FunctionMessage
{
    [Parameter("uint256", "amount", 1)]
    public BigInteger Amount { get; set; }
    
    [Parameter("address", "to", 2)]
    public string To { get; set; }
}