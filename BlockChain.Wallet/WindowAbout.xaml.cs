//using ABI.System;
using BlockChain.Share;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
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
    /// WindowAbout.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAbout : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowAbout()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            //certutil -hashfile file.zip SHA256

            TabControl_SelectionChanged(TabControlMain, null);

            TextBlockNotice.Text = LanguageHelper.GetTranslationText(ShareParam.PrivacyAndSafeText);
            TextBlockDisclaimer.Text = LanguageHelper.GetTranslationText(ShareParam.DisclaimerText);

            //LabelDonation.Content = Share.ShareParam.DonationAddress;

            SetTextBokSelectAll(TextBoxBT);
            SetTextBokSelectAll(TextBoxEd2k);
            SetTextBokSelectAll(TextBoxHttp);
            SetTextBokSelectAll(TextBoxIpfs);
            SetTextBokSelectAll(TextBoxOther);
            SetTextBokSelectAll(TextBoxSha256);

            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void SetTextBokSelectAll(TextBox LIKE_textBox)
        {
            //https://www.cnblogs.com/babietongtianta/p/3952214.html
            LIKE_textBox.PreviewMouseDown += new MouseButtonEventHandler(LIKE_textBox_PreviewMouseDown);        //注意，这个事件的注册必须在LIKE_textBox获得焦点之前
            LIKE_textBox.GotFocus += new RoutedEventHandler(LIKE_textBox_GotFocus);
            LIKE_textBox.LostFocus += new RoutedEventHandler(LIKE_textBox_LostFocus);
        }

        void LIKE_textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var LIKE_textBox = sender as TextBox;
            LIKE_textBox.PreviewMouseDown += new MouseButtonEventHandler(LIKE_textBox_PreviewMouseDown);
        }

        void LIKE_textBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var LIKE_textBox = sender as TextBox;
            LIKE_textBox.Focus();
            e.Handled = true;
        }

        void LIKE_textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var LIKE_textBox = sender as TextBox;
            LIKE_textBox.SelectAll();
            LIKE_textBox.PreviewMouseDown -= new MouseButtonEventHandler(LIKE_textBox_PreviewMouseDown);
        }


        public static void ShowAbout(Window _owner)
        {
            WindowAbout w = new WindowAbout();
            w.Owner = _owner;
            w.ShowDialog();
        }

        private int PageIndex = -1;

        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                if (PageIndex == TabControlMain.SelectedIndex)
                {
                    return;
                }
                PageIndex = TabControlMain.SelectedIndex;

                if (TabControlMain.SelectedIndex == 0)
                {
                    //处理当前版本
                    TextBlockCurVersion.Text = SystemParam.Version.ToString();

                    try
                    {
                        try
                        {
                            HyperlinkLinkInfo.NavigateUri = new System.Uri(SystemParam.LinkInfo);
                            HyperlinkLinkInfo.Inlines.Clear();
                            HyperlinkLinkInfo.Inlines.Add(SystemParam.LinkInfo);
                        }
                        catch (System.Exception e1) 
                        { 
                            log.Error("", e1); }

                        Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();
                        Share.Contract.AppInfo.AppInfoService service = new Share.Contract.AppInfo.AppInfoService(web3, ShareParam.AddressAppInfo);
                        var ContractInfo = await service.ContactInfoQueryAsync();
                        if (!string.IsNullOrEmpty(ContractInfo))
                        {
                            HyperlinkLinkInfo.Inlines.Clear();
                            try
                            {
                                HyperlinkLinkInfo.NavigateUri = new System.Uri(ContractInfo);      //如果是 email， 需要写成 mailto:*** ; 如果是网址，需要写成 http****
                            }
                            catch (System.Exception ex1)
                            {
                                log.Error("NavigateUri", ex1);
                            }
                            HyperlinkLinkInfo.Inlines.Add(ContractInfo);
                        }
                        //else
                        //{
                        //    //HyperlinkLinkInfo.NavigateUri = new Uri(SystemParam.LinkInfo);
                        //    //HyperlinkLinkInfo.Inlines.Clear();
                        //    //HyperlinkLinkInfo.Inlines.Add("email");
                        //}
                        //var PR = service.CurProgramInfoQueryAsync.GetProgramInfoQueryAsync().Result;
                        var ProgramInfo = await service.CurAppVersionOfQueryAsync(SystemParam.AppId, Share.BlockChainAppId.PlatformId);
                        var DownLoadInfo = await service.CurAppDownloadOfQueryAsync(SystemParam.AppId, Share.BlockChainAppId.PlatformId);
                        if (null != ProgramInfo)
                        {
                            TextBlockLastVer.Text = ProgramInfo.Version.ToString();
                            TextBoxBT.Text = DownLoadInfo.BTLink;
                            TextBoxEd2k.Text = DownLoadInfo.EMuleLink;
                            TextBoxHttp.Text = DownLoadInfo.HttpLink;
                            TextBoxUpdateInfo.Text = ProgramInfo.UpdateInfo;
                            TextBoxSha256.Text = ProgramInfo.Sha256Value.ToHex(true);            //certutil -hashfile SystemParam.LOG.20200401.TXT SHA256
                            TextBoxIpfs.Text = DownLoadInfo.IpfsLink;                  //后加的，可能会出错
                            TextBoxOther.Text = DownLoadInfo.OtherLink;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        log.Error("", ex);
                    }
                }
                else if (TabControlMain.SelectedIndex == 1)
                {
                    LableAppInfo.Content = ShareParam.AddressAppInfo;
                    LabelBacth.Content = SystemParam.AddressWalletHelper;
                    LableUniswapV2RouterV2.Content = ShareParam.AddressUniV2Router02;
                    LableUniswapV3SwapRouter.Content = ShareParam.AddressUniV3SwapRouter;
                }
                else if (TabControlMain.SelectedIndex == 2)
                {
                }

            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void LabelContract_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                var la = sender as Label;
                var ContractAddress = la.Content.ToString();
                var url = ShareParam.GetAddressUrl(ContractAddress);
                System.Diagnostics.Process.Start("explorer.exe", url);
            }
        }

        private void OnDownLoadBT(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxBT.Text))
            {
                //System.Diagnostics.Process.Start("explorer.exe", TextBoxBT.Text);
                ShareParam.OperUrl(TextBoxBT.Text);

            }
        }

        private void OnDownLoadeMule(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxEd2k.Text))
            {
                //System.Diagnostics.Process.Start("explorer.exe", TextBoxEd2k.Text);
                ShareParam.OperUrl(TextBoxEd2k.Text);
            }
        }

        private void OnClickTelegram(object sender, RoutedEventArgs e)
        {
            try
            {
                //不一定是telegram！
                string url = HyperlinkLinkInfo.NavigateUri.ToString();
                if (!string.IsNullOrEmpty(url))
                {
                    //System.Diagnostics.Process.Start("explorer.exe", url);
                    ShareParam.OperUrl(url);
                }
            }
            catch (System.Exception ex) { log.Error("Contract Person Info", ex); }
        }

        private void OnDownLoadHttp(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxHttp.Text))
            {
                //System.Diagnostics.Process.Start("explorer.exe", TextBoxHttp.Text);
                ShareParam.OperUrl(TextBoxHttp.Text);
            }
        }

        private void OnDownLoadIpfs(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxIpfs.Text))
            {
                //System.Diagnostics.Process.Start("explorer.exe", TextBoxIpfs.Text);
                ShareParam.OperUrl(TextBoxIpfs.Text);
            }
        }

        private void OnDownLoadOther(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxOther.Text))
            {
                //System.Diagnostics.Process.Start("explorer.exe", TextBoxOther.Text);
                ShareParam.OperUrl(TextBoxOther.Text);
            }
        }


        private void OnLinkClick(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink)
            {
                Hyperlink link = sender as Hyperlink;
                Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
            }
        }

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


        //private void OnClickDonation(object sender, RoutedEventArgs e)
        //{
        //    string to = LabelDonation.Content.ToString();
        //    WindowTransfer.DoTransfer(this, to);
        //}


    }
}
