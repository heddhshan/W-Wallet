using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Share.Model
{

    [Serializable]
    public class UserTokenBalance
    {
        public string UserAddress { get; set; }

        public string TokenAddress { get; set; }

        public double Balance { get; set; }

        public BigInteger BigBalance { get; set; }

    }

}
