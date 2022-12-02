using BlockChain.Share.BLL;
using Nethereum.Contracts;
using Nethereum.Web3;
using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowPasswordTransaction.xaml 的交互逻辑  此 Password 用于执行事务的时候调用！！！
    /// </summary>
    public partial class WindowPasswordTransaction : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public WindowPasswordTransaction()
        {
            InitializeComponent();
            this.Title = Share.LanguageHelper.GetTranslationText(this.Title);

            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
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

        private async void PasswordBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            var account = await GetAccount();
        }

        private async Task<Nethereum.Web3.Accounts.Account?> GetAccount()
        {
            ThisAccount = null;

            var password = PasswordBox1.Password;

            if (string.IsNullOrEmpty(password))
            {
                return null;
            }

            try
            {
                ThisAccount = ThisAddressImp.GetAccount(Caller, password);
                if (ThisAccount == null)
                {
                    LabelInputPassword.Foreground = Brushes.Red;
                    return null;
                }

                if (CheckBoxRemenberPassword.IsChecked == true)
                {
                    Common.PasswordManager.SavePassword(Caller, password);
                }

                try
                {
                    //计算 gas fee           
                    var ThisWeb3 = new Web3(ShareParam.Web3Url);

                    var Gas = await Common.Web3Helper.GetGasLimit(ShareParam.Web3Url, ButtonFrom.Content.ToString(), ButtonTo.Content.ToString(), TextBoxInputData.Text);
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

                try
                {
                    LabelAccountEth.Content = ThisAddressImp.GetRealBalance(Caller, ShareParam.AddressEth);
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    LabelAccountEth.Content = ex.Message;
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
        private string? Caller;
        private string? To;
        private string? InputData;
        //private long? GasLimit = 0;

        private Nethereum.Web3.Accounts.Account? ThisAccount = null;

        private void SetUI()
        {
            ButtonTo.Content = To;
            TextBoxInputData.Text = InputData;
            ButtonFrom.Content = Caller;
        }

        #region call back 

        //public delegate BigInteger OnEstimateGas(object sender, Nethereum.Web3.Accounts.Account caller, string to, string inputData);


        //public OnEstimateGas? OnEstimateGasCallBack = null;

        //private BigInteger DoOnEstimateGas(Nethereum.Web3.Accounts.Account caller)
        //{
        //    if (GasLimit > 0)
        //    {
        //        return (BigInteger)GasLimit;
        //    }

        //    if (OnEstimateGasCallBack != null)
        //    {
        //        return OnEstimateGasCallBack(this, caller, To, InputData);
        //    }
        //    return -1;
        //}

        //public static bool GetPassword(Window _owner, string AddressOrHd, string title, out string password, bool enforceSavePassword = false, string tip = "")
        public static Nethereum.Web3.Accounts.Account? GetAccount(Window _owner, IAddress _addressImp, string _caller, string _to, string _inputData, bool _enforceSavePassword = false)
        {
            WindowPasswordTransaction f = new WindowPasswordTransaction();
            f.Owner = _owner;
            f.To = _to;
            f.InputData = _inputData;
            f.Caller = _caller;
            f.ThisAddressImp = _addressImp;

            //if (_gasLimit > 0)
            //{
            //    f.GasLimit = _gasLimit;
            //}

            //f.OnEstimateGasCallBack = _gasCallBack;

            f.SetUI();

            if (_enforceSavePassword)
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

        #endregion


        #region 以后更新的代码，现在执行不了！ 其他几个地方也要做相应修改！

        //private FunctionMessage Param;

        //public static Nethereum.Web3.Accounts.Account? GetAccount<T>(Window _owner, IAddress _addressImp, string _contract, T _param, bool enforceSavePassword = false) where T : FunctionMessage, new()
        //{
        //    WindowPasswordTransaction f = new WindowPasswordTransaction();
        //    f.Owner = _owner;
        //    f.To = _contract;
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
