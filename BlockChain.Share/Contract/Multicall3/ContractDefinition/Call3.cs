using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace BlockChain.Share.Contract.Multicall3.ContractDefinition
{
    public partial class Call3 : Call3Base { }

    public class Call3Base 
    {
        [Parameter("address", "target", 1)]
        public virtual string Target { get; set; }
        [Parameter("bool", "allowFailure", 2)]
        public virtual bool AllowFailure { get; set; }
        [Parameter("bytes", "callData", 3)]
        public virtual byte[] CallData { get; set; }
    }
}
