using BlockChain.Share;
using BlockChain.Wallet.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace BlockChain.Wallet.BLL
{
    public class Address : IAddress
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static DataSet GetAllAccountInfo()
        {
            string sql = @"
SELECT   *
FROM      HD_Mnemonic;
SELECT   *
FROM      KeyStore_Address;
SELECT   *
FROM      HD_Address;
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

            //DbScript.DataSetAccount ds = new DbScript.DataSetAccount();
            //da.Fill(ds);

            return ds;
        }

        public double GetRealBalance(string address, string token, bool isCache = false)
        {
            return ShareParam.GetRealBalance(address, token, isCache);

            //string key = address + "{A5DA013E-06CC-4C9A-A02B-AF34E60B1FFC}" + token;
            //if (isCache)
            //{
            //    var result = Common.Cache.GetData(key);
            //    if (result != null)
            //    {
            //        return (double)result;
            //    }
            //}

            //double amount = 0;
            //Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();

            //if (token == "0x0" || Share.ShareParam.IsAnEmptyAddress(token))
            //{
            //    amount = (double)(web3.Eth.GetBalance.SendRequestAsync(address).Result).Value;
            //    amount = amount / Math.Pow(10, 18.0);
            //    //return amount;
            //}
            //else
            //{
            //    Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, token);
            //    amount = (double)service.BalanceOfQueryAsync(address).Result;

            //    var tm = Share.BLL.Token.GetModel(token);
            //    amount = amount / Math.Pow(10, tm.Decimals);
            //    //return amount;
            //}

            //if (isCache)
            //{
            //    Common.Cache.AddByAbsoluteTime(key, amount, 5);     //缓存五秒钟
            //}
            //return amount;
        }

        public static void DeleteAllBalance(string address)
        {
            string sql = @"
DELETE FROM AddressBalance
WHERE   (UserAddress = @address)
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@address", SqlDbType.NVarChar).Value = address;

            cn.Open();
            try
            {
                cm.ExecuteNonQuery();
            }
            finally { cn.Close(); }
        }

        public static bool UpdateAddressAlias(string address, string newAlias)
        {

            var km = Share.DAL.KeyStore_Address.GetModel(Share.ShareParam.DbConStr, address);
            if (km != null)
            {
                km.AddressAlias = newAlias;
                Share.DAL.KeyStore_Address.Update(Share.ShareParam.DbConStr, km);
                return true;
            }

            var hm = DAL.HD_Address.GetModel(Share.ShareParam.DbConStr, address);
            if (hm != null)
            {
                hm.AddressAlias = newAlias;
                DAL.HD_Address.Update(Share.ShareParam.DbConStr, hm);
                return true;
            }

            return false;
        }


        public static List<Model.ViewAddressEthTokenBalance> GetHDAddressAmount(Guid MneId, string tokenAddress)
        {
            //            string sql = @"
            //SELECT  1 AS IsSelected, HD_Address.MneId, HD_Address.MneSecondSalt, HD_Address.AddressIndex, HD_Address.AddressAlias, 
            //                   HD_Address.Address, AddressBalance.Balance, 0.0 AS Amount
            //FROM      HD_Address LEFT OUTER JOIN
            //                   AddressBalance ON HD_Address.Address = AddressBalance.UserAddress AND 
            //                   AddressBalance.TokenAddress = @TokenAddress
            //WHERE   (HD_Address.MneId = @MneId)
            //";
            string sql = @"
SELECT  1 AS IsSelected, HD_Address.MneId, HD_Address.MneSecondSalt, HD_Address.AddressIndex, HD_Address.AddressAlias, 
                   HD_Address.Address, CONVERT(float, 
                   CASE WHEN AddressBalance_1.Balance = 0 THEN '0' ELSE CAST(AddressBalance_1.Balance AS varchar(64)) END) 
                   AS Balance, 0.0 AS Amount,
                       (SELECT  Balance
                        FROM       AddressBalance
                        WHERE    (TokenAddress = '0x0000000000000000000000000000000000000000') AND (UserAddress = HD_Address.Address)) AS EthAmount
FROM      HD_Address LEFT OUTER JOIN
                   AddressBalance AS AddressBalance_1 ON HD_Address.Address = AddressBalance_1.UserAddress AND 
                   AddressBalance_1.TokenAddress = @TokenAddress
WHERE   (HD_Address.MneId = @MneId)
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 64).Value = tokenAddress;
            cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier).Value = MneId;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            var dt = ds.Tables[0];

            return Model.ViewAddressEthTokenBalance.DataTable2List(dt);
        }


        public static List<Model.ViewAddressRealBalance> GetHDAddressAmount2(Guid MneId, string tokenAddress)
        {
            var tokenmodel = Share.BLL.Token.GetModel(tokenAddress);
            //var BlockNumber = Common.Web3Helper.GetNowBlockNuber(Share.ShareParam.Web3Url);
            //var web3 = Share.ShareParam.GetWeb3();

            string sql = @"
SELECT  AddressAlias, Address
FROM      HD_Address
WHERE   (MneId = @MneId)
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier).Value = MneId;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            var dt = ds.Tables[0];
            int i = 1;
            List<Model.ViewAddressRealBalance> result = new List<ViewAddressRealBalance>();

            foreach (DataRow row in dt.Rows)
            {
                Model.ViewAddressRealBalance m = new ViewAddressRealBalance();
                m.AddressAlias = row["AddressAlias"].ToString();
                m.Address = row["Address"].ToString();

                m.IsSelected = true;
                m.AddressIndex = i;
                i++;

                //var eth = await ShareParam.GetRealBalanceValue(m.Address, ShareParam.AddressEth);
                //m.EthValue = eth;
                //m.EthAmount = (decimal)((double)eth / Math.Pow(10, 18));

                //if (Share.ShareParam.IsAnEmptyAddress(tokenAddress))
                //{
                //    var tokenvalue = eth;
                //    m.TokenValue = m.EthValue;
                //    m.TokenAmount = m.EthAmount;
                //    m.TokenSymbol = tokenmodel.Symbol;      //ETH
                //}
                //else
                //{
                //    var tokenvalue = await ShareParam.GetRealBalanceValue(m.Address, tokenAddress);
                //    m.TokenValue = tokenvalue;
                //    m.TokenAmount = (decimal)((double)tokenvalue / Math.Pow(10, tokenmodel.Decimals));
                //    m.TokenSymbol = tokenmodel.Symbol;
                //}

                //m.TokenAmountTransfer = 0;
                //m.BlockNumber = BlockNumber;

                //m.Nonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(m.Address);      // ShareParam.GetNonce(m.Address);

                //下面的做法是直接从数据库读取, 如果数据库没有，就显示 0 ！
                var eth = BLL.Address.GetAddressBalance(m.Address, ShareParam.AddressEth);
                m.EthValue = (System.Numerics.BigInteger)((double)eth * Math.Pow(10, 18));      //这个值不准 todo:
                m.EthAmount = eth;

                if (Share.ShareParam.IsAnEmptyAddress(tokenAddress))
                {
                    var tokenvalue = eth;
                    m.TokenValue = m.EthValue;
                    m.TokenAmount = m.EthAmount;
                    m.TokenSymbol = tokenmodel.Symbol;      //ETH
                }
                else
                {
                    var tokenvalue = BLL.Address.GetAddressBalance(m.Address, tokenAddress);
                    m.TokenValue = (System.Numerics.BigInteger)((double)tokenvalue * Math.Pow(10, tokenmodel.Decimals));   //这个值不准 todo:
                    m.TokenAmount = tokenvalue;
                    m.TokenSymbol = tokenmodel.Symbol;
                }

                m.TokenAmountTransfer = 0;
                m.BlockNumber = 0;

                m.Nonce = 0;

                result.Add(m);
            }

            return result;
        }


        public static decimal GetAddressBalance(string address, string tokenAddress)
        {
            var model = DAL.AddressBalance.GetModel(Share.ShareParam.DbConStr, address, tokenAddress);
            if (model != null)
            {
                return model.Balance;
            }
            else
                return 0;

        }


        //通过地址，密码，得到 account 无论 keystore 还是 hd , 这个方法会常常用到。
        public Nethereum.Web3.Accounts.Account GetAccount(string address, string password)
        {
            try
            {
                var chainid = Share.ShareParam.GetChainId();    //20210814 add
                var km = Share.DAL.KeyStore_Address.GetModel(Share.ShareParam.DbConStr, address);
                if (km != null)
                {
                    var jsoin = km.KeyStoreText;
                    var account = Nethereum.Web3.Accounts.Account.LoadFromKeyStore(jsoin, password, chainid);
                    return account;
                }

                var hm = DAL.HD_Address.GetModel(Share.ShareParam.DbConStr, address);
                if (hm != null)
                {
                    var model = DAL.HD_Mnemonic.GetModel(Share.ShareParam.DbConStr, hm.MneId);
                    password = Common.PasswordHelper.GetHDRealPassword(model.Salt, password);
                    var mne = Common.Security.SymmetricalEncrypt.Decrypt(password, model.MneEncrypted);
                    string salt2 = hm.MneSecondSalt;
                    var salt = model.MneFirstSalt + salt2;
                    Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(mne, salt, model.MnePath);

                    var account = w.GetAccount(hm.AddressIndex, chainid);

                    //下面的代码是检查账号合法性！
                    var a = new Nethereum.Web3.Accounts.Account(account.PrivateKey.HexToByteArray());
                    if (!a.Address.IsTheSameAddress(account.Address) || !a.Address.IsTheSameAddress(address))
                    {
                        string err = @"!a.Address.IsTheSameAddress(account.Address) || a.Address.IsTheSameAddress(address), Address Is " + address;
                        throw new Exception(err);
                    }

                    return account;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception", ex);
            }

            return null;
        }



        //通过地址，密码，得到 account 无论 keystore 还是 hd , 这个方法会常常用到。
        public Nethereum.Web3.Accounts.Account GetOfflineAccount(string address, string password)
        {
            try
            {
                var km = Share.DAL.KeyStore_Address.GetModel(Share.ShareParam.DbConStr, address);
                if (km != null)
                {
                    var jsoin = km.KeyStoreText;
                    var account = Nethereum.Web3.Accounts.Account.LoadFromKeyStore(jsoin, password);
                    return account;
                }

                var hm = DAL.HD_Address.GetModel(Share.ShareParam.DbConStr, address);
                if (hm != null)
                {
                    var model = DAL.HD_Mnemonic.GetModel(Share.ShareParam.DbConStr, hm.MneId);
                    password = Common.PasswordHelper.GetHDRealPassword(model.Salt, password);
                    var mne = Common.Security.SymmetricalEncrypt.Decrypt(password, model.MneEncrypted);
                    string salt2 = hm.MneSecondSalt;
                    var salt = model.MneFirstSalt + salt2;
                    Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(mne, salt, model.MnePath);

                    var account = w.GetAccount(hm.AddressIndex);

                    //下面的代码是检查账号合法性！
                    var a = new Nethereum.Web3.Accounts.Account(account.PrivateKey.HexToByteArray());
                    if (!a.Address.IsTheSameAddress(account.Address) || !a.Address.IsTheSameAddress(address))
                    {
                        string err = @"!a.Address.IsTheSameAddress(account.Address) || a.Address.IsTheSameAddress(address), Address Is " + address;
                        throw new Exception(err);
                    }
                    return account;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception", ex);
            }

            return null;
        }


        //        /// <summary>
        //        /// 有 缓存
        //        /// </summary>
        //        /// <returns></returns>
        //        public static System.Data.DataTable GetAllTxAddress()
        //        {
        //            string key = "{C2960C7D-21F4-45AF-8A9D-0179596F51A0}";
        //            var result = Common.Cache.GetData(key);
        //            if (result == null)
        //            {
        //                result = _GetAllTxAddress();
        //                Common.Cache.AddByAbsoluteTime(key, result);

        //            }
        //            return (System.Data.DataTable)result;
        //        }

        //        /// <summary>
        //        /// 无 缓存
        //        /// </summary>
        //        /// <returns></returns>
        //        private static System.Data.DataTable _GetAllTxAddress()
        //        {
        //            string sql = @"
        //SELECT   TOP (300) AddressAlias, Address, SourceName
        //FROM      View_Address
        //WHERE   (IsTxAddress = 1)
        //";

        //            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
        //            SqlCommand cm = new SqlCommand();
        //            cm.Connection = cn;
        //            cm.CommandType = System.Data.CommandType.Text;
        //            cm.CommandText = sql;

        //            SqlDataAdapter da = new SqlDataAdapter();
        //            da.SelectCommand = cm;
        //            System.Data.DataSet ds = new System.Data.DataSet();
        //            da.Fill(ds);

        //            return ds.Tables[0];
        //        }


        /// <summary>
        /// 有 缓存
        /// </summary>
        /// <returns></returns>
        public List<Share.TxAddress> GetAllTxAddress()
        {
            string key = "{C2960C7D-21F4-45AF-8A9D-0179596F51A0}";
            var result = Common.Cache.GetData(key);
            if (result == null)
            {
                result = _GetAllTxAddress();
                Common.Cache.AddByAbsoluteTime(key, result);

            }
            return (List<Share.TxAddress>)result;
        }

        /// <summary>
        /// 无 缓存
        /// </summary>
        /// <returns></returns>
        private static List<Share.TxAddress> _GetAllTxAddress()
        {
            string sql = @"
SELECT   TOP (300) AddressAlias, Address, SourceName
FROM      View_Address
WHERE   (IsTxAddress = 1)
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


        /// <summary>
        /// 无 缓存
        /// </summary>
        /// <returns></returns>
        public static System.Data.DataTable GetRealAllAddress(string _filter, int _recordMin, int _recordMax)
        {
            if (string.IsNullOrWhiteSpace(_filter))
            {
                return GetRealAllAddress(_recordMin, _recordMax);
            }

            string sql = @"
select * from
(
SELECT   ROW_NUMBER() OVER(Order by IsTxAddress desc, SourceName) AS RowId, AddressAlias, Address, SourceName, IsTxAddress, HasPrivatekey
FROM      View_Address
WHERE   (AddressAlias LIKE '%' + @filter + '%') OR
                (Address LIKE '%' + @filter + '%') OR
                (SourceName LIKE '%' + @filter + '%')
) T
where @Rmin < RowId and RowId <= @Rmax
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@filter", SqlDbType.NVarChar, 64).Value = _filter;
            cm.Parameters.Add("@Rmin", SqlDbType.Int).Value = _recordMin;
            cm.Parameters.Add("@Rmax", SqlDbType.Int).Value = _recordMax;


            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }

        public static System.Data.DataTable GetRealAllAddress(int _recordMin, int _recordMax)
        {
            string sql = @"
select * from
(
SELECT   ROW_NUMBER() OVER(Order by  IsTxAddress desc, SourceName) AS RowId, AddressAlias, Address, SourceName, IsTxAddress, HasPrivatekey
FROM      View_Address
) T
where @Rmin < RowId and RowId <= @Rmax
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@Rmin", SqlDbType.Int).Value = _recordMin;
            cm.Parameters.Add("@Rmax", SqlDbType.Int).Value = _recordMax;


            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }


        public static int GetRealAllAddressCount(string _filter)
        {
            if (string.IsNullOrWhiteSpace(_filter))
            {
                return GetRealAllAddressCount();
            }

            string sql = @"
select count(*) 
FROM      View_Address
WHERE   (AddressAlias LIKE '%' + @filter + '%') OR
                (Address LIKE '%' + @filter + '%') OR
                (SourceName LIKE '%' + @filter + '%')

";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@filter", SqlDbType.NVarChar, 64).Value = _filter;

            cn.Open();
            try
            {
                var result = cm.ExecuteScalar();
                return (int)result;
            }
            finally
            {
                cn.Close();
            }


        }

        public static int GetRealAllAddressCount()
        {
            string sql = @"
select count(*) 
FROM      View_Address
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;


            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = cm;
            //System.Data.DataSet ds = new System.Data.DataSet();
            //da.Fill(ds);

            cn.Open();
            try
            {
                var result = cm.ExecuteScalar();
                return (int)result;
            }
            finally
            {
                cn.Close();
            }


        }


        public static void UpdateIsTxAddress(string _address, bool _isTxAddress)
        {
            string sql = @"
update HD_Address set IsTxAddress = @IsTxAddress where Address = @Address;
update KeyStore_Address set IsTxAddress = @IsTxAddress where Address = @Address and HasPrivatekey=1;
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@IsTxAddress", SqlDbType.Bit).Value = _isTxAddress;
            cm.Parameters.Add("@Address", SqlDbType.NVarChar, 64).Value = _address;

            cn.Open();
            try
            {
                var result = cm.ExecuteNonQuery();
            }
            finally
            {
                cn.Close();
            }


        }


        /// <summary>
        /// 得到 token 余额
        /// </summary>
        /// <param name="_recordMin"></param>
        /// <param name="_recordMax"></param>
        /// <returns></returns>
        public static List<Model.ViewUserTokenBalance> GetAllAddressBalance(int _recordMin, int _recordMax, string _FilterAddress)
        {
            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;

            //            string sql = @"
            //select * from
            //(
            //SELECT   ROW_NUMBER() OVER(Order by AddressBalance.Balance desc) AS RowId,    0.0 StablecoinAmount, '*' StablecoinSymbol,
            //'" + BasePath + @"\' + Token.ImagePath AS ImagePath, T.AddressAlias, AddressBalance.UserAddress, Token.Symbol, 
            //                AddressBalance.UpdateDateTime, AddressBalance.UpdateBlockNumber, CONVERT(float, 
            //                CASE WHEN AddressBalance.Balance = 0 THEN '0' ELSE CAST(AddressBalance.Balance AS varchar(64)) END) 
            //                AS Balance, Token.Address AS TokenAddress
            //FROM      AddressBalance INNER JOIN
            //                Token ON Token.Address = AddressBalance.TokenAddress INNER JOIN
            //                    (SELECT   AddressAlias, Address
            //                     FROM      KeyStore_Address
            //                     UNION
            //                     SELECT   AddressAlias, Address
            //                     FROM      HD_Address) AS T ON T.Address = AddressBalance.UserAddress
            //) T
            //where @Rmin < RowId and RowId <= @Rmax
            //";
            string sql = @"
select * from
(
SELECT   ROW_NUMBER() OVER(Order by AddressBalance.Balance desc) AS RowId,    0.0 StablecoinAmount, '*' StablecoinSymbol,
'" + BasePath + @"\' + Token.ImagePath AS ImagePath, T.AddressAlias, AddressBalance.UserAddress, Token.Symbol, 
                AddressBalance.UpdateDateTime, AddressBalance.UpdateBlockNumber, CONVERT(float, 
                CASE WHEN AddressBalance.Balance = 0 THEN '0' ELSE CAST(AddressBalance.Balance AS varchar(64)) END) 
                AS Balance, Token.Address AS TokenAddress
FROM      AddressBalance INNER JOIN
                Token ON Token.Address = AddressBalance.TokenAddress INNER JOIN
                    (SELECT   AddressAlias, Address
                     FROM      KeyStore_Address
                     UNION
                     SELECT   AddressAlias, Address
                     FROM      HD_Address) AS T ON T.Address = AddressBalance.UserAddress
where AddressBalance.UserAddress like @UserAddress
) T
where @Rmin < RowId and RowId <= @Rmax
";


            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar).Value = _FilterAddress;
            cm.Parameters.Add("@Rmin", SqlDbType.Int).Value = _recordMin;
            cm.Parameters.Add("@Rmax", SqlDbType.Int).Value = _recordMax;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            var bl = Model.ViewUserTokenBalance.DataTable2List(ds.Tables[0]);

            var Stablecoin = Share.BLL.Token.GetModel(Share.ShareParam.AddressPricingToken);
            if (Stablecoin != null)
            {

                foreach (Model.ViewUserTokenBalance m in bl)
                {
                    if (m.Balance > 0)
                    {
                        m.StablecoinSymbol = Stablecoin.Symbol;

                        var Token = Share.BLL.Token.GetModel(m.TokenAddress);
                        if (Token.PricingTokenPrice < 0)
                        {
                            Token.PricingTokenPrice = 0;
                        }
                        m.StablecoinAmount = Token.PricingTokenPrice * m.Balance;
                    }
                }
            }

            return bl;
        }


        /// <summary>
        /// 得到 token 余额
        /// </summary>
        /// <param name="_recordMin"></param>
        /// <param name="_recordMax"></param>
        /// <returns></returns>
        public static int GetAllAddressBalanceCount(string _FilterAddress)
        {
            string sql = @"
select count(*) 
FROM      AddressBalance INNER JOIN
                Token ON Token.Address = AddressBalance.TokenAddress INNER JOIN
                    (SELECT   AddressAlias, Address
                     FROM      KeyStore_Address
                     UNION
                     SELECT   AddressAlias, Address
                     FROM      HD_Address) AS T ON T.Address = AddressBalance.UserAddress
where AddressBalance.UserAddress like @UserAddress
";


            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar).Value = _FilterAddress;

            cn.Open();
            try
            {
                var result = cm.ExecuteScalar();
                return (int)result;
            }
            finally
            {
                cn.Close();
            }
        }



        /// <summary>
        /// 得到 各个地址累加 token 数量的统计 
        /// </summary>
        /// <returns></returns>
        public static List<Model.ViewUserTokenBalanceSummary> GetAddressBalanceSummary()
        {
            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;

            string sql = @"
SELECT   ImagePath, Symbol, TokenAddress, COUNT(*) AS AddressNum, SUM(Balance) AS AllBalance, 0.0 StablecoinAmount, '*' StablecoinSymbol
FROM      (SELECT   '" + BasePath + @"\' + Token.ImagePath AS ImagePath, T_1.AddressAlias, AddressBalance.UserAddress,                                   
                                 Token.Symbol, CONVERT(float, 
                                 CASE WHEN AddressBalance.Balance = 0 THEN '0' ELSE CAST(AddressBalance.Balance AS varchar(64)) 
                                 END) AS Balance, Token.Address AS TokenAddress
                 FROM      AddressBalance INNER JOIN
                                 Token ON Token.Address = AddressBalance.TokenAddress INNER JOIN
                                     (SELECT   AddressAlias, Address
                                      FROM      KeyStore_Address
                                      UNION
                                      SELECT   AddressAlias, Address
                                      FROM      HD_Address) AS T_1 ON T_1.Address = AddressBalance.UserAddress
                 WHERE   (AddressBalance.Balance > 0)) AS T
GROUP BY ImagePath, Symbol, TokenAddress
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

            var vl = Model.ViewUserTokenBalanceSummary.DataTable2List(ds.Tables[0]);

            var Stablecoin = Share.BLL.Token.GetModel(Share.ShareParam.AddressPricingToken);                //!!! 这里的逻辑错了！ 稳定币要取当前计价的稳定币，而不是当前设置的稳定币，，， 这是当前设置的稳定币！

            foreach (Model.ViewUserTokenBalanceSummary m in vl)
            {
                if (m.AllBalance > 0)
                {
                    //var Stablecoin = Share.BLL.Token.GetModel(m.TokenAddress);                              //!!! 稳定币要取当前计价的稳定币

                    m.StablecoinSymbol = Stablecoin.Symbol;

                    var Token = Share.BLL.Token.GetModel(m.TokenAddress);
                    if (Token.PricingTokenPrice < 0)
                    {
                        Token.PricingTokenPrice = 0;
                    }
                    m.StablecoinAmount = Token.PricingTokenPrice * m.AllBalance;
                }
            }

            return vl;

        }

        public static bool IsSysAllAddressBalanceFlag
        {
            get
            {
                lock (_SysAllAddressBalanceObj)
                {
                    return _IsSysAllAddressBalanceFlag;
                }
            }
        }

        public static async Task<bool> ExeFullAddressBalance()
        {
            bool result = await FullAddressBalance();
            while (result)
            {
                result = await FullAddressBalance();
            }
            return true;
        }


        /// <summary>
        /// AddressBalance 表的数据，要动态生成，当token增加了，或者地址增加了，都需要生成！
        /// 但是数据量很大，很可能会执行超时而出错, 所以增加一个  TOP (3000) ，可以多次执行！
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> FullAddressBalance()
        {
            string sql = @"
INSERT INTO AddressBalance
                (UserAddress, TokenAddress, Balance, UpdateBlockNumber, Balance_Text, UpdateDateTime)
SELECT   TOP (3000) UserAddress, TokenAddress, 0.0 AS Balance, 0 AS UpdateBlockNumber, '' AS Balance_Text, 
                CAST('2000-10-20' AS datetime) AS UpdateDateTime
FROM      (SELECT   UserAddress, TokenAddress
                 FROM      (SELECT   View_Address.Address AS UserAddress, Token.Address AS TokenAddress
                                  FROM      View_Address CROSS JOIN
                                                  Token) AS T
                 WHERE   (NOT EXISTS
                                     (SELECT   UserAddress
                                      FROM      AddressBalance AS AB
                                      WHERE   (UserAddress = T.UserAddress) AND (TokenAddress = T.TokenAddress)))) AS TOK
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cn.Open();
            try
            {
                var result = await cm.ExecuteNonQueryAsync();
                return result > 0;


            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return false;
            }
            finally
            {
                cn.Close();
            }
        }


        private static bool _IsSysAllAddressBalanceFlag = false;     //标记，只能同时执行一个， 因为好几个地方调用！
        private static object _SysAllAddressBalanceObj = new object();

        public static async Task<bool> SysAllAddressBalance(bool isBackground)
        {
            //更新 记账表 AddressBalance 
            await ExeFullAddressBalance();

            //使用 Multi Call ，更高效！
            return await SysAllAddressBalance_MultiCall(isBackground);

            #region 删除相关代码！

            //// 删除相关代码！
            //lock (_SysAllAddressBalanceObj)
            //{
            //    if (_IsSysAllAddressBalanceFlag)
            //    {
            //        Share.WpfToastNotifications.Show(LanguageHelper.GetTranslationText("提示"), LanguageHelper.GetTranslationText("后台正在同步用户的Token余额"), NotificationType.Information, 120);
            //        return false; ;
            //    }
            //    _IsSysAllAddressBalanceFlag = true;
            //}

            ////如果数量大的话，会很慢，例如三万个地址，需要一个小时！  每隔 20 秒处理一笔，否则 web3 网站会当作攻击而拒绝！
            //try
            //{
            //    var dt = GetAllAddressForBalance();
            //    foreach (var row in dt.Rows)
            //    {
            //        try
            //        {
            //            //if (isBackground)                               //后台进程
            //            //{
            //            //    for (int i = 0; i < 600; i++)
            //            //    {
            //            //        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 1));       //休息600秒，10分钟
            //            //        if (!App.IsRunning)
            //            //        {
            //            //            return false;
            //            //        }
            //            //    }
            //            //}

            //            DataRow m = row as DataRow;
            //            string UserAddress =(string) m["UserAddress"];
            //            string TokenAddress = (string)m["TokenAddress"];

            //            await UpdateBalance(UserAddress, TokenAddress);

            //            if (isBackground)                               //后台进程
            //            {
            //                for (int i = 0; i < 600; i++)
            //                {
            //                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 1));      //休息600秒，10分钟
            //                    if (!App.IsRunning)
            //                    {
            //                        return false;
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("", ex);
            //        }
            //    }
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("", ex);
            //    return false;
            //}

            #endregion

        }


        public class SysAddressBalanceProcess
        {
            public int AddressNumber { get; set; }

            public DateTime StartTime { get; set; }

            public int DoneNumber { get; set; }

            public DateTime DoingTime { get; set; }

        }

        private static SysAddressBalanceProcess _SysABProcess = new SysAddressBalanceProcess();


        /// <summary>
        /// 得到同步用户token balance的处理进度。
        /// </summary>
        /// <returns></returns>
        public static SysAddressBalanceProcess GetSysAddressBalanceProcess()
        {
            //复制一份，毕竟是跨线程读取。
            var result = new SysAddressBalanceProcess();
            result.AddressNumber = _SysABProcess.AddressNumber;
            result.DoneNumber = _SysABProcess.DoneNumber;
            result.StartTime = _SysABProcess.StartTime;
            result.DoingTime = _SysABProcess.DoingTime;
            return result;
        }


        public static async Task<bool> SysAllAddressBalance_MultiCall(bool isBackground)
        {
            lock (_SysAllAddressBalanceObj)
            {
                if (_IsSysAllAddressBalanceFlag)
                {
                    Share.WpfToastNotifications.Show(LanguageHelper.GetTranslationText("提示"), LanguageHelper.GetTranslationText("后台正在同步用户的Token余额"), NotificationType.Information, 120);    //保证一次只有一个同步执行！
                    return false; ;
                }
                _IsSysAllAddressBalanceFlag = true;
            }

            //如果数量大的话，会很慢，例如三万个地址，需要一个小时！  每秒处理频率（例如每隔 20 秒处理一笔）需要控制，否则 web3 网站会当作攻击而拒绝！ 这个也要看付费用户和免费用户
            try
            {
                var AddressDt = GetAllAddressForBalance_MultiCall();
                var TokenDt = GetAllTokenForBalance_MultiCall();

                _SysABProcess.AddressNumber = AddressDt.Rows.Count;
                _SysABProcess.StartTime = System.DateTime.Now;
                _SysABProcess.DoneNumber = 0;
                _SysABProcess.DoingTime = System.DateTime.Now;

                foreach (DataRow AddressRow in AddressDt.Rows)
                {
                    if (!App.IsRunning)
                    {
                        return false;
                    }

                    try
                    {
                        //DataRow m = row as DataRow;
                        string UserAddress = (string)AddressRow["UserAddress"];

                        List<string> TokenList = new List<string>();
                        foreach (DataRow t in TokenDt.Rows)
                        {
                            TokenList.Add((string)t["TokenAddress"]);
                        }

                        var result = await UpdateBalance_MultiCall(UserAddress, TokenList);
                        if (Properties.Settings.Default.LogUpdateBalance)
                        {
                            //这个日志太多了，设置一个参数控制一下
                            log.Info("UpdateBalance_MultiCall, Result Is " + result.ToString() + "; UserAddress Is " + UserAddress);
                        }

                        _SysABProcess.DoneNumber = _SysABProcess.DoneNumber + 1;       //处理一个用户地址的token
                        _SysABProcess.DoingTime = System.DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        log.Error("", ex);
                    }

                    if (isBackground)
                    {
                        if (!App.IsRunning)
                        {
                            return false;
                        }
                        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 1));       //休息 1 秒
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return false;
            }
            finally
            {
                _IsSysAllAddressBalanceFlag = false;
            }
        }


        private static bool IsDoSysAddressBalanceFlag = false;      //标记这个线程只能启动一次

        public static void DoSysAddressBalance()
        {
            if (IsDoSysAddressBalanceFlag) return;
            IsDoSysAddressBalanceFlag = true;
            System.Threading.Thread t = new System.Threading.Thread(ExeSysAddressBalance);
            t.Start();
        }


        private static void ExeSysAddressBalance()
        {
            log.Info("后台线程: ExeSysAddressBalance(同步地址余额) Begin");

            try
            {
                for (int i = 0; i < 30; i++)
                {
                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 1));      //休息30秒;
                    if (!App.IsRunning)
                    {
                        return;
                    }
                }

                while (App.IsRunning)
                {
                    if (SystemParam.IsUpdateBalanceByBackground)
                    {
                        //只有设置了需要后台更新用户的余额，才在后台更新。
                        try
                        {
                            log.Info("SysAllAddressBalance Begin");
                            var t0 = System.DateTime.Now;
                            var result = SysAllAddressBalance(true).Result;       //长时间处理  UI 也调用
                            var t1 = System.DateTime.Now;
                            log.Info("SysAllAddressBalance End, Result Is " + result.ToString() + ", Seconds Is " + (t1 - t0).TotalSeconds.ToString());
                            Share.WpfToastNotifications.Show(LanguageHelper.GetTranslationText("后台处理"), LanguageHelper.GetTranslationText("同步用户的Token余额完成，结束时间是") + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), NotificationType.Information, 120);
                        }
                        catch (Exception ex)
                        {
                            log.Error("", ex);
                        }
                    }
                    else
                    {
                        ;   //do nothing 
                    }

                    for (int i = 0; i < 600; i++)
                    {
                        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 1));      //休息10分钟;
                        if (!App.IsRunning)
                        {
                            return;
                        }
                        //if (!SystemParam.IsUpdateBalanceByBackground)
                        //{
                        //    continue;                                           //不等
                        //}
                    }
                }
            }
            finally
            {
                log.Info("后台线程: ExeSysAddressBalance(同步地址余额) End");
            }
        }

        //private static List<Model.AddressBalance> DataTable2List(System.Data.DataTable dt)
        //{
        //    List<Model.AddressBalance> result = new List<Model.AddressBalance>();
        //    foreach (System.Data.DataRow dr in dt.Rows)
        //    {
        //        Model.AddressBalance Obj = new Model.AddressBalance();
        //        Obj.UserAddress = dr["UserAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["UserAddress"]);
        //        Obj.TokenAddress = dr["TokenAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["TokenAddress"]);
        //        Obj.Balance = 0;
        //        Obj.UpdateBlockNumber = 0;
        //        result.Add(Obj);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 得到所有的地址，和 AddressBalance 对比，如果不存在就要 insert 。写到一个SQL语句里面太复杂了，
        /// </summary>
        /// <returns></returns>

        //        private static System.Data.DataTable GetAllAddressForBalance()
        //        {
        //            string sql = @"
        //SELECT   T1.UserAddress, T2.TokenAddress
        //FROM      (SELECT   Address AS UserAddress
        //                 FROM      KeyStore_Address
        //                 UNION
        //                 SELECT   Address
        //                 FROM      HD_Address) AS T1 CROSS JOIN
        //                    (SELECT   Address AS TokenAddress
        //                     FROM      Token) AS T2
        //";

        //            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
        //            SqlCommand cm = new SqlCommand();
        //            cm.Connection = cn;
        //            cm.CommandType = System.Data.CommandType.Text;
        //            cm.CommandText = sql;

        //            SqlDataAdapter da = new SqlDataAdapter();
        //            da.SelectCommand = cm;
        //            System.Data.DataSet ds = new System.Data.DataSet();
        //            da.Fill(ds);

        //            return ds.Tables[0];
        //        }

        private static System.Data.DataTable GetAllAddressForBalance_MultiCall()
        {
            //            string sql = @"
            //SELECT   Address AS UserAddress
            //FROM      KeyStore_Address
            //UNION
            //SELECT   Address
            //FROM      HD_Address
            //";

            //同步地址余额，先后秩序不能一成不变。  如果地址数量少无所谓，但地址多了就很费时间。
            string sql = @"
SELECT   Address AS UserAddress
FROM      View_Address
";
            if (!Properties.Settings.Default.AddressSortIsASC)
            {
                sql = @"
SELECT   Address AS UserAddress
FROM      View_Address
ORDER BY Address DESC
";
            }

            log.Info("GetAllAddressForBalance, SQL : " + sql.Replace(Environment.NewLine, " "));

            Properties.Settings.Default.AddressSortIsASC = !Properties.Settings.Default.AddressSortIsASC;
            Properties.Settings.Default.Save();

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }

        private static System.Data.DataTable GetAllTokenForBalance_MultiCall()
        {
            string sql = @"
SELECT   Address AS TokenAddress
FROM      Token
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

            return ds.Tables[0];
        }


        public static async Task<decimal> UpdateBalance(string UserAddress, string TokenAddress)
        {
            try
            {
                var web3 = Share.ShareParam.GetWeb3();

                System.Numerics.BigInteger Balance = 0;
                byte Decimals = 18;

                if (TokenAddress == "0x0" || Share.ShareParam.IsAnEmptyAddress(TokenAddress))
                {
                    Balance = await web3.Eth.GetBalance.SendRequestAsync(UserAddress);
                    Decimals = 18;
                }
                else
                {
                    Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, TokenAddress);
                    Decimals = await service.DecimalsQueryAsync();
                    Balance = await service.BalanceOfQueryAsync(UserAddress);
                }

                var BlockNumber = await BlockChain.Common.Web3Helper.GetLastBlockNuber(Share.ShareParam.Web3Url);

                Model.AddressBalance model = new Model.AddressBalance();

                model.Balance = (decimal)((double)Balance / Math.Pow(10.0, (double)Decimals));
                model.Balance_Text = Balance.ToString();

                model.TokenAddress = TokenAddress;
                model.UserAddress = UserAddress;

                model.UpdateDateTime = System.DateTime.Now;
                model.UpdateBlockNumber = (long)BlockNumber;

                model.ValidateEmptyAndLen();

                if (DAL.AddressBalance.Exist(Share.ShareParam.DbConStr, model.UserAddress, model.TokenAddress))
                {
                    DAL.AddressBalance.Update(Share.ShareParam.DbConStr, model);
                }
                else
                {
                    DAL.AddressBalance.Insert(Share.ShareParam.DbConStr, model);
                }

                return model.Balance;
            }
            catch (Exception ex)
            {
                log.Error("UpdateBalance", ex);
                return -1;
            }
        }

        /// <summary>
        /// 处理一个用户，所有的token数量，包括ETH
        /// </summary>
        /// <param name="UserAddress"></param>
        /// <param name="TokenList"></param>
        /// <returns></returns>
        public static async Task<bool> UpdateBalance_MultiCall(string UserAddress, List<string> TokenList)
        {
            try
            {
                var BlockNumber = await BlockChain.Common.Web3Helper.GetLastBlockNuber(Share.ShareParam.Web3Url);
                var bl = await Share.BLL.Address.QueryUserTokenBalance_MultiCall(UserAddress, TokenList);

                foreach (var m in bl)
                {
                    var Balance = m.BigBalance;
                    var Decimals = Share.BLL.Token.GetTokenDecimals(m.TokenAddress);

                    Model.AddressBalance model = new Model.AddressBalance();

                    model.Balance = (decimal)m.Balance;         // (decimal)((double)Balance / Math.Pow(10.0, (double)Decimals));
                    model.Balance_Text = Balance.ToString();
                    model.TokenAddress = m.TokenAddress;
                    model.UserAddress = UserAddress;
                    model.UpdateDateTime = System.DateTime.Now;
                    model.UpdateBlockNumber = (long)BlockNumber;

                    model.ValidateEmptyAndLen();

                    if (DAL.AddressBalance.Exist(Share.ShareParam.DbConStr, model.UserAddress, model.TokenAddress))
                    {
                        DAL.AddressBalance.Update(Share.ShareParam.DbConStr, model);
                    }
                    else
                    {
                        DAL.AddressBalance.Insert(Share.ShareParam.DbConStr, model);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("UpdateBalance", ex);
                return false;
            }
        }


    }
}
