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

namespace BlockChain.Lido.Contract.ILido.ContractDefinition
{


    public partial class ILidoDeployment : ILidoDeploymentBase
    {
        public ILidoDeployment() : base(BYTECODE) { }
        public ILidoDeployment(string byteCode) : base(byteCode) { }
    }

    public class ILidoDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public ILidoDeploymentBase() : base(BYTECODE) { }
        public ILidoDeploymentBase(string byteCode) : base(byteCode) { }

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

    public partial class GetBeaconStatFunction : GetBeaconStatFunctionBase { }

    [Function("getBeaconStat", typeof(GetBeaconStatOutputDTO))]
    public class GetBeaconStatFunctionBase : FunctionMessage
    {

    }

    public partial class GetBufferedEtherFunction : GetBufferedEtherFunctionBase { }

    [Function("getBufferedEther", "uint256")]
    public class GetBufferedEtherFunctionBase : FunctionMessage
    {

    }

    public partial class GetCurrentStakeLimitFunction : GetCurrentStakeLimitFunctionBase { }

    [Function("getCurrentStakeLimit", "uint256")]
    public class GetCurrentStakeLimitFunctionBase : FunctionMessage
    {

    }

    public partial class GetFeeFunction : GetFeeFunctionBase { }

    [Function("getFee", "uint16")]
    public class GetFeeFunctionBase : FunctionMessage
    {

    }

    public partial class GetFeeDistributionFunction : GetFeeDistributionFunctionBase { }

    [Function("getFeeDistribution", typeof(GetFeeDistributionOutputDTO))]
    public class GetFeeDistributionFunctionBase : FunctionMessage
    {

    }

    public partial class GetPooledEthBySharesFunction : GetPooledEthBySharesFunctionBase { }

    [Function("getPooledEthByShares", "uint256")]
    public class GetPooledEthBySharesFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_sharesAmount", 1)]
        public virtual BigInteger SharesAmount { get; set; }
    }

    public partial class GetSharesByPooledEthFunction : GetSharesByPooledEthFunctionBase { }

    [Function("getSharesByPooledEth", "uint256")]
    public class GetSharesByPooledEthFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_pooledEthAmount", 1)]
        public virtual BigInteger PooledEthAmount { get; set; }
    }

    public partial class GetStakeLimitFullInfoFunction : GetStakeLimitFullInfoFunctionBase { }

    [Function("getStakeLimitFullInfo", typeof(GetStakeLimitFullInfoOutputDTO))]
    public class GetStakeLimitFullInfoFunctionBase : FunctionMessage
    {

    }

    public partial class GetTotalPooledEtherFunction : GetTotalPooledEtherFunctionBase { }

    [Function("getTotalPooledEther", "uint256")]
    public class GetTotalPooledEtherFunctionBase : FunctionMessage
    {

    }

    public partial class GetTotalSharesFunction : GetTotalSharesFunctionBase { }

    [Function("getTotalShares", "uint256")]
    public class GetTotalSharesFunctionBase : FunctionMessage
    {

    }

    public partial class GetWithdrawalCredentialsFunction : GetWithdrawalCredentialsFunctionBase { }

    [Function("getWithdrawalCredentials", "bytes")]
    public class GetWithdrawalCredentialsFunctionBase : FunctionMessage
    {

    }

    public partial class HandleOracleReportFunction : HandleOracleReportFunctionBase { }

    [Function("handleOracleReport")]
    public class HandleOracleReportFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_epoch", 1)]
        public virtual BigInteger Epoch { get; set; }
        [Parameter("uint256", "_eth2balance", 2)]
        public virtual BigInteger Eth2balance { get; set; }
    }

    public partial class IsStakingPausedFunction : IsStakingPausedFunctionBase { }

    [Function("isStakingPaused", "bool")]
    public class IsStakingPausedFunctionBase : FunctionMessage
    {

    }

    public partial class PauseStakingFunction : PauseStakingFunctionBase { }

    [Function("pauseStaking")]
    public class PauseStakingFunctionBase : FunctionMessage
    {

    }

    public partial class ReceiveELRewardsFunction : ReceiveELRewardsFunctionBase { }

    [Function("receiveELRewards")]
    public class ReceiveELRewardsFunctionBase : FunctionMessage
    {

    }

    public partial class RemoveStakingLimitFunction : RemoveStakingLimitFunctionBase { }

    [Function("removeStakingLimit")]
    public class RemoveStakingLimitFunctionBase : FunctionMessage
    {

    }

    public partial class ResumeFunction : ResumeFunctionBase { }

    [Function("resume")]
    public class ResumeFunctionBase : FunctionMessage
    {

    }

    public partial class ResumeStakingFunction : ResumeStakingFunctionBase { }

    [Function("resumeStaking")]
    public class ResumeStakingFunctionBase : FunctionMessage
    {

    }

    public partial class SetELRewardsVaultFunction : SetELRewardsVaultFunctionBase { }

    [Function("setELRewardsVault")]
    public class SetELRewardsVaultFunctionBase : FunctionMessage
    {
        [Parameter("address", "_executionLayerRewardsVault", 1)]
        public virtual string ExecutionLayerRewardsVault { get; set; }
    }

    public partial class SetELRewardsWithdrawalLimitFunction : SetELRewardsWithdrawalLimitFunctionBase { }

    [Function("setELRewardsWithdrawalLimit")]
    public class SetELRewardsWithdrawalLimitFunctionBase : FunctionMessage
    {
        [Parameter("uint16", "_limitPoints", 1)]
        public virtual ushort LimitPoints { get; set; }
    }

    public partial class SetFeeFunction : SetFeeFunctionBase { }

    [Function("setFee")]
    public class SetFeeFunctionBase : FunctionMessage
    {
        [Parameter("uint16", "_feeBasisPoints", 1)]
        public virtual ushort FeeBasisPoints { get; set; }
    }

    public partial class SetFeeDistributionFunction : SetFeeDistributionFunctionBase { }

    [Function("setFeeDistribution")]
    public class SetFeeDistributionFunctionBase : FunctionMessage
    {
        [Parameter("uint16", "_treasuryFeeBasisPoints", 1)]
        public virtual ushort TreasuryFeeBasisPoints { get; set; }
        [Parameter("uint16", "_insuranceFeeBasisPoints", 2)]
        public virtual ushort InsuranceFeeBasisPoints { get; set; }
        [Parameter("uint16", "_operatorsFeeBasisPoints", 3)]
        public virtual ushort OperatorsFeeBasisPoints { get; set; }
    }

    public partial class SetProtocolContractsFunction : SetProtocolContractsFunctionBase { }

    [Function("setProtocolContracts")]
    public class SetProtocolContractsFunctionBase : FunctionMessage
    {
        [Parameter("address", "_oracle", 1)]
        public virtual string Oracle { get; set; }
        [Parameter("address", "_treasury", 2)]
        public virtual string Treasury { get; set; }
        [Parameter("address", "_insuranceFund", 3)]
        public virtual string InsuranceFund { get; set; }
    }

    public partial class SetStakingLimitFunction : SetStakingLimitFunctionBase { }

    [Function("setStakingLimit")]
    public class SetStakingLimitFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_maxStakeLimit", 1)]
        public virtual BigInteger MaxStakeLimit { get; set; }
        [Parameter("uint256", "_stakeLimitIncreasePerBlock", 2)]
        public virtual BigInteger StakeLimitIncreasePerBlock { get; set; }
    }

    public partial class SetWithdrawalCredentialsFunction : SetWithdrawalCredentialsFunctionBase { }

    [Function("setWithdrawalCredentials")]
    public class SetWithdrawalCredentialsFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "_withdrawalCredentials", 1)]
        public virtual byte[] WithdrawalCredentials { get; set; }
    }

    public partial class StopFunction : StopFunctionBase { }

    [Function("stop")]
    public class StopFunctionBase : FunctionMessage
    {

    }

    public partial class SubmitFunction : SubmitFunctionBase { }

    [Function("submit", "uint256")]
    public class SubmitFunctionBase : FunctionMessage
    {
        [Parameter("address", "_referral", 1)]
        public virtual string Referral { get; set; }
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

    public partial class ELRewardsReceivedEventDTO : ELRewardsReceivedEventDTOBase { }

    [Event("ELRewardsReceived")]
    public class ELRewardsReceivedEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "amount", 1, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class ELRewardsVaultSetEventDTO : ELRewardsVaultSetEventDTOBase { }

    [Event("ELRewardsVaultSet")]
    public class ELRewardsVaultSetEventDTOBase : IEventDTO
    {
        [Parameter("address", "executionLayerRewardsVault", 1, false )]
        public virtual string ExecutionLayerRewardsVault { get; set; }
    }

    public partial class ELRewardsWithdrawalLimitSetEventDTO : ELRewardsWithdrawalLimitSetEventDTOBase { }

    [Event("ELRewardsWithdrawalLimitSet")]
    public class ELRewardsWithdrawalLimitSetEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "limitPoints", 1, false )]
        public virtual BigInteger LimitPoints { get; set; }
    }

    public partial class FeeDistributionSetEventDTO : FeeDistributionSetEventDTOBase { }

    [Event("FeeDistributionSet")]
    public class FeeDistributionSetEventDTOBase : IEventDTO
    {
        [Parameter("uint16", "treasuryFeeBasisPoints", 1, false )]
        public virtual ushort TreasuryFeeBasisPoints { get; set; }
        [Parameter("uint16", "insuranceFeeBasisPoints", 2, false )]
        public virtual ushort InsuranceFeeBasisPoints { get; set; }
        [Parameter("uint16", "operatorsFeeBasisPoints", 3, false )]
        public virtual ushort OperatorsFeeBasisPoints { get; set; }
    }

    public partial class FeeSetEventDTO : FeeSetEventDTOBase { }

    [Event("FeeSet")]
    public class FeeSetEventDTOBase : IEventDTO
    {
        [Parameter("uint16", "feeBasisPoints", 1, false )]
        public virtual ushort FeeBasisPoints { get; set; }
    }

    public partial class ProtocolContactsSetEventDTO : ProtocolContactsSetEventDTOBase { }

    [Event("ProtocolContactsSet")]
    public class ProtocolContactsSetEventDTOBase : IEventDTO
    {
        [Parameter("address", "oracle", 1, false )]
        public virtual string Oracle { get; set; }
        [Parameter("address", "treasury", 2, false )]
        public virtual string Treasury { get; set; }
        [Parameter("address", "insuranceFund", 3, false )]
        public virtual string InsuranceFund { get; set; }
    }





    public partial class StakingLimitSetEventDTO : StakingLimitSetEventDTOBase { }

    [Event("StakingLimitSet")]
    public class StakingLimitSetEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "maxStakeLimit", 1, false )]
        public virtual BigInteger MaxStakeLimit { get; set; }
        [Parameter("uint256", "stakeLimitIncreasePerBlock", 2, false )]
        public virtual BigInteger StakeLimitIncreasePerBlock { get; set; }
    }







    public partial class SubmittedEventDTO : SubmittedEventDTOBase { }

    [Event("Submitted")]
    public class SubmittedEventDTOBase : IEventDTO
    {
        [Parameter("address", "sender", 1, true )]
        public virtual string Sender { get; set; }
        [Parameter("uint256", "amount", 2, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("address", "referral", 3, false )]
        public virtual string Referral { get; set; }
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

    public partial class UnbufferedEventDTO : UnbufferedEventDTOBase { }

    [Event("Unbuffered")]
    public class UnbufferedEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "amount", 1, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class WithdrawalEventDTO : WithdrawalEventDTOBase { }

    [Event("Withdrawal")]
    public class WithdrawalEventDTOBase : IEventDTO
    {
        [Parameter("address", "sender", 1, true )]
        public virtual string Sender { get; set; }
        [Parameter("uint256", "tokenAmount", 2, false )]
        public virtual BigInteger TokenAmount { get; set; }
        [Parameter("uint256", "sentFromBuffer", 3, false )]
        public virtual BigInteger SentFromBuffer { get; set; }
        [Parameter("bytes32", "pubkeyHash", 4, true )]
        public virtual byte[] PubkeyHash { get; set; }
        [Parameter("uint256", "etherAmount", 5, false )]
        public virtual BigInteger EtherAmount { get; set; }
    }

    public partial class WithdrawalCredentialsSetEventDTO : WithdrawalCredentialsSetEventDTOBase { }

    [Event("WithdrawalCredentialsSet")]
    public class WithdrawalCredentialsSetEventDTOBase : IEventDTO
    {
        [Parameter("bytes32", "withdrawalCredentials", 1, false )]
        public virtual byte[] WithdrawalCredentials { get; set; }
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

    public partial class GetBeaconStatOutputDTO : GetBeaconStatOutputDTOBase { }

    [FunctionOutput]
    public class GetBeaconStatOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "depositedValidators", 1)]
        public virtual BigInteger DepositedValidators { get; set; }
        [Parameter("uint256", "beaconValidators", 2)]
        public virtual BigInteger BeaconValidators { get; set; }
        [Parameter("uint256", "beaconBalance", 3)]
        public virtual BigInteger BeaconBalance { get; set; }
    }

    public partial class GetBufferedEtherOutputDTO : GetBufferedEtherOutputDTOBase { }

    [FunctionOutput]
    public class GetBufferedEtherOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetCurrentStakeLimitOutputDTO : GetCurrentStakeLimitOutputDTOBase { }

    [FunctionOutput]
    public class GetCurrentStakeLimitOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetFeeOutputDTO : GetFeeOutputDTOBase { }

    [FunctionOutput]
    public class GetFeeOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint16", "feeBasisPoints", 1)]
        public virtual ushort FeeBasisPoints { get; set; }
    }

    public partial class GetFeeDistributionOutputDTO : GetFeeDistributionOutputDTOBase { }

    [FunctionOutput]
    public class GetFeeDistributionOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint16", "treasuryFeeBasisPoints", 1)]
        public virtual ushort TreasuryFeeBasisPoints { get; set; }
        [Parameter("uint16", "insuranceFeeBasisPoints", 2)]
        public virtual ushort InsuranceFeeBasisPoints { get; set; }
        [Parameter("uint16", "operatorsFeeBasisPoints", 3)]
        public virtual ushort OperatorsFeeBasisPoints { get; set; }
    }

    public partial class GetPooledEthBySharesOutputDTO : GetPooledEthBySharesOutputDTOBase { }

    [FunctionOutput]
    public class GetPooledEthBySharesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetSharesByPooledEthOutputDTO : GetSharesByPooledEthOutputDTOBase { }

    [FunctionOutput]
    public class GetSharesByPooledEthOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetStakeLimitFullInfoOutputDTO : GetStakeLimitFullInfoOutputDTOBase { }

    [FunctionOutput]
    public class GetStakeLimitFullInfoOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "isStakingPaused", 1)]
        public virtual bool IsStakingPaused { get; set; }
        [Parameter("bool", "isStakingLimitSet", 2)]
        public virtual bool IsStakingLimitSet { get; set; }
        [Parameter("uint256", "currentStakeLimit", 3)]
        public virtual BigInteger CurrentStakeLimit { get; set; }
        [Parameter("uint256", "maxStakeLimit", 4)]
        public virtual BigInteger MaxStakeLimit { get; set; }
        [Parameter("uint256", "maxStakeLimitGrowthBlocks", 5)]
        public virtual BigInteger MaxStakeLimitGrowthBlocks { get; set; }
        [Parameter("uint256", "prevStakeLimit", 6)]
        public virtual BigInteger PrevStakeLimit { get; set; }
        [Parameter("uint256", "prevStakeBlockNumber", 7)]
        public virtual BigInteger PrevStakeBlockNumber { get; set; }
    }

    public partial class GetTotalPooledEtherOutputDTO : GetTotalPooledEtherOutputDTOBase { }

    [FunctionOutput]
    public class GetTotalPooledEtherOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetTotalSharesOutputDTO : GetTotalSharesOutputDTOBase { }

    [FunctionOutput]
    public class GetTotalSharesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetWithdrawalCredentialsOutputDTO : GetWithdrawalCredentialsOutputDTOBase { }

    [FunctionOutput]
    public class GetWithdrawalCredentialsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }



    public partial class IsStakingPausedOutputDTO : IsStakingPausedOutputDTOBase { }

    [FunctionOutput]
    public class IsStakingPausedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }





























    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }




}
