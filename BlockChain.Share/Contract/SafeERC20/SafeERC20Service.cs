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
using BlockChain.Share.Contract.SafeERC20.ContractDefinition;

namespace BlockChain.Share.Contract.SafeERC20
{
    public partial class SafeERC20Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, SafeERC20Deployment safeERC20Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<SafeERC20Deployment>().SendRequestAndWaitForReceiptAsync(safeERC20Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, SafeERC20Deployment safeERC20Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<SafeERC20Deployment>().SendRequestAsync(safeERC20Deployment);
        }

        public static async Task<SafeERC20Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, SafeERC20Deployment safeERC20Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, safeERC20Deployment, cancellationTokenSource);
            return new SafeERC20Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public SafeERC20Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
