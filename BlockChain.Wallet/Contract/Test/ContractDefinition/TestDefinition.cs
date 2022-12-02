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

namespace BlockChain.Wallet.Contract.Test.ContractDefinition
{


    public partial class TestDeployment : TestDeploymentBase
    {
        public TestDeployment() : base(BYTECODE) { }
        public TestDeployment(string byteCode) : base(byteCode) { }
    }

    public class TestDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b50610235806100206000396000f3fe608060405234801561001057600080fd5b50600436106100415760003560e01c8063364244ad146100465780636f3319d81461006f5780638ec38c6014610082575b600080fd5b610059610054366004610166565b6100e7565b60405161006691906101aa565b60405180910390f35b61005961007d366004610166565b610139565b6100d9610090366004610166565b6040805160208082019590955260609390931b6bffffffffffffffffffffffff191683820152605480840192909252805180840390920182526074909201909152805191012090565b604051908152602001610066565b60608383836040516020016101219392919092835260609190911b6bffffffffffffffffffffffff19166020830152603482015260540190565b60405160208183030381529060405290509392505050565b60408051602081018590526001600160a01b03841691810191909152606081810183905290608001610121565b60008060006060848603121561017b57600080fd5b8335925060208401356001600160a01b038116811461019957600080fd5b929592945050506040919091013590565b600060208083528351808285015260005b818110156101d7578581018301518582016040015282016101bb565b818111156101e9576000604083870101525b50601f01601f191692909201604001939250505056fea26469706673582212206b953fad7d875e06f6e784690292ef089291d11dda2b48c69c35008794ed256364736f6c634300080f0033";
        public TestDeploymentBase() : base(BYTECODE) { }
        public TestDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GetroundresultEncodeFunction : GetroundresultEncodeFunctionBase { }

    [Function("getRoundResult_encode", "bytes")]
    public class GetroundresultEncodeFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_resultNum", 1)]
        public virtual BigInteger ResultNum { get; set; }
        [Parameter("address", "_game", 2)]
        public virtual string Game { get; set; }
        [Parameter("bytes32", "_clientNonce", 3)]
        public virtual byte[] ClientNonce { get; set; }
    }

    public partial class GetroundresultEncodepackedFunction : GetroundresultEncodepackedFunctionBase { }

    [Function("getRoundResult_encodePacked", "bytes")]
    public class GetroundresultEncodepackedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_resultNum", 1)]
        public virtual BigInteger ResultNum { get; set; }
        [Parameter("address", "_game", 2)]
        public virtual string Game { get; set; }
        [Parameter("bytes32", "_clientNonce", 3)]
        public virtual byte[] ClientNonce { get; set; }
    }

    public partial class GetroundresultEncodepackedHashFunction : GetroundresultEncodepackedHashFunctionBase { }

    [Function("getRoundResult_encodePacked_Hash", "bytes32")]
    public class GetroundresultEncodepackedHashFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_resultNum", 1)]
        public virtual BigInteger ResultNum { get; set; }
        [Parameter("address", "_game", 2)]
        public virtual string Game { get; set; }
        [Parameter("bytes32", "_clientNonce", 3)]
        public virtual byte[] ClientNonce { get; set; }
    }

    public partial class GetroundresultEncodeOutputDTO : GetroundresultEncodeOutputDTOBase { }

    [FunctionOutput]
    public class GetroundresultEncodeOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "_result", 1)]
        public virtual byte[] Result { get; set; }
    }

    public partial class GetroundresultEncodepackedOutputDTO : GetroundresultEncodepackedOutputDTOBase { }

    [FunctionOutput]
    public class GetroundresultEncodepackedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "_result", 1)]
        public virtual byte[] Result { get; set; }
    }

    public partial class GetroundresultEncodepackedHashOutputDTO : GetroundresultEncodepackedHashOutputDTOBase { }

    [FunctionOutput]
    public class GetroundresultEncodepackedHashOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "_result", 1)]
        public virtual byte[] Result { get; set; }
    }
}
