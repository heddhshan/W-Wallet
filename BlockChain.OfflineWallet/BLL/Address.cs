using BlockChain.Share;
using BlockChain.OfflineWallet.Model;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Threading.Tasks;


namespace BlockChain.OfflineWallet.BLL
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
            throw new Exception("Offline Wallet Not Imp this Function!");           
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


     

        //通过地址，密码，得到 account 无论 keystore 还是 hd , 这个方法会常常用到。
        public Nethereum.Web3.Accounts.Account GetAccount(string address, string password)
        {
            try
            {
                //var chainid = Share.ShareParam.GetChainId();    // 注意： 离线钱包没有 chainid
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

                    //生成地址的代码，要一致！
                    //string salt2 = Common.Security.Tools.GenRandomStr(32);
                    //var salt = model.MneFirstSalt + salt2;
                    //Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(mne, salt, model.MnePath);// BLL.HDWallet.MnePath);

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

                    //for (int i = 90; i < 100; i++)
                    //    {
                    //    var aaa = new Nethereum.Web3.Accounts.Account(account.PrivateKey.HexToByteArray(), new BigInteger(i));
                    //    log.Info(aaa.Address);
                    //}

                    return account;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception", ex);
            }

            return null;
        }



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
        private static List< Share.TxAddress> _GetAllTxAddress()
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

            return Share.TxAddress.DataTable2List( ds.Tables[0]);
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
SELECT   ROW_NUMBER() OVER(Order by Address) AS RowId, AddressAlias, Address, SourceName, IsTxAddress
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
SELECT   ROW_NUMBER() OVER(Order by Address) AS RowId, AddressAlias, Address, SourceName, IsTxAddress
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
update KeyStore_Address set IsTxAddress = @IsTxAddress where Address = @Address;
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




    }
}
