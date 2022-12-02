using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using BlockChain.Lido.Contract.ILido.ContractDefinition;

namespace BlockChain.Lido.Contract.ILido
{
    public partial class ILidoService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, ILidoDeployment iLidoDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<ILidoDeployment>().SendRequestAndWaitForReceiptAsync(iLidoDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, ILidoDeployment iLidoDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<ILidoDeployment>().SendRequestAsync(iLidoDeployment);
        }

        public static async Task<ILidoService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, ILidoDeployment iLidoDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iLidoDeployment, cancellationTokenSource);
            return new ILidoService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public ILidoService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> AllowanceQueryAsync(AllowanceFunction allowanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        
        public Task<BigInteger> AllowanceQueryAsync(string owner, string spender, BlockParameter blockParameter = null)
        {
            var allowanceFunction = new AllowanceFunction();
                allowanceFunction.Owner = owner;
                allowanceFunction.Spender = spender;
            
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        public Task<string> ApproveRequestAsync(ApproveFunction approveFunction)
        {
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(ApproveFunction approveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<string> ApproveRequestAsync(string spender, BigInteger amount)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(string spender, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Account = account;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<byte> DecimalsQueryAsync(DecimalsFunction decimalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(decimalsFunction, blockParameter);
        }

        
        public Task<byte> DecimalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(null, blockParameter);
        }

        public Task<GetBeaconStatOutputDTO> GetBeaconStatQueryAsync(GetBeaconStatFunction getBeaconStatFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetBeaconStatFunction, GetBeaconStatOutputDTO>(getBeaconStatFunction, blockParameter);
        }

        public Task<GetBeaconStatOutputDTO> GetBeaconStatQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetBeaconStatFunction, GetBeaconStatOutputDTO>(null, blockParameter);
        }

        public Task<BigInteger> GetBufferedEtherQueryAsync(GetBufferedEtherFunction getBufferedEtherFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBufferedEtherFunction, BigInteger>(getBufferedEtherFunction, blockParameter);
        }

        
        public Task<BigInteger> GetBufferedEtherQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBufferedEtherFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetCurrentStakeLimitQueryAsync(GetCurrentStakeLimitFunction getCurrentStakeLimitFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentStakeLimitFunction, BigInteger>(getCurrentStakeLimitFunction, blockParameter);
        }

        
        public Task<BigInteger> GetCurrentStakeLimitQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentStakeLimitFunction, BigInteger>(null, blockParameter);
        }

        public Task<ushort> GetFeeQueryAsync(GetFeeFunction getFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFeeFunction, ushort>(getFeeFunction, blockParameter);
        }

        
        public Task<ushort> GetFeeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFeeFunction, ushort>(null, blockParameter);
        }

        public Task<GetFeeDistributionOutputDTO> GetFeeDistributionQueryAsync(GetFeeDistributionFunction getFeeDistributionFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetFeeDistributionFunction, GetFeeDistributionOutputDTO>(getFeeDistributionFunction, blockParameter);
        }

        public Task<GetFeeDistributionOutputDTO> GetFeeDistributionQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetFeeDistributionFunction, GetFeeDistributionOutputDTO>(null, blockParameter);
        }

        public Task<BigInteger> GetPooledEthBySharesQueryAsync(GetPooledEthBySharesFunction getPooledEthBySharesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetPooledEthBySharesFunction, BigInteger>(getPooledEthBySharesFunction, blockParameter);
        }

        
        public Task<BigInteger> GetPooledEthBySharesQueryAsync(BigInteger sharesAmount, BlockParameter blockParameter = null)
        {
            var getPooledEthBySharesFunction = new GetPooledEthBySharesFunction();
                getPooledEthBySharesFunction.SharesAmount = sharesAmount;
            
            return ContractHandler.QueryAsync<GetPooledEthBySharesFunction, BigInteger>(getPooledEthBySharesFunction, blockParameter);
        }

        public Task<BigInteger> GetSharesByPooledEthQueryAsync(GetSharesByPooledEthFunction getSharesByPooledEthFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetSharesByPooledEthFunction, BigInteger>(getSharesByPooledEthFunction, blockParameter);
        }

        
        public Task<BigInteger> GetSharesByPooledEthQueryAsync(BigInteger pooledEthAmount, BlockParameter blockParameter = null)
        {
            var getSharesByPooledEthFunction = new GetSharesByPooledEthFunction();
                getSharesByPooledEthFunction.PooledEthAmount = pooledEthAmount;
            
            return ContractHandler.QueryAsync<GetSharesByPooledEthFunction, BigInteger>(getSharesByPooledEthFunction, blockParameter);
        }

        public Task<GetStakeLimitFullInfoOutputDTO> GetStakeLimitFullInfoQueryAsync(GetStakeLimitFullInfoFunction getStakeLimitFullInfoFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetStakeLimitFullInfoFunction, GetStakeLimitFullInfoOutputDTO>(getStakeLimitFullInfoFunction, blockParameter);
        }

        public Task<GetStakeLimitFullInfoOutputDTO> GetStakeLimitFullInfoQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetStakeLimitFullInfoFunction, GetStakeLimitFullInfoOutputDTO>(null, blockParameter);
        }

        public Task<BigInteger> GetTotalPooledEtherQueryAsync(GetTotalPooledEtherFunction getTotalPooledEtherFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTotalPooledEtherFunction, BigInteger>(getTotalPooledEtherFunction, blockParameter);
        }

        
        public Task<BigInteger> GetTotalPooledEtherQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTotalPooledEtherFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetTotalSharesQueryAsync(GetTotalSharesFunction getTotalSharesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTotalSharesFunction, BigInteger>(getTotalSharesFunction, blockParameter);
        }

        
        public Task<BigInteger> GetTotalSharesQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTotalSharesFunction, BigInteger>(null, blockParameter);
        }

        public Task<byte[]> GetWithdrawalCredentialsQueryAsync(GetWithdrawalCredentialsFunction getWithdrawalCredentialsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetWithdrawalCredentialsFunction, byte[]>(getWithdrawalCredentialsFunction, blockParameter);
        }

        
        public Task<byte[]> GetWithdrawalCredentialsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetWithdrawalCredentialsFunction, byte[]>(null, blockParameter);
        }

        public Task<string> HandleOracleReportRequestAsync(HandleOracleReportFunction handleOracleReportFunction)
        {
             return ContractHandler.SendRequestAsync(handleOracleReportFunction);
        }

        public Task<TransactionReceipt> HandleOracleReportRequestAndWaitForReceiptAsync(HandleOracleReportFunction handleOracleReportFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(handleOracleReportFunction, cancellationToken);
        }

        public Task<string> HandleOracleReportRequestAsync(BigInteger epoch, BigInteger eth2balance)
        {
            var handleOracleReportFunction = new HandleOracleReportFunction();
                handleOracleReportFunction.Epoch = epoch;
                handleOracleReportFunction.Eth2balance = eth2balance;
            
             return ContractHandler.SendRequestAsync(handleOracleReportFunction);
        }

        public Task<TransactionReceipt> HandleOracleReportRequestAndWaitForReceiptAsync(BigInteger epoch, BigInteger eth2balance, CancellationTokenSource cancellationToken = null)
        {
            var handleOracleReportFunction = new HandleOracleReportFunction();
                handleOracleReportFunction.Epoch = epoch;
                handleOracleReportFunction.Eth2balance = eth2balance;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(handleOracleReportFunction, cancellationToken);
        }

        public Task<bool> IsStakingPausedQueryAsync(IsStakingPausedFunction isStakingPausedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsStakingPausedFunction, bool>(isStakingPausedFunction, blockParameter);
        }

        
        public Task<bool> IsStakingPausedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsStakingPausedFunction, bool>(null, blockParameter);
        }

        public Task<string> PauseStakingRequestAsync(PauseStakingFunction pauseStakingFunction)
        {
             return ContractHandler.SendRequestAsync(pauseStakingFunction);
        }

        public Task<string> PauseStakingRequestAsync()
        {
             return ContractHandler.SendRequestAsync<PauseStakingFunction>();
        }

        public Task<TransactionReceipt> PauseStakingRequestAndWaitForReceiptAsync(PauseStakingFunction pauseStakingFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pauseStakingFunction, cancellationToken);
        }

        public Task<TransactionReceipt> PauseStakingRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<PauseStakingFunction>(null, cancellationToken);
        }

        public Task<string> ReceiveELRewardsRequestAsync(ReceiveELRewardsFunction receiveELRewardsFunction)
        {
             return ContractHandler.SendRequestAsync(receiveELRewardsFunction);
        }

        public Task<string> ReceiveELRewardsRequestAsync()
        {
             return ContractHandler.SendRequestAsync<ReceiveELRewardsFunction>();
        }

        public Task<TransactionReceipt> ReceiveELRewardsRequestAndWaitForReceiptAsync(ReceiveELRewardsFunction receiveELRewardsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(receiveELRewardsFunction, cancellationToken);
        }

        public Task<TransactionReceipt> ReceiveELRewardsRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<ReceiveELRewardsFunction>(null, cancellationToken);
        }

        public Task<string> RemoveStakingLimitRequestAsync(RemoveStakingLimitFunction removeStakingLimitFunction)
        {
             return ContractHandler.SendRequestAsync(removeStakingLimitFunction);
        }

        public Task<string> RemoveStakingLimitRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RemoveStakingLimitFunction>();
        }

        public Task<TransactionReceipt> RemoveStakingLimitRequestAndWaitForReceiptAsync(RemoveStakingLimitFunction removeStakingLimitFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeStakingLimitFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RemoveStakingLimitRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RemoveStakingLimitFunction>(null, cancellationToken);
        }

        public Task<string> ResumeRequestAsync(ResumeFunction resumeFunction)
        {
             return ContractHandler.SendRequestAsync(resumeFunction);
        }

        public Task<string> ResumeRequestAsync()
        {
             return ContractHandler.SendRequestAsync<ResumeFunction>();
        }

        public Task<TransactionReceipt> ResumeRequestAndWaitForReceiptAsync(ResumeFunction resumeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(resumeFunction, cancellationToken);
        }

        public Task<TransactionReceipt> ResumeRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<ResumeFunction>(null, cancellationToken);
        }

        public Task<string> ResumeStakingRequestAsync(ResumeStakingFunction resumeStakingFunction)
        {
             return ContractHandler.SendRequestAsync(resumeStakingFunction);
        }

        public Task<string> ResumeStakingRequestAsync()
        {
             return ContractHandler.SendRequestAsync<ResumeStakingFunction>();
        }

        public Task<TransactionReceipt> ResumeStakingRequestAndWaitForReceiptAsync(ResumeStakingFunction resumeStakingFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(resumeStakingFunction, cancellationToken);
        }

        public Task<TransactionReceipt> ResumeStakingRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<ResumeStakingFunction>(null, cancellationToken);
        }

        public Task<string> SetELRewardsVaultRequestAsync(SetELRewardsVaultFunction setELRewardsVaultFunction)
        {
             return ContractHandler.SendRequestAsync(setELRewardsVaultFunction);
        }

        public Task<TransactionReceipt> SetELRewardsVaultRequestAndWaitForReceiptAsync(SetELRewardsVaultFunction setELRewardsVaultFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setELRewardsVaultFunction, cancellationToken);
        }

        public Task<string> SetELRewardsVaultRequestAsync(string executionLayerRewardsVault)
        {
            var setELRewardsVaultFunction = new SetELRewardsVaultFunction();
                setELRewardsVaultFunction.ExecutionLayerRewardsVault = executionLayerRewardsVault;
            
             return ContractHandler.SendRequestAsync(setELRewardsVaultFunction);
        }

        public Task<TransactionReceipt> SetELRewardsVaultRequestAndWaitForReceiptAsync(string executionLayerRewardsVault, CancellationTokenSource cancellationToken = null)
        {
            var setELRewardsVaultFunction = new SetELRewardsVaultFunction();
                setELRewardsVaultFunction.ExecutionLayerRewardsVault = executionLayerRewardsVault;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setELRewardsVaultFunction, cancellationToken);
        }

        public Task<string> SetELRewardsWithdrawalLimitRequestAsync(SetELRewardsWithdrawalLimitFunction setELRewardsWithdrawalLimitFunction)
        {
             return ContractHandler.SendRequestAsync(setELRewardsWithdrawalLimitFunction);
        }

        public Task<TransactionReceipt> SetELRewardsWithdrawalLimitRequestAndWaitForReceiptAsync(SetELRewardsWithdrawalLimitFunction setELRewardsWithdrawalLimitFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setELRewardsWithdrawalLimitFunction, cancellationToken);
        }

        public Task<string> SetELRewardsWithdrawalLimitRequestAsync(ushort limitPoints)
        {
            var setELRewardsWithdrawalLimitFunction = new SetELRewardsWithdrawalLimitFunction();
                setELRewardsWithdrawalLimitFunction.LimitPoints = limitPoints;
            
             return ContractHandler.SendRequestAsync(setELRewardsWithdrawalLimitFunction);
        }

        public Task<TransactionReceipt> SetELRewardsWithdrawalLimitRequestAndWaitForReceiptAsync(ushort limitPoints, CancellationTokenSource cancellationToken = null)
        {
            var setELRewardsWithdrawalLimitFunction = new SetELRewardsWithdrawalLimitFunction();
                setELRewardsWithdrawalLimitFunction.LimitPoints = limitPoints;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setELRewardsWithdrawalLimitFunction, cancellationToken);
        }

        public Task<string> SetFeeRequestAsync(SetFeeFunction setFeeFunction)
        {
             return ContractHandler.SendRequestAsync(setFeeFunction);
        }

        public Task<TransactionReceipt> SetFeeRequestAndWaitForReceiptAsync(SetFeeFunction setFeeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeFunction, cancellationToken);
        }

        public Task<string> SetFeeRequestAsync(ushort feeBasisPoints)
        {
            var setFeeFunction = new SetFeeFunction();
                setFeeFunction.FeeBasisPoints = feeBasisPoints;
            
             return ContractHandler.SendRequestAsync(setFeeFunction);
        }

        public Task<TransactionReceipt> SetFeeRequestAndWaitForReceiptAsync(ushort feeBasisPoints, CancellationTokenSource cancellationToken = null)
        {
            var setFeeFunction = new SetFeeFunction();
                setFeeFunction.FeeBasisPoints = feeBasisPoints;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeFunction, cancellationToken);
        }

        public Task<string> SetFeeDistributionRequestAsync(SetFeeDistributionFunction setFeeDistributionFunction)
        {
             return ContractHandler.SendRequestAsync(setFeeDistributionFunction);
        }

        public Task<TransactionReceipt> SetFeeDistributionRequestAndWaitForReceiptAsync(SetFeeDistributionFunction setFeeDistributionFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeDistributionFunction, cancellationToken);
        }

        public Task<string> SetFeeDistributionRequestAsync(ushort treasuryFeeBasisPoints, ushort insuranceFeeBasisPoints, ushort operatorsFeeBasisPoints)
        {
            var setFeeDistributionFunction = new SetFeeDistributionFunction();
                setFeeDistributionFunction.TreasuryFeeBasisPoints = treasuryFeeBasisPoints;
                setFeeDistributionFunction.InsuranceFeeBasisPoints = insuranceFeeBasisPoints;
                setFeeDistributionFunction.OperatorsFeeBasisPoints = operatorsFeeBasisPoints;
            
             return ContractHandler.SendRequestAsync(setFeeDistributionFunction);
        }

        public Task<TransactionReceipt> SetFeeDistributionRequestAndWaitForReceiptAsync(ushort treasuryFeeBasisPoints, ushort insuranceFeeBasisPoints, ushort operatorsFeeBasisPoints, CancellationTokenSource cancellationToken = null)
        {
            var setFeeDistributionFunction = new SetFeeDistributionFunction();
                setFeeDistributionFunction.TreasuryFeeBasisPoints = treasuryFeeBasisPoints;
                setFeeDistributionFunction.InsuranceFeeBasisPoints = insuranceFeeBasisPoints;
                setFeeDistributionFunction.OperatorsFeeBasisPoints = operatorsFeeBasisPoints;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeDistributionFunction, cancellationToken);
        }

        public Task<string> SetProtocolContractsRequestAsync(SetProtocolContractsFunction setProtocolContractsFunction)
        {
             return ContractHandler.SendRequestAsync(setProtocolContractsFunction);
        }

        public Task<TransactionReceipt> SetProtocolContractsRequestAndWaitForReceiptAsync(SetProtocolContractsFunction setProtocolContractsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setProtocolContractsFunction, cancellationToken);
        }

        public Task<string> SetProtocolContractsRequestAsync(string oracle, string treasury, string insuranceFund)
        {
            var setProtocolContractsFunction = new SetProtocolContractsFunction();
                setProtocolContractsFunction.Oracle = oracle;
                setProtocolContractsFunction.Treasury = treasury;
                setProtocolContractsFunction.InsuranceFund = insuranceFund;
            
             return ContractHandler.SendRequestAsync(setProtocolContractsFunction);
        }

        public Task<TransactionReceipt> SetProtocolContractsRequestAndWaitForReceiptAsync(string oracle, string treasury, string insuranceFund, CancellationTokenSource cancellationToken = null)
        {
            var setProtocolContractsFunction = new SetProtocolContractsFunction();
                setProtocolContractsFunction.Oracle = oracle;
                setProtocolContractsFunction.Treasury = treasury;
                setProtocolContractsFunction.InsuranceFund = insuranceFund;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setProtocolContractsFunction, cancellationToken);
        }

        public Task<string> SetStakingLimitRequestAsync(SetStakingLimitFunction setStakingLimitFunction)
        {
             return ContractHandler.SendRequestAsync(setStakingLimitFunction);
        }

        public Task<TransactionReceipt> SetStakingLimitRequestAndWaitForReceiptAsync(SetStakingLimitFunction setStakingLimitFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setStakingLimitFunction, cancellationToken);
        }

        public Task<string> SetStakingLimitRequestAsync(BigInteger maxStakeLimit, BigInteger stakeLimitIncreasePerBlock)
        {
            var setStakingLimitFunction = new SetStakingLimitFunction();
                setStakingLimitFunction.MaxStakeLimit = maxStakeLimit;
                setStakingLimitFunction.StakeLimitIncreasePerBlock = stakeLimitIncreasePerBlock;
            
             return ContractHandler.SendRequestAsync(setStakingLimitFunction);
        }

        public Task<TransactionReceipt> SetStakingLimitRequestAndWaitForReceiptAsync(BigInteger maxStakeLimit, BigInteger stakeLimitIncreasePerBlock, CancellationTokenSource cancellationToken = null)
        {
            var setStakingLimitFunction = new SetStakingLimitFunction();
                setStakingLimitFunction.MaxStakeLimit = maxStakeLimit;
                setStakingLimitFunction.StakeLimitIncreasePerBlock = stakeLimitIncreasePerBlock;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setStakingLimitFunction, cancellationToken);
        }

        public Task<string> SetWithdrawalCredentialsRequestAsync(SetWithdrawalCredentialsFunction setWithdrawalCredentialsFunction)
        {
             return ContractHandler.SendRequestAsync(setWithdrawalCredentialsFunction);
        }

        public Task<TransactionReceipt> SetWithdrawalCredentialsRequestAndWaitForReceiptAsync(SetWithdrawalCredentialsFunction setWithdrawalCredentialsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setWithdrawalCredentialsFunction, cancellationToken);
        }

        public Task<string> SetWithdrawalCredentialsRequestAsync(byte[] withdrawalCredentials)
        {
            var setWithdrawalCredentialsFunction = new SetWithdrawalCredentialsFunction();
                setWithdrawalCredentialsFunction.WithdrawalCredentials = withdrawalCredentials;
            
             return ContractHandler.SendRequestAsync(setWithdrawalCredentialsFunction);
        }

        public Task<TransactionReceipt> SetWithdrawalCredentialsRequestAndWaitForReceiptAsync(byte[] withdrawalCredentials, CancellationTokenSource cancellationToken = null)
        {
            var setWithdrawalCredentialsFunction = new SetWithdrawalCredentialsFunction();
                setWithdrawalCredentialsFunction.WithdrawalCredentials = withdrawalCredentials;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setWithdrawalCredentialsFunction, cancellationToken);
        }

        public Task<string> StopRequestAsync(StopFunction stopFunction)
        {
             return ContractHandler.SendRequestAsync(stopFunction);
        }

        public Task<string> StopRequestAsync()
        {
             return ContractHandler.SendRequestAsync<StopFunction>();
        }

        public Task<TransactionReceipt> StopRequestAndWaitForReceiptAsync(StopFunction stopFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stopFunction, cancellationToken);
        }

        public Task<TransactionReceipt> StopRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<StopFunction>(null, cancellationToken);
        }

        public Task<string> SubmitRequestAsync(SubmitFunction submitFunction)
        {
             return ContractHandler.SendRequestAsync(submitFunction);
        }

        public Task<TransactionReceipt> SubmitRequestAndWaitForReceiptAsync(SubmitFunction submitFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(submitFunction, cancellationToken);
        }

        public Task<string> SubmitRequestAsync(string referral)
        {
            var submitFunction = new SubmitFunction();
                submitFunction.Referral = referral;
            
             return ContractHandler.SendRequestAsync(submitFunction);
        }

        public Task<TransactionReceipt> SubmitRequestAndWaitForReceiptAsync(string referral, CancellationTokenSource cancellationToken = null)
        {
            var submitFunction = new SubmitFunction();
                submitFunction.Referral = referral;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(submitFunction, cancellationToken);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> TransferRequestAsync(TransferFunction transferFunction)
        {
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferRequestAsync(string recipient, BigInteger amount)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(TransferFromFunction transferFromFunction)
        {
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(TransferFromFunction transferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(string sender, string recipient, BigInteger amount)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(string sender, string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }
    }
}
