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

namespace BlockChain.Lido.Contract.IWstETH.ContractDefinition
{


    public partial class IWstETHDeployment : IWstETHDeploymentBase
    {
        public IWstETHDeployment() : base(BYTECODE) { }
        public IWstETHDeployment(string byteCode) : base(byteCode) { }
    }

    public class IWstETHDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public IWstETHDeploymentBase() : base(BYTECODE) { }
        public IWstETHDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AllowanceFunction : AllowanceFunctionBase { }

    [Function("allowance", "uint256")]
    public class AllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2)]
        public virtual string Spender { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve", "bool")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class DecimalsFunction : DecimalsFunctionBase { }

    [Function("decimals", "uint8")]
    public class DecimalsFunctionBase : FunctionMessage
    {

    }

    public partial class GetStETHByWstETHFunction : GetStETHByWstETHFunctionBase { }

    [Function("getStETHByWstETH", "uint256")]
    public class GetStETHByWstETHFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_wstETHAmount", 1)]
        public virtual BigInteger WstETHAmount { get; set; }
    }

    public partial class GetWstETHByStETHFunction : GetWstETHByStETHFunctionBase { }

    [Function("getWstETHByStETH", "uint256")]
    public class GetWstETHByStETHFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_stETHAmount", 1)]
        public virtual BigInteger StETHAmount { get; set; }
    }

    public partial class StEthPerTokenFunction : StEthPerTokenFunctionBase { }

    [Function("stEthPerToken", "uint256")]
    public class StEthPerTokenFunctionBase : FunctionMessage
    {

    }

    public partial class TokensPerStEthFunction : TokensPerStEthFunctionBase { }

    [Function("tokensPerStEth", "uint256")]
    public class TokensPerStEthFunctionBase : FunctionMessage
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
        [Parameter("address", "recipient", 1)]
        public virtual string Recipient { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom", "bool")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "sender", 1)]
        public virtual string Sender { get; set; }
        [Parameter("address", "recipient", 2)]
        public virtual string Recipient { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class UnwrapFunction : UnwrapFunctionBase { }

    [Function("unwrap", "uint256")]
    public class UnwrapFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_wstETHAmount", 1)]
        public virtual BigInteger WstETHAmount { get; set; }
    }

    public partial class WrapFunction : WrapFunctionBase { }

    [Function("wrap", "uint256")]
    public class WrapFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_stETHAmount", 1)]
        public virtual BigInteger StETHAmount { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2, true )]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "from", 1, true )]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
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

    public partial class GetStETHByWstETHOutputDTO : GetStETHByWstETHOutputDTOBase { }

    [FunctionOutput]
    public class GetStETHByWstETHOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetWstETHByStETHOutputDTO : GetWstETHByStETHOutputDTOBase { }

    [FunctionOutput]
    public class GetWstETHByStETHOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class StEthPerTokenOutputDTO : StEthPerTokenOutputDTOBase { }

    [FunctionOutput]
    public class StEthPerTokenOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class TokensPerStEthOutputDTO : TokensPerStEthOutputDTOBase { }

    [FunctionOutput]
    public class TokensPerStEthOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }








}
