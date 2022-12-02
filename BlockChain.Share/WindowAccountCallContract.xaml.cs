using Nethereum.Contracts;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts.Extensions;
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
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowAccountCallContract.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAccountCallContract : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowAccountCallContract()
        {
            InitializeComponent();

            this.Title = Share.LanguageHelper.GetTranslationText(this.Title);

            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private async void ComboBoxAddress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)System.Windows.Application.Current.MainWindow).ShowProcessing();
            try
            {
                var row = ComboBoxAddress.SelectedItem;
                if (row is TxAddress)
                {
                    TxAddress r = (TxAddress)row;
                    //AddressAlias, Address, 'KeyStore' AS SourceName
                    string Address = r.Address;         // r[1].ToString();
                    ComboBoxAddress.Tag = Address;      //保存地址              不需要这种写法， TODO

                    string password;
                    if (Common.PasswordManager.GetPassword(Address, out password))
                    {
                        PasswordBox1.Password = password;
                        CheckBoxRemenberPassword.IsChecked = true;
                    }

                    try
                    {
                        LabelAccountEth.Content = ThisAddressImp.GetRealBalance(Address, ShareParam.AddressEth);
                    }
                    catch (Exception ex)
                    {
                        log.Error("", ex);
                        LabelAccountEth.Content = ex.Message;
                    }

                    //刷新
                    var account = await GetAccount();

                }
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)System.Windows.Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async void OnDoOk(object sender, RoutedEventArgs e)
        {

            if (ThisAccount != null)
            {
                this.DialogResult = true;
            }

            else
            {
                var account = await GetAccount();
                if (ThisAccount != null)
                {
                    this.DialogResult = true;
                }
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnGoToAddress(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var address = b.Content.ToString();
                System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(address));
            }
        }

        private void TextBoxInputData_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBoxInputData.SelectAll();
        }

        private void PasswordBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            //不需要！
            //var account = await GetAccount();
        }

        private async Task<Nethereum.Web3.Accounts.Account?> GetAccount()
        {
            ThisAccount = null;

            if (ComboBoxAddress.Tag == null)
            {
                return null;
            }

            var address = ComboBoxAddress.Tag.ToString();
            var password = PasswordBox1.Password;

            if (string.IsNullOrEmpty(password))
            {
                return null;
            }

            try
            {
                ThisAccount = ThisAddressImp.GetAccount(address, password);
                if (ThisAccount == null)
                {
                    LabelInputPassword.Foreground = Brushes.Red;
                    return null;
                }

                if (CheckBoxRemenberPassword.IsChecked == true)
                {
                    Common.PasswordManager.SavePassword(address, password);
                }

                LabelGasFee.Content = "?";
                LabelGasFeeUsdt.Content = "?";

                try
                {
                    //计算 gas fee           
                    var ThisWeb3 = new Web3(ShareParam.Web3Url);

                    //var handler = ThisWeb3.Eth.GetContractHandler(ContractAddress);
                    //var Gas = GetEstimateGas<>()
                    //var Gas = DoOnEstimateGas(ThisAccount);     // await handler.EstimateGasAsync(CallParam);                    //这里可能也有问题！！！  可能是和 GetCallData 类似问题，其方法是类的扩展方法导致的，而不是子类继承！！！

                    var Gas = await Common.Web3Helper.GetGasLimit(ShareParam.Web3Url, address, ButtonTo.Content.ToString(), TextBoxInputData.Text);
                   LabelEstimateGas.Content = Gas;

                    var FeeSug = ThisWeb3.FeeSuggestion.GetTimePreferenceFeeSuggestionStrategy();
                    var fee = await FeeSug.SuggestFeeAsync();
                    var maxfee = fee.MaxFeePerGas;

                    var txfee = Gas * maxfee;         //得到预估的事务费用！

                    var FeeEth = ((double)txfee / Math.Pow(10, 18));
                    LabelGasFee.Content = ((decimal)FeeEth).ToString() + "(ETH)";

                    var Price = await UniswapTokenPrice.getPrice(Share.ShareParam.AddressEth, ShareParam.AddressPricingToken);
                    var UsdtAmount = Price * FeeEth;
                    LabelGasFeeUsdt.Content = UsdtAmount.ToString("N3") + "(" + BLL.Token.GetModel(ShareParam.AddressPricingToken).Symbol + ")";
                }
                catch (Exception ex)
                {
                    log.Error("", ex); 
                    LabelGasFee.Content = ex.Message;
                }

                return ThisAccount;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                LabelInputPassword.Foreground = Brushes.Red;
                //PasswordBox1.Focus();
                return null;
            }
        }

        private IAddress? ThisAddressImp;
        private string? ContractAddress;
        //private FunctionMessage CallParam;
        private string? InputData;

        private Nethereum.Web3.Accounts.Account? ThisAccount = null;

        private void SetUI()
        {
            ButtonTo.Content = ContractAddress;
            TextBoxInputData.Text = InputData;

            var dv = ThisAddressImp.GetAllTxAddress();
            ComboBoxAddress.ItemsSource = dv;
        }

        //public delegate BigInteger OnEstimateGas(object sender, Nethereum.Web3.Accounts.Account caller, string inputData);

        //public OnEstimateGas? OnEstimateGasCallBack = null;

        //private BigInteger DoOnEstimateGas(Nethereum.Web3.Accounts.Account caller)
        //{
        //    if (OnEstimateGasCallBack != null)
        //    {
        //        return OnEstimateGasCallBack(this, caller, InputData);
        //    }
        //    return -1;
        //}


        //todo: 评估 gas 不需要 私钥 ： https://playground.nethereum.com/csharp/id/1045     暂时不修改，因为私钥在本地，不影响安全性！


        public static Nethereum.Web3.Accounts.Account? GetAccount(Window _owner, IAddress _addressImp, string _contract, string _inputData, bool enforceSavePassword = false)
        {
            WindowAccountCallContract f = new WindowAccountCallContract();
            f.Owner = _owner;
            f.ContractAddress = _contract;
            f.InputData = _inputData;
            f.ThisAddressImp = _addressImp;

            //f.OnEstimateGasCallBack = gasCallBack;          //蹩脚的回调！是因为目前测试 nethereum(4.8) 的泛型 gas 评估有问题！！！ 有可能下个版本 nethereum 会解决这个问题

            f.SetUI();

            if (enforceSavePassword)
            {
                f.CheckBoxRemenberPassword.IsChecked = true;
                f.CheckBoxRemenberPassword.IsEnabled = false;
            }

            if (f.ShowDialog() == true)
            {
                return f.ThisAccount;
            }
            else { return null; }
        }


        #region 以后更新的代码，现在执行不了！ 其他几个地方也要做相应修改！

        //private FunctionMessage Param;

        //public static Nethereum.Web3.Accounts.Account? GetAccount<T>(Window _owner, IAddress _addressImp, string _contract, T _param, bool enforceSavePassword = false) where T : FunctionMessage, new()
        //{
        //    WindowAccountCallContract f = new WindowAccountCallContract();
        //    f.Owner = _owner;
        //    f.ContractAddress = _contract;
        //    f.Param = _param;
        //    f.ThisAddressImp = _addressImp;

        //    f.SetUI();

        //    if (enforceSavePassword)
        //    {
        //        f.CheckBoxRemenberPassword.IsChecked = true;
        //        f.CheckBoxRemenberPassword.IsEnabled = false;
        //    }

        //    if (f.ShowDialog() == true)
        //    {
        //        return f.ThisAccount;
        //    }
        //    else { return null; }

        //}

        ///// 这个代码执行有问题，导致评估 Gas 无法正确！
        ////public string getInputData<T>(T _param) where T : FunctionMessage, new()
        ////{
        ////    return _param.GetCallData().ToHex();
        ////}

        //public BigInteger GetEstimateGas(Nethereum.Web3.Accounts.Account _caller, FunctionMessage _param)
        //{
        //    try
        //    {
        //        var ThisWeb3 = ShareParam.GetWeb3(_caller);
        //        var handler = ThisWeb3.Eth.GetContractHandler(ShareParam.AddressAppInfo);
        //        var Gas = handler.EstimateGasAsync(_param).Result.Value;

        //        return Gas;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("EstimateGas", ex);
        //        return -2;
        //    }
        //}

        #endregion


    }

}
