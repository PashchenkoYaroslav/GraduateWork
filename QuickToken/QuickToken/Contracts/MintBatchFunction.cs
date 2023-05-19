using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace QuickToken.Contracts;

[Function("mintBatch")]
public class MintBatchFunction: FunctionMessage
{
    [Parameter("address", "to", 1)]
    public string To { get; set; }
    
    [Parameter("uint256", "amount", 2)]
    public BigInteger Amount { get; set; }
}