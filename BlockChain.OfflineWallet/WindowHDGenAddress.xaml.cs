
using BlockChain.Share;
using BlockChain.OfflineWallet.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
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

namespace BlockChain.OfflineWallet
{
    /// <summary>
    /// WindowHDGenAddress.xaml 的交互逻辑
    /// </summary>
    public partial class WindowHDGenAddress : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowHDGenAddress()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            ComBoxMne.ItemsSource = BLL.HDWallet.GetHDWalletsAliasHash().DefaultView;
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private async void OnGenHDWalletAddress(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();

            try
            {
                var MneHash = ComBoxMne.SelectedValue.ToString();
                //  var model = BlockChain.OfflineWallet.DAL.HD_Mnemonic.GetModel(Share. ShareParam.DbConStr, MneHash);
                var model = BLL.HDWallet.GetModel(Share.ShareParam.DbConStr, MneHash);
                if (model == null)
                {
                    return;
                }

                string password = PasswordBox1.Password;

                string EncPassword = Common.PasswordHelper.GetHDRealPassword(model.Salt, password);
                string mne = Common.Security.SymmetricalEncrypt.Decrypt(EncPassword, model.MneEncrypted);

                if (MneHash != BLL.HDWallet.GetMneHash(mne, model.Salt))
                {
                    MessageBox.Show(this, LanguageHelper.GetTranslationText("输入验证没通过，可能是密码错误。"), "Error", MessageBoxButton.OK);
                    PasswordBox1.Focus();
                    return;
                }

                if (true != CheckBox1.IsChecked)
                {
                    int num = int.Parse(TextBoxAddressNum.Text.Trim());
                    string AliasBase = TextBoxAlias.Text.Trim();

                    var T = Task<bool>.Run(() => GenAddress1(model, mne, num, AliasBase));
                    bool result = await T;
                    log.Info("GenAddress1:" + result.ToString());

                    this.DialogResult = true;
                }
                else
                {
                    int num = int.Parse(TextBoxAddressNum.Text.Trim());
                    string AliasBase = TextBoxAlias.Text.Trim();

                    var T = Task<bool>.Run(() => GenAddress2(model, mne, num, AliasBase));
                    bool result = await T;
                    log.Info("GenAddress1:" + result.ToString());

                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, LanguageHelper.GetTranslationText("出错，多半是密码或数据保存错误。") + Environment.NewLine + ex.Message, "Error", MessageBoxButton.OK);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private static bool GenAddress2(HD_Mnemonic model, string mne, int num, string AliasBase)
        {
            for (int i = 1; i <= num; i++)
            {
                string salt2 = Common.Security.Tools.GenRandomStr(32);
                var salt = model.MneFirstSalt + salt2;

                Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(mne, salt, model.MnePath);// BLL.HDWallet.MnePath);

                //int accountIndex = 0;   // Common.Security.Tools.GenRandomInt();
                int accountIndex = Common.Security.Tools.GenRandomInt();
                string alias = AliasBase + "_" + i.ToString();

                var Account = w.GetAccount(accountIndex);

                //验证！
                var a = new Nethereum.Web3.Accounts.Account(Account.PrivateKey.HexToByteArray());
                if (!a.Address.IsTheSameAddress(Account.Address))
                {
                    string err = @"Address Is Error. Mnemonic Account.Address: " + Account.Address + "; Validate Account Address: " + a.Address;
                    throw new Exception(err);
                }

                BlockChain.OfflineWallet.Model.HD_Address m = new BlockChain.OfflineWallet.Model.HD_Address();
                m.Address = Account.Address;
                m.AddressAlias = alias;                     // "默认地址";
                m.AddressIndex = accountIndex;
                m.MneId = model.MneId;
                //m.MneSecondSalt = string.Empty;
                m.MneSecondSalt = salt2;

                m.HasPrivatekey = true;

                m.ValidateEmptyAndLen();
                BlockChain.OfflineWallet.DAL.HD_Address.Insert(Share.ShareParam.DbConStr, m);
            }

            return true;
        }

        private static bool GenAddress1(HD_Mnemonic model, string mne, int num, string AliasBase)
        {
            Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(mne, model.MneFirstSalt, BLL.HDWallet.MnePath);
            for (int i = 1; i <= num; i++)
            {
                int accountIndex = Common.Security.Tools.GenRandomInt();
                string alias = AliasBase + "_" + i.ToString();

                var Account = w.GetAccount(accountIndex);

                //验证！
                var a = new Nethereum.Web3.Accounts.Account(Account.PrivateKey.HexToByteArray());
                if (!a.Address.IsTheSameAddress(Account.Address))
                {
                    string err = @"Address Is Error. Mnemonic Account.Address: " + Account.Address + "; Validate Account Address: " + a.Address;
                    throw new Exception(err);
                }

                BlockChain.OfflineWallet.Model.HD_Address m = new BlockChain.OfflineWallet.Model.HD_Address();
                m.Address = Account.Address;
                m.AddressAlias = alias;                     // "默认地址";
                m.AddressIndex = accountIndex;
                m.MneId = model.MneId;
                m.MneSecondSalt = string.Empty;

                m.HasPrivatekey = true;

                m.ValidateEmptyAndLen();
                BlockChain.OfflineWallet.DAL.HD_Address.Insert(Share.ShareParam.DbConStr, m);
            }
            return true;
        }

        public static bool GenAddress(Window _owner)
        {
            WindowHDGenAddress w = new WindowHDGenAddress();
            w.Owner = _owner;
            return w.ShowDialog() == true;
        }
    }

}
