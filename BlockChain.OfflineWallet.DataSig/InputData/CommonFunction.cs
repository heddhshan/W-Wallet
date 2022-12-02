using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.DataSig.InputData
{
    public class CommonFunction : IInputData
    {
        private string _FunctionName;
        private List<Parameter> _Parameters;

        public CommonFunction(string functionName, List<Parameter> parameters)
        {
            _FunctionName = functionName;
            _Parameters = parameters;
        }

        string IInputData.getFunctionName()
        {
            return _FunctionName;
        }

        string IInputData.getInputData(params object[] functionInput)
        {
            return InputDataHelper.getInputData(_FunctionName, _Parameters, functionInput);
        }

        List<Parameter> IInputData.getParameters()
        {
            return _Parameters;
        }

        List<object> IInputData.loadParameterValues(string inputData)
        {
            string sha3Signature = InputDataHelper.getSha3SignatureFunction(_FunctionName).ToHex(true);
            Parameter[] parameters = _Parameters.ToArray();
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
