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
using BlockChain.Share.Contract.Multicall3.ContractDefinition;

namespace BlockChain.Share.Contract.Multicall3
{
    public partial class Multicall3Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, Multicall3Deployment multicall3Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<Multicall3Deployment>().SendRequestAndWaitForReceiptAsync(multicall3Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, Multicall3Deployment multicall3Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<Multicall3Deployment>().SendRequestAsync(multicall3Deployment);
        }

        public static async Task<Multicall3Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, Multicall3Deployment multicall3Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, multicall3Deployment, cancellationTokenSource);
            return new Multicall3Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public Multicall3Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AggregateRequestAsync(AggregateFunction aggregateFunction)
        {
             return ContractHandler.SendRequestAsync(aggregateFunction);
        }

        public Task<TransactionReceipt> AggregateRequestAndWaitForReceiptAsync(AggregateFunction aggregateFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(aggregateFunction, cancellationToken);
        }

        public Task<string> AggregateRequestAsync(List<Call> calls)
        {
            var aggregateFunction = new AggregateFunction();
                aggregateFunction.Calls = calls;
            
             return ContractHandler.SendRequestAsync(aggregateFunction);
        }

        public Task<TransactionReceipt> AggregateRequestAndWaitForReceiptAsync(List<Call> calls, CancellationTokenSource cancellationToken = null)
        {
            var aggregateFunction = new AggregateFunction();
                aggregateFunction.Calls = calls;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(aggregateFunction, cancellationToken);
        }

        public Task<string> Aggregate3RequestAsync(Aggregate3Function aggregate3Function)
        {
             return ContractHandler.SendRequestAsync(aggregate3Function);
        }

        public Task<TransactionReceipt> Aggregate3RequestAndWaitForReceiptAsync(Aggregate3Function aggregate3Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(aggregate3Function, cancellationToken);
        }

        public Task<string> Aggregate3RequestAsync(List<Call3> calls)
        {
            var aggregate3Function = new Aggregate3Function();
                aggregate3Function.Calls = calls;
            
             return ContractHandler.SendRequestAsync(aggregate3Function);
        }

        public Task<TransactionReceipt> Aggregate3RequestAndWaitForReceiptAsync(List<Call3> calls, CancellationTokenSource cancellationToken = null)
        {
            var aggregate3Function = new Aggregate3Function();
                aggregate3Function.Calls = calls;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(aggregate3Function, cancellationToken);
        }

        public Task<string> Aggregate3ValueRequestAsync(Aggregate3ValueFunction aggregate3ValueFunction)
        {
             return ContractHandler.SendRequestAsync(aggregate3ValueFunction);
        }

        public Task<TransactionReceipt> Aggregate3ValueRequestAndWaitForReceiptAsync(Aggregate3ValueFunction aggregate3ValueFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(aggregate3ValueFunction, cancellationToken);
        }

        public Task<string> Aggregate3ValueRequestAsync(List<Call3Value> calls)
        {
            var aggregate3ValueFunction = new Aggregate3ValueFunction();
                aggregate3ValueFunction.Calls = calls;
            
             return ContractHandler.SendRequestAsync(aggregate3ValueFunction);
        }

        public Task<TransactionReceipt> Aggregate3ValueRequestAndWaitForReceiptAsync(List<Call3Value> calls, CancellationTokenSource cancellationToken = null)
        {
            var aggregate3ValueFunction = new Aggregate3ValueFunction();
                aggregate3ValueFunction.Calls = calls;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(aggregate3ValueFunction, cancellationToken);
        }

        public Task<string> BlockAndAggregateRequestAsync(BlockAndAggregateFunction blockAndAggregateFunction)
        {
             return ContractHandler.SendRequestAsync(blockAndAggregateFunction);
        }

        public Task<TransactionReceipt> BlockAndAggregateRequestAndWaitForReceiptAsync(BlockAndAggregateFunction blockAndAggregateFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(blockAndAggregateFunction, cancellationToken);
        }

        public Task<string> BlockAndAggregateRequestAsync(List<Call> calls)
        {
            var blockAndAggregateFunction = new BlockAndAggregateFunction();
                blockAndAggregateFunction.Calls = calls;
            
             return ContractHandler.SendRequestAsync(blockAndAggregateFunction);
        }

        public Task<TransactionReceipt> BlockAndAggregateRequestAndWaitForReceiptAsync(List<Call> calls, CancellationTokenSource cancellationToken = null)
        {
            var blockAndAggregateFunction = new BlockAndAggregateFunction();
                blockAndAggregateFunction.Calls = calls;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(blockAndAggregateFunction, cancellationToken);
        }

        public Task<BigInteger> GetBasefeeQueryAsync(GetBasefeeFunction getBasefeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBasefeeFunction, BigInteger>(getBasefeeFunction, blockParameter);
        }

        
        public Task<BigInteger> GetBasefeeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBasefeeFunction, BigInteger>(null, blockParameter);
        }

        public Task<byte[]> GetBlockHashQueryAsync(GetBlockHashFunction getBlockHashFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBlockHashFunction, byte[]>(getBlockHashFunction, blockParameter);
        }

        
        public Task<byte[]> GetBlockHashQueryAsync(BigInteger blockNumber, BlockParameter blockParameter = null)
        {
            var getBlockHashFunction = new GetBlockHashFunction();
                getBlockHashFunction.BlockNumber = blockNumber;
            
            return ContractHandler.QueryAsync<GetBlockHashFunction, byte[]>(getBlockHashFunction, blockParameter);
        }

        public Task<BigInteger> GetBlockNumberQueryAsync(GetBlockNumberFunction getBlockNumberFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBlockNumberFunction, BigInteger>(getBlockNumberFunction, blockParameter);
        }

        
        public Task<BigInteger> GetBlockNumberQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBlockNumberFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetChainIdQueryAsync(GetChainIdFunction getChainIdFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetChainIdFunction, BigInteger>(getChainIdFunction, blockParameter);
        }

        
        public Task<BigInteger> GetChainIdQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetChainIdFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> GetCurrentBlockCoinbaseQueryAsync(GetCurrentBlockCoinbaseFunction getCurrentBlockCoinbaseFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockCoinbaseFunction, string>(getCurrentBlockCoinbaseFunction, blockParameter);
        }

        
        public Task<string> GetCurrentBlockCoinbaseQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockCoinbaseFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> GetCurrentBlockDifficultyQueryAsync(GetCurrentBlockDifficultyFunction getCurrentBlockDifficultyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockDifficultyFunction, BigInteger>(getCurrentBlockDifficultyFunction, blockParameter);
        }

        
        public Task<BigInteger> GetCurrentBlockDifficultyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockDifficultyFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetCurrentBlockGasLimitQueryAsync(GetCurrentBlockGasLimitFunction getCurrentBlockGasLimitFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockGasLimitFunction, BigInteger>(getCurrentBlockGasLimitFunction, blockParameter);
        }

        
        public Task<BigInteger> GetCurrentBlockGasLimitQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockGasLimitFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetCurrentBlockTimestampQueryAsync(GetCurrentBlockTimestampFunction getCurrentBlockTimestampFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockTimestampFunction, BigInteger>(getCurrentBlockTimestampFunction, blockParameter);
        }

        
        public Task<BigInteger> GetCurrentBlockTimestampQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockTimestampFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetEthBalanceQueryAsync(GetEthBalanceFunction getEthBalanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetEthBalanceFunction, BigInteger>(getEthBalanceFunction, blockParameter);
        }

        
        public Task<BigInteger> GetEthBalanceQueryAsync(string addr, BlockParameter blockParameter = null)
        {
            var getEthBalanceFunction = new GetEthBalanceFunction();
                getEthBalanceFunction.Addr = addr;
            
            return ContractHandler.QueryAsync<GetEthBalanceFunction, BigInteger>(getEthBalanceFunction, blockParameter);
        }

        public Task<byte[]> GetLastBlockHashQueryAsync(GetLastBlockHashFunction getLastBlockHashFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetLastBlockHashFunction, byte[]>(getLastBlockHashFunction, blockParameter);
        }

        
        public Task<byte[]> GetLastBlockHashQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetLastBlockHashFunction, byte[]>(null, blockParameter);
        }

        public Task<string> TryAggregateRequestAsync(TryAggregateFunction tryAggregateFunction)
        {
             return ContractHandler.SendRequestAsync(tryAggregateFunction);
        }

        public Task<TransactionReceipt> TryAggregateRequestAndWaitForReceiptAsync(TryAggregateFunction tryAggregateFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tryAggregateFunction, cancellationToken);
        }

        public Task<string> TryAggregateRequestAsync(bool requireSuccess, List<Call> calls)
        {
            var tryAggregateFunction = new TryAggregateFunction();
                tryAggregateFunction.RequireSuccess = requireSuccess;
                tryAggregateFunction.Calls = calls;
            
             return ContractHandler.SendRequestAsync(tryAggregateFunction);
        }

        public Task<TransactionReceipt> TryAggregateRequestAndWaitForReceiptAsync(bool requireSuccess, List<Call> calls, CancellationTokenSource cancellationToken = null)
        {
            var tryAggregateFunction = new TryAggregateFunction();
                tryAggregateFunction.RequireSuccess = requireSuccess;
                tryAggregateFunction.Calls = calls;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tryAggregateFunction, cancellationToken);
        }

        public Task<string> TryBlockAndAggregateRequestAsync(TryBlockAndAggregateFunction tryBlockAndAggregateFunction)
        {
             return ContractHandler.SendRequestAsync(tryBlockAndAggregateFunction);
        }

        public Task<TransactionReceipt> TryBlockAndAggregateRequestAndWaitForReceiptAsync(TryBlockAndAggregateFunction tryBlockAndAggregateFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tryBlockAndAggregateFunction, cancellationToken);
        }

        public Task<string> TryBlockAndAggregateRequestAsync(bool requireSuccess, List<Call> calls)
        {
            var tryBlockAndAggregateFunction = new TryBlockAndAggregateFunction();
                tryBlockAndAggregateFunction.RequireSuccess = requireSuccess;
                tryBlockAndAggregateFunction.Calls = calls;
            
             return ContractHandler.SendRequestAsync(tryBlockAndAggregateFunction);
        }

        public Task<TransactionReceipt> TryBlockAndAggregateRequestAndWaitForReceiptAsync(bool requireSuccess, List<Call> calls, CancellationTokenSource cancellationToken = null)
        {
            var tryBlockAndAggregateFunction = new TryBlockAndAggregateFunction();
                tryBlockAndAggregateFunction.RequireSuccess = requireSuccess;
                tryBlockAndAggregateFunction.Calls = calls;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tryBlockAndAggregateFunction, cancellationToken);
        }
    }
}
