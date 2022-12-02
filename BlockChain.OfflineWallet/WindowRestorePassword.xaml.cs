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

namespace BlockChain.OfflineWallet
{
    /// <summary>
    /// WindowRestorePassword.xaml 的交互逻辑
    /// </summary>
    public partial class WindowRestorePassword : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowRestorePassword()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            ComBoxMne.ItemsSource = BLL.HDWallet.GetHDWalletsAliasHash().DefaultView;
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
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

        private void OnVilidate(object sender, RoutedEventArgs e)
        {
            if (!CheckMneInput())
            {
                return;
            }

            try
            {
                var MneHash = ComBoxMne.SelectedValue.ToString();
                var model = BLL.HDWallet.GetModel(Share. ShareParam.DbConStr, MneHash);
                if (model == null)
                {
                    return;
                }

                var mne = TextBoxMne.Text.Trim();
                var mnehash = Common.Security.Tools.GetHashValue(model.Salt + mne);
                if (mnehash != model.MneHash)
                {
                    MessageBox.Show (this, LanguageHelper.GetTranslationText("检验结果一致。"), "Message", MessageBoxButton.OK);
                        return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show (this, ex.Message, "Error", MessageBoxButton.OK);
                TextBoxMne.Focus();
            }
        }

        private void OnModifyPassword(object sender, RoutedEventArgs e)
        {
            if (PasswordBox21.Password != PasswordBox22.Password)
            {
                MessageBox.Show (this, LanguageHelper.GetTranslationText("两次输入密码不一致。"));
                PasswordBox21.Focus();
                return;
            }

            var MneHash = ComBoxMne.SelectedValue.ToString();

            //    var model = BlockChain.OfflineWallet.DAL.HD_Mnemonic.GetModel(Share. ShareParam.DbConStr, MneHash);
            var model = BLL.HDWallet.GetModel(Share. ShareParam.DbConStr, MneHash);
            if (model == null)
            {
                return;
            }

            var mne = TextBoxMne.Text.Trim();


            Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(mne, model.MneFirstSalt, BLL.HDWallet.MnePath);
            //验证地址
            var ma = BLL.HDWallet.GetHdFirstAddressModel(model.MneId);

            if (w.GetAccount(ma.AddressIndex).Address.ToLower() != ma.Address.ToLower())
            {
                MessageBox.Show (this, LanguageHelper.GetTranslationText("检验结果不一致。"), "Message", MessageBoxButton.OK);
                return;
            }

            string NewUsePassword = Common.PasswordHelper.GetHDRealPassword( model.Salt ,PasswordBox21.Password);
            model.MneEncrypted = Common.Security.SymmetricalEncrypt.Encrypt(NewUsePassword, mne);
            model.UserPasswordHash = Common.Security.Tools.GetHashValue(NewUsePassword);

            BlockChain.OfflineWallet.DAL.HD_Mnemonic.Update(Share. ShareParam.DbConStr, model);

            this.DialogResult = true;
        }


        public static bool ModifyPassword(Window _owner)
        {
            WindowRestorePassword w = new WindowRestorePassword();
            w.Owner = _owner;
            return w.ShowDialog() == true;
        }

        private void OnInputPassword(object sender, RoutedEventArgs e)
        {
            var v = (int)Common.PasswordHelper.PasswordStrength(PasswordBox21.Password);
            ProgressBar1.Value = v;

        }
    }
}
