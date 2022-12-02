using BlockChain.Share;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowMessage.xaml 的交互逻辑
    /// </summary>
    public partial class WindowMessage : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowMessage()
        {
            InitializeComponent();

            ComboBoxFrom.SelectedValuePath = "Address";              //user address
            var dv = new BLL.Address().GetAllTxAddress();
            ComboBoxFrom.ItemsSource = dv;
        }


        /// <summary>
        /// ref http://playground.nethereum.com/csharp/id/1059 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SendOnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                string from = ComboBoxFrom.SelectedValue.ToString();
                string to = TextBoxTo.Text.Trim();                          //如果是合约地址，会执行失败，但仍然会记录到链上，仍然可以看见输入消息
                string message = TextBoxMessage.Text.Trim();
                //double eth = double.Parse(TextBoxETH.Text.Trim());
                //System.Numerics.BigInteger BigEth = (System.Numerics.BigInteger) (eth * Math.Pow(10, 18));
                decimal eth = decimal.Parse(TextBoxETH.Text.Trim());
                System.Numerics.BigInteger gaslimit = System.Numerics.BigInteger.Parse(TextBoxGas.Text.Trim());

                Nethereum.RPC.Eth.DTOs.TransactionInput param = new Nethereum.RPC.Eth.DTOs.TransactionInput()
                {
                    Data = message.ToHexUTF8(),
                    From = from,
                    To = to,
                    Value = new HexBigInteger(UnitConversion.Convert.ToWei(eth)),
                    Gas = new HexBigInteger(gaslimit),                       // require > 21_000
                };

                //string Password;
                //if (!BlockChain.Share.WindowPassword.GetPassword(this, from, "Get Password", out Password)) return;

                //var Account = new BLL.Address().GetAccount(from, Password);
                //if (Account == null) { MessageBox.Show(this, @"No Account"); return; } null, (long)gaslimit

                ///  new code
                var Account = WindowPasswordTransaction.GetAccount(this, new BLL.Address(), from, to, param.Data);

                var web3 = Share.ShareParam.GetWeb3(Account, Share.ShareParam.Web3Url);
                //web3.TransactionManager.UseLegacyAsDefault = true;


                var tx = await web3.TransactionManager.SendTransactionAsync(param);
                    Share.BLL.TransactionReceipt.LogTx(from, tx, TxUserMethod.Transfer_EtherWithData.ToString(), "Transfer_EtherWithData");

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

        //public  BigInteger SendTransaction_OnEstimateGas(object sender, Nethereum.Web3.Accounts.Account caller, string to, string inputData) {
        //    try
        //    {
        //        Contract.GameAuction.ContractDefinition.SetGameBankerFunction param = new Contract.GameAuction.ContractDefinition.SetGameBankerFunction();//1
        //        param.DecodeInput(inputData);

        //        var ThisWeb3 = ShareParam.GetWeb3(caller);
        //        var handler = ThisWeb3.Eth.GetContractHandler(to);     //2
        //        var Gas = handler.EstimateGasAsync(param).Result.Value;

        //        return Gas;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("EstimateGas", ex);
        //        return -2;
        //    }
        //}

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBoxFromOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //do nothing 
        }

    }

}
