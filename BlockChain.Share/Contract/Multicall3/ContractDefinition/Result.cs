using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace BlockChain.Share.Contract.Multicall3.ContractDefinition
{
    public partial class Result : ResultBase { }

    public class ResultBase 
    {
        [Parameter("bool", "success", 1)]
        public virtual bool Success { get; set; }
        [Parameter("bytes", "returnData", 2)]
        public virtual byte[] ReturnData { get; set; }
    }
}
