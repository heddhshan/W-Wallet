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

namespace BlockChain.Wallet.Contract.WalletHelper.ContractDefinition
{


    public partial class WalletHelperDeployment : WalletHelperDeploymentBase
    {
        public WalletHelperDeployment() : base(BYTECODE) { }
        public WalletHelperDeployment(string byteCode) : base(byteCode) { }
    }

    public class WalletHelperDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60806040526000805460ff1916600117905534801561001d57600080fd5b50611e008061002d6000396000f3fe6080604052600436106101095760003560e01c806396f0d36f11610095578063c711709311610064578063c7117093146102de578063d6ff88d9146102fe578063e782f72e14610311578063f8bef55714610331578063ff57c9291461037e57600080fd5b806396f0d36f1461025e578063a7b250541461027e578063b0a17fcd1461029e578063b8dff9e1146102be57600080fd5b80634f285592116100dc5780634f2855921461019357806357199b1d146101c357806360dc44f5146101e35780638e6ae8401461021157806396324ba01461023e57600080fd5b8063064438561461010e57806322828e231461014b578063383dbb9614610160578063498eacbe14610180575b600080fd5b34801561011a57600080fd5b5061012e6101293660046116fc565b61039e565b6040516001600160a01b0390911681526020015b60405180910390f35b61015e610159366004611814565b61045c565b005b34801561016c57600080fd5b5061015e61017b3660046118b5565b6106d3565b61012e61018e3660046116fc565b610819565b34801561019f57600080fd5b506101b36101ae366004611908565b610891565b6040519015158152602001610142565b3480156101cf57600080fd5b5061015e6101de366004611925565b6108a5565b3480156101ef57600080fd5b506102036101fe3660046119c5565b610b93565b604051908152602001610142565b34801561021d57600080fd5b5061023161022c3660046119de565b610bc5565b6040516101429190611a24565b34801561024a57600080fd5b506102036102593660046119c5565b610bd8565b34801561026a57600080fd5b5061012e610279366004611a57565b610c13565b34801561028a57600080fd5b50610203610299366004611a93565b610c7b565b3480156102aa57600080fd5b5061015e6102b9366004611b05565b610c9d565b3480156102ca57600080fd5b5061012e6102d9366004611b73565b610e5e565b3480156102ea57600080fd5b506102316102f93660046119c5565b610e73565b61015e61030c366004611bac565b610e7e565b34801561031d57600080fd5b5061012e61032c3660046119de565b6110a0565b34801561033d57600080fd5b5061035161034c3660046116fc565b6110ac565b604080516001600160a01b03909516855260208501939093529183015260ff166060820152608001610142565b34801561038a57600080fd5b506102316103993660046119c5565b61116a565b60008060008084516041036104535750505060208201516040830151604184015160ff16601b8110156103d9576103d6601b82611c2d565b90505b8060ff16601b14806103ee57508060ff16601c145b156104535760408051600081526020810180835288905260ff831691810191909152606081018490526080810183905260019060a0016020604051602081039080840390855afa158015610446573d6000803e3d6000fd5b5050506020604051035193505b50505092915050565b60005460ff16151560011461048c5760405162461bcd60e51b815260040161048390611c46565b60405180910390fd5b6000805460ff19169055826104c85760405162461bcd60e51b8152602060048201526002602482015261423360f01b6044820152606401610483565b846104fa5760405162461bcd60e51b8152602060048201526002602482015261108d60f21b6044820152606401610483565b82851461052e5760405162461bcd60e51b8152602060048201526002602482015261423560f01b6044820152606401610483565b3460005b8681101561068757600088888381811061054e5761054e611c61565b90506020020160208101906105639190611908565b9050600087878481811061057957610579611c61565b60200291909101359150506001600160a01b0382161580159061059c5750600081115b80156105b057506001600160a01b0382163b155b1561063b576105bf8185611c77565b6040519094506001600160a01b0383169082156108fc029083906000818181858888f193505050501580156105f8573d6000803e3d6000fd5b50841561063657604080518281523360208201526001916001600160a01b038516918991600080516020611d8b833981519152910160405180910390a45b610672565b604080518281523360208201526000916001600160a01b038516918991600080516020611d8b833981519152910160405180910390a45b5050808061067f90611c8a565b915050610532565b5080156106bd57604051339082156108fc029083906000818181858888f193505050501580156106bb573d6000803e3d6000fd5b505b50506000805460ff191660011790555050505050565b60005460ff1615156001146106fa5760405162461bcd60e51b815260040161048390611c46565b6000805460ff191690556001600160a01b03841661073f5760405162461bcd60e51b8152602060048201526002602482015261543160f01b6044820152606401610483565b6001600160a01b03831661077a5760405162461bcd60e51b81526020600482015260026024820152612a1960f11b6044820152606401610483565b600082116107af5760405162461bcd60e51b8152602060048201526002602482015261543360f01b6044820152606401610483565b836107c56001600160a01b038216338686611175565b81156108055760011515846001600160a01b03166000600080516020611dab83398151915286338a6040516107fc93929190611ca3565b60405180910390a45b50506000805460ff19166001179055505050565b600034816108288286866111d5565b84516020808701919091206040805186815292830189905282018190526001600160a01b038316606083015291925033907fd3438c46b19a693c0cc0f5a6e39b75742eb27495f6d406907d76b7f464dc05aa9060800160405180910390a2509150505b92915050565b60006001600160a01b0382163b151561088b565b60005460ff1615156001146108cc5760405162461bcd60e51b815260040161048390611c46565b6000805460ff191690556001600160a01b0387166109125760405162461bcd60e51b815260206004820152600360248201526204232360ec1b6044820152606401610483565b826109455760405162461bcd60e51b815260206004820152600360248201526242323160e81b6044820152606401610483565b846109785760405162461bcd60e51b815260206004820152600360248201526221191960e91b6044820152606401610483565b8285146109ad5760405162461bcd60e51b815260206004820152600360248201526242323360e81b6044820152606401610483565b8660005b86811015610b7b5760008888838181106109cd576109cd611c61565b90506020020160208101906109e29190611908565b6001600160a01b031614158015610a1157506000868683818110610a0857610a08611c61565b90506020020135115b15610af457610a7433898984818110610a2c57610a2c611c61565b9050602002016020810190610a419190611908565b888885818110610a5357610a53611c61565b90506020020135856001600160a01b0316611175909392919063ffffffff16565b8215610aef576001888883818110610a8e57610a8e611c61565b9050602002016020810190610aa39190611908565b6001600160a01b031685600080516020611dab833981519152898986818110610ace57610ace611c61565b90506020020135338e604051610ae693929190611ca3565b60405180910390a45b610b69565b6000888883818110610b0857610b08611c61565b9050602002016020810190610b1d9190611908565b6001600160a01b031685600080516020611dab833981519152898986818110610b4857610b48611c61565b90506020020135338e604051610b6093929190611ca3565b60405180910390a45b80610b7381611c8a565b9150506109b1565b50506000805460ff1916600117905550505050505050565b600081604051602001610ba891815260200190565b604051602081830303815290604052805190602001209050919050565b6060610bd18383611289565b9392505050565b6040517f19457468657265756d205369676e6564204d6573736167653a0a3332000000006020820152603c8101829052600090605c01610ba8565b6040805160008082526020820180845287905260ff841692820192909252606081018590526080810184905260019060a0016020604051602081039080840390855afa158015610c67573d6000803e3d6000fd5b5050604051601f1901519695505050505050565b60008282604051610c8d929190611cc2565b6040518091039020905092915050565b60005460ff161515600114610cc45760405162461bcd60e51b815260040161048390611c46565b6000805460ff191690556001600160a01b038616610d0a5760405162461bcd60e51b815260206004820152600360248201526204231360ec1b6044820152606401610483565b60008311610d405760405162461bcd60e51b815260206004820152600360248201526242313160e81b6044820152606401610483565b83610d735760405162461bcd60e51b815260206004820152600360248201526221189960e91b6044820152606401610483565b8560005b858110156106bb576000878783818110610d9357610d93611c61565b9050602002016020810190610da89190611908565b90506001600160a01b03811615610e1257610dce6001600160a01b038416338389611175565b8315610e0d5760011515816001600160a01b031686600080516020611dab83398151915289338e604051610e0493929190611ca3565b60405180910390a45b610e4b565b60001515816001600160a01b031686600080516020611dab83398151915289338e604051610e4293929190611ca3565b60405180910390a45b5080610e5681611c8a565b915050610d77565b6000610e6b84848461140a565b949350505050565b606061088b82611465565b60005460ff161515600114610ea55760405162461bcd60e51b815260040161048390611c46565b6000805460ff1916905582610ee15760405162461bcd60e51b8152602060048201526002602482015261423160f01b6044820152606401610483565b83610f135760405162461bcd60e51b8152602060048201526002602482015261211960f11b6044820152606401610483565b6000610f1f8585611cd2565b610f299034611c77565b905060005b85811015611055576000878783818110610f4a57610f4a611c61565b9050602002016020810190610f5f9190611908565b90506001600160a01b03811615801590610f795750600086115b8015610f8d57506001600160a01b0381163b155b1561100b576040516001600160a01b0382169087156108fc029088906000818181858888f19350505050158015610fc8573d6000803e3d6000fd5b50831561100657604080518781523360208201526001916001600160a01b038416918891600080516020611d8b833981519152910160405180910390a45b611042565b604080518781523360208201526000916001600160a01b038416918891600080516020611d8b833981519152910160405180910390a45b508061104d81611c8a565b915050610f2e565b50801561108b57604051339082156108fc029083906000818181858888f19350505050158015611089573d6000803e3d6000fd5b505b50506000805460ff1916600117905550505050565b6000610bd183836114bc565b60008060008084516041036111615750505060208201516040830151604184015160ff16601b8110156110e7576110e4601b82611c2d565b90505b8060ff16601b14806110fc57508060ff16601c145b156111615760408051600081526020810180835288905260ff831691810191909152606081018490526080810183905260019060a0016020604051602081039080840390855afa158015611154573d6000803e3d6000fd5b5050506020604051035193505b92959194509250565b606061088b826114c9565b604080516001600160a01b0385811660248301528416604482015260648082018490528251808303909101815260849091019091526020810180516001600160e01b03166323b872dd60e01b1790526111cf9085906115ca565b50505050565b6000808447101561120d5760405162461bcd60e51b8152602060048201526002602482015261433160f01b6044820152606401610483565b82516000036112435760405162461bcd60e51b8152602060048201526002602482015261219960f11b6044820152606401610483565b8383516020850187f590506001600160a01b038116610e6b5760405162461bcd60e51b8152602060048201526002602482015261433360f01b6044820152606401610483565b60606000611298836002611cd2565b6112a3906002611ce9565b67ffffffffffffffff8111156112bb576112bb6116e6565b6040519080825280601f01601f1916602001820160405280156112e5576020820181803683370190505b509050600360fc1b8160008151811061130057611300611c61565b60200101906001600160f81b031916908160001a905350600f60fb1b8160018151811061132f5761132f611c61565b60200101906001600160f81b031916908160001a9053506000611353846002611cd2565b61135e906001611ce9565b90505b60018111156113d6576f181899199a1a9b1b9c1cb0b131b232b360811b85600f166010811061139257611392611c61565b1a60f81b8282815181106113a8576113a8611c61565b60200101906001600160f81b031916908160001a90535060049490941c936113cf81611cfc565b9050611361565b508315610bd15760405162461bcd60e51b8152602060048201526002602482015261545360f01b6044820152606401610483565b604080516001600160f81b031960208083019190915260609390931b6bffffffffffffffffffffffff191660218201526035810194909452605580850193909352805180850390930183526075909301909252805191012090565b60608160000361148f5750506040805180820190915260048152630307830360e41b602082015290565b8160005b81156114b257806114a381611c8a565b915050600882901c9150611493565b610e6b8482611289565b6000610bd183833061140a565b6060816000036114f05750506040805180820190915260018152600360fc1b602082015290565b8160005b811561151a578061150481611c8a565b91506115139050600a83611d29565b91506114f4565b60008167ffffffffffffffff811115611535576115356116e6565b6040519080825280601f01601f19166020018201604052801561155f576020820181803683370190505b5090505b8415610e6b57611574600183611c77565b9150611581600a86611d3d565b61158c906030611ce9565b60f81b8183815181106115a1576115a1611c61565b60200101906001600160f81b031916908160001a9053506115c3600a86611d29565b9450611563565b6001600160a01b0382163b6116065760405162461bcd60e51b8152602060048201526002602482015261299960f11b6044820152606401610483565b600080836001600160a01b0316836040516116219190611d51565b6000604051808303816000865af19150503d806000811461165e576040519150601f19603f3d011682016040523d82523d6000602084013e611663565b606091505b50915091508161169a5760405162461bcd60e51b8152602060048201526002602482015261533360f01b6044820152606401610483565b8051156111cf57808060200190518101906116b59190611d6d565b6111cf5760405162461bcd60e51b815260206004820152600260248201526114cd60f21b6044820152606401610483565b634e487b7160e01b600052604160045260246000fd5b6000806040838503121561170f57600080fd5b82359150602083013567ffffffffffffffff8082111561172e57600080fd5b818501915085601f83011261174257600080fd5b813581811115611754576117546116e6565b604051601f8201601f19908116603f0116810190838211818310171561177c5761177c6116e6565b8160405282815288602084870101111561179557600080fd5b8260208601602083013760006020848301015280955050505050509250929050565b60008083601f8401126117c957600080fd5b50813567ffffffffffffffff8111156117e157600080fd5b6020830191508360208260051b85010111156117fc57600080fd5b9250929050565b801515811461181157600080fd5b50565b6000806000806000806080878903121561182d57600080fd5b863567ffffffffffffffff8082111561184557600080fd5b6118518a838b016117b7565b9098509650602089013591508082111561186a57600080fd5b5061187789828a016117b7565b90955093505060408701359150606087013561189281611803565b809150509295509295509295565b6001600160a01b038116811461181157600080fd5b600080600080608085870312156118cb57600080fd5b84356118d6816118a0565b935060208501356118e6816118a0565b92506040850135915060608501356118fd81611803565b939692955090935050565b60006020828403121561191a57600080fd5b8135610bd1816118a0565b600080600080600080600060a0888a03121561194057600080fd5b873561194b816118a0565b9650602088013567ffffffffffffffff8082111561196857600080fd5b6119748b838c016117b7565b909850965060408a013591508082111561198d57600080fd5b5061199a8a828b016117b7565b9095509350506060880135915060808801356119b581611803565b8091505092959891949750929550565b6000602082840312156119d757600080fd5b5035919050565b600080604083850312156119f157600080fd5b50508035926020909101359150565b60005b83811015611a1b578181015183820152602001611a03565b50506000910152565b6020815260008251806020840152611a43816040850160208701611a00565b601f01601f19169190910160400192915050565b60008060008060808587031215611a6d57600080fd5b843593506020850135925060408501359150606085013560ff811681146118fd57600080fd5b60008060208385031215611aa657600080fd5b823567ffffffffffffffff80821115611abe57600080fd5b818501915085601f830112611ad257600080fd5b813581811115611ae157600080fd5b866020828501011115611af357600080fd5b60209290920196919550909350505050565b60008060008060008060a08789031215611b1e57600080fd5b8635611b29816118a0565b9550602087013567ffffffffffffffff811115611b4557600080fd5b611b5189828a016117b7565b9096509450506040870135925060608701359150608087013561189281611803565b600080600060608486031215611b8857600080fd5b83359250602084013591506040840135611ba1816118a0565b809150509250925092565b600080600080600060808688031215611bc457600080fd5b853567ffffffffffffffff811115611bdb57600080fd5b611be7888289016117b7565b90965094505060208601359250604086013591506060860135611c0981611803565b809150509295509295909350565b634e487b7160e01b600052601160045260246000fd5b60ff818116838216019081111561088b5761088b611c17565b6020808252600190820152601360fa1b604082015260600190565b634e487b7160e01b600052603260045260246000fd5b8181038181111561088b5761088b611c17565b600060018201611c9c57611c9c611c17565b5060010190565b9283526001600160a01b03918216602084015216604082015260600190565b8183823760009101908152919050565b808202811582820484141761088b5761088b611c17565b8082018082111561088b5761088b611c17565b600081611d0b57611d0b611c17565b506000190190565b634e487b7160e01b600052601260045260246000fd5b600082611d3857611d38611d13565b500490565b600082611d4c57611d4c611d13565b500690565b60008251611d63818460208701611a00565b9190910192915050565b600060208284031215611d7f57600080fd5b8151610bd18161180356fec08e18f6a8a367ee85cda322ed13bb7fc20da830c5e543042494dbd69545563e7e227584bd14244ffc37d140c7437b1b8c05c500012af64cc303c39ff3487c95a26469706673582212204c5754fe551de8227b12c2a1c2cd996bf07bcfaea395e6f51fca627e1620c27364736f6c63430008110033";
        public WalletHelperDeploymentBase() : base(BYTECODE) { }
        public WalletHelperDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GetBytes32HashFunction : GetBytes32HashFunctionBase { }

    [Function("GetBytes32Hash", "bytes32")]
    public class GetBytes32HashFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "_data", 1)]
        public virtual byte[] Data { get; set; }
    }

    public partial class GetBytesHashFunction : GetBytesHashFunctionBase { }

    [Function("GetBytesHash", "bytes32")]
    public class GetBytesHashFunctionBase : FunctionMessage
    {
        [Parameter("bytes", "_bytecode", 1)]
        public virtual byte[] Bytecode { get; set; }
    }

    public partial class GetEthSignedMessageHashFunction : GetEthSignedMessageHashFunctionBase { }

    [Function("GetEthSignedMessageHash", "bytes32")]
    public class GetEthSignedMessageHashFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "_hash", 1)]
        public virtual byte[] Hash { get; set; }
    }

    public partial class GetRSVFunction : GetRSVFunctionBase { }

    [Function("GetRSV", typeof(GetRSVOutputDTO))]
    public class GetRSVFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "h", 1)]
        public virtual byte[] H { get; set; }
        [Parameter("bytes", "sig", 2)]
        public virtual byte[] Sig { get; set; }
    }

    public partial class IsContractFunction : IsContractFunctionBase { }

    [Function("IsContract", "bool")]
    public class IsContractFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class ToHexStrin2Function : ToHexStrin2FunctionBase { }

    [Function("ToHexStrin2", "string")]
    public class ToHexStrin2FunctionBase : FunctionMessage
    {
        [Parameter("uint256", "value", 1)]
        public virtual BigInteger Value { get; set; }
        [Parameter("uint256", "length", 2)]
        public virtual BigInteger Length { get; set; }
    }

    public partial class ToHexString1Function : ToHexString1FunctionBase { }

    [Function("ToHexString1", "string")]
    public class ToHexString1FunctionBase : FunctionMessage
    {
        [Parameter("uint256", "value", 1)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class ToStringFunction : ToStringFunctionBase { }

    [Function("ToString", "string")]
    public class ToStringFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "value", 1)]
        public virtual BigInteger Value { get; set; }
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

    public partial class ComputeContractAddress1Function : ComputeContractAddress1FunctionBase { }

    [Function("computeContractAddress1", "address")]
    public class ComputeContractAddress1FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "_salt", 1)]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes32", "_bytecodeHash", 2)]
        public virtual byte[] BytecodeHash { get; set; }
    }

    public partial class ComputeContractAddress2Function : ComputeContractAddress2FunctionBase { }

    [Function("computeContractAddress2", "address")]
    public class ComputeContractAddress2FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "_salt", 1)]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes32", "_bytecodeHash", 2)]
        public virtual byte[] BytecodeHash { get; set; }
        [Parameter("address", "_deployer", 3)]
        public virtual string Deployer { get; set; }
    }

    public partial class DeployContractFunction : DeployContractFunctionBase { }

    [Function("deployContract", "address")]
    public class DeployContractFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "_salt", 1)]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes", "_bytecode", 2)]
        public virtual byte[] Bytecode { get; set; }
    }

    public partial class GetAddress1Function : GetAddress1FunctionBase { }

    [Function("getAddress1", "address")]
    public class GetAddress1FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "h", 1)]
        public virtual byte[] H { get; set; }
        [Parameter("bytes32", "_r", 2)]
        public virtual byte[] R { get; set; }
        [Parameter("bytes32", "_s", 3)]
        public virtual byte[] S { get; set; }
        [Parameter("uint8", "_v", 4)]
        public virtual byte V { get; set; }
    }

    public partial class GetAddress2Function : GetAddress2FunctionBase { }

    [Function("getAddress2", "address")]
    public class GetAddress2FunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "h", 1)]
        public virtual byte[] H { get; set; }
        [Parameter("bytes", "sig", 2)]
        public virtual byte[] Sig { get; set; }
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

    public partial class OnDeployContractEventDTO : OnDeployContractEventDTOBase { }

    [Event("OnDeployContract")]
    public class OnDeployContractEventDTOBase : IEventDTO
    {
        [Parameter("address", "_user", 1, true )]
        public virtual string User { get; set; }
        [Parameter("uint256", "_amount", 2, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("bytes32", "_salt", 3, false )]
        public virtual byte[] Salt { get; set; }
        [Parameter("bytes32", "_bytecodeHash", 4, false )]
        public virtual byte[] BytecodeHash { get; set; }
        [Parameter("address", "_contract", 5, false )]
        public virtual string Contract { get; set; }
    }

    public partial class OnTokenTransferEventDTO : OnTokenTransferEventDTOBase { }

    [Event("OnTokenTransfer")]
    public class OnTokenTransferEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "_batchId", 1, true )]
        public virtual BigInteger BatchId { get; set; }
        [Parameter("address", "_to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("bool", "_done", 3, true )]
        public virtual bool Done { get; set; }
        [Parameter("uint256", "_amount", 4, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("address", "_sender", 5, false )]
        public virtual string Sender { get; set; }
        [Parameter("address", "_erc20Token", 6, false )]
        public virtual string Erc20Token { get; set; }
    }

    public partial class OnTransferEventDTO : OnTransferEventDTOBase { }

    [Event("OnTransfer")]
    public class OnTransferEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "_batchId", 1, true )]
        public virtual BigInteger BatchId { get; set; }
        [Parameter("address", "_to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("bool", "_done", 3, true )]
        public virtual bool Done { get; set; }
        [Parameter("uint256", "_amount", 4, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("address", "_sender", 5, false )]
        public virtual string Sender { get; set; }
    }

    public partial class GetBytes32HashOutputDTO : GetBytes32HashOutputDTOBase { }

    [FunctionOutput]
    public class GetBytes32HashOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }

    public partial class GetBytesHashOutputDTO : GetBytesHashOutputDTOBase { }

    [FunctionOutput]
    public class GetBytesHashOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }

    public partial class GetEthSignedMessageHashOutputDTO : GetEthSignedMessageHashOutputDTOBase { }

    [FunctionOutput]
    public class GetEthSignedMessageHashOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes32", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }

    public partial class GetRSVOutputDTO : GetRSVOutputDTOBase { }

    [FunctionOutput]
    public class GetRSVOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "_address", 1)]
        public virtual string Address { get; set; }
        [Parameter("bytes32", "_r", 2)]
        public virtual byte[] R { get; set; }
        [Parameter("bytes32", "_s", 3)]
        public virtual byte[] S { get; set; }
        [Parameter("uint8", "_v", 4)]
        public virtual byte V { get; set; }
    }

    public partial class IsContractOutputDTO : IsContractOutputDTOBase { }

    [FunctionOutput]
    public class IsContractOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class ToHexStrin2OutputDTO : ToHexStrin2OutputDTOBase { }

    [FunctionOutput]
    public class ToHexStrin2OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ToHexString1OutputDTO : ToHexString1OutputDTOBase { }

    [FunctionOutput]
    public class ToHexString1OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ToStringOutputDTO : ToStringOutputDTOBase { }

    [FunctionOutput]
    public class ToStringOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }









    public partial class ComputeContractAddress1OutputDTO : ComputeContractAddress1OutputDTOBase { }

    [FunctionOutput]
    public class ComputeContractAddress1OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ComputeContractAddress2OutputDTO : ComputeContractAddress2OutputDTOBase { }

    [FunctionOutput]
    public class ComputeContractAddress2OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }



    public partial class GetAddress1OutputDTO : GetAddress1OutputDTOBase { }

    [FunctionOutput]
    public class GetAddress1OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "_address", 1)]
        public virtual string Address { get; set; }
    }

    public partial class GetAddress2OutputDTO : GetAddress2OutputDTOBase { }

    [FunctionOutput]
    public class GetAddress2OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "_address", 1)]
        public virtual string Address { get; set; }
    }


}