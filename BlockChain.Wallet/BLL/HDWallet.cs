using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using BlockChain.Share;
using BlockChain.Wallet.Model;

namespace BlockChain.Wallet.BLL
{

    /// <summary>
    /// 助记词钱包相关逻辑
    /// </summary>
    public class HDWallet
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static void DeleteAllAddress(Guid MneId)
        {
            string sql = @"
DELETE FROM AddressBalance
FROM      AddressBalance INNER JOIN
                   HD_Address ON AddressBalance.UserAddress = HD_Address.Address
WHERE   (HD_Address.MneId = @MneId);

DELETE FROM HD_Address
WHERE   (MneId = @MneId);
";

            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier).Value = MneId;

            cn.Open();
            try { 
            cm.ExecuteNonQuery();
            }
            finally { cn.Close(); }
        }

        public static List<Model.ViewHdAddress> GetExHdAddress(Guid MneId)
        {
            string sql = @"
SELECT  1 AS IsSelected, AddressIndex, AddressAlias, Address, MneSecondSalt
FROM      HD_Address
WHERE   (MneId = @MneId)
";

            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier).Value = MneId;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
          
                return Model.ViewHdAddress.DataTable2List( ds.Tables[0]);
        }


        public static Model.HD_Mnemonic GetModel(string conStr, string mneHash)
        {
            string sql = @"select * from HD_Mnemonic Where mneHash = @mneHash ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@mneHash", SqlDbType.NVarChar).Value = mneHash;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.HD_Mnemonic.DataRow2Object(ds.Tables[0].Rows[0]);
            }
            else if (ds.Tables[0].Rows.Count > 1)
            {
                throw new Exception("数据错误，对应多条记录！");
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// 得到助记的第 0 个地址，
        /// </summary>
        /// <param name="MneId"></param>
        /// <returns></returns>
        public static Model.HD_Address GetHdFirstAddressModel(Guid MneId)
        {
            string sql = @"
SELECT  TOP (1) *
FROM      HD_Address
WHERE   (MneId = @MneId) AND (MneSecondSalt = '') AND (AddressIndex = 0)
";

            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier).Value = MneId;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.HD_Address.DataRow2Object(ds.Tables[0].Rows[0]);
            }
            else if (ds.Tables[0].Rows.Count > 1)
            {
                throw new Exception("数据错误，对应多条记录！");
            }
            else
            {
                return null;
            }
            // DAL.KeyStore_Address.GetModel(Share. ShareParam.DbConStr, mneHash)

        }


        /// <summary>
        /// 得到别名，hash 两个字段，主要用于 combox 下拉列表
        /// </summary>
        /// <returns></returns>
        public static System.Data.DataTable GetHDWalletsAliasHash()
        {
            string sql = @"
SELECT   MneHash, MneAlias
FROM      HD_Mnemonic
WHERE   (IsBackUp = 1)
ORDER BY CreateTime
";

            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
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

        public static System.Data.DataTable GetHDWallets(bool all=true)
        {
            string sql = @"
SELECT   MneHash, MneId, MneAlias,HasPrivatekey,
                    (SELECT   COUNT(*) AS AN
                     FROM      HD_Address
                     WHERE   (MneId = HD_Mnemonic.MneId)) AS AddrNum
FROM      HD_Mnemonic
WHERE   (IsBackUp = 1)
ORDER BY CreateTime
";
            if (!all) {
                sql = @"
SELECT   MneHash, MneId, MneAlias, HasPrivatekey,
                    (SELECT   COUNT(*) AS AN
                     FROM      HD_Address
                     WHERE   (MneId = HD_Mnemonic.MneId)) AS AddrNum
FROM      HD_Mnemonic
WHERE   (IsBackUp = 1) AND (HasPrivatekey = 1)
ORDER BY CreateTime
";
            }

            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);                                                            //这个地方，如果数据量太大，会超时， 暂时不处理，

            return ds.Tables[0];
        }


        /// <summary>
        /// 助记词长度，18个单词
        /// </summary>
        public const int MneLen = 18;
                                     //m/44'/60'/0'/0/x
        public const string MnePath = "m/44'/60'/0'/0/x";         //m/44'/60'/0'/x

        public static string GetPasswordHash(string password, string salt)
        {
            var UsePassword =   Common.PasswordHelper.GetHDRealPassword(salt, password );
            var UserPasswordHash = Common.Security.Tools.GetHashValue(UsePassword);
            return UserPasswordHash;
        }

        public static string GetMneHash(string mne, string salt)
        {
            return Common.Security.Tools.GetHashValue(mne + salt);
        }


        public static void DeleteHdWallet(Guid mneId)
        {
            DAL.HD_Mnemonic.Delete(Share. ShareParam.DbConStr, mneId);
            BLL.HDWallet.DeleteAllAddress(mneId);


            //DAL.HD_Mnemonic.Delete(Share. ShareParam.DbConStr, mneId);

            ////DAL.KeyStore_Address.Delete()
            //string sql = @"Delete From HD_Address Where mneId = @mneId";
            //SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
            //SqlCommand cm = new SqlCommand();
            //cm.Connection = cn;
            //cm.CommandType = System.Data.CommandType.Text;
            //cm.CommandText = sql;

            //cm.Parameters.Add("@mneId", SqlDbType.Int).Value = mneId;
            //int RecordAffected = -1;
            //cn.Open();
            //try
            //{
            //    RecordAffected = cm.ExecuteNonQuery();
            //}
            //finally
            //{
            //    cn.Close();
            //}
           
        }


        public static Model.HD_Mnemonic NewHDWallet(string alias, string password, string tip, string mne = "", bool isBackUp = false )
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    if (string.IsNullOrEmpty(mne))
                    {
                        mne = GenMnemonic(MneLen);
                    }

                    Model.HD_Mnemonic model = new Model.HD_Mnemonic();
                    model.MneAlias = alias;

                    model.Salt = Common.Security.Tools.GenRandomStr(32);
                    var UsePassword = Common.PasswordHelper.GetHDRealPassword(model.Salt, password );
                    model.UserPasswordHash = Common.Security.Tools.GetHashValue(UsePassword);
                    model.MneEncrypted = Common.Security.SymmetricalEncrypt.Encrypt(UsePassword, mne);
                    model.MneHash = Common.Security.Tools.GetHashValue(mne + model.Salt);

                    model.MnePath = MnePath;
                    model.UserPasswordTip = tip;
                    model.EncryptedTimes = 1;
                    model.MneSource = HDSource.FromThis;
                    model.WordCount = MneLen;
                    model.MneFirstSalt = string.Empty;       //主要是为了和第三方助记词兼容，才设计这个密码字段，官方的助记词没有这个密码。
                    model.IsBackUp = isBackUp;// false;
                    model.CreateTime = System.DateTime.Now;
                    model.MneId = Guid.NewGuid();
                    model.HasPrivatekey = true;

                    model.ValidateEmptyAndLen();
                    DAL.HD_Mnemonic.Insert(Share. ShareParam.DbConStr, model);

                    SaveNewHdFirstAddress(model, mne, Share. LanguageHelper.GetTranslationText( "第0个地址（主地址）"), 0);    //保存 第 0 个地址， 是为了和其他钱包兼容。

                    ts.Complete();

                    return model;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static Model.HD_Mnemonic NewHDWalletBy3Party(string mne, string mneSalt, string path, string alias, string walletPassword, string tip)
        {
            try
            {
                Model.HD_Mnemonic model = new Model.HD_Mnemonic();
                model.MneAlias = alias;

                model.Salt = Common.Security.Tools.GenRandomStr(32);
                //var UsePassword = Common. walletPassword + model.Salt;
                var UsePassword = Common.PasswordHelper.GetHDRealPassword(model.Salt, walletPassword);
                model.UserPasswordHash = Common.Security.Tools.GetHashValue(UsePassword);
                model.MneEncrypted = Common.Security.SymmetricalEncrypt.Encrypt(UsePassword, mne);
                model.MneHash = Common.Security.Tools.GetHashValue(mne + model.Salt);

                model.MnePath = path;
                model.UserPasswordTip = tip;
                model.EncryptedTimes = 1;
                model.MneSource = HDSource.FromOther;
                model.WordCount = MneLen;
                model.MneFirstSalt = mneSalt;   // Common.Security.Tools.GetHashValue(mnePassword + model.Salt); ;  //mnePassword;       //主要是为了和第三方助记词兼容，才设计这个密码字段，官方的助记词没有这个密码。
                model.IsBackUp = true;
                model.CreateTime = System.DateTime.Now;

                model.ValidateEmptyAndLen();

                model.MneId = Guid.NewGuid();
                DAL.HD_Mnemonic.Insert(Share. ShareParam.DbConStr, model);

                //SaveNewHdAddressFor3Party( mne, "第0个地址（主地址）", 0);    //使用第 0 个，是为了和其他钱包兼容。
                SaveNewHdAddressFor3Party(model.MneId,  mne, mneSalt,  path, model.MneHash, "0th address(Main Address)", 0);    //默认使用第 0 个。

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int UpdateIsBackUp(string mneHash)
        {
            string sql = @"Update HD_Mnemonic Set IsBackUp = 1  Where MneHash = @MneHash ";
            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@MneHash", SqlDbType.NVarChar, 64).Value = mneHash;

            int RecordAffected = -1;
            cn.Open();
            try
            {
                RecordAffected = cm.ExecuteNonQuery();
            }
            finally
            {
                cn.Close();
            }
            return RecordAffected;

        }


        /// <summary>
        /// 保存第0个地址，不允许使用！只能用于验证！！！
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mne"></param>
        /// <param name="alias"></param>
        /// <param name="accountIndex"></param>
        /// <exception cref="Exception"></exception>
        public static void SaveNewHdFirstAddress(Model.HD_Mnemonic model, string mne, string alias, int accountIndex)
        {
            Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(mne, model.MneFirstSalt, model.MnePath);    //密码（salt）为空
            var Account = w.GetAccount(accountIndex);

            //验证助记词生成的地址是否和nethereum生成的助记词一样！
            var a = new Nethereum.Web3.Accounts.Account(Account.PrivateKey.HexToByteArray());
            if (!a.Address.IsTheSameAddress(Account.Address))
            {
                string err = @"Address Is Error. Mnemonic Account.Address: " + Account.Address + "; Validate Account Address: " + a.Address;
                throw new Exception(err);
            }

            Model.HD_Address m = new Model.HD_Address();
            m.Address = Account.Address;
            m.AddressAlias = alias;                     // "默认地址";
            m.AddressIndex = accountIndex;
            m.MneId = model.MneId;
            m.MneSecondSalt = string.Empty;
            m.HasPrivatekey = false;                //第0个地址不允许使用！只能用于验证！！！

            m.ValidateEmptyAndLen();
            DAL.HD_Address.Insert(Share. ShareParam.DbConStr, m);
        }

        public static void SaveNewHdAddressFor3Party(Guid mneId, string mne, string mneSalt, string path, string mneHash, string alias, int accountIndex)
        {
            Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(mne, mneSalt, path);    //密码（salt）为空
            var Account = w.GetAccount(accountIndex);

            //验证助记词生成的地址是否和nethereum生成的助记词一样！
            var a = new Nethereum.Web3.Accounts.Account(Account.PrivateKey.HexToByteArray());
            if (!a.Address.IsTheSameAddress(Account.Address))
            {
                string err = @"Address Is Error. Mnemonic Account.Address: " + Account.Address + "; Validate Account Address: " + a.Address;
                throw new Exception(err);
            }

            Model.HD_Address m = new Model.HD_Address();
            m.Address = Account.Address;
            m.AddressAlias = alias;                     // "默认地址";
            m.AddressIndex = accountIndex;
            m.MneId = mneId;
            m.MneSecondSalt = string.Empty;
            m.HasPrivatekey = true;                     //第三方导入的助记词，其所有地址都可以使用！

            m.ValidateEmptyAndLen();
            DAL.HD_Address.Insert(Share. ShareParam.DbConStr, m);
        }


        public static string GenMnemonic(int len)
        {
            string Mnemonic = string.Empty;

            //var task = NBitcoin.Wordlist.LoadWordList(NBitcoin.Language.English);
            //task.Wait();
            //var AllWords = task.Result;
            var AllWords = NBitcoin.Wordlist.LoadWordList(NBitcoin.Language.English).Result;

            var WordCount = AllWords.WordCount;
            List<int> p = new List<int>();
            for (int i = 0; i < len; i++)
            {
                var r = Common.Security.Tools.GenRandomInt(WordCount);
                p.Add(r);
            }

            var words = AllWords.GetWords(p.ToArray());
            foreach (var w in words)
            {
                Mnemonic = Mnemonic + w + " ";
            }
            Mnemonic = Mnemonic.Trim();
            return Mnemonic;
        }

    }
}
