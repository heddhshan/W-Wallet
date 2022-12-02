using BlockChain.Share;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.Web3;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowTxStatus.xaml 的交互逻辑
    /// </summary>
    public partial class WindowTxStatus : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private DispatcherTimer timer = new DispatcherTimer();

        public WindowTxStatus()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            DataGridCurTxs.ItemsSource = BLL.TransactionReceipt.GetDoingTx();
            DataGridHisTxs.ItemsSource = BLL.TransactionReceipt.GetTop500HisTx().DefaultView;

            timer.Interval = TimeSpan.FromSeconds(60);  //每分钟刷新就好，毕竟有手动刷新！
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void OnRefresh(object sender, RoutedEventArgs e)
        {
            DataGridCurTxs.ItemsSource = BLL.TransactionReceipt.GetDoingTx();
            DataGridHisTxs.ItemsSource = BLL.TransactionReceipt.GetTop500HisTx().DefaultView;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (w != null)
            {
                DataGridCurTxs.ItemsSource = BLL.TransactionReceipt.GetDoingTx();
                DataGridHisTxs.ItemsSource = BLL.TransactionReceipt.GetTop500HisTx().DefaultView;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            w = null;
        }

        private static WindowTxStatus w = null;

        public static void ShowTxStatus(Window _owner)
        {
            if (w == null)
            {
                w = new WindowTxStatus();
                w.Owner = _owner;   // App.Current.MainWindow;
                //w.WindowStyle = WindowStyle.None;
                w.ShowInTaskbar = true;
                w.Show();
            }
            else
            {
                
                if (w.WindowState == WindowState.Minimized) 
                {
                    w.WindowState = WindowState.Normal;
                    //w.Show(); 
                }
                else if (w.WindowState == WindowState.Maximized || w.WindowState == WindowState.Normal)
                {
                    //w.Show(); 
                    w.Activate(); 
                }
            }
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void OnUmchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }

        private void OnGotoTxUrl(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var txid = b.Content.ToString();
                System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetTxUrl(txid));
            }
        }

        /// <summary>
        /// 替换 事务， 只限 UI 线程调用！
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
        public static async Task<bool> UiReplaceTransaction(Window owner, string tx)
        {
            try
            {
                var web3 = Share.ShareParam.GetWeb3();                      //getPendingTransactions
                var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(tx);
                if (receipt != null)
                {
                    MessageBox.Show("Transaction Is Done!");
                    return false;
                }

                var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(tx);     // ??? 能取得 pending 状态的相关消息吗？ yes

                if (transaction != null)
                {
                    string password;
                    Share.WindowPassword.GetPassword(owner, transaction.From, "Get Password", out password);
                    var account = new Share.BLL.Address().GetAccount(transaction.From, password);
                    if (account == null) { MessageBox.Show(owner, @"没有这个账号！"); return false; }

                    var result = await Share.ShareParam.CancelTransaction(tx, account, transaction.Nonce, transaction.GasPrice.Value);

                    MessageBox.Show("Replace Transaction Result: " + result.ToString());

                    return result;
                }
                else
                {
                    MessageBox.Show("Transaction Is not found, TransactionHash: " + tx);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
                return false;
            }
        }


        private async void CancelTransaction_Click(object sender, RoutedEventArgs e)
        {
            //取消事务， 逻辑：得到 nonce， 再使用 该 nonce 重写发送一笔交易！ 同时记录该笔交易为取消(记录日志就可以了，数据库也记录)！
            if (DataGridCurTxs.SelectedItem is Model.TransactionReceipt)
            {
                var m = (Model.TransactionReceipt)DataGridCurTxs.SelectedItem;
                string tx = m.TransactionHash;

                await UiReplaceTransaction(this, tx);
            }


        }


    }

}
