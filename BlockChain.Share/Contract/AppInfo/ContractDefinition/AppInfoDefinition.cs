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

namespace BlockChain.Share.Contract.AppInfo.ContractDefinition
{


    public partial class AppInfoDeployment : AppInfoDeploymentBase
    {
        public AppInfoDeployment() : base(BYTECODE) { }
        public AppInfoDeployment(string byteCode) : base(byteCode) { }
    }

    public class AppInfoDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405260016007553480156200001657600080fd5b506040516200224c3803806200224c8339810160408190526200003991620000bc565b600080546001600160a01b038085166001600160a01b0319928316179092556001805492841692909116919091178155620000759043620000f4565b60025550600380546001600160a01b0319166001600160a01b03929092169190911790556200011c565b80516001600160a01b0381168114620000b757600080fd5b919050565b60008060408385031215620000d057600080fd5b620000db836200009f565b9150620000eb602084016200009f565b90509250929050565b818103818111156200011657634e487b7160e01b600052601160045260246000fd5b92915050565b612120806200012c6000396000f3fe608060405234801561001057600080fd5b50600436106101c45760003560e01c80639fcb88f1116100f9578063c6b677b511610097578063e1cd1fb111610071578063e1cd1fb1146103f0578063f45e2fcf14610414578063fecca51014610427578063ff1b636d1461043a57600080fd5b8063c6b677b5146103c2578063d37aec92146103ca578063d7b2aae6146103dd57600080fd5b8063b372e8a0116100d3578063b372e8a014610354578063b67f705814610367578063b8aaac3514610390578063c03882c2146103b957600080fd5b80639fcb88f1146102fa578063a74f36271461030d578063aac724351461033057600080fd5b80634c8d18cc1161016657806368e432b21161014057806368e432b2146102ae578063704b6c02146102c157806375d99a98146102d45780639cac8e0e146102e757600080fd5b80634c8d18cc14610275578063512200ac1461028857806351cff8d91461029b57600080fd5b806315700052116101a2578063157000521461020f5780632a12a623146102245780633db2c0f31461024f57806349ce8d861461026257600080fd5b80630204bd7e146101c957806307d914ed146101f25780630f28c97d14610209575b600080fd5b6101dc6101d736600461178d565b61044d565b6040516101e991906117ec565b60405180910390f35b6101fb60025481565b6040519081526020016101e9565b426101fb565b61022261021d366004611822565b6104e7565b005b600354610237906001600160a01b031681565b6040516001600160a01b0390911681526020016101e9565b61023761025d366004611886565b61053c565b6102226102703660046118c8565b610569565b61022261028336600461198b565b6107b3565b6102226102963660046119df565b610852565b6102226102a9366004611822565b610de0565b6102226102bc366004611822565b610f1f565b6102226102cf366004611822565b610f6d565b6102226102e2366004611a6f565b610fbb565b6101dc6102f536600461178d565b61104a565b6101dc610308366004611886565b6110ec565b61032061031b366004611a9b565b61119e565b60405190151581526020016101e9565b61034361033e366004611b15565b6112d3565b6040516101e9959493929190611b37565b600154610237906001600160a01b031681565b61023761037536600461178d565b6000908152600860205260409020546001600160a01b031690565b61023761039e36600461178d565b6008602052600090815260409020546001600160a01b031681565b6101fb60075481565b6101dc6115b4565b6101fb6103d8366004611886565b6115c1565b6102226103eb366004611886565b6115f7565b6104036103fe366004611b15565b611630565b6040516101e9959493929190611ba4565b610222610422366004611bd5565b611663565b610222610435366004611c21565b6116e3565b600054610237906001600160a01b031681565b6009602052600090815260409020805461046690611c8d565b80601f016020809104026020016040519081016040528092919081815260200182805461049290611c8d565b80156104df5780601f106104b4576101008083540402835291602001916104df565b820191906000526020600020905b8154815290600101906020018083116104c257829003601f168201915b505050505081565b6001546001600160a01b0316331461051a5760405162461bcd60e51b815260040161051190611cc7565b60405180910390fd5b600180546001600160a01b0319166001600160a01b0392909216919091179055565b60008061054984846115c1565b6000908152600860205260409020546001600160a01b0316949350505050565b6000546001600160a01b031633148061058c57506001546001600160a01b031633145b61059557600080fd5b60808511156105ca5760405162461bcd60e51b81526020600482015260016024820152603160f81b6044820152606401610511565b6104008311156105ec5760405162461bcd60e51b815260040161051190611cc7565b6104008111156106225760405162461bcd60e51b81526020600482015260016024820152603360f81b6044820152606401610511565b60006040518060a001604052808a815260200189815260200188888080601f016020809104026020016040519081016040528093929190818152602001838380828437600092019190915250505090825250604080516020601f890181900481028201810190925287815291810191908890889081908401838280828437600092019190915250505090825250604080516020601f87018190048102820181019092528581529181019190869086908190840183828082843760009201829052509390945250508d81526005602090815260408083208f84528252918290208451815590840151600182015590830151929350839290915060028201906107299082611d3e565b506060820151600382019061073e9082611d3e565b50608082015160048201906107539082611d3e565b50905050888a8c7f62c4bcb61c8b3ce5a58a1e766589979cb544814f711a158750a5ab10edbee1518b8b8b8b8b8b8b61078a611773565b60405161079e989796959493929190611e27565b60405180910390a45050505050505050505050565b6000546001600160a01b03163314806107d657506001546001600160a01b031633145b6107df57600080fd5b60006107eb84846115c1565b9050600081116108225760405162461bcd60e51b8152602060048201526002602482015261158d60f21b6044820152606401610511565b600090815260086020526040902080546001600160a01b0319166001600160a01b03929092169190911790555050565b6000546001600160a01b031633148061087557506001546001600160a01b031633145b61087e57600080fd5b600581146108b25760405162461bcd60e51b81526020600482015260016024820152600360fc1b6044820152606401610511565b610400828260008181106108c8576108c8611e7f565b90506020028101906108da9190611e95565b9050111561090e5760405162461bcd60e51b81526020600482015260016024820152603160f81b6044820152606401610511565b6104008282600181811061092457610924611e7f565b90506020028101906109369190611e95565b905011156109565760405162461bcd60e51b815260040161051190611cc7565b6104008282600281811061096c5761096c611e7f565b905060200281019061097e9190611e95565b905011156109b25760405162461bcd60e51b81526020600482015260016024820152603360f81b6044820152606401610511565b610400828260038181106109c8576109c8611e7f565b90506020028101906109da9190611e95565b90501115610a0e5760405162461bcd60e51b81526020600482015260016024820152600d60fa1b6044820152606401610511565b61040082826004818110610a2457610a24611e7f565b9050602002810190610a369190611e95565b90501115610a6a5760405162461bcd60e51b81526020600482015260016024820152603560f81b6044820152606401610511565b60006040518060a0016040528084846000818110610a8a57610a8a611e7f565b9050602002810190610a9c9190611e95565b8080601f01602080910402602001604051908101604052809392919081815260200183838082843760009201919091525050509082525060200184846001818110610ae957610ae9611e7f565b9050602002810190610afb9190611e95565b8080601f01602080910402602001604051908101604052809392919081815260200183838082843760009201919091525050509082525060200184846002818110610b4857610b48611e7f565b9050602002810190610b5a9190611e95565b8080601f01602080910402602001604051908101604052809392919081815260200183838082843760009201919091525050509082525060200184846003818110610ba757610ba7611e7f565b9050602002810190610bb99190611e95565b8080601f01602080910402602001604051908101604052809392919081815260200183838082843760009201919091525050509082525060200184846004818110610c0657610c06611e7f565b9050602002810190610c189190611e95565b8080601f01602080910402602001604051908101604052809392919081815260200183838082843760009201829052509390945250508881526006602090815260408083208a84529091529020825192935083929091508190610c7b9082611d3e565b5060208201516001820190610c909082611d3e565b5060408201516002820190610ca59082611d3e565b5060608201516003820190610cba9082611d3e565b5060808201516004820190610ccf9082611d3e565b5090505083857f8bc079df9b30e6f80b11cc9d2ebb61a2dd55af77450bfc34673896eaf07c67bc8886866000818110610d0a57610d0a611e7f565b9050602002810190610d1c9190611e95565b88886001818110610d2f57610d2f611e7f565b9050602002810190610d419190611e95565b8a8a6002818110610d5457610d54611e7f565b9050602002810190610d669190611e95565b8c8c6003818110610d7957610d79611e7f565b9050602002810190610d8b9190611e95565b8e8e6004818110610d9e57610d9e611e7f565b9050602002810190610db09190611e95565b610db8611773565b604051610dd09c9b9a99989796959493929190611edc565b60405180910390a3505050505050565b6001600160a01b038116610e3557478015610e31576003546040516001600160a01b039091169082156108fc029083906000818181858888f19350505050158015610e2f573d6000803e3d6000fd5b505b5050565b6040516370a0823160e01b81523060048201526000906001600160a01b038316906370a0823190602401602060405180830381865afa158015610e7c573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190610ea09190611f62565b90508015610e315760035460405163a9059cbb60e01b81526001600160a01b039182166004820152602481018390529083169063a9059cbb906044016020604051808303816000875af1158015610efb573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190610e2f9190611f7b565b6000546001600160a01b0316331480610f4257506001546001600160a01b031633145b610f4b57600080fd5b600380546001600160a01b0319166001600160a01b0392909216919091179055565b6000546001600160a01b0316331480610f9057506001546001600160a01b031633145b610f9957600080fd5b600080546001600160a01b0319166001600160a01b0392909216919091179055565b6000546001600160a01b0316331480610fde57506001546001600160a01b031633145b610fe757600080fd5b6000821161101c5760405162461bcd60e51b8152602060048201526002602482015261563360f01b6044820152606401610511565b60009182526008602052604090912080546001600160a01b0319166001600160a01b03909216919091179055565b600081815260096020526040902080546060919061106790611c8d565b80601f016020809104026020016040519081016040528092919081815260200182805461109390611c8d565b80156110e05780601f106110b5576101008083540402835291602001916110e0565b820191906000526020600020905b8154815290600101906020018083116110c357829003601f168201915b50505050509050919050565b606060006110fa84846115c1565b600081815260096020526040902080549192509061111790611c8d565b80601f016020809104026020016040519081016040528092919081815260200182805461114390611c8d565b80156111905780601f1061116557610100808354040283529160200191611190565b820191906000526020600020905b81548152906001019060200180831161117357829003601f168201915b505050505091505092915050565b600080546001600160a01b03163314806111c257506001546001600160a01b031633145b6111cb57600080fd5b8560001080156111e2575067800000000000000086105b6112135760405162461bcd60e51b8152602060048201526002602482015261503160f01b6044820152606401610511565b8315801590611223575061010084105b6112545760405162461bcd60e51b8152602060048201526002602482015261281960f11b6044820152606401610511565b816112865760405162461bcd60e51b8152602060048201526002602482015261503360f01b6044820152606401610511565b7fdfff0162547cc8c9ee96872dfca97d5962213c067703187ceabdb335ab941b7e8686868686336040516112bf96959493929190611f9d565b60405180910390a150600195945050505050565b60066020908152600092835260408084209091529082529020805481906112f990611c8d565b80601f016020809104026020016040519081016040528092919081815260200182805461132590611c8d565b80156113725780601f1061134757610100808354040283529160200191611372565b820191906000526020600020905b81548152906001019060200180831161135557829003601f168201915b50505050509080600101805461138790611c8d565b80601f01602080910402602001604051908101604052809291908181526020018280546113b390611c8d565b80156114005780601f106113d557610100808354040283529160200191611400565b820191906000526020600020905b8154815290600101906020018083116113e357829003601f168201915b50505050509080600201805461141590611c8d565b80601f016020809104026020016040519081016040528092919081815260200182805461144190611c8d565b801561148e5780601f106114635761010080835404028352916020019161148e565b820191906000526020600020905b81548152906001019060200180831161147157829003601f168201915b5050505050908060030180546114a390611c8d565b80601f01602080910402602001604051908101604052809291908181526020018280546114cf90611c8d565b801561151c5780601f106114f15761010080835404028352916020019161151c565b820191906000526020600020905b8154815290600101906020018083116114ff57829003601f168201915b50505050509080600401805461153190611c8d565b80601f016020809104026020016040519081016040528092919081815260200182805461155d90611c8d565b80156115aa5780601f1061157f576101008083540402835291602001916115aa565b820191906000526020600020905b81548152906001019060200180831161158d57829003601f168201915b5050505050905085565b6004805461046690611c8d565b60008083836040516020016115d7929190611fe6565b60408051808303601f190181529190528051602090910120949350505050565b6000546001600160a01b031633148061161a57506001546001600160a01b031633145b61162357600080fd5b6004610e2f828483612002565b60056020908152600092835260408084209091529082529020805460018201546002830180549293919261141590611c8d565b6000546001600160a01b031633148061168657506001546001600160a01b031633145b61168f57600080fd5b600083116116c45760405162461bcd60e51b8152602060048201526002602482015261563160f01b6044820152606401610511565b60008381526009602052604090206116dd828483612002565b50505050565b6000546001600160a01b031633148061170657506001546001600160a01b031633145b61170f57600080fd5b600061171b85856115c1565b9050600081116117525760405162461bcd60e51b81526020600482015260026024820152612b1960f11b6044820152606401610511565b600081815260096020526040902061176b838583612002565b505050505050565b600780549081906000611785836120c3565b919050555090565b60006020828403121561179f57600080fd5b5035919050565b6000815180845260005b818110156117cc576020818501810151868301820152016117b0565b506000602082860101526020601f19601f83011685010191505092915050565b6020815260006117ff60208301846117a6565b9392505050565b80356001600160a01b038116811461181d57600080fd5b919050565b60006020828403121561183457600080fd5b6117ff82611806565b60008083601f84011261184f57600080fd5b50813567ffffffffffffffff81111561186757600080fd5b60208301915083602082850101111561187f57600080fd5b9250929050565b6000806020838503121561189957600080fd5b823567ffffffffffffffff8111156118b057600080fd5b6118bc8582860161183d565b90969095509350505050565b60008060008060008060008060008060e08b8d0312156118e757600080fd5b8a35995060208b0135985060408b0135975060608b0135965060808b013567ffffffffffffffff8082111561191b57600080fd5b6119278e838f0161183d565b909850965060a08d013591508082111561194057600080fd5b61194c8e838f0161183d565b909650945060c08d013591508082111561196557600080fd5b506119728d828e0161183d565b915080935050809150509295989b9194979a5092959850565b6000806000604084860312156119a057600080fd5b833567ffffffffffffffff8111156119b757600080fd5b6119c38682870161183d565b90945092506119d6905060208501611806565b90509250925092565b6000806000806000608086880312156119f757600080fd5b853594506020860135935060408601359250606086013567ffffffffffffffff80821115611a2457600080fd5b818801915088601f830112611a3857600080fd5b813581811115611a4757600080fd5b8960208260051b8501011115611a5c57600080fd5b9699959850939650602001949392505050565b60008060408385031215611a8257600080fd5b82359150611a9260208401611806565b90509250929050565b600080600080600060608688031215611ab357600080fd5b85359450602086013567ffffffffffffffff80821115611ad257600080fd5b611ade89838a0161183d565b90965094506040880135915080821115611af757600080fd5b50611b048882890161183d565b969995985093965092949392505050565b60008060408385031215611b2857600080fd5b50508035926020909101359150565b60a081526000611b4a60a08301886117a6565b8281036020840152611b5c81886117a6565b90508281036040840152611b7081876117a6565b90508281036060840152611b8481866117a6565b90508281036080840152611b9881856117a6565b98975050505050505050565b85815284602082015260a060408201526000611bc360a08301866117a6565b8281036060840152611b8481866117a6565b600080600060408486031215611bea57600080fd5b83359250602084013567ffffffffffffffff811115611c0857600080fd5b611c148682870161183d565b9497909650939450505050565b60008060008060408587031215611c3757600080fd5b843567ffffffffffffffff80821115611c4f57600080fd5b611c5b8883890161183d565b90965094506020870135915080821115611c7457600080fd5b50611c818782880161183d565b95989497509550505050565b600181811c90821680611ca157607f821691505b602082108103611cc157634e487b7160e01b600052602260045260246000fd5b50919050565b6020808252600190820152601960f91b604082015260600190565b634e487b7160e01b600052604160045260246000fd5b601f821115610e2f57600081815260208120601f850160051c81016020861015611d1f5750805b601f850160051c820191505b8181101561176b57828155600101611d2b565b815167ffffffffffffffff811115611d5857611d58611ce2565b611d6c81611d668454611c8d565b84611cf8565b602080601f831160018114611da15760008415611d895750858301515b600019600386901b1c1916600185901b17855561176b565b600085815260208120601f198616915b82811015611dd057888601518255948401946001909101908401611db1565b5085821015611dee5787850151600019600388901b60f8161c191681555b5050505050600190811b01905550565b81835281816020850137506000828201602090810191909152601f909101601f19169091010190565b88815260a060208201526000611e4160a08301898b611dfe565b8281036040840152611e5481888a611dfe565b90508281036060840152611e69818688611dfe565b9150508260808301529998505050505050505050565b634e487b7160e01b600052603260045260246000fd5b6000808335601e19843603018112611eac57600080fd5b83018035915067ffffffffffffffff821115611ec757600080fd5b60200191503681900382131561187f57600080fd5b8c815260e060208201526000611ef660e083018d8f611dfe565b8281036040840152611f09818c8e611dfe565b90508281036060840152611f1e818a8c611dfe565b90508281036080840152611f3381888a611dfe565b905082810360a0840152611f48818688611dfe565b9150508260c08301529d9c50505050505050505050505050565b600060208284031215611f7457600080fd5b5051919050565b600060208284031215611f8d57600080fd5b815180151581146117ff57600080fd5b868152608060208201526000611fb7608083018789611dfe565b8281036040840152611fca818688611dfe565b91505060018060a01b0383166060830152979650505050505050565b602081526000611ffa602083018486611dfe565b949350505050565b67ffffffffffffffff83111561201a5761201a611ce2565b61202e836120288354611c8d565b83611cf8565b6000601f841160018114612062576000851561204a5750838201355b600019600387901b1c1916600186901b1783556120bc565b600083815260209020601f19861690835b828110156120935786850135825560209485019460019092019101612073565b50868210156120b05760001960f88860031b161c19848701351681555b505060018560011b0183555b5050505050565b6000600182016120e357634e487b7160e01b600052601160045260246000fd5b506001019056fea26469706673582212200a0763462229b6cd0bc6f2fee55000e8953ace1fef7cb53d5c673282c13bc4a864736f6c63430008110033";
        public AppInfoDeploymentBase() : base(BYTECODE) { }
        public AppInfoDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address", "admin_", 1)]
        public virtual string Admin_ { get; set; }
        [Parameter("address", "superAdmin_", 2)]
        public virtual string SuperAdmin_ { get; set; }
    }

    public partial class AdminFunction : AdminFunctionBase { }

    [Function("Admin", "address")]
    public class AdminFunctionBase : FunctionMessage
    {

    }

    public partial class ContactInfoFunction : ContactInfoFunctionBase { }

    [Function("ContactInfo", "string")]
    public class ContactInfoFunctionBase : FunctionMessage
    {

    }

    public partial class CurAppDownloadOfFunction : CurAppDownloadOfFunctionBase { }

    [Function("CurAppDownloadOf", typeof(CurAppDownloadOfOutputDTO))]
    public class CurAppDownloadOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class CurAppVersionOfFunction : CurAppVersionOfFunctionBase { }

    [Function("CurAppVersionOf", typeof(CurAppVersionOfOutputDTO))]
    public class CurAppVersionOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class CurEventIdFunction : CurEventIdFunctionBase { }

    [Function("CurEventId", "uint256")]
    public class CurEventIdFunctionBase : FunctionMessage
    {

    }

    public partial class DonationFunction : DonationFunctionBase { }

    [Function("Donation", "address")]
    public class DonationFunctionBase : FunctionMessage
    {

    }

    public partial class FromBlockFunction : FromBlockFunctionBase { }

    [Function("FromBlock", "uint256")]
    public class FromBlockFunctionBase : FunctionMessage
    {

    }

    public partial class KeyAddressFunction : KeyAddressFunctionBase { }

    [Function("KeyAddress", "address")]
    public class KeyAddressFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class KeyStringFunction : KeyStringFunctionBase { }

    [Function("KeyString", "string")]
    public class KeyStringFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class PublishNoticeFunction : PublishNoticeFunctionBase { }

    [Function("PublishNotice", "bool")]
    public class PublishNoticeFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_appId", 1)]
        public virtual BigInteger AppId { get; set; }
        [Parameter("string", "_subject", 2)]
        public virtual string Subject { get; set; }
        [Parameter("string", "_body", 3)]
        public virtual string Body { get; set; }
    }

    public partial class SuperAdminFunction : SuperAdminFunctionBase { }

    [Function("SuperAdmin", "address")]
    public class SuperAdminFunctionBase : FunctionMessage
    {

    }

    public partial class GetCurrentBlockTimestampFunction : GetCurrentBlockTimestampFunctionBase { }

    [Function("getCurrentBlockTimestamp", "uint256")]
    public class GetCurrentBlockTimestampFunctionBase : FunctionMessage
    {

    }

    public partial class GetKeyFunction : GetKeyFunctionBase { }

    [Function("getKey", "uint256")]
    public class GetKeyFunctionBase : FunctionMessage
    {
        [Parameter("string", "_strkey", 1)]
        public virtual string Strkey { get; set; }
    }

    public partial class GetKeyAddressFunction : GetKeyAddressFunctionBase { }

    [Function("getKeyAddress", "address")]
    public class GetKeyAddressFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_key", 1)]
        public virtual BigInteger Key { get; set; }
    }

    public partial class GetKeyAddress1Function : GetKeyAddress1FunctionBase { }

    [Function("getKeyAddress1", "address")]
    public class GetKeyAddress1FunctionBase : FunctionMessage
    {
        [Parameter("string", "_strkey", 1)]
        public virtual string Strkey { get; set; }
    }

    public partial class GetKeyStringFunction : GetKeyStringFunctionBase { }

    [Function("getKeyString", "string")]
    public class GetKeyStringFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_key", 1)]
        public virtual BigInteger Key { get; set; }
    }

    public partial class GetKeyString1Function : GetKeyString1FunctionBase { }

    [Function("getKeyString1", "string")]
    public class GetKeyString1FunctionBase : FunctionMessage
    {
        [Parameter("string", "_strkey", 1)]
        public virtual string Strkey { get; set; }
    }

    public partial class PublishAppDownloadFunction : PublishAppDownloadFunctionBase { }

    [Function("publishAppDownload")]
    public class PublishAppDownloadFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_AppId", 1)]
        public virtual BigInteger AppId { get; set; }
        [Parameter("uint256", "_PlatformId", 2)]
        public virtual BigInteger PlatformId { get; set; }
        [Parameter("uint256", "_Version", 3)]
        public virtual BigInteger Version { get; set; }
        [Parameter("string[]", "_Links", 4)]
        public virtual List<string> Links { get; set; }
    }

    public partial class PublishAppVersionFunction : PublishAppVersionFunctionBase { }

    [Function("publishAppVersion")]
    public class PublishAppVersionFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_AppId", 1)]
        public virtual BigInteger AppId { get; set; }
        [Parameter("uint256", "_PlatformId", 2)]
        public virtual BigInteger PlatformId { get; set; }
        [Parameter("uint256", "_Version", 3)]
        public virtual BigInteger Version { get; set; }
        [Parameter("bytes32", "_Sha256Value", 4)]
        public virtual byte[] Sha256Value { get; set; }
        [Parameter("string", "_AppName", 5)]
        public virtual string AppName { get; set; }
        [Parameter("string", "_UpdateInfo", 6)]
        public virtual string UpdateInfo { get; set; }
        [Parameter("string", "_IconUri", 7)]
        public virtual string IconUri { get; set; }
    }

    public partial class SaveKeyAddressFunction : SaveKeyAddressFunctionBase { }

    [Function("saveKeyAddress")]
    public class SaveKeyAddressFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_key", 1)]
        public virtual BigInteger Key { get; set; }
        [Parameter("address", "_value", 2)]
        public virtual string Value { get; set; }
    }

    public partial class SaveKeyAddress1Function : SaveKeyAddress1FunctionBase { }

    [Function("saveKeyAddress1")]
    public class SaveKeyAddress1FunctionBase : FunctionMessage
    {
        [Parameter("string", "_strkey", 1)]
        public virtual string Strkey { get; set; }
        [Parameter("address", "_value", 2)]
        public virtual string Value { get; set; }
    }

    public partial class SaveKeyStringFunction : SaveKeyStringFunctionBase { }

    [Function("saveKeyString")]
    public class SaveKeyStringFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_key", 1)]
        public virtual BigInteger Key { get; set; }
        [Parameter("string", "_value", 2)]
        public virtual string Value { get; set; }
    }

    public partial class SaveKeyString1Function : SaveKeyString1FunctionBase { }

    [Function("saveKeyString1")]
    public class SaveKeyString1FunctionBase : FunctionMessage
    {
        [Parameter("string", "_strkey", 1)]
        public virtual string Strkey { get; set; }
        [Parameter("string", "_value", 2)]
        public virtual string Value { get; set; }
    }

    public partial class SetAdminFunction : SetAdminFunctionBase { }

    [Function("setAdmin")]
    public class SetAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "_value", 1)]
        public virtual string Value { get; set; }
    }

    public partial class SetContactInfoFunction : SetContactInfoFunctionBase { }

    [Function("setContactInfo")]
    public class SetContactInfoFunctionBase : FunctionMessage
    {
        [Parameter("string", "_ContactInfo", 1)]
        public virtual string ContactInfo { get; set; }
    }

    public partial class SetDonationFunction : SetDonationFunctionBase { }

    [Function("setDonation")]
    public class SetDonationFunctionBase : FunctionMessage
    {
        [Parameter("address", "_value", 1)]
        public virtual string Value { get; set; }
    }

    public partial class SetSuperAdminFunction : SetSuperAdminFunctionBase { }

    [Function("setSuperAdmin")]
    public class SetSuperAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "_value", 1)]
        public virtual string Value { get; set; }
    }

    public partial class WithdrawFunction : WithdrawFunctionBase { }

    [Function("withdraw")]
    public class WithdrawFunctionBase : FunctionMessage
    {
        [Parameter("address", "_token", 1)]
        public virtual string Token { get; set; }
    }

    public partial class OnPublishAppDownloadEventDTO : OnPublishAppDownloadEventDTOBase { }

    [Event("OnPublishAppDownload")]
    public class OnPublishAppDownloadEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "_AppId", 1, false )]
        public virtual BigInteger AppId { get; set; }
        [Parameter("uint256", "_PlatformId", 2, true )]
        public virtual BigInteger PlatformId { get; set; }
        [Parameter("uint256", "_version", 3, true )]
        public virtual BigInteger Version { get; set; }
        [Parameter("string", "_HttpLink", 4, false )]
        public virtual string HttpLink { get; set; }
        [Parameter("string", "_BTLink", 5, false )]
        public virtual string BTLink { get; set; }
        [Parameter("string", "_eMuleLink", 6, false )]
        public virtual string EMuleLink { get; set; }
        [Parameter("string", "_IpfsLink", 7, false )]
        public virtual string IpfsLink { get; set; }
        [Parameter("string", "_OtherLink", 8, false )]
        public virtual string OtherLink { get; set; }
        [Parameter("uint256", "_eventId", 9, false )]
        public virtual BigInteger EventId { get; set; }
    }

    public partial class OnPublishAppVersionEventDTO : OnPublishAppVersionEventDTOBase { }

    [Event("OnPublishAppVersion")]
    public class OnPublishAppVersionEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "_AppId", 1, true )]
        public virtual BigInteger AppId { get; set; }
        [Parameter("uint256", "_PlatformId", 2, true )]
        public virtual BigInteger PlatformId { get; set; }
        [Parameter("uint256", "_Version", 3, true )]
        public virtual BigInteger Version { get; set; }
        [Parameter("bytes32", "_Sha256Value", 4, false )]
        public virtual byte[] Sha256Value { get; set; }
        [Parameter("string", "_AppName", 5, false )]
        public virtual string AppName { get; set; }
        [Parameter("string", "_UpdateInfo", 6, false )]
        public virtual string UpdateInfo { get; set; }
        [Parameter("string", "_IconUri", 7, false )]
        public virtual string IconUri { get; set; }
        [Parameter("uint256", "_eventId", 8, false )]
        public virtual BigInteger EventId { get; set; }
    }

    public partial class OnPublishNoticeEventDTO : OnPublishNoticeEventDTOBase { }

    [Event("OnPublishNotice")]
    public class OnPublishNoticeEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "_appId", 1, false )]
        public virtual BigInteger AppId { get; set; }
        [Parameter("string", "_subject", 2, false )]
        public virtual string Subject { get; set; }
        [Parameter("string", "_body", 3, false )]
        public virtual string Body { get; set; }
        [Parameter("address", "_publisher", 4, false )]
        public virtual string Publisher { get; set; }
    }

    public partial class AdminOutputDTO : AdminOutputDTOBase { }

    [FunctionOutput]
    public class AdminOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ContactInfoOutputDTO : ContactInfoOutputDTOBase { }

    [FunctionOutput]
    public class ContactInfoOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class CurAppDownloadOfOutputDTO : CurAppDownloadOfOutputDTOBase { }

    [FunctionOutput]
    public class CurAppDownloadOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "HttpLink", 1)]
        public virtual string HttpLink { get; set; }
        [Parameter("string", "BTLink", 2)]
        public virtual string BTLink { get; set; }
        [Parameter("string", "eMuleLink", 3)]
        public virtual string EMuleLink { get; set; }
        [Parameter("string", "IpfsLink", 4)]
        public virtual string IpfsLink { get; set; }
        [Parameter("string", "OtherLink", 5)]
        public virtual string OtherLink { get; set; }
    }

    public partial class CurAppVersionOfOutputDTO : CurAppVersionOfOutputDTOBase { }

    [FunctionOutput]
    public class CurAppVersionOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "Version", 1)]
        public virtual BigInteger Version { get; set; }
        [Parameter("bytes32", "Sha256Value", 2)]
        public virtual byte[] Sha256Value { get; set; }
        [Parameter("string", "AppName", 3)]
        public virtual string AppName { get; set; }
        [Parameter("string", "UpdateInfo", 4)]
        public virtual string UpdateInfo { get; set; }
        [Parameter("string", "IconUri", 5)]
        public virtual string IconUri { get; set; }
    }

    public partial class CurEventIdOutputDTO : CurEventIdOutputDTOBase { }

    [FunctionOutput]
    public class CurEventIdOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DonationOutputDTO : DonationOutputDTOBase { }

    [FunctionOutput]
    public class DonationOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class FromBlockOutputDTO : FromBlockOutputDTOBase { }

    [FunctionOutput]
    public class FromBlockOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class KeyAddressOutputDTO : KeyAddressOutputDTOBase { }

    [FunctionOutput]
    public class KeyAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class KeyStringOutputDTO : KeyStringOutputDTOBase { }

    [FunctionOutput]
    public class KeyStringOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }



    public partial class SuperAdminOutputDTO : SuperAdminOutputDTOBase { }

    [FunctionOutput]
    public class SuperAdminOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetCurrentBlockTimestampOutputDTO : GetCurrentBlockTimestampOutputDTOBase { }

    [FunctionOutput]
    public class GetCurrentBlockTimestampOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "timestamp", 1)]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class GetKeyOutputDTO : GetKeyOutputDTOBase { }

    [FunctionOutput]
    public class GetKeyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetKeyAddressOutputDTO : GetKeyAddressOutputDTOBase { }

    [FunctionOutput]
    public class GetKeyAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetKeyAddress1OutputDTO : GetKeyAddress1OutputDTOBase { }

    [FunctionOutput]
    public class GetKeyAddress1OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetKeyStringOutputDTO : GetKeyStringOutputDTOBase { }

    [FunctionOutput]
    public class GetKeyStringOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetKeyString1OutputDTO : GetKeyString1OutputDTOBase { }

    [FunctionOutput]
    public class GetKeyString1OutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }






















}
