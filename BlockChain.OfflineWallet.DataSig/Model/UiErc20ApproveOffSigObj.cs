using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.DataSig.Model
{
    [Serializable]
    public class UiErc20ApproveOffSigObj: UiBaseOffSigObj
    {
        public string Spender { get; set; }



        public BigInteger TokenAmount { get; set; }

    }

}
