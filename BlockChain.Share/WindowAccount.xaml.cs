using BlockChain.Share;
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


namespace BlockChain.Share
{
    /// <summary>
    /// WindowAccount.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAccount : Window
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowAccount()
        {
            InitializeComponent();
            this.Title = Share.LanguageHelper.GetTranslationText(this.Title);


            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            //from 地址   
            var dv = ThisAddressImp.GetAllTxAddress();
            ComboBoxAddress1.ItemsSource = dv;

            LabelBaseToken.Content = Share.ShareParam.BaseToken;

        }

        private void OnSelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)System.Windows.Application.Current.MainWindow).ShowProcessing();
            try
            {
                var row = ComboBoxAddress1.SelectedItem;
                if (row is TxAddress)
                {
                    TxAddress r = (TxAddress)row;
                    //AddressAlias, Address, 'KeyStore' AS SourceName
                    string Address = r.Address;// r[1].ToString();
                    ComboBoxAddress1.Tag = Address;     //保存地址

                    string password;
                    if (Common.PasswordManager.GetPassword(Address, out password))
                    {
                        PasswordBox1.Password = password;
                        CheckBoxRemenberPassword.IsChecked = true;
                    }

                    if (!Share.ShareParam.IsAnEmptyAddress(ThisToken))
                    {
                        try
                        {
                            LabelAmount.Content = ThisAddressImp.GetRealBalance(Address, ThisToken);
                        }
                        catch (Exception ex)
                        {
                            log.Error("", ex);
                            LabelAmount.Content = ex.Message;
                        }
                    }

                    try
                    {
                        LabelEthAmount.Content = ThisAddressImp.GetRealBalance(Address, ShareParam.EmptyAddress);
                    }
                    catch (Exception ex)
                    {
                        log.Error("", ex);
                        LabelAmount.Content = ex.Message;
                    }
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

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private Nethereum.Web3.Accounts.Account ThisAccount = null;

        private void OnDoOk(object sender, RoutedEventArgs e)
        {
            if (ComboBoxAddress1.Tag == null)
            {
                ComboBoxAddress1.Focus();
                return;
            }

            var address = ComboBoxAddress1.Tag.ToString();
            var password = PasswordBox1.Password;

            try
            {
                ThisAccount = ThisAddressImp.GetAccount(address, password);
                if (ThisAccount == null)
                {
                    LabelInputPassword.Foreground = Brushes.Red;
                    PasswordBox1.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                LabelInputPassword.Foreground = Brushes.Red;
                PasswordBox1.Focus();
                return;
            }

            if (CheckBoxRemenberPassword.IsChecked == true)
            {
                Common.PasswordManager.SavePassword(address, password);
            }

            this.DialogResult = true;
        }

        private string ThisToken = ShareParam.AddressEth;       // 默认是筹码币，相当于给筹码币打广告  // BLL.Token.EmptyAddress;      //默认是以太坊
        //private string ThisToken = ShareParam.AddressCasino; // 默认是筹码币，相当于给筹码币打广告  // BLL.Token.EmptyAddress;      //默认是以太坊
        private IAddress ThisAddressImp;

        public static Nethereum.Web3.Accounts.Account GetAccount(Window _owner, IAddress thisAddressImp, string title = "Select Account", bool enforceSavePassword = false)
        {
            WindowAccount w = new WindowAccount();
            w.ThisAddressImp = thisAddressImp;
            w.Owner = _owner;       // System.Windows.Application.Current.MainWindow;
            w.Title = title;

            w.LabelToken.Visibility = Visibility.Hidden;
            w.LabelAmount.Visibility = Visibility.Hidden;

            if (enforceSavePassword)
            {
                w.CheckBoxRemenberPassword.IsChecked = true;
                w.CheckBoxRemenberPassword.IsEnabled = false;
            }

            if (w.ShowDialog() == true)
            {
                return w.ThisAccount;
            }
            else
            {
                return null;
            }
        }

        public static Nethereum.Web3.Accounts.Account GetAccount(Window _owner, IAddress thisAddressImp, string token, string tokenSymbol, string title = "Select Account", bool enforceSavePassword = false)
        {
            WindowAccount w = new WindowAccount();
            w.ThisAddressImp = thisAddressImp;
            w.Owner = _owner;       // System.Windows.Application.Current.MainWindow;
            w.Title = title;
            if (Share.ShareParam.IsAnEmptyAddress(token))
            {
                w.ThisToken = token;
                w.LabelToken.Visibility = Visibility.Hidden;
                w.LabelAmount.Visibility = Visibility.Hidden;
                ////token = ShareParam.AddressCasino;
                //w.ThisToken = token;
                //这个 token 不能为 ETH , 否则就重复了！！！
                //w.LabelToken.Content = "Casino Balance:";               
            }
            else
            {
                w.ThisToken = token;                                    //这个 token 不能为 ETH , 否则就重复了！！！
                w.LabelToken.Content = tokenSymbol + " Balance:";
            }
            w.LabelToken.ToolTip = w.ThisToken;

            if (enforceSavePassword)
            {
                w.CheckBoxRemenberPassword.IsChecked = true;
                w.CheckBoxRemenberPassword.IsEnabled = false;
            }

            if (w.ShowDialog() == true)
            {
                return w.ThisAccount;
            }
            else
            {
                return null;
            }
        }

    }

}
