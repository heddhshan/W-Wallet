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
using BlockChain.Share;

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowLoadMnemonic.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLoadMnemonic : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowLoadMnemonic()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void OnLoadHDWallet(object sender, RoutedEventArgs e)
        {
            if (!CheckMneInput())
            {
                return;
            }

            string alias = TextBoxWalletName.Text.Trim();
            string password = PasswordBoxInput1.Password;
            string tip = TextBoxPasswordTip.Text.Trim();
            var Mnemonic = this.TextBoxMne.Text.Trim();

            var model = BLL.HDWallet.NewHDWallet(alias, password, tip, Mnemonic, true);

            log.Info(LanguageHelper.GetTranslationText("导入助记词成功:") + model.MneAlias);

            this.DialogResult = true;   //关闭
        }


        private void OnLoadHDWallet1(object sender, RoutedEventArgs e)
        {
            if (!CheckMneInput())
            {
                return;
            }

            string alias = TextBox1WalletName.Text.Trim();
            string walletPassword = PasswordBox1Input1.Password;
            string tip = TextBoxPassword1Tip.Text.Trim();
            var Mnemonic = this.TextBox1Mne.Text.Trim();

            string mneSalt = PasswordBox1MnePassword.Password;
            string path = ComboBox1Path.Text.Trim();

            BLL.HDWallet.NewHDWalletBy3Party(Mnemonic, mneSalt, path, alias, walletPassword, tip);
            this.DialogResult = true;   //关闭
        }


        private void OnValidate(object sender, RoutedEventArgs e)
        {
            if (!CheckMneInput())
            {
                return;
            }

            try
            {
                var Mnemonic = this.TextBoxMne.Text.Trim();

                //生成钱包          
                Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(Mnemonic, "", BLL.HDWallet.MnePath);
                if (w.GetAccount(0).Address.ToLower() == TextBoxMainAddress.Text.Trim().ToLower())
                {
                    MessageBox.Show (this, LanguageHelper.GetTranslationText("检验结果一致。"), "Message", MessageBoxButton.OK);
                    TextBoxWalletName.Focus();
                }
                else
                {
                    MessageBox.Show (this, LanguageHelper.GetTranslationText("检验结果不同，请检查助记词和主地址输入是否有误。"), "Message", MessageBoxButton.OK);
                    TextBoxMne.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (this, ex.Message, "Message", MessageBoxButton.OK);
                TextBoxMne.Focus();
            }
        }

        private void OnValidate1(object sender, RoutedEventArgs e)
        {
            if (!CheckMneInput1())
            {
                return;
            }

            try
            {
                var Mnemonic = this.TextBox1Mne.Text.Trim();
                string mnePassword = PasswordBox1MnePassword.Password;
                string path = ComboBox1Path.Text.Trim();
                var addressNo = int.Parse(TextBox1MainAddressNo.Text.Trim());
                Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(Mnemonic, mnePassword, path);

                string address1 = w.GetAccount(addressNo).Address;

                ////验证助记词生成的地址是否和nethereum生成的助记词一样！
                //var a = new Nethereum.Web3.Accounts.Account(ccount.PrivateKey.HexToByteArray());
                //if (a.Address != Account.Address || !a.Address.IsTheSameAddress(Account.Address))
                //{
                //    string err = @"Address Is Error. Mnemonic Account.Address: " + Account.Address + "; Validate Account Address: " + a.Address;
                //    throw new Exception(err);
                //}


                if (address1.ToLower() == TextBoxMainAddress.Text.Trim().ToLower())
                {
                    MessageBox.Show (this, LanguageHelper.GetTranslationText("检验结果一致。"), "Message", MessageBoxButton.OK);
                    TextBoxWalletName.Focus();
                }
                else
                {
                    MessageBox.Show (this, LanguageHelper.GetTranslationText("检验结果不同，请检查助记词和主地址输入是否有误。"), "Message", MessageBoxButton.OK);
                    TextBoxMne.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (this, ex.Message, "Message", MessageBoxButton.OK);
                TextBoxMne.Focus();
            }
        }

        private bool CheckMneInput()
        {
            var Mnemonic = this.TextBoxMne.Text.Trim();

            var words = Mnemonic.Split(' ');//12，15，18，21，24
            if (words.Length == 12 || words.Length == 15 || words.Length == 18 || words.Length == 21 || words.Length == 24)
            {
                return true;
            }
            else
            {
                MessageBox.Show (this, LanguageHelper.GetTranslationText("输入单词数目不对，按照目前助记词的标准只支持12、15、18、21、24个单词。"), "Message", MessageBoxButton.OK);
                TextBoxMne.Focus();
                return false;
            }
        }


        private bool CheckMneInput1()
        {
            var Mnemonic = this.TextBox1Mne.Text.Trim();

            var words = Mnemonic.Split(' ');//12，15，18，21，24
            if (words.Length == 12 || words.Length == 15 || words.Length == 18 || words.Length == 21 || words.Length == 24)
            {
                return true;
            }
            else
            {
                MessageBox.Show (this, LanguageHelper.GetTranslationText("输入单词数目不对，按照目前助记词的标准只支持12、15、18、21、24个单词。"), "Message", MessageBoxButton.OK);
                TextBox1Mne.Focus();
                return false;
            }
        }


        public static bool LoadMne(Window _owner)
        {
            WindowLoadMnemonic w = new WindowLoadMnemonic();
            w.Owner = _owner;
            return w.ShowDialog() == true;
        }

        private void OnInputPassword(object sender, RoutedEventArgs e)
        {
            var v = (int)Common.PasswordHelper.PasswordStrength(PasswordBoxInput1.Password);
            ProgressBar1.Value = v;
        }
    }

}
