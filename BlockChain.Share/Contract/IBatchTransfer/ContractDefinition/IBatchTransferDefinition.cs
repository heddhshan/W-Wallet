using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace BlockChain.Share.Contract.IBatchTransfer.ContractDefinition
{


    public partial class IBatchTransferDeployment : IBatchTransferDeploymentBase
    {
        public IBatchTransferDeployment() : base(BYTECODE) { }
        public IBatchTransferDeployment(string byteCode) : base(byteCode) { }
    }

    public class IBatchTransferDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public IBatchTransferDeploymentBase() : base(BYTECODE) { }
        public IBatchTransferDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class BatchTokenTransfer1Function : BatchTokenTransfer1FunctionBase { }

    [Function("batchTokenTransfer1")]
    public class BatchTokenTransfer1FunctionBase : FunctionMessage
    {
        [Parameter("address", "_erc20Token", 1)]
        public virtual string Erc20Token { get; set; }
        [Parameter("address[]", "_tos", 2)]
        public virtual List<string> Tos { get; set; }
        [Parameter("uint256", "_amount", 3)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "_batchId", 4)]
        public virtual BigInteger BatchId { get; set; }
        [Parameter("bool", "_isShowSuccess", 5)]
        public virtual bool IsShowSuccess { get; set; }
    }

    public partial class BatchTokenTransfer2Function : BatchTokenTransfer2FunctionBase { }

    [Function("batchTokenTransfer2")]
    public class BatchTokenTransfer2FunctionBase : FunctionMessage
    {
        [Parameter("address", "_erc20Token", 1)]
        public virtual string Erc20Token { get; set; }
        [Parameter("address[]", "_tos", 2)]
        public virtual List<string> Tos { get; set; }
        [Parameter("uint256[]", "_amounts", 3)]
        public virtual List<BigInteger> Amounts { get; set; }
        [Parameter("uint256", "_batchId", 4)]
        public virtual BigInteger BatchId { get; set; }
        [Parameter("bool", "_isShowSuccess", 5)]
        public virtual bool IsShowSuccess { get; set; }
    }

    public partial class BatchTransfer1Function : BatchTransfer1FunctionBase { }

    [Function("batchTransfer1")]
    public class BatchTransfer1FunctionBase : FunctionMessage
    {
        [Parameter("address[]", "_tos", 1)]
        public virtual List<string> Tos { get; set; }
        [Parameter("uint256", "_amount", 2)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "_batchId", 3)]
        public virtual BigInteger BatchId { get; set; }
        [Parameter("bool", "_isShowSuccess", 4)]
        public virtual bool IsShowSuccess { get; set; }
    }

    public partial class BatchTransfer2Function : BatchTransfer2FunctionBase { }

    [Function("batchTransfer2")]
    public class BatchTransfer2FunctionBase : FunctionMessage
    {
        [Parameter("address[]", "_tos", 1)]
        public virtual List<string> Tos { get; set; }
        [Parameter("uint256[]", "_amounts", 2)]
        public virtual List<BigInteger> Amounts { get; set; }
        [Parameter("uint256", "_batchId", 3)]
        public virtual BigInteger BatchId { get; set; }
        [Parameter("bool", "_isShowSuccess", 4)]
        public virtual bool IsShowSuccess { get; set; }
    }

    public partial class TokenTransferFunction : TokenTransferFunctionBase { }

    [Function("tokenTransfer")]
    public class TokenTransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "_erc20Token", 1)]
        public virtual string Erc20Token { get; set; }
        [Parameter("address", "_to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "_amount", 3)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("bool", "_isShowSuccess", 4)]
        public virtual bool IsShowSuccess { get; set; }
    }










}
