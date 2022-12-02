using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Nethereum.Uniswap.ISwapRouter.ContractDefinition
{
    public partial class ExactOutputSingleParams : ExactOutputSingleParamsBase { }

    public class ExactOutputSingleParamsBase 
    {
        [Parameter("address", "tokenIn", 1)]
        public virtual string TokenIn { get; set; }
        [Parameter("address", "tokenOut", 2)]
        public virtual string TokenOut { get; set; }
        [Parameter("uint24", "fee", 3)]
        public virtual uint Fee { get; set; }
        [Parameter("address", "recipient", 4)]
        public virtual string Recipient { get; set; }
        [Parameter("uint256", "deadline", 5)]
        public virtual BigInteger Deadline { get; set; }
        [Parameter("uint256", "amountOut", 6)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("uint256", "amountInMaximum", 7)]
        public virtual BigInteger AmountInMaximum { get; set; }
        [Parameter("uint160", "sqrtPriceLimitX96", 8)]
        public virtual BigInteger SqrtPriceLimitX96 { get; set; }
    }
}
