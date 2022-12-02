using BlockChain.Share;
using Microsoft.Win32;
using Nethereum.Model;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlockChain.OfflineWallet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, Share.IMainWindow
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IMainWindow


        private static int _Counter = 0;

        private void _ShowProcessing(string msg = "Processing")
        {
            lock (this)
            {
                TextBlockStatus.Text = msg;
                progressBar1.IsIndeterminate = true;
                _Counter = _Counter + 1;
                System.Threading.Thread.Sleep(1);
            }
        }

        private void _ShowProcesed(string msg = "Ready")
        {
            lock (this)
            {
                _Counter = _Counter - 1;
                if (_Counter <= 0)
                {
                    _Counter = 0;
                    TextBlockStatus.Text = msg;
                    progressBar1.IsIndeterminate = false;
                    System.Threading.Thread.Sleep(1);
                }
            }
        }


        public void _ShowInfo(string _Info)
        {
            LabelInfo.Content = _Info;
        }


        public void UpdateNodity(string info)
        {
            _ShowInfo(info);
        }

        public void ShowProcessing(string msg = "Processing")
        {
            _ShowProcessing(msg);
        }

        public void ShowProcesed(string msg = "Ready")
        {
            _ShowProcesed(msg);
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

        public MainWindow()
        {
            InitializeComponent();
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

                await QueryAddress();

                DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
               
                ButtonAddressFilter.Tag = TextBoxAddressFilter.Text.Trim();                 //查询条件保存下来，只有点击了这个按钮才能变更查询条件！
                
                LabelInfo.Content = "Version:" + OffWalletParam.Version;
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


        private int LastTabIndex = -1;

        private void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var CurrentTabIndex = TabControlMain.SelectedIndex;
            if (CurrentTabIndex == LastTabIndex)
            {
                return;
            }

            else if (CurrentTabIndex == 1)
            {
                if (DataGridContact.ItemsSource == null)
                {
                    DataGridContact.ItemsSource = BLL.Contact.GetAllContact().DefaultView;
                }
            }

            else if (CurrentTabIndex == 2)
            {               
                TabControlMain.SelectedIndex = LastTabIndex;
            }
            
            LastTabIndex = TabControlMain.SelectedIndex;
        }



        private async void OnNewHDWallet(object sender, RoutedEventArgs e)
        {
            //WindowNewMnemonic mne = new WindowNewMnemonic();
            //mne.ShowDialog();

            bool result = WindowNewMnemonic.NewMne(this);
            if (result)
            {
                DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
                //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                await QueryAddress();
            }

        }

        private async void OnGenAddress(object sender, RoutedEventArgs e)
        {
            if (WindowHDGenAddress.GenAddress(this))
            {
                //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                await QueryAddress();
            }
        }

        private async void OnLoadHDWallet(object sender, RoutedEventArgs e)
        {
            WindowLoadMnemonic.LoadMne(this);
            DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
            //地址，等都要刷新
            //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
            //ShowAddressBalance();
            await QueryAddress();
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



        private void OnExpAddress(object sender, RoutedEventArgs e)
        {
            WindowExportHDAddress.DoEx(this);
        }


        private void OnChangepassword(object sender, RoutedEventArgs e)
        {
            WindowModifyPassword.ModifyPassword(this);
        }

        private void OnForgetpassword(object sender, RoutedEventArgs e)
        {
            WindowRestorePassword.ModifyPassword(this);
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

                    //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                    await QueryAddress(); //还需要处理页面！
                }
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

                    var account = new BLL.Address().GetAccount(address, password);
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

                            var account = new BLL.Address().GetAccount(address, password);
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
                                BlockChain.OfflineWallet.DAL.HD_Address.Delete(Share.ShareParam.DbConStr, address);
                            }
                            //BLL.Address.DeleteAllBalance(address);                //离线钱包没有 balance 

                            //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                            await QueryAddress();
                          
                        //}
                    }
                }
                catch (Exception ex)
                {
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
                    var model = BlockChain.OfflineWallet.DAL.HD_Mnemonic.GetModel(Share.ShareParam.DbConStr, MneId);

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
                        BlockChain.OfflineWallet.DAL.HD_Mnemonic.Delete(Share.ShareParam.DbConStr, MneId);
                        BLL.HDWallet.DeleteAllAddress(MneId);
                        DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;

                        //地址，等都要刷新
                        //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                        await QueryAddress();
                    
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

        private void OnAccountBackUp(object sender, RoutedEventArgs e)
        {
            try
            {
                //string FileName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BackUp", "T3sAccountBackUp(" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ").xml");
                string FileName = System.IO.Path.Combine(Share.ShareParam.BackUpDir, "OfflienWalletAccountBackUp(" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ").xml");

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
            try
            {
                string BackUpPath = Share.ShareParam.BackUpDir;// System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BackUp");

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = BackUpPath;
                ofd.Filter = "Account File|*.xml|All File|*.*";

                if (ofd.ShowDialog() == true)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        System.Data.DataSet ds = new System.Data.DataSet();
                        //DbScript.DataSetAccount ds = new DbScript.DataSetAccount();
                        ds.ReadXml(ofd.FileName);

                        var dt1 = ds.Tables["table"];//.Tables[0];
                        var dt2 = ds.Tables["table1"];//.Tables[1];
                        var dt3 = ds.Tables["table2"];//.Tables[2];

                        //var dt1 = ds.HD_Mnemonic;//.Tables["table"];//.Tables[0];
                        //var dt2 = ds.KeyStore_Address;//.Tables["table1"];//.Tables[1];
                        //var dt3 = ds.HD_Address;//.Tables["table2"];//.Tables[2];

                        if (dt1 != null)
                        {
                            var l1 = Model.HD_Mnemonic.DataTable2List(dt1);// BlockChain.OfflineWallet.Model.HD_Mnemonic.DataTable2List(dt1);
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
                            var l3 = Model.HD_Address.DataTable2List(dt3);// BlockChain.OfflineWallet.Model.HD_Address.DataTable2List(dt3);
                            foreach (var m in l3)
                            {
                                BlockChain.OfflineWallet.DAL.HD_Address.Insert(Share.ShareParam.DbConStr, m);
                            }
                        }

                        //SqlBulkCopy sqlbulkcopy;
                        //if (null != dt1)
                        //{
                        //    sqlbulkcopy = new SqlBulkCopy(Share. ShareParam.DbConStr, SqlBulkCopyOptions.UseInternalTransaction);
                        //    sqlbulkcopy.DestinationTableName = "HD_Mnemonic";
                        //    sqlbulkcopy.WriteToServer(dt1);
                        //}

                        //if (null != dt2)
                        //{
                        //    sqlbulkcopy = new SqlBulkCopy(Share. ShareParam.DbConStr, SqlBulkCopyOptions.UseInternalTransaction);
                        //    sqlbulkcopy.DestinationTableName = "KeyStore_Address";
                        //    sqlbulkcopy.WriteToServer(dt2);
                        //}

                        //if (null != dt3)
                        //{
                        //    sqlbulkcopy = new SqlBulkCopy(Share. ShareParam.DbConStr, SqlBulkCopyOptions.UseInternalTransaction);
                        //    sqlbulkcopy.DestinationTableName = "HD_Address";
                        //    sqlbulkcopy.WriteToServer(dt3);
                        //}

                        scope.Complete();
                    }

                    DataGridHDWallets.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;

                    //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                    await QueryAddress();
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
            }
        }
        ///// <summary>
        ///// xml 只能把所有数据存为string格式，可以采用强类型dataset避免，但强类型dataset太罗嗦，所有不采用。
        ///// </summary>
        //class DataHelper
        //{
        //    public static List<BlockChain.OfflineWallet.Model.HD_Mnemonic> Mnemonic_DataTable2List(System.Data.DataTable dt)
        //    {
        //        List<BlockChain.OfflineWallet.Model.HD_Mnemonic> result = new List<BlockChain.OfflineWallet.Model.HD_Mnemonic>();
        //        foreach (System.Data.DataRow dr in dt.Rows)
        //        {
        //            result.Add(Mnemonic_DataRow2Object(dr));
        //        }
        //        return result;
        //    }

        //    public static BlockChain.OfflineWallet.Model.HD_Mnemonic Mnemonic_DataRow2Object(System.Data.DataRow dr)
        //    {
        //        if (dr == null)
        //        {
        //            return null;
        //        }
        //        BlockChain.OfflineWallet.Model.HD_Mnemonic Obj = new BlockChain.OfflineWallet.Model.HD_Mnemonic();
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

        //    public static List<BlockChain.OfflineWallet.Model.HD_Address> HDAddress_DataTable2List(System.Data.DataTable dt)
        //    {
        //        List<BlockChain.OfflineWallet.Model.HD_Address> result = new List<BlockChain.OfflineWallet.Model.HD_Address>();
        //        foreach (System.Data.DataRow dr in dt.Rows)
        //        {
        //            result.Add(HDAddress_DataRow2Object(dr));
        //        }
        //        return result;
        //    }

        //    public static BlockChain.OfflineWallet.Model.HD_Address HDAddress_DataRow2Object(System.Data.DataRow dr)
        //    {
        //        if (dr == null)
        //        {
        //            return null;
        //        }
        //        BlockChain.OfflineWallet.Model.HD_Address Obj = new BlockChain.OfflineWallet.Model.HD_Address();
        //        Obj.MneId = dr["MneId"] == DBNull.Value ? System.Guid.Empty : System.Guid.Parse(dr["MneId"].ToString());
        //        Obj.MneSecondSalt = dr["MneSecondSalt"] == DBNull.Value ? string.Empty : (System.String)(dr["MneSecondSalt"]);
        //        Obj.AddressIndex = dr["AddressIndex"] == DBNull.Value ? 0 : System.Int32.Parse(dr["AddressIndex"].ToString());
        //        Obj.AddressAlias = dr["AddressAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["AddressAlias"]);
        //        Obj.Address = dr["Address"] == DBNull.Value ? string.Empty : (System.String)(dr["Address"]);
        //        return Obj;
        //    }

        //}

        private async void OnNewKeyStore(object sender, RoutedEventArgs e)
        {
            if (WindowNewKeyStore.NewKeyStoreAddress(this))
            {
                //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                await QueryAddress();
            }
        }

        private void OnLoadPrivateKey11(object sender, RoutedEventArgs e)
        {
            var item = DataGridAddress.SelectedItem;
            if (item is System.Data.DataRowView)
            {
                var drv = (System.Data.DataRowView)item;
                string address = drv["Address"].ToString();
                BlockChain.OfflineWallet.WindowExportPrivateKey.ExportPrivateKey(this, address); //导出私钥不需要单独一个窗体，直接输入密码就好，地址可以取到。
            }

        }


        private async void OnLoadKeyStore(object sender, RoutedEventArgs e)
        {
            if (WindowLoadKeyStore.LoadKeyStoreFile(this))
            {
                //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
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
                //DataGridAddress.ItemsSource = BLL.Address.GetRealAllAddress().DefaultView;
                await QueryAddress();
            }
        }

        private async void AddressFilterOnClick(object sender, RoutedEventArgs e)
        {
            ButtonAddressFilter.Tag = TextBoxAddressFilter.Text.Trim();                 //查询条件保存下来，只有点击了这个按钮才能变更查询条件！
            await QueryAddress();
        }

        private void TextBoxAddressFilter_KeyDown(object sender, KeyEventArgs e)
        {
            AddressFilterOnClick(null, null);
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


        private void ButtonTools_OnClick(object sender, RoutedEventArgs e)
        {
            if (!MenuItemTools.IsSubmenuOpen)
            {
                MenuItemTools.IsSubmenuOpen = true;
            }
        }

      

        private void OnAbout(object sender, RoutedEventArgs e)
        {
            WindowAbout.ShowAbout(this);
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
        private void OnSysExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OffSig_OnClick(object sender, RoutedEventArgs e)
        {
            WindowOfflineSig f = new WindowOfflineSig();
            f.Owner = this;
            f.ShowDialog();
        }

        private void Fuction_OnClick(object sender, RoutedEventArgs e)
        {
            WindowFunction f = new WindowFunction();
            f.Owner = this;
            f.ShowDialog();
        }

        private void OnCalTools(object sender, RoutedEventArgs e)
        {
            Share.WindowCalculateTools.ExeWindowCalculateTools(this);
        }

      

    }

}
