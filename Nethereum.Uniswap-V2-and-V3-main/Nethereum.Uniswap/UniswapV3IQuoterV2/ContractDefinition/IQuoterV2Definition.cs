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

namespace Nethereum.Uniswap.Contracts.IQuoterV2.ContractDefinition
{


    public partial class IQuoterV2Deployment : IQuoterV2DeploymentBase
    {
        public IQuoterV2Deployment() : base(BYTECODE) { }
        public IQuoterV2Deployment(string byteCode) : base(byteCode) { }
    }

    public class IQuoterV2DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public IQuoterV2DeploymentBase() : base(BYTECODE) { }
        public IQuoterV2DeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class QuoteExactInputFunction : QuoteExactInputFunctionBase { }

    [Function("quoteExactInput", typeof(QuoteExactInputOutputDTO))]
    public class QuoteExactInputFunctionBase : FunctionMessage
    {
        [Parameter("bytes", "path", 1)]
        public virtual byte[] Path { get; set; }
        [Parameter("uint256", "amountIn", 2)]
        public virtual BigInteger AmountIn { get; set; }
    }

    public partial class QuoteExactInputSingleFunction : QuoteExactInputSingleFunctionBase { }

    [Function("quoteExactInputSingle", typeof(QuoteExactInputSingleOutputDTO))]
    public class QuoteExactInputSingleFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "params", 1)]
        public virtual QuoteExactInputSingleParams Params { get; set; }
    }

    public partial class QuoteExactOutputFunction : QuoteExactOutputFunctionBase { }

    [Function("quoteExactOutput", typeof(QuoteExactOutputOutputDTO))]
    public class QuoteExactOutputFunctionBase : FunctionMessage
    {
        [Parameter("bytes", "path", 1)]
        public virtual byte[] Path { get; set; }
        [Parameter("uint256", "amountOut", 2)]
        public virtual BigInteger AmountOut { get; set; }
    }

    public partial class QuoteExactOutputSingleFunction : QuoteExactOutputSingleFunctionBase { }

    [Function("quoteExactOutputSingle", typeof(QuoteExactOutputSingleOutputDTO))]
    public class QuoteExactOutputSingleFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "params", 1)]
        public virtual QuoteExactOutputSingleParams Params { get; set; }
    }

    public partial class QuoteExactInputOutputDTO : QuoteExactInputOutputDTOBase { }

    [FunctionOutput]
    public class QuoteExactInputOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "amountOut", 1)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("uint160[]", "sqrtPriceX96AfterList", 2)]
        public virtual List<BigInteger> SqrtPriceX96AfterList { get; set; }
        [Parameter("uint32[]", "initializedTicksCrossedList", 3)]
        public virtual List<uint> InitializedTicksCrossedList { get; set; }
        [Parameter("uint256", "gasEstimate", 4)]
        public virtual BigInteger GasEstimate { get; set; }
    }

    public partial class QuoteExactInputSingleOutputDTO : QuoteExactInputSingleOutputDTOBase { }

    [FunctionOutput]
    public class QuoteExactInputSingleOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "amountOut", 1)]
        public virtual BigInteger AmountOut { get; set; }
        [Parameter("uint160", "sqrtPriceX96After", 2)]
        public virtual BigInteger SqrtPriceX96After { get; set; }
        [Parameter("uint32", "initializedTicksCrossed", 3)]
        public virtual uint InitializedTicksCrossed { get; set; }
        [Parameter("uint256", "gasEstimate", 4)]
        public virtual BigInteger GasEstimate { get; set; }
    }

    public partial class QuoteExactOutputOutputDTO : QuoteExactOutputOutputDTOBase { }

    [FunctionOutput]
    public class QuoteExactOutputOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "amountIn", 1)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint160[]", "sqrtPriceX96AfterList", 2)]
        public virtual List<BigInteger> SqrtPriceX96AfterList { get; set; }
        [Parameter("uint32[]", "initializedTicksCrossedList", 3)]
        public virtual List<uint> InitializedTicksCrossedList { get; set; }
        [Parameter("uint256", "gasEstimate", 4)]
        public virtual BigInteger GasEstimate { get; set; }
    }

    public partial class QuoteExactOutputSingleOutputDTO : QuoteExactOutputSingleOutputDTOBase { }

    [FunctionOutput]
    public class QuoteExactOutputSingleOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "amountIn", 1)]
        public virtual BigInteger AmountIn { get; set; }
        [Parameter("uint160", "sqrtPriceX96After", 2)]
        public virtual BigInteger SqrtPriceX96After { get; set; }
        [Parameter("uint32", "initializedTicksCrossed", 3)]
        public virtual uint InitializedTicksCrossed { get; set; }
        [Parameter("uint256", "gasEstimate", 4)]
        public virtual BigInteger GasEstimate { get; set; }
    }
}
