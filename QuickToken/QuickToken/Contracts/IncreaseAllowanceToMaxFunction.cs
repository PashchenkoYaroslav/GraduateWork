using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace QuickToken.Contracts;

[Function("increaseAllowanceToMax")]
public class IncreaseAllowanceToMaxFunction : FunctionMessage
{
    [Parameter("address", "to", 1)]
    public string To { get; set; }
}