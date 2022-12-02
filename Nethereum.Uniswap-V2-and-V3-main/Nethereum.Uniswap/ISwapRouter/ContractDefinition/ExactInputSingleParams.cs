using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Nethereum.Uniswap.ISwapRouter.ContractDefinition
{
    public partial class ExactInputSingleParams : ExactInputSingleParamsBase { }

    public class ExactInputSingleParamsBase 
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
        [Parameter("uint256", "amountIn", 6)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint256", "amountOutMinimum", 7)]
        public virtual BigInteger AmountOutMinimum { get; set; }
        [Parameter("uint160", "sqrtPriceLimitX96", 8)]
        public virtual BigInteger SqrtPriceLimitX96 { get; set; }
    }
}
