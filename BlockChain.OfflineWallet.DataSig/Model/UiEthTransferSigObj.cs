using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.DataSig.Model
{
    public class UiEthTransferSigObj : UiBaseOffSigObj
    {

        public decimal EthValue
        {
            get
            {
                return (decimal)EthWeiAmount / (decimal)Math.Pow(10, 18);
            }
        }

    }
}
