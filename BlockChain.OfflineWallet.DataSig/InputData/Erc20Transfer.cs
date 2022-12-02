using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using System.Reflection;

namespace BlockChain.OfflineWallet.DataSig.InputData
{

    /// <summary>
    /// demo : 使用 dll 实现接口 IInputData 实现 参数输入转换成 inputdata ！  这个功能目前（202210）没做。
    /// </summary>
    public class Erc20Transfer : IInputData
    {
        public string getFunctionName()
        {
            return "transfer(address,uint256)";            //transfer(address, uint256)： 0xa9059cbb
        }

        public string getInputData(params object[] functionInput)
        {
            return InputDataHelper.getInputData(getFunctionName(), getParameters(), functionInput);
        }

        public List<Parameter> getParameters()
        {
            //transfer(address, uint256)：
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("address", "to", 1));
            parameters.Add(new Parameter("uint256", "amount", 2));
            return parameters;
        }


        public List<object> loadParameterValues(string inputData)
        {
            string sha3Signature = InputDataHelper.getSha3SignatureFunction(getFunctionName()).ToHex(true);
            Parameter[] parameters = getParameters().ToArray();
            var fun = new FunctionCallDecoder();
            List<ParameterOutput> result = fun.DecodeFunctionInput(sha3Signature, inputData, parameters);

            List<object> list = new List<object>();
            foreach (var p in result)
            {
                list.Add(p.Result);
            }

            return list;
        }

    }

}