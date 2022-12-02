using BlockChain.Share;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
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
using Windows.ApplicationModel.Contacts;
using static BlockChain.Share.WindowAccountCallContract;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowNotice.xaml 的交互逻辑
    /// </summary>
    public partial class WindowNotice : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private long ThisNoticeId = 0;
        private long ThisAppId = 0;


        public WindowNotice()
        {
            InitializeComponent();

            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private async void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGridNotices.ItemsSource = Share.BLL.AppInfo.GetAllNotices(ThisAppId);

                var web3 = ShareParam.GetWeb3();
                Contract.AppInfo.AppInfoService service = new Contract.AppInfo.AppInfoService(web3, ShareParam.AddressAppInfo);
                var admin = await service.AdminQueryAsync();
                LabelAdmin.Content = admin;
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
            }
        }

        private async void OnClickPublish(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            try
            {
                string subject = TextBoxSubject.Text.Trim();
                string body = TextBoxBody.Text.Trim();

                Contract.AppInfo.ContractDefinition.PublishNoticeFunction param = new Contract.AppInfo.ContractDefinition.PublishNoticeFunction()
                {
                    AppId = ThisAppId,
                    Subject = subject,
                    Body = body,  
                     ////GasPrice = 0,      //for test
                     //// Nonce= 0,
                     ////  MaxFeePerGas = 0                     
                };

                var account = WindowAccountCallContract.GetAccount(this, ThisImpAddress, ShareParam.AddressAppInfo, param.GetCallData().ToHex(true));
                //var account = WindowAccountCallContract.GetAccount(this, new Share.BLL.Address(), ShareParam.AddressAppInfo, param.GetCallData().ToHex(true));

                if (account == null)
                {
                    return;
                }

                var web3 = ShareParam.GetWeb3(account);
                Contract.AppInfo.AppInfoService service = new Contract.AppInfo.AppInfoService(web3, ShareParam.AddressAppInfo);
                //var tx = await service.PublishNoticeRequestAsync(  BlockChainAppId.ProjectId, ThisNoticeId, subject, body);       
                var tx = await service.PublishNoticeRequestAsync(param);

                //account.NonceService.GetNextNonceAsync(); //for test  get Nonce

                Share.BLL.TransactionReceipt.LogTx(account.Address, tx, TxUserMethod.ToolsPublishNotice.ToString(), "Tools PublishNotice ");

                string text = LanguageHelper.GetTranslationText(@"交易已提交到以太坊网络，点击‘确定’查看详情。");
                string url = ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }

        }


        //public BigInteger PublishNoticeFunction_GetEstimateGasCallBack(object sender, Nethereum.Web3.Accounts.Account caller, string inputData) 
        //{
        //    try
        //    {
        //        Contract.AppInfo.ContractDefinition.PublishNoticeFunction param = new Contract.AppInfo.ContractDefinition.PublishNoticeFunction();
        //        param.DecodeInput(inputData);

        //        var ThisWeb3 = ShareParam.GetWeb3(caller);
        //        var handler = ThisWeb3.Eth.GetContractHandler(ShareParam.AddressAppInfo);
        //        var Gas = handler.EstimateGasAsync(param).Result.Value;

        //        return Gas;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("EstimateGas", ex);
        //        return -2;
        //    }
        //}


        private async void OnClickRefresh(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            try
            {
                await Share.BLL.AppInfo.SynOnPublishNotice();
                DataGridNotices.ItemsSource = Share.BLL.AppInfo.GetAllNotices(ThisAppId);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void OnClickBlockNumber(object sender, RoutedEventArgs e)
        {
            var hl = sender as Hyperlink;
            var tx = hl.Tag.ToString();

            System.Diagnostics.Process.Start("explorer.exe", ShareParam.GetTxUrl(tx));
        }

        private Share.IAddress ThisImpAddress = null;

        public static void ShowWindowNotice(Window _owner, long _appId, Share.IAddress _ImpAddress)
        {
            WindowNotice w = new WindowNotice();
            w.ThisImpAddress = _ImpAddress;
            w.ThisAppId = _appId;
            w.Owner = _owner;
            w.ShowDialog();
        }

    }
}
