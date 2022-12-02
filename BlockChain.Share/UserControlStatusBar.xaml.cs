using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlockChain.Share
{
    /// <summary>
    /// UserControlStatusBar.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlStatusBar : UserControl
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserControlStatusBar()
        {
            InitializeComponent();
        }

      
        public async Task<bool> UpdateInfo()
        {
            //Web3Url = _Web3Url;
            //Web3UrlAlias = _Web3UrlAlias;
            bool Ok = await UpdateStatusInfo();
            log.Info("UpdateStatusInfo: " + Ok.ToString());
            return Ok;
        }

        public void ShowInfo(string _Info)
        {
            LabelInfo.Content = _Info;  
        }

        private async void OnClickRefreshBlockInfo(object sender, RoutedEventArgs e)
        {
            //UpdateStatusInfo();
            bool Ok = await UpdateStatusInfo();
            log.Info("UpdateStatusInfo: " + Ok.ToString());
        }

        /// <summary>
        /// 更新状态栏信息，定时      网络交互很慢，必须采用异步！！！
        /// </summary>
        private async Task<bool> UpdateStatusInfo()
        {
            var a1 = System.DateTime.Now;
            log.Info("---UpdateStatusInfo---begin    Refresh Geth Server Info, From: " + ShareParam.Web3Url);
            this.Cursor = Cursors.Wait;

            ////progressBar1.IsIndeterminate = true;
            ShowProcessing();  

            System.Threading.Thread.Sleep(0);

            try
            {
                var model = BLL.Web3Url.GetCurWeb3UrlModel(false);
                if (model == null) return false;

                LabelBlockTime.Content = "Block Time: ***";
                LabelChainId.Content = "ChainId: ***";
                LabelLatestBlock.Content = "Block High: ***";
                LabelGasPrice.Content = "Gas Price: ***";

                LabelBlockTime.ToolTip = "Year-Month-Day Hour:Minute:Second";

                LabelApiUrl.Content = model.UrlAlias;           // Web3UrlAlias ;    // BLL.Web3Url.GetCurWeb3UrlModel().UrlAlias;
                LabelApiUrl.ToolTip = model.Url;                // Web3Url;          // BLL.Web3Url.GetCurWeb3UrlModel().Url;

                var web3 = Share.ShareParam.GetWeb3();      
                Contract.AppInfo.AppInfoService service = new Contract.AppInfo.AppInfoService(web3, ShareParam.AddressAppInfo);

                var TaskTime = await ShareParam.getNowBlockTimestamp();
                LabelBlockTime.Content = "Block Time: " + Common.DateTimeHelper.ConvertInt2DateTime((long)TaskTime).ToString(Common.DateTimeHelper.DefaultDateTimeFormat);

                var TaskBlock = web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                LabelLatestBlock.Content = "Block High: " + (await TaskBlock).ToString();

                var TaskGasPrice = web3.Eth.GasPrice.SendRequestAsync();
                var g = await TaskGasPrice;
                var gp = double.Parse(g.ToString()) / Math.Pow(10, 9);
                LabelGasPrice.Content = "Gas Price: " + gp.ToString("0.#########") + "(Gwei)";      //保留9位小数！ 因为L2 的手续费都是 0.0***的, 测试网的手续费也很低，甚至低于 0.00000001 。

                var TaskChainId = web3.Eth.ChainId.SendRequestAsync();
                LabelChainId.Content = "ChainId: " + (await TaskChainId).ToString();

                System.Threading.Thread.Sleep(0);             
                return true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return false;
            }
            finally
            {
                //progressBar1.IsIndeterminate = false;
                ShowProcesed();   
                System.Threading.Thread.Sleep(0);
                Cursor = Cursors.Arrow;
                var a2 = System.DateTime.Now;

                log.Info("---UpdateStatusInfo---end: seconds = " + (a2 - a1).TotalSeconds.ToString());
            }
        }


        public static int Counter = 0;

        public void ShowProcessing(string msg = "Processing")
        {
            lock (this)
            {
                TextBlockStatus.Text = msg;
                progressBar1.IsIndeterminate = true;
                Counter = Counter + 1;
                System.Threading.Thread.Sleep(1);
            }
        }

        public void ShowProcesed(string msg = "Ready")
        {
            //if (this != null)
            //{
            lock (this)
            {
                Counter = Counter - 1;
                if (Counter <= 0)
                {
                    Counter = 0;
                    TextBlockStatus.Text = msg;
                    progressBar1.IsIndeterminate = false;
                    System.Threading.Thread.Sleep(1);
                }
            }
            //}
        }
    }
}
