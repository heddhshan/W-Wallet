
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
    /// WindowLoadPrivateKey.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLoadPrivateKey : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowLoadPrivateKey()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            LabelPasswordErr.Visibility = Visibility.Hidden;
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }


        private void OnLoadPrivateKey(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password != PasswordBox2.Password) {
                LabelPasswordErr.Visibility = Visibility.Visible;
                return;
            }
            try
            {
                string password = PasswordBox1.Password;
                if (string.IsNullOrEmpty(password)) return;

                string privatekey = TextBoxPrivateKey.Text.Trim();
                if (privatekey.Substring(0, 2) == "0x")
                {
                    privatekey = privatekey.Substring(2);
                }

                var account = new Nethereum.Web3.Accounts.Account(privatekey);
                var address = account.Address;

                string ksText;
                string fileName;
                               
                var pk = Nethereum.Hex.HexConvertors.Extensions.HexByteConvertorExtensions.HexToByteArray(privatekey);
                Share.BLL.KeyStore.SaveKeyStoreFile(password, address, pk, out ksText, out fileName);

                string alias = this.TextBoxAlias.Text.Trim();
                Share.BLL.KeyStore.SaveKeyStoreToDb(alias, address, ksText, fileName);

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show (this, LanguageHelper.GetTranslationText("一般是地址重复、或者没有文件写入权限，等造成的异常。") + Environment.NewLine +  ex.Message, "Error", MessageBoxButton.OK);
            }
        }


        public static bool LoadPrivateKey(Window _owner)
        {
            WindowLoadPrivateKey w = new WindowLoadPrivateKey();
            w.Owner = _owner;
            return w.ShowDialog() == true;
        }

        private void OnInputPassword(object sender, RoutedEventArgs e)
        {
            var v = (int)Common.PasswordHelper.PasswordStrength(PasswordBox1.Password);
            ProgressBar1.Value = v;
        }

        private void OnPassword2Changed(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password != PasswordBox2.Password)
            {
                LabelPasswordErr.Visibility = Visibility.Visible;
            }
            else
            {
                LabelPasswordErr.Visibility = Visibility.Hidden;
            }
        }
    }
}
