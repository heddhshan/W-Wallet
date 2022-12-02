
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
    /// WindowModifyPassword.xaml 的交互逻辑
    /// </summary>
    public partial class WindowModifyPassword : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowModifyPassword()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            ComBoxMne.ItemsSource = BLL.HDWallet.GetHDWalletsAliasHash().DefaultView;

            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void OnModifyPassword(object sender, RoutedEventArgs e)
        {
            try
            {
                //修改密码步骤，1，验证输入密码；2，使用新密码生成加密助记词，并更新。
                string oldPassword = PasswordBox1.Password;
                string newPassword = PasswordBox21.Password;

                var MneHash = ComBoxMne.SelectedValue.ToString();
                //            var model = BlockChain.Wallet.DAL.HD_Mnemonic.GetModel(Share. ShareParam.DbConStr, MneHash);
                var model = BLL.HDWallet.GetModel(Share. ShareParam.DbConStr, MneHash);
                if (model == null)
                {
                    return;
                }

                string UsePassword = Common.PasswordHelper.GetHDRealPassword(model.Salt, oldPassword);
                var PasswordHash = Common.Security.Tools.GetHashValue(UsePassword);
                if (PasswordHash != model.UserPasswordHash)
                {
                    MessageBox.Show (this, LanguageHelper.GetTranslationText("密码验证失败。"));
                    return;
                }

                var mne = Common.Security.SymmetricalEncrypt.Decrypt(UsePassword, model.MneEncrypted);

                //Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(mne, model.MneFirstSalt, BLL.HDWallet.MnePath);
                ////验证地址
                //var ma = BLL.HDWallet.GetOneAddressModel(model.MneId);

                //var thisaddress = w.GetAccount(ma.AddressIndex).Address;
                //if (thisaddress.ToLower() != ma.Address.ToLower())
                //{
                //    MessageBox.Show (this, "检验结果不一致。", "Message", MessageBoxButton.OK);
                //    return;
                //}

                var NewUsePassword = Common.PasswordHelper.GetHDRealPassword(model.Salt, newPassword);
                model.MneEncrypted = Common.Security.SymmetricalEncrypt.Encrypt(NewUsePassword, mne);
                model.UserPasswordHash = Common.Security.Tools.GetHashValue(NewUsePassword);

                BlockChain.Wallet.DAL.HD_Mnemonic.Update(Share. ShareParam.DbConStr, model);

                this.DialogResult = true;
            }
            catch (Exception ex) { 
                MessageBox.Show (this, ex.Message);
            }
        }


        public static bool ModifyPassword(Window _owner)
        {
            WindowModifyPassword w = new WindowModifyPassword();
            w.Owner = _owner;
            return w.ShowDialog() == true;
        }

        private void OnHdSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var MneHash = ComBoxMne.SelectedValue.ToString();
            var model = BLL.HDWallet.GetModel(Share. ShareParam.DbConStr, MneHash);
            LabelUserPasswordTip.Content = model.UserPasswordTip;
        }
    }
}
