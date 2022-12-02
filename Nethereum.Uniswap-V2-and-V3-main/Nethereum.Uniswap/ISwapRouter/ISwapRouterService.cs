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
using Nethereum.Uniswap.ISwapRouter.ContractDefinition;

namespace Nethereum.Uniswap.ISwapRouter
{
    public partial class ISwapRouterService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, ISwapRouterDeployment iSwapRouterDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<ISwapRouterDeployment>().SendRequestAndWaitForReceiptAsync(iSwapRouterDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, ISwapRouterDeployment iSwapRouterDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<ISwapRouterDeployment>().SendRequestAsync(iSwapRouterDeployment);
        }

        public static async Task<ISwapRouterService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, ISwapRouterDeployment iSwapRouterDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iSwapRouterDeployment, cancellationTokenSource);
            return new ISwapRouterService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public ISwapRouterService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> ExactInputRequestAsync(ExactInputFunction exactInputFunction)
        {
             return ContractHandler.SendRequestAsync(exactInputFunction);
        }

        public Task<TransactionReceipt> ExactInputRequestAndWaitForReceiptAsync(ExactInputFunction exactInputFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exactInputFunction, cancellationToken);
        }

        public Task<string> ExactInputRequestAsync(ExactInputParams @params)
        {
            var exactInputFunction = new ExactInputFunction();
                exactInputFunction.Params = @params;
            
             return ContractHandler.SendRequestAsync(exactInputFunction);
        }

        public Task<TransactionReceipt> ExactInputRequestAndWaitForReceiptAsync(ExactInputParams @params, CancellationTokenSource cancellationToken = null)
        {
            var exactInputFunction = new ExactInputFunction();
                exactInputFunction.Params = @params;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exactInputFunction, cancellationToken);
        }

        public Task<string> ExactInputSingleRequestAsync(ExactInputSingleFunction exactInputSingleFunction)
        {
             return ContractHandler.SendRequestAsync(exactInputSingleFunction);
        }

        public Task<TransactionReceipt> ExactInputSingleRequestAndWaitForReceiptAsync(ExactInputSingleFunction exactInputSingleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exactInputSingleFunction, cancellationToken);
        }

        public Task<string> ExactInputSingleRequestAsync(ExactInputSingleParams @params)
        {
            var exactInputSingleFunction = new ExactInputSingleFunction();
                exactInputSingleFunction.Params = @params;
            
             return ContractHandler.SendRequestAsync(exactInputSingleFunction);
        }

        public Task<TransactionReceipt> ExactInputSingleRequestAndWaitForReceiptAsync(ExactInputSingleParams @params, CancellationTokenSource cancellationToken = null)
        {
            var exactInputSingleFunction = new ExactInputSingleFunction();
                exactInputSingleFunction.Params = @params;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exactInputSingleFunction, cancellationToken);
        }

        public Task<string> ExactOutputRequestAsync(ExactOutputFunction exactOutputFunction)
        {
             return ContractHandler.SendRequestAsync(exactOutputFunction);
        }

        public Task<TransactionReceipt> ExactOutputRequestAndWaitForReceiptAsync(ExactOutputFunction exactOutputFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exactOutputFunction, cancellationToken);
        }

        public Task<string> ExactOutputRequestAsync(ExactOutputParams @params)
        {
            var exactOutputFunction = new ExactOutputFunction();
                exactOutputFunction.Params = @params;
            
             return ContractHandler.SendRequestAsync(exactOutputFunction);
        }

        public Task<TransactionReceipt> ExactOutputRequestAndWaitForReceiptAsync(ExactOutputParams @params, CancellationTokenSource cancellationToken = null)
        {
            var exactOutputFunction = new ExactOutputFunction();
                exactOutputFunction.Params = @params;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exactOutputFunction, cancellationToken);
        }

        public Task<string> ExactOutputSingleRequestAsync(ExactOutputSingleFunction exactOutputSingleFunction)
        {
             return ContractHandler.SendRequestAsync(exactOutputSingleFunction);
        }

        public Task<TransactionReceipt> ExactOutputSingleRequestAndWaitForReceiptAsync(ExactOutputSingleFunction exactOutputSingleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exactOutputSingleFunction, cancellationToken);
        }

        public Task<string> ExactOutputSingleRequestAsync(ExactOutputSingleParams @params)
        {
            var exactOutputSingleFunction = new ExactOutputSingleFunction();
                exactOutputSingleFunction.Params = @params;
            
             return ContractHandler.SendRequestAsync(exactOutputSingleFunction);
        }

        public Task<TransactionReceipt> ExactOutputSingleRequestAndWaitForReceiptAsync(ExactOutputSingleParams @params, CancellationTokenSource cancellationToken = null)
        {
            var exactOutputSingleFunction = new ExactOutputSingleFunction();
                exactOutputSingleFunction.Params = @params;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exactOutputSingleFunction, cancellationToken);
        }

        public Task<string> UniswapV3SwapCallbackRequestAsync(UniswapV3SwapCallbackFunction uniswapV3SwapCallbackFunction)
        {
             return ContractHandler.SendRequestAsync(uniswapV3SwapCallbackFunction);
        }

        public Task<TransactionReceipt> UniswapV3SwapCallbackRequestAndWaitForReceiptAsync(UniswapV3SwapCallbackFunction uniswapV3SwapCallbackFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(uniswapV3SwapCallbackFunction, cancellationToken);
        }

        public Task<string> UniswapV3SwapCallbackRequestAsync(BigInteger amount0Delta, BigInteger amount1Delta, byte[] data)
        {
            var uniswapV3SwapCallbackFunction = new UniswapV3SwapCallbackFunction();
                uniswapV3SwapCallbackFunction.Amount0Delta = amount0Delta;
                uniswapV3SwapCallbackFunction.Amount1Delta = amount1Delta;
                uniswapV3SwapCallbackFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(uniswapV3SwapCallbackFunction);
        }

        public Task<TransactionReceipt> UniswapV3SwapCallbackRequestAndWaitForReceiptAsync(BigInteger amount0Delta, BigInteger amount1Delta, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var uniswapV3SwapCallbackFunction = new UniswapV3SwapCallbackFunction();
                uniswapV3SwapCallbackFunction.Amount0Delta = amount0Delta;
                uniswapV3SwapCallbackFunction.Amount1Delta = amount1Delta;
                uniswapV3SwapCallbackFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(uniswapV3SwapCallbackFunction, cancellationToken);
        }
    }
}
