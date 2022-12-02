using Nethereum.KeyStore;
using Nethereum.Signer;
using System;


namespace BlockChain.Share.BLL
{
    public  class KeyStore
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static Nethereum.Web3.Accounts.Account GetKeyStoreAccount(string keyStoreFile, string password)
        {
            var file = System.IO.File.OpenText(keyStoreFile);
            var json = file.ReadToEnd();
            file.Close();

            var account = Nethereum.Web3.Accounts.Account.LoadFromKeyStore(json, password);
            return account;
        }


        /// <summary>
        /// 把公钥（地址）和私钥加密保存到KeyStore文件中去
        /// </summary>
        /// <param name="password"></param>
        /// <param name="address"></param>
        /// <param name="privateKey"></param>
        /// <param name="ksText"></param>
        /// <param name="fileName"></param>
        public static void SaveKeyStoreFile(string password, string address, byte[] privateKey, out string ksText, out string fileName)
        {
            var pbkdf2Service = new KeyStorePbkdf2Service();
            ksText = pbkdf2Service.EncryptAndGenerateKeyStoreAsJson(password, privateKey, address);

            var service = new KeyStoreService();
            fileName = service.GenerateUTCFileName(address);

            fileName = System.IO.Path.Combine(Share.ShareParam.KeystoreDir, fileName);    //重写FileName路径，默认就是程序路径

            log.Info("SaveKeyStoreFile: " + fileName);  //test

            try
            {
                using (var newfile = System.IO.File.CreateText(fileName))           //这也可能失败！windows 系统写文件权限！
                {
                    newfile.Write(ksText);
                    newfile.Flush();
                }
            }
            catch (Exception ex)
            {
                log.Error("SaveKeyStoreFile", ex);
                throw ex;
            }

            //System.Console.WriteLine(System.IO.Path.GetFullPath(fileName)); //test
        }

        /// <summary>
        /// 生成公钥（地址）和私钥
        /// </summary>
        /// <param name="address"></param>
        /// <param name="privateKey"></param>
        public static void GenKey(out string address, out byte[] privateKey)
        {
            var ecKey = EthECKey.GenerateKey();
            privateKey = ecKey.GetPrivateKeyAsBytes();
            if (privateKey.Length < 32)     //是有可能小于32位的
            {
                GenKey(out address, out privateKey);
            }
            address = ecKey.GetPublicAddress();

            if (Common.Security.Tools.IsWeekPrivateKey(privateKey))
            {
                GenKey(out address, out privateKey);
            }
        }





        /// <summary>
        /// 新建keystore，并 写入文件和数据库
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="password"></param>
        public static void SaveKeyStore(string alias, string password)
        {
            string address;
            byte[] privateKey;
            GenKey(out address, out privateKey);

            string ksText;
            string fileName;
            SaveKeyStoreFile(password, address, privateKey, out ksText, out fileName);

            SaveKeyStoreToDb(alias, address, ksText, fileName);
        }


        /// <summary>
        /// 写入数据库
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="address"></param>
        /// <param name="ksText"></param>
        /// <param name="fileName"></param>
        public static void SaveKeyStoreToDb(string alias, string address, string ksText, string fileName)
        {
            Model.KeyStore_Address m = new Model.KeyStore_Address();
            m.Address = address;
            m.AddressAlias = alias;
            m.FilePath = fileName;
            m.KeyStoreText = ksText;
            m.IsTxAddress = true;                           //key store 默认需要处理交易！
            m.HasPrivatekey = true;

            m.ValidateEmptyAndLen();
            DAL.KeyStore_Address.Insert(Share.ShareParam.DbConStr, m);
        }




    }
}
