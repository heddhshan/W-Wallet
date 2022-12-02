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
using static BlockChain.Share.BLL.Address;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowAddressList.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAddressList : Window
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public WindowAddressList()
        {
            InitializeComponent();
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            QueryAddress();
        }


        private void RefreshOnClick(object sender, RoutedEventArgs e)
        {
            QueryAddress();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (OneInstance != null) { OneInstance = null; }
        }

        private void OnLoadPrivateKey11(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                var m = (MenuItem)sender;
                if (m.Parent is MenuItem)
                {
                    var p = (MenuItem)m.Parent;
                    string address = p.Tag.ToString();
                    WindowExportPrivateKey.ExportPrivateKey(this, address);                    //导出私钥不需要单独一个窗体，直接输入密码就好，地址可以取到。
                }

            }
        }

        private void OnShouQRcode(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                var m = (MenuItem)sender;
                if (m.Parent is MenuItem)
                {
                    var p = (MenuItem)m.Parent;
                    string address = p.Tag.ToString();
                    Share.WindowQRcode.ShowQRcode(this, address);
                }
            }
        }

        private void OnDeleteAddress(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                try
                {
                    var m = (MenuItem)sender;
                    if (m.Parent is MenuItem)
                    {
                        var p = (MenuItem)m.Parent;
                        string address = p.Tag.ToString();
                        var Source = m.Tag.ToString();

                        #region 验证密码

                        try
                        {
                            //var password = WindowPassword.GetPassword("请输入密码");
                            string password;
                            if (!Share.WindowPassword.GetPassword(this, address, Share.LanguageHelper.GetTranslationText("请输入密码"), out password))
                            {
                                return;
                            }

                            var account = new BLL.Address().GetAccount(address, password);
                            if (null == account)
                            {
                                MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("账号不存在，多半是密码错误导致。"));
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, LanguageHelper.GetTranslationText("输入密码错误，删除失败。异常信息是：") + ex.Message, "Exception", MessageBoxButton.OK);
                            return;
                        }

                        #endregion

                        if (MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("确认要删除地址该吗？"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (Source == "KeyStore")
                            {
                                DAL.KeyStore_Address.Delete(Share.ShareParam.DbConStr, address);
                            }
                            else
                            {
                                DAL.KeyStore_Address.Delete(Share.ShareParam.DbConStr, address);
                            }
                            //BLL.Address.DeleteAllBalance(address);

                            //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                            QueryAddress();       

                            DoExeCallBack(address);

                            //await ShowAddressBalance(0, PagerAddressToken.CurrentPageRecNum);     // no  在另外页面呢 
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Message", MessageBoxButton.OK);
                }
            }


        }


        private bool QueryAddress()
        {
            //throw new NotImplementedException();

            this.Cursor = Cursors.Wait;
            ((Share.IMainWindow)(Application.Current.MainWindow)).ShowProcessing();
            try
            {
                // var T2 = Task.Run(() =>
                // {
                //     return BLL.Address.GetRealAllAddressLisr();
                // });
                //var addresslist = await T2;
                // DataGridAddress.ItemsSource = addresslist;

                DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddressList();

                return true;

            }
            catch (Exception ex)
            {
                log.Error("*", ex);
                MessageBox.Show(this, ex.Message);
                return false;
            }
            finally
            {
                ((Share.IMainWindow)(Application.Current.MainWindow)).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }


        private void OnModifyAddressAlias(object sender, RoutedEventArgs e)
        {
            var item = DataGridAddress.SelectedItem;
            if (item is Share.TxAddress)
            {
                var drv = (Share.TxAddress)item;

                string address = drv.Address;
                string alias = drv.AddressAlias;

                string newalias = Share.WindowModifyAddressAlias.GetUpdateAlias(address, alias, this);

                if (newalias != alias)
                {
                    BLL.Address.UpdateAddressAlias(address, newalias);

                    QueryAddress();

                    DoExeCallBack(address);
                }
            }
        }

        private void OnSeeTxs(object sender, RoutedEventArgs e)
        {
            var item = DataGridAddress.SelectedItem;
            if (item is Share.TxAddress)
            {
                var drv = (Share.TxAddress)item;

                string address = drv.Address;
                string url = Share.ShareParam.GetAddressUrl(address);
                System.Diagnostics.Process.Start("explorer.exe", url);
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

        private void OnCopyAddress(object sender, RoutedEventArgs e)
        {
            var item = DataGridAddress.SelectedItem;
            if (item is Share.TxAddress)
            {
                try
                {
                    var drv = (Share.TxAddress)item;
                    string address = drv.Address;

                    Clipboard.SetText(address);


                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                }
            }
        }

        private void OnExKeyStore(object sender, RoutedEventArgs e)
        {
            var item = DataGridAddress.SelectedItem;
            if (item is Share.TxAddress)
            {
                try
                {
                    var drv = (Share.TxAddress)item;
                    string address = drv.Address;

                    //var password = WindowPassword.GetPassword(address);
                    string password;
                    if (!Share.WindowPassword.GetPassword(this, address, "", out password))
                    {
                        return;
                    }

                    var account = new BLL.Address().GetAccount(address, password);
                    if (null == account)
                    {
                        MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("账号不存在，多半是密码错误导致。"));
                        return;
                    }

                    //var address = account.Address;

                    string privatekey = account.PrivateKey;

                    if (privatekey.Substring(0, 2) == "0x")
                    {
                        privatekey = privatekey.Substring(2);
                    }

                    string ksText;
                    string fileName;
                    var pk = Nethereum.Hex.HexConvertors.Extensions.HexByteConvertorExtensions.HexToByteArray(privatekey);
                    Share.BLL.KeyStore.SaveKeyStoreFile(password, address, pk, out ksText, out fileName);

                    BlockChain.Common.FileHelper.PositionFile(fileName);

                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("导出失败。一般是密码输入错误导致的，详细错误信息是：") + ex.Message, "Message", MessageBoxButton.OK);
                }
            }
        }

        private void OnLoadPrivateKey(object sender, RoutedEventArgs e)
        {
            if (WindowLoadPrivateKey.LoadPrivateKey(this))
            {
                //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                QueryAddress();

                DoExeCallBack(string.Empty);
            }

        }

        private void OnLoadKeyStore(object sender, RoutedEventArgs e)
        {
            if (WindowLoadKeyStore.LoadKeyStoreFile(this))
            {
                //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                QueryAddress();

                DoExeCallBack(string.Empty);
            }
        }

        private async void QueryTokenAmount_Click(object sender, RoutedEventArgs e)
        {
            #region 传统方法

            //this.Cursor = Cursors.Wait;
            //((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            //try
            //{
            //    string address = TextBoxAddress.Text;

            //    var list = await (new BLL.Address()).QueryAddressTokenAmount(address);
            //    DataGridTokenAmount.ItemsSource = list;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("", ex);
            //}
            //finally
            //{
            //    Cursor = Cursors.Arrow;
            //    ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
            //}

            #endregion

            #region 使用 multicall ,效率更高 , 只需要和 web3 api 交互一次！

            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            try
            {
                string address = TextBoxAddress.Text;
                var tl = Model.Token.DataTable2List(BLL.Token.GetAllSelectedToken()).Select(m => { return m.Address; });

                Nethereum.Web3.Web3 w3 = Share.ShareParam.GetWeb3();
                var bl = await w3.Eth.ERC20.GetAllTokenBalancesUsingMultiCallAsync(address, tl.ToArray<string>());//, Common.CommonAddresses.ENS_REGISTRY_ADDRESS
                var list = new List<AddressTokenAmount>();
                foreach (var b in bl)
                {
                    AddressTokenAmount at = new AddressTokenAmount();
                    at.TokenAddress = b.ContractAddress;
                    at.UserAddress = b.Owner;
                    var m = BLL.Token.GetModel(b.ContractAddress);
                    at.Amount = (decimal)((double)b.Balance / Math.Pow((double)10, (double)m.Decimals));
                    at.ImagePath = m.ImagePath;
                    at.Symbol = m.Symbol;
                    list.Add(at);
                }

                DataGridTokenAmount.ItemsSource = list;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
            }

            #endregion

        }


        private void Topmost_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void Topmost_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }


        public delegate void AddressChanged(object sender, string address);


        public AddressChanged? ExeCallBack = null;

        private void DoExeCallBack(string address)
        {
            if (ExeCallBack != null) {
                ExeCallBack(this, address);
            }
        }


        private static WindowAddressList OneInstance = null;

        public static void ShowAddressListTopmost(Window _owner, AddressChanged callBack= null) {
            if (OneInstance == null)
            {
                OneInstance = new WindowAddressList();
                OneInstance.Owner = _owner;
                OneInstance.Topmost = true;
                OneInstance.ExeCallBack = callBack;
                OneInstance.Show();
            }
            else
            {
                OneInstance.Activate();
            }        
        }

      
    }
}
