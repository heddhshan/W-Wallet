using BlockChain.Share;
using BlockChain.Wallet.BLL;
using log4net.Filter;
using Microsoft.Win32;
using NBitcoin;
using Nethereum.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
//using System.Windows.Forms;
//using System.Drawing;
////using System.Windows.Forms;
//using System.Windows.Input;
using Cursors = System.Windows.Input.Cursors;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace BlockChain.Wallet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, Share.IMainWindow
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IMainWindow

        public void UpdateNodity(string info)
        {
            UcStatusBar.ShowInfo(info);
        }

        public void ShowProcessing(string msg = "Processing")
        {
            UcStatusBar.ShowProcessing(msg);
        }

        public void ShowProcesed(string msg = "Ready")
        {
            UcStatusBar.ShowProcesed(msg);
        }

        public void UpdateStartStatus()
        {
            this.Cursor = Cursors.Wait;
            ShowProcessing();
        }

        public void UpdateFinalStatus()
        {
            ShowProcesed();
            Cursor = Cursors.Arrow;
        }


        public Share.MainWindowHelper GetHelper()
        {
            return Helper;
        }

        private bool _IsRunning = true;

        public bool GetIsRunning()
        {
            return _IsRunning;
        }

        #endregion

        private Share.MainWindowHelper Helper;

        //public static System.Numerics.BigInteger GetBlcockNow()
        //{
        //    try
        //    {
        //        return Share.Contract.Tools.GetBlcockNow();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetBlcockNow", ex);
        //        return 0;
        //    }
        //}

        #region notifyIcon  https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.forms.notifyicon.-ctor?f1url=%3FappId%3DDev16IDEF1%26l%3DZH-CN%26k%3Dk(System.Windows.Forms.NotifyIcon.%2523ctor)%3Bk(DevLang-csharp)%26rd%3Dtrue&view=windowsdesktop-6.0
        ////// 注意： net6 wpf 不支持 托盘图标（net6 考虑跨平台），下面这些代码是 netframwork4.8 的， 无法运用的，需要第三方代码库， 可以参考： https://github.com/MV10/WPF.NotifyIcon.NetCore3  但我觉得这个功能微软应该做，此程序暂时不处理此功能！
        //////              微软的类库可以对各个平台都增加一个 AddOne.Dll, 例如 WindowsAddOne.Dll 。

        //private System.Windows.Forms.NotifyIcon notifyIcon;

        //private void Notify_Show(object sender, EventArgs e)
        //{
        //    this.Visibility = System.Windows.Visibility.Visible;
        //    this.ShowInTaskbar = true;
        //    this.WindowState = WindowState.Normal;
        //    this.Activate();

        //    Process procCurrent = Process.GetCurrentProcess();
        //    System.Threading.Thread.Sleep(1);
        //    Process.Start(procCurrent.ProcessName);                 //另外启动一个线程，把当前线程窗口置顶。
        //}

        //private void Notify_Hide(object sender, EventArgs e)
        //{
        //    this.ShowInTaskbar = false;
        //    this.Visibility = System.Windows.Visibility.Hidden;
        //}

        //private void Notify_Close(object sender, EventArgs e)
        //{
        //    System.Windows.Application.Current.Shutdown();
        //}

        #endregion


        public MainWindow()
        {
            InitializeComponent();


            #region notify

            //this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            //this.notifyIcon.BalloonTipText = "GoldCity";
            //this.notifyIcon.ShowBalloonTip(2000);
            //this.notifyIcon.Text = "GoldCity";
            //this.notifyIcon.Icon = new System.Drawing.Icon(@"favicon.ico");
            //this.notifyIcon.Visible = true;
            ////打开菜单项
            //System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("Open");
            //open.Click += new EventHandler(Notify_Show);

            ////退出菜单项
            //System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            //exit.Click += new EventHandler(Notify_Close);
            ////关联托盘控件
            //System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            //notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            //{
            //    if (e.Button == System.Windows.Forms.MouseButtons.Left) this.Notify_Show(o, e);
            //});

            #endregion



            this.Title = Share.LanguageHelper.GetTranslationText(this.Title);
            Helper = new Share.MainWindowHelper(this);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ShowProcessing();
            try
            {
                WindowPositionHelperConfig.SetSize(this);
                log.Info("Window_Loaded");

                await UcStatusBar.UpdateInfo();             // 慢，影响界面显示

                await QueryAddress();

                DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
                CheckBoxTokenRun.IsChecked = SystemParam.IsUpdateBalanceByBackground;
                ButtonAddressFilter.Tag = TextBoxAddressFilter.Text.Trim();                 //查询条件保存下来，只有点击了这个按钮才能变更查询条件！

                UpdateNodity("Version: " + SystemParam.Version);

                await UpdateEthUsdtPrice();

                ShowStablecoin();
                //var pt = Share.BLL.Token.GetPricingToken();
                //if (pt == null)
                //{
                //    LabelStablecoinSymbol.Content = "ETH";
                //}
                //else
                //{
                //    LabelStablecoinSymbol.Content = pt.Symbol;
                //}
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                ShowProcesed();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowPositionHelperConfig.SaveSize(this);
        }

        private void ButtonTransfer_OnClick(object sender, RoutedEventArgs e)
        {
            WindowTransfer.DoTransfer(this);
        }

        private void ButtonToken_OnClick(object sender, RoutedEventArgs e)
        {
            WindowToken.AddToken(this, new BLL.Address());
        }


        //private void ButtonExeOfflineTx_OnClick(object sender, RoutedEventArgs e)
        //{
        //    WindowExeOffTx.ExeOffTx(this);
        //}

        private void OnDeployContract(object sender, RoutedEventArgs e)
        {
            WindowDeployContract f = new WindowDeployContract();
            f.Owner = this;
            f.ShowDialog();
        }

        private void OnExeOfflineTx(object sender, RoutedEventArgs e)
        {
            //执行离线签名
            WindowExeOffTx.ExeOffTx(this);
        }


        private void OnUniswap(object sender, RoutedEventArgs e)
        {
            WindowUniswap.ShowWindowUniswapV2();
        }

        private void ButtonTxExeStatus_OnClick(object sender, RoutedEventArgs e)
        {
            WindowTxStatus.ShowTxStatus(this);
        }

        private void ButtonTools_OnClick(object sender, RoutedEventArgs e)
        {
            if (!MenuItemTools.IsSubmenuOpen)
            {
                MenuItemTools.IsSubmenuOpen = true;
            }
        }

        //private void OnSetParam(object sender, RoutedEventArgs e)
        //{
        //    WindowSetParam.SetParam(this);
        //}

        private void OnAbout(object sender, RoutedEventArgs e)
        {
            WindowAbout.ShowAbout(this);

        }

        private void OnSysExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private int LastTabIndex = -1;

        private async void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var CurrentTabIndex = TabControlMain.SelectedIndex;
            if (CurrentTabIndex == LastTabIndex)
            {
                return;
            }

            else if (CurrentTabIndex == 1)
            {
                if (DataGridAddrBalance.ItemsSource == null)
                {
                    await ShowAddressBalance(1);
                }
            }

            else if (CurrentTabIndex == 2)
            {
                if (DataGridContact.ItemsSource == null)
                {
                    DataGridContact.ItemsSource = BLL.Contact.GetAllContact().DefaultView;
                }
            }

            else if (CurrentTabIndex == 3)
            {
                TabControlMain.SelectedIndex = LastTabIndex;
            }

            LastTabIndex = TabControlMain.SelectedIndex;
        }


        ////////////////////////////////////////////////////////////////////

        private void TextBoxAssAddressFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnClickTokenAddressAmount(null, null);
                e.Handled = true;
            }
        }


        private bool IsDoSysUserBalance = false;

        //刷新： 这个操作执行很慢，不要重复执行
        private async void OnClickTokenAddressAmount(object sender, RoutedEventArgs e)
        {
            if (IsDoSysUserBalance) return;
            IsDoSysUserBalance = true;

            bool doing = BLL.Address.IsSysAllAddressBalanceFlag;                        // 判断后台是否正在执行 更新余额的操作
            if (!doing)
            {
                WindowLoading.ShowLoading(this);
            }
            System.Threading.Thread.Sleep(0);
            var b1 = System.DateTime.Now;
            this.Cursor = Cursors.Wait;
            try
            {
                await BLL.Address.SysAllAddressBalance(false);

                ShowStablecoin();

                RefreshAddressBalanceProcess();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                await ShowAddressBalance(1);        //显示第一页数据

                IsDoSysUserBalance = false;
                Cursor = Cursors.Arrow;
                var b2 = System.DateTime.Now;
                log.Info("Sys Account Balance's Seconds Is " + (b2 - b1).TotalSeconds.ToString());
                if (!doing)
                {
                    WindowLoading.CloseLoading();       //使用大的等待提示窗体！！！
                }
            }
        }

        private void ShowStablecoin()
        {
            var pt = Share.BLL.Token.GetPricingToken();
            if (pt == null)
            {
                LabelStablecoinSymbol.Content = "?ETH";
            }
            else
            {
                LabelStablecoinSymbol.Content = pt.Symbol;
            }
        }

        private void RefreshAddressBalance_Click(object sender, RoutedEventArgs e)
        {
            RefreshAddressBalanceProcess();
        }

        private void ShowEndTime(System.DateTime? dt)
        {
            if (dt == null)
            {
                LabelEndTime.Content = "...";
                LabelEndTime.Foreground = System.Windows.Media.Brushes.Blue;
            }
            else {
                LabelEndTime.Content = dt.ToString();
                if (dt <= System.DateTime.Now)
                {
                    LabelEndTime.Foreground = System.Windows.Media.Brushes.Green;
                }
                else {
                    LabelEndTime.Foreground = System.Windows.Media.Brushes.Red;
                }
            }

        }

        private void RefreshAddressBalanceProcess()
        {
            var thisprocess = BLL.Address.GetSysAddressBalanceProcess();
            LabelAllAddress.Content = thisprocess.AddressNumber;
            //LabelAllAddress.ToolTip = "Start Time: " + thisprocess.StartTime.ToString();
            LabelDoneAddress.Content = thisprocess.DoneNumber;
            LabelDoneAddress.ToolTip = "Last Done Time: " + thisprocess.DoingTime.ToString();

            LabelStartTime.Content = thisprocess.StartTime.ToString();

            if (thisprocess.DoneNumber == 0)
            {
                //LabelEndTime.Content = "";
                ShowEndTime(null);
            }
            else if (thisprocess.AddressNumber == thisprocess.DoneNumber)
            {
                //LabelEndTime.Content = thisprocess.DoingTime.ToString();
                ShowEndTime(thisprocess.DoingTime);
            }
            else if (thisprocess.AddressNumber > thisprocess.DoneNumber)
            {
                //计算预计结束时间
                var dt = (double)(thisprocess.DoingTime - thisprocess.StartTime).TotalSeconds;
                var dc = (double)thisprocess.DoneNumber;
                var p = dt / dc;
                var alltime = (thisprocess.AddressNumber - thisprocess.DoneNumber) * p;
                var endtime = thisprocess.DoingTime.AddSeconds(alltime);
                //if (endtime > System.DateTime.Now)
                //{
                //    LabelEndTime.Content = endtime.ToString();
                //}
                //else
                //{
                //    LabelEndTime.Content = "???";
                //}
                ShowEndTime(endtime);
            }
            else
            {
                log.Error("thisprocess.AddressNumber < thisprocess.DoneNumber???");
                //LabelEndTime.Content = "?";
                ShowEndTime(null);
            }
        }

        private async Task<bool> ShowAddressBalance(int pageIndex)
        {
            this.Cursor = Cursors.Wait;
            ShowProcessing();
            try
            {
                string filterAddress = "%";
                TextBoxAssAddressFilter.Text = TextBoxAssAddressFilter.Text.Trim();
                if (!string.IsNullOrWhiteSpace(TextBoxAssAddressFilter.Text) && TextBoxAssAddressFilter.Text.IsValidEthereumAddressHexFormat())
                {
                    filterAddress = TextBoxAssAddressFilter.Text;
                }
                else
                {
                    TextBoxAssAddressFilter.Text = string.Empty;
                }

                int recordMin = PagerAddressToken.CurrentPageRecNum * (pageIndex - 1);
                int recordMax = PagerAddressToken.CurrentPageRecNum * pageIndex;

                var t = Task.Run(() => { return BLL.Address.GetAllAddressBalance(recordMin, recordMax, filterAddress); });
                var view = await t;

                var tcount = Task.Run(() => { return BLL.Address.GetAllAddressBalanceCount(filterAddress); });

                PagerAddressToken.TotalPage = ((await tcount - 1) / PagerAddressToken.CurrentPageRecNum + 1);
                PagerAddressToken.CurrentPage = pageIndex;

                DataGridAddrBalance.ItemsSource = view;
                DataGridAddrBalance.Tag = view;

                CheckBoxFilterAmount.IsChecked = false;

                return true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
                return false;
            }
            finally
            {
                ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }


        private async void SummyOnExpanded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ShowProcessing();
            try
            {

                var T = Task.Run(() => { return BLL.Address.GetAddressBalanceSummary(); });
                var l = await T;
                DataGridSummy.ItemsSource = l;

                var sa = l.Sum(m => m.StablecoinAmount);
                LabelStableCoinAllAmount.Content = sa;

                ShowStablecoin();

                double d = GridTokenAmount.ActualHeight;            //滚到最下面代码
                ScrollViewerToken.ScrollToVerticalOffset(d);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                ShowProcesed();
            }
        }




        private async void OnNewHDWallet(object sender, RoutedEventArgs e)
        {
            //WindowNewMnemonic mne = new WindowNewMnemonic();
            //mne.ShowDialog();

            bool result = WindowNewMnemonic.NewMne(this);
            if (result)
            {
                DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
                await QueryAddress();
            }

        }

        private async void OnGenAddress(object sender, RoutedEventArgs e)
        {
            if (WindowHDGenAddress.GenAddress(this))
            {
                await QueryAddress();
            }
        }

        private void OnBatchTransfer(object sender, RoutedEventArgs e)
        {
            WindowTransfer.DoTransfer(this, 3);
        }

        private async void OnLoadKeyStore(object sender, RoutedEventArgs e)
        {
            if (WindowLoadKeyStore.LoadKeyStoreFile(this))
            {
                await QueryAddress();
            }
        }

        private async void OnNewKeyStore(object sender, RoutedEventArgs e)
        {
            if (WindowNewKeyStore.NewKeyStoreAddress(this))
            {
                await QueryAddress();
            }
        }

        private void OnAddContact(object sender, RoutedEventArgs e)
        {
            //if (WindowAddContact.AddContact())
            //{
            WindowAddContact.AddContact(this);
            DataGridContact.ItemsSource = BLL.Contact.GetAllContact().DefaultView;
            //}
        }

        private async void OnLoadPrivateKey(object sender, RoutedEventArgs e)
        {
            if (WindowLoadPrivateKey.LoadPrivateKey(this))
            {
                await QueryAddress();
            }
        }

        private void OnChangepassword(object sender, RoutedEventArgs e)
        {
            WindowModifyPassword.ModifyPassword(this);
        }

        private void OnForgetpassword(object sender, RoutedEventArgs e)
        {
            WindowRestorePassword.ModifyPassword(this);
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            if (DataGridAddrBalance != null && DataGridAddrBalance.ItemsSource != null)
            {
                //((System.Data.DataView)(DataGridAddrBalance.ItemsSource)).RowFilter = "Balance <> '0'";
                var list = (List<Model.ViewUserTokenBalance>)(DataGridAddrBalance.ItemsSource);
                DataGridAddrBalance.ItemsSource = null;

                DataGridAddrBalance.ItemsSource = list.Where(b => b.Balance > 0);
                DataGridAddrBalance.Tag = list;
            }
        }

        private void OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (DataGridAddrBalance != null && DataGridAddrBalance.ItemsSource != null && DataGridAddrBalance.Tag != null)
            {
                //((System.Data.DataView)(DataGridAddrBalance.ItemsSource)).RowFilter = string.Empty;

                DataGridAddrBalance.ItemsSource = null;
                DataGridAddrBalance.ItemsSource = (List<Model.ViewUserTokenBalance>)(DataGridAddrBalance.Tag);
            }
        }

        private async void OnAddToken(object sender, RoutedEventArgs e)
        {
            WindowToken.AddToken(this, new BLL.Address());

            await ShowAddressBalance(1);
        }

        private async void OnRefreshBalance(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            ShowProcessing();
            try
            {
                if (sender is MenuItem)
                {
                    var mi = (MenuItem)sender;
                    var p = ((MenuItem)(mi.Parent)).Parent;
                    if (p is Menu)
                    {
                        var me = (Menu)(p);
                        var UserAddress = me.Tag.ToString();
                        var TokenAddress = mi.Tag.ToString();

                        //var result = BLL.Address.UpdateBalance(UserAddress, TokenAddress).Result;   //停在这里了！！！
                        await BLL.Address.UpdateBalance(UserAddress, TokenAddress);                 //这么就OK

                        //log.Info(result);
                        await ShowAddressBalance(1);
                    }
                }
            }
            finally
            {
                ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async void OnLoadHDWallet(object sender, RoutedEventArgs e)
        {
            WindowLoadMnemonic.LoadMne(this);
            DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
            await QueryAddress();
        }



        /// <summary>
        /// 导入无私钥的地址列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoadHDWalletWithoutPrivatekey_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string BackUpPath = Share.ShareParam.BackUpDir;

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = BackUpPath;
                ofd.Filter = "Address Files | *.address | All Files | *.*";

                if (ofd.ShowDialog() == true)
                {
                    string filename = ofd.FileName;

                    string jsonStr = System.IO.File.ReadAllText(filename);
                    var hd = JsonConvert.DeserializeObject<OfflineWallet.DataSig.Model.HDWalletWithoutPrivatekey>(jsonStr);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            Model.HD_Mnemonic hm = new Model.HD_Mnemonic();
                            hm.MneId = hd.MneId;
                            hm.MneAlias = hd.MneAlias;
                            hm.HasPrivatekey = false;

                            hm.CreateTime = System.DateTime.Now;
                            hm.MnePath = "?";
                            hm.MneSource = HDSource.WithoutPrivatekey;
                            hm.MneEncrypted = "?";
                            hm.EncryptedTimes = 0;
                            hm.IsBackUp = true;
                            hm.MneFirstSalt = "?";
                            hm.WordCount = 0;
                            hm.UserPasswordHash = "?";
                            hm.UserPasswordTip = "?";
                            hm.MneHash = "?";

                            DAL.HD_Mnemonic.Insert(ShareParam.DbConStr, hm);

                            foreach (var obj in hd.AddressList)
                            {
                                Model.HD_Address m = new Model.HD_Address();
                                m.MneId = obj.MneId;
                                m.Address = obj.Address;
                                m.AddressAlias = obj.AddressAlias;
                                m.HasPrivatekey = false;

                                m.IsTxAddress = false;
                                m.AddressIndex = 0;
                                m.MneSecondSalt = "?";

                                DAL.HD_Address.Insert(ShareParam.DbConStr, m);
                            }

                            // 没有错误,提交事务
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            //释放资源
                            scope.Dispose();
                        }
                    }

                    // 地址，等都要刷新
                    DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
                    await QueryAddress();
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }


        private void OnShouQRcode(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                var m = (MenuItem)sender;
                if (m.Parent is MenuItem)
                {
                    var p = (MenuItem)m.Parent;
                    string address = p.Tag.ToString();
                    WindowQRcode.ShowQRcode(this, address);
                }
            }
        }

        private void OnShowContactAddress(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                var m = (MenuItem)sender;
                if (m.Parent is MenuItem)
                {
                    var p = (MenuItem)m.Parent;
                    string address = p.Tag.ToString();
                    WindowQRcode.ShowQRcode(this, address);
                }
            }
        }

        private void OnTransfer11(object sender, RoutedEventArgs e)
        {
            WindowTransfer.DoTransfer(this, 0);
        }

        private void OnLoadPrivateKey11(object sender, RoutedEventArgs e)
        {
            var item = DataGridAddress.SelectedItem;
            if (item is System.Data.DataRowView)
            {
                var drv = (System.Data.DataRowView)item;
                string address = drv["Address"].ToString();
                BlockChain.Wallet.WindowExportPrivateKey.ExportPrivateKey(this, address); //导出私钥不需要单独一个窗体，直接输入密码就好，地址可以取到。
            }

        }

        private async void OnModifyAddressAlias(object sender, RoutedEventArgs e)
        {
            var item = DataGridAddress.SelectedItem;
            if (item is System.Data.DataRowView)
            {
                var drv = (System.Data.DataRowView)item;

                string address = drv["Address"].ToString();
                string alias = drv["AddressAlias"].ToString();

                string newalias = Share.WindowModifyAddressAlias.GetUpdateAlias(address, alias, this);

                if (newalias != alias)
                {
                    BLL.Address.UpdateAddressAlias(address, newalias);

                    await QueryAddress(); //还需要处理页面！
                }
            }
        }

        private void OnSeeTxs(object sender, RoutedEventArgs e)
        {
            var item = DataGridAddress.SelectedItem;
            if (item is System.Data.DataRowView)
            {
                var drv = (System.Data.DataRowView)item;

                string address = drv["Address"].ToString();
                string url = Share.ShareParam.GetAddressUrl(address);
                System.Diagnostics.Process.Start("explorer.exe", url);
            }
        }

        private void OnExKeyStore(object sender, RoutedEventArgs e)
        {
            var item = DataGridAddress.SelectedItem;
            if (item is System.Data.DataRowView)
            {
                try
                {
                    var drv = (System.Data.DataRowView)item;
                    string address = drv["Address"].ToString();

                    //var password = WindowPassword.GetPassword(address);
                    string password;
                    if (!WindowPassword.GetPassword(this, address, "Get Password", out password))
                    {
                        return;
                    }

                    //var account = new BLL.Address().GetAccount(address, password); //GetOfflineAccount
                    var account = new BLL.Address().GetOfflineAccount(address, password); //GetOfflineAccount
                    if (null == account)
                    {
                        MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("账号不存在，多半是密码错误导致。"));
                        return;
                    }

                    //var address = account.Address;

                    string privatekey = account.PrivateKey;

                    if (privatekey.Substring(0, 2) == "0x")
                    {
                        privatekey = privatekey.Substring(2);
                    }

                    string ksText;
                    string fileName;
                    var pk = Nethereum.Hex.HexConvertors.Extensions.HexByteConvertorExtensions.HexToByteArray(privatekey);
                    Share.BLL.KeyStore.SaveKeyStoreFile(password, address, pk, out ksText, out fileName);

                    BlockChain.Common.FileHelper.PositionFile(fileName);

                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("导出失败。一般是密码输入错误导致的，详细错误信息是：") + ex.Message, "Message", MessageBoxButton.OK);
                }
            }

        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            //删除联系人 DataGridContact// ContactName, ContactAddress, ContactRemark, CreateTime
            var item = DataGridContact.SelectedItem;
            if (item is System.Data.DataRowView)
            {
                var drv = (System.Data.DataRowView)item;
                string address = drv["ContactAddress"].ToString();

                if (MessageBox.Show(Share.LanguageHelper.GetTranslationText("确定删除此地址吗？") + Environment.NewLine + address, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DAL.Contact.Delete(Share.ShareParam.DbConStr, address);
                    DataGridContact.ItemsSource = BLL.Contact.GetAllContact().DefaultView;
                }
            }
        }

        private void OnExpAddress(object sender, RoutedEventArgs e)
        {
            WindowExportHDAddress.DoEx(this);
        }

        private async void OnDeleteAddress(object sender, RoutedEventArgs e)
        {
            //地址可以重复 NO
            if (sender is MenuItem)
            {
                try
                {
                    var m = (MenuItem)sender;
                    if (m.Parent is MenuItem)
                    {
                        var p = (MenuItem)m.Parent;
                        string address = p.Tag.ToString();
                        var Source = m.Tag.ToString();

                        #region 验证密码

                        try
                        {
                            //var password = WindowPassword.GetPassword("请输入密码");
                            string password;
                            if (!WindowPassword.GetPassword(this, address, Share.LanguageHelper.GetTranslationText("请输入密码"), out password))
                            {
                                return;
                            }

                            var account = new BLL.Address().GetOfflineAccount(address, password);
                            if (null == account)
                            {
                                MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("账号不存在，多半是密码错误导致。"));
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, LanguageHelper.GetTranslationText("输入密码错误，删除失败。异常信息是：") + ex.Message, "Exception", MessageBoxButton.OK);
                            return;
                        }

                        #endregion

                        //if (MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("确认要删除地址该吗？"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        //{
                        if (Source == "KeyStore")
                        {
                            Share.DAL.KeyStore_Address.Delete(Share.ShareParam.DbConStr, address);
                        }
                        else
                        {
                            BlockChain.Wallet.DAL.HD_Address.Delete(Share.ShareParam.DbConStr, address);
                        }
                        BLL.Address.DeleteAllBalance(address);

                        await QueryAddress();

                        //await ShowAddressBalance(0, PagerAddressToken.CurrentPageRecNum);     // no  在另外页面呢 
                        //}
                    }
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(this, ex.Message, "Message", MessageBoxButton.OK);
                }
            }
        }

        private async void OnDeleteMne(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                try
                {
                    var m = (MenuItem)sender;
                    var MneId = Guid.Parse(m.Tag.ToString());
                    var model = BlockChain.Wallet.DAL.HD_Mnemonic.GetModel(Share.ShareParam.DbConStr, MneId);

                    #region 验证密码

                    if (model == null)
                    {
                        MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("本地数据没有这样的助记词和密码对应数据。"), "Message", MessageBoxButton.OK);
                        return;
                    }
                    try
                    {
                        //var Password = WindowPassword.GetPassword("请输入助记词钱包的密码");
                        string password;
                        if (!WindowPassword.GetPassword(this, model.MneAlias, Share.LanguageHelper.GetTranslationText("请输入助记词钱包的密码"), out password))
                        {
                            return;
                        }

                        var UsePassword = Common.PasswordHelper.GetHDRealPassword(model.Salt, password);
                        string Mnemonic = Common.Security.SymmetricalEncrypt.Decrypt(UsePassword, model.MneEncrypted);
                        var words = Mnemonic.Split(' ');//12，15，18，21，24
                        if (words.Length == 12 || words.Length == 15 || words.Length == 18 || words.Length == 21 || words.Length == 24)
                        {
                            //默认15个！
                        }
                        else
                        {
                            throw new Exception(LanguageHelper.GetTranslationText("助记词数量不对。"));
                        }
                        //不需要生成钱包   只需要解密成功就好了。
                        //Nethereum.HdWallet.Wallet w = new Nethereum.HdWallet.Wallet(Mnemonic, "", BLL.HDWallet.MnePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Message", MessageBoxButton.OK);
                        return;
                    }

                    #endregion

                    if (MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("删除该助记词钱包将把其生成的地址全部一同删除。确认要删除该钱包吗？"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        BlockChain.Wallet.DAL.HD_Mnemonic.Delete(Share.ShareParam.DbConStr, MneId);
                        BLL.HDWallet.DeleteAllAddress(MneId);
                        DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;

                        //地址，等都要刷新
                        await QueryAddress();

                        //ShowAddressBalance(0, PagerAddressToken.CurrentPageRecNum);       //不需要！ 在另外一个页面呢！
                    }
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(this, ex.Message, "Message", MessageBoxButton.OK);
                }
            }
        }

        private void OnOpenKeyStore(object sender, RoutedEventArgs e)
        {
            string path = Share.ShareParam.KeystoreDir;
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        //private bool GetDataGridAddrBalanceSort(out int colIndex, out string sortStr, out System.ComponentModel.ListSortDirection sortDirection)
        //{
        //    colIndex = -1;
        //    sortStr = string.Empty;
        //    sortDirection = System.ComponentModel.ListSortDirection.Ascending;

        //    for (int i = 0; i < 4; i++)
        //    {
        //        if (DataGridAddrBalance.Columns[i].SortDirection != null)
        //        {
        //            //一个字符串，该字符串包含后跟“ASC”（升序）或"DESC"（降序）的列名
        //            // view.Sort = "State ASC, ZipCode ASC";

        //            colIndex = i;
        //            sortDirection = (System.ComponentModel.ListSortDirection)DataGridAddrBalance.Columns[i].SortDirection;

        //            if (colIndex == 0)
        //            {
        //                if (DataGridAddrBalance.Columns[i].SortDirection == System.ComponentModel.ListSortDirection.Ascending)
        //                {
        //                    sortStr = "AddressAlias ASC";
        //                }
        //                else
        //                {
        //                    sortStr = "AddressAlias DESC";
        //                }
        //                return true;
        //            }
        //            else if (colIndex == 1)
        //            {
        //                if (DataGridAddrBalance.Columns[i].SortDirection == System.ComponentModel.ListSortDirection.Ascending)
        //                {
        //                    sortStr = "UserAddress ASC";
        //                }
        //                else
        //                {
        //                    sortStr = "UserAddress DESC";
        //                }
        //                return true;
        //            }
        //            else if (colIndex == 2)
        //            {
        //                if (DataGridAddrBalance.Columns[i].SortDirection == System.ComponentModel.ListSortDirection.Ascending)
        //                {
        //                    sortStr = "Symbol ASC";
        //                }
        //                else
        //                {
        //                    sortStr = "Symbol DESC";
        //                }
        //                return true;
        //            }
        //            else if (colIndex == 3)
        //            {
        //                if (DataGridAddrBalance.Columns[i].SortDirection == System.ComponentModel.ListSortDirection.Ascending)
        //                {
        //                    sortStr = "Balance ASC";
        //                }
        //                else
        //                {
        //                    sortStr = "Balance DESC";
        //                }
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        private void OnAccountBackUp(object sender, RoutedEventArgs e)
        {
            try
            {
                //string FileName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BackUp", "T3sAccountBackUp(" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ").xml");
                string FileName = System.IO.Path.Combine(Share.ShareParam.BackUpDir, "WalletAccountBackUp(" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ").xml");

                log.Info("OnAccountBackUp: " + FileName);  //test

                System.Data.DataSet ds = BLL.Address.GetAllAccountInfo();
                ds.WriteXml(FileName, System.Data.XmlWriteMode.WriteSchema);        //必须写入架构，否则读取会出错！

                System.Diagnostics.Process.Start("Explorer", "/select," + FileName);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
            }

            //try
            //{
            //    string BackUpPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BackUp");

            //    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //    dlg.FileName = "Document";
            //    dlg.DefaultExt = ".xml";
            //    dlg.Filter = "Text documents (.xml)|*.xml";
            //    Nullable<bool> result = dlg.ShowDialog();
            //    if (result == true)
            //    {
            //        string FileName = dlg.FileName;

            //        System.Data.DataSet ds = BLL.Address.GetAllAccountInfo();
            //        ds.WriteXml(FileName);

            //        System.Diagnostics.Process.Start("Explorer", "/select," + FileName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error("", ex);
            //    MessageBox.Show(this, ex.Message);
            //}

        }

        private async void OnAccountReStore(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ShowProcessing();

            try
            {
                string BackUpPath = Share.ShareParam.BackUpDir;// System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BackUp");

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = BackUpPath;
                ofd.Filter = "Account File|*.xml|All File|*.*";
                if (ofd.ShowDialog() == true)
                {
                    System.Data.DataSet ds = new System.Data.DataSet();
                    ds.ReadXml(ofd.FileName);

                    var T = Task.Run(() => { return ExeAccountReStore(ds); });       //如果数据量很大，可能花费很长时间！
                    bool result = await T;
                    log.Info("ExeAccountReStore Result: " + result.ToString());

                    DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
                    await QueryAddress();
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private static bool ExeAccountReStore(System.Data.DataSet ds)
        {
            try
            {
                var dt1 = ds.Tables["table"];//.Tables[0];
                var dt2 = ds.Tables["table1"];//.Tables[1];
                var dt3 = ds.Tables["table2"];//.Tables[2];

                //var dt1 = ds.HD_Mnemonic;         //.Tables["table"];//.Tables[0];
                //var dt2 = ds.KeyStore_Address;    //.Tables["table1"];//.Tables[1];
                //var dt3 = ds.HD_Address;          //.Tables["table2"];//.Tables[2];

                ////方法1： TransactionScope(TransactionScopeOption, TimeSpan)                new TransactionScope(TransactionScopeOption.Required, new TimeSpan(1, 0, 0));
                ////事务的好坏： 有事务执行速度更快，要全部执行成功才执行，有可能事务超时所有数据都无法插入；无事务执行一笔是一笔，错的无法执行，但所有数据都可以插入！      暂时不采用有事务，数据量大（10000）的时候时间超过一个小时！
                //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(1, 0, 0)))     
                //{
                    if (dt1 != null)
                    {
                        var l1 = BlockChain.Wallet.Model.HD_Mnemonic.DataTable2List(dt1);// DataHelper.Mnemonic_DataTable2List(dt1);// 
                        foreach (var m in l1)
                        {
                            DAL.HD_Mnemonic.Insert(Share.ShareParam.DbConStr, m);
                        }
                    }

                    if (dt2 != null)
                    {
                        var l2 = Share.Model.KeyStore_Address.DataTable2List(dt2);
                        foreach (var m in l2)
                        {
                            Share.DAL.KeyStore_Address.Insert(Share.ShareParam.DbConStr, m);
                        }
                    }

                    if (dt3 != null)
                    {
                        var l3 = BlockChain.Wallet.Model.HD_Address.DataTable2List(dt3);//DataHelper.HDAddress_DataTable2List(dt3);// 
                        foreach (var m in l3)
                        {
                            BlockChain.Wallet.DAL.HD_Address.Insert(Share.ShareParam.DbConStr, m);
                        }
                    }
                //    scope.Complete();
                //}


                //////方法2：SqlBulkCopy sqlbulkcopy;  大数据效率很高， 但是不支持无格式数据！
                //if (null != dt1)
                //{
                //    var sqlbulkcopy1 = new SqlBulkCopy(Share.ShareParam.DbConStr, SqlBulkCopyOptions.UseInternalTransaction);
                //    sqlbulkcopy1.DestinationTableName = "HD_Mnemonic";
                //    sqlbulkcopy1.WriteToServer(dt1);
                //}

                //if (null != dt2)
                //{
                //    var sqlbulkcopy2 = new SqlBulkCopy(Share.ShareParam.DbConStr, SqlBulkCopyOptions.UseInternalTransaction);
                //    sqlbulkcopy2.DestinationTableName = "KeyStore_Address";
                //    sqlbulkcopy2.WriteToServer(dt2);
                //}

                //if (null != dt3)
                //{
                //    var sqlbulkcopy3 = new SqlBulkCopy(Share.ShareParam.DbConStr, SqlBulkCopyOptions.UseInternalTransaction);
                //    sqlbulkcopy3.DestinationTableName = "HD_Address";
                //    sqlbulkcopy3.WriteToServer(dt3);
                //}

                return true;
            }
            catch (Exception ex)
            {
                log.Error("", ex); return false;
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

        private void OnGotoAddress2(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                if (b.Content != null)
                {
                    var address = b.Tag.ToString();
                    System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(address));
                }
            }
        }

        private void OnGotoTxId(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                if (b.Content != null)
                {
                    var txid = b.Content.ToString();
                    System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetTxUrl(txid));
                }
            }
        }


        ///// <summary>
        ///// xml 只能把所有数据存为string格式，可以采用强类型dataset避免，但强类型dataset太罗嗦，所有不采用。
        ///// </summary>
        //class DataHelper
        //{
        //    public static List<BlockChain.Wallet.Model.HD_Mnemonic> Mnemonic_DataTable2List(System.Data.DataTable dt)
        //    {
        //        List<BlockChain.Wallet.Model.HD_Mnemonic> result = new List<BlockChain.Wallet.Model.HD_Mnemonic>();
        //        foreach (System.Data.DataRow dr in dt.Rows)
        //        {
        //            result.Add(Mnemonic_DataRow2Object(dr));
        //        }
        //        return result;
        //    }

        //    public static BlockChain.Wallet.Model.HD_Mnemonic Mnemonic_DataRow2Object(System.Data.DataRow dr)
        //    {
        //        if (dr == null)
        //        {
        //            return null;
        //        }
        //        BlockChain.Wallet.Model.HD_Mnemonic Obj = new BlockChain.Wallet.Model.HD_Mnemonic();
        //        Obj.MneId = System.Guid.Parse(dr["MneId"].ToString());
        //        Obj.MneAlias = dr["MneAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["MneAlias"]);
        //        Obj.MneEncrypted = dr["MneEncrypted"] == DBNull.Value ? string.Empty : (System.String)(dr["MneEncrypted"]);
        //        Obj.EncryptedTimes = System.Int32.Parse(dr["EncryptedTimes"].ToString());
        //        Obj.MneHash = dr["MneHash"] == DBNull.Value ? string.Empty : (System.String)(dr["MneHash"]);
        //        Obj.WordCount = System.Int32.Parse(dr["WordCount"].ToString());
        //        Obj.MnePath = dr["MnePath"] == DBNull.Value ? string.Empty : (System.String)(dr["MnePath"]);
        //        Obj.Salt = dr["Salt"] == DBNull.Value ? string.Empty : (System.String)(dr["Salt"]);
        //        Obj.UserPasswordHash = dr["UserPasswordHash"] == DBNull.Value ? string.Empty : (System.String)(dr["UserPasswordHash"]);
        //        Obj.UserPasswordTip = dr["UserPasswordTip"] == DBNull.Value ? string.Empty : (System.String)(dr["UserPasswordTip"]);
        //        Obj.MneFirstSalt = dr["MneFirstSalt"] == DBNull.Value ? string.Empty : (System.String)(dr["MneFirstSalt"]);
        //        Obj.MneSource = dr["MneSource"] == DBNull.Value ? 0 : System.Int32.Parse(dr["MneSource"].ToString());
        //        Obj.IsBackUp = dr["IsBackUp"] == DBNull.Value ? false : System.Boolean.Parse(dr["IsBackUp"].ToString());
        //        Obj.CreateTime = dr["CreateTime"] == DBNull.Value ? System.DateTime.Now : System.DateTime.Parse(dr["CreateTime"].ToString());
        //        return Obj;
        //    }

        //    public static List<BlockChain.Wallet.Model.HD_Address> HDAddress_DataTable2List(System.Data.DataTable dt)
        //    {
        //        List<BlockChain.Wallet.Model.HD_Address> result = new List<BlockChain.Wallet.Model.HD_Address>();
        //        foreach (System.Data.DataRow dr in dt.Rows)
        //        {
        //            result.Add(HDAddress_DataRow2Object(dr));
        //        }
        //        return result;
        //    }

        //    public static BlockChain.Wallet.Model.HD_Address HDAddress_DataRow2Object(System.Data.DataRow dr)
        //    {
        //        if (dr == null)
        //        {
        //            return null;
        //        }
        //        BlockChain.Wallet.Model.HD_Address Obj = new BlockChain.Wallet.Model.HD_Address();
        //        Obj.MneId = dr["MneId"] == DBNull.Value ? System.Guid.Empty : System.Guid.Parse(dr["MneId"].ToString());
        //        Obj.MneSecondSalt = dr["MneSecondSalt"] == DBNull.Value ? string.Empty : (System.String)(dr["MneSecondSalt"]);
        //        Obj.AddressIndex = dr["AddressIndex"] == DBNull.Value ? 0 : System.Int32.Parse(dr["AddressIndex"].ToString());
        //        Obj.AddressAlias = dr["AddressAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["AddressAlias"]);
        //        Obj.Address = dr["Address"] == DBNull.Value ? string.Empty : (System.String)(dr["Address"]);
        //        return Obj;
        //    }

        //}


        private void CopyAddressOnClick(object sender, RoutedEventArgs e)
        {
            var m = sender as MenuItem;
            if (m == null) return;
            var p = m.Parent as ContextMenu;
            if (p == null) return;
            var b = p.PlacementTarget as Button;
            if (b == null) return;
            var address = b.Content.ToString();
            if (string.IsNullOrEmpty(address)) return;

            Clipboard.SetText(address);
        }

        private void ViewTxOnClick(object sender, RoutedEventArgs e)
        {
            var m = sender as MenuItem;
            if (m == null) return;
            var p = m.Parent as ContextMenu;
            if (p == null) return;
            var b = p.PlacementTarget as Button;
            if (b == null) return;
            var address = b.Content.ToString();
            if (string.IsNullOrEmpty(address)) return;

            System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(address));
        }

        private void OnSendMessage(object sender, RoutedEventArgs e)
        {
            var w = new WindowMessage();
            w.Owner = this;
            w.ShowDialog();
        }

        private async void AddressFilterOnClick(object sender, RoutedEventArgs e)
        {
            ButtonAddressFilter.Tag = TextBoxAddressFilter.Text.Trim();                 //查询条件保存下来，只有点击了这个按钮才能变更查询条件！
            await QueryAddress();
        }

        private void TextBoxAddressFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                AddressFilterOnClick(null, null);
                e.Handled = true;
            }
        }

        private async Task<bool> QueryAddress()
        {
            this.Cursor = Cursors.Wait;
            ShowProcessing();
            try
            {
                if (ButtonAddressFilter.Tag == null) { TextBoxAddressFilter.Text = String.Empty; }
                else
                {
                    TextBoxAddressFilter.Text = ButtonAddressFilter.Tag.ToString();
                }
                string filter = TextBoxAddressFilter.Text.Trim();

                int RecordMin = 0;
                int RecordMax = PagerAddress.CurrentPageRecNum;

                var T1 = Task.Run(() => { return BLL.Address.GetRealAllAddressCount(filter); });
                var count = await T1;
                PagerAddress.TotalPage = ((count - 1) / RecordMax + 1);              //页面数量 = （记录条数 - 1） / 每页数量 + 1 ；
                PagerAddress.CurrentPage = 1;

                var T2 = Task.Run(() =>
                {
                    return BLL.Address.GetRealAllAddress(filter, RecordMin, RecordMax).DefaultView;
                });
                DataGridAddress.ItemsSource = await T2;

                return true;

            }
            //catch (Exception ex)
            //{
            //    log.Error("*", ex);
            //    MessageBox.Show(this, ex.Message);
            //}
            finally
            {
                ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }


        /// <summary>
        /// 查询地址列表，  _page 从 1 开始
        /// </summary>
        /// <param name="_page"></param>
        /// <returns></returns>
        private async Task<bool> QueryAddress(int _page)
        {
            this.Cursor = Cursors.Wait;
            ShowProcessing();
            try
            {
                TextBoxAddressFilter.Text = ButtonAddressFilter.Tag.ToString();
                string filter = TextBoxAddressFilter.Text.Trim();
                int PageRecNum = PagerAddress.CurrentPageRecNum;
                int RecordMin = (_page - 1) * PageRecNum;
                int RecordMax = _page * PageRecNum;

                PagerAddress.CurrentPage = _page;

                var T2 = Task.Run(() =>
                {
                    return BLL.Address.GetRealAllAddress(filter, RecordMin, RecordMax).DefaultView;
                });
                DataGridAddress.ItemsSource = await T2;

                return true;
            }
            //catch (Exception ex)
            //{
            //    log.Error("*", ex);
            //    MessageBox.Show(this, ex.Message);
            //}
            finally
            {
                ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async void PagerAddress_FirstPage(object sender, RoutedEventArgs e)
        {
            //TextBoxAddressFilter.Text = ButtonAddressFilter.Tag.ToString();
            await QueryAddress(1);
        }

        private async void PagerAddress_LastPage(object sender, RoutedEventArgs e)
        {
            //TextBoxAddressFilter.Text = ButtonAddressFilter.Tag.ToString();
            await QueryAddress(PagerAddress.TotalPage);
        }

        private async void PagerAddress_NextPage(object sender, RoutedEventArgs e)
        {
            //TextBoxAddressFilter.Text = ButtonAddressFilter.Tag.ToString();
            var page = 1 + PagerAddress.CurrentPage;
            if (page <= PagerAddress.TotalPage)
            {
                await QueryAddress(page);
            }
        }


        private async void PagerAddress_PreviousPage(object sender, RoutedEventArgs e)
        {
            //TextBoxAddressFilter.Text = ButtonAddressFilter.Tag.ToString();
            int page = PagerAddress.CurrentPage - 1;
            if (page < 1) { page = 1; }
            await QueryAddress(page);
        }
        private async void PagerAddress_PageRecNumChanged(object sender, RoutedEventArgs e)
        {
            //TextBoxAddressFilter.Text = ButtonAddressFilter.Tag.ToString();
            await QueryAddress();
        }

        private void IsTxAddress_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                var ck = sender as CheckBox;
                var Address = ck.Tag.ToString();
                BLL.Address.UpdateIsTxAddress(Address, true);
            }
        }

        private void IsTxAddress_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                var ck = sender as CheckBox;
                var Address = ck.Tag.ToString();
                BLL.Address.UpdateIsTxAddress(Address, false);
            }
        }

        private void CheckBoxTokenRun_Checked(object sender, RoutedEventArgs e)
        {
            // 启动后台线程！
            SystemParam.IsUpdateBalanceByBackground = true;
        }

        private void CheckBoxTokenRun_Unchecked(object sender, RoutedEventArgs e)
        {
            // 启动后台线程！
            SystemParam.IsUpdateBalanceByBackground = false;
        }

        private async void PagerAddressToken_PreviousPage(object sender, RoutedEventArgs e)
        {
            int page = PagerAddressToken.CurrentPage - 1;
            if (page < 1) { page = 1; }
            await ShowAddressBalance(page);
        }

        private async void PagerAddressToken_PageRecNumChanged(object sender, RoutedEventArgs e)
        {
            int page = PagerAddressToken.CurrentPage - 1;
            if (page < 1) { page = 1; }
            await ShowAddressBalance(page);
        }

        private async void PagerAddressToken_NextPage(object sender, RoutedEventArgs e)
        {
            var page = 1 + PagerAddressToken.CurrentPage;
            if (page <= PagerAddressToken.TotalPage)
            {
                await ShowAddressBalance(page);
            }
        }

        private async void PagerAddressToken_LastPage(object sender, RoutedEventArgs e)
        {
            await ShowAddressBalance(PagerAddressToken.TotalPage);
        }

        private async void PagerAddressToken_FirstPage(object sender, RoutedEventArgs e)
        {
            await ShowAddressBalance(1);
        }



        //////////////////////////////////////////////////////////////////// 
        //////////////////////////////////////////////////////////////////// AddressFilterOnClick
        ///


        private void ButtonGameList_OnClick(object sender, RoutedEventArgs e)
        {
            WindowAppInfo f = new WindowAppInfo();
            f.Owner = this;
            f.ShowDialog();
        }

        private void OnWeb3Url(object sender, RoutedEventArgs e)
        {
            Share.WindowWeb3Test f = new WindowWeb3Test();
            f.Owner = this;
            f.ShowDialog();
            MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("如果修改了Web3链接，请重启。"), "Message", MessageBoxButton.OK);
        }

        private void OnDataBase(object sender, RoutedEventArgs e)
        {
            Share.WindowDbSet f = new WindowDbSet();
            f.Owner = this;
            f.ShowDialog();

            MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("如果修改了数据库链接，请重启。"), "Message", MessageBoxButton.OK);
        }

        private void OnLanguage(object sender, RoutedEventArgs e)
        {
            Share.WindowLanguage f = new WindowLanguage();
            f.Owner = this;
            f.ShowDialog();

            MessageBox.Show(this, Share.LanguageHelper.GetTranslationText("如果改变了语言，请重启。"), "Message", MessageBoxButton.OK);
        }

        private void OnNntTransfer(object sender, RoutedEventArgs e)
        {
            WindowNft f = new WindowNft();
            f.Owner = this;
            f.ShowDialog();
        }

        private void OnNotice(object sender, RoutedEventArgs e)
        {
            Share.WindowNotice.ShowWindowNotice(this, SystemParam.AppId, new BlockChain.Wallet.BLL.Address());
        }

        private async void EthUsdtPrice_Click(object sender, RoutedEventArgs e)
        {
            await UpdateEthUsdtPrice();
        }

        private async Task<bool> UpdateEthUsdtPrice()
        {
            LabelEthUsdtPrice.Content = "?";
            ButtonPrice.ToolTip = "";
            LabelStablecoin.Content = "? Stable Coin";

            this.Cursor = Cursors.Wait;
            ShowProcessing();

            try
            {
                string ETH = Share.ShareParam.AddressEth;
                //string Stablecoin = "0xdAC17F958D2ee523a2206206994597C13D831ec7";  // 主网 usdt  for test
                string Stablecoin = ShareParam.AddressPricingToken;

                if (Stablecoin == ShareParam.EmptyAddress)
                {
                    LabelStablecoin.Content = "ETH";
                }
                else
                {
                    LabelStablecoin.Content = Share.BLL.Token.GetModel(Stablecoin).Symbol;
                }

                var price = await Share.UniswapTokenPrice.getPrice(ETH, Stablecoin);

                LabelEthUsdtPrice.Content = price.ToString("N6");

                ButtonPrice.ToolTip = "Update Time is " + System.DateTime.Now.ToString();
                return true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return false;
            }
            finally
            {
                ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void OnExpSigData(object sender, RoutedEventArgs e)
        {
            //处理导出转账的 数据，用于签名！  多个地址 给一个地址转账
            WindowBatchTransferOffSig f = new WindowBatchTransferOffSig();
            f.Owner = this;
            f.ShowDialog();
        }

        private void ButtonSigData_OnClick(object sender, RoutedEventArgs e)
        {
            WindowBatchTransferOffSig f = new WindowBatchTransferOffSig();
            f.Owner = this;
            f.ShowDialog();
        }

        private void OnExeTools(object sender, RoutedEventArgs e)
        {
            WindowTools.ExeTools(this);
        }

        private void OnCalTools(object sender, RoutedEventArgs e)
        {
            Share.WindowCalculateTools.ExeWindowCalculateTools(this);
        }

        private void OnLido(object sender, RoutedEventArgs e)
        {
            WindowLido f = new WindowLido();
            f.Owner = this;
            f.ShowDialog();
        }




        //private void TextBoxAddressFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{

        //}
    }
}
