using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.DataSig.Model
{

    [Serializable]
    public class UiErc20TransferOffSigObj : UiBaseOffSigObj
    {
        public string TokenTo { get; set; }


        public BigInteger TokenAmount { get; set; }


        //public decimal TokenValue
        //{
        //    get
        //    {
        //        return (decimal)TokenAmount / (decimal)Math.Pow(10, ?);
        //    }
        //}



    }

}
