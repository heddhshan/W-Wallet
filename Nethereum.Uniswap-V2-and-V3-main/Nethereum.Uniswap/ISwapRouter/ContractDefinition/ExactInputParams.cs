using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Nethereum.Uniswap.ISwapRouter.ContractDefinition
{
    public partial class ExactInputParams : ExactInputParamsBase { }

    public class ExactInputParamsBase 
    {
        [Parameter("bytes", "path", 1)]
        public virtual byte[] Path { get; set; }
        [Parameter("address", "recipient", 2)]
        public virtual string Recipient { get; set; }
        [Parameter("uint256", "deadline", 3)]
        public virtual BigInteger Deadline { get; set; }
        [Parameter("uint256", "amountIn", 4)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint256", "amountOutMinimum", 5)]
        public virtual BigInteger AmountOutMinimum { get; set; }
    }
}
