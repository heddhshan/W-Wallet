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
using Windows.System;
using static System.Net.Mime.MediaTypeNames;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowErc20TokenApprove.xaml 的交互逻辑
    /// todo: 还需要修改！ 要把 gas 费用 和 inputdata 显示出来！
    /// </summary>
    public partial class WindowErc20TokenApprove : Window
    {
        // 暂缓处理，暂时不需要！ 使用原来的代码就行了！


        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowErc20TokenApprove()
        {
            InitializeComponent();
        }

        private void Onwer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                string assress = (sender as Button).Content.ToString();
                System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(assress));
            }
        }

        private void Token_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                var TokenModel = (Share.Model.Token)(ButtonERC20Token.DataContext);
                string assress = TokenModel.Address;
                System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(assress));
            }
        }

        private void Spender_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                string assress = (sender as Button).Content.ToString();
                System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(assress));
            }
        }

        private async void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                var r = sender as RadioButton;
                var index = int.Parse(r.Tag.ToString());
                if (index == 1)
                {
                    TextBoxApproveAmount.Text = TextBoxTransferAmount.Text;
                }
                else if (index == 2)
                {
                    var TokenModel = (Share.Model.Token)(ButtonERC20Token.DataContext);
                    if (TokenModel != null)
                    {                 
                        string token = TokenModel.Address;
                        TextBoxApproveAmount.Text =( await ShareParam.GetTokenTotalSupply(token)).ToString();
                    }
                }
                else if (index == 3)
                {
                    TextBoxApproveAmount.Text = "";
                    TextBoxApproveAmount.Focus();
                }
                TextBoxApproveAmount.IsEnabled = index != 3;
            }
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)System.Windows.Application.Current.MainWindow).ShowProcessing();
            try
            {
                Nethereum.Web3.Web3 web3 = ShareParam.GetWeb3(ThisAccount);

                string spender = ButtonSpender.Content.ToString();
                BigInteger BigAmount = BigInteger.Parse(TextBoxApproveAmount.Text);
                var TokenModel = (Share.Model.Token)(ButtonERC20Token.DataContext);

                Nethereum.StandardTokenEIP20.StandardTokenService Erc20Token = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, ThisToken);
                var input = new Nethereum.StandardTokenEIP20.ContractDefinition.ApproveFunction()
                {
                    Spender = spender,
                    Value = BigAmount,
                    //GasPrice = Common.Web3Helper.GetNowGasPrice(ShareParam.Web3Url) * 12 / 10,    //增加20%，主要是提高速度，方便下一个操作。
                };
                //Erc20Token.ApproveRequestAsync("");
                var tx = await Erc20Token.ApproveRequestAsync(input);   //ok，有时候会出错
                Share.BLL.TransactionReceipt.LogTx(ThisAccount.Address, tx, "Erc20Token.Approve", "To:" + ThisToken + ",Spender:" + spender + ",Value:" + BigAmount.ToString());

                var ApproveAmount = (double)BigAmount / Math.Pow(10, TokenModel.Decimals);
                Share.BLL.UserTokenApprove.SaveApproveInfo(ThisAccount.Address, ThisToken, spender, TokenModel, ApproveAmount, tx);

                if (CheckBoxGotoTxUrl.IsChecked == true) {
                    //string text = LanguageHelper.GetTranslationText(@"交易已提交到以太坊网络，点击‘确定’查看详情。");
                    string url = Share.ShareParam.GetTxUrl(tx);
                    System.Diagnostics.Process.Start("explorer.exe", url);                   
                }

                System.Threading.Thread.Sleep(1);

                //增加下面这段代码更保险，只有授权成功才能扣钱！但会增加执行时间！
                //int MaxS = 0;                               //最长时间，两分钟

                while (true)
                {
                    var query = new Nethereum.StandardTokenEIP20.ContractDefinition.AllowanceFunction()
                    {
                        Owner = ThisAccount.Address,
                        Spender = spender,
                    };
                    var an = await Erc20Token.AllowanceQueryAsync(query);
                    if (BigAmount <= an)
                    {
                        this.DialogResult = true;
                        return;                            //OK
                    }

                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));      //休息 10 秒， 以太坊 13 秒处理一个 block 。

                    //MaxS++;

                    //if (12 <= MaxS)
                    //{
                    //    var IsGo = (MessageBox.Show(this, LanguageHelper.GetTranslationText("授权转账（Approve）在两分钟内还未执行成功，是否继续等等？")
                    //                                        + Environment.NewLine
                    //                                        + LanguageHelper.GetTranslationText("如果不等待，下一步操作可能失败。"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
                    //    if (!IsGo)
                    //    {
                    //        this.DialogResult = false;
                    //        return;
                    //    }
                    //    MaxS = 0;
                    //}
                }

                throw new Exception("???");
                //this.DialogResult = true;

            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                ((IMainWindow)System.Windows.Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private Nethereum.Web3.Accounts.Account ThisAccount;
        private String ThisToken;

        private void SetUi(Nethereum.Web3.Accounts.Account _account, string _token, string _spender, System.Numerics.BigInteger _bigAmount)
        {
            ThisAccount = _account;
            ThisToken = _token;

            ButtonOwner.Content = _account.Address;

            var TokenModel = BLL.Token.GetModel(_token);
            ButtonERC20Token.DataContext = TokenModel;

            ButtonSpender.Content = _spender;
            TextBoxTransferAmount.Text = _bigAmount.ToString();

            var ApproveAmount = (double)_bigAmount / Math.Pow(10, TokenModel.Decimals);
            LabelTransferAmount.Content = (decimal)ApproveAmount;

            ////RadioButton_Checked()

        }


        private static bool ShowErc20TokenApprove(Window _owner, Nethereum.Web3.Accounts.Account _account, string _token, string _spender, System.Numerics.BigInteger _bigAmount)
        {
            WindowErc20TokenApprove f = new WindowErc20TokenApprove();
            f.Owner = _owner;
            f.SetUi(_account, _token, _spender, _bigAmount);

            return f.ShowDialog() == true;
        }


        /// <summary>
        /// 得到授权金额
        /// </summary>
        /// <param name="token"></param>
        /// <param name="owner"></param>
        /// <param name="spender"></param>
        /// <returns></returns>
        private static System.Numerics.BigInteger GetApprovedAmount(string token, string owner, string spender)
        {
            Nethereum.Web3.Web3 web3 = ShareParam.GetWeb3();
            Nethereum.StandardTokenEIP20.StandardTokenService Erc20Token = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, token);

            var query = new Nethereum.StandardTokenEIP20.ContractDefinition.AllowanceFunction()
            {
                Owner = owner,
                Spender = spender,
            };
            var result = Erc20Token.AllowanceQueryAsync(query).Result;
            return result;
        }

        //// Approve 授权
        //public async Task<bool> UiErc20TokenApprove(Window _owner, Nethereum.Web3.Accounts.Account _account, string _token, string _spender, System.Numerics.BigInteger _bigAmount)
        //{
        //    //var OldCursor = MainWindow.Cursor;
        //    //MainWindow.Cursor = Cursors.Wait;
        //    try
        //    {
        //        var TokenModel = BLL.Token.GetModel(_token);

        //        var ApprovedAmount = GetApprovedAmount(_token, _account.Address, _spender);
        //        if (_bigAmount < ApprovedAmount)
        //        {
        //            Share.BLL.UserTokenApprove.SaveApproveInfo(_account.Address, _token, _spender, TokenModel, (double)ApprovedAmount / Math.Pow(10, TokenModel.Decimals), "?");
        //            return true; //授权够
        //        }

        //        ////授权不够，才执行授权
        //        //if (ShareParam.IsErc20ApproveMax)
        //        //{
        //        //    _bigAmount = ShareParam.Erc20ApproveMaxValue;
        //        //}

        //        //授权必须弹出窗体，明确责任，
        //        //var ApproveAmount = (double)_bigAmount / Math.Pow(10, TokenModel.Decimals);
        //        //string Msg = LanguageHelper.GetTranslationText(@"允许执行 ERC20 Token 的 Approve 操作吗？") + Environment.NewLine
        //        //            + LanguageHelper.GetTranslationText(@"如果不允许，则无法继续下一步操作。") + Environment.NewLine
        //        //            + LanguageHelper.GetTranslationText(@"ERC20Token Address:") + _token + Environment.NewLine
        //        //            + LanguageHelper.GetTranslationText(@"被授权地址(Spender Address)：") + _spender + Environment.NewLine
        //        //            + LanguageHelper.GetTranslationText(@"被授权的金额(Approved Amount)：") + ApproveAmount.ToString() + " .";
        //        //bool IsApproved = MessageBox.Show(_owner, Msg, "ERC20Token Approve", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        //        bool IsApproved = ShowErc20TokenApprove(_owner, _account, _token, _spender, _bigAmount);
        //        if (!IsApproved)
        //        {
        //            return false;
        //        }

        //        return true;

        //        //aprove  //ERC20 Token转账, 分两步：1， token授权，2，然后调用  //1， token授权
        //        //Nethereum.Web3.Web3 web3 = ShareParam.GetWeb3(_account);

        //        //Nethereum.StandardTokenEIP20.StandardTokenService Erc20Token = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, _token);
        //        //var input = new Nethereum.StandardTokenEIP20.ContractDefinition.ApproveFunction()
        //        //{
        //        //    Spender = _spender,
        //        //    Value = _bigAmount,
        //        //    //GasPrice = Common.Web3Helper.GetNowGasPrice(ShareParam.Web3Url) * 12 / 10,    //增加20%，主要是提高速度，方便下一个操作。
        //        //};
        //        ////Erc20Token.ApproveRequestAsync("");
        //        //var tx = await Erc20Token.ApproveRequestAsync(input);   //ok，有时候会出错
        //        //Share.BLL.TransactionReceipt.LogTx(_account.Address, tx, "Erc20Token.Approve", "To:" + _token + ",Spender:" + _spender + ",Value:" + _bigAmount.ToString());

        //        //Share.BLL.UserTokenApprove.SaveApproveInfo(_account.Address, _token, _spender, TokenModel, ApproveAmount, tx);

        //        //////使用事件执行日志记录，记录到数据库
        //        ////OnLogTx?. Invoke(_account.Address, tx, "Erc20TokenApprove", "Token:" + _token + ",Spender:" + _spender + ",Value:" + _bigAmount.ToString());

        //        //System.Threading.Thread.Sleep(1);

        //        ////增加下面这段代码更保险，只有授权成功才能扣钱！但会增加执行时间！
        //        //int MaxS = 0;                               //最长时间，两分钟

        //        //while (true)
        //        //{
        //        //    var query = new Nethereum.StandardTokenEIP20.ContractDefinition.AllowanceFunction()
        //        //    {
        //        //        Owner = _account.Address,
        //        //        Spender = _spender,
        //        //    };
        //        //    var an = await Erc20Token.AllowanceQueryAsync(query);
        //        //    if (_bigAmount <= an)
        //        //    {
        //        //        return true;                            //OK
        //        //    }

        //        //    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));      //休息 10 秒， 以太坊 13 秒处理一个 block 。
        //        //    MaxS++;

        //        //    if (12 <= MaxS)
        //        //    {
        //        //        var IsGo = (MessageBox.Show(_owner, LanguageHelper.GetTranslationText("授权转账（Approve）在两分钟内还未执行成功，是否继续等等？")
        //        //                                            + Environment.NewLine
        //        //                                            + LanguageHelper.GetTranslationText("如果不等待，下一步操作可能失败。"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        //        //        if (!IsGo)
        //        //        {
        //        //            return false;
        //        //        }
        //        //        MaxS = 0;
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("", ex); MessageBox.Show(_owner, ex.Message);
        //        return false;
        //    }
        //    //finally
        //    //{
        //    //    MainWindow.Cursor = OldCursor;
        //    //}
        //}



    }
}
