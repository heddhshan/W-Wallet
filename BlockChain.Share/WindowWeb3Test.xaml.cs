using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


namespace BlockChain.Share
{
    /// <summary>
    /// WindowWeb3Test.xaml 的交互逻辑
    /// </summary>
    public partial class WindowWeb3Test : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowWeb3Test()
        {
            InitializeComponent();
            this.Title = Share.LanguageHelper.GetTranslationText(this.Title);


            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            LabelEthExplore.Content = "Current Block Chain Browser : " + Share.ShareParam.EtherSacn;     // Properties.Settings.Default.EtherSacn;

            DataGridWeb3Url.ItemsSource = BLL.Web3Url.GetUrlDt().DefaultView;
            LabelWeb3Url.Content = ShareParam.Web3Url;// BLL.Web3Url.GetCurWeb3UrlModel(false).Url;
            HyperlinkEthereumNodes.NavigateUri = new Uri(Share.ShareParam.GetNodesListUrl());
        }


        /// <summary>
        /// 如果是测试进入，在退出设置前需要检查！
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task<bool> ThisTestWeb3Url()
        {
            Cursor = Cursors.Wait;
            try
            {
                var web3 = Share.ShareParam.GetWeb3(LabelWeb3Url.Content.ToString());
                var blocknum = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                LabelWeb3Url.Foreground = Brushes.Green;

                this.DialogResult = true;
                //MessageBox.Show (this, "连接成功，获取的最新区块号是" + blocknum.ToString(), "测试连接Web3 Url结果", MessageBoxButton.OK);

                return true;
            }
            catch
            {
                LabelWeb3Url.Foreground = Brushes.Red;
                return false;
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void OnTestWeb3Url2(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Cursor = Cursors.Wait;
                try
                {
                    var b = ((Button)sender);
                    var s = (StackPanel)b.Parent;
                    var UrlHash = s.Tag.ToString();

                    string url = DAL.Web3Url.GetModel(Share.ShareParam.DbConStr, UrlHash).Url;

                    var result = Common.Web3Helper.GetNowBlockNuber(url);

                    MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("连接成功") + ", Last BlockNumber is" + result.ToString(), Share.LanguageHelper.GetTranslationText("测试连接Web3 Url结果"), MessageBoxButton.OK);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK);
                    log.Error("", ex);
                }
                finally
                {
                    Cursor = Cursors.Arrow;
                }
            }
        }


        private async void CloseOnClick(object sender, RoutedEventArgs e)
        {
            if (!IsTest)
            {
                this.Close();
            }
            else
            {
                bool result = await ThisTestWeb3Url();
                if (!result)
                {
                    this.DialogResult = false;
                }

            }
        }

        private void OnUse(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Cursor = Cursors.Wait;
                try
                {
                    var b = ((Button)sender);
                    var s = (StackPanel)b.Parent;
                    var UrlHash = s.Tag.ToString();

                    BLL.Web3Url.UpdateIsSelected(UrlHash);

                    DataGridWeb3Url.ItemsSource = BLL.Web3Url.GetUrlDt().DefaultView;
                    var web3url = BLL.Web3Url.GetCurWeb3UrlModel(false).Url;      //  ShareParam.Web3Url;// 
                    LabelWeb3Url.Content = web3url;

                    //更改 web3    
                    Share.BLL.Web3Url.GetCurWeb3UrlModel(false);                // todo: 这个写法很恶心，先暂时这么写！

                    MessageBox.Show("OK");
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(this,ex.Message, "Error");
                }
                finally
                {
                    Cursor = Cursors.Arrow;
                }
            }
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Cursor = Cursors.Wait;
                try
                {

                    var b = ((Button)sender);
                    var s = (StackPanel)b.Parent;
                    var UrlHash = s.Tag.ToString();

                    DAL.Web3Url.Delete(Share.ShareParam.DbConStr, UrlHash);

                    DataGridWeb3Url.ItemsSource = BLL.Web3Url.GetUrlDt().DefaultView;

                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                }
                finally
                {
                    Cursor = Cursors.Arrow;
                }
            }
        }

        private void OnAddWeb3Url(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                Model.Web3Url model = new Model.Web3Url();
                model.Url = TextBoxUrl.Text;
                model.UrlHash = Common.Security.Tools.GetHashValue(model.Url);
                model.UrlAlias = TextBoxAlias.Text.Trim();
                model.IsSelected = false;
                model.ValidateEmptyAndLen();
                DAL.Web3Url.Insert(Share.ShareParam.DbConStr, model);

                DataGridWeb3Url.ItemsSource = BLL.Web3Url.GetUrlDt().DefaultView;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        private void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsTest)
            {
                TabControlMain.SelectedIndex = 0;
            }
        }

        private bool IsTest = false;

        public static bool TestWeb3(string _web3url)
        {
            //测试web3连接  todo
            if (Common.Web3Helper.TestWeb3Url(_web3url))
            {
                return true;
            }

            WindowWeb3Test f = new WindowWeb3Test();
            f.IsTest = true;

            var result = f.ShowDialog() == true;
            return result;
        }

        private void OnUrlClick(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink)
            {
                var hl = (sender as Hyperlink);
                System.Diagnostics.Process.Start("explorer.exe", hl.NavigateUri.AbsoluteUri);
            }
        }


        private void OnSelectEtherSacn(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                RadioButton r = (RadioButton)sender;
                if (r.Tag.ToString() == "Customize") {
                    //Share.ShareParam.EtherSacn = TextBoxCustomize.Text.Trim();
                    //LabelEthExplore.Content = "Current Block Chain Browser : " + r.Tag.ToString() + " - " + Share.ShareParam.EtherSacn;     // Properties.Settings.Default.EtherSacn;
                }
                else
                {
                    Share.ShareParam.EtherSacn = r.Content.ToString();                              // @"https://etherscan.io/";            //   Properties.Settings.Default.EtherSacn 
                    LabelEthExplore.Content = "Current Block Chain Browser : " + r.Tag.ToString() + " - " + Share.ShareParam.EtherSacn;     // Properties.Settings.Default.EtherSacn;
                }
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Share.ShareParam.EtherSacn = TextBoxCustomize.Text.Trim();
            LabelEthExplore.Content = "Current Block Chain Browser : Customize - " + Share.ShareParam.EtherSacn;     // Properties.Settings.Default.EtherSacn;
        }
    }
}
