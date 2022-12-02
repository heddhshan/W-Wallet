using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Nethereum.Uniswap.IQuoter.ContractDefinition
{


    public partial class IQuoterDeployment : IQuoterDeploymentBase
    {
        public IQuoterDeployment() : base(BYTECODE) { }
        public IQuoterDeployment(string byteCode) : base(byteCode) { }
    }

    public class IQuoterDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public IQuoterDeploymentBase() : base(BYTECODE) { }
        public IQuoterDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class QuoteExactInputFunction : QuoteExactInputFunctionBase { }

    [Function("quoteExactInput", "uint256")]
    public class QuoteExactInputFunctionBase : FunctionMessage
    {
        [Parameter("bytes", "path", 1)]
        public virtual byte[] Path { get; set; }
        [Parameter("uint256", "amountIn", 2)]
        public virtual BigInteger AmountIn { get; set; }
    }

    public partial class QuoteExactInputSingleFunction : QuoteExactInputSingleFunctionBase { }

    [Function("quoteExactInputSingle", "uint256")]
    public class QuoteExactInputSingleFunctionBase : FunctionMessage
    {
        [Parameter("address", "tokenIn", 1)]
        public virtual string TokenIn { get; set; }
        [Parameter("address", "tokenOut", 2)]
        public virtual string TokenOut { get; set; }
        [Parameter("uint24", "fee", 3)]
        public virtual uint Fee { get; set; }
        [Parameter("uint256", "amountIn", 4)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint160", "sqrtPriceLimitX96", 5)]
        public virtual BigInteger SqrtPriceLimitX96 { get; set; }
    }

    public partial class QuoteExactOutputFunction : QuoteExactOutputFunctionBase { }

    [Function("quoteExactOutput", "uint256")]
    public class QuoteExactOutputFunctionBase : FunctionMessage
    {
        [Parameter("bytes", "path", 1)]
        public virtual byte[] Path { get; set; }
        [Parameter("uint256", "amountOut", 2)]
        public virtual BigInteger AmountOut { get; set; }
    }

    public partial class QuoteExactOutputSingleFunction : QuoteExactOutputSingleFunctionBase { }

    [Function("quoteExactOutputSingle", "uint256")]
    public class QuoteExactOutputSingleFunctionBase : FunctionMessage
    {
        [Parameter("address", "tokenIn", 1)]
        public virtual string TokenIn { get; set; }
        [Parameter("address", "tokenOut", 2)]
        public virtual string TokenOut { get; set; }
        [Parameter("uint24", "fee", 3)]
        public virtual uint Fee { get; set; }
        [Parameter("uint256", "amountOut", 4)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("uint160", "sqrtPriceLimitX96", 5)]
        public virtual BigInteger SqrtPriceLimitX96 { get; set; }
    }



    public partial class QuoteExactInputSingleOutputDTO : QuoteExactInputSingleOutputDTOBase { }

    [FunctionOutput]
    public class QuoteExactInputSingleOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "amountOut", 1)]
        public virtual BigInteger AmountOut { get; set; }
    }




}
