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
using BlockChain.Lido.Contract.IStETH.ContractDefinition;

namespace BlockChain.Lido.Contract.IStETH
{
    public partial class IStETHService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IStETHDeployment iStETHDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IStETHDeployment>().SendRequestAndWaitForReceiptAsync(iStETHDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IStETHDeployment iStETHDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IStETHDeployment>().SendRequestAsync(iStETHDeployment);
        }

        public static async Task<IStETHService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IStETHDeployment iStETHDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iStETHDeployment, cancellationTokenSource);
            return new IStETHService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IStETHService(Nethereum.Web3.Web3 web3, string contractAddress)
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
