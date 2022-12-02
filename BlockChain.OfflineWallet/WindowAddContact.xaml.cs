
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
using Nethereum.Util;

namespace BlockChain.OfflineWallet
{
    /// <summary>
    /// WindowAddContact.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAddContact : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowAddContact()
        {
            InitializeComponent();
            this.Title = Share. LanguageHelper.GetTranslationText(this.Title);
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void OnAddContact(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBocCName.Text.Trim()))
                {
                    TextBocCName.Foreground = Brushes.Red;
                    TextBocCName.Focus();
                    return;
                }

                string address = TextBoxCAddress.Text.Trim();
                if (!address.IsValidEthereumAddressHexFormat())
                {
                    TextBoxCAddress.Foreground = Brushes.Red;
                    TextBocCName.Focus();
                    return;
                }

                BLL.Contact.SaveNewContact(TextBocCName.Text.Trim(), TextBoxCAddress.Text.Trim(), TextBoxCRemark.Text.Trim());

                TextBocCName.Text = string.Empty;
                TextBoxCAddress.Text = string.Empty;
                TextBoxCRemark.Text = string.Empty;
                //this.DialogResult = true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show (this, ex.Message, "Message", MessageBoxButton.OK);
            }
        }
      

        public static bool AddContact(Window _owner)
        {
            WindowAddContact w = new WindowAddContact();
            w.Owner = _owner;

            return w.ShowDialog() == true;
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
