using BlockChain.Share;
using Microsoft.Win32;
using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowExeOffTx.xaml 的交互逻辑
    /// 本来只是做了离线签名的执行，但增加的小工具越来越多，就变成了百宝箱！
    /// </summary>
    public partial class WindowExeOffTx : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowExeOffTx()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            //LabelResult.Content = string.Empty;
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());

            //TextBoxHash.Text = Share.ShareParam.GetNowBytes32().ToHex(true);
            //ButtonExeOffTxUrl1.Tag = ShareParam.GetExeOffTxUrl();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //do nothing 
        }

        private async void OnPraseOffTx(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //Hashtable ht = new Hashtable();
                var Txs = TextBoxTx.Text.Split('\n');

                if (Txs.Length > 0)
                {
                    List<BlockChain.OfflineWallet.DataSig.Model.UiBaseOffSigObj> result = new List<OfflineWallet.DataSig.Model.UiBaseOffSigObj>();
                    int i = 1;

                    foreach (string t in Txs)
                    {
                        if (string.IsNullOrWhiteSpace(t)) { continue; }
                        string rlp = t.Trim();
                        var tx = Nethereum.Signer.Transaction1559Encoder.Current.Decode(rlp.HexToByteArray());

                        BlockChain.OfflineWallet.DataSig.Model.UiBaseOffSigObj m = new OfflineWallet.DataSig.Model.UiBaseOffSigObj();
                        m.InputData = tx.Data;
                        //m.TransactionType = tx.TransactionType;
                        m.TransactionHash = Common.Web3Helper.GetTxHashFrom(rlp);
                        m.ChainId = tx.ChainId;
                        m.MaxPriorityFeePerGas = (BigInteger)tx.MaxPriorityFeePerGas;
                        m.MaxFeePerGas = (BigInteger)tx.MaxFeePerGas;
                        m.EthWeiAmount = (BigInteger)tx.Amount;
                        m.Nonce = (BigInteger)tx.Nonce;
                        m.To = tx.ReceiverAddress;
                        m.From = Common.Web3Helper.TransactionGetAddressFrom(rlp);
                        m.GasLimit = (BigInteger)tx.GasLimit;

                        m.EthBalance = await Share.ShareParam.GetRealBalanceValue(m.From, Share.ShareParam.AddressEth);
                        m.IsSelected = m.HasEnoughEth;

                        m.IndexId = i; i++;
                        m.SigData = rlp;

                        m.CurrrentNonce = ShareParam.GetNonce(m.From);      //新增这个，方便比较

                        result.Add(m);
                    }

                    DataGridCommonTxSig.ItemsSource = result;

                    //nonce 检查
                    var query = result.GroupBy(x => { return x.From + "-" + x.Nonce.ToString(); }).Where(g => g.Count() > 1).Select(y => new { Element = y.Key, Counter = y.Count() }).ToList();
                    if (query != null && query.Count > 0)
                    {
                        string str = string.Empty;
                        foreach (var item in query)
                        {
                            str = str + item.Element.ToString() + ", From's Nonce Duplicate:" + item.Counter.ToString() + Environment.NewLine;
                        }
                        MessageBox.Show(Share.LanguageHelper.GetTranslationText("相同的From地址存在多个相同的Nonce，如果执行将只有一个事务会成功！") + Environment.NewLine + str,
                            Share.LanguageHelper.GetTranslationText("数据错误提示"));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }

        }

        private async void RefreshEth_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridCommonTxSig.ItemsSource != null)
            {
                this.Cursor = Cursors.Wait;
                ((IMainWindow)App.Current.MainWindow).ShowProcessing();
                try
                {
                    List<BlockChain.OfflineWallet.DataSig.Model.UiBaseOffSigObj> list = (List<BlockChain.OfflineWallet.DataSig.Model.UiBaseOffSigObj>)DataGridCommonTxSig.ItemsSource;
                    foreach (var m in list)
                    {
                        m.EthBalance = await Share.ShareParam.GetRealBalanceValue(m.From, Share.ShareParam.AddressEth);
                        m.IsSelected = m.IsSelected && m.HasEnoughEth;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                    Cursor = Cursors.Arrow;
                }
            }
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            string BackUpPath = Share.ShareParam.BackUpDir;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = BackUpPath;
            ofd.Filter = "Signature Files|*.sig|All Files|*.*";

            if (ofd.ShowDialog() == true)
            {
                var lines = System.IO.File.ReadAllText(ofd.FileName);
                TextBoxTx.Text = lines;
            }
        }


        private async void OnExeOffTx(object sender, RoutedEventArgs e)
        {
            if (!Share.BLL.Web3Url.Web3IsConnected)
            {
                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("好像没有连接到以太坊节点（Web3），如果继续执行可能会失败。确认要继续执行吗?"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }

            if (DataGridCommonTxSig.ItemsSource == null)
            {
                return;

            }

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();

            List<BlockChain.OfflineWallet.DataSig.Model.UiBaseOffSigObj> list = (List<BlockChain.OfflineWallet.DataSig.Model.UiBaseOffSigObj>)DataGridCommonTxSig.ItemsSource;

            try
            {
                string result = string.Empty;
                foreach (var m in list)
                {
                    if (!m.IsSelected) { continue; }

                    var web3 = Share.ShareParam.GetWeb3();
                    var txid = await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(m.SigData);

                    var from = Common.Web3Helper.TransactionGetAddressFrom(m.SigData);

                    Share.BLL.TransactionReceipt.LogTx(from, txid, TxUserMethod.OffiSig.ToString(), "***");

                    result = result + m.IndexId.ToString() + ", Transaction(Hash:" + txid + ") is sent" + Environment.NewLine;

                    try
                    {
                        var url = Share.ShareParam.GetTxUrl(txid);
                        System.Diagnostics.Process.Start("explorer.exe", url);
                    }
                    catch (Exception thisex)
                    {
                        log.Error("", thisex);
                    }
                }

                //LabelResult.Content = result;
                MessageBox.Show(result, "Exe Result:");
                //TextBoxTx.Text = string.Empty;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show( ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }


        private void OnGotoUrl(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var url = b.Tag.ToString();
                System.Diagnostics.Process.Start("explorer.exe", url);
            }
        }
        private void OnGotoAddress(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var adress = b.Content.ToString();
                var url = Share.ShareParam.GetAddressUrl(adress);
                System.Diagnostics.Process.Start("explorer.exe", url);
            }
        }

        private void OnGotoTx(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var tx = b.Content.ToString();
                var url = Share.ShareParam.GetTxUrl(tx);
                System.Diagnostics.Process.Start("explorer.exe", url);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static void ExeOffTx(Window _owner)
        {
            WindowExeOffTx w = new WindowExeOffTx();
            w.Owner = _owner;
            w.ShowDialog();
        }


    }

}
