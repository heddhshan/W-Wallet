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

namespace BlockChain.Wallet.Contract.Strings.ContractDefinition
{


    public partial class StringsDeployment : StringsDeploymentBase
    {
        public StringsDeployment() : base(BYTECODE) { }
        public StringsDeployment(string byteCode) : base(byteCode) { }
    }

    public class StringsDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea2646970667358221220e3f13b00e90973224e0cdeb7be0e658cd841f02a0be30d9b97aa0ebe1522e97964736f6c63430008110033";
        public StringsDeploymentBase() : base(BYTECODE) { }
        public StringsDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
