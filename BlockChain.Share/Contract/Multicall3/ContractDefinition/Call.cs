using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace BlockChain.Share.Contract.Multicall3.ContractDefinition
{
    public partial class Call : CallBase { }

    public class CallBase 
    {
        [Parameter("address", "target", 1)]
        public virtual string Target { get; set; }
        [Parameter("bytes", "callData", 2)]
        public virtual byte[] CallData { get; set; }
    }
}
