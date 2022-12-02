using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Wallet.BLL
{
  public  class HDSource
    {
        /// <summary>
        /// 本钱包，官方钱包
        /// </summary>
        public static int FromThis = 0;


        /// <summary>
        /// 其他地方，
        /// </summary>
        public static int FromOther = 1;

       
        public static int WithoutPrivatekey = 2;

    }
}
