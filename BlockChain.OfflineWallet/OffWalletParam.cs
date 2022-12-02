using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet
{
    public static class OffWalletParam
    {
        #region APP 版本信息，要和 合约定义一样！

        public const int AppId = 1;
        public const int Version = 20220903;

        #endregion

        public const string LinkInfo = "mailto:alphawallet@outlook.com";

        public const string DefaultDbFileName = "OfflineWalletDb.mdf";


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


    }
}
