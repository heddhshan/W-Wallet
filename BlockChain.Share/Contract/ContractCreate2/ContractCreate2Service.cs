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
using BlockChain.Share.Contract.ContractCreate2.ContractDefinition;

namespace BlockChain.Share.Contract.ContractCreate2
{
    public partial class ContractCreate2Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, ContractCreate2Deployment contractCreate2Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<ContractCreate2Deployment>().SendRequestAndWaitForReceiptAsync(contractCreate2Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, ContractCreate2Deployment contractCreate2Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<ContractCreate2Deployment>().SendRequestAsync(contractCreate2Deployment);
        }

        public static async Task<ContractCreate2Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, ContractCreate2Deployment contractCreate2Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, contractCreate2Deployment, cancellationTokenSource);
            return new ContractCreate2Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public ContractCreate2Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> ComputeContractAddress1QueryAsync(ComputeContractAddress1Function computeContractAddress1Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ComputeContractAddress1Function, string>(computeContractAddress1Function, blockParameter);
        }

        
        public Task<string> ComputeContractAddress1QueryAsync(byte[] salt, byte[] bytecodeHash, BlockParameter blockParameter = null)
        {
            var computeContractAddress1Function = new ComputeContractAddress1Function();
                computeContractAddress1Function.Salt = salt;
                computeContractAddress1Function.BytecodeHash = bytecodeHash;
            
            return ContractHandler.QueryAsync<ComputeContractAddress1Function, string>(computeContractAddress1Function, blockParameter);
        }

        public Task<string> ComputeContractAddress2QueryAsync(ComputeContractAddress2Function computeContractAddress2Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ComputeContractAddress2Function, string>(computeContractAddress2Function, blockParameter);
        }

        
        public Task<string> ComputeContractAddress2QueryAsync(byte[] salt, byte[] bytecodeHash, string deployer, BlockParameter blockParameter = null)
        {
            var computeContractAddress2Function = new ComputeContractAddress2Function();
                computeContractAddress2Function.Salt = salt;
                computeContractAddress2Function.BytecodeHash = bytecodeHash;
                computeContractAddress2Function.Deployer = deployer;
            
            return ContractHandler.QueryAsync<ComputeContractAddress2Function, string>(computeContractAddress2Function, blockParameter);
        }

        public Task<string> DeployContractRequestAsync(DeployContractFunction deployContractFunction)
        {
             return ContractHandler.SendRequestAsync(deployContractFunction);
        }

        public Task<TransactionReceipt> DeployContractRequestAndWaitForReceiptAsync(DeployContractFunction deployContractFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deployContractFunction, cancellationToken);
        }

        public Task<string> DeployContractRequestAsync(byte[] salt, byte[] bytecode)
        {
            var deployContractFunction = new DeployContractFunction();
                deployContractFunction.Salt = salt;
                deployContractFunction.Bytecode = bytecode;
            
             return ContractHandler.SendRequestAsync(deployContractFunction);
        }

        public Task<TransactionReceipt> DeployContractRequestAndWaitForReceiptAsync(byte[] salt, byte[] bytecode, CancellationTokenSource cancellationToken = null)
        {
            var deployContractFunction = new DeployContractFunction();
                deployContractFunction.Salt = salt;
                deployContractFunction.Bytecode = bytecode;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deployContractFunction, cancellationToken);
        }
    }
}
