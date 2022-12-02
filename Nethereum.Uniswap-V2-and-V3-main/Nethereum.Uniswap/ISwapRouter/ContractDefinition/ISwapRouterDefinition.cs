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

namespace Nethereum.Uniswap.ISwapRouter.ContractDefinition
{


    public partial class ISwapRouterDeployment : ISwapRouterDeploymentBase
    {
        public ISwapRouterDeployment() : base(BYTECODE) { }
        public ISwapRouterDeployment(string byteCode) : base(byteCode) { }
    }

    public class ISwapRouterDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public ISwapRouterDeploymentBase() : base(BYTECODE) { }
        public ISwapRouterDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class ExactInputFunction : ExactInputFunctionBase { }

    [Function("exactInput", "uint256")]
    public class ExactInputFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "params", 1)]
        public virtual ExactInputParams Params { get; set; }
    }

    public partial class ExactInputSingleFunction : ExactInputSingleFunctionBase { }

    [Function("exactInputSingle", "uint256")]
    public class ExactInputSingleFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "params", 1)]
        public virtual ExactInputSingleParams Params { get; set; }
    }

    public partial class ExactOutputFunction : ExactOutputFunctionBase { }

    [Function("exactOutput", "uint256")]
    public class ExactOutputFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "params", 1)]
        public virtual ExactOutputParams Params { get; set; }
    }

    public partial class ExactOutputSingleFunction : ExactOutputSingleFunctionBase { }

    [Function("exactOutputSingle", "uint256")]
    public class ExactOutputSingleFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "params", 1)]
        public virtual ExactOutputSingleParams Params { get; set; }
    }

    public partial class UniswapV3SwapCallbackFunction : UniswapV3SwapCallbackFunctionBase { }

    [Function("uniswapV3SwapCallback")]
    public class UniswapV3SwapCallbackFunctionBase : FunctionMessage
    {
        [Parameter("int256", "amount0Delta", 1)]
        public virtual BigInteger Amount0Delta { get; set; }
        [Parameter("int256", "amount1Delta", 2)]
        public virtual BigInteger Amount1Delta { get; set; }
        [Parameter("bytes", "data", 3)]
        public virtual byte[] Data { get; set; }
    }










}
