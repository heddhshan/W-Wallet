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

namespace BlockChain.Share
{
    /// <summary>
    /// WindowLoading.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLoading : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowLoading()
        {
            InitializeComponent();
        }

        private static WindowLoading Instance = null;

        private void OnClickHide(object sender, RoutedEventArgs e)
        {
            this.Close();
            Instance = null;
        }

        public static int Counter = 0;

        public static object LockObj = new object();


        public static bool ShowLoading(Window owner, string msg = "Please Wait")
        {
            lock (LockObj)
            {
                try
                {
                    if (Instance == null)
                    {
                        Instance = new WindowLoading();
                    }
                    Instance.Owner = owner;
                    Instance.Title = msg;
                    Instance.Topmost = true;
                    Instance.Show();

                    ((IMainWindow)System.Windows.Application.Current.MainWindow).ShowProcessing();     //同步主窗体的处理
                    Counter = Counter + 1; return true;
                }
                catch (Exception ex)
                {
                    log.Error("CloseLoading", ex);
                    return false;
                }
            }
        }

        public static bool CloseLoading()
        {
            lock (LockObj)
            {
                try
                {
                    if (Instance != null)
                    {
                        Instance.Close();
                        Instance = null;
                    }

                    Counter = Counter - 1;
                    if (Counter <= 0)
                    {
                        Counter = 0;
                        if (System.Windows.Application.Current.MainWindow != null)
                        {
                            ((IMainWindow)System.Windows.Application.Current.MainWindow).ShowProcesed();     //同步主窗体的处理
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error("CloseLoading", ex);
                    return false;
                }
            }
        }

    }
}
