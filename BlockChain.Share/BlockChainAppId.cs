using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Share
{


    /// <summary>
    /// 定义常见几个 APPID， 但是版本在各个 APP 的 Param 里面定义
    /// </summary>
    public static class BlockChainAppId
    {
      

        /// <summary>
        /// 平台ID， windows 10  不要定义在一起，不便于扩展，定义在各个APP中。
        /// </summary>
        public const uint Platform_Windows = 10;
        public const uint PlatformId = Platform_Windows;


        //public const int AppId_OfflineWallet = 1;
        //public const int AppId_Wallet = 2;



    }


}
