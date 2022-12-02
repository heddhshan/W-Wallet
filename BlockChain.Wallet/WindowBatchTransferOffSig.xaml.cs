using BlockChain.Common;
using BlockChain.Share;
using Nethereum.Web3;
using Org.BouncyCastle.Math;
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
using Nethereum.Contracts;
using BlockChain.Share.Model;
using BlockChain.Wallet.Model;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using Microsoft.Win32;
using System.Data;
using System.Numerics;
using BigInteger = System.Numerics.BigInteger;
using Nethereum.Util;

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowBatchTransferOffSig.xaml 的交互逻辑
    /// </summary>
    public partial class WindowBatchTransferOffSig : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowBatchTransferOffSig()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var tokens = Share.BLL.Token.GetAllSelectedToken().DefaultView;
            ComboBoxToken1.ItemsSource = tokens;
            ComboBoxToken1.SelectedValuePath = "Address";

            ComboBoxToken2.ItemsSource = tokens;
            ComboBoxToken2.SelectedValuePath = "Address";

            //助记词选择
            ComboBoxHD4.SelectedValuePath = "MneId";
            ComboBoxHD4.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private static readonly AsyncLock m_lock = new AsyncLock();    //异步锁, 需要等等当前执行完成后才进行其他操作的时候使用

        #region 多个地址向一个地址转账

        private async void OnSelectionChanged4(object sender, SelectionChangedEventArgs e)
        {
            using (var releaser = await m_lock.LockAsync())     //使用异步锁
            {
                var row = ComboBoxHD4.SelectedItem;
                if (row is System.Data.DataRowView && ComboBoxToken1.SelectedValue != null)
                {
                    this.Cursor = Cursors.Wait;
                    ((IMainWindow)App.Current.MainWindow).ShowProcessing();
                    try
                    {
                        System.Data.DataRowView r = (System.Data.DataRowView)row;
                        var MneId = (Guid)ComboBoxHD4.SelectedValue;
                        var ThisTokenAddress = ComboBoxToken1.SelectedValue.ToString();
                        DataGridHDAddress4.ItemsSource = BLL.Address.GetHDAddressAmount2(MneId, ThisTokenAddress);                                 //这个需要改变！
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
        }

        private void OnSelectAll(object sender, RoutedEventArgs e)
        {

            SelectAll(true);
        }

        private void OnUnSelectAll(object sender, RoutedEventArgs e)
        {

            SelectAll(false);
        }

        private async void SelectAll(bool IsSelected)
        {
            using (var releaser = await m_lock.LockAsync())     //使用异步锁
            {
                if (DataGridHDAddress4 == null || DataGridHDAddress4.ItemsSource == null) return;

                var list = (List<Model.ViewAddressRealBalance>)DataGridHDAddress4.ItemsSource;
                foreach (var m in list)
                {
                    m.IsSelected = IsSelected;
                }
            }

        }

        private async void OnRefeshDG4(object sender, RoutedEventArgs e)
        {
            using (var releaser = await m_lock.LockAsync())     //使用异步锁
            {
                //需要处理 nonce 等信息！
                this.Cursor = Cursors.Wait;
                ((IMainWindow)App.Current.MainWindow).ShowProcessing();
                try
                {
                    if (DataGridHDAddress4 == null || DataGridHDAddress4.ItemsSource == null) return;
                    if (ComboBoxToken1 == null || ComboBoxToken1.SelectedValue == null) return;

                    var tokenAddress = ComboBoxToken1.SelectedValue.ToString();
                    //var tokenmodel = Share.BLL.Token.GetModel(tokenAddress);
                    var BlockNumber = Common.Web3Helper.GetNowBlockNuber(Share.ShareParam.Web3Url);
                    var web3 = Share.ShareParam.GetWeb3();

                    var list = (List<Model.ViewAddressRealBalance>)DataGridHDAddress4.ItemsSource;
                    foreach (var m in list)
                    {
                        if (!m.IsSelected)
                        { continue; }

                        await UpdateTokenAmount(tokenAddress, m);

                        //m.TokenAmountTransfer = 0;                //保留以前录入的数据
                        m.BlockNumber = BlockNumber;

                        m.Nonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(m.Address);      // ShareParam.GetNonce(m.Address);

                        System.Threading.Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("", ex); MessageBox.Show(ex.Message);
                }
                finally
                {
                    ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                    Cursor = Cursors.Arrow;
                }

            }
        }

        private static async Task UpdateTokenAmount(string tokenAddress, ViewAddressRealBalance m)
        {
            m.TokenAddress = tokenAddress;

            var tokenmodel = Share.BLL.Token.GetModel(tokenAddress);
            var eth = await ShareParam.GetRealBalanceValue(m.Address, ShareParam.AddressEth);
            m.EthValue = eth;
            m.EthAmount = (decimal)((double)eth / Math.Pow(10, 18));
            m.BlockNumber = Common.Web3Helper.GetNowBlockNuber(ShareParam.Web3Url);

            if (Share.ShareParam.IsAnEmptyAddress(tokenAddress))
            {
                var tokenvalue = eth;
                m.TokenValue = m.EthValue;
                m.TokenAmount = m.EthAmount;
                m.TokenSymbol = tokenmodel.Symbol;      //ETH
            }
            else
            {
                var tokenvalue = await ShareParam.GetRealBalanceValue(m.Address, tokenAddress);
                m.TokenValue = tokenvalue;
                m.TokenAmount = (decimal)((double)tokenvalue / Math.Pow(10, tokenmodel.Decimals));
                m.TokenSymbol = tokenmodel.Symbol;
            }
        }

        private async void GenDataForOffSig_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                using (var releaser = await m_lock.LockAsync())     //使用异步锁
                {
                    var MaxFeePerGas = System.Numerics.BigInteger.Parse(this.TextBoxMaxFeePerGas.Text);
                    var MaxPriorityFeePerGas = System.Numerics.BigInteger.Parse(this.TextBoxMaxPriorityFeePerGas.Text);
                    var GasLimit = System.Numerics.BigInteger.Parse(TextBoxGasLimit.Text);
                    if (string.IsNullOrWhiteSpace(TextBoxTo.Text) || !TextBoxTo.Text.IsValidEthereumAddressHexFormat()) { TextBoxTo.Focus(); return; }

                    List<OfflineWallet.DataSig.Model.UiBaseOffSigObj> result = new List<OfflineWallet.DataSig.Model.UiBaseOffSigObj>();
                    int i = 1;

                    var list = (List<Model.ViewAddressRealBalance>)DataGridHDAddress4.ItemsSource;
                    if (list == null)
                    {
                        return;
                    }
                    foreach (var m in list)
                    {
                        if (m.IsSelected)
                        {
                            OfflineWallet.DataSig.Model.UiBaseOffSigObj obj = new OfflineWallet.DataSig.Model.UiBaseOffSigObj();

                            obj.IndexId = i; i++;
                            obj.IsSelected = true;
                            obj.Nonce = m.Nonce;
                            obj.From = m.Address;
                            obj.ChainId = Share.ShareParam.GetChainId();


                            //eth
                            if (Share.ShareParam.IsAnEmptyAddress(m.TokenAddress))
                            {
                                obj.To = TextBoxTo.Text;            //1

                                if (m.TokenAmountTransfer == 0)     //2
                                {
                                    obj.EthWeiAmount = m.EthValue;
                                }
                                else
                                {
                                    obj.EthWeiAmount = (System.Numerics.BigInteger)((m.TokenAmountTransfer) * (decimal)Share.ShareParam.ether);                                 //可能数据超限了！！！ 
                                }

                                obj.InputData = "0x";               //3
                            }
                            //token
                            else
                            {
                                var AmountTo = TextBoxTo.Text;
                                obj.To = m.TokenAddress;            //1

                                obj.EthWeiAmount = 0;               //2

                                if (m.TokenAmountTransfer == 0)     //3
                                {
                                    obj.InputData = new OfflineWallet.DataSig.InputData.Erc20Transfer().getInputData(AmountTo, 0);
                                }
                                else
                                {
                                    //var amount = (System.Numerics.BigInteger)((double)m.TokenAmountTransfer * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(m.TokenAddress)));  //可能有误差 
                                    var amount = (System.Numerics.BigInteger)(m.TokenAmountTransfer * (decimal)Share.ShareParam.getPowerValue(Share.BLL.Token.GetTokenDecimals(m.TokenAddress)));          //可能数据超限了！！！ 
                                    obj.InputData = new OfflineWallet.DataSig.InputData.Erc20Transfer().getInputData(AmountTo, amount);
                                }
                            }


                            obj.GasLimit = GasLimit;
                            obj.MaxPriorityFeePerGas = MaxPriorityFeePerGas;
                            obj.MaxFeePerGas = MaxFeePerGas;

                            result.Add(obj);
                        }
                    }

                    DataGridCommonTxForSig.ItemsSource = result;
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

        private async void UpdateBlockNumber_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                using (var releaser = await m_lock.LockAsync())     //使用异步锁
                {
                    if (ComboBoxToken1.SelectedValue != null
                    && DataGridHDAddress4.SelectedItem != null
                    && DataGridHDAddress4.SelectedItem is Model.ViewAddressRealBalance)
                    {
                        var m = (Model.ViewAddressRealBalance)DataGridHDAddress4.SelectedItem;

                        var tokenAddress = ComboBoxToken1.SelectedValue.ToString();
                        await UpdateTokenAmount(tokenAddress, m);
                    }
                }
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void GetOffDataForSig_Click(object sender, RoutedEventArgs e)
        {

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //格式：
                //ChainId,From,To,Nonce,GasLimit,MaxFeePerGas,MaxPriorityFeePerGas,EthWeiAmount,InputData

                if (DataGridCommonTxForSig.ItemsSource != null)
                {
                    var list = (List<OfflineWallet.DataSig.Model.UiBaseOffSigObj>)DataGridCommonTxForSig.ItemsSource;
                    //var result = list.Select(x => x.IsSelected == true);
                    bool IsEth = true;

                    //方法1
                    List<string> text = new List<string>();
                    foreach (OfflineWallet.DataSig.Model.UiBaseOffSigObj m in list)
                    {
                        if (!m.IsSelected)
                        {
                            continue;
                        }
                        string s = m.ChainId.ToString() + "," + m.From + "," + m.To + "," + m.Nonce.ToString() + "," + m.GasLimit.ToString() + "," + m.MaxFeePerGas.ToString() + "," + m.MaxPriorityFeePerGas.ToString() + ","
                                + m.EthWeiAmount.ToString() + "," + m.InputData;
                        text.Add(s);

                        IsEth = m.InputData == "0x" || string.IsNullOrEmpty(m.InputData);       //只需要设置一遍！这里重复执行了
                    }

                    ////方法2
                    //string jsointxt = JsonConvert.SerializeObject(result.ToArray());

                    string BackUpPath = Share.ShareParam.BackUpDir;
                    string filename = System.IO.Path.Combine(BackUpPath, "TokenTransferTxForSig_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".wfs");   //waiting for sig
                    if (IsEth)
                    {
                        filename = System.IO.Path.Combine(BackUpPath, "EtherTransferTxForSig_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".wfs");        //waiting for sig
                    }

                    //System.IO.File.WriteAllText(filename, jsointxt);
                    System.IO.File.WriteAllLines(filename, text);
                    System.Diagnostics.Process.Start("Explorer", "/select," + filename);
                }
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void Gas_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var eip1559 = Share.ShareParam.GetEip1559Fee();
                this.TextBoxMaxFeePerGas.Text = eip1559.MaxFeePerGas.ToString();
                this.TextBoxMaxPriorityFeePerGas.Text = eip1559.MaxPriorityFeePerGas.ToString();

                var ThisTokenAddress = ComboBoxToken1.SelectedValue.ToString();
                LabeSelectedToken.Content = Share.BLL.Token.GetModel(ThisTokenAddress).Symbol;

                if (Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                {
                    TextBoxGasLimit.Text = 21000.ToString();
                    TextBoxGasLimit.Foreground = Brushes.Blue;
                }
                else
                {
                    ////TextBoxGasLimit
                    //var ThisTokenAddress = ComboBoxToken1.SelectedValue.ToString();
                    //var web3 = Share.ShareParam.GetWeb3();
                    //var contract = web3.Eth.GetContract<Nethereum.StandardTokenEIP20.StandardTokenService>(ThisTokenAddress);
                    //var transferFunction = contract.GetFunction("transfer");
                    //var gas = await transferFunction.EstimateGasAsync(TextBoxTo.Text, null, null, TextBoxTo.Text, 123);
                    TextBoxGasLimit.Text = 100000.ToString();       //这个不好评估，自己去填写就行了，默认参照usdt的gaslimit，一般不会超过 10 万！
                    TextBoxGasLimit.Foreground = Brushes.Red;
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

        private void ComboBoxToken1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnSelectionChanged4(null, null);
        }

        #endregion


        #region 一个地址向多个地址转账


        private void OnSelectAll2(object sender, RoutedEventArgs e)
        {
            SelectItem();
        }

        private void OnUnSelectAll2(object sender, RoutedEventArgs e)
        {
            SelectItem();
        }

        private void SelectItem()
        {
            if (DataGridToAddressAmount != null && DataGridToAddressAmount.ItemsSource != null)
            {
                bool IsSelected = CheckBoxSelectAll42.IsChecked == true;
                var ls = (List<OfflineWallet.DataSig.Model.UiTransferObj>)DataGridToAddressAmount.ItemsSource;
                foreach (var m in ls)
                {
                    m.IsSelected = IsSelected;

                }
            }
        }

        private void LoadAddressAmount_Click(object sender, RoutedEventArgs e)
        {
            //导入文本格式 ： address,amount,   （地址，金额）
            if (ComboBoxToken2.SelectedValue == null) return;


            decimal allamount = 0;

            string token = ComboBoxToken2.SelectedValue.ToString();
            var tm = Share.BLL.Token.GetModel(token);
            string from = TextBoxFrom.Text;
            var nonce = ShareParam.GetNonce(from);

            string BackUpPath = Share.ShareParam.BackUpDir;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "To Address Amount File|*.taa|All File|*.*";

            ofd.InitialDirectory = BackUpPath;
            if (ofd.ShowDialog() == true)
            {
                var lines = System.IO.File.ReadAllLines(ofd.FileName);
                List<OfflineWallet.DataSig.Model.UiTransferObj> ls = new List<OfflineWallet.DataSig.Model.UiTransferObj>();
                int i = 1;

                foreach (var line in lines)
                {
                    var arr = line.Split(',');

                    OfflineWallet.DataSig.Model.UiTransferObj m = new OfflineWallet.DataSig.Model.UiTransferObj();

                    m.TokenTo = arr[0];
                    //var bigamount = decimal.Parse(arr[1]) * (decimal)Share.ShareParam.getPowerValue( Share.BLL.Token.GetTokenDecimals(token));
                    m.TokenAmountTransfer = decimal.Parse(arr[1]);// (BigInteger)bigamount;

                    m.From = from;
                    m.FromNonce = nonce;
                    nonce++;

                    m.TokenAddress = token;
                    m.TokenSymbol = tm.Symbol;

                    m.IndexId = i;
                    i++;
                    m.IsSelected = true;

                    ls.Add(m);

                    allamount = allamount + m.TokenAmountTransfer;
                }

                DataGridToAddressAmount.ItemsSource = ls;
                LabelToAllAmount.Content = allamount.ToString();


                var fromamount = (decimal)LabelFromAmount.Content;

                SetAmountColor(allamount, fromamount);
            }
        }

        private void RefreshAllAmount_Click(object sender, RoutedEventArgs e)
        {
            //刷新：
            if (DataGridToAddressAmount.ItemsSource != null)
            {
                this.Cursor = Cursors.Wait;
                ((IMainWindow)App.Current.MainWindow).ShowProcessing();
                try
                {
                    //总金额
                    var list = (List<OfflineWallet.DataSig.Model.UiTransferObj>)DataGridToAddressAmount.ItemsSource;
                    var allamount = list.Sum(x => { if (x.IsSelected) return x.TokenAmountTransfer; return 0; });
                    LabelToAllAmount.Content = allamount.ToString();

                    //from
                    string from = TextBoxFrom.Text;

                    //token
                    string token = "***";
                    if (ComboBoxToken2.SelectedValue != null)
                    {
                        token = ComboBoxToken2.SelectedValue.ToString();
                    }

                    //nonce
                    var nonce = ShareParam.GetNonce(from);

                    //更新
                    foreach (OfflineWallet.DataSig.Model.UiTransferObj vo in list)
                    {
                        vo.TokenAddress = token;
                        vo.TokenSymbol = Share.BLL.Token.GetModel(token).Symbol;
                        vo.From = from;
                        vo.FromNonce = nonce;
                        nonce++;
                    }

                    //LabelToAllAmount
                    var fromamount = (decimal)LabelFromAmount.Content;
                    SetAmountColor(allamount, fromamount);

                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                }
                finally
                {
                    ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                    Cursor = Cursors.Arrow;
                }
            }
        }

        private void SetAmountColor(decimal allamount, decimal fromamount)
        {
            if (allamount <= fromamount)
            {
                LabelToAllAmount.Foreground = Brushes.Green;
            }
            else
            {
                LabelToAllAmount.Foreground = Brushes.Red;
            }
        }

        private void Gas_Click2(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var eip1559 = Share.ShareParam.GetEip1559Fee();
                this.TextBoxMaxFeePerGas2.Text = eip1559.MaxFeePerGas.ToString();
                this.TextBoxMaxPriorityFeePerGas2.Text = eip1559.MaxPriorityFeePerGas.ToString();

                var ThisTokenAddress = ComboBoxToken2.SelectedValue.ToString();
                LabeSelectedToken2.Content = Share.BLL.Token.GetModel(ThisTokenAddress).Symbol;

                if (Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                {
                    TextBoxGasLimit2.Text = 21000.ToString();
                    TextBoxGasLimit2.Foreground = Brushes.Blue;
                }
                else
                {
                    ////TextBoxGasLimit
                    //var ThisTokenAddress = ComboBoxToken1.SelectedValue.ToString();
                    //var web3 = Share.ShareParam.GetWeb3();
                    //var contract = web3.Eth.GetContract<Nethereum.StandardTokenEIP20.StandardTokenService>(ThisTokenAddress);
                    //var transferFunction = contract.GetFunction("transfer");
                    //var gas = await transferFunction.EstimateGasAsync(TextBoxTo.Text, null, null, TextBoxTo.Text, 123);
                    TextBoxGasLimit2.Text = 100000.ToString();       //这个不好评估，自己去填写就行了，默认参照usdt的gaslimit，一般不会超过 10 万！
                    TextBoxGasLimit2.Foreground = Brushes.Red;
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

        private async void GenDataForOffSig_Click2(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                using (var releaser = await m_lock.LockAsync())     //使用异步锁
                {
                    var MaxFeePerGas = System.Numerics.BigInteger.Parse(this.TextBoxMaxFeePerGas2.Text);
                    var MaxPriorityFeePerGas = System.Numerics.BigInteger.Parse(this.TextBoxMaxPriorityFeePerGas2.Text);
                    var GasLimit = System.Numerics.BigInteger.Parse(TextBoxGasLimit2.Text);

                    List<OfflineWallet.DataSig.Model.UiBaseOffSigObj> result = new List<OfflineWallet.DataSig.Model.UiBaseOffSigObj>();
                    int i = 1;

                    var list = (List<OfflineWallet.DataSig.Model.UiTransferObj>)DataGridToAddressAmount.ItemsSource;
                    foreach (var m in list)
                    {
                        if (m.IsSelected)
                        {
                            OfflineWallet.DataSig.Model.UiBaseOffSigObj obj = new OfflineWallet.DataSig.Model.UiBaseOffSigObj();

                            obj.IndexId = i; i++;
                            obj.IsSelected = true;
                            obj.Nonce = m.FromNonce;
                            obj.From = m.From;
                            obj.ChainId = Share.ShareParam.GetChainId();

                            var amount = (BigInteger)(m.TokenAmountTransfer * (decimal)Share.ShareParam.getPowerValue(Share.BLL.Token.GetTokenDecimals(m.TokenAddress)));

                            //eth
                            if (Share.ShareParam.IsAnEmptyAddress(m.TokenAddress))
                            {
                                obj.To = m.TokenTo;                 //1

                                //if (m.TokenAmountTransfer == 0)     //2
                                //{
                                obj.EthWeiAmount = amount;
                                //}
                                //else
                                //{
                                //    obj.EthWeiAmount = (System.Numerics.BigInteger)((m.TokenAmountTransfer) * (decimal)Share.ShareParam.ether);                                 //可能数据超限了！！！ 
                                //}

                                obj.InputData = "0x";               //3
                            }
                            //token
                            else
                            {
                                obj.To = m.TokenAddress;            //1

                                obj.EthWeiAmount = 0;               //2

                                //if (m.TokenAmountTransfer == 0)     //3
                                //{
                                obj.InputData = new OfflineWallet.DataSig.InputData.Erc20Transfer().getInputData(obj.To, amount);
                                //}
                                //else
                                //{
                                //    //var amount = (System.Numerics.BigInteger)((double)m.TokenAmountTransfer * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(m.TokenAddress)));  //可能有误差 
                                //    var amount = (BigInteger)amount;   
                                //    obj.InputData = new OfflineWallet.DataSig.Erc20Transfer().getInputData(obj.To, amount);
                                //}
                            }

                            obj.GasLimit = GasLimit;
                            obj.MaxPriorityFeePerGas = MaxPriorityFeePerGas;
                            obj.MaxFeePerGas = MaxFeePerGas;

                            result.Add(obj);
                        }
                    }

                    DataGridCommonTxForSig2.ItemsSource = result;
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

        private void GetOffDataForSig_Click2(object sender, RoutedEventArgs e)
        {
            //格式：
            //ChainId,From,To,Nonce,GasLimit,MaxFeePerGas,MaxPriorityFeePerGas,EthWeiAmount,InputData

            if (DataGridCommonTxForSig2.ItemsSource != null)
            {
                var list = (List<OfflineWallet.DataSig.Model.UiBaseOffSigObj>)DataGridCommonTxForSig2.ItemsSource;
                //var result = list.Select(x => x.IsSelected == true);
                bool IsEth = true;
                //方法1
                List<string> text = new List<string>();
                foreach (OfflineWallet.DataSig.Model.UiBaseOffSigObj m in list)
                {
                    if (!m.IsSelected)
                    {
                        continue;
                    }
                    string s = m.ChainId.ToString() + "," + m.From + "," + m.To + "," + m.Nonce.ToString() + "," + m.GasLimit.ToString() + "," + m.MaxFeePerGas.ToString() + "," + m.MaxPriorityFeePerGas.ToString() + ","
                            + m.EthWeiAmount.ToString() + "," + m.InputData;
                    text.Add(s);

                    IsEth = m.InputData == "0x" || string.IsNullOrEmpty(m.InputData);       //只需要设置一遍！这里重复执行了
                }

                ////方法2
                //string jsointxt = JsonConvert.SerializeObject(result.ToArray());

                string BackUpPath = Share.ShareParam.BackUpDir;
                //string filename = System.IO.Path.Combine(BackUpPath, "TransferTxForSig_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".wfs");
                string filename = System.IO.Path.Combine(BackUpPath, "TokenTransferTxForSig_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".wfs");   //waiting for sig
                if (IsEth)
                {
                    filename = System.IO.Path.Combine(BackUpPath, "EtherTransferTxForSig_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".wfs");        //waiting for sig
                }

                //System.IO.File.WriteAllText(filename, jsointxt);
                System.IO.File.WriteAllLines(filename, text);
                System.Diagnostics.Process.Start("Explorer", "/select," + filename);
            }
        }

        private void Token2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFromTokenAmount();
        }

        private void From2_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateFromTokenAmount();
        }

        private void UpdateFromTokenAmount()
        {

            // LabelFromAmount 更新token金额
            if (string.IsNullOrEmpty(TextBoxFrom.Text) || ComboBoxToken2.SelectedItem == null)
            {
                return;
            }
            try
            {
                string from = TextBoxFrom.Text;
                string token = ComboBoxToken2.SelectedValue.ToString();

                var amount = ShareParam.GetRealBalance(from, token, false);

                LabelFromAmount.Content = (decimal)amount;
            }
            catch (Exception ex)
            {
                LabelFromAmount.Content = "?";
                log.Error("", ex);
            }
        }




        #endregion


    }

}
