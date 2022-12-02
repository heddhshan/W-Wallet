using NBitcoin.Secp256k1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlockChain.OfflineWallet.DataSig.Model
{

    [Serializable]
    //[JsonObject("UiBaseOffSigObj")]
    public class UiBaseOffSigObj : BaseOffSigObj, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        ////[JsonProperty("IndexId")]
        //[DataMember(Name = "IndexId")]
        public int IndexId { get; set; }


        private bool _IsSelected = true;

        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("IsSelected")]
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnPropertyChanged("IsSelected"); } }

        public string TransactionHash { get; set; }

        private string _SigData = "0x";

        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("SigData")]
        public string SigData
        {
            get
            {
                return _SigData;
            }
            set
            {
                _SigData = value;
                OnPropertyChanged("SigData");
            }
        }


        public BigInteger EthBalance{ get; set; }

        public BigInteger TransactionFee
        { 
            get {
                return GasLimit * MaxFeePerGas;         //得到预估的事务费用！
            }
        }

        /// <summary>
        /// 是否有足够的 eth 执行事务！
        /// </summary>
        public bool HasEnoughEth { get { return EthBalance >= TransactionFee + EthWeiAmount;   }  }


        /// <summary>
        /// From 当前从网络读取的 Nonce ！
        /// </summary>
        public BigInteger CurrrentNonce { get; set; }


        public bool NonceIsOk { get { return Nonce >= CurrrentNonce; } }


    }

}
