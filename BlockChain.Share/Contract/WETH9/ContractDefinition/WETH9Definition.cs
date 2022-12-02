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

namespace BlockChain.Share.Contract.WETH9.ContractDefinition
{


    public partial class WETH9Deployment : WETH9DeploymentBase
    {
        public WETH9Deployment() : base(BYTECODE) { }
        public WETH9Deployment(string byteCode) : base(byteCode) { }
    }

    public class WETH9DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60c0604052600d60809081526c2bb930b83832b21022ba3432b960991b60a05260009061002c9082610114565b506040805180820190915260048152630ae8aa8960e31b60208201526001906100559082610114565b506002805460ff1916601217905534801561006f57600080fd5b506101d3565b634e487b7160e01b600052604160045260246000fd5b600181811c9082168061009f57607f821691505b6020821081036100bf57634e487b7160e01b600052602260045260246000fd5b50919050565b601f82111561010f57600081815260208120601f850160051c810160208610156100ec5750805b601f850160051c820191505b8181101561010b578281556001016100f8565b5050505b505050565b81516001600160401b0381111561012d5761012d610075565b6101418161013b845461008b565b846100c5565b602080601f831160018114610176576000841561015e5750858301515b600019600386901b1c1916600185901b17855561010b565b600085815260208120601f198616915b828110156101a557888601518255948401946001909101908401610186565b50858210156101c35787850151600019600388901b60f8161c191681555b5050505050600190811b01905550565b6107be806101e26000396000f3fe6080604052600436106100a05760003560e01c8063313ce56711610064578063313ce5671461016c57806370a082311461019857806395d89b41146101c5578063a9059cbb146101da578063d0e30db0146101fa578063dd62ed3e1461020257600080fd5b806306fdde03146100b4578063095ea7b3146100df57806318160ddd1461010f57806323b872dd1461012c5780632e1a7d4d1461014c57600080fd5b366100af576100ad61023a565b005b600080fd5b3480156100c057600080fd5b506100c9610295565b6040516100d691906105db565b60405180910390f35b3480156100eb57600080fd5b506100ff6100fa366004610645565b610323565b60405190151581526020016100d6565b34801561011b57600080fd5b50475b6040519081526020016100d6565b34801561013857600080fd5b506100ff61014736600461066f565b610390565b34801561015857600080fd5b506100ad6101673660046106ab565b610514565b34801561017857600080fd5b506002546101869060ff1681565b60405160ff90911681526020016100d6565b3480156101a457600080fd5b5061011e6101b33660046106c4565b60036020526000908152604090205481565b3480156101d157600080fd5b506100c96105ba565b3480156101e657600080fd5b506100ff6101f5366004610645565b6105c7565b6100ad61023a565b34801561020e57600080fd5b5061011e61021d3660046106df565b600460209081526000928352604080842090915290825290205481565b3360009081526003602052604081208054349290610259908490610728565b909155505060405134815233907fe1fffcc4923d04b559f4d29a8bfc6cda04eb5b0d3c460751c2402c5c5cc9109c9060200160405180910390a2565b600080546102a29061073b565b80601f01602080910402602001604051908101604052809291908181526020018280546102ce9061073b565b801561031b5780601f106102f05761010080835404028352916020019161031b565b820191906000526020600020905b8154815290600101906020018083116102fe57829003601f168201915b505050505081565b3360008181526004602090815260408083206001600160a01b038716808552925280832085905551919290917f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b9259061037e9086815260200190565b60405180910390a35060015b92915050565b6001600160a01b0383166000908152600360205260408120548211156103b557600080fd5b6001600160a01b03841633148015906103f357506001600160a01b038416600090815260046020908152604080832033845290915290205460001914155b15610461576001600160a01b038416600090815260046020908152604080832033845290915290205482111561042857600080fd5b6001600160a01b03841660009081526004602090815260408083203384529091528120805484929061045b908490610775565b90915550505b6001600160a01b03841660009081526003602052604081208054849290610489908490610775565b90915550506001600160a01b038316600090815260036020526040812080548492906104b6908490610728565b92505081905550826001600160a01b0316846001600160a01b03167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef8460405161050291815260200190565b60405180910390a35060019392505050565b3360009081526003602052604090205481111561053057600080fd5b336000908152600360205260408120805483929061054f908490610775565b9091555050604051339082156108fc029083906000818181858888f19350505050158015610581573d6000803e3d6000fd5b5060405181815233907f7fcf532c15f0a6db0bd6d0e038bea71d30d808c7d98cb3bf7268a95bf5081b659060200160405180910390a250565b600180546102a29061073b565b60006105d4338484610390565b9392505050565b600060208083528351808285015260005b81811015610608578581018301518582016040015282016105ec565b506000604082860101526040601f19601f8301168501019250505092915050565b80356001600160a01b038116811461064057600080fd5b919050565b6000806040838503121561065857600080fd5b61066183610629565b946020939093013593505050565b60008060006060848603121561068457600080fd5b61068d84610629565b925061069b60208501610629565b9150604084013590509250925092565b6000602082840312156106bd57600080fd5b5035919050565b6000602082840312156106d657600080fd5b6105d482610629565b600080604083850312156106f257600080fd5b6106fb83610629565b915061070960208401610629565b90509250929050565b634e487b7160e01b600052601160045260246000fd5b8082018082111561038a5761038a610712565b600181811c9082168061074f57607f821691505b60208210810361076f57634e487b7160e01b600052602260045260246000fd5b50919050565b8181038181111561038a5761038a61071256fea2646970667358221220e368d0e023f2a7b20298694c073ebe6238b0f3df3a18677b2754ab61d0ddf6eb64736f6c63430008110033";
        public WETH9DeploymentBase() : base(BYTECODE) { }
        public WETH9DeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AllowanceFunction : AllowanceFunctionBase { }

    [Function("allowance", "uint256")]
    public class AllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("address", "", 2)]
        public virtual string ReturnValue2 { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve", "bool")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "guy", 1)]
        public virtual string Guy { get; set; }
        [Parameter("uint256", "wad", 2)]
        public virtual BigInteger Wad { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class DecimalsFunction : DecimalsFunctionBase { }

    [Function("decimals", "uint8")]
    public class DecimalsFunctionBase : FunctionMessage
    {

    }

    public partial class DepositFunction : DepositFunctionBase { }

    [Function("deposit")]
    public class DepositFunctionBase : FunctionMessage
    {

    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "dst", 1)]
        public virtual string Dst { get; set; }
        [Parameter("uint256", "wad", 2)]
        public virtual BigInteger Wad { get; set; }
    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom", "bool")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "src", 1)]
        public virtual string Src { get; set; }
        [Parameter("address", "dst", 2)]
        public virtual string Dst { get; set; }
        [Parameter("uint256", "wad", 3)]
        public virtual BigInteger Wad { get; set; }
    }

    public partial class WithdrawFunction : WithdrawFunctionBase { }

    [Function("withdraw")]
    public class WithdrawFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "wad", 1)]
        public virtual BigInteger Wad { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "src", 1, true )]
        public virtual string Src { get; set; }
        [Parameter("address", "guy", 2, true )]
        public virtual string Guy { get; set; }
        [Parameter("uint256", "wad", 3, false )]
        public virtual BigInteger Wad { get; set; }
    }

    public partial class DepositEventDTO : DepositEventDTOBase { }

    [Event("Deposit")]
    public class DepositEventDTOBase : IEventDTO
    {
        [Parameter("address", "dst", 1, true )]
        public virtual string Dst { get; set; }
        [Parameter("uint256", "wad", 2, false )]
        public virtual BigInteger Wad { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "src", 1, true )]
        public virtual string Src { get; set; }
        [Parameter("address", "dst", 2, true )]
        public virtual string Dst { get; set; }
        [Parameter("uint256", "wad", 3, false )]
        public virtual BigInteger Wad { get; set; }
    }

    public partial class WithdrawalEventDTO : WithdrawalEventDTOBase { }

    [Event("Withdrawal")]
    public class WithdrawalEventDTOBase : IEventDTO
    {
        [Parameter("address", "src", 1, true )]
        public virtual string Src { get; set; }
        [Parameter("uint256", "wad", 2, false )]
        public virtual BigInteger Wad { get; set; }
    }

    public partial class AllowanceOutputDTO : AllowanceOutputDTOBase { }

    [FunctionOutput]
    public class AllowanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DecimalsOutputDTO : DecimalsOutputDTOBase { }

    [FunctionOutput]
    public class DecimalsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "", 1)]
        public virtual byte ReturnValue1 { get; set; }
    }



    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }






}
