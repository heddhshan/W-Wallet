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
using Nethereum.Uniswap.IQuoter.ContractDefinition;

namespace Nethereum.Uniswap.IQuoter
{
    public partial class IQuoterService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IQuoterDeployment iQuoterDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IQuoterDeployment>().SendRequestAndWaitForReceiptAsync(iQuoterDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IQuoterDeployment iQuoterDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IQuoterDeployment>().SendRequestAsync(iQuoterDeployment);
        }

        public static async Task<IQuoterService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IQuoterDeployment iQuoterDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iQuoterDeployment, cancellationTokenSource);
            return new IQuoterService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IQuoterService(Nethereum.Web3.Web3 web3, string contractAddress)
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

        public Task<BigInteger> QuoteExactInputSingleQueryAsync(QuoteExactInputSingleFunction quoteExactInputSingleFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<QuoteExactInputSingleFunction, BigInteger>(quoteExactInputSingleFunction, blockParameter);
        }

        
        public Task<BigInteger> QuoteExactInputSingleQueryAsync(string tokenIn, string tokenOut, uint fee, BigInteger amountIn, BigInteger sqrtPriceLimitX96, BlockParameter blockParameter = null)
        {
            var quoteExactInputSingleFunction = new QuoteExactInputSingleFunction();
                quoteExactInputSingleFunction.TokenIn = tokenIn;
                quoteExactInputSingleFunction.TokenOut = tokenOut;
                quoteExactInputSingleFunction.Fee = fee;
                quoteExactInputSingleFunction.AmountIn = amountIn;
                quoteExactInputSingleFunction.SqrtPriceLimitX96 = sqrtPriceLimitX96;
            
            return ContractHandler.QueryAsync<QuoteExactInputSingleFunction, BigInteger>(quoteExactInputSingleFunction, blockParameter);
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

        public Task<string> QuoteExactOutputSingleRequestAsync(string tokenIn, string tokenOut, uint fee, BigInteger amountOut, BigInteger sqrtPriceLimitX96)
        {
            var quoteExactOutputSingleFunction = new QuoteExactOutputSingleFunction();
                quoteExactOutputSingleFunction.TokenIn = tokenIn;
                quoteExactOutputSingleFunction.TokenOut = tokenOut;
                quoteExactOutputSingleFunction.Fee = fee;
                quoteExactOutputSingleFunction.AmountOut = amountOut;
                quoteExactOutputSingleFunction.SqrtPriceLimitX96 = sqrtPriceLimitX96;
            
             return ContractHandler.SendRequestAsync(quoteExactOutputSingleFunction);
        }

        public Task<TransactionReceipt> QuoteExactOutputSingleRequestAndWaitForReceiptAsync(string tokenIn, string tokenOut, uint fee, BigInteger amountOut, BigInteger sqrtPriceLimitX96, CancellationTokenSource cancellationToken = null)
        {
            var quoteExactOutputSingleFunction = new QuoteExactOutputSingleFunction();
                quoteExactOutputSingleFunction.TokenIn = tokenIn;
                quoteExactOutputSingleFunction.TokenOut = tokenOut;
                quoteExactOutputSingleFunction.Fee = fee;
                quoteExactOutputSingleFunction.AmountOut = amountOut;
                quoteExactOutputSingleFunction.SqrtPriceLimitX96 = sqrtPriceLimitX96;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(quoteExactOutputSingleFunction, cancellationToken);
        }
    }
}
