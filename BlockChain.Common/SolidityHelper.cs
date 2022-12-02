using Nethereum.Hex.HexConvertors.Extensions;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Text;

namespace BlockChain.Common
{
    public static class SolidityHelper
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region 字符，16进制，byte[] 之间的转换, 主要用于使用 bytes32 来存储短文本

        ///// <summary>
        ///// 字符转换为字节，使用Utf8格式
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static byte[] Str2ByteArr(string s)
        //{
        //    byte[] arrByte = System.Text.Encoding.UTF8.GetBytes(s);
        //    return arrByte;
        //}

        ///// <summary>
        ///// 字节数组转为16进制
        ///// </summary>
        ///// <param name="arrByte"></param>
        ///// <returns></returns>
        //public static string ByteArr2Hex(byte[] arrByte)
        //{
        //    string result = string.Empty;
        //    for (int i = 0; i < arrByte.Length; i++)
        //    {
        //        result += System.Convert.ToString(arrByte[i], 16);        //Convert.ToString(byte, 16)把byte转化成十六进制string 
        //    }
        //    return result;
        //}


        ///// <summary>
        ///// 字符串转16进制
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static string Str2Hex(string s)
        //{
        //    string result = string.Empty;
        //    byte[] arrByte = System.Text.Encoding.UTF8.GetBytes(s);
        //    for (int i = 0; i < arrByte.Length; i++)
        //    {
        //        result += System.Convert.ToString(arrByte[i], 16);        //Convert.ToString(byte, 16)把byte转化成十六进制string 
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 把字符串转换成bytes32的16进制，超长会报异常
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static byte[] StrToBytes32(string s)
        //{
        //    byte[] arrByte = System.Text.Encoding.UTF8.GetBytes(s);
        //    if (arrByte.Length > 32)
        //    {
        //        throw new Exception("超长，长度超过bytes32");
        //    }

        //    byte[] Result = new byte[32];/// arrByte;

        //    for (int i = 0; i < 32; i++)
        //    {
        //        if (i < arrByte.Length)
        //        {
        //            Result[i] = arrByte[i];
        //        }
        //        else
        //        {
        //            Result[i] = 0;
        //        }
        //    }

        //    return Result;
        //}

        ///// <summary>
        ///// Bytes32 转换成字符串（不是16进制字符，是文本字符）
        ///// </summary>
        ///// <param name="arrByte"></param>
        ///// <returns></returns>
        //public static string Bytes32ToStr(byte[] arrByte)
        //{
        //    string s = System.Text.Encoding.UTF8.GetString(arrByte);
        //    s = s.Replace("\0", "").Trim();     //把多余的 \0 去掉
        //    return s;
        //}

        ///// <summary>
        ///// 16进制转字符串
        ///// </summary>
        ///// <param name="hex"></param>
        ///// <returns></returns>
        //public static string Hex2Str(string hex)
        //{
        //    byte[] arrByte = hex.HexToByteArray();      // System.Text.Encoding.UTF8.GetBytes(s);
        //    string s = System.Text.Encoding.UTF8.GetString(arrByte);
        //    return s;
        //}

        #endregion


        ///// <summary>
        ///// 把16进制地址转换成符合Solidity函数参数类型的byte数组
        ///// </summary>
        ///// <param name="hex"></param>
        ///// <returns></returns>
        //public static byte[] Hex2ByteArr(string hex)    //==HexByteConvertorExtensions.HexToByteArray 
        //{
        //    var inputByteArray = new byte[hex.Length / 2];
        //    for (var x = 0; x < inputByteArray.Length; x++)
        //    {
        //        var i = System.Convert.ToInt32(hex.Substring(x * 2, 2), 16);
        //        inputByteArray[x] = (byte)i;
        //    }

        //    return inputByteArray;
        //}

        /// <summary>
        /// 把uint转换成符合Solidity函数参数类型的byte数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetSolidityUint256(uint value)
        {
            return new byte[32]
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                (byte)((value >> 24) & 0xFF),
                (byte)((value >> 16) & 0xFF),
                (byte)((value >> 8) & 0xFF),
                (byte)value,
            };
        }


        public static byte[] GetSolidityUint256(System.Numerics.BigInteger value)
        {
            var result = value.ToByteArray();
            if (result.Length < 32)
            {
                var R = new byte[32];

                for (int i = 0; i < 32; i++)
                {
                    if (i < result.Length)
                    {
                        R[i] = result[i];
                    }
                    else
                    {
                        R[i] = 0;
                    }
                }
                Array.Reverse(R);           //反转!!!
                return R;
            }
            else
            {
                //非法的！
                return result;
            }
        }


        ///// <summary>
        ///// 注意了：solidity6.9版本，没有byte，只有uint256！
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static byte[] GetSolidityByte(byte value)
        //{
        //    return new byte[1]
        //    {
        //        value,
        //    };
        //}


        ///// <summary>
        ///// 把uint数组转换成符合Solidity函数参数类型的byte数组
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static byte[] UintArr2ByteArr(uint[] value)  //==HexByteConvertorExtensions.HexToByteArray 
        //{
        //    System.Collections.Generic.List<byte> l = new System.Collections.Generic.List<byte>();
        //    foreach (uint i in value)
        //    {
        //        var add = GetSolidityUint256(i);
        //        foreach (var j in add)
        //        {
        //            l.Add(j);
        //        }
        //    }
        //    var a = l.ToArray();
        //    return a;
        //}

        ////todo: 具体的hash需要定义一个方法，

        //public static byte[] CalculateHash(byte[] msgsender, byte[] addressthis, byte[] _betNums, byte[] currentGameId, byte[] _betAmounts, byte[] eventId, byte[] _nonce)
        //{
        //    var digest = new KeccakDigest(256);
        //    var output = new byte[digest.GetDigestSize()];
        //    digest.BlockUpdate(msgsender, 0, msgsender.Length);
        //    digest.BlockUpdate(addressthis, 0, addressthis.Length);
        //    digest.BlockUpdate(_betNums, 0, _betNums.Length);

        //    digest.BlockUpdate(currentGameId, 0, currentGameId.Length);
        //    digest.BlockUpdate(_betAmounts, 0, _betAmounts.Length);
        //    digest.BlockUpdate(eventId, 0, eventId.Length);
        //    digest.BlockUpdate(_nonce, 0, _nonce.Length);

        //    digest.DoFinal(output, 0);
        //    return output;
        //}



        //public static string CalGameResultHash(byte gameResult, string spacing, string nonce)
        //{
        //    var input1 = GetSolidityByte(gameResult);
        //    var input2 = Encoding.UTF8.GetBytes(spacing);
        //    var input3 = Encoding.UTF8.GetBytes(nonce);
        //    var output = CalculateHash(input1, input2, input3);
        //    return output.ToHex(true);
        //}



        //public static string CalculateHash(uint value1, string value2, string value3)
        //{
        //    var input1 = GetSolidityUint256(value1);
        //    var input2 = Encoding.UTF8.GetBytes(value2);
        //    var input3 = Encoding.UTF8.GetBytes(value3);
        //    var output = CalculateHash(input1, input2, input3);
        //    return output.ToHex(true);
        //}

        //public static byte[] CalculateHash(byte[] value1, byte[] value2, byte[] value3)
        //{
        //    var digest = new KeccakDigest(256);
        //    var output = new byte[digest.GetDigestSize()];
        //    digest.BlockUpdate(value1, 0, value1.Length);
        //    digest.BlockUpdate(value2, 0, value2.Length);
        //    digest.BlockUpdate(value3, 0, value3.Length);
        //    digest.DoFinal(output, 0);
        //    return output;
        //}



        #region 数据类型转换


        public static System.Numerics.BigInteger Double2BigInteger(double _value)
        {
            if (_value > (double)decimal.MaxValue) 
            {
                // 这种方式有舍入误差，不过都这么大的值了，没关系的。 在某些情况下很关键，例如 erc20 approve，少一点都无法 transferFrom ！！！
                var result = (System.Numerics.BigInteger)_value; 
                return result;
            }

            var r1 = (decimal)_value;
            var r2 = (System.Numerics.BigInteger)r1;

            //// 测试转换是否正确！
            //var r11 = (decimal)r2;
            //var r12 = (double)r11;
            //if (r12 != _value) 
            //{
            //    throw new Exception("Double2BigInteger Failed， input _value=" + _value.ToString() + " | output value=" + r12.ToString());            
            //}

            return r2;
        }


        public static System.Numerics.BigInteger Decimal2BigInteger(decimal _value)
        {
            var r1 = (System.Numerics.BigInteger)_value;
         
            //// 测试转换是否正确！
            //var r11 = (decimal)r1;
            //if (r11 != _value)
            //{
            //    throw new Exception("Double2BigInteger Failed， input _value=" + _value.ToString() + " | output value=" + r11.ToString());
            //}

            return r1;
        }

        #endregion

    }
}
