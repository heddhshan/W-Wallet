
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
    /// WindowPassword.xaml 的交互逻辑
    /// </summary>
    public partial class WindowPassword : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowPassword()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            PasswordBoxInput.Focus();
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public static bool GetPassword(Window _owner, string AddressOrHd, string title, out string password, bool enforceSavePassword = false, string tip = "")
        {
            password = string.Empty;
            WindowPassword w = new WindowPassword();
            w.Owner = _owner;

            w.LabelAddress.Content = AddressOrHd;

            if (enforceSavePassword)
            {
                w.CheckBoxRemenberPassword.IsChecked = true;
                w.CheckBoxRemenberPassword.IsEnabled = false;
            }

            if (tip != "")
            {
                w.LabelTip.Content = tip;           //输入密码的时候，可以输入提示语！
            }

            //string password;
            if (Common.PasswordManager.GetPassword(AddressOrHd,  out password))
            {
                w.PasswordBoxInput.Password = password;
                w.CheckBoxRemenberPassword.IsChecked = true;
            }

            if (!string.IsNullOrEmpty(title))
            {
                w.Title = title;
            }

            if (w.ShowDialog() == true)
            {
                password = w.PasswordBoxInput.Password;

                if (w.CheckBoxRemenberPassword.IsChecked == true)
                {
                    Common.PasswordManager.SavePassword(AddressOrHd, password);                
                }

                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
