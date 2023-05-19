using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace QuickToken.DTO;
[FunctionOutput]
public class GetPoolDTO : IFunctionOutputDTO
{
    [Parameter("uint256", "pool", 1)]
    public BigInteger Pool { get; set; }
}