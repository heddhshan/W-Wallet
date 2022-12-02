using Nethereum.Util;
//using Nethereum.Util.AddressExtensions; //IsValidEthereumAddressHexFormat
using Microsoft.Win32;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;

using BlockChain.Share;
using System.Data;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts;
using NBitcoin.Secp256k1;

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowTransfer.xaml 的交互逻辑
    /// </summary>
    public partial class WindowTransfer : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowTransfer()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            // 初始化 选中的 Token ，是 ETH
            LbelTokenDecimals.Tag = 18;
            //DataGridToken.Tag = ShareParam.AddressEth;
            LabelTokenSymbol.Tag = "ETH";

            //token
            DataGridToken.SelectedValuePath = "Address";
            DataGridToken.ItemsSource = Share.BLL.Token.GetAllSelectedToken().DefaultView;

            //from 地址
            var dv = new BLL.Address().GetAllTxAddress();// BLL.Address.GetAllAddress().DefaultView;
            ComboBoxAddress1.SelectedValuePath = "Address";
            ComboBoxAddress1.ItemsSource = dv;
            ComboBoxAddress2.SelectedValuePath = "Address";
            ComboBoxAddress2.ItemsSource = dv;

            //联系人
            ComboBoxContact1.ItemsSource = BLL.Contact.GetAllContact().DefaultView;


            //助记词选择
            ComboBoxHD4.ItemsSource = BLL.HDWallet.GetHDWallets(false).DefaultView;
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ShowGasPrice();
        }

        //private async void ShowGasPrice()
        //{
        //    this.Cursor = Cursors.Wait;
        //    ((IMainWindow)App.Current.MainWindow).ShowProcessing();
        //    try
        //    {
        //        var gasprice = Share.ShareParam.GetDefaultGasPriceAsync() / Math.Pow(10, 9);//  (double)await Common.Web3Helper.GetLastGasPrice(Share.ShareParam.Web3Url) / Math.Pow(10, 9);
        //        //Slider1.Value = gasprice;
        //        //Slider2.Value = gasprice;
        //        //Slider3.Value = gasprice;
        //        //Slider4.Value = gasprice;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("", ex);
        //    }
        //    finally
        //    {
        //        ((IMainWindow)App.Current.MainWindow).ShowProcesed();
        //        Cursor = Cursors.Arrow;
        //    }
        //}

        private int ThisTabIndex = -1;

        private void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TabControlMain.SelectedIndex == ThisTabIndex) { return; }

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //把地址列表全部动一次
                if (this.TabControlMain.SelectedIndex == 0)
                {
                    OnSelectionChanged1(null, null);
                }
                else if (this.TabControlMain.SelectedIndex == 1)
                {
                    OnSelectionChanged2(null, null);
                }
                else if (this.TabControlMain.SelectedIndex == 2)
                { }
                else if (this.TabControlMain.SelectedIndex == 3)
                {
                    //<ComboBox  Name="ComboBoxHD4" Grid.Row="0" Grid.Column="1" VerticalAlignment="Center" SelectionChanged="OnSelectionChanged4">
                    OnSelectionChanged4(null, null);
                }
            }
            finally
            {
                ThisTabIndex = this.TabControlMain.SelectedIndex;
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void OnTokenChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                RadioButton rb = (RadioButton)sender;

                var row = DataGridToken.SelectedItem;
                if (row is System.Data.DataRowView)
                {
                    System.Data.DataRowView r = (System.Data.DataRowView)row;

                    LbelTokenDecimals.Tag = rb.Tag;             //  r["Decimals"];//TODO： 不知为什么，不能写成  r["Decimals"] ，按道理是可以的！！！
                    //DataGridToken.Tag = r["Address"];
                    _ThisTokenAddress = r["Address"].ToString();
                    LabelTokenSymbol.Tag = r["Symbol"];

                    //把地址列表全部动一次
                    ThisTabIndex = -1;
                    TabControlMain_SelectionChanged(null, null);
                }
            }
        }

        private string _ThisTokenAddress = ShareParam.EmptyAddress;

        private string ThisTokenAddress
        {
            get
            {
                return _ThisTokenAddress;
                //if (DataGridToken.Tag != null)
                //    return DataGridToken.Tag.ToString();
                //else
                //    return ShareParam.EmptyAddress;                //return "0x0000000000000000000000000000000000000000";
            }
        }

        private int ThisTokenDecimals
        {
            get
            {
                if (LbelTokenDecimals.Tag != null)
                    return (int)LbelTokenDecimals.Tag;
                else
                    return 18;
            }
        }

        private string ThisTokenSymbol
        {
            get
            {
                if (LabelTokenSymbol.Tag != null)
                    return LabelTokenSymbol.Tag.ToString();
                else
                    return "*";
            }
        }


        /// <summary>
        /// 转账（或Sender）From的来源类型，助记词或KeyStore
        /// </summary>
        public enum FromType { HDWallet, KeyStore };


        public void SetDefault(string address)
        {
            if (string.IsNullOrEmpty(address) || Share.ShareParam.IsAnEmptyAddress(address))
            {
                return;
            }
            if (TabControlMain.SelectedIndex == 0)
            {
                ComboBoxAddress1.SelectedIndex = 1;
            }
        }


        private void OnContactSelected1(object sender, SelectionChangedEventArgs e)
        {
            var row = ComboBoxContact1.SelectedItem;

            if (row != null && row is System.Data.DataRowView)
            {
                System.Data.DataRowView r = (System.Data.DataRowView)row;
                TextBoxToAddress1.Text = r["ContactAddress"].ToString();

                PopupContact1.IsOpen = false;
            }
        }

        private void OnSelectContact1(object sender, RoutedEventArgs e)
        {
            PopupContact1.IsOpen = !PopupContact1.IsOpen;
            ComboBoxContact1.IsDropDownOpen = PopupContact1.IsOpen;

            if (!PopupContact1.IsOpen)
            {
                if (string.IsNullOrEmpty(TextBoxToAddress1.Text))
                {
                    OnContactSelected1(null, null);
                }
            }
        }



        /// <summary>
        /// 计算可用的所有余额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnChecked1(object sender, RoutedEventArgs e)
        {
            if (ComboBoxAddress1.SelectedValue == null) { return; }

            if (!Share.BLL.Web3Url.Web3IsConnected)
            {
                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("好像没有连接到以太坊节点（Web3），如果继续执行可能会失败。确认要继续执行吗"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }

            if (!(sender is CheckBox))
            {
                return;
            }
            var c = (CheckBox)sender;
            TextBoxAmount1.IsEnabled = c.IsChecked == true;
            if (c.IsChecked != true)
            {
                return;
            }

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var from = ComboBoxAddress1.SelectedValue.ToString();
                var web3 = Share.ShareParam.GetWeb3();
                if (ThisTokenAddress == "0x0" || Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                {
                    try
                    {
                        await CalMaxEth(from, web3);
                    }
                    catch (Exception ex)
                    {
                        log.Error(from + "CalculateTotalAmountToTransferWholeBalanceInEther", ex);
                        TextBoxAmount1.Text = "0";
                    }
                }
                else
                {
                    Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, ThisTokenAddress);
                    var amount = await service.BalanceOfQueryAsync(from);
                    TextBoxAmount1.Text = ((double)amount / Math.Pow(10, ThisTokenDecimals)).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("CheckBoxAllA_CheckedChanged", ex);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async Task<bool> CalMaxEth(string from, Web3 web3)
        {
            if (CheckBoxAllAmount.IsChecked == true)
            {
                try
                {
                    BigInteger maxFeePerGas = BigInteger.Parse(this.TextBoxMaxFeePerGas.Text);
                    var amount = await web3.Eth.GetEtherTransferService().CalculateTotalAmountToTransferWholeBalanceInEtherAsync(from, maxFeePerGas);
                    TextBoxAmount1.Text = amount.ToString();

                    return true;
                }
                catch (Exception ex)
                {
                    TextBoxAmount1.Text = "0";
                    log.Error("private async void CalMaxEth(string from, Web3 web3)", ex);
                    return false;
                }
            }
            return false;
        }



        public static string CalculateTransactionHash(string rawSignedTransaction)
        {
            var sha3 = new Sha3Keccack();
            return sha3.CalculateHashFromHex(rawSignedTransaction);
        }


        private async void OnGenOffSig1(object sender, RoutedEventArgs e)
        {
            //生产离线签名消息
            if (!ButtonOffSig.IsEnabled)
            {
                return;
            }

            ButtonOffSig.IsEnabled = false;
            //this.Cursor = Cursors.Wait;
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //var eip1559 = ShareParam.GetEip1559Fee();
                //eip1559.MaxFeePerGas = 
                //eip1559.BaseFee = 

                var MaxGas = int.Parse(TextBoxGasLimit.Text);
                //var gasprice = (BigInteger)await Common.Web3Helper.GetLastGasPrice(Share.ShareParam.Web3Url);   // / (decimal)Math.Pow(10, 9);// (decimal)1000000000;
                //var gasprice = (BigInteger)(Slider1.Value * Math.Pow(10, 9));

                var from = ComboBoxAddress1.SelectedValue.ToString();

                //var password = WindowPassword.GetPassword("转账");
                string password;
                if (!WindowPassword.GetPassword(this, from, LanguageHelper.GetTranslationText("输入密码"), out password))
                {
                    return;
                }

                var account = new BLL.Address().GetAccount(from, password);
                if (null == account)
                {
                    MessageBox.Show(this, LanguageHelper.GetTranslationText("账号不存在，多半是密码错误导致。"));
                    return;
                }

                //account.ChainId = null;

                var to = TextBoxToAddress1.Text.Trim();
                var amount = decimal.Parse(this.TextBoxAmount1.Text.Trim());     //默认金额

                if (ThisTokenAddress == "0x0" || Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                {
                    var thisnonce = int.Parse(TextBoxNonce.Text);
                    amount = amount * (decimal)Math.Pow(10, 18);

                    var BigA = (BigInteger)amount;

                    ////// LegacyTransaction 这种事务签名
                    //Nethereum.Signer.LegacyTransaction tx;      //事务分三种封装格式：旧的（Legacy），2390 ，1559， 详情参见： https://eips.ethereum.org/EIPS/eip-1559
                    //if (MaxGas == 0)
                    //{
                    //    tx = new Nethereum.Signer.LegacyTransaction(to, BigA, thisnonce);  //没有提供 不包括gas但包括gasprice的 方法。
                    //}
                    //else
                    //{
                    //    tx = new Nethereum.Signer.LegacyTransaction(to, BigA, thisnonce, gasprice, MaxGas);
                    //}

                    //public Transaction1559(BigInteger chainId, BigInteger? nonce, BigInteger? maxPriorityFeePerGas, BigInteger? maxFeePerGas, BigInteger? gasLimit, string receiverAddress, BigInteger? amount, string data, List<AccessListItem> accessList)
                    //public Transaction1559(BigInteger chainId, BigInteger nonce, BigInteger maxPriorityFeePerGas, BigInteger maxFeePerGas, BigInteger gasLimit, string receiverAddress, BigInteger amount, string data, List<AccessListItem> accessList, EthECDSASignature signature)
                    BigInteger maxPriorityFeePerGas = BigInteger.Parse(this.TextBoxMaxPriorityFeePerGas.Text);
                    BigInteger maxFeePerGas = BigInteger.Parse(this.TextBoxMaxFeePerGas.Text);
                    //BigInteger BaseFee = BigInteger.Parse(this.TextBoxBaseFee.Text);

                    Nethereum.Signer.Transaction1559 tx;      //事务分三种封装格式：Transaction1559， 详情参见： https://eips.ethereum.org/EIPS/eip-1559
                    tx = new Nethereum.Signer.Transaction1559(ShareParam.GetChainId(), thisnonce, maxPriorityFeePerGas, maxFeePerGas, MaxGas, to, BigA, "", null);

                    //签名
                    tx.Sign(new Nethereum.Signer.EthECKey(account.PrivateKey.HexToByteArray(), true));
                    var msg = tx.GetRLPEncoded().ToHex(true);
                    //hash
                    Nethereum.Signer.EthereumMessageSigner m = new Nethereum.Signer.EthereumMessageSigner();
                    var h1 = m.Hash(msg.HexToByteArray());
                    var txid = h1.ToHex(true);

                    TextBoxOffSig.Text = msg;
                    TextBoxTxId.Text = txid;
                    return;
                }
                else
                {
                    var web3 = Share.ShareParam.GetWeb3(account);
                    //web3.TransactionManager.UseLegacyAsDefault = true;      //目前只支持 Legacy 模式
                    //ERC20 Token transfer
                    amount = amount * (decimal)Math.Pow(10, ThisTokenDecimals);
                    Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, ThisTokenAddress);
                    Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction param = new Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction
                    {
                        To = to,
                        Value = (BigInteger)amount,
                        //Gas= MaxGas,
                        //GasPrice = gasprice,
                    };
                    if (MaxGas > 0)
                    {
                        param.Gas = MaxGas;
                    }

                    var msg = await service.ContractHandler.SignTransactionAsync<Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction>(param);
                    Nethereum.Signer.EthereumMessageSigner m = new Nethereum.Signer.EthereumMessageSigner();
                    var txid = m.Hash(msg.HexToByteArray()).ToHex(true);

                    TextBoxOffSig.Text = "0x" + msg;
                    TextBoxTxId.Text = txid;
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error("*", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ButtonOffSig.IsEnabled = true;
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void OnRefreshNonce(object sender, RoutedEventArgs e)
        {
            if (!Share.BLL.Web3Url.Web3IsConnected)
            {
                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("好像没有连接到以太坊节点（Web3），如果继续执行可能会失败。确认要继续执行吗"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                string address = ComboBoxAddress1.SelectedValue.ToString();

                RefeshGasAndNonce(address);
            }
            catch (Exception ex)
            {
                log.Error("OnRefreshNonce", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }

        }

        private async void RefeshGasAndNonce(string address)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                TextBoxNonce.Text = ShareParam.GetNonce(address).ToString();
                var eip1559 = ShareParam.GetEip1559Fee();
                TextBoxBaseFee.Text = eip1559.BaseFee.ToString();
                TextBoxMaxPriorityFeePerGas.Text = eip1559.MaxPriorityFeePerGas.ToString();
                TextBoxMaxFeePerGas.Text = eip1559.MaxFeePerGas.ToString();

                //处理 gaslimit 的评估！
                if (ComboBoxAddress1.SelectedValue != null && !string.IsNullOrEmpty(TextBoxToAddress1.Text) && DataGridToken.SelectedValue != null)
                {
                    try
                    {
                        string from = ComboBoxAddress1.SelectedValue.ToString();
                        string to = TextBoxToAddress1.Text;
                        string token = DataGridToken.SelectedValue.ToString();
                        string data = "0x";
                        if (Share.ShareParam.IsAnEmptyAddress(token))
                        {
                            //do nothing    不写死，采用统一的方法  Common.Web3Helper.GetGasLimit(
                            //TextBoxGasLimit.Text = "21000";
                        }
                        else
                        {
                            var amount = decimal.Parse(TextBoxAmount1.Text);
                            var d = Share.BLL.Token.GetTokenDecimals(token);
                            Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction param = new Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction()
                            {
                                To = to,
                                Value = (BigInteger)(amount * (decimal)ShareParam.getPowerValue(d)),
                            };

                            data = param.GetCallData().ToHex(true);
                            to = token;
                        }

                        TextBoxGasLimit.Text = (await Common.Web3Helper.GetGasLimit(Share.ShareParam.Web3Url, from, to, data)).ToString();
                    }
                    catch (Exception ex)
                    {
                        log.Error("", ex);
                        TextBoxGasLimit.Text = "???";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }


        private async void OnSliderValueChanged1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CheckBoxAllAmount.IsChecked == true)
            {
                if (ThisTokenAddress == "0x0" || Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                {
                    this.Cursor = Cursors.Wait;
                    ((IMainWindow)App.Current.MainWindow).ShowProcessing();
                    try
                    {
                        //重新计算最高转账金额}
                        var web3 = Share.ShareParam.GetWeb3();
                        var from = ComboBoxAddress1.SelectedValue.ToString();
                        await CalMaxEth(from, web3);
                    }
                    finally
                    {
                        ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                        Cursor = Cursors.Arrow;
                    }
                }
            }
        }

        private async void OnTransfer1(object sender, RoutedEventArgs e)
        {
            //转账
            if (!ButtonTransfer1.IsEnabled)
            {
                return;
            }

            if (!Share.BLL.Web3Url.Web3IsConnected)
            {
                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("好像没有连接到以太坊节点（Web3），如果继续执行可能会失败。确认要继续执行吗?"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }

            var to = TextBoxToAddress1.Text.Trim();
            Nethereum.Util.AddressUtil AU = new Nethereum.Util.AddressUtil();
            if (!AU.IsValidEthereumAddressHexFormat(to))
            {
                MessageBox.Show(this, LanguageHelper.GetTranslationText("接收地址输入错误。"), "Message", MessageBoxButton.OK);
                TextBoxToAddress1.Focus();
                return;
            }

            ButtonTransfer1.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var MaxGas = int.Parse(TextBoxGasLimit.Text);
                var from = ComboBoxAddress1.SelectedValue.ToString();

                string password;
                if (!WindowPassword.GetPassword(this, from, LanguageHelper.GetTranslationText("输入密码"), out password))
                {
                    return;
                }
                var account = new BLL.Address().GetAccount(from, password);

                if (null == account)
                {
                    MessageBox.Show(this, LanguageHelper.GetTranslationText("账号不存在，多半是密码错误导致。"));
                    return;
                }

                var web3 = Share.ShareParam.GetWeb3(account);

                var amount = decimal.Parse(this.TextBoxAmount1.Text.Trim());     //默认金额
                string tranhash = string.Empty;

                BigInteger maxFeePerGas = BigInteger.Parse(this.TextBoxMaxFeePerGas.Text);
                BigInteger maxPriorityFee = BigInteger.Parse(this.TextBoxMaxPriorityFeePerGas.Text);

                //web3.Eth.TransactionManager.UseLegacyAsDefault = false;

                if (ThisTokenAddress == "0x0" || Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                {
                    //以太币转账
                    if (CheckBoxAllAmount.IsChecked == true)
                    {
                        try
                        {
                            //Task<decimal> CalculateTotalAmountToTransferWholeBalanceInEtherAsync(string address, decimal gasPriceGwei, BigInteger? gas = null);
                            //Task<decimal> CalculateTotalAmountToTransferWholeBalanceInEtherAsync(string address, BigInteger maxFeePerGas, BigInteger? gas = null);
                            if (MaxGas == 0)
                            {
                                amount = await web3.Eth.GetEtherTransferService().CalculateTotalAmountToTransferWholeBalanceInEtherAsync(from, maxFeePerGas);
                            }
                            else
                            {
                                amount = await web3.Eth.GetEtherTransferService().CalculateTotalAmountToTransferWholeBalanceInEtherAsync(from, maxFeePerGas, MaxGas);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(from, ex);
                            MessageBox.Show(this, LanguageHelper.GetTranslationText("不能转账，多半是账户余额不够或网络问题") + Environment.NewLine + ex.Message, "Message", MessageBoxButton.OK);
                            return;
                        }
                    }

                    if (CheckBoxToIsConAddr.IsChecked != true)
                    {
                        //solidity 6.8 版本改了：转账的gas费用可以超过2100，特别是给合约转账，合约里面有处理逻辑，其gas必然要高于21000。所以这里暂时写 十万 gas 。
                        //todo ： 还要看看合约之间的调用具体的 gas
                        //tranhash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, amount, (decimal)await Share.ShareParam.GetDefaultGasPriceAsync(), (BigInteger)1000000);
                        if (MaxGas == 0)
                        {
                            //TransferEtherAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, BigInteger? nonce = null);
                            tranhash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, amount);
                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Transfer.ToString(), "To:" + to + ",Eth:" + amount.ToString());
                        }
                        else
                        {
                            //Task<string> TransferEtherAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, BigInteger? nonce = null);
                            //Task<string> TransferEtherAsync(string toAddress, decimal etherAmount, BigInteger maxPriorityFee, BigInteger maxFeePerGas, BigInteger? gas = null, BigInteger? nonce = null);
                            if (web3.Eth.TransactionManager.UseLegacyAsDefault)
                            {
                                //Task<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, BigInteger? nonce = null, CancellationTokenSource tokenSource = null);
                                tranhash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, amount);     // 某些网络不支持1559，无法设置 maxPriorityFee 这里也不支持 GasPrice 了！
                            }
                            else
                            {
                                // Task<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, BigInteger maxPriorityFee, BigInteger maxFeePerGas, BigInteger? gas = null, BigInteger? nonce = null, CancellationTokenSource tokenSource = null);
                                tranhash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, amount, maxPriorityFee, maxFeePerGas, MaxGas);
                            }
                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Transfer.ToString(), "To:" + to + ",Eth:" + amount.ToString());
                        }
                    }
                    else
                    {
                        //https://github.com/Nethereum/Nethereum/blob/12b188dab1a8dbbbf4ff9d273d956e15c7f053e0/src/Nethereum.RPC/TransactionManagers/EtherTransferService.cs#L29
                        //public Task<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, CancellationTokenSource tokenSource = null, BigInteger? nonce = null)
                        if (MaxGas == 0)
                        {
                            var reciept = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(to, amount);
                            tranhash = reciept.TransactionHash;
                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Transfer.ToString(), "To:" + to + ",Eth:" + amount.ToString());
                        }
                        else
                        {
                            //Task<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, BigInteger maxPriorityFee,
                            TransactionReceipt reciept;
                            if (web3.Eth.TransactionManager.UseLegacyAsDefault)     // 这个不支持
                            {
                                //Task<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, BigInteger? nonce = null, CancellationTokenSource tokenSource = null);
                                reciept = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(to, amount); // 某些网络不支持1559，无法设置 maxPriorityFee 这里也不支持 GasPrice 了！
                            }
                            else
                            {
                                // Task<TransactionReceipt> TransferEtherAndWaitForReceiptAsync(string toAddress, decimal etherAmount, BigInteger maxPriorityFee, BigInteger maxFeePerGas, BigInteger? gas = null, BigInteger? nonce = null, CancellationTokenSource tokenSource = null);
                                reciept = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(to, amount, maxPriorityFee, maxFeePerGas, MaxGas);
                            }
                            tranhash = reciept.TransactionHash;
                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Transfer.ToString(), "To:" + to + ",Eth:" + amount.ToString());
                        }
                    }
                }
                else
                {
                    //ERC20 Token转账
                    BigInteger BigAmount = (BigInteger)amount * (BigInteger)Math.Pow(10, (double)ThisTokenDecimals);

                    Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, ThisTokenAddress);
                    if (CheckBoxAllAmount.IsChecked == true)
                    {
                        BigAmount = await service.BalanceOfQueryAsync(from);
                        amount = (decimal)((double)BigAmount / Math.Pow(10, (double)ThisTokenDecimals));
                    }

                    Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction param = new Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction
                    {
                        To = to,
                        Value = BigAmount,
                        MaxPriorityFeePerGas = maxPriorityFee,
                        MaxFeePerGas = maxFeePerGas,
                        Gas = MaxGas,
                    };

                    tranhash = await service.TransferRequestAsync(param);
                    Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Erc20TokenTransfer.ToString(), "To:" + to + ",Token(" + ThisTokenAddress + ")" + amount.ToString());
                }

                string text = LanguageHelper.GetTranslationText(@"交易已提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tranhash);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                log.Error("*", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                ButtonTransfer1.IsEnabled = true;
                this.Cursor = Cursors.Arrow;
            }
        }

        private async void OnSelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                if (ComboBoxAddress1.SelectedValue != null)
                {
                    System.Threading.Thread.Sleep(1);
                    var Address = ComboBoxAddress1.SelectedValue.ToString();
                    await UpdateTokenBalance1(ThisTokenSymbol, Address);

                    var web3 = Share.ShareParam.GetWeb3();
                    await CalMaxEth(Address, web3);

                    //string address = ComboBoxAddress1.SelectedValue.ToString();
                    RefeshGasAndNonce(Address);
                }
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

        private async Task<bool> UpdateTokenBalance1(string TokenSymbol, string Address)
        {
            LabelToken1.Content = TokenSymbol;

            //LabelTokenBalance1.Content = new BLL.Address().GetRealBalance(Address, ThisTokenAddress);
            var TB1 = Task.Run(() => { return new BLL.Address().GetRealBalance(Address, ThisTokenAddress); });
            LabelTokenBalance1.Content = await TB1;


            if (ThisTokenAddress == ShareParam.AddressEth)
            {
                LabelEthBalance1.Content = LabelTokenBalance1.Content;
            }
            else
            {
                //LabelEthBalance1.Content = new BLL.Address().GetRealBalance(Address, ShareParam.AddressEth);
                var TB2 = Task.Run(() => { return new BLL.Address().GetRealBalance(Address, ShareParam.AddressEth); });
                LabelEthBalance1.Content = await TB2;
            }

            return true;
        }

        private async Task<bool> UpdateTokenBalance2(string TokenSymbol, string Address)
        {
            LabelToken2.Content = TokenSymbol;
            //LabelTokenBalance2.Content = new BLL.Address().GetRealBalance(Address, ThisTokenAddress);
            var TB1 = Task.Run(() => { return new BLL.Address().GetRealBalance(Address, ThisTokenAddress); });
            LabelTokenBalance2.Content = await TB1;

            if (ThisTokenAddress == ShareParam.AddressEth)
            {
                LabelEthBalance2.Content = LabelTokenBalance2.Content;
            }
            else
            {
                //LabelEthBalance2.Content = new BLL.Address().GetRealBalance(Address, ShareParam.AddressEth);
                var TB2 = Task.Run(() => { return new BLL.Address().GetRealBalance(Address, ShareParam.AddressEth); });
                LabelEthBalance2.Content = await TB2;
            }
            return true;
        }


        private async void OnSelectionChanged2(object sender, SelectionChangedEventArgs e)
        {

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                if (ComboBoxAddress2.SelectedValue != null)
                {
                    var Address = ComboBoxAddress2.SelectedValue.ToString();
                    await UpdateTokenBalance2(ThisTokenSymbol, Address);
                }
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }

        }

        private void OnLoadAddressAmount2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                TextBoxAddressAmount2.Text = string.Empty;
                string FileName = fileDialog.FileName;
                var lines = System.IO.File.ReadAllText(FileName);
                TextBoxAddressAmount2.Text = lines;// lines.Trim();
            }
        }


        private string GetTokenSymbol(string tokenAddress)              //可以不要，本身有保存！
        {
            var m = Share.BLL.Token.GetModel(tokenAddress);
            if (m != null)
            { return m.Symbol; }

            return tokenAddress;
        }


        /// <summary>
        /// 批量转账，不使用合约
        /// </summary>
        private async void DoBatchTransfer21()
        {
            if (!ButtonBT2.IsEnabled)
            {
                return;
            }
            ButtonBT2.IsEnabled = false;

            var lines = TextBoxAddressAmount2.Text.Split('\n');
            if (lines.Length == 0)
            {
                return;
            }

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            System.Collections.Generic.List<string> l = new List<string>(); //保存处理结果

            try
            {
                var from = ComboBoxAddress2.SelectedValue.ToString();

                var Fee = ShareParam.GetEip1559Fee();

                string password;
                if (!WindowPassword.GetPassword(this, from, LanguageHelper.GetTranslationText("输入密码"), out password))
                {
                    return;
                }
                var account = new BLL.Address().GetAccount(from, password);
                if (null == account)
                {
                    MessageBox.Show(this, LanguageHelper.GetTranslationText("账号不存在，多半是密码错误导致。"));
                    return;
                }
                var web3 = Share.ShareParam.GetWeb3(account);


                foreach (var ToItem in lines)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(ToItem) || string.IsNullOrEmpty(ToItem.Trim()))
                        {
                            continue;
                        }

                        var ts = ToItem.Split(',');
                        var to = ts[0].Trim();
                        var amount = decimal.Parse(ts[1].Trim());
                        if (amount <= 0)
                        {
                            l.Add(@"Error, Amount <= 0, To:" + to);
                            continue;
                        }

                        string tranhash = string.Empty;

                        if (ThisTokenAddress == "ETH" || Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                        {
                            //web3.TransactionManager.UseLegacyAsDefault = true;
                            if (amount <= 0)
                            {
                                l.Add("amount <= 0, from:" + from);
                                continue;
                            }
                            //Task<string> TransferEtherAsync(string toAddress, decimal etherAmount, decimal? gasPriceGwei = null, BigInteger? gas = null, BigInteger? nonce = null);
                            //Task<string> TransferEtherAsync(string toAddress, decimal etherAmount, BigInteger maxPriorityFee, BigInteger maxFeePerGas, BigInteger? gas = null, BigInteger? nonce = null);
                            //tranhash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, amount, (BigInteger)(Fee.MaxPriorityFeePerGas), Fee.MaxFeePerGas,(BigInteger)21000);        //只支持向非合约地址转账，所以写死 21000 ！
                            tranhash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, amount);
                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Transfer.ToString(), "To:" + to + ", Eth:" + amount.ToString());
                        }
                        else
                        {
                            //web3.TransactionManager.UseLegacyAsDefault = false;
                            //ERC20 Token转账
                            amount = amount * (decimal)Math.Pow(10, (double)ThisTokenDecimals);
                            if (amount <= 0)
                            {
                                l.Add("amount <= 0, from: " + from);
                                continue;
                            }

                            Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction param = new Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction
                            {
                                To = to,
                                Value = (BigInteger)amount,
                                MaxFeePerGas = Fee.MaxFeePerGas,
                                MaxPriorityFeePerGas = Fee.MaxPriorityFeePerGas,
                                //Gas = todo,
                            };
                            //var gaslimit = ShareParam.GetGasLimit(from, ThisTokenAddress, param.GetCallData().ToHex());     //没必要， 
                            //param.Gas = gaslimit * 110 / 100;

                            Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, ThisTokenAddress);
                            tranhash = await service.TransferRequestAsync(param);

                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Erc20TokenTransfer.ToString(), "To:" + to + ", Token(" + ThisTokenAddress + ")" + amount.ToString());
                        }

                        string result = "TX:" + tranhash + ", From:" + from + ", To:" + to + ", Token:" + GetTokenSymbol(ThisTokenAddress) + ", Amount(Big):" + amount.ToString();
                        l.Add(result);

                        string url = Share.ShareParam.GetAddressUrl(from);
                        if (!ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                        {
                            url = Share.ShareParam.GetTokenUserUrl(ThisTokenAddress, from);
                        }
                        System.Diagnostics.Process.Start("explorer.exe", url);


                        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
                    }
                    catch (Exception ex)
                    {
                        l.Add(ex.Message);
                    }
                }//end foreach (var ToItem in checkedListBox1.CheckedItems)
            }
            catch (Exception ex)
            {
                l.Add(ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();

                string result = string.Empty;
                foreach (var s in l)
                {
                    result = result + s + Environment.NewLine;
                }

                log.Info("Batch Result：" + Environment.NewLine + result);

                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("点击‘是’把下面结果消息复制到剪贴板：") + Environment.NewLine + result, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Clipboard.SetDataObject(result);
                }
                ButtonBT2.IsEnabled = true;
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// 使用智能合约进行批量转账
        /// </summary>
        private async void DoBatchTransfer22()
        {
            if (!ButtonBT2.IsEnabled)
            {
                return;
            }
            ButtonBT2.IsEnabled = false;

            var lines = TextBoxAddressAmount2.Text.Split('\n');
            if (lines.Length == 0)
            {
                return;
            }

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();

            System.Collections.Generic.List<string> l = new List<string>(); //保存处理结果

            System.Collections.Generic.List<string> tos = new List<string>();
            System.Collections.Generic.List<BigInteger> amounts = new List<BigInteger>();
            BigInteger allamount = 0;

            try
            {
                var from = ComboBoxAddress2.SelectedValue.ToString();

                string password;
                if (!WindowPassword.GetPassword(this, from, LanguageHelper.GetTranslationText("输入密码"), out password))
                {
                    return;
                }
                var account = new BLL.Address().GetAccount(from, password);
                if (null == account)
                {
                    MessageBox.Show(this, LanguageHelper.GetTranslationText("账号不存在，多半是密码错误导致。"));
                    return;
                }
                var web3 = Share.ShareParam.GetWeb3(account);

                try
                {
                    try
                    {
                        foreach (var ToItem in lines)
                        {
                            if (string.IsNullOrEmpty(ToItem) || string.IsNullOrEmpty(ToItem.Trim()))
                            {
                                continue;
                            }

                            string text = ToItem.ToString();
                            var ts = text.Split(',');
                            var to = ts[0].Trim();

                            var amount = (BigInteger)(double.Parse(ts[1].Trim()) * Math.Pow(10, ThisTokenDecimals));
                            if (amount <= 0)
                            {
                                l.Add(@"Break, amount <= 0, To:" + to);
                                return;
                            }
                            tos.Add(to);
                            amounts.Add(amount);
                            allamount = allamount + amount;
                        }
                    }
                    catch (Exception ex)
                    {
                        l.Add(@"Break：" + ex.Message);
                        return;
                    }

                    string tranhash = string.Empty;
                    var flag = Common.Security.Tools.GenRandomInt();
                    Contract.WalletHelper.WalletHelperService s = new Contract.WalletHelper.WalletHelperService(web3, SystemParam.AddressWalletHelper);

                    if (Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                    {
                        //以太币，直接转账
                        Contract.WalletHelper.ContractDefinition.BatchTransfer2Function param = new Contract.WalletHelper.ContractDefinition.BatchTransfer2Function()
                        {
                            Tos = tos,
                            Amounts = amounts,
                            BatchId = flag,                 // BigInteger.Parse(textBoxNonce.Text),
                            IsShowSuccess = true,
                            AmountToSend = allamount,
                        };
                        tranhash = await s.BatchTransfer2RequestAsync(param);
                        Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.TransferByContract.ToString(), LanguageHelper.GetTranslationText("给多个地址以太币") + allamount.ToString());
                    }
                    else
                    {
                        var ApproveAmount1 = allamount;
                        var ApproveAmount2 = BLL.Erc20TokenApprove.GetApprovedAmount(ThisTokenAddress, account.Address, SystemParam.AddressWalletHelper);
                        if (ApproveAmount1 > ApproveAmount2)            //如果授权不够，才执行授权
                        {
                            if (Share.ShareParam.IsErc20ApproveMax)
                            {
                                ApproveAmount1 = await Share.ShareParam.GetTokenTotalSupply(ThisTokenAddress);
                            }

                            //处理授权
                            bool IsOkApprove1 = await ((IMainWindow)(App.Current.MainWindow)).GetHelper().UiErc20TokenApprove(this, account, ThisTokenAddress, SystemParam.AddressWalletHelper, ApproveAmount1);
                            if (!IsOkApprove1)
                            {
                                return;
                            }
                        }

                        //function batchTokenTransfer2(address _erc20Token, address[] calldata _tos, uint256[] calldata _amounts, uint256 _batchId, bool _isShowSuccess) external override  lock  

                        Contract.WalletHelper.ContractDefinition.BatchTokenTransfer2Function param = new Contract.WalletHelper.ContractDefinition.BatchTokenTransfer2Function()
                        {
                            Erc20Token = ThisTokenAddress,
                            Tos = tos,
                            Amounts = amounts,
                            BatchId = flag,
                            IsShowSuccess = true,
                        };
                        tranhash = await s.BatchTokenTransfer2RequestAsync(param);

                        Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Erc20TokenTransfer.ToString(), "Transfer to some Addresses, Token(" + ThisTokenAddress + ")" + allamount.ToString());
                    }

                    string loginfo = "TX:" + tranhash + Environment.NewLine
                                    + "From:" + from + Environment.NewLine
                                    + "AddressAmount:" + Environment.NewLine
                                    + TextBoxAddressAmount2.Text.Trim() + Environment.NewLine
                                    + "Token:" + GetTokenSymbol(ThisTokenAddress) + ", Amount(Big):" + allamount.ToString();
                    l.Add(loginfo);

                    string url = Share.ShareParam.GetTxUrl(tranhash);
                    System.Diagnostics.Process.Start("explorer.exe", url);

                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
                }
                catch (Exception ex)
                {
                    l.Add(ex.Message);
                }
            }
            catch (Exception ex)
            {
                l.Add(ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();

                string result = string.Empty;
                foreach (var s in l)
                {
                    result = result + s + Environment.NewLine;
                }

                log.Info(LanguageHelper.GetTranslationText("批量转账执行结果：") + Environment.NewLine + result);

                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("点击‘是’把下面结果消息复制到剪贴板：") + Environment.NewLine + result, LanguageHelper.GetTranslationText("批量转账结果"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Clipboard.SetDataObject(result);
                }
                ButtonBT2.IsEnabled = true;
                this.Cursor = Cursors.Arrow;
            }

        }

        private void OnBatchTransfer2(object sender, RoutedEventArgs e)
        {
            if (!Share.BLL.Web3Url.Web3IsConnected)
            {
                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("好像没有连接到以太坊节点（Web3），如果继续执行可能会失败。确认要继续执行吗"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }

            if (CheckBoxContract2.IsChecked == true)
            {
                DoBatchTransfer22();
            }
            else
            {
                DoBatchTransfer21();
            }
        }

        private async void OnBatchTransfer3(object sender, RoutedEventArgs e)
        {
            if (!Share.BLL.Web3Url.Web3IsConnected)
            {
                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("好像没有连接到以太坊节点（Web3），如果继续执行可能会失败。确认要继续执行吗"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }

            //使用私钥，多个地址向一个地址转账
            if (!ButtonT3.IsEnabled)
            {
                return;
            }

            //var gasprice = Slider3.Value * Math.Pow(10, 9);
            var to = this.TextBoxTo3.Text.Trim();

            Nethereum.Util.AddressUtil AU = new Nethereum.Util.AddressUtil();
            if (!AU.IsValidEthereumAddressHexFormat(to))
            {
                MessageBox.Show(this, LanguageHelper.GetTranslationText("接收地址输入错误。"), "Message", MessageBoxButton.OK);
                TextBoxTo3.Focus();
                return;
            }

            ButtonT3.IsEnabled = false;

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();

            System.Collections.Generic.List<string> l = new List<string>();

            var lines = TextBoxAddrAmountPrivate.Text.Split('\n');
            if (lines.Length == 0)
            {
                return;
            }

            try
            {
                var Fee = Share.ShareParam.GetEip1559Fee();
                var gasprice = (decimal)(Share.ShareParam.GetDefaultGasPriceAsync() / Math.Pow(10, 9));

                foreach (var FromItem in lines)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(FromItem) || string.IsNullOrEmpty(FromItem.Trim()))
                        {
                            continue;
                        }

                        string text = FromItem.ToString();
                        var ts = text.Split(',');
                        var from = ts[0].Trim();
                        var amount = decimal.Parse(ts[1].Trim());
                        var privateKey = ts[2].Trim();

                        var account = new Nethereum.Web3.Accounts.Account(privateKey);
                        var web3 = Share.ShareParam.GetWeb3(account);

                        string tranhash = string.Empty;

                        if (GetTokenSymbol(ThisTokenAddress).ToUpper() == "ETH" || Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                        {
                            web3.TransactionManager.UseLegacyAsDefault = true;      //无法编译 GetEtherTransferService(to, amount, (BigInteger)Fee.MaxPriorityFeePerGas, Fee.MaxFeePerGas); ，
                            //以太币转账
                            if (amount <= 0)
                            {
                                try
                                {
                                    amount = await web3.Eth.GetEtherTransferService().CalculateTotalAmountToTransferWholeBalanceInEtherAsync(from, gasprice);
                                    //amount = await web3.Eth.GetEtherTransferService().CalculateTotalAmountToTransferWholeBalanceInEtherAsync(from, (BigInteger)Fee.MaxFeePerGas);
                                }
                                catch (Exception ex)
                                {
                                    l.Add("amount is not enough, from:" + from + "," + ex.Message);
                                    continue;
                                }
                            }
                            if (amount <= 0)
                            {
                                l.Add("amount <= 0, from:" + from);
                                continue;
                            }

                            //tranhash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, amount, (BigInteger)Fee.MaxPriorityFeePerGas, Fee.MaxFeePerGas);       //无法编译，只能采用下面做法!
                            tranhash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, amount, gasprice);
                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Transfer.ToString(), "To: " + to + ", Eth:" + amount.ToString());
                        }
                        else
                        {
                            web3.TransactionManager.UseLegacyAsDefault = false;
                            //ERC20 Token转账
                            amount = amount * (decimal)Math.Pow(10, (double)ThisTokenDecimals);
                            Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, ThisTokenAddress);

                            if (amount == 0)
                            {
                                amount = (decimal)(BigInteger)(await service.BalanceOfQueryAsync(from));
                            }
                            if (amount < 0)
                            {
                                l.Add("amount <= 0, from:" + from);
                                continue;
                            }

                            //tranhash = await service.TransferRequestAsync(to, (BigInteger)amount);
                            Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction param = new Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction
                            {
                                To = to,
                                Value = (BigInteger)amount,
                                MaxPriorityFeePerGas = Fee.MaxPriorityFeePerGas,
                                MaxFeePerGas = Fee.MaxFeePerGas,
                            };
                            tranhash = await service.TransferRequestAsync(param);

                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Erc20TokenTransfer.ToString(), "To: " + to + ", Token(" + ThisTokenAddress + ")" + amount.ToString());
                        }
                        string result = "TX:" + tranhash + ", From:" + from + ", To:" + to + ", Token:" + GetTokenSymbol(ThisTokenAddress) + ", Amount:" + amount.ToString();
                        l.Add(result);
                        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
                    }
                    catch (Exception ex)
                    {
                        l.Add(ex.Message);
                    }
                }

                string url = Share.ShareParam.GetAddressUrl(to);
                if (!ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                {
                    url = Share.ShareParam.GetTokenUserUrl(ThisTokenAddress, to);
                }
                System.Diagnostics.Process.Start("explorer.exe", url);

            }
            catch (Exception ex)
            {
                l.Add(ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();

                string result = string.Empty;
                foreach (var s in l)
                {
                    result = result + s + Environment.NewLine;
                }

                log.Info(LanguageHelper.GetTranslationText("批量转账执行结果：") + Environment.NewLine + result);

                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("点击‘是’把下面结果消息复制到剪贴板：") + Environment.NewLine + result, LanguageHelper.GetTranslationText("批量转账结果"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Clipboard.SetDataObject(result);
                }
                ButtonT3.IsEnabled = true;

                this.Cursor = Cursors.Arrow;
            }
        }

        private void OnLoadAddrAmountPrivate3(object sender, RoutedEventArgs e)
        {
            //导入 地址，金额，私钥 的列表
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                TextBoxAddrAmountPrivate.Text = string.Empty;
                string FileName = fileDialog.FileName;

                var lines = System.IO.File.ReadAllText(FileName);
                TextBoxAddrAmountPrivate.Text = lines;
            }
        }

        //public List<Model.ViewAddressEthTokenBalance> ETL4;

        private void OnSelectionChanged4(object sender, SelectionChangedEventArgs e)
        {
            var row = ComboBoxHD4.SelectedItem;
            if (row is System.Data.DataRowView)
            {
                this.Cursor = Cursors.Wait;
                ((IMainWindow)App.Current.MainWindow).ShowProcessing();
                try
                {
                    System.Data.DataRowView r = (System.Data.DataRowView)row;
                    var MneId = (Guid)r["MneId"];
                    ComboBoxHD4.Tag = MneId;

                    //var HasPrivatekey = (bool)r["HasPrivatekey"];
                    //DataGridHDAddress4.IsEnabled = HasPrivatekey;

                    //DataGridHDAddress4.ItemsSource = BLL.Address.GetHDAddressAmount(MneId, TokenAddress).DefaultView;
                    DataGridHDAddress4.ItemsSource = BLL.Address.GetHDAddressAmount(MneId, ThisTokenAddress);
                    //DataGridHDAddress4.ItemsSource = ETL4;

                    DataGridHDAddress4.Columns[4].Header = ThisTokenSymbol + " Balance";

                    CheckBoxSelectAll4.IsChecked = true;
                }
                finally
                {
                    ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                    Cursor = Cursors.Arrow;
                }
            }
        }

        private async void OnBatchTransfer4(object sender, RoutedEventArgs e)
        {
            if (!Share.BLL.Web3Url.Web3IsConnected)
            {
                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("好像没有连接到以太坊节点（Web3），如果继续执行可能会失败。确认要继续执行吗"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
            }

            if (!ButtonT4.IsEnabled)
            {
                return;
            }

            var to = this.TextBoxTo4.Text.Trim();
            Nethereum.Util.AddressUtil AU = new Nethereum.Util.AddressUtil();
            if (!AU.IsValidEthereumAddressHexFormat(to))
            {
                MessageBox.Show(this, LanguageHelper.GetTranslationText("接收地址输入错误。"), "Message", MessageBoxButton.OK);
                TextBoxTo4.Focus();
                return;
            }

            var MneId = (Guid)ComboBoxHD4.Tag;
            var model = BlockChain.Wallet.DAL.HD_Mnemonic.GetModel(Share.ShareParam.DbConStr, MneId);
            if (model == null)
            {
                MessageBox.Show(this, LanguageHelper.GetTranslationText("本地数据没有这样的助记词和密码对应数据。"), "Message", MessageBoxButton.OK);
                return;
            }

            string password;
            if (!WindowPassword.GetPassword(this, model.MneAlias, LanguageHelper.GetTranslationText("输入密码"), out password))
            {
                return;
            }

            ButtonT4.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();

            System.Collections.Generic.List<string> l = new List<string>();

            try
            {
                var UsePassword = Common.PasswordHelper.GetHDRealPassword(model.Salt, password);
                string Mnemonic = Common.Security.SymmetricalEncrypt.Decrypt(UsePassword, model.MneEncrypted);                  //密码错误，这里会抛出异常： Padding is invalid and cannot be removed.
                var words = Mnemonic.Split(' ');//12，15，18，21，24
                if (words.Length == 12 || words.Length == 15 || words.Length == 18 || words.Length == 21 || words.Length == 24)
                {
                    //默认15个！
                }
                else
                {
                    return;
                }

                if (DataGridHDAddress4.ItemsSource == null) { return; }

                //1，生成钱包   
                var ETL4 = (List<Model.ViewAddressEthTokenBalance>)DataGridHDAddress4.ItemsSource;

                var Fee = Share.ShareParam.GetEip1559Fee();
                decimal gasprice = (decimal)(Share.ShareParam.GetDefaultGasPriceAsync() / Math.Pow(10, 9));

                foreach (Model.ViewAddressEthTokenBalance FromItem in ETL4)      //ETL4
                {
                    try
                    {
                        Model.ViewAddressEthTokenBalance vm = (Model.ViewAddressEthTokenBalance)FromItem;

                        var IsSelected = vm.IsSelected;
                        if (!IsSelected)
                        {
                            continue;
                        }
                        int AddressIndex = vm.AddressIndex;
                        string from = vm.Address;
                        var amount = vm.Amount;

                        var ma = BlockChain.Wallet.DAL.HD_Address.GetModel(Share.ShareParam.DbConStr, from);
                        if (ma == null)
                        {
                            l.Add(LanguageHelper.GetTranslationText(@"地址在本地不存在:") + from);
                            continue;
                        }

                        //每一个地址，都重新生成钱包，这种做法最保险，支持两级 salt ！
                        var salt = model.MneFirstSalt + ma.MneSecondSalt;
                        Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(Mnemonic, salt, model.MnePath);

                        var account = w.GetAccount(AddressIndex);
                        if (null == account)
                        {
                            l.Add(LanguageHelper.GetTranslationText(@"地址和本地数据不匹配:") + from);
                            continue;
                        }

                        //验证助记词生成的地址是否和nethereum生成的助记词一样！
                        var a = new Nethereum.Web3.Accounts.Account(account.PrivateKey.HexToByteArray());
                        if (!a.Address.IsTheSameAddress(account.Address))
                        {
                            string err = @"Address Is Error. Mnemonic Account.Address: " + account.Address + "; Validate Account Address: " + a.Address;
                            throw new Exception(err);
                        }

                        if (account.Address.ToLower() != from.ToLower())
                        {
                            l.Add(@"地址和本地数据不匹配:" + from);
                            continue;
                        }

                        var web3 = Share.ShareParam.GetWeb3(account);

                        string tranhash = string.Empty;

                        if (Share.ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                        {
                            web3.TransactionManager.UseLegacyAsDefault = true;

                            if (amount <= 0)
                            {
                                try
                                {
                                    //Task<decimal> CalculateTotalAmountToTransferWholeBalanceInEtherAsync(string address, decimal gasPriceGwei, BigInteger? gas = null);
                                    //Task<decimal> CalculateTotalAmountToTransferWholeBalanceInEtherAsync(string address, BigInteger maxFeePerGas, BigInteger? gas = null);
                                    //amount = await web3.Eth.GetEtherTransferService().CalculateTotalAmountToTransferWholeBalanceInEtherAsync(from,(BigInteger)Fee.MaxFeePerGas);      //無法編譯
                                    amount = await web3.Eth.GetEtherTransferService().CalculateTotalAmountToTransferWholeBalanceInEtherAsync(from, gasprice);
                                }
                                catch (Exception ex)
                                {
                                    l.Add(LanguageHelper.GetTranslationText("不能转账，账户余额不够:") + from + ", " + ex.Message);
                                    continue;
                                }
                            }
                            if (amount < 0)
                            {
                                l.Add("amount <= 0, from:" + from);
                                continue;
                            }
                            tranhash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, amount, gasprice);
                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Transfer.ToString(), "To:" + to + ", Eth:" + amount.ToString());
                        }
                        else
                        {
                            web3.TransactionManager.UseLegacyAsDefault = false;
                            //ERC20 Token转账
                            amount = amount * (decimal)Math.Pow(10, (double)ThisTokenDecimals);
                            Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, ThisTokenAddress);

                            if (amount == 0)
                            {
                                amount = (decimal)(BigInteger)(await service.BalanceOfQueryAsync(from));
                            }
                            if (amount < 0)
                            {
                                l.Add("amount <= 0, from:" + from);
                                continue;
                            }

                            //tranhash = await service.TransferRequestAsync(to, (BigInteger)amount);
                            Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction param = new Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction
                            {
                                To = to,
                                Value = (BigInteger)amount,
                                MaxPriorityFeePerGas = Fee.MaxPriorityFeePerGas,
                                MaxFeePerGas = Fee.MaxFeePerGas,
                                //TransactionType = (byte)2,
                            };
                            tranhash = await service.TransferRequestAsync(param);

                            Share.BLL.TransactionReceipt.LogTx(from, tranhash, TxUserMethod.Erc20TokenTransfer.ToString(), "To: " + to + ", Token(" + ThisTokenAddress + ")" + amount.ToString());
                        }
                        string result = "TX:" + tranhash + ", From:" + from + ", To:" + to + ", Token:" + ThisTokenAddress + ", Amount:" + vm.Amount.ToString();
                        l.Add(result);

                        string url = Share.ShareParam.GetAddressUrl(to);
                        if (!ShareParam.IsAnEmptyAddress(ThisTokenAddress))
                        {
                            url = Share.ShareParam.GetTokenUserUrl(ThisTokenAddress, to);
                        }
                        System.Diagnostics.Process.Start("explorer.exe", url);

                        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
                    }
                    catch (Exception ex)
                    {
                        log.Error("*", ex);

                        l.Add(ex.Message);
                    }
                }//end foreach (var ToItem in checkedListBox1.CheckedItems)
            }
            catch (Exception ex)
            {
                log.Error("*", ex);

                l.Add(ex.Message);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();

                string result = string.Empty;
                foreach (var s in l)
                {
                    result = result + s + Environment.NewLine;
                }

                log.Info(LanguageHelper.GetTranslationText("批量转账执行结果：") + Environment.NewLine + result);

                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("点击‘是’把下面结果消息复制到剪贴板：") + Environment.NewLine + result, LanguageHelper.GetTranslationText("批量转账结果"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Clipboard.SetDataObject(result);
                }

                ButtonT4.IsEnabled = true;
                this.Cursor = Cursors.Arrow;
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void OnRefeshDG4(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //刷新数据库中的余额 
                await BLL.Address.SysAllAddressBalance(false);
                OnSelectionChanged4(null, null);
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

        private void OnSelectAll(object sender, RoutedEventArgs e)
        {
            //if (ETL4 != null)
            SelectAll(true);
        }

        private void OnUnSelectAll(object sender, RoutedEventArgs e)
        {
            //if (ETL4 != null)
            SelectAll(false);
        }

        private void SelectAll(bool IsSelected)
        {
            if (DataGridHDAddress4 != null && DataGridHDAddress4.ItemsSource != null)
            {

                var ETL4 = (List<Model.ViewAddressEthTokenBalance>)DataGridHDAddress4.ItemsSource;
                if (ETL4 == null) { return; }
                foreach (var m in ETL4)
                {
                    m.IsSelected = IsSelected;
                }
                //DataGridHDAddress4.ItemsSource = ETL4;
            }
        }

        private void OnAddress1LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var address = TextBoxToAddress1.Text.Trim();
                var result = BLL.WalletHelper.IsContractAddress(address);           //这个不准确，有的合约可以重复创建和销毁，
                CheckBoxToIsConAddr.IsChecked = result == true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
        }


        //public static void DoTransfer(Window _owner, FromType type, string fromAddress)
        //{
        //    WindowTransfer w = new WindowTransfer();
        //    w.Owner = _owner;
        //    w.ShowDialog();
        //}

        public static void DoTransfer(Window _owner, int pageIndex = 0)
        {
            WindowTransfer w = new WindowTransfer();
            w.Owner = _owner;
            w.TabControlMain.SelectedIndex = pageIndex;
            w.ShowDialog();
        }

        private void OnGotoAddress(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var address = b.Content.ToString();
                System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(address));
            }
        }

        private void RefreshAddressBalance_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var address = (sender as Button).Tag.ToString();
                var token = ThisTokenAddress;

                var model = (Model.ViewAddressEthTokenBalance)DataGridHDAddress4.SelectedItem;

                model.Balance = ShareParam.GetRealBalance(address, token, false);
                if (ShareParam.IsAnEmptyAddress(token))
                {

                    model.EthAmount = (decimal)model.Balance;
                }
                else
                {
                    model.EthAmount = (decimal)ShareParam.GetRealBalance(address, ShareParam.AddressEth, false);
                }

            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }


        //public static void DoTransfer(Window _owner, string to)
        //{
        //    WindowTransfer w = new WindowTransfer();
        //    w.Owner = _owner;
        //    w.TabControlMain.SelectedIndex = 0;
        //    w.TextBoxToAddress1.Text = to;
        //    w.ShowDialog();
        //}


    }
}