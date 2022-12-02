using BlockChain.Share;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using System.Data;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using ZXing;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Math;

namespace BlockChain.Share.BLL
{
    public class Address : Share.IAddress
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public Account GetAccount(string address, string password)
        {
            try
            {
                var chainid = Share.ShareParam.GetChainId();    //20210814 add
                var km = DAL.KeyStore_Address.GetModel(Share.ShareParam.DbConStr, address);
                if (km != null)
                {
                    var jsoin = km.KeyStoreText;
                    var account = Nethereum.Web3.Accounts.Account.LoadFromKeyStore(jsoin, password, chainid);
                    return account;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception", ex);
            }

            return null;
        }

        public Account GetAccount(string address, string password, System.Numerics. BigInteger chainid)
        {
            try
            { 
                var km = DAL.KeyStore_Address.GetModel(Share.ShareParam.DbConStr, address);
                if (km != null)
                {
                    var jsoin = km.KeyStoreText;
                    var account = Nethereum.Web3.Accounts.Account.LoadFromKeyStore(jsoin, password, chainid);
                    return account;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception", ex);
            }

            return null;
        }


        public List<TxAddress> GetAllTxAddress()
        {
            string sql = @"
SELECT AddressAlias, Address, 'KeyStore' AS SourceName
FROM   KeyStore_Address
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return Share.TxAddress.DataTable2List(ds.Tables[0]);
        }


        public double GetRealBalance(string address, string token, bool isCache = false)
        {
            return ShareParam.GetRealBalance(address, token, isCache);           
        }


        public static List<TxAddress> GetRealAllAddressList()
        {
            return new Address().GetAllTxAddress();
        }


        public static bool UpdateAddressAlias(string address, string newAlias)
        {

            var km = DAL.KeyStore_Address.GetModel(Share.ShareParam.DbConStr, address);
            if (km != null)
            {
                km.AddressAlias = newAlias;
                DAL.KeyStore_Address.Update(Share.ShareParam.DbConStr, km);
                return true;
            }

            var hm = DAL.KeyStore_Address.GetModel(Share.ShareParam.DbConStr, address);
            if (hm != null)
            {
                hm.AddressAlias = newAlias;
                DAL.KeyStore_Address.Update(Share.ShareParam.DbConStr, hm);
                return true;
            }

            return false;
        }


        #region QueryAddressTokenAmount， 查询地址的token数量
              

        #region Add Time 2022/6/20 23:12:03

        [Serializable]
        public class AddressTokenAmount
        {
            #region 生成该数据实体的SQL语句
            public const string SQL = @"SELECT 'a' AS UserAddress, 0 AS Amount, Address AS TokenAddress, Symbol, ImagePath FROM Token";
            #endregion

            #region Public Properties

            private System.String _UserAddress = System.String.Empty;
            public System.String UserAddress
            {
                get { return _UserAddress; }
                set { _UserAddress = value; }
            }

            private System.Decimal _Amount;
            public System.Decimal Amount
            {
                get { return _Amount; }
                set { _Amount = value; }
            }

            private System.String _TokenAddress = System.String.Empty;
            public System.String TokenAddress
            {
                get { return _TokenAddress; }
                set { _TokenAddress = value; }
            }

            private System.String _Symbol = System.String.Empty;
            public System.String Symbol
            {
                get { return _Symbol; }
                set { _Symbol = value; }
            }

            private System.String _ImagePath = System.String.Empty;
            public System.String ImagePath
            {
                get { return _ImagePath; }
                set { _ImagePath = value; }
            }

            #endregion

            #region Public construct

            public AddressTokenAmount()
            {
            }
            
            public AddressTokenAmount(System.String AUserAddress, System.Int32 AAmount)
            {
                _UserAddress = AUserAddress;
                _Amount = AAmount;
            }

            #endregion

            #region Public DataRow2Object

            public static AddressTokenAmount DataRow2Object(System.Data.DataRow dr)
            {
                if (dr == null)
                {
                    return null;
                }
                AddressTokenAmount Obj = new AddressTokenAmount();
                Obj.UserAddress = dr["UserAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["UserAddress"]);
                Obj.Amount = dr["Amount"] == DBNull.Value ? 0 : (System.Decimal)(dr["Amount"]);
                Obj.TokenAddress = dr["TokenAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["TokenAddress"]);
                Obj.Symbol = dr["Symbol"] == DBNull.Value ? string.Empty : (System.String)(dr["Symbol"]);
                Obj.ImagePath = dr["ImagePath"] == DBNull.Value ? string.Empty : (System.String)(dr["ImagePath"]);
                return Obj;
            }

            #endregion


            public static List<AddressTokenAmount> DataTable2List(System.Data.DataTable dt)
            {
                List<AddressTokenAmount> result = new List<AddressTokenAmount>();
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    result.Add(AddressTokenAmount.DataRow2Object(dr));
                }
                return result;
            }



            private static AddressTokenAmount _NullObject = null;

            public static AddressTokenAmount NullObject
            {
                get
                {
                    if (_NullObject == null)
                    {
                        _NullObject = new AddressTokenAmount();
                    }
                    return _NullObject;
                }
            }


        }


        #endregion

        /// <summary>
        ///  这个没用到 ？？？ 被 QueryUserTokenBalance_MultiCall 替换了
        /// </summary>
        /// <param name="_UserAddress"></param>
        /// <returns></returns>
        public async Task< List<AddressTokenAmount> > QueryAddressTokenAmount(string _UserAddress)
        {
            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
           

            string sql = @"
SELECT   '"+ _UserAddress + @"' AS UserAddress, 0.0 AS Amount, Address AS TokenAddress, Symbol,  '" + BasePath + @"\' + ImagePath AS ImagePath
FROM      Token
WHERE   (ChainId = @ChainId)
";
            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            var ChainId = (int)ShareParam.GetChainId();
            cm.Parameters.Add("@ChainId", SqlDbType.Int).Value = ChainId;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            var list = AddressTokenAmount.DataTable2List( ds.Tables[0]);
            Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();

            foreach (var m in list)
            {
                if (Share.ShareParam.IsAnEmptyAddress(m.TokenAddress))
    {
                  var amount =((double) (await web3.Eth.GetBalance.SendRequestAsync(m.UserAddress)).Value ) / Math.Pow(10,18);
                    m.Amount = (decimal)amount;
                }
                else
                {
                Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, m.TokenAddress);
                    var amount =(double) await service.BalanceOfQueryAsync(m.UserAddress);
                    var d = await service.DecimalsQueryAsync();
                    amount = amount / Math.Pow(10, d);
                    m.Amount = (decimal)amount;
                }            
            }

            return list;
        }

        #endregion


        public async static Task<List<Model.UserTokenBalance>> QueryUserTokenBalance_MultiCall(string UserAddress, List<string> TokenList)
        {
            List<Model.UserTokenBalance> result = new List<Model.UserTokenBalance>();

            try
            {
                var web3 = Share.ShareParam.GetWeb3();

                List<IMulticallInputOutput> multiCalls = new List<Nethereum.Contracts.QueryHandlers.MultiCall.IMulticallInputOutput>();
                var UserBalanceOfMessage = new BalanceOfFunction() { Owner = UserAddress };

                foreach (string token in TokenList)
                {
                    if (Share.ShareParam.IsAnEmptyAddress(token))
                    {
                        var EthBalance = (await web3.Eth.GetBalance.SendRequestAsync(UserAddress)).Value;

                        Model.UserTokenBalance m = new Model.UserTokenBalance();
                        m.UserAddress = UserAddress;
                        m.TokenAddress = Share.ShareParam.AddressEth;
                        m.BigBalance = EthBalance;
                        m.Balance = (double)EthBalance / Math.Pow(10.0, 18);
                        result.Add(m);

                        continue;
                    }

                    var call = new MulticallInputOutput<BalanceOfFunction, BalanceOfOutputDTO>(UserBalanceOfMessage, token);
                    multiCalls.Add(call);
                    //var b = call.Output.Balance;
                }

                IMulticallInputOutput[] Calls = multiCalls.ToArray();
                var CallResult = await web3.Eth.GetMultiQueryHandler().MultiCallAsync(Calls);
                //下面这种方式也可以，更简单！
                //  var bl = await w3.Eth.ERC20.GetAllTokenBalancesUsingMultiCallAsync(address, tl.ToArray<string>());//, Common.CommonAddresses.ENS_REGISTRY_ADDRESS

                foreach (var c in Calls)
                {
                    var call = (MulticallInputOutput<BalanceOfFunction, BalanceOfOutputDTO>)c;
                    var Decimals = Share.BLL.Token.GetTokenDecimals(call.Target);

                    Model.UserTokenBalance m = new Model.UserTokenBalance();
                    m.UserAddress = UserAddress;
                    m.TokenAddress = call.Target;
                    m.BigBalance = call.Output.Balance;
                    m.Balance = (double)call.Output.Balance / Math.Pow(10.0, (double)Decimals);
                    result.Add(m);
                }
            }
            catch (Exception ex)
            {
                log.Error("UpdateBalance", ex);
            }
            return result;
        }


        }
}
