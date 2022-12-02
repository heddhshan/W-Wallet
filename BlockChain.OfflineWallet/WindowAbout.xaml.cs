using BlockChain.Share;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// WindowAbout.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAbout : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string  UpdateRemark = @"版本更新说明：离线钱包被设计为脱离网络使用，如果要下载新版本，可以在连接互联网的电脑上打开在线钱包，在线钱包里面会提供下载的链接。";
        public WindowAbout()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            TextBlockUpdateRemark.Text = LanguageHelper.GetTranslationText(UpdateRemark);
            TextBlockCurVersion.Text = OffWalletParam.Version.ToString();
            TextBoxLinkInfo.Text = OffWalletParam.LinkInfo;
           
            //certutil -hashfile file.zip SHA256
            //TabControl_SelectionChanged(TabControlMain, null);

            TextBlockNotice.Text = LanguageHelper.GetTranslationText(ShareParam.PrivacyAndSafeText);
            TextBlockDisclaimer.Text = LanguageHelper.GetTranslationText(ShareParam.DisclaimerText);

            //LabelDonation.Content = Share.ShareParam.DonationAddress;

            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        public static void ShowAbout(Window _owner)
        {
            WindowAbout w = new WindowAbout();
            w.Owner = _owner;
            w.ShowDialog();
        }

        ////private int PageIndex = -1;

        //private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void LabelContract_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (sender is Label)
        //    {
        //        var la = sender as Label;
        //        var ContractAddress = la.Content.ToString();
        //        var url = ShareParam.GetAddressUrl(ContractAddress);
        //        System.Diagnostics.Process.Start("explorer.exe", url);
        //    }
        //}


        //private void OnClickTelegram(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //不一定是telegram！
        //        string url = HyperlinkLinkInfo.NavigateUri.ToString();
        //        if (!string.IsNullOrEmpty(url))
        //        {
        //            System.Diagnostics.Process.Start("explorer.exe", url);
        //        }
        //    }
        //    catch (Exception ex) { log.Error("Contract Person Info", ex); }
        //}


        //private void OnLinkClick(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Hyperlink)
        //    {
        //        Hyperlink link = sender as Hyperlink;
        //        Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        //    }
        //}


        private void GoToUrl_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                var tag = (sender as Button).Tag;
                if (tag != null)
                {
                    var url = tag.ToString();
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
        }

        //}



    }
}
