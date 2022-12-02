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

namespace Nethereum.Uniswap.IPeripheryImmutableStateIPeripheryImmutableState.ContractDefinition
{


    public partial class IPeripheryImmutableStateDeployment : IPeripheryImmutableStateDeploymentBase
    {
        public IPeripheryImmutableStateDeployment() : base(BYTECODE) { }
        public IPeripheryImmutableStateDeployment(string byteCode) : base(byteCode) { }
    }

    public class IPeripheryImmutableStateDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public IPeripheryImmutableStateDeploymentBase() : base(BYTECODE) { }
        public IPeripheryImmutableStateDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class Weth9Function : Weth9FunctionBase { }

    [Function("WETH9", "address")]
    public class Weth9FunctionBase : FunctionMessage
    {

    }

    public partial class FactoryFunction : FactoryFunctionBase { }

    [Function("factory", "address")]
    public class FactoryFunctionBase : FunctionMessage
    {

    }

    public partial class Weth9OutputDTO : Weth9OutputDTOBase { }

    [FunctionOutput]
    public class Weth9OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class FactoryOutputDTO : FactoryOutputDTOBase { }

    [FunctionOutput]
    public class FactoryOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }
}
