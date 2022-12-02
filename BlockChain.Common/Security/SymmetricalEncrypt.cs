using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BlockChain.Common.Security
{


    /// <summary>
    /// 对称加密类  20220207更新 : https://docs.microsoft.com/zh-cn/dotnet/api/system.security.cryptography.aes?view=net-6.0
    /// </summary>
    public static class SymmetricalEncrypt
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 创建随机的Key和IV
        /// </summary>
        /// <returns>Key和IV</returns>
        public static KeyIV GenerateRandomKeyIV()
        {
            // https://docs.microsoft.com/zh-cn/dotnet/api/system.security.cryptography.aes.create?view=net-6.0
            // 可能 algorithmName 的值包括： "AES"、"AesCryptoServiceProvider"、"AesCryptoServiceProvider"、"AesManaged" 和 ""，以及 "AesManaged"。
            Aes Crypt = Aes.Create();
            KeyIV result = new KeyIV();
            result.Key = Crypt.Key;
            result.IV = Crypt.IV;
            return result;


            //Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(pwd1, salt1,myIterations);
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="Utf8OriginalString"></param>
        /// <returns>Base64String</returns>
        public static string Encrypt(string Password, string Utf8OriginalString)
        {
            UTF8Encoding Enc = new UTF8Encoding();                      //这种形式的编码不一定合适
            byte[] OriginalDataByte = Enc.GetBytes(Utf8OriginalString);
            KeyIV keyAndIV = KeyIV.GenerateKeyIV(Password);
            byte[] EncryByte = Encrypt(keyAndIV, OriginalDataByte);
            string EncryString = Convert.ToBase64String(EncryByte);
            return EncryString;
        }



        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="keyAndIV">Key 和 IV</param>
        /// <param name="OriginalDataString">需要被加密的原文</param>
        /// <returns>加密后的文本，采用Base64编码</returns>
        public static string Encrypt(KeyIV keyAndIV, string Utf8OriginalString)
        {
            UTF8Encoding Enc = new UTF8Encoding();                  //这种形式的编码不一定合适
            byte[] OriginalDataByte = Enc.GetBytes(Utf8OriginalString);

            byte[] EncryByte = Encrypt(keyAndIV, OriginalDataByte);

            string EncryString = Convert.ToBase64String(EncryByte);
            return EncryString;
        }

        /// <summary>
        /// 加密字节数组
        /// </summary>
        /// <param name="keyAndIV">Key 和 IV</param>
        /// <param name="OriginalDataByte">需要被加密的原文</param>
        /// <returns>加密后的字节</returns>
        public static byte[] Encrypt(KeyIV keyAndIV, byte[] OriginalDataByte)
        {
            // Check arguments.
            if (OriginalDataByte == null || OriginalDataByte.Length <= 0)
                throw new ArgumentNullException("OriginalDataByte");
            if (keyAndIV.Key == null || keyAndIV.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (keyAndIV.IV == null || keyAndIV.IV.Length <= 0)
                throw new ArgumentNullException("IV");
            //byte[] encrypted;

            //// Create an Aes object
            //// with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyAndIV.Key;
                aesAlg.IV = keyAndIV.IV;

                //    // Create an encryptor to perform the stream transform.
                //    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                //    // Create the streams used for encryption.
                //    using (MemoryStream msEncrypt = new MemoryStream())
                //    {
                //        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                //        {
                //            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                //            {
                //                //Write all data to the stream.
                //                swEncrypt.Write(OriginalDataByte);
                //            }
                //            encrypted = msEncrypt.ToArray();
                //        }
                //    }
                //}

                //// Return the encrypted bytes from the memory stream.
                //return encrypted;

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, aesAlg.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(OriginalDataByte, 0, OriginalDataByte.Length);
                cs.FlushFinalBlock();

                return ms.ToArray();
            }
        }

        public static string Decrypt(string Password, string Base64EncryptedString)
        {
            byte[] EncryptedDataByte = Convert.FromBase64String(Base64EncryptedString);
            KeyIV keyAndIV = KeyIV.GenerateKeyIV(Password);
            byte[] DecryptByte = Decrypt(keyAndIV, EncryptedDataByte);

            UTF8Encoding Enc = new UTF8Encoding();
            string DecryptString = Enc.GetString(DecryptByte);
            return DecryptString;
        }


        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="keyAndIV">Key 和 IV</param>
        /// <param name="EncryptedDataString">已经加密的文本</param>
        /// <returns>被解密的文本</returns>
        public static string Decrypt(KeyIV keyAndIV, string Base64EncryptedString)
        {
            byte[] EncryptedDataByte = Convert.FromBase64String(Base64EncryptedString);
            byte[] DecryptByte = Decrypt(keyAndIV, EncryptedDataByte);

            UTF8Encoding Enc = new UTF8Encoding();
            string DecryptString = Enc.GetString(DecryptByte);
            return DecryptString;
        }

        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="keyAndIV">Key 和 IV</param>
        /// <param name="EncryptedDataByte">已经加密的字节，需要解密的字节</param>
        /// <returns>解密后字节，原始数据</returns>
        public static byte[] Decrypt(KeyIV keyAndIV, byte[] EncryptedDataByte)
        {
            // Check arguments.
            if (EncryptedDataByte == null || EncryptedDataByte.Length <= 0)
                throw new ArgumentNullException("EncryptedDataByte");
            if (keyAndIV.Key == null || keyAndIV.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (keyAndIV.IV == null || keyAndIV.IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted byte[].
            //byte[] plainbyte;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyAndIV.Key;
                aesAlg.IV = keyAndIV.IV;

                //// Create a decryptor to perform the stream transform.
                //ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                //// Create the streams used for decryption.
                //using (MemoryStream msDecrypt = new MemoryStream(EncryptedDataByte))
                //{
                //    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                //    {
                //        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                //        {
                //            // Read the decrypted bytes from the decrypting stream
                //            // and place them in a string.
                //            //plainbyte = srDecrypt.ReadToEnd();

                //            BinaryReader br = new BinaryReader(srDecrypt.BaseStream);
                //            //br.Read(byte[], int, int);
                //            //或者
                //            plainbyte = br.ReadBytes(srDecrypt.Read);//.ReadBytes();
                //        }
                //    }
                //}

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, aesAlg.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(EncryptedDataByte, 0, EncryptedDataByte.Length);
                cs.FlushFinalBlock();

                return ms.ToArray();
            }

            //return plainbyte;
        }

    }


    /// <summary>
    /// 对称加密的Key和IV
    /// </summary>
    public sealed class KeyIV
    {
        /// <summary>
        /// 对称加密的Key
        /// </summary>
        public byte[] Key;

        /// <summary>
        /// 对称加密的IV
        /// </summary>
        public byte[] IV;


        /// <summary>
        /// 通过一个密码生成Key和IV，用于加密解密自己设定密码的地方
        /// </summary>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public static KeyIV GenerateKeyIV(string Password)
        {
            //KeyIV RandomKeyIV =  new KeyIV(); //SymmetricalEncrypt.GenerateRandomKeyIV();
            KeyIV RandomKeyIV = SymmetricalEncrypt.GenerateRandomKeyIV();

            //Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(pwd1, salt1);

            string HashStr = Tools.GetHashValue(Password + @"Key");
            //int keyLength = 16; // RandomKeyIV.Key.Length;
            int keyLength = RandomKeyIV.Key.Length;
            byte[] KeyArr = GetByteArr(HashStr, keyLength);

            //int IVLength = 16;  // RandomKeyIV.IV.Length;
            int IVLength = RandomKeyIV.IV.Length;
            HashStr = Tools.GetHashValue(Password + @"IV");
            byte[] IVArr = GetByteArr(HashStr, IVLength);

            KeyIV result = new KeyIV();
            result.IV = IVArr;
            result.Key = KeyArr;

            return result;
        }

        /// <summary>
        /// 根据字符，得到定长的字节数组
        /// </summary>
        /// <param name="HashStr">字符串</param>
        /// <param name="keyLength">字节数组长度</param>
        /// <returns>字节数组</returns>
        private static byte[] GetByteArr(string HashStr, int keyLength)
        {
            byte[] HashArr = System.Text.Encoding.UTF8.GetBytes(HashStr);

            byte[] result = new byte[keyLength];

            for (int i = 0; i < result.Length; i++)
            {
                if (i > HashArr.Length - 1)
                {
                    result[i] = 111;
                }
                else
                {
                    result[i] = HashArr[i];
                }
            }
            return result;
        }


        ///// <summary>
        ///// 得到字符串的hash值，使用Base64编码。
        ///// </summary>
        ///// <param name="ObjectText">目标文本</param>
        ///// <returns>Base64编码的hash值</returns>
        //private static string GetHashValue(string ObjectText)
        //{
        //    byte[] result = new byte[ObjectText.Length];
        //    SHA1 sha = new SHA1CryptoServiceProvider();
        //    result = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(ObjectText));
        //    string hashString = Convert.ToBase64String(result);
        //    return hashString;
        //}

    }





}
