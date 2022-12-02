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
using BlockChain.Wallet.Contract.WalletHelper.ContractDefinition;

namespace BlockChain.Wallet.Contract.WalletHelper
{
    public partial class WalletHelperService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, WalletHelperDeployment walletHelperDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<WalletHelperDeployment>().SendRequestAndWaitForReceiptAsync(walletHelperDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, WalletHelperDeployment walletHelperDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<WalletHelperDeployment>().SendRequestAsync(walletHelperDeployment);
        }

        public static async Task<WalletHelperService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, WalletHelperDeployment walletHelperDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, walletHelperDeployment, cancellationTokenSource);
            return new WalletHelperService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public WalletHelperService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<byte[]> GetBytes32HashQueryAsync(GetBytes32HashFunction getBytes32HashFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBytes32HashFunction, byte[]>(getBytes32HashFunction, blockParameter);
        }

        
        public Task<byte[]> GetBytes32HashQueryAsync(byte[] data, BlockParameter blockParameter = null)
        {
            var getBytes32HashFunction = new GetBytes32HashFunction();
                getBytes32HashFunction.Data = data;
            
            return ContractHandler.QueryAsync<GetBytes32HashFunction, byte[]>(getBytes32HashFunction, blockParameter);
        }

        public Task<byte[]> GetBytesHashQueryAsync(GetBytesHashFunction getBytesHashFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBytesHashFunction, byte[]>(getBytesHashFunction, blockParameter);
        }

        
        public Task<byte[]> GetBytesHashQueryAsync(byte[] bytecode, BlockParameter blockParameter = null)
        {
            var getBytesHashFunction = new GetBytesHashFunction();
                getBytesHashFunction.Bytecode = bytecode;
            
            return ContractHandler.QueryAsync<GetBytesHashFunction, byte[]>(getBytesHashFunction, blockParameter);
        }

        public Task<byte[]> GetEthSignedMessageHashQueryAsync(GetEthSignedMessageHashFunction getEthSignedMessageHashFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetEthSignedMessageHashFunction, byte[]>(getEthSignedMessageHashFunction, blockParameter);
        }

        
        public Task<byte[]> GetEthSignedMessageHashQueryAsync(byte[] hash, BlockParameter blockParameter = null)
        {
            var getEthSignedMessageHashFunction = new GetEthSignedMessageHashFunction();
                getEthSignedMessageHashFunction.Hash = hash;
            
            return ContractHandler.QueryAsync<GetEthSignedMessageHashFunction, byte[]>(getEthSignedMessageHashFunction, blockParameter);
        }

        public Task<GetRSVOutputDTO> GetRSVQueryAsync(GetRSVFunction getRSVFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetRSVFunction, GetRSVOutputDTO>(getRSVFunction, blockParameter);
        }

        public Task<GetRSVOutputDTO> GetRSVQueryAsync(byte[] h, byte[] sig, BlockParameter blockParameter = null)
        {
            var getRSVFunction = new GetRSVFunction();
                getRSVFunction.H = h;
                getRSVFunction.Sig = sig;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetRSVFunction, GetRSVOutputDTO>(getRSVFunction, blockParameter);
        }

        public Task<bool> IsContractQueryAsync(IsContractFunction isContractFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsContractFunction, bool>(isContractFunction, blockParameter);
        }

        
        public Task<bool> IsContractQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isContractFunction = new IsContractFunction();
                isContractFunction.Account = account;
            
            return ContractHandler.QueryAsync<IsContractFunction, bool>(isContractFunction, blockParameter);
        }

        public Task<string> ToHexStrin2QueryAsync(ToHexStrin2Function toHexStrin2Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ToHexStrin2Function, string>(toHexStrin2Function, blockParameter);
        }

        
        public Task<string> ToHexStrin2QueryAsync(BigInteger value, BigInteger length, BlockParameter blockParameter = null)
        {
            var toHexStrin2Function = new ToHexStrin2Function();
                toHexStrin2Function.Value = value;
                toHexStrin2Function.Length = length;
            
            return ContractHandler.QueryAsync<ToHexStrin2Function, string>(toHexStrin2Function, blockParameter);
        }

        public Task<string> ToHexString1QueryAsync(ToHexString1Function toHexString1Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ToHexString1Function, string>(toHexString1Function, blockParameter);
        }

        
        public Task<string> ToHexString1QueryAsync(BigInteger value, BlockParameter blockParameter = null)
        {
            var toHexString1Function = new ToHexString1Function();
                toHexString1Function.Value = value;
            
            return ContractHandler.QueryAsync<ToHexString1Function, string>(toHexString1Function, blockParameter);
        }

        public Task<string> ToStringQueryAsync(ToStringFunction toStringFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ToStringFunction, string>(toStringFunction, blockParameter);
        }

        
        public Task<string> ToStringQueryAsync(BigInteger value, BlockParameter blockParameter = null)
        {
            var toStringFunction = new ToStringFunction();
                toStringFunction.Value = value;
            
            return ContractHandler.QueryAsync<ToStringFunction, string>(toStringFunction, blockParameter);
        }

        public Task<string> BatchTokenTransfer1RequestAsync(BatchTokenTransfer1Function batchTokenTransfer1Function)
        {
             return ContractHandler.SendRequestAsync(batchTokenTransfer1Function);
        }

        public Task<TransactionReceipt> BatchTokenTransfer1RequestAndWaitForReceiptAsync(BatchTokenTransfer1Function batchTokenTransfer1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTokenTransfer1Function, cancellationToken);
        }

        public Task<string> BatchTokenTransfer1RequestAsync(string erc20Token, List<string> tos, BigInteger amount, BigInteger batchId, bool isShowSuccess)
        {
            var batchTokenTransfer1Function = new BatchTokenTransfer1Function();
                batchTokenTransfer1Function.Erc20Token = erc20Token;
                batchTokenTransfer1Function.Tos = tos;
                batchTokenTransfer1Function.Amount = amount;
                batchTokenTransfer1Function.BatchId = batchId;
                batchTokenTransfer1Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(batchTokenTransfer1Function);
        }

        public Task<TransactionReceipt> BatchTokenTransfer1RequestAndWaitForReceiptAsync(string erc20Token, List<string> tos, BigInteger amount, BigInteger batchId, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var batchTokenTransfer1Function = new BatchTokenTransfer1Function();
                batchTokenTransfer1Function.Erc20Token = erc20Token;
                batchTokenTransfer1Function.Tos = tos;
                batchTokenTransfer1Function.Amount = amount;
                batchTokenTransfer1Function.BatchId = batchId;
                batchTokenTransfer1Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTokenTransfer1Function, cancellationToken);
        }

        public Task<string> BatchTokenTransfer2RequestAsync(BatchTokenTransfer2Function batchTokenTransfer2Function)
        {
             return ContractHandler.SendRequestAsync(batchTokenTransfer2Function);
        }

        public Task<TransactionReceipt> BatchTokenTransfer2RequestAndWaitForReceiptAsync(BatchTokenTransfer2Function batchTokenTransfer2Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTokenTransfer2Function, cancellationToken);
        }

        public Task<string> BatchTokenTransfer2RequestAsync(string erc20Token, List<string> tos, List<BigInteger> amounts, BigInteger batchId, bool isShowSuccess)
        {
            var batchTokenTransfer2Function = new BatchTokenTransfer2Function();
                batchTokenTransfer2Function.Erc20Token = erc20Token;
                batchTokenTransfer2Function.Tos = tos;
                batchTokenTransfer2Function.Amounts = amounts;
                batchTokenTransfer2Function.BatchId = batchId;
                batchTokenTransfer2Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(batchTokenTransfer2Function);
        }

        public Task<TransactionReceipt> BatchTokenTransfer2RequestAndWaitForReceiptAsync(string erc20Token, List<string> tos, List<BigInteger> amounts, BigInteger batchId, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var batchTokenTransfer2Function = new BatchTokenTransfer2Function();
                batchTokenTransfer2Function.Erc20Token = erc20Token;
                batchTokenTransfer2Function.Tos = tos;
                batchTokenTransfer2Function.Amounts = amounts;
                batchTokenTransfer2Function.BatchId = batchId;
                batchTokenTransfer2Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTokenTransfer2Function, cancellationToken);
        }

        public Task<string> BatchTransfer1RequestAsync(BatchTransfer1Function batchTransfer1Function)
        {
             return ContractHandler.SendRequestAsync(batchTransfer1Function);
        }

        public Task<TransactionReceipt> BatchTransfer1RequestAndWaitForReceiptAsync(BatchTransfer1Function batchTransfer1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTransfer1Function, cancellationToken);
        }

        public Task<string> BatchTransfer1RequestAsync(List<string> tos, BigInteger amount, BigInteger batchId, bool isShowSuccess)
        {
            var batchTransfer1Function = new BatchTransfer1Function();
                batchTransfer1Function.Tos = tos;
                batchTransfer1Function.Amount = amount;
                batchTransfer1Function.BatchId = batchId;
                batchTransfer1Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(batchTransfer1Function);
        }

        public Task<TransactionReceipt> BatchTransfer1RequestAndWaitForReceiptAsync(List<string> tos, BigInteger amount, BigInteger batchId, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var batchTransfer1Function = new BatchTransfer1Function();
                batchTransfer1Function.Tos = tos;
                batchTransfer1Function.Amount = amount;
                batchTransfer1Function.BatchId = batchId;
                batchTransfer1Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTransfer1Function, cancellationToken);
        }

        public Task<string> BatchTransfer2RequestAsync(BatchTransfer2Function batchTransfer2Function)
        {
             return ContractHandler.SendRequestAsync(batchTransfer2Function);
        }

        public Task<TransactionReceipt> BatchTransfer2RequestAndWaitForReceiptAsync(BatchTransfer2Function batchTransfer2Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTransfer2Function, cancellationToken);
        }

        public Task<string> BatchTransfer2RequestAsync(List<string> tos, List<BigInteger> amounts, BigInteger batchId, bool isShowSuccess)
        {
            var batchTransfer2Function = new BatchTransfer2Function();
                batchTransfer2Function.Tos = tos;
                batchTransfer2Function.Amounts = amounts;
                batchTransfer2Function.BatchId = batchId;
                batchTransfer2Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(batchTransfer2Function);
        }

        public Task<TransactionReceipt> BatchTransfer2RequestAndWaitForReceiptAsync(List<string> tos, List<BigInteger> amounts, BigInteger batchId, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var batchTransfer2Function = new BatchTransfer2Function();
                batchTransfer2Function.Tos = tos;
                batchTransfer2Function.Amounts = amounts;
                batchTransfer2Function.BatchId = batchId;
                batchTransfer2Function.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(batchTransfer2Function, cancellationToken);
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

        public Task<string> GetAddress1QueryAsync(GetAddress1Function getAddress1Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetAddress1Function, string>(getAddress1Function, blockParameter);
        }

        
        public Task<string> GetAddress1QueryAsync(byte[] h, byte[] r, byte[] s, byte v, BlockParameter blockParameter = null)
        {
            var getAddress1Function = new GetAddress1Function();
                getAddress1Function.H = h;
                getAddress1Function.R = r;
                getAddress1Function.S = s;
                getAddress1Function.V = v;
            
            return ContractHandler.QueryAsync<GetAddress1Function, string>(getAddress1Function, blockParameter);
        }

        public Task<string> GetAddress2QueryAsync(GetAddress2Function getAddress2Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetAddress2Function, string>(getAddress2Function, blockParameter);
        }

        
        public Task<string> GetAddress2QueryAsync(byte[] h, byte[] sig, BlockParameter blockParameter = null)
        {
            var getAddress2Function = new GetAddress2Function();
                getAddress2Function.H = h;
                getAddress2Function.Sig = sig;
            
            return ContractHandler.QueryAsync<GetAddress2Function, string>(getAddress2Function, blockParameter);
        }

        public Task<string> TokenTransferRequestAsync(TokenTransferFunction tokenTransferFunction)
        {
             return ContractHandler.SendRequestAsync(tokenTransferFunction);
        }

        public Task<TransactionReceipt> TokenTransferRequestAndWaitForReceiptAsync(TokenTransferFunction tokenTransferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tokenTransferFunction, cancellationToken);
        }

        public Task<string> TokenTransferRequestAsync(string erc20Token, string to, BigInteger amount, bool isShowSuccess)
        {
            var tokenTransferFunction = new TokenTransferFunction();
                tokenTransferFunction.Erc20Token = erc20Token;
                tokenTransferFunction.To = to;
                tokenTransferFunction.Amount = amount;
                tokenTransferFunction.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAsync(tokenTransferFunction);
        }

        public Task<TransactionReceipt> TokenTransferRequestAndWaitForReceiptAsync(string erc20Token, string to, BigInteger amount, bool isShowSuccess, CancellationTokenSource cancellationToken = null)
        {
            var tokenTransferFunction = new TokenTransferFunction();
                tokenTransferFunction.Erc20Token = erc20Token;
                tokenTransferFunction.To = to;
                tokenTransferFunction.Amount = amount;
                tokenTransferFunction.IsShowSuccess = isShowSuccess;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tokenTransferFunction, cancellationToken);
        }
    }
}
