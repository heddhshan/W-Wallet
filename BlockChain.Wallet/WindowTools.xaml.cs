using BlockChain.Share;
using Microsoft.Win32;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
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

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowTools.xaml 的交互逻辑
    /// 本来只是做了离线签名的执行，但增加的小工具越来越多，就变成了百宝箱！
    /// </summary>
    public partial class WindowTools : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowTools()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);
        }


        #region MultiCall

        private List<BLL.MultiCallInputOutput> MultiCallList = new List<BLL.MultiCallInputOutput>();

        private void AddCall_Click(object sender, RoutedEventArgs e)
        {
            DataGridFunction.ItemsSource = null;
            try
            {
                var Call = new BLL.MultiCallInputOutput();
                Call.Id = Guid.NewGuid();
                Call.Target = TextBoxTarget.Text.Trim();
                Call.Value = (BigInteger)((double)decimal.Parse(TextBoxDecimalValue.Text) * Math.Pow(10, 18));
                Call.InputData = TextBoxInput.Text.Trim();
                MultiCallList.Add(Call);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DataGridFunction.ItemsSource = MultiCallList;
            }
        }


        private async void ExeMultiCallList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //// 1, nethereum code 
                //var Handler = new Nethereum.Contracts.QueryHandlers.MultiCall.MultiQueryHandler(web3.Client);
                //var result = await Handler.MultiCallAsync(MultiCallList.ToArray());

                // 2, 使用原生的代码 OK
                List<BlockChain.Share.Contract.Multicall3.ContractDefinition.Call3Value> cl = new List<Share.Contract.Multicall3.ContractDefinition.Call3Value>();
                BigInteger AllValue = 0;
                foreach (var call in MultiCallList)
                {
                    var cv = new BlockChain.Share.Contract.Multicall3.ContractDefinition.Call3Value();
                    cv.AllowFailure = false;
                    cv.Value = call.Value;
                    cv.CallData = call.GetCallData();
                    cv.Target = call.Target;

                    cl.Add(cv);
                    AllValue = AllValue + call.Value;
                }

                BlockChain.Share.Contract.Multicall3.ContractDefinition.Aggregate3ValueFunction param = new Share.Contract.Multicall3.ContractDefinition.Aggregate3ValueFunction()
                {
                    Calls = cl,
                    AmountToSend = AllValue,
                };

                var account = Share.WindowAccount.GetAccount(this, new BLL.Address());
                //var account = Share.WindowAccountCallContract.GetAccount(this, new BLL.Address(), Nethereum.Contracts.Constants.CommonAddresses.MULTICALL_ADDRESS, param.GetCallData().ToHex(true), Aggregate3ValueFunction_GetEstimateGasCallBack);
                if (account == null) { return; }

                var web3 = Share.ShareParam.GetWeb3(account);
                BlockChain.Share.Contract.Multicall3.Multicall3Service service = new Share.Contract.Multicall3.Multicall3Service(web3, Nethereum.Contracts.Constants.CommonAddresses.MULTICALL_ADDRESS);
                var tx = await service.Aggregate3ValueRequestAsync(param);

                Share.BLL.TransactionReceipt.LogTx(account.Address, tx, "Multicall3 - Aggregate3Value", "To:" + Nethereum.Contracts.Constants.CommonAddresses.MULTICALL_ADDRESS + ",BigInteger Eth:" + AllValue.ToString());
                string text = LanguageHelper.GetTranslationText(@"交易已提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
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
        }


        private void DeleteCall_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridFunction.SelectedItem != null)
            {
                if (DataGridFunction.SelectedItem is BLL.MultiCallInputOutput)
                {
                    try
                    {
                        var call = (BLL.MultiCallInputOutput)DataGridFunction.SelectedItem;
                        DataGridFunction.ItemsSource = null;
                        var index = MultiCallList.FindIndex(e => e.Id == call.Id);
                        MultiCallList.RemoveAt(index);
                    }
                    catch (Exception ex)
                    {
                        log.Error("", ex);
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        DataGridFunction.ItemsSource = MultiCallList;
                    }
                }
            }

        }


        //导入导出格式： Target,Value,InputData

        private const string Space = ",";

        private void ImportMultiCallList_Click(object sender, RoutedEventArgs e)
        {
            DataGridFunction.ItemsSource = null;
            MultiCallList.Clear();

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Share.ShareParam.UserDataDir;
                if (ofd.ShowDialog() == true)
                {
                    var CallList = System.IO.File.ReadAllLines(ofd.FileName);
                    foreach (var Call in CallList)
                    {
                        if (string.IsNullOrEmpty(Call)) { continue; }

                        var data = Call.Split(Space);

                        BLL.MultiCallInputOutput c = new BLL.MultiCallInputOutput();
                        c.Id = Guid.NewGuid();
                        c.Target = data[0];
                        c.Value = BigInteger.Parse(data[1]);
                        c.InputData = data[2];

                        MultiCallList.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
            }
            finally
            {
                DataGridFunction.ItemsSource = MultiCallList;
            }
        }
        private void ExportMultiCallList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string result = string.Empty;
                foreach (var call in MultiCallList)
                {
                    result = result + call.Target + Space + call.Value.ToString() + Space + call.InputData + Environment.NewLine;
                }

                string filename = "MultiCall_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                filename = System.IO.Path.Combine(Share.ShareParam.UserDataDir, filename);

                System.IO.File.WriteAllText(filename, result);
                BlockChain.Common.FileHelper.PositionFile(filename);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }


        private void ClearMultiCallList_Click(object sender, RoutedEventArgs e)
        {
            DataGridFunction.ItemsSource = null;
            try
            {
                MultiCallList.Clear();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DataGridFunction.ItemsSource = MultiCallList;
            }
        }
        private void OnGotoAddress(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                if (b.Content != null)
                {
                    var address = b.Content.ToString();
                    System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(address));
                }
            }
        }


        #endregion

        private async void CancleTransaction_Click(object sender, RoutedEventArgs e)
        {
            string tx = TextBoxCancleTransactionHash.Text;
            await Share.WindowTxStatus.UiReplaceTransaction(this, tx);
        }



        public static void ExeTools(Window _owner)
        {
            WindowTools w = new WindowTools();
            w.Owner = _owner;
            w.ShowDialog();
        }



    }

}
