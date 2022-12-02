using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

using BlockChain.Share;
using Newtonsoft.Json;
using System.Data;

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowExportHDAddress.xaml 的交互逻辑
    /// </summary>
    public partial class WindowExportHDAddress : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowExportHDAddress()
        {
            InitializeComponent();

            this.Title = LanguageHelper.GetTranslationText(this.Title);

            //助记词选择
            ComboBoxHD4.ItemsSource = BLL.HDWallet.GetHDWallets().DefaultView;
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());

        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //private bool ItemIsSelected(int IndexId)
        //{
        //    var cntr = DataGrid1.ItemContainerGenerator.ContainerFromIndex(IndexId);
        //    DataGridRow ObjROw = (DataGridRow)cntr;
        //    if (ObjROw != null)
        //    {
        //        FrameworkElement objElement = DataGrid1.Columns[0].GetCellContent(ObjROw);
        //        if (objElement != null)
        //        {
        //            System.Windows.Controls.CheckBox objChk = (System.Windows.Controls.CheckBox)objElement;
        //            return objChk.IsChecked == true;
        //        }
        //    }

        //    log.Error("where");
        //    return false;//
        //}

        private void OnExAddress(object sender, RoutedEventArgs e)
        {
            //this.Cursor = Cursors.Wait;
            //((IMainWindow)App.Current.MainWindow).ShowProcessing();
            //try
            //{
            //    StringBuilder l = new StringBuilder();

            //    //foreach (var FromItem in DataGrid1.Items)
            //    //{
            //    for (int i = 0; i < this.DataGrid1.Items.Count; i++)
            //    {
            //        var FromItem = DataGrid1.Items[i];
            //        if (!(FromItem is Model.ViewHdAddress))
            //        {
            //            return;
            //        }

            //        Model.ViewHdAddress model = (Model.ViewHdAddress)FromItem;

            //        //var IsSelected = (int)r["IsSelected"] == 1;
            //        //var IsSelected = ItemIsSelected(i);
            //        if (!model.IsSelected)
            //        {
            //            continue;
            //        }
            //        string AddressIndex = model.AddressIndex.ToString();
            //        string address = model.Address;
            //        string salt = model.MneSecondSalt;

            //        if (CheckBox1.IsChecked == true)
            //        {
            //            address = address + "," + AddressIndex.ToString();
            //        }
            //        if (CheckBox2.IsChecked == true)
            //        {
            //            address = address + "," + salt;
            //        }
            //        if (CheckBox3.IsChecked == true)
            //        {
            //            //导出当前 nonce 
            //            address = address + "," + Share.ShareParam.GetNonce(model.Address).ToString();
            //        }

            //        l.AppendLine(address);
            //    }

            //    SaveFileDialog sfd = new SaveFileDialog();
            //    sfd.Filter = "Txt Files ( *.txt)|*.txt | All Files | *.*";
            //    sfd.RestoreDirectory = true;
            //    if (sfd.ShowDialog() == true)
            //    {
            //        string result = l.ToString();
            //        StreamWriter sw = File.AppendText(sfd.FileName);
            //        sw.Write(result);
            //        sw.Flush();
            //        sw.Close();

            //        Common.FileHelper.PositionFile(sfd.FileName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(this, ex.Message);
            //}
            //finally
            //{
            //    ((IMainWindow)App.Current.MainWindow).ShowProcesed();
            //    Cursor = Cursors.Arrow;
            //}
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var row = ComboBoxHD4.SelectedItem;

                System.Data.DataRowView r = (System.Data.DataRowView)row;
                var MneId = (Guid)r["MneId"];
                var MneAlias = (string)r["MneAlias"];

                OfflineWallet.DataSig.Model.HDWalletWithoutPrivatekey hdw = new OfflineWallet.DataSig.Model.HDWalletWithoutPrivatekey();
                hdw.MneId = MneId;
                hdw.MneAlias = MneAlias;

                var list = (List<Model.ViewHdAddress>)DataGrid1.ItemsSource;
                foreach (var m in list)
                {
                    OfflineWallet.DataSig.Model.HDAddressWithoutPrivatekey address = new OfflineWallet.DataSig.Model.HDAddressWithoutPrivatekey();
                    address.Address = m.Address;
                    if (CheckBoxHasAddressAlias.IsChecked == true)
                    {
                        address.AddressAlias = m.AddressAlias;
                    }
                    else
                    {
                        address.AddressAlias = "***";
                    }
                    address.MneId = MneId;

                    hdw.AddressList.Add(address);
                }
                string jsonStr = JsonConvert.SerializeObject(hdw);

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Txt Files ( *.txt)|*.txt | All Files | *.*";
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() == true)
                {
                    StreamWriter sw = File.AppendText(sfd.FileName);
                    sw.Write(jsonStr);
                    sw.Flush();
                    sw.Close();

                    Common.FileHelper.PositionFile(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }


        }

        private void OnSelectionChanged4(object sender, SelectionChangedEventArgs e)
        {
            var row = ComboBoxHD4.SelectedItem;
            if (row is System.Data.DataRowView)
            {
                System.Data.DataRowView r = (System.Data.DataRowView)row;
                var MneId = (Guid)r["MneId"];
                DataGrid1.ItemsSource = BLL.HDWallet.GetExHdAddress(MneId);//.DefaultView;
            }

        }

        public static void DoEx(Window _owner)
        {
            WindowExportHDAddress w = new WindowExportHDAddress();
            w.Owner = _owner;
            w.ShowDialog();
        }

        private void OnUnchecked(object sender, RoutedEventArgs e)
        {
            SelectAddressAll(false);
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            SelectAddressAll(true);
        }

        private void SelectAddressAll(bool IsSelected)
        {
            if (this.DataGrid1 == null) { return; }

            if (this.DataGrid1 == null || DataGrid1.ItemsSource == null) { return; }

            var list = (List<Model.ViewHdAddress>)DataGrid1.ItemsSource;
            foreach (var m in list)
            {
                m.IsSelected = IsSelected;
            }

            //下面两种方法，各有各的问题！！！

            ////方法1
            //for (int i = 0; i < this.DataGrid1.Items.Count; i++)
            //{               
            //    var cntr = DataGrid1.ItemContainerGenerator.ContainerFromIndex(i);
            //    DataGridRow ObjROw = (DataGridRow)cntr;
            //    if (ObjROw != null)
            //    {
            //        FrameworkElement objElement = DataGrid1.Columns[0].GetCellContent(ObjROw);
            //        if (objElement != null)
            //        {
            //            System.Windows.Controls.CheckBox objChk = (System.Windows.Controls.CheckBox)objElement;
            //            //if (objChk.IsChecked == false)
            //            //{
            //            objChk.IsChecked = IsSelected;
            //            //}
            //        }
            //    }
            //}

            //return;

            ////方法2
            //object FirstItem = null;
            //object LastItem = null;

            //for (int i = 0; i < this.DataGrid1.Items.Count; i++)
            //{
            //    var FromItem = DataGrid1.Items[i];
            //    if (!(FromItem is Model.ViewHdAddress))
            //    {
            //        return;
            //    }

            //    Model.ViewHdAddress model = (Model.ViewHdAddress)FromItem;
            //    model.IsSelected = IsSelected;

            //    if (i == 0) { FirstItem = model; }
            //    if (i == this.DataGrid1.Items.Count - 1)
            //    { LastItem = model; }                
            //}

            ////界面没有更新，要滚动一下才更新，微软的bug ？  下面代码也无法完全解决
            //if (FirstItem != null)
            //{
            //    DataGrid1.ScrollIntoView(FirstItem);
            //}
            //if (LastItem != null)
            //{
            //    DataGrid1.ScrollIntoView(LastItem);
            //}

        }

    }
}
