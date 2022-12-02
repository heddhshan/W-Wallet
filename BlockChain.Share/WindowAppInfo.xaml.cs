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

namespace BlockChain.Share
{
    /// <summary>
    /// WindowAppInfo.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAppInfo : Window
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowAppInfo()
        {
            InitializeComponent();
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            DataGridAppList.ItemsSource = Share.BLL.AppInfo.GetCurAppInfo().DefaultView;

            ButtonOfficeUri.Content = Share.ShareParam.GetT3sOfficeUri();
        }


        private void LinkOnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button bu = (sender as Button);
                object thisurl = bu.Tag;
                if (thisurl == null) { return; }

                string url = bu.Tag.ToString();
                if (string.IsNullOrEmpty(url)) { return; }

                if (!string.IsNullOrEmpty(url))
                {
                    //System.Diagnostics.Process.Start("explorer.exe", url);

                    ShareParam.OperUrl(url);

                    //Process p = new Process();
                    //p.StartInfo.FileName = "cmd.exe";
                    //p.StartInfo.UseShellExecute = false;    //不使用shell启动
                    //p.StartInfo.RedirectStandardInput = true;//喊cmd接受标准输入
                    //p.StartInfo.RedirectStandardOutput = false;//不想听cmd讲话所以不要他输出
                    //p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                    //p.StartInfo.CreateNoWindow = true;//不显示窗口
                    //p.Start();

                    ////向cmd窗口发送输入信息 后面的&exit告诉cmd运行好之后就退出
                    ////p.StandardInput.WriteLine("start " + url + "&exit");
                    //p.StandardInput.WriteLine("start " + url); 
                    //p.StandardInput.AutoFlush = true;
                    //p.WaitForExit();//等待程序执行完退出进程
                    //p.Close();

                }
            }
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void RefreshOnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)System.Windows.Application.Current.MainWindow).ShowProcessing();
            try
            {
                if (CheckBoxSynFull.IsChecked == true) 
                { 
                Share.BLL.AppInfo.DeleteAllAppInfo();
                }

                await Share.BLL.AppInfo.UpdateAppInfo();
                DataGridAppList.ItemsSource = Share.BLL.AppInfo.GetCurAppInfo().DefaultView;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                ((IMainWindow)System.Windows.Application.Current.MainWindow).ShowProcesed();
            }
        }

        private void WebSiteOnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var txid = b.Content.ToString();
                System.Diagnostics.Process.Start("explorer.exe", b.Content.ToString());
            }
        }

        ////AddingNewItem="DataGridAppListOnAddingNewItem"

        //private void DataGridAppListOnAddingNewItem(object sender, AddingNewItemEventArgs e)
        //{
        //    var row = e.NewItem;

        //}


    }
}
