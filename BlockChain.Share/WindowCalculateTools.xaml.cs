using Microsoft.Win32;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
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

namespace BlockChain.Share
{
    /// <summary>
    /// WindowCalculateTools.xaml 的交互逻辑
    /// </summary>
    public partial class WindowCalculateTools : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowCalculateTools()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());

            TextBoxPrivateKey_LostFocus(null, null);
            TextBoxPrivateKey41_LostFocus(null, null);

            TextBoxLocalDt.Text = System.DateTime.Now.ToString();
        }


        private void OnClickFS(object sender, RoutedEventArgs e)
        {
            var fun = TextBoxFun.Text.Trim().Replace(" ", "");    //不能有空格！！！

            var hash = Nethereum.Web3.Web3.Sha3(fun);
            if (!hash.StartsWith("0x"))
            {
                hash = "0x" + hash;
            }

            //public const string transfer = "0xa9059cbb";        //   transfer(address, uint256)： 0xa9059cbb
            //public const string balanceOf = "0x70a08231";       //   balanceOf(address)：0x70a08231

            var result = hash.Substring(0, 10);
            TextBoxSig.Text = result;
        }

        private void OnClickES(object sender, RoutedEventArgs e)
        {
            var solidityevent = TextBoxEvent.Text.Trim().Replace(" ", "");    //不能有空格！！！

            var hash = Nethereum.Web3.Web3.Sha3(solidityevent);
            if (!hash.StartsWith("0x"))
            {
                hash = "0x" + hash;
            }

            //public const string transfer = "0xa9059cbb";        //   transfer(address, uint256)： 0xa9059cbb
            //public const string balanceOf = "0x70a08231";       //   balanceOf(address)：0x70a08231

            TextBoxEventSig.Text = hash;
        }

        private void OnGotoUrl(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var url = b.Tag.ToString();
                System.Diagnostics.Process.Start("explorer.exe", url);
            }
        }

        private void OnSignature(object sender, RoutedEventArgs e)
        {
            string hash = TextBoxHash.Text;
            string privatekey = TextBoxPrivateKey.Text;

            TextBoxSignature.Text = Share.ShareParam.GenSignature(hash, privatekey);
        }


        private void OnRecover(object sender, RoutedEventArgs e)
        {
            string hash = TextBoxHash1.Text;
            string sig = TextBoxSignature1.Text;

            TextBoxAddress.Text = Share.ShareParam.GetAddress(hash, sig);
        }

        private void TextBoxPrivateKey_LostFocus(object sender, RoutedEventArgs e)
        {
            LabelAddressFP.Content = string.Empty;
            try
            {
                var pk = TextBoxPrivateKey.Text;
                Nethereum.Web3.Accounts.Account account = new Nethereum.Web3.Accounts.Account(pk.HexToByteArray());
                LabelAddressFP.Content = account.Address;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
        }

        private void TextBoxPrivateKey41_LostFocus(object sender, RoutedEventArgs e)
        {
            LabelAddressFP41.Content = string.Empty;
            try
            {
                var pk = TextBoxPrivateKey41.Text;
                Nethereum.Web3.Accounts.Account account = new Nethereum.Web3.Accounts.Account(pk.HexToByteArray());
                LabelAddressFP41.Content = account.Address;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
        }

        private void OnSignature41(object sender, RoutedEventArgs e)
        {
            TextBoxSignature41.Text = string.Empty;
            try
            {
                string plainMessage = TextBoxPlainMessage.Text;
                string privateKey = TextBoxPrivateKey41.Text;

                var signer = new MessageSigner();
                TextBoxSignature41.Text = signer.HashAndSign(plainMessage, privateKey);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void OnRecover42(object sender, RoutedEventArgs e)
        {

            TextBoxAddress42.Text = string.Empty;
            try
            {
                string plainMessage = TextBoxPlainMessage42.Text;
                string signature = TextBoxSignature42.Text;

                var signer = new MessageSigner();
                TextBoxAddress42.Text = signer.HashAndEcRecover(plainMessage, signature);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }


        private void ToUnix_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dt1 = System.DateTime.Parse(TextBoxLocalDt.Text.Trim());
                TextBoxUnixDt.Text = Common.DateTimeHelper.ConvertDateTime2Int(dt1).ToString();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void ToDateTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dt1 = int.Parse(TextBoxUnixDt.Text.Trim());
                TextBoxLocalDt.Text = Common.DateTimeHelper.ConvertInt2DateTime(dt1).ToString();
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
            }
        }



        private void ToHex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string s = TextBoxString.Text.Trim();
                TextBoxHex.Text = UTF8Encoding.UTF8.GetBytes(s).ToHex(true);        //s.ToHexUTF8();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void ToString_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var h = TextBoxHex.Text.Trim().HexToByteArray();
                TextBoxString.Text = UTF8Encoding.UTF8.GetString(h);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }

        }

        public static void ExeWindowCalculateTools(Window _owner)
        {
            WindowCalculateTools w = new WindowCalculateTools();
            w.Owner = _owner;
            w.ShowDialog();
        }
    }
}
