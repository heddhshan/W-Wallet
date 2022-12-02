using System;
using System.Security.Cryptography;
using System.Text;

namespace BlockChain.Common.Security
{
    /// <summary>
    /// 非对称加密 类
    /// </summary>
    public static class UnsymmetricalEncrypt
    {
        ////私钥签名 
        //RSACryptoServiceProvider oRSA3 = new RSACryptoServiceProvider();
        //oRSA3.FromXmlString(privatekey); 
        //    byte[] AOutput = oRSA3.SignData(messagebytes, "SHA256");
        ////公钥验证 
        //RSACryptoServiceProvider oRSA4 = new RSACryptoServiceProvider();
        //oRSA4.FromXmlString(publickey); 
        //    bool bVerify = oRSA4.VerifyData(messagebytes, "SHA256", AOutput);

        /// <summary>
        /// 创建密钥对，得到xml形式的私钥和公钥
        /// </summary>
        /// <param name="PrivateKey">xml形式的私钥</param>
        /// <param name="PublicKey">xml形式的公钥</param>
        public static void GenerateRandomKey(out string PrivateKey, out string PublicKey)
        {
            RSACryptoServiceProvider Crypt = new RSACryptoServiceProvider();
            PrivateKey = Crypt.ToXmlString(true);
            PublicKey = Crypt.ToXmlString(false);
        }

        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="PublicKey">xml形式的公钥</param>
        /// <param name="OriginalDataString">需要被加密的原文</param>
        /// <returns>加密后的文本，采用Base64编码</returns>
        public static string Encrypt(string PublicKey, string OriginalDataString)
        {
            UTF8Encoding Enc = new UTF8Encoding();      //这种形式的编码不一定合适
            byte[] OriginalDataByte = Enc.GetBytes(OriginalDataString);
            byte[] EncryByte = Encrypt(PublicKey, OriginalDataByte);

            string EncryString = Convert.ToBase64String(EncryByte);
            return EncryString;
        }

        /// <summary>
        /// 加密字节数组
        /// </summary>
        /// <param name="PublicKey">xml形式的公钥</param>
        /// <param name="OriginalDataByte">需要被加密的原文</param>
        /// <returns>加密后的字节</returns>
        public static byte[] Encrypt(string PublicKey, byte[] OriginalDataByte)
        {
            RSACryptoServiceProvider Crypt = new RSACryptoServiceProvider();
            Crypt.FromXmlString(PublicKey);
            byte[] EncryByte = Crypt.Encrypt(OriginalDataByte, false);
            return EncryByte;
        }

        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="PrivateKey">xml形式的私钥</param>
        /// <param name="EncryptedDataString">已经加密的文本</param>
        /// <returns>被解密的文本</returns>
        public static string Decrypt(string PrivateKey, string EncryptedDataString)
        {
            byte[] EncryptedDataByte = Convert.FromBase64String(EncryptedDataString);
            byte[] DecryptByte = Decrypt(PrivateKey, EncryptedDataByte);

            UTF8Encoding Enc = new UTF8Encoding();
            string DecryptString = Enc.GetString(DecryptByte);
            return DecryptString;
        }

        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="PrivateKey">xml形式的私钥</param>
        /// <param name="EncryptedDataByte">已经加密的字节，需要解密的字节</param>
        /// <returns>解密后字节，原始数据</returns>
        public static byte[] Decrypt(string PrivateKey, byte[] EncryptedDataByte)
        {
            RSACryptoServiceProvider Crypt = new RSACryptoServiceProvider();
            Crypt.FromXmlString(PrivateKey);
            byte[] DecryptByte = Crypt.Decrypt(EncryptedDataByte, false);
            return DecryptByte;
        }

    }

}
