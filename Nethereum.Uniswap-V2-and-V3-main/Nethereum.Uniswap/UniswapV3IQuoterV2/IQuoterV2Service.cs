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
using Nethereum.Uniswap.Contracts.IQuoterV2.ContractDefinition;

namespace Nethereum.Uniswap.Contracts.IQuoterV2
{
    public partial class IQuoterV2Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IQuoterV2Deployment iQuoterV2Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IQuoterV2Deployment>().SendRequestAndWaitForReceiptAsync(iQuoterV2Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IQuoterV2Deployment iQuoterV2Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IQuoterV2Deployment>().SendRequestAsync(iQuoterV2Deployment);
        }

        public static async Task<IQuoterV2Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IQuoterV2Deployment iQuoterV2Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iQuoterV2Deployment, cancellationTokenSource);
            return new IQuoterV2Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IQuoterV2Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> QuoteExactInputRequestAsync(QuoteExactInputFunction quoteExactInputFunction)
        {
             return ContractHandler.SendRequestAsync(quoteExactInputFunction);
        }

        public Task<TransactionReceipt> QuoteExactInputRequestAndWaitForReceiptAsync(QuoteExactInputFunction quoteExactInputFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(quoteExactInputFunction, cancellationToken);
        }

        public Task<string> QuoteExactInputRequestAsync(byte[] path, BigInteger amountIn)
        {
            var quoteExactInputFunction = new QuoteExactInputFunction();
                quoteExactInputFunction.Path = path;
                quoteExactInputFunction.AmountIn = amountIn;
            
             return ContractHandler.SendRequestAsync(quoteExactInputFunction);
        }

        public Task<TransactionReceipt> QuoteExactInputRequestAndWaitForReceiptAsync(byte[] path, BigInteger amountIn, CancellationTokenSource cancellationToken = null)
        {
            var quoteExactInputFunction = new QuoteExactInputFunction();
                quoteExactInputFunction.Path = path;
                quoteExactInputFunction.AmountIn = amountIn;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(quoteExactInputFunction, cancellationToken);
        }

        public Task<string> QuoteExactInputSingleRequestAsync(QuoteExactInputSingleFunction quoteExactInputSingleFunction)
        {
             return ContractHandler.SendRequestAsync(quoteExactInputSingleFunction);
        }

        public Task<TransactionReceipt> QuoteExactInputSingleRequestAndWaitForReceiptAsync(QuoteExactInputSingleFunction quoteExactInputSingleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(quoteExactInputSingleFunction, cancellationToken);
        }

        public Task<string> QuoteExactInputSingleRequestAsync(QuoteExactInputSingleParams @params)
        {
            var quoteExactInputSingleFunction = new QuoteExactInputSingleFunction();
                quoteExactInputSingleFunction.Params = @params;
            
             return ContractHandler.SendRequestAsync(quoteExactInputSingleFunction);
        }

        public Task<TransactionReceipt> QuoteExactInputSingleRequestAndWaitForReceiptAsync(QuoteExactInputSingleParams @params, CancellationTokenSource cancellationToken = null)
        {
            var quoteExactInputSingleFunction = new QuoteExactInputSingleFunction();
                quoteExactInputSingleFunction.Params = @params;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(quoteExactInputSingleFunction, cancellationToken);
        }

        public Task<string> QuoteExactOutputRequestAsync(QuoteExactOutputFunction quoteExactOutputFunction)
        {
             return ContractHandler.SendRequestAsync(quoteExactOutputFunction);
        }

        public Task<TransactionReceipt> QuoteExactOutputRequestAndWaitForReceiptAsync(QuoteExactOutputFunction quoteExactOutputFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(quoteExactOutputFunction, cancellationToken);
        }

        public Task<string> QuoteExactOutputRequestAsync(byte[] path, BigInteger amountOut)
        {
            var quoteExactOutputFunction = new QuoteExactOutputFunction();
                quoteExactOutputFunction.Path = path;
                quoteExactOutputFunction.AmountOut = amountOut;
            
             return ContractHandler.SendRequestAsync(quoteExactOutputFunction);
        }

        public Task<TransactionReceipt> QuoteExactOutputRequestAndWaitForReceiptAsync(byte[] path, BigInteger amountOut, CancellationTokenSource cancellationToken = null)
        {
            var quoteExactOutputFunction = new QuoteExactOutputFunction();
                quoteExactOutputFunction.Path = path;
                quoteExactOutputFunction.AmountOut = amountOut;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(quoteExactOutputFunction, cancellationToken);
        }

        public Task<string> QuoteExactOutputSingleRequestAsync(QuoteExactOutputSingleFunction quoteExactOutputSingleFunction)
        {
             return ContractHandler.SendRequestAsync(quoteExactOutputSingleFunction);
        }

        public Task<TransactionReceipt> QuoteExactOutputSingleRequestAndWaitForReceiptAsync(QuoteExactOutputSingleFunction quoteExactOutputSingleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(quoteExactOutputSingleFunction, cancellationToken);
        }

        public Task<string> QuoteExactOutputSingleRequestAsync(QuoteExactOutputSingleParams @params)
        {
            var quoteExactOutputSingleFunction = new QuoteExactOutputSingleFunction();
                quoteExactOutputSingleFunction.Params = @params;
            
             return ContractHandler.SendRequestAsync(quoteExactOutputSingleFunction);
        }

        public Task<TransactionReceipt> QuoteExactOutputSingleRequestAndWaitForReceiptAsync(QuoteExactOutputSingleParams @params, CancellationTokenSource cancellationToken = null)
        {
            var quoteExactOutputSingleFunction = new QuoteExactOutputSingleFunction();
                quoteExactOutputSingleFunction.Params = @params;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(quoteExactOutputSingleFunction, cancellationToken);
        }
    }
}
