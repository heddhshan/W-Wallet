using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Profile;

namespace BlockChain.Wallet.BLL
{


    //public interface IMulticallInputOutput
    //{
    //    string Target { get; set; }
    //    byte[] GetCallData();
    //    void Decode(byte[] output);
    //    bool Success { get; set; }
    //    bool AllowFailure { get; set; }
    //    BigInteger Value { get; set; }
    //}

    /// <summary>
    /// 通用的 Multi Call 的 输入输出参数定义
    /// </summary>
    [Serializable]
    public class MultiCallInputOutput : Nethereum.Contracts.QueryHandlers.MultiCall.IMulticallInputOutput
    {
        private Guid _Id;

        public Guid Id { get => _Id; set => _Id = value; }

        private string _Target;
        public string Target { get => _Target; set => _Target = value; }


        private bool _Success;
        public bool Success { get => _Success; set => _Success = value; }


        private bool _AllowFailure;

        public bool AllowFailure { get => _AllowFailure; set => _AllowFailure = value; }

        private BigInteger _Value;
        public BigInteger Value { get => _Value; set => _Value = value; }

        public decimal DecimalValue { get =>  (decimal)((double)_Value / Math.Pow(10, 18) ); }

        private string?  _CallOutput;

        public string? CallOutput { get => _CallOutput; set => _CallOutput = value; }

        public void Decode(byte[] output)
        {
            if (output != null)
            {
                _CallOutput = output.ToHex();
            }
            else {
                _CallOutput = "";
            }
        }

        private string? _InputData;

        public string? InputData { get => _InputData; set => _InputData = value; }

        public byte[] GetCallData()
        {
            if (_InputData != null)
            {
                return _InputData.HexToByteArray();
            }
            else
            {
                return null;
                //return new byte[0];
            }
        }

    }
}
