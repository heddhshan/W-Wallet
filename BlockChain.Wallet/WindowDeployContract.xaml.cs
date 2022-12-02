using BlockChain.Share;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;
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

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowDeployContract.xaml 的交互逻辑
    /// </summary>
    public partial class WindowDeployContract : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowDeployContract()
        {
            InitializeComponent();

            LabelChainId.Content = Share.ShareParam.GetChainId();

            //Share.ShareParam.AddressAppInfo = "0x06224502E0e2F98A4624E58d3856bD9660581CfB";
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private async void OnClickD(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var amount = Common.SolidityHelper.Double2BigInteger(double.Parse(TexBokEthAmount.Text.Trim()) * Math.Pow(10, 18));
                Contract.WalletHelper.ContractDefinition.DeployContractFunction param = new Contract.WalletHelper.ContractDefinition.DeployContractFunction()
                {
                    AmountToSend = amount,
                    Bytecode = TexBokBytecode.Text.HexToByteArray(),
                    Salt = TexBokSalt.Text.HexToByteArray(),                   
                };

                //var account = Share.WindowAccount.GetAccount(this, new BLL.Address());
                //public static Nethereum.Web3.Accounts.Account? GetAccount(Window _owner, IAddress _addressImp, string _contract, string _inputData, OnEstimateGas gasCallBack, bool enforceSavePassword = false)
                var account = WindowAccountCallContract.GetAccount(this, new BLL.Address(), SystemParam.AddressWalletHelper, param.GetCallData().ToHex(true));

                if (null == account) return;

                Nethereum.Web3.Web3 w3 = Share.ShareParam.GetWeb3(account);
                Contract.WalletHelper.WalletHelperService Service = new Contract.WalletHelper.WalletHelperService(w3, SystemParam.AddressWalletHelper);
                
                //var tranhash = await Service.DeployContractRequestAsync(param);
                var receipt = await Service.DeployContractRequestAndWaitForReceiptAsync(param);
                var tranhash = receipt.TransactionHash;
                Share.BLL.TransactionReceipt.LogTx(account.Address, tranhash, TxUserMethod.DeployContract.ToString(), "DeployContract");

                string text = LanguageHelper.GetTranslationText(@"交易已提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tranhash);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }

                //Service.DeployContractRequestAndWaitForReceiptAsync

                Model.WalletHelper_OnDeployContract_Local model = new Model.WalletHelper_OnDeployContract_Local();
                model.ChainId = (int)Share.ShareParam.GetChainId();
                model.ContractAddress = SystemParam.AddressWalletHelper;
                model.LocalRemark = this.TexBokRemark.Text.Trim();
                model.TransactionHash = tranhash;
                model._bytecode = TexBokBytecode.Text;

                try
                {
                    DAL.WalletHelper_OnDeployContract_Local.Insert(Share. ShareParam.DbConStr, model);
                }
                catch (Exception)
                {
                    log.Error("DAL.WalletHelper_OnDeployContract_Local.Insert Error" + Environment.NewLine + Common.Security.Tools.CreateXML(model));
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



        ////public BigInteger DeployContractFunction_GetEstimateGasCallBack(object sender, Nethereum.Web3.Accounts.Account caller, string inputData)
        ////{
        ////    try
        ////    {
        ////        Contract.WalletHelper.ContractDefinition.DeployContractFunction param = new Contract.WalletHelper.ContractDefinition.DeployContractFunction();      //1
        ////        param.DecodeInput(inputData);

        ////        var ThisWeb3 = ShareParam.GetWeb3(caller);
        ////        var handler = ThisWeb3.Eth.GetContractHandler(SystemParam.AddressWalletHelper);     //2
        ////        var Gas = handler.EstimateGasAsync(param).Result.Value;

        ////        return Gas;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        log.Error("EstimateGas", ex);
        ////        return -2;
        ////    }
        ////}


        private async void OnClickQ(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                await  BLL.WalletHelper.SynOnDeployContract();
                DataGridDeployContract.ItemsSource = BLL.WalletHelper.GetOnDeployContract(TextBoxUser.Text.Trim()).DefaultView;
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

        private void OnGotoAdUrl(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var address = b.Content.ToString();
                System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(address));
            }
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
    }

}
