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

namespace BlockChain.Share.Contract.Create2.ContractDefinition
{


    public partial class Create2Deployment : Create2DeploymentBase
    {
        public Create2Deployment() : base(BYTECODE) { }
        public Create2Deployment(string byteCode) : base(byteCode) { }
    }

    public class Create2DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea2646970667358221220f905c36fe214cf52fe0e7511f210c5f21ca184e1acf6c136abd00019185042a764736f6c634300080f0033";
        public Create2DeploymentBase() : base(BYTECODE) { }
        public Create2DeploymentBase(string byteCode) : base(byteCode) { }

    }
}
