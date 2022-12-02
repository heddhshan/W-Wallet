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
using BlockChain.Wallet.Contract.Test.ContractDefinition;

namespace BlockChain.Wallet.Contract.Test
{
    public partial class TestService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TestDeployment testDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TestDeployment>().SendRequestAndWaitForReceiptAsync(testDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, TestDeployment testDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TestDeployment>().SendRequestAsync(testDeployment);
        }

        public static async Task<TestService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, TestDeployment testDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, testDeployment, cancellationTokenSource);
            return new TestService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public TestService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<byte[]> GetroundresultEncodeQueryAsync(GetroundresultEncodeFunction getroundresultEncodeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetroundresultEncodeFunction, byte[]>(getroundresultEncodeFunction, blockParameter);
        }

        
        public Task<byte[]> GetroundresultEncodeQueryAsync(BigInteger resultNum, string game, byte[] clientNonce, BlockParameter blockParameter = null)
        {
            var getroundresultEncodeFunction = new GetroundresultEncodeFunction();
                getroundresultEncodeFunction.ResultNum = resultNum;
                getroundresultEncodeFunction.Game = game;
                getroundresultEncodeFunction.ClientNonce = clientNonce;
            
            return ContractHandler.QueryAsync<GetroundresultEncodeFunction, byte[]>(getroundresultEncodeFunction, blockParameter);
        }

        public Task<byte[]> GetroundresultEncodepackedQueryAsync(GetroundresultEncodepackedFunction getroundresultEncodepackedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetroundresultEncodepackedFunction, byte[]>(getroundresultEncodepackedFunction, blockParameter);
        }

        
        public Task<byte[]> GetroundresultEncodepackedQueryAsync(BigInteger resultNum, string game, byte[] clientNonce, BlockParameter blockParameter = null)
        {
            var getroundresultEncodepackedFunction = new GetroundresultEncodepackedFunction();
                getroundresultEncodepackedFunction.ResultNum = resultNum;
                getroundresultEncodepackedFunction.Game = game;
                getroundresultEncodepackedFunction.ClientNonce = clientNonce;
            
            return ContractHandler.QueryAsync<GetroundresultEncodepackedFunction, byte[]>(getroundresultEncodepackedFunction, blockParameter);
        }

        public Task<byte[]> GetroundresultEncodepackedHashQueryAsync(GetroundresultEncodepackedHashFunction getroundresultEncodepackedHashFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetroundresultEncodepackedHashFunction, byte[]>(getroundresultEncodepackedHashFunction, blockParameter);
        }

        
        public Task<byte[]> GetroundresultEncodepackedHashQueryAsync(BigInteger resultNum, string game, byte[] clientNonce, BlockParameter blockParameter = null)
        {
            var getroundresultEncodepackedHashFunction = new GetroundresultEncodepackedHashFunction();
                getroundresultEncodepackedHashFunction.ResultNum = resultNum;
                getroundresultEncodepackedHashFunction.Game = game;
                getroundresultEncodepackedHashFunction.ClientNonce = clientNonce;
            
            return ContractHandler.QueryAsync<GetroundresultEncodepackedHashFunction, byte[]>(getroundresultEncodepackedHashFunction, blockParameter);
        }
    }
}
