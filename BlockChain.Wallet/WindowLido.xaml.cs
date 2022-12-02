using BlockChain.Share;
using NBitcoin;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using Windows.ApplicationModel.DataTransfer;

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowLido.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLido : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public WindowLido()
        {
            InitializeComponent();
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            var dv = new BLL.Address().GetAllTxAddress();
            ComboBoxAddress1.SelectedValuePath = "Address";              //user address
            ComboBoxAddress1.ItemsSource = dv;

            ComboBoxAddress3.SelectedValuePath = "Address";              //user address
            ComboBoxAddress3.ItemsSource = dv;
        }

        private void ComboBoxAddress1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxAddress1.SelectedValue != null)
            {
                var UserAddress = ComboBoxAddress1.SelectedValue.ToString();

                LabelUserEthAmount.Content = (decimal)Share.ShareParam.GetRealBalance(UserAddress, Share.ShareParam.AddressEth, false);
            }
        }


        /// <summary>
        /// 质押最大金额，暂时不处理！
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Max_Checked(object sender, RoutedEventArgs e)
        {
            if (LabelUserEthAmount.Content != null)
            {
                ////评估 gas 费用       不处理！
                //var ThisWeb3 = new Web3(ShareParam.Web3Url);
                //var Gas = await Common.Web3Helper.GetGasLimit(ShareParam.Web3Url, ButtonFrom.Content.ToString(), ButtonTo.Content.ToString(), TextBoxInputData.Text);
                //LabelEstimateGas.Content = Gas;
                //var FeeSug = ThisWeb3.FeeSuggestion.GetTimePreferenceFeeSuggestionStrategy();
                //var fee = await FeeSug.SuggestFeeAsync();
                //var maxfee = fee.MaxFeePerGas;
                //var txfee = Gas * maxfee;         //得到预估的事务费用！
                //var FeeEth = ((double)txfee / Math.Pow(10, 18));

                var amount = (decimal)LabelUserEthAmount.Content;       // - FeeEth
                TextBoxStake.Text = amount.ToString();
            }
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                if (ComboBoxAddress1.SelectedValue == null) { return; }
                var UserAddress = ComboBoxAddress1.SelectedValue.ToString();

                var EthAmount = (BigInteger)((decimal)Share.ShareParam.getPowerValue(18) * decimal.Parse(TextBoxStake.Text));

                Lido.Contract.ILido.ContractDefinition.SubmitFunction param = new Lido.Contract.ILido.ContractDefinition.SubmitFunction()
                {
                    Referral = Lido.LidoParam.AddressSubmitReferral,
                    AmountToSend = EthAmount,
                };

                var account = Share.WindowPasswordTransaction.GetAccount(this, new BLL.Address(), UserAddress, Lido.LidoParam.GetAddressLido((int)Share.ShareParam.GetChainId()), param.GetCallData().ToHex());
                if (account == null) { return; }

                var web3 = Share.ShareParam.GetWeb3(account);
                Lido.Contract.ILido.ILidoService service = new Lido.Contract.ILido.ILidoService(web3, Lido.LidoParam.GetAddressLido((int)Share.ShareParam.GetChainId()));

                var tx = await service.SubmitRequestAsync(param);
                Share.BLL.TransactionReceipt.LogTx(UserAddress, tx, "LidoStake(Submit)", "Lido(stETH)_Submit");

                string text = LanguageHelper.GetTranslationText(@"交易已经提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                log.Error("OnSwap", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async void Query_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var web3 = Share.ShareParam.GetWeb3();
                Lido.Contract.ILido.ILidoService service1 = new Lido.Contract.ILido.ILidoService(web3, Lido.LidoParam.GetAddressLido((int)Share.ShareParam.GetChainId()));

                var sl = await service1.GetCurrentStakeLimitQueryAsync();

                LabelLidoCurrentStakeLimit.Content = ((decimal)sl / (decimal)Share.ShareParam.getPowerValue(18)).ToString();
                CheckBoxLidoIsStakingPaused.IsChecked = service1.IsStakingPausedQueryAsync().Result == true;
                CheckBoxLidoIsStakingPaused.Content = CheckBoxLidoIsStakingPaused.IsChecked;

                var UserAddress = TextBoxUserAddress.Text;
                var d1 = await service1.DecimalsQueryAsync();
                var stEthAmount = (decimal)await service1.BalanceOfQueryAsync(UserAddress) / (decimal)Share.ShareParam.getPowerValue(d1);
                TextBoxUser_stETHAmount.Text = stEthAmount.ToString();


                Lido.Contract.IWstETH.IWstETHService service2 = new Lido.Contract.IWstETH.IWstETHService(web3, Lido.LidoParam.GetAddress_wstETH((int)Share.ShareParam.GetChainId()));
                var d2 = await service2.DecimalsQueryAsync();
                var wstEthAmount = (decimal)await service2.BalanceOfQueryAsync(UserAddress) / (decimal)Share.ShareParam.getPowerValue(d2);
                TextBoxUser_wstETHAmount.Text = wstEthAmount.ToString();
            }
            catch (Exception ex)
            {
                log.Error("OnSwap", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void GotoLidoWebSite_Clidk(object sender, RoutedEventArgs e)
        {
            var url = @"https://lido.fi";
            System.Diagnostics.Process.Start("explorer.exe", url);
        }

        private async void ComboBoxAddress3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxAddress3.SelectedValue != null)
            {
                this.Cursor = Cursors.Wait;
                ((IMainWindow)App.Current.MainWindow).ShowProcessing();
                try
                {
                    var UserAddress = ComboBoxAddress3.SelectedValue.ToString();

                    var web3 = Share.ShareParam.GetWeb3();
                    Lido.Contract.ILido.ILidoService service1 = new Lido.Contract.ILido.ILidoService(web3, Lido.LidoParam.GetAddressLido((int)Share.ShareParam.GetChainId()));

                    var d1 = await service1.DecimalsQueryAsync();
                    var stEthAmount = (decimal)await service1.BalanceOfQueryAsync(UserAddress) / (decimal)Share.ShareParam.getPowerValue(d1);
                    Label_stEth.Content = stEthAmount;


                    Lido.Contract.IWstETH.IWstETHService service2 = new Lido.Contract.IWstETH.IWstETHService(web3, Lido.LidoParam.GetAddress_wstETH((int)Share.ShareParam.GetChainId()));
                    var d2 = await service2.DecimalsQueryAsync();
                    var wstEthAmount = (decimal)await service2.BalanceOfQueryAsync(UserAddress) / (decimal)Share.ShareParam.getPowerValue(d2);
                    Label_wstEth.Content = wstEthAmount;

                    //stETH 和 wstETH 的数量转换
                    //uint256 wstETHAmount = stETH.getSharesByPooledEth(_stETHAmount);
                    //uint256 stETHAmount = stETH.getPooledEthByShares(_wstETHAmount);
                    var price_wst =(decimal) (await service1.GetSharesByPooledEthQueryAsync(1 * Share.ShareParam.getPowerValue(d1))) / (decimal)Share.ShareParam.getPowerValue(d2);
                    Label_stEth_wstETH.Content = "1 => " + price_wst.ToString("N9");
                    var price_st = (decimal)(await service1.GetPooledEthBySharesQueryAsync(1 * Share.ShareParam.getPowerValue(d2))) / (decimal)Share.ShareParam.getPowerValue(d1);
                    Label_wstEth_stETH.Content = "1 => " + price_st.ToString("N9");

                }
                catch (Exception ex)
                {
                    log.Error("OnSwap", ex);
                    MessageBox.Show(this, ex.Message);
                }
                finally
                {
                    ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                    Cursor = Cursors.Arrow;
                }
            }
        }

        private async void Wrap_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var UserAddress = ComboBoxAddress3.SelectedValue.ToString();

                var stEthAmount = decimal.Parse(TextBoxInputstEth.Text);
                var w3 = Share.ShareParam.GetWeb3();
                Lido.Contract.ILido.ILidoService service1 = new Lido.Contract.ILido.ILidoService(w3, Lido.LidoParam.GetAddressLido((int)Share.ShareParam.GetChainId()));
                var d1 = await service1.DecimalsQueryAsync();
                var BigstEthAmount = (BigInteger)(stEthAmount * (decimal)Share.ShareParam.getPowerValue(d1));

                string passwrod;
                if (!Share.WindowPassword.GetPassword(this, UserAddress, "Get Password", out passwrod)){ return; }
                var account = new BLL.Address().GetAccount(UserAddress, passwrod);
                if (account == null) { return; }

                //                                                  public async Task<bool> UiErc20TokenApprove(Window _owner, Nethereum.Web3.Accounts.Account _account, string _token,                                                                                                            tring _spender, System.Numerics.BigInteger _bigAmount)
                bool IsOkApprove1 = await ((IMainWindow)App.Current.MainWindow).GetHelper().UiErc20TokenApprove(this,                                           account, Lido.LidoParam.GetAddress_stETH((int)Share.ShareParam.GetChainId()), Lido.LidoParam.GetAddress_wstETH((int)Share.ShareParam.GetChainId()),                       BigstEthAmount);
                if (!IsOkApprove1)
                {
                    MessageBox.Show(this, "Erc20 Token Approve Failed!");
                    return;
                }

                Lido.Contract.IWstETH.ContractDefinition.WrapFunction param1 = new Lido.Contract.IWstETH.ContractDefinition.WrapFunction()
                {
                    StETHAmount = BigstEthAmount,
                };

                //因为前面approve授权，需要密码，已经获得密码了，这里就不用再次获得密码了！ 但也因为这样看不见 inputdata 和 gasfee 了。
                //var account = Share.WindowPasswordTransaction.GetAccount(this, new BLL.Address(), UserAddress, Lido.LidoParam.GetAddressLido((int)Share.ShareParam.GetChainId()), param1.GetCallData().ToHex());
                //if (account == null) { return; }

                var web3 = Share.ShareParam.GetWeb3(account);
                Lido.Contract.IWstETH.IWstETHService service2 = new Lido.Contract.IWstETH.IWstETHService(web3, Lido.LidoParam.GetAddress_wstETH((int)Share.ShareParam.GetChainId()));

                var tx = await service2.WrapRequestAsync(param1);

                Share.BLL.TransactionReceipt.LogTx(UserAddress, tx, "LidoWrap(stETH.wrap)", "stETH wrap to wstEth");

                string text = LanguageHelper.GetTranslationText(@"交易已经提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                log.Error("OnSwap", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async void UnWrap_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var UserAddress = ComboBoxAddress3.SelectedValue.ToString();

                var wstEthAmount = decimal.Parse(TextBoxInputwstEth.Text);
                var w3 = Share.ShareParam.GetWeb3();
                Lido.Contract.IWstETH.IWstETHService service1 = new Lido.Contract.IWstETH.IWstETHService(w3, Lido.LidoParam.GetAddress_wstETH((int)Share.ShareParam.GetChainId()));
                var d2 = await service1.DecimalsQueryAsync();
                var BigwstEthAmount = (BigInteger)(wstEthAmount * (decimal)Share.ShareParam.getPowerValue(d2));

                Lido.Contract.IWstETH.ContractDefinition.UnwrapFunction param2 = new Lido.Contract.IWstETH.ContractDefinition.UnwrapFunction()
                {
                    WstETHAmount = BigwstEthAmount,
                };

                //这个unwrap 不需要 approve 

                var account = Share.WindowPasswordTransaction.GetAccount(this, new BLL.Address(), UserAddress, Lido.LidoParam.GetAddressLido((int)Share.ShareParam.GetChainId()), param2.GetCallData().ToHex());
                if (account == null) { return; }

                var web3 = Share.ShareParam.GetWeb3(account);
                Lido.Contract.IWstETH.IWstETHService service2 = new Lido.Contract.IWstETH.IWstETHService(web3, Lido.LidoParam.GetAddress_wstETH((int)Share.ShareParam.GetChainId()));

                var tx = await service2.UnwrapRequestAsync(param2);

                Share.BLL.TransactionReceipt.LogTx(UserAddress, tx, "LidoWrap(stETH.unwrap)", "wstETH unwrap to stEth");

                string text = LanguageHelper.GetTranslationText(@"交易已经提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }

            }
            catch (Exception ex)
            {
                log.Error("OnSwap", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }





    }

}
