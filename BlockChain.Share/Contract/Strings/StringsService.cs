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
using BlockChain.Share.Contract.Strings.ContractDefinition;

namespace BlockChain.Share.Contract.Strings
{
    public partial class StringsService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, StringsDeployment stringsDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<StringsDeployment>().SendRequestAndWaitForReceiptAsync(stringsDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, StringsDeployment stringsDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<StringsDeployment>().SendRequestAsync(stringsDeployment);
        }

        public static async Task<StringsService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, StringsDeployment stringsDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, stringsDeployment, cancellationTokenSource);
            return new StringsService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public StringsService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
