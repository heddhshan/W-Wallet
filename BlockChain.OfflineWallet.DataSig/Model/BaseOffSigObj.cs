using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.DataSig.Model
{
    //[JsonObject("BaseOffSigObj")]

    [Serializable]
    public class BaseOffSigObj 
    {
        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("ChainId")]
        public BigInteger ChainId { get; set; }


        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("From")]
        public string From { get; set; }


        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("To")]
        public string To { get; set; }


        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("GasLimit")]
        public BigInteger GasLimit { get; set; }


        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("MaxFeePerGas")]
        public BigInteger MaxFeePerGas { get; set; }


        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("MaxPriorityFeePerGas")]
        public BigInteger MaxPriorityFeePerGas { get; set; }


        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("Nonce")]
        public BigInteger Nonce { get; set; }


        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("EthWeiAmount")]
        public BigInteger EthWeiAmount { get; set; }


        ////[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("InputData")]
        public string InputData { get; set; }


        //private string _InputData="0x";

        //protected virtual string getInputData()
        //{
        //    return _InputData;
        //}

        //protected virtual void setInputData(string _value)
        //{
        //     _InputData = _value;
        //}

        //public string InputData { get { return getInputData(); } set { setInputData(value); } }



    }


}
