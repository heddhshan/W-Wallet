using Nethereum.ABI;
using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.DataSig
{

    public static class InputDataHelper
    {
        /// <summary>
        /// 得到 function 签名的 hash，  
        /// </summary>
        /// <param name="functionname"></param>
        /// <returns></returns>
        public static string getFunctionHash(string functionname)
        {
            var hash = Nethereum.Web3.Web3.Sha3(functionname);
            return hash;
        }


        /// <summary>
        /// 得到 function 的签名，hash 后取前四个字节，   //   transfer(address, uint256)： 0xa9059cbb
        /// </summary>
        /// <param name="functionname"></param>
        /// <returns></returns>
        public static byte[] getSha3SignatureFunction(string functionname)
        {
            var abiEncode = new ABIEncode();
            var fun = abiEncode.GetSha3ABIEncodedPacked(functionname);      //取前面四个字节

            byte[] output = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                output[i] = fun[i];
            }

            return output;//.ToHex(true);

            //这两种写法一样的！

            //var hash = Nethereum.Web3.Web3.Sha3(functionname);
            //if (!hash.StartsWith("0x"))
            //{
            //    hash = "0x" + hash;
            //}
            //var result = hash.Substring(0, 10);
            //return result.HexToByteArray();
        }

        /// <summary>
        /// 对输入参数编码，得到 inputdata  不适用于 eth 转账
        /// </summary>
        /// <param name="functionname"></param>
        /// <param name="parameters"></param>
        /// <param name="functionInput"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string getInputData(string functionname, List<Nethereum.ABI.Model.Parameter> parameters, params object[] functionInput)
        {
            byte[] fun4 = getSha3SignatureFunction(functionname);

            if (parameters != null && functionInput != null)
            {
                if (parameters.Count != functionInput.Length)
                {
                    throw new Exception("parameters.Count != functionInput.Length 1");
                }

                Nethereum.ABI.FunctionEncoding.FunctionCallEncoder coder = new Nethereum.ABI.FunctionEncoding.FunctionCallEncoder();
                var result = coder.EncodeRequest(fun4.ToHex(), parameters.ToArray(), functionInput);

                return result;
            }
            else if (parameters == null && functionInput == null)
            {
                Nethereum.ABI.FunctionEncoding.FunctionCallEncoder coder = new Nethereum.ABI.FunctionEncoding.FunctionCallEncoder();
                var result = fun4.ToHex(true);
                return result;
            }
            else 
            {
                throw new Exception("parameters.Count != functionInput.Length 2");
            }
        }


    }

}
