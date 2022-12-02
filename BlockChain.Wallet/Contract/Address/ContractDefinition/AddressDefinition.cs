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

namespace BlockChain.Wallet.Contract.Address.ContractDefinition
{


    public partial class AddressDeployment : AddressDeploymentBase
    {
        public AddressDeployment() : base(BYTECODE) { }
        public AddressDeployment(string byteCode) : base(byteCode) { }
    }

    public class AddressDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea2646970667358221220db28ab32bb4b0422fcac87651bcb1669072d81f06573df95639aed657db192f064736f6c63430008110033";
        public AddressDeploymentBase() : base(BYTECODE) { }
        public AddressDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
