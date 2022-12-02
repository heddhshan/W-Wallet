using BlockChain.Share;
using BlockChain.Share.BLL;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using Windows.System;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowToken.xaml 的交互逻辑
    /// </summary>
    public partial class WindowToken : Window
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //[DllImport("user32.dll")]
        //static extern IntPtr PostMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
        ////       private IntPtr ThisHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)


        private const string PriceRemark = @"
说明：
    1，ETH并不是ERC20代币，本系统中为了统一标识，使用地址0（0x0000000000000000000000000000000000000000）代表ETH，同时代码做了例外处理。
    2，Token的浮动价格是通过Uniswap获取，直接寻找该Token和PricingToken这两个Token交易对（池）的价格(没有通过交易对路由查询)，但有可能没有这样的交易对（池），或者流动性很少，就会导致价格无法查询（-1）或者价格错误；可以通过使用固定价格来替换浮动价格。
";

        public WindowToken()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;
            //DataGridTokenDataBind();
            HyperlinkTokenListUrl.NavigateUri = new Uri(ShareParam.GetErc20TokenListUrl());

            TextBlockPriceRemark.Text = LanguageHelper.GetTranslationText(PriceRemark);

            SetPricingFixed();
        }


        //private bool IsDataGridTokenDataBind = false;       //这种写法没用！

        //private void DataGridTokenDataBind()                //这种写法没用！
        //{
        //    IsDataGridTokenDataBind = true;
        //    try
        //    {
        //        DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;
        //        System.Threading.Thread.Sleep(1);
        //    }
        //    finally
        //    {
        //        IsDataGridTokenDataBind = false;
        //    }
        //}


        private void OnAddToken(object sender, RoutedEventArgs e)
        {
            try
            {
                // todo: 
                //if (!BLL.Web3Url.Web3IsConnected)
                //{
                //    if (MessageBox.Show(this, LanguageHelper.GetTranslationText("好像没有连接到以太坊节点（Web3），如果继续执行可能会失败。确认要继续执行吗"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.No)
                //    {
                //        return;
                //    }
                //}

                string address = this.TextBoxTokenAddress.Text.Trim();
                string imagePath = this.TextBoxTokenImagePath.Text;

                string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;

                string ToImageFile = System.IO.Path.Combine(BasePath, "TokenIcon", System.IO.Path.GetFileName(imagePath));    //重写FileName路径，默认就是程序路径
                try
                {
                    ////if (!System.IO.Directory.Exists(Share.ShareParam.ImageDir))
                    ////{
                    ////    System.IO.Directory.CreateDirectory(Share.ShareParam.ImageDir);
                    ////}
                    System.IO.File.Copy(imagePath, ToImageFile);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show (this, "复制图像文件出错，详细信息是：" + ex.Message, "Message", MessageBoxButton.OK);
                    //return;
                    log.Error(LanguageHelper.GetTranslationText("复制图像文件出错"), ex);
                }

                string filename = System.IO.Path.GetFileName(ToImageFile);  //保存相对路径的文件名
                string savename = @"TokenIcon\" + filename;

                bool isFixedPrice = CheckBoxPricingFixed.IsChecked == true;
                string priceToken = string.Empty;
                decimal price = 0;
                if (isFixedPrice)
                {
                    priceToken = TextBoxPricingToken.Text;
                    price = decimal.Parse(TextBoxPricingAmount.Text);
                }


                ButtonToken.IsEnabled = false;

                SaveToken(address, savename, isFixedPrice, priceToken, price);

                //DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;

                ////广播消息，主要是通知主窗体，token变了！
                //var hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
                //PostMessage(hwnd, (int)MessageId.WM_Token, (IntPtr)0, (IntPtr)0);

            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message, "Error");
            }
        }

        public async void SaveToken(string address, string imagePath, bool isFixedPrice, string priceToken, decimal price)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            try
            {
                var result = await BLL.Token.SaveTokenData(address, imagePath, true, isFixedPrice, priceToken, price);

                ////更新显示数据  其实不需要，自己手动刷新去
                //DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;
                ////DataGridTokenDataBind();

                //清空输入框
                this.TextBoxTokenAddress.Text = string.Empty;
                this.TextBoxTokenImagePath.Text = string.Empty;

                MessageBox.Show(this, "Save Token Result Is " + result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK);
            }
            finally
            {
                ButtonToken.IsEnabled = true;
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void OnSelectImage(object sender, RoutedEventArgs e)
        {
            //string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;//System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase; //
            //string ThisPath = System.IO.Path.Combine(BasePath, "TokenIcon");

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".png";
            ofd.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            //ofd.InitialDirectory = ThisPath;              //不需要！
            if (ofd.ShowDialog() == true)
            {
                TextBoxTokenImagePath.Text = ofd.FileName;
            }
        }

        private void OnSelectToken(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        //private void OnChecked(object sender, RoutedEventArgs e)
        //{
        //    SetTokenIsSelected(sender);
        //}

        //private void OnUnchecked(object sender, RoutedEventArgs e)
        //{
        //    SetTokenIsSelected(sender);
        //}

        //private static void SetTokenIsSelected(object sender)
        //{
        //    //<CheckBox IsChecked="{Binding Path=IsSelected}" Tag="{Binding Path=Address}" Checked="OnChecked" Unchecked="OnUnchecked" HorizontalAlignment="Center" VerticalAlignment="Center"></CheckBox>
        //    if (sender is CheckBox)
        //    {
        //        CheckBox c = (CheckBox)sender;
        //        string Address = c.Tag.ToString();
        //        bool IsSelected = c.IsChecked == true;

        //        BLL.Token.Update(Address, IsSelected);
        //    }
        //}

        private void OnOpenErc20TokenPage(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            //Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
            System.Diagnostics.Process.Start("explorer.exe", link.NavigateUri.AbsoluteUri);

        }

        private void OnDelToken(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                string tokenaddress = b.Tag.ToString();

                //这几种特殊token不允许删除
                if (tokenaddress == "0x0" || ShareParam.IsAnEmptyAddress(tokenaddress))
                {
                    return;
                }


                if (MessageBox.Show(this, LanguageHelper.GetTranslationText("确信要删除这个Token吗？"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var chainid = (int)Share.ShareParam.GetChainId();
                    DAL.Token.Delete(Share.ShareParam.DbConStr, chainid, tokenaddress);
                    DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;
                    //DataGridTokenDataBind();
                }
            }
        }

        private async void OnIniToken(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            WindowLoading.ShowLoading(this);
            try
            {
                var chainid = (int)Share.ShareParam.GetChainId();
                //var t = Task.Run(() => { return BLL.Token.IniTokenData(chainid); });
                //await t;
                await BLL.Token.IniTokenData(chainid);

                //System.Threading.Thread.Sleep(1);
                DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;
                //DataGridTokenDataBind();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message, "Exception");
            }
            finally
            {
                WindowLoading.CloseLoading();
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }

        }

        private void TokenListRefesh_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            try
            {
                DataGridToken.ItemsSource = null;
                System.Threading.Thread.Sleep(1);
                DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;
            }

            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message, "Exception");
            }
            finally
            {
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async void ThisTokenPrice_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                string Token = b.Tag.ToString();

                this.Cursor = Cursors.Wait;
                ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
                WindowLoading.ShowLoading(this);
                try
                {
                    await Share.BLL.Token.UpdateOneTokenStablecoinPrice(Token);
                    DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;
                    //DataGridTokenDataBind();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK);
                }
                finally
                {
                    WindowLoading.CloseLoading();
                    ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                    Cursor = Cursors.Arrow;
                }

            }
        }

        private void OnGotoAddress(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                if (b.Content != null)
                {
                    var address = b.Content.ToString();
                    System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(address));
                }
            }
        }

        //private bool IsDoingPrice = false;

        ///// <summary>
        ///// 设定 Pricing Token ，当数据绑定的时候也会执行，但不应该，会造成死循环！
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private async void IsPricingToken_Checked(object sender, RoutedEventArgs e)
        //{
        //    // IsDataGridTokenDataBind 不起作用！
        //    if (!IsDataGridTokenDataBind && sender is CheckBox && !IsDoingPrice)
        //    {
        //        IsDoingPrice = true;
        //        this.Cursor = Cursors.Wait;
        //        ((IMainWindow)Application.Current.MainWindow).ShowProcessing();

        //        try
        //        {
        //            var ck = (sender as CheckBox);
        //            if (ck.IsChecked == true)
        //            {
        //                string token = ck.Tag.ToString();
        //                BLL.Token.SetIsPricingToken(token);

        //                await Share.BLL.Token.UpdateTokenStablecoinPrice();
        //                //DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;      //循环打开，循环执行  IsPricingToken_Checked
        //                DataGridTokenDataBind();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error("", ex);
        //            MessageBox.Show(ex.Message);
        //        }
        //        finally
        //        {
        //            IsDoingPrice = false;
        //            ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
        //            Cursor = Cursors.Arrow;
        //        }
        //    }
        //}


        private async void Pticing_Click(object sender, RoutedEventArgs e)
        {
            // IsDataGridTokenDataBind 不起作用！
            //if (!IsDataGridTokenDataBind && sender is CheckBox && !IsDoingPrice)
            //{
            //IsDoingPrice = true;
            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            WindowLoading.ShowLoading(this);
            try
            {
                var ck = (sender as Button);

                string token = ck.Tag.ToString();
                BLL.Token.SetIsPricingToken(token);

                await Share.BLL.Token.UpdateTokenStablecoinPrice();
                DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;      //循环打开，循环执行  IsPricingToken_Checked
                //DataGridTokenDataBind();

            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                WindowLoading.CloseLoading();
                //IsDoingPrice = false;
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
            //}

        }

        private void FixedPrice_Checked(object sender, RoutedEventArgs e)
        {
            SetPricingFixed();
        }

        private void SetPricingFixed()
        {
            TextBoxPricingToken.IsEnabled = CheckBoxPricingFixed.IsChecked == true;
            TextBoxPricingAmount.IsEnabled = CheckBoxPricingFixed.IsChecked == true;
            if (!TextBoxPricingToken.IsEnabled)
            {
                TextBoxPricingToken.Text = string.Empty;
                TextBoxPricingAmount.Text = string.Empty;
            }
        }

        private void FixedPrice_UnChecked(object sender, RoutedEventArgs e)
        {
            SetPricingFixed();
        }


        private async void PriceRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            WindowLoading.ShowLoading(this);
            try
            {
                await Share.BLL.Token.UpdateTokenStablecoinPrice();
                DataGridToken.ItemsSource = BLL.Token.GetAllToken().DefaultView;
                //DataGridTokenDataBind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK);
            }
            finally
            {
                WindowLoading.CloseLoading();
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async void RefreshApprove_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            try
            {
                bool ShowAll = CheckBoxShowAll.IsChecked == true;
                DataGridUserTokenApprove.ItemsSource = null;

                var web3 = Share.ShareParam.GetWeb3(Share.ShareParam.Web3Url);

                var AL = BLL.UserTokenApprove.GetAllUserTokenApprove(true);
                foreach (var m in AL)
                {
                    Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, m.TokenAddress);
                    var AllowanceAmount = await service.AllowanceQueryAsync(m.UserAddress, m.SpenderAddress);
                    m.CurrentAmount = (double)AllowanceAmount / Math.Pow(10, m.TokenDecimals);
                    DAL.UserTokenApprove.Update(Share.ShareParam.DbConStr, m);
                }

                if (ShowAll)
                {
                    DataGridUserTokenApprove.ItemsSource = AL;
                }
                else
                {
                    DataGridUserTokenApprove.ItemsSource = AL.Where(x => x.CurrentAmount > 0);
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }


        private Share.IAddress AddressImp;

        private async void CancelApprove_Click(object sender, RoutedEventArgs e)
        {
            //Model.UserTokenApprove
            if (DataGridUserTokenApprove.SelectedItem is Model.UserTokenApprove)
            {
                this.Cursor = Cursors.Wait;
                ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
                try
                {
                    var m = DataGridUserTokenApprove.SelectedItem as Model.UserTokenApprove;
                    string UserAddress = m.UserAddress;
                    string SpenderAddress = m.SpenderAddress;
                    string TokenAddress = m.TokenAddress;

                    var result = await UiCancelApprove(UserAddress, SpenderAddress, TokenAddress);
                    if (result)
                    {
                        BLL.UserTokenApprove.WriteCancleApprove(m.TransactionHash);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                    Cursor = Cursors.Arrow;
                }
            }
        }

        private async Task<bool> UiCancelApprove(string UserAddress, string SpenderAddress, string TokenAddress)
        {
            string password;
            if (!WindowPassword.GetPassword(this, UserAddress, "Get Password", out password))
            {
                return false;
            }
            var ThisAccount = AddressImp.GetAccount(UserAddress, password);
            if (ThisAccount == null) { return false; }

            var web3 = Share.ShareParam.GetWeb3(ThisAccount, Share.ShareParam.Web3Url);

            Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, TokenAddress);
            var AllowanceAmount = await service.AllowanceQueryAsync(UserAddress, SpenderAddress);
            if (AllowanceAmount > 0)
            {
                var tx = await service.ApproveRequestAsync(SpenderAddress, 0);
                Share.BLL.TransactionReceipt.LogTx(UserAddress, tx, "Approve Zero(Cancel Approve)", "Cancel Approve");

                string text = LanguageHelper.GetTranslationText(@"交易已提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }

                //BLL.UserTokenApprove.WriteCancleApprove(OldTx);
                return true;
            }
            else
            {
                MessageBox.Show(LanguageHelper.GetTranslationText("授权金额为0，不需要执行取消操作！"));
                return false;
            }
        }

        private void TokenAddress_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                if (b.Content != null)
                {
                    var address = b.ToolTip.ToString();
                    System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(address));
                }
            }
        }

        private async void QueryAllowance_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
            try
            {
                string User = TextBoxUserAddress3.Text;
                string Spender = TextBoxSpenderAddress3.Text;
                string Token = TextBoxTokenAddress3.Text;

                var web3 = Share.ShareParam.GetWeb3(Share.ShareParam.Web3Url);
                Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, Token);
                var AllowanceAmount = await service.AllowanceQueryAsync(User, Spender);
                var d = BLL.Token.GetTokenDecimals(Token);
                var aa = (double)AllowanceAmount / Math.Pow(10, d);

                TextBoxAllowanceAmount3.Text = aa.ToString("N" + d.ToString());
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async void CancelAllowance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string User = TextBoxUserAddress3.Text;
                string Spender = TextBoxSpenderAddress3.Text;
                string Token = TextBoxTokenAddress3.Text;

                var result = await UiCancelApprove(User, Spender, Token);
                //if (!result) {
                //    MessageBox.Show("Failed");
                //}

                log.Info("CancelApprove Result:" + result.ToString());      //这个日志没有任何作用
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 外部调用，这个名字取得不好
        /// </summary>
        /// <param name="_owner"></param>
        /// <param name="_AddressImp"></param>
        /// <returns></returns>
        public static bool AddToken(Window _owner, Share.IAddress _AddressImp)
        {
            WindowToken w = new WindowToken();
            w.Owner = _owner;
            w.AddressImp = _AddressImp;

            return w.ShowDialog() == true;
        }


    }
}
