
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BlockChain.OfflineWallet
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
            this.Title = Share.LanguageHelper.GetTranslationText(this.Title);

            var dv = new  BLL.Address().GetAllTxAddress();          //钱包 和 Share 里面的处理不一样！
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
        


        /// <summary>
        /// 导出私钥， Share 中也有这个窗体，但 Share 中的面对无助剂词的地址，获取账号的方式不一样，所以这里单独列出来！！！！
        /// 传入账号获取的回调函数可以解决这个问题，以后再说！
        /// </summary>
        /// <param name="_owner"></param>
        /// <param name="_address"></param>
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

            var account = new BLL.Address().GetAccount(a, p);               //钱包 和 Share 里面的处理不一样！
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
            if (row is Share.TxAddress)
            {
                Share.TxAddress r = (Share.TxAddress)row;
                //AddressAlias, Address, 'KeyStore' AS SourceName
                string Address = r.Address;// r[1].ToString();
                ComboBoxAddress.Tag = Address;
                TextBoxPrivateKey.Text = string.Empty;
            }
        }
    }
}
