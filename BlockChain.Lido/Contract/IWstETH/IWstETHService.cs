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
using BlockChain.Lido.Contract.IWstETH.ContractDefinition;

namespace BlockChain.Lido.Contract.IWstETH
{
    public partial class IWstETHService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IWstETHDeployment iWstETHDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IWstETHDeployment>().SendRequestAndWaitForReceiptAsync(iWstETHDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IWstETHDeployment iWstETHDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IWstETHDeployment>().SendRequestAsync(iWstETHDeployment);
        }

        public static async Task<IWstETHService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IWstETHDeployment iWstETHDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iWstETHDeployment, cancellationTokenSource);
            return new IWstETHService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IWstETHService(Nethereum.Web3.Web3 web3, string contractAddress)
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

        public Task<BigInteger> GetStETHByWstETHQueryAsync(GetStETHByWstETHFunction getStETHByWstETHFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetStETHByWstETHFunction, BigInteger>(getStETHByWstETHFunction, blockParameter);
        }

        
        public Task<BigInteger> GetStETHByWstETHQueryAsync(BigInteger wstETHAmount, BlockParameter blockParameter = null)
        {
            var getStETHByWstETHFunction = new GetStETHByWstETHFunction();
                getStETHByWstETHFunction.WstETHAmount = wstETHAmount;
            
            return ContractHandler.QueryAsync<GetStETHByWstETHFunction, BigInteger>(getStETHByWstETHFunction, blockParameter);
        }

        public Task<BigInteger> GetWstETHByStETHQueryAsync(GetWstETHByStETHFunction getWstETHByStETHFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetWstETHByStETHFunction, BigInteger>(getWstETHByStETHFunction, blockParameter);
        }

        
        public Task<BigInteger> GetWstETHByStETHQueryAsync(BigInteger stETHAmount, BlockParameter blockParameter = null)
        {
            var getWstETHByStETHFunction = new GetWstETHByStETHFunction();
                getWstETHByStETHFunction.StETHAmount = stETHAmount;
            
            return ContractHandler.QueryAsync<GetWstETHByStETHFunction, BigInteger>(getWstETHByStETHFunction, blockParameter);
        }

        public Task<BigInteger> StEthPerTokenQueryAsync(StEthPerTokenFunction stEthPerTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StEthPerTokenFunction, BigInteger>(stEthPerTokenFunction, blockParameter);
        }

        
        public Task<BigInteger> StEthPerTokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StEthPerTokenFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> TokensPerStEthQueryAsync(TokensPerStEthFunction tokensPerStEthFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokensPerStEthFunction, BigInteger>(tokensPerStEthFunction, blockParameter);
        }

        
        public Task<BigInteger> TokensPerStEthQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokensPerStEthFunction, BigInteger>(null, blockParameter);
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

        public Task<string> UnwrapRequestAsync(UnwrapFunction unwrapFunction)
        {
             return ContractHandler.SendRequestAsync(unwrapFunction);
        }

        public Task<TransactionReceipt> UnwrapRequestAndWaitForReceiptAsync(UnwrapFunction unwrapFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unwrapFunction, cancellationToken);
        }

        public Task<string> UnwrapRequestAsync(BigInteger wstETHAmount)
        {
            var unwrapFunction = new UnwrapFunction();
                unwrapFunction.WstETHAmount = wstETHAmount;
            
             return ContractHandler.SendRequestAsync(unwrapFunction);
        }

        public Task<TransactionReceipt> UnwrapRequestAndWaitForReceiptAsync(BigInteger wstETHAmount, CancellationTokenSource cancellationToken = null)
        {
            var unwrapFunction = new UnwrapFunction();
                unwrapFunction.WstETHAmount = wstETHAmount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unwrapFunction, cancellationToken);
        }

        public Task<string> WrapRequestAsync(WrapFunction wrapFunction)
        {
             return ContractHandler.SendRequestAsync(wrapFunction);
        }

        public Task<TransactionReceipt> WrapRequestAndWaitForReceiptAsync(WrapFunction wrapFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(wrapFunction, cancellationToken);
        }

        public Task<string> WrapRequestAsync(BigInteger stETHAmount)
        {
            var wrapFunction = new WrapFunction();
                wrapFunction.StETHAmount = stETHAmount;
            
             return ContractHandler.SendRequestAsync(wrapFunction);
        }

        public Task<TransactionReceipt> WrapRequestAndWaitForReceiptAsync(BigInteger stETHAmount, CancellationTokenSource cancellationToken = null)
        {
            var wrapFunction = new WrapFunction();
                wrapFunction.StETHAmount = stETHAmount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(wrapFunction, cancellationToken);
        }
    }
}
