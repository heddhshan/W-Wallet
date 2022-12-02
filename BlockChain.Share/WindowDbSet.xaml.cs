using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace BlockChain.Share
{
    /// <summary>
    /// WindowDbSet.xaml 的交互逻辑
    /// </summary>
    public partial class WindowDbSet : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        //private string ThisDbConStr;

        public WindowDbSet()
        {
            InitializeComponent();
            //this.Title = Share.LanguageHelper.GetTranslationText(this.Title);

            TextBoxDbStr.Text = Properties.Settings.Default.UserDbConnectionString;
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void OnTest(object sender, RoutedEventArgs e)
        {
            string conStr = TextBoxDbStr.Text;
            bool IsOk = TestDbConnect(conStr);
            MessageBox.Show (this, "SQLServer database connection Is: " + IsOk.ToString());
        }

        private void OnOk(object sender, RoutedEventArgs e)
        {
            if (TestDbConnect(TextBoxDbStr.Text))
            {
                var DbConStr = TextBoxDbStr.Text;
                UpdateDbConStr(DbConStr);
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show (this, "SQLServer database connection Is failed!");
            }
        }

        private static void UpdateDbConStr(string DbConStr)
        {
            Properties.Settings.Default.UserDbConnectionString = DbConStr;
            Properties.Settings.Default.Save();
        }

        public static bool TestDbConnect(string dbConStr)
        {
            try
            {
                //TODO:这么写的测试是不够的，还需要打开表！
                SqlConnection cn = new SqlConnection(dbConStr);

                log.Info("TestDbConnect: DataSource is " + cn.DataSource + ", Database is " + cn.Database);

                cn.Open();
                cn.Close();
                //_IsConnectioned = true;
                return true;
            }
            catch (Exception ex)
            {
                log.Error("IsDbConOk", ex);
                return false;
            }
        }

        public static bool ShowUiTestDbConOk(Window _owner, string _DbConStr)
        {
            if (!TestDbConnect(_DbConStr))
            {
                WindowDbSet f = new WindowDbSet();
                f.TextBoxDbStr.Text = _DbConStr;
                f.Owner = _owner;
                if (f.ShowDialog() == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (ShareParam.DbConStr != _DbConStr)
                {
                    UpdateDbConStr(_DbConStr);
                }
                return true;
            }
        }

        private void GotoPage_Click(object sender, RoutedEventArgs e)
        {
            var url = @"https://www.microsoft.com/en-us/sql-server/sql-server-downloads";
            System.Diagnostics.Process.Start("explorer.exe", url);
        }

        private void DownLoadExpress_Click(object sender, RoutedEventArgs e)
        {
            var url = @"https://go.microsoft.com/fwlink/p/?linkid=866658";

            var info = new System.Diagnostics.ProcessStartInfo();
            info.Verb = "POST";
            info.FileName = "explorer.exe";
            info.Arguments = url;

            System.Diagnostics.Process.Start(info);

            //System.Diagnostics.Process.Start( "explorer.exe",url);
        }

    }
}
