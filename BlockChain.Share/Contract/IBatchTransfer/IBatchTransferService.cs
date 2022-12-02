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
using BlockChain.Share.Contract.IBatchTransfer.ContractDefinition;

namespace BlockChain.Share.Contract.IBatchTransfer
{
    public partial class IBatchTransferService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IBatchTransferDeployment iBatchTransferDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IBatchTransferDeployment>().SendRequestAndWaitForReceiptAsync(iBatchTransferDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IBatchTransferDeployment iBatchTransferDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IBatchTransferDeployment>().SendRequestAsync(iBatchTransferDeployment);
        }

        public static async Task<IBatchTransferService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IBatchTransferDeployment iBatchTransferDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iBatchTransferDeployment, cancellationTokenSource);
            return new IBatchTransferService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IBatchTransferService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> BatchTokenTransfer1RequestAsync(BatchTokenTransfer1Function batchTokenTransfer1Function)
        {
             return ContractHandler.SendRequestAsync(batchTokenTransfer1Function);
        }

        public Task<TransactionReceipt> BatchTokenTransfer1RequestAndWaitForReceiptAsync(BatchTokenTransfer1Function batchTokenTransfer1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTokenTransfer1Function, cancellationToken);
        }

        public Task<string> BatchTokenTransfer1RequestAsync(string erc20Token, List<string> tos, BigInteger amount, BigInteger batchId, bool isShowSuccess)
        {
            var batchTokenTransfer1Function = new BatchTokenTransfer1Function();
                batchTokenTransfer1Function.Erc20Token = erc20Token;
                batchTokenTransfer1Function.Tos = tos;
                batchTokenTransfer1Function.Amount = amount;
                batchTokenTransfer1Function.BatchId = batchId;
                batchTokenTransfer1Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(batchTokenTransfer1Function);
        }

        public Task<TransactionReceipt> BatchTokenTransfer1RequestAndWaitForReceiptAsync(string erc20Token, List<string> tos, BigInteger amount, BigInteger batchId, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var batchTokenTransfer1Function = new BatchTokenTransfer1Function();
                batchTokenTransfer1Function.Erc20Token = erc20Token;
                batchTokenTransfer1Function.Tos = tos;
                batchTokenTransfer1Function.Amount = amount;
                batchTokenTransfer1Function.BatchId = batchId;
                batchTokenTransfer1Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTokenTransfer1Function, cancellationToken);
        }

        public Task<string> BatchTokenTransfer2RequestAsync(BatchTokenTransfer2Function batchTokenTransfer2Function)
        {
             return ContractHandler.SendRequestAsync(batchTokenTransfer2Function);
        }

        public Task<TransactionReceipt> BatchTokenTransfer2RequestAndWaitForReceiptAsync(BatchTokenTransfer2Function batchTokenTransfer2Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTokenTransfer2Function, cancellationToken);
        }

        public Task<string> BatchTokenTransfer2RequestAsync(string erc20Token, List<string> tos, List<BigInteger> amounts, BigInteger batchId, bool isShowSuccess)
        {
            var batchTokenTransfer2Function = new BatchTokenTransfer2Function();
                batchTokenTransfer2Function.Erc20Token = erc20Token;
                batchTokenTransfer2Function.Tos = tos;
                batchTokenTransfer2Function.Amounts = amounts;
                batchTokenTransfer2Function.BatchId = batchId;
                batchTokenTransfer2Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(batchTokenTransfer2Function);
        }

        public Task<TransactionReceipt> BatchTokenTransfer2RequestAndWaitForReceiptAsync(string erc20Token, List<string> tos, List<BigInteger> amounts, BigInteger batchId, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var batchTokenTransfer2Function = new BatchTokenTransfer2Function();
                batchTokenTransfer2Function.Erc20Token = erc20Token;
                batchTokenTransfer2Function.Tos = tos;
                batchTokenTransfer2Function.Amounts = amounts;
                batchTokenTransfer2Function.BatchId = batchId;
                batchTokenTransfer2Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTokenTransfer2Function, cancellationToken);
        }

        public Task<string> BatchTransfer1RequestAsync(BatchTransfer1Function batchTransfer1Function)
        {
             return ContractHandler.SendRequestAsync(batchTransfer1Function);
        }

        public Task<TransactionReceipt> BatchTransfer1RequestAndWaitForReceiptAsync(BatchTransfer1Function batchTransfer1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTransfer1Function, cancellationToken);
        }

        public Task<string> BatchTransfer1RequestAsync(List<string> tos, BigInteger amount, BigInteger batchId, bool isShowSuccess)
        {
            var batchTransfer1Function = new BatchTransfer1Function();
                batchTransfer1Function.Tos = tos;
                batchTransfer1Function.Amount = amount;
                batchTransfer1Function.BatchId = batchId;
                batchTransfer1Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(batchTransfer1Function);
        }

        public Task<TransactionReceipt> BatchTransfer1RequestAndWaitForReceiptAsync(List<string> tos, BigInteger amount, BigInteger batchId, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var batchTransfer1Function = new BatchTransfer1Function();
                batchTransfer1Function.Tos = tos;
                batchTransfer1Function.Amount = amount;
                batchTransfer1Function.BatchId = batchId;
                batchTransfer1Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTransfer1Function, cancellationToken);
        }

        public Task<string> BatchTransfer2RequestAsync(BatchTransfer2Function batchTransfer2Function)
        {
             return ContractHandler.SendRequestAsync(batchTransfer2Function);
        }

        public Task<TransactionReceipt> BatchTransfer2RequestAndWaitForReceiptAsync(BatchTransfer2Function batchTransfer2Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTransfer2Function, cancellationToken);
        }

        public Task<string> BatchTransfer2RequestAsync(List<string> tos, List<BigInteger> amounts, BigInteger batchId, bool isShowSuccess)
        {
            var batchTransfer2Function = new BatchTransfer2Function();
                batchTransfer2Function.Tos = tos;
                batchTransfer2Function.Amounts = amounts;
                batchTransfer2Function.BatchId = batchId;
                batchTransfer2Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(batchTransfer2Function);
        }

        public Task<TransactionReceipt> BatchTransfer2RequestAndWaitForReceiptAsync(List<string> tos, List<BigInteger> amounts, BigInteger batchId, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var batchTransfer2Function = new BatchTransfer2Function();
                batchTransfer2Function.Tos = tos;
                batchTransfer2Function.Amounts = amounts;
                batchTransfer2Function.BatchId = batchId;
                batchTransfer2Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTransfer2Function, cancellationToken);
        }

        public Task<string> TokenTransferRequestAsync(TokenTransferFunction tokenTransferFunction)
        {
             return ContractHandler.SendRequestAsync(tokenTransferFunction);
        }

        public Task<TransactionReceipt> TokenTransferRequestAndWaitForReceiptAsync(TokenTransferFunction tokenTransferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tokenTransferFunction, cancellationToken);
        }

        public Task<string> TokenTransferRequestAsync(string erc20Token, string to, BigInteger amount, bool isShowSuccess)
        {
            var tokenTransferFunction = new TokenTransferFunction();
                tokenTransferFunction.Erc20Token = erc20Token;
                tokenTransferFunction.To = to;
                tokenTransferFunction.Amount = amount;
                tokenTransferFunction.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(tokenTransferFunction);
        }

        public Task<TransactionReceipt> TokenTransferRequestAndWaitForReceiptAsync(string erc20Token, string to, BigInteger amount, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var tokenTransferFunction = new TokenTransferFunction();
                tokenTransferFunction.Erc20Token = erc20Token;
                tokenTransferFunction.To = to;
                tokenTransferFunction.Amount = amount;
                tokenTransferFunction.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tokenTransferFunction, cancellationToken);
        }
    }
}
