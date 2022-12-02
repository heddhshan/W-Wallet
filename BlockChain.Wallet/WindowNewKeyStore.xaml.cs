
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

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowNewKeyStore.xaml 的交互逻辑
    /// </summary>
    public partial class WindowNewKeyStore : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowNewKeyStore()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            LabelPasswordErr.Visibility = Visibility.Hidden;
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password == PasswordBox2.Password)
            {
                LabelPasswordErr.Visibility = Visibility.Hidden;
            }
            else
            {
                LabelPasswordErr.Visibility = Visibility.Visible;
                PasswordBox1.Focus();
                return;
            }

            Share.BLL.KeyStore.SaveKeyStore(TextBoxAlias.Text.Trim(), PasswordBox1.Password);
            this.DialogResult = true;
        }


        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            //ProgressBarPassword
            var ps = BlockChain.Common.PasswordHelper.PasswordStrength(PasswordBox1.Password);
            ProgressBarPassword.Value = (int)ps;
        }
               
        public static bool NewKeyStoreAddress(Window _owner)
        {
            WindowNewKeyStore w = new WindowNewKeyStore();
            w.Owner = _owner;
            var result = w.ShowDialog() == true;
            return result;
        }

        private void OnPasswordChanged2(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password == PasswordBox2.Password)
            {
                LabelPasswordErr.Visibility = Visibility.Hidden;
            }
            else
            {
                LabelPasswordErr.Visibility = Visibility.Visible;
            }
        }
    }
}
