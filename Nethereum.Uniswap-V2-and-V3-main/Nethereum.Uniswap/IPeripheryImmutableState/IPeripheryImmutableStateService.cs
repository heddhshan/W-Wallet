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
using Nethereum.Uniswap.IPeripheryImmutableStateIPeripheryImmutableState.ContractDefinition;

namespace Nethereum.Uniswap.IPeripheryImmutableStateIPeripheryImmutableState
{
    public partial class IPeripheryImmutableStateService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IPeripheryImmutableStateDeployment iPeripheryImmutableStateDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IPeripheryImmutableStateDeployment>().SendRequestAndWaitForReceiptAsync(iPeripheryImmutableStateDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IPeripheryImmutableStateDeployment iPeripheryImmutableStateDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IPeripheryImmutableStateDeployment>().SendRequestAsync(iPeripheryImmutableStateDeployment);
        }

        public static async Task<IPeripheryImmutableStateService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IPeripheryImmutableStateDeployment iPeripheryImmutableStateDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iPeripheryImmutableStateDeployment, cancellationTokenSource);
            return new IPeripheryImmutableStateService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IPeripheryImmutableStateService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> Weth9QueryAsync(Weth9Function weth9Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<Weth9Function, string>(weth9Function, blockParameter);
        }

        
        public Task<string> Weth9QueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<Weth9Function, string>(null, blockParameter);
        }

        public Task<string> FactoryQueryAsync(FactoryFunction factoryFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FactoryFunction, string>(factoryFunction, blockParameter);
        }

        
        public Task<string> FactoryQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FactoryFunction, string>(null, blockParameter);
        }
    }
}
