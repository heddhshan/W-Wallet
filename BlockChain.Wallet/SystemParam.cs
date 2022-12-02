using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Wallet
{
    internal class SystemParam
    {
        #region APP 版本信息，要和 合约定义一样！

        public const int AppId = 2;
        public const int Version = 20220903;

        #endregion

        public const string LinkInfo = @"mailto:alphawallet@outlook.com";    //注意，这个联系信息，要写协议！包括合约里面也是这样的！ 例如 mailto*** http***， 等

        public const string DefaultDbFileName = "WalletDb.mdf";


        public static string DefaultDbConStr
        {
            get
            {
                string d1 = Path.Combine(Environment.CurrentDirectory, "DataBase");
                string d2 = Path.Combine(d1, DefaultDbFileName);

                string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + d2 + @";Integrated Security=True";

                return con;
            }
        }


        /// <summary>
        /// 钱包helper合约，需要更新
        /// </summary>
        public static string AddressWalletHelper
        {
            get
            {
                var chainId = Share.ShareParam.GetChainId();
                if (chainId == 1)
                {
                    return "0xa7bf6541c109db78ce67a2897604eebcd36ac1bd";        //https://etherscan.io/address/0xa7bf6541c109db78ce67a2897604eebcd36ac1bd
                }
                else if (chainId == 5)
                {
                    return "0x04077d9ea84e6fcfac2d7073ce871a0f90771361";        //https://goerli.etherscan.io/address/0x04077d9ea84e6fcfac2d7073ce871a0f90771361
                }
                else
                {
                    throw new Exception("Not Deployed");
                }
            }
        }


        public static bool IsFirstRun
        {
            get
            {
                return Properties.Settings.Default.IsFirstRun;
            }
            set
            {
                Properties.Settings.Default.IsFirstRun = value;
                Properties.Settings.Default.Save();
            }
        }


        public static bool IsUpdateBalanceByBackground
        {
            get
            {
                return Properties.Settings.Default.IsUpdateBalanceByBackground;
            }
            set
            {
                Properties.Settings.Default.IsUpdateBalanceByBackground = value;
                Properties.Settings.Default.Save();
            }
        }

    }
}
