using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Wallet.BLL 

{ 

    /// <summary>
    /// ERC20 Token Approve 的查询授权, 先通过 EtherScan 查到所有交易，然后核对交易，如果是 Approve操作，就查询 授权金额 
    /// </summary>
public class Erc20TokenApprove
    {

        /// <summary>
        /// 得到授权金额
        /// </summary>
        /// <param name="token"></param>
        /// <param name="owner"></param>
        /// <param name="spender"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetApprovedAmount(string token, string owner, string spender)
        {
            Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();
            Nethereum.StandardTokenEIP20.StandardTokenService Erc20Token = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, token);

            var query = new Nethereum.StandardTokenEIP20.ContractDefinition.AllowanceFunction()
            {
                Owner = owner,
                Spender = spender,
            };
            var result = Erc20Token.AllowanceQueryAsync(query).Result;
            return result;
        }


        public async static Task<List<TokenApprove>> GetApprovedAmount(EtherSacnApi.AddressTxState ApiData)
        {
            List<TokenApprove> result = new List<TokenApprove>();

            if (ApiData != null && ApiData.status == 1 && ApiData.message == "OK" && ApiData.result != null && ApiData.result.Length > 0)
            {
                foreach (var data in ApiData.result)
                {
                    string funcSigCode = string.Empty;
                    if (!string.IsNullOrEmpty(data.input) && data.input.Length >= 10)
                    {
                        funcSigCode = data.input.Substring(0, 10);
                    }

                    if (funcSigCode == Erc20FunSigCode.approve)
                    {
                        var erc20token = data.to;
                        //function approve(address spender, uint256 amount) public   returns (bool) 
                        //function transfer(address recipient, uint256 amount) public   returns (bool)
                        var spender = data.input.Substring(10, 64);
                        spender = "0x" + spender.Substring(24, 40);                         //spender 的地址，来自于 Input 

                        string stTokenAmount = data.input.Substring(74, 64);                //输入的金额 这个没用，可以不要
                        var BigTokenAmount = Nethereum.Hex.HexConvertors.Extensions.HexBigIntegerConvertorExtensions.HexToBigInteger(stTokenAmount, false);

                        if (BigTokenAmount > 0)
                        {                      
                            Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();
                            Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, erc20token);
                            var amount = await service.AllowanceQueryAsync(data.from, spender);

                            if (amount > 0)
                            {
                                TokenApprove ta = new TokenApprove();
                                ta.BigApprovedAmount = amount;
                                ta.TokenAddress = erc20token;
                                ta.UserAddress = data.from;
                                ta.SpenderAddress = spender;
                                ta.SpenderIsContract = BLL.WalletHelper.IsContractAddress(spender) == true;    //标记是否合约地址
                                ta.TokenDecimals = service.DecimalsQueryAsync().Result;
                                ta.TokenSymbol = service.SymbolQueryAsync().Result;
                                ta.DecApprovedAmount =  (double)ta.BigApprovedAmount / Math.Pow(10, ta.TokenDecimals);

                                result.Add(ta);
                            }
                        }
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Aprove 的 Token 对象
        /// </summary>
        public class TokenApprove
        {
            /// <summary>
            /// 用户地址
            /// </summary>
            public string UserAddress { get; set; }

            /// <summary>
            /// Token 地址
            /// </summary>
            public string TokenAddress { get; set; }

            /// <summary>
            /// 授权地址（大富翁）
            /// </summary>
            public string SpenderAddress { get; set; }

            /// <summary>
            /// 授权地址是否合约地址
            /// </summary>
            public bool SpenderIsContract { get; set; }

            /// <summary>
            /// 授权金额， 没有除以 10**decimals ，
            /// </summary>
            public System.Numerics.BigInteger BigApprovedAmount { get; set; }

            /// <summary>
            /// 授权金额，
            /// </summary>
            public double DecApprovedAmount { get; set; }

            /// <summary>
            /// 小数位，主要用于计算 ApprovedAmount 
            /// </summary>
            public int TokenDecimals { get; set; }


            public string TokenSymbol { get; set; }


        }

    }

}
