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

namespace BlockChain.Share.Contract.Multicall3.ContractDefinition
{


    public partial class Multicall3Deployment : Multicall3DeploymentBase
    {
        public Multicall3Deployment() : base(BYTECODE) { }
        public Multicall3Deployment(string byteCode) : base(byteCode) { }
    }

    public class Multicall3DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b50610ccf806100206000396000f3fe6080604052600436106100f35760003560e01c80634d2301cc1161008a578063a8b0574e11610059578063a8b0574e1461022f578063bce38bd71461024a578063c3077fa91461025d578063ee82ac5e1461027057600080fd5b80634d2301cc146101ce57806372425d9d146101f657806382ad56cb1461020957806386d516e81461021c57600080fd5b80633408e470116100c65780633408e47014610173578063399542e9146101865780633e64a696146101a857806342cbb15c146101bb57600080fd5b80630f28c97d146100f8578063174dea711461011a578063252dba421461013a57806327e86d6e1461015b575b600080fd5b34801561010457600080fd5b50425b6040519081526020015b60405180910390f35b61012d61012836600461098c565b61028f565b6040516101119190610a82565b61014d61014836600461098c565b61047d565b604051610111929190610a9c565b34801561016757600080fd5b50436000190140610107565b34801561017f57600080fd5b5046610107565b610199610194366004610b06565b6105f1565b60405161011193929190610b60565b3480156101b457600080fd5b5048610107565b3480156101c757600080fd5b5043610107565b3480156101da57600080fd5b506101076101e9366004610b88565b6001600160a01b03163190565b34801561020257600080fd5b5044610107565b61012d61021736600461098c565b61060c565b34801561022857600080fd5b5045610107565b34801561023b57600080fd5b50604051418152602001610111565b61012d610258366004610b06565b61078e565b61019961026b36600461098c565b610921565b34801561027c57600080fd5b5061010761028b366004610bb1565b4090565b60606000828067ffffffffffffffff8111156102ad576102ad610bca565b6040519080825280602002602001820160405280156102f357816020015b6040805180820190915260008152606060208201528152602001906001900390816102cb5790505b5092503660005b8281101561041f57600085828151811061031657610316610be0565b6020026020010151905087878381811061033257610332610be0565b90506020028101906103449190610bf6565b60408101359586019590935061035d6020850185610b88565b6001600160a01b0316816103746060870187610c16565b604051610382929190610c5d565b60006040518083038185875af1925050503d80600081146103bf576040519150601f19603f3d011682016040523d82523d6000602084013e6103c4565b606091505b5060208085019190915290151580845290850135176104155762461bcd60e51b6000526020600452601760245276135d5b1d1a58d85b1b0cce8818d85b1b0819985a5b1959604a1b60445260846000fd5b50506001016102fa565b508234146104745760405162461bcd60e51b815260206004820152601a60248201527f4d756c746963616c6c333a2076616c7565206d69736d6174636800000000000060448201526064015b60405180910390fd5b50505092915050565b436060828067ffffffffffffffff81111561049a5761049a610bca565b6040519080825280602002602001820160405280156104cd57816020015b60608152602001906001900390816104b85790505b5091503660005b828110156105e75760008787838181106104f0576104f0610be0565b90506020028101906105029190610c6d565b92506105116020840184610b88565b6001600160a01b03166105276020850185610c16565b604051610535929190610c5d565b6000604051808303816000865af19150503d8060008114610572576040519150601f19603f3d011682016040523d82523d6000602084013e610577565b606091505b5086848151811061058a5761058a610be0565b60209081029190910101529050806105de5760405162461bcd60e51b8152602060048201526017602482015276135d5b1d1a58d85b1b0cce8818d85b1b0819985a5b1959604a1b604482015260640161046b565b506001016104d4565b5050509250929050565b438040606061060186868661078e565b905093509350939050565b6060818067ffffffffffffffff81111561062857610628610bca565b60405190808252806020026020018201604052801561066e57816020015b6040805180820190915260008152606060208201528152602001906001900390816106465790505b5091503660005b8281101561047457600084828151811061069157610691610be0565b602002602001015190508686838181106106ad576106ad610be0565b90506020028101906106bf9190610c83565b92506106ce6020840184610b88565b6001600160a01b03166106e46040850185610c16565b6040516106f2929190610c5d565b6000604051808303816000865af19150503d806000811461072f576040519150601f19603f3d011682016040523d82523d6000602084013e610734565b606091505b5060208084019190915290151580835290840135176107855762461bcd60e51b6000526020600452601760245276135d5b1d1a58d85b1b0cce8818d85b1b0819985a5b1959604a1b60445260646000fd5b50600101610675565b6060818067ffffffffffffffff8111156107aa576107aa610bca565b6040519080825280602002602001820160405280156107f057816020015b6040805180820190915260008152606060208201528152602001906001900390816107c85790505b5091503660005b8281101561091757600084828151811061081357610813610be0565b6020026020010151905086868381811061082f5761082f610be0565b90506020028101906108419190610c6d565b92506108506020840184610b88565b6001600160a01b03166108666020850185610c16565b604051610874929190610c5d565b6000604051808303816000865af19150503d80600081146108b1576040519150601f19603f3d011682016040523d82523d6000602084013e6108b6565b606091505b50602083015215158152871561090e57805161090e5760405162461bcd60e51b8152602060048201526017602482015276135d5b1d1a58d85b1b0cce8818d85b1b0819985a5b1959604a1b604482015260640161046b565b506001016107f7565b5050509392505050565b6000806060610932600186866105f1565b919790965090945092505050565b60008083601f84011261095257600080fd5b50813567ffffffffffffffff81111561096a57600080fd5b6020830191508360208260051b850101111561098557600080fd5b9250929050565b6000806020838503121561099f57600080fd5b823567ffffffffffffffff8111156109b657600080fd5b6109c285828601610940565b90969095509350505050565b6000815180845260005b818110156109f4576020818501810151868301820152016109d8565b506000602082860101526020601f19601f83011685010191505092915050565b600082825180855260208086019550808260051b84010181860160005b84811015610a7557858303601f1901895281518051151584528401516040858501819052610a61818601836109ce565b9a86019a9450505090830190600101610a31565b5090979650505050505050565b602081526000610a956020830184610a14565b9392505050565b600060408201848352602060408185015281855180845260608601915060608160051b870101935082870160005b82811015610af857605f19888703018452610ae68683516109ce565b95509284019290840190600101610aca565b509398975050505050505050565b600080600060408486031215610b1b57600080fd5b83358015158114610b2b57600080fd5b9250602084013567ffffffffffffffff811115610b4757600080fd5b610b5386828701610940565b9497909650939450505050565b838152826020820152606060408201526000610b7f6060830184610a14565b95945050505050565b600060208284031215610b9a57600080fd5b81356001600160a01b0381168114610a9557600080fd5b600060208284031215610bc357600080fd5b5035919050565b634e487b7160e01b600052604160045260246000fd5b634e487b7160e01b600052603260045260246000fd5b60008235607e19833603018112610c0c57600080fd5b9190910192915050565b6000808335601e19843603018112610c2d57600080fd5b83018035915067ffffffffffffffff821115610c4857600080fd5b60200191503681900382131561098557600080fd5b8183823760009101908152919050565b60008235603e19833603018112610c0c57600080fd5b60008235605e19833603018112610c0c57600080fdfea2646970667358221220c80ccb653cc5658991170676d6be3f8e1b5115303e80670a0c108b8293d8bbd764736f6c63430008100033";
        public Multicall3DeploymentBase() : base(BYTECODE) { }
        public Multicall3DeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AggregateFunction : AggregateFunctionBase { }

    [Function("aggregate", typeof(AggregateOutputDTO))]
    public class AggregateFunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "calls", 1)]
        public virtual List<Call> Calls { get; set; }
    }

    public partial class Aggregate3Function : Aggregate3FunctionBase { }

    [Function("aggregate3", typeof(Aggregate3OutputDTO))]
    public class Aggregate3FunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "calls", 1)]
        public virtual List<Call3> Calls { get; set; }
    }

    public partial class Aggregate3ValueFunction : Aggregate3ValueFunctionBase { }

    [Function("aggregate3Value", typeof(Aggregate3ValueOutputDTO))]
    public class Aggregate3ValueFunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "calls", 1)]
        public virtual List<Call3Value> Calls { get; set; }
    }

    public partial class BlockAndAggregateFunction : BlockAndAggregateFunctionBase { }

    [Function("blockAndAggregate", typeof(BlockAndAggregateOutputDTO))]
    public class BlockAndAggregateFunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "calls", 1)]
        public virtual List<Call> Calls { get; set; }
    }

    public partial class GetBasefeeFunction : GetBasefeeFunctionBase { }

    [Function("getBasefee", "uint256")]
    public class GetBasefeeFunctionBase : FunctionMessage
    {

    }

    public partial class GetBlockHashFunction : GetBlockHashFunctionBase { }

    [Function("getBlockHash", "bytes32")]
    public class GetBlockHashFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "blockNumber", 1)]
        public virtual BigInteger BlockNumber { get; set; }
    }

    public partial class GetBlockNumberFunction : GetBlockNumberFunctionBase { }

    [Function("getBlockNumber", "uint256")]
    public class GetBlockNumberFunctionBase : FunctionMessage
    {

    }

    public partial class GetChainIdFunction : GetChainIdFunctionBase { }

    [Function("getChainId", "uint256")]
    public class GetChainIdFunctionBase : FunctionMessage
    {

    }

    public partial class GetCurrentBlockCoinbaseFunction : GetCurrentBlockCoinbaseFunctionBase { }

    [Function("getCurrentBlockCoinbase", "address")]
    public class GetCurrentBlockCoinbaseFunctionBase : FunctionMessage
    {

    }

    public partial class GetCurrentBlockDifficultyFunction : GetCurrentBlockDifficultyFunctionBase { }

    [Function("getCurrentBlockDifficulty", "uint256")]
    public class GetCurrentBlockDifficultyFunctionBase : FunctionMessage
    {

    }

    public partial class GetCurrentBlockGasLimitFunction : GetCurrentBlockGasLimitFunctionBase { }

    [Function("getCurrentBlockGasLimit", "uint256")]
    public class GetCurrentBlockGasLimitFunctionBase : FunctionMessage
    {

    }

    public partial class GetCurrentBlockTimestampFunction : GetCurrentBlockTimestampFunctionBase { }

    [Function("getCurrentBlockTimestamp", "uint256")]
    public class GetCurrentBlockTimestampFunctionBase : FunctionMessage
    {

    }

    public partial class GetEthBalanceFunction : GetEthBalanceFunctionBase { }

    [Function("getEthBalance", "uint256")]
    public class GetEthBalanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "addr", 1)]
        public virtual string Addr { get; set; }
    }

    public partial class GetLastBlockHashFunction : GetLastBlockHashFunctionBase { }

    [Function("getLastBlockHash", "bytes32")]
    public class GetLastBlockHashFunctionBase : FunctionMessage
    {

    }

    public partial class TryAggregateFunction : TryAggregateFunctionBase { }

    [Function("tryAggregate", typeof(TryAggregateOutputDTO))]
    public class TryAggregateFunctionBase : FunctionMessage
    {
        [Parameter("bool", "requireSuccess", 1)]
        public virtual bool RequireSuccess { get; set; }
        [Parameter("tuple[]", "calls", 2)]
        public virtual List<Call> Calls { get; set; }
    }

    public partial class TryBlockAndAggregateFunction : TryBlockAndAggregateFunctionBase { }

    [Function("tryBlockAndAggregate", typeof(TryBlockAndAggregateOutputDTO))]
    public class TryBlockAndAggregateFunctionBase : FunctionMessage
    {
        [Parameter("bool", "requireSuccess", 1)]
        public virtual bool RequireSuccess { get; set; }
        [Parameter("tuple[]", "calls", 2)]
        public virtual List<Call> Calls { get; set; }
    }

    public partial class AggregateOutputDTO : AggregateOutputDTOBase { }

    [FunctionOutput]
    public class AggregateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "blockNumber", 1)]
        public virtual BigInteger BlockNumber { get; set; }
        [Parameter("bytes[]", "returnData", 2)]
        public virtual List<byte[]> ReturnData { get; set; }
    }

    public partial class Aggregate3OutputDTO : Aggregate3OutputDTOBase { }

    [FunctionOutput]
    public class Aggregate3OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple[]", "returnData", 1)]
        public virtual List<Result> ReturnData { get; set; }
    }

    public partial class Aggregate3ValueOutputDTO : Aggregate3ValueOutputDTOBase { }

    [FunctionOutput]
    public class Aggregate3ValueOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple[]", "returnData", 1)]
        public virtual List<Result> ReturnData { get; set; }
    }

    public partial class BlockAndAggregateOutputDTO : BlockAndAggregateOutputDTOBase { }

    [FunctionOutput]
    public class BlockAndAggregateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "blockNumber", 1)]
        public virtual BigInteger BlockNumber { get; set; }
        [Parameter("bytes32", "blockHash", 2)]
        public virtual byte[] BlockHash { get; set; }
        [Parameter("tuple[]", "returnData", 3)]
        public virtual List<Result> ReturnData { get; set; }
    }

    public partial class GetBasefeeOutputDTO : GetBasefeeOutputDTOBase { }

    [FunctionOutput]
    public class GetBasefeeOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "basefee", 1)]
        public virtual BigInteger Basefee { get; set; }
    }

    public partial class GetBlockHashOutputDTO : GetBlockHashOutputDTOBase { }

    [FunctionOutput]
    public class GetBlockHashOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "blockHash", 1)]
        public virtual byte[] BlockHash { get; set; }
    }

    public partial class GetBlockNumberOutputDTO : GetBlockNumberOutputDTOBase { }

    [FunctionOutput]
    public class GetBlockNumberOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "blockNumber", 1)]
        public virtual BigInteger BlockNumber { get; set; }
    }

    public partial class GetChainIdOutputDTO : GetChainIdOutputDTOBase { }

    [FunctionOutput]
    public class GetChainIdOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "chainid", 1)]
        public virtual BigInteger Chainid { get; set; }
    }

    public partial class GetCurrentBlockCoinbaseOutputDTO : GetCurrentBlockCoinbaseOutputDTOBase { }

    [FunctionOutput]
    public class GetCurrentBlockCoinbaseOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "coinbase", 1)]
        public virtual string Coinbase { get; set; }
    }

    public partial class GetCurrentBlockDifficultyOutputDTO : GetCurrentBlockDifficultyOutputDTOBase { }

    [FunctionOutput]
    public class GetCurrentBlockDifficultyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "difficulty", 1)]
        public virtual BigInteger Difficulty { get; set; }
    }

    public partial class GetCurrentBlockGasLimitOutputDTO : GetCurrentBlockGasLimitOutputDTOBase { }

    [FunctionOutput]
    public class GetCurrentBlockGasLimitOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "gaslimit", 1)]
        public virtual BigInteger Gaslimit { get; set; }
    }

    public partial class GetCurrentBlockTimestampOutputDTO : GetCurrentBlockTimestampOutputDTOBase { }

    [FunctionOutput]
    public class GetCurrentBlockTimestampOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "timestamp", 1)]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class GetEthBalanceOutputDTO : GetEthBalanceOutputDTOBase { }

    [FunctionOutput]
    public class GetEthBalanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "balance", 1)]
        public virtual BigInteger Balance { get; set; }
    }

    public partial class GetLastBlockHashOutputDTO : GetLastBlockHashOutputDTOBase { }

    [FunctionOutput]
    public class GetLastBlockHashOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "blockHash", 1)]
        public virtual byte[] BlockHash { get; set; }
    }

    public partial class TryAggregateOutputDTO : TryAggregateOutputDTOBase { }

    [FunctionOutput]
    public class TryAggregateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple[]", "returnData", 1)]
        public virtual List<Result> ReturnData { get; set; }
    }

    public partial class TryBlockAndAggregateOutputDTO : TryBlockAndAggregateOutputDTOBase { }

    [FunctionOutput]
    public class TryBlockAndAggregateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "blockNumber", 1)]
        public virtual BigInteger BlockNumber { get; set; }
        [Parameter("bytes32", "blockHash", 2)]
        public virtual byte[] BlockHash { get; set; }
        [Parameter("tuple[]", "returnData", 3)]
        public virtual List<Result> ReturnData { get; set; }
    }
}
