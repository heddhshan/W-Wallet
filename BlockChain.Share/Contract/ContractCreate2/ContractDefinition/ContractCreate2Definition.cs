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

namespace BlockChain.Share.Contract.ContractCreate2.ContractDefinition
{


    public partial class ContractCreate2Deployment : ContractCreate2DeploymentBase
    {
        public ContractCreate2Deployment() : base(BYTECODE) { }
        public ContractCreate2Deployment(string byteCode) : base(byteCode) { }
    }

    public class ContractCreate2DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b506103d5806100206000396000f3fe6080604052600436106100345760003560e01c8063498eacbe14610039578063b8dff9e114610068578063e782f72e14610088575b600080fd5b61004c61004736600461027d565b6100a8565b6040516001600160a01b03909116815260200160405180910390f35b34801561007457600080fd5b5061004c610083366004610338565b61011e565b34801561009457600080fd5b5061004c6100a336600461037d565b610133565b600034816100b7828686610146565b84516020808701919091206040805186815292830189905282018190526001600160a01b038316606083015291925033907fd3438c46b19a693c0cc0f5a6e39b75742eb27495f6d406907d76b7f464dc05aa9060800160405180910390a250949350505050565b600061012b8484846101ff565b949350505050565b600061013f838361025a565b9392505050565b600080844710156101835760405162461bcd60e51b8152602060048201526002602482015261433160f01b60448201526064015b60405180910390fd5b82516000036101b95760405162461bcd60e51b8152602060048201526002602482015261219960f11b604482015260640161017a565b8383516020850187f590506001600160a01b03811661012b5760405162461bcd60e51b8152602060048201526002602482015261433360f01b604482015260640161017a565b604080516001600160f81b031960208083019190915260609390931b6bffffffffffffffffffffffff191660218201526035810194909452605580850193909352805180850390930183526075909301909252805191012090565b600061013f8383306101ff565b634e487b7160e01b600052604160045260246000fd5b6000806040838503121561029057600080fd5b82359150602083013567ffffffffffffffff808211156102af57600080fd5b818501915085601f8301126102c357600080fd5b8135818111156102d5576102d5610267565b604051601f8201601f19908116603f011681019083821181831017156102fd576102fd610267565b8160405282815288602084870101111561031657600080fd5b8260208601602083013760006020848301015280955050505050509250929050565b60008060006060848603121561034d57600080fd5b833592506020840135915060408401356001600160a01b038116811461037257600080fd5b809150509250925092565b6000806040838503121561039057600080fd5b5050803592602090910135915056fea2646970667358221220f3ff9b01856309dc67db0c4d3176673286fe799d6b51242d005f6b1c79de549b64736f6c634300080f0033";
        public ContractCreate2DeploymentBase() : base(BYTECODE) { }
        public ContractCreate2DeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class ComputeContractAddress1Function : ComputeContractAddress1FunctionBase { }

    [Function("computeContractAddress1", "address")]
    public class ComputeContractAddress1FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "_salt", 1)]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes32", "_bytecodeHash", 2)]
        public virtual byte[] BytecodeHash { get; set; }
    }

    public partial class ComputeContractAddress2Function : ComputeContractAddress2FunctionBase { }

    [Function("computeContractAddress2", "address")]
    public class ComputeContractAddress2FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "_salt", 1)]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes32", "_bytecodeHash", 2)]
        public virtual byte[] BytecodeHash { get; set; }
        [Parameter("address", "_deployer", 3)]
        public virtual string Deployer { get; set; }
    }

    public partial class DeployContractFunction : DeployContractFunctionBase { }

    [Function("deployContract", "address")]
    public class DeployContractFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "_salt", 1)]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes", "_bytecode", 2)]
        public virtual byte[] Bytecode { get; set; }
    }

    public partial class OnDeployContractEventDTO : OnDeployContractEventDTOBase { }

    [Event("OnDeployContract")]
    public class OnDeployContractEventDTOBase : IEventDTO
    {
        [Parameter("address", "_user", 1, true )]
        public virtual string User { get; set; }
        [Parameter("uint256", "_amount", 2, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("bytes32", "_salt", 3, false )]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes32", "_bytecodeHash", 4, false )]
        public virtual byte[] BytecodeHash { get; set; }
        [Parameter("address", "_contract", 5, false )]
        public virtual string Contract { get; set; }
    }

    public partial class ComputeContractAddress1OutputDTO : ComputeContractAddress1OutputDTOBase { }

    [FunctionOutput]
    public class ComputeContractAddress1OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ComputeContractAddress2OutputDTO : ComputeContractAddress2OutputDTOBase { }

    [FunctionOutput]
    public class ComputeContractAddress2OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }


}
