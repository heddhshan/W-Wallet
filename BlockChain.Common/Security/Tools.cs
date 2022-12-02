using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace BlockChain.Common.Security
{
    public static class Tools
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public static readonly log4net.ILog log = log4net.LogManager.GetLogger("loginfo");

        public static string CreateXML<T>(T obj)
        {
            string result = string.Empty;
            System.IO.StringWriter w = new System.IO.StringWriter();
            XmlTextWriter writer = new XmlTextWriter(w);
            try
            {
                XmlSerializer xser = new XmlSerializer(typeof(T));  //实例化序列化对象
                xser.Serialize(writer, obj);                        //序列化对象到xml文档
                result = w.ToString();
            }
            catch (Exception ex)
            {
                log.Error("CreateXML", ex);
            }
            finally
            {
                writer.Close();
            }
            return result;
        }

        public static string GetHashValue(string ObjectText)
        {
            byte[] result = new byte[ObjectText.Length];
            SHA256 sha = SHA256.Create();                       //  new SHA1CryptoServiceProvider();
            result = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(ObjectText));
            string hashString = Convert.ToBase64String(result);
            return hashString;
        }


        ///// <summary>
        ///// 解密，用于节点私钥或本地私钥
        ///// </summary>
        ///// <param name="salt"></param>
        ///// <param name="encrypted"></param>
        ///// <returns></returns>
        //public static string GetDecrypt(string salt, string encrypted)
        //{
        //    string password = GetPassword();
        //    string EnPassword = GetHashValue(password + salt);
        //    string result = Common.Security.SymmetricalEncrypt.Decrypt(EnPassword, encrypted);
        //    return result;
        //}

        ///// <summary>
        ///// 加密，用于节点私钥和本地私钥
        ///// </summary>
        ///// <param name="salt"></param>
        ///// <param name="original"></param>
        ///// <returns></returns>
        //public static string GetEncrypt(string salt, string original)
        //{
        //    string password = GetPassword();
        //    string EnPassword = GetHashValue(password + salt);
        //    string result = Common.Security.SymmetricalEncrypt.Encrypt(EnPassword, original);
        //    return result;
        //}


        //private static SecureString ThisPassword;

        //private static SecureString GetSecurePassword(string passwrd)
        //{
        //    SecureString securePwd = new SecureString();
        //    foreach (char c in passwrd.ToCharArray())
        //    {
        //        securePwd.AppendChar(c);
        //    }
        //    return securePwd;
        //}


        //private static String SecureStringToString1(SecureString value)
        //{
        //    if (null == value)
        //    {
        //        return string.Empty;
        //    }

        //    IntPtr valuePtr = IntPtr.Zero;
        //    try
        //    {
        //        valuePtr = Marshal.SecureStringToBSTR(value);
        //        return Marshal.PtrToStringBSTR(valuePtr);
        //    }
        //    finally
        //    {
        //        Marshal.ZeroFreeBSTR(valuePtr);
        //    }
        //}


        //public static void SavePassword(string password)
        //{
        //    ThisPassword = GetSecurePassword(password);
        //}


        //public static string GetPassword()
        //{
        //    return SecureStringToString1(ThisPassword);
        //}


        public static byte[] GenRandomBytes(int len)
        {
            return RandomNumberGenerator.GetBytes(len);
            //byte[] bytes = new byte[len];
            //new RNGCryptoServiceProvider().GetBytes(bytes);
            //return bytes;
        }

        public static string GenRandomStr(int len)
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(len);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(string.Format("{0:X2}", bytes[i]));
            }
            return sb.ToString();
        }


        /// ///////////////////////////////////////////////////////     删除 node （wisper） 的相关代码



        ///// <summary>
        ///// 创建Node密钥对，得到xml形式的私钥和公钥
        ///// </summary>
        ///// <param name="PrivateKey">xml形式的私钥</param>
        ///// <param name="PublicKey">xml形式的公钥</param>
        //public static void GenerateRandomNodeKey(out string nodePrivateKey, out string nodePublicKey)
        //{
        //    bool prefix = true;
        //    var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
        //    nodePrivateKey = ecKey.GetPrivateKeyAsBytes().ToHex(prefix);
        //    nodePublicKey = ecKey.GetPubKey().ToHex(prefix);           
        //}

        ///// <summary>
        ///// 创建node对称加密的密码
        ///// </summary>
        ///// <returns></returns>
        //public static string GenerateNodeSymKey()
        //{
        //    var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
        //    return ecKey.GetPrivateKeyAsBytes().ToHex(true);
        //}


        //public static string GenerateNodeSymTopic()
        //{
        //    byte[] bytes = new byte[4];
        //    new RNGCryptoServiceProvider().GetBytes(bytes);
        //    return bytes.ToHex(true);
        //}


        //private static Random rd = new Random();

        //GenRandomStr
        //GetRandomInt
        public static int GenRandomInt()
        {
            return RandomNumberGenerator.GetInt32(int.MaxValue);

            //byte[] randomBytes = new byte[4]; 
            //RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider(); 
            //rngServiceProvider.GetBytes(randomBytes); 
            //Int32 result = BitConverter.ToInt32(randomBytes, 0);
            //return Math.Abs( result);
        }


        public static int GenRandomInt(int max)
        {
            return RandomNumberGenerator.GetInt32(max);

            ////int num = int.MaxValue / max;
            //byte[] randomBytes = new byte[4];
            //RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            //rngServiceProvider.GetBytes(randomBytes);
            //Int32 result = BitConverter.ToInt32(randomBytes, 0);
            ////result = result / num;
            //result = result % max;
            //result = Math.Abs(result);
            //return result;
        }

        public static int GenRandomInt(int min, int max)
        {
            return RandomNumberGenerator.GetInt32(min, max);
            //byte[] randomBytes = new byte[4];
            //RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            //rngServiceProvider.GetBytes(randomBytes);
            //Int32 result = BitConverter.ToInt32(randomBytes, 0);
            //result = result % max;
            //result = Math.Abs(result);
            //if (result < min)
            //{
            //    return GenRandomInt(min, max);  //递归
            //}
            //else
            //{
            //    return result;
            //}
        }

        public static bool IsWeekPrivateKey(string priviteKey)
        {
            var pk = priviteKey.HexToByteArray();
            return IsWeekPrivateKey(pk);
        }

        public static bool IsWeekPrivateKey(byte[] priviteKey)
        {
            if (priviteKey[0] == 0)
            {
                return true;
            }

            if (priviteKey[priviteKey.Length - 1] == 0)
            {
                return true;
            }

            //1，不允许0值；
            //2，不允许有相同值；或者不允许连续相等！

            for (int i = 0; i < priviteKey.Length - 2; i++)
            {
                if (priviteKey[i] == 0)
                {
                    return true;
                }

                if (priviteKey[i] == priviteKey[i + 1])
                {
                    return true;
                }
            }

            return false;
        }



    }

}
