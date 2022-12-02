using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Nethereum.Uniswap.ISwapRouter.ContractDefinition
{
    public partial class ExactOutputParams : ExactOutputParamsBase { }

    public class ExactOutputParamsBase 
    {
        [Parameter("bytes", "path", 1)]
        public virtual byte[] Path { get; set; }
        [Parameter("address", "recipient", 2)]
        public virtual string Recipient { get; set; }
        [Parameter("uint256", "deadline", 3)]
        public virtual BigInteger Deadline { get; set; }
        [Parameter("uint256", "amountOut", 4)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("uint256", "amountInMaximum", 5)]
        public virtual BigInteger AmountInMaximum { get; set; }
    }
}
