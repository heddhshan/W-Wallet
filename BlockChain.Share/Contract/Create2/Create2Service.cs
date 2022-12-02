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
using BlockChain.Share.Contract.Create2.ContractDefinition;

namespace BlockChain.Share.Contract.Create2
{
    public partial class Create2Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, Create2Deployment create2Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<Create2Deployment>().SendRequestAndWaitForReceiptAsync(create2Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, Create2Deployment create2Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<Create2Deployment>().SendRequestAsync(create2Deployment);
        }

        public static async Task<Create2Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, Create2Deployment create2Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, create2Deployment, cancellationTokenSource);
            return new Create2Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public Create2Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
