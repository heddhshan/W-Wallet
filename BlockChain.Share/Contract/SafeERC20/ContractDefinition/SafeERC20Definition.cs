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

namespace BlockChain.Share.Contract.SafeERC20.ContractDefinition
{


    public partial class SafeERC20Deployment : SafeERC20DeploymentBase
    {
        public SafeERC20Deployment() : base(BYTECODE) { }
        public SafeERC20Deployment(string byteCode) : base(byteCode) { }
    }

    public class SafeERC20DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea2646970667358221220c80519595c7fe9d7a6235d70fcc0a9640ff8a9c53fc11e30d4febb6209926a4864736f6c634300080f0033";
        public SafeERC20DeploymentBase() : base(BYTECODE) { }
        public SafeERC20DeploymentBase(string byteCode) : base(byteCode) { }

    }
}
