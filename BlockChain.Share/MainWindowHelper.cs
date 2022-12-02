using BlockChain.Share.Model;
using Nethereum.Web3.Accounts;
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
    interface IErc20Token
    {
        Task<bool> UiErc20TokenApprove(Window _owner, Nethereum.Web3.Accounts.Account _account, string _token, string _spender, System.Numerics.BigInteger _bigAmount);
    }

    /// <summary>
    /// Base MainWindow  3#T 和 各个游戏 主窗体的基类，提高的功能包括：统一的风格（最小长宽），ERC20的授权，更新状态栏（区块链信息 和 状态信息）
    /// </summary>
    public class MainWindowHelper : IErc20Token
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Window MainWindow = null;

        //public delegate void SystemLogTx(string from, string txHash, string method, string remark);
        //public static event SystemLogTx OnLogTx;

        public MainWindowHelper(Window mainwin)
        {
            MainWindow = mainwin;
            //MinHeight = "800" MinWidth = "1280"
            // todo : 分辨率要调为1280 * 720，因为14寸的笔记本，默认1920*1080，但是缩放是1.5倍，所以实际长宽就是 1920/1.5=1280，1080/1.5=720 !!!  最好找一台这种电脑试一下(去电脑城看看)。
            MainWindow.MinHeight = 800;
            MainWindow.MinWidth = 1280;
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }


        /// <summary>
        /// 得到授权金额
        /// </summary>
        /// <param name="token"></param>
        /// <param name="owner"></param>
        /// <param name="spender"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetApprovedAmount(string token, string owner, string spender)
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

        // Approve 授权
        //public async Task<bool> UiErc20TokenApprove(Window _owner, Nethereum.Web3.Accounts.Account _account, string _token, string _spender, double _amount)
        public async Task<bool> UiErc20TokenApprove(Window _owner, Nethereum.Web3.Accounts.Account _account, string _token, string _spender, System.Numerics.BigInteger _bigAmount)
        {
            //var OldCursor = MainWindow.Cursor;
            //MainWindow.Cursor = Cursors.Wait;
            try
            {
                var TokenModel = BLL.Token.GetModel(_token);

                var ApprovedAmount = GetApprovedAmount(_token, _account.Address, _spender);
                if (_bigAmount <= ApprovedAmount)
                {
                    Share.BLL.UserTokenApprove.SaveApproveInfo(_account.Address, _token, _spender, TokenModel, (double)ApprovedAmount / Math.Pow(10, TokenModel.Decimals), "?");
                    return true; //授权够
                }

                ////授权不够，才执行授权
                //if (ShareParam.IsErc20ApproveMax)
                //{
                //    _bigAmount = ShareParam.Erc20ApproveMaxValue;
                //}

                //授权必须弹出窗体，明确责任，
                var ApproveAmount = (double)_bigAmount / Math.Pow(10, TokenModel.Decimals);
                string Msg = LanguageHelper.GetTranslationText(@"允许执行 ERC20 Token 的 Approve 操作吗？") + Environment.NewLine
                            + LanguageHelper.GetTranslationText(@"如果不允许，则无法继续下一步操作。") + Environment.NewLine
                            + LanguageHelper.GetTranslationText(@"ERC20Token Address:") + _token + Environment.NewLine
                            + LanguageHelper.GetTranslationText(@"被授权地址(Spender Address)：") + _spender + Environment.NewLine
                            + LanguageHelper.GetTranslationText(@"被授权的金额(Approved Amount)：") + ApproveAmount.ToString() + " .";
                bool IsApproved = MessageBox.Show(_owner, Msg, "ERC20Token Approve", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                if (!IsApproved)
                {
                    return false;
                }

                //aprove  //ERC20 Token转账, 分两步：1， token授权，2，然后调用  //1， token授权
                Nethereum.Web3.Web3 web3 = ShareParam.GetWeb3(_account);

                Nethereum.StandardTokenEIP20.StandardTokenService Erc20Token = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, _token);
                var input = new Nethereum.StandardTokenEIP20.ContractDefinition.ApproveFunction()
                {
                    Spender = _spender,
                    Value = _bigAmount,
                    //GasPrice = Common.Web3Helper.GetNowGasPrice(ShareParam.Web3Url) * 12 / 10,    //增加20%，主要是提高速度，方便下一个操作。
                };
                
                ////写法1
                //var tx = await Erc20Token.ApproveRequestAsync(input);  

                var FullTx = await Erc20Token.ApproveRequestAndWaitForReceiptAsync(input);
                var tx = FullTx.TransactionHash;
                Share.BLL.TransactionReceipt.LogTx(_account.Address, tx, "Erc20Token.Approve", "To:" + _token + ",Spender:" + _spender + ",Value:" + _bigAmount.ToString());
                Share.BLL.UserTokenApprove.SaveApproveInfo(_account.Address, _token, _spender, TokenModel, ApproveAmount, tx);
                return true;

                ////写法1
                ////增加下面这段代码更保险，只有授权成功才能扣钱！但会增加执行时间！
                //int MaxS = 0;                               //最长时间，两分钟
                //while (true)
                //{
                //    var query = new Nethereum.StandardTokenEIP20.ContractDefinition.AllowanceFunction()
                //    {
                //        Owner = _account.Address,
                //        Spender = _spender,
                //    };
                //    var an = await Erc20Token.AllowanceQueryAsync(query);
                //    if (_bigAmount <= an)
                //    {
                //        return true;                            //OK
                //    }
                //    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));      //休息 10 秒， 以太坊 13 秒处理一个 block 。
                //    MaxS++;
                //    if (12 <= MaxS)
                //    {
                //        var IsGo = (MessageBox.Show(_owner, LanguageHelper.GetTranslationText("授权转账（Approve）在两分钟内还未执行成功，是否继续等等？")
                //                                            + Environment.NewLine
                //                                            + LanguageHelper.GetTranslationText("如果不等待，下一步操作可能失败。"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
                //        if (!IsGo)
                //        {
                //            return false;
                //        }
                //        MaxS = 0;
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(_owner, ex.Message);
                return false;
            }
            //finally
            //{
            //    MainWindow.Cursor = OldCursor;
            //}
        }


    }


}
