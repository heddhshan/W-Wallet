
using BlockChain.Share;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowExportPrivateKey.xaml 的交互逻辑
    /// </summary>
    public partial class WindowExportPrivateKey : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DispatcherTimer timer = new DispatcherTimer();

        public WindowExportPrivateKey()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            var dv = new  BLL.Address().GetAllTxAddress();          //todo: 对于钱包来说，这个是有问题的！！！
            ComboBoxAddress.ItemsSource = dv;

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromMinutes(3);               //设置刷新的间隔时间
            timer.Start();
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Close();                                       //三分钟后自动关闭
        }
        

        public static void ExportPrivateKey(Window _owner, string _address)
        {
            WindowExportPrivateKey w = new WindowExportPrivateKey();
            w.MyLabelAddress.Content = _address;
            w.Owner = _owner;                   
            w.ShowDialog();
        }

        private void OnExPrivate(object sender, RoutedEventArgs e)
        {
            var p = PasswordBox1.Password;
            var a = ComboBoxAddress.Tag.ToString();

            var account = new BLL.Address().GetAccount(a, p);
            if (account != null)
            {
                TextBoxPrivateKey.Text = account.PrivateKey;
                Labelpassword.Foreground = Brushes.Black;

                TimerNewStart();
            }
            else
            {
                Labelpassword.Foreground = Brushes.Red;
                PasswordBox1.Focus();
            }
        }


        /// <summary>
        /// 每次显示私钥后，三分钟内关闭。
        /// </summary>
        private void TimerNewStart()
        {
            timer.IsEnabled = false;
            timer.Start();
        }

        private void MyOnExPrivate(object sender, RoutedEventArgs e)
        {
            var p = MyPasswordBox1.Password;
            var a = this.MyLabelAddress.Content.ToString();

            var account = new BLL.Address().GetAccount(a, p);
            if (account != null)
            {
                MyTextBoxPrivateKey.Text = account.PrivateKey;
                MyLabelpassword.Foreground = Brushes.Black;
                TimerNewStart();
            }
            else
            {
                MyLabelpassword.Foreground = Brushes.Red;
                MyPasswordBox1.Focus();
            }
        }



        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnSelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            var row = ComboBoxAddress.SelectedItem;
            if (row is TxAddress)
            {
                TxAddress r = (TxAddress)row;
                //AddressAlias, Address, 'KeyStore' AS SourceName
                string Address = r.Address;// r[1].ToString();
                ComboBoxAddress.Tag = Address;
                TextBoxPrivateKey.Text = string.Empty;
            }
        }
    }
}
