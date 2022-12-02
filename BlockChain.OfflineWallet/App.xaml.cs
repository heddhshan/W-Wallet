using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BlockChain.OfflineWallet
{
    ///// <summary>
    ///// Interaction logic for App.xaml
    ///// </summary>
    //public partial class App : Application
    //{
    //    public static bool IsRunning = true;




    //}

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 重写 main ，单实例，参照 ：https://www.cnblogs.com/lenlen-de/p/3422777.html https://www.cnblogs.com/lonelyxmas/p/9946220.html

        private const string MutexName = "{9C16DB33-731B-4D9A-8AEE-3BECFA144F03}";                          //通过Mutex对象判断，和 FindWindow 重复，但还是有点不一样。

        [DllImportAttribute("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ShowToFront(string windowName)
        {
            try
            {
                IntPtr firstInstance = FindWindow(null, windowName);
                ShowWindow(firstInstance, 1);
                SetForegroundWindow(firstInstance);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
        }


        [DllImport("user32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);


        [STAThread]
        public static void Main()
        {
            #region 处理前置, 通过进程名字判断，前置老进程窗口，终止当前进程。

            Process procCurrent = Process.GetCurrentProcess();
            Process[] procProgram = Process.GetProcessesByName(procCurrent.ProcessName);

            if (procProgram.Length > 1)
            {
                System.Threading.Thread.Sleep(new TimeSpan(0, 0, 3));    //休息3秒，让重启执行！

                for (int nIndex = 0; nIndex < procProgram.Length; nIndex++)
                {
                    if (procProgram[nIndex].Id != procCurrent.Id)
                    {
                        SwitchToThisWindow(procProgram[nIndex].MainWindowHandle, true);
                        return;                         //退出程序
                    }
                }
            }

            #endregion

            log4net.Config.XmlConfigurator.Configure();
            log.Info("1" + Environment.NewLine);
            log.Info("2" + Environment.NewLine);
            log.Info("3" + Environment.NewLine);
            log.Info("**********************************************************************************");
            log.Info("app start, CurrentVersion:" + OffWalletParam.Version);
            log.Info("Exe Path: " + Environment.CurrentDirectory);

            bool createNew;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, MutexName, out createNew);  //单实例，和前面处理前置一样功能，但进程名字改了也起作用。

            if (!createNew)
            {
                log.Error("app is running, can not has two instances, mutex:" + mutex.ToString());
                ShowToFront("MainWindow");              //这是不起作用的，SwitchToThisWindow 才起作用.
                return;
            }
            else
            {
                System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;

                try
                {
                    ThisSplashScreen = new SplashScreen("SplashScreen.png");
                    ThisSplashScreen.Show(false);
                    System.Threading.Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    log.Error("SplashScreen", ex);
                }

                Share.ShareParam.SetDataDirectory();                //设置数据库默认目录！

                Share.ShareParam.IsOfflineWallet = true;            //设置离线钱包

                //Share.ShareParam.IniDbPath("WalletDb.mdf");         // 初始化 数据库 连接字符串中的 DataDirectory (如果有) 具体指向的目录！ 必须在调用数据库之前执行一次
                //第一次运行，把默认数据库链接保存
                if (OffWalletParam.IsFirstRun)
                {
                    Share.ShareParam.UpdateDbConStr(OffWalletParam.DefaultDbConStr);

                    //Share.LanguageHelper.IniLangageData();  //初始化脚本包括了这些数据
                }

                //测试数据库连接
                if (!Share.WindowDbSet.TestDbConnect(Share.ShareParam.DbConStr))                   //1,测试 已经保存的数据库
                {
                    if (!Share.WindowDbSet.ShowUiTestDbConOk(null, OffWalletParam.DefaultDbConStr))    //1,测试 默认的数据库
                    {

                        MessageBox.Show("App can not link db，it will exit。");
                        return;
                    }
                    else
                    {
                        // 数据库链接里面可能有密码，不能写入日志
                        //log.Error("Update Db Connection String" + Environment.NewLine + "Old Db Connection String Is: " + Share.ShareParam.DbConStr + Environment.NewLine + "New Db Connection String Is: " + OffWalletParam.DefaultDbConStr);
                        log.Error("Update Db Connection String");
                        //Share.ShareParam.UpdateDbConStr(OffWalletParam.DefaultDbConStr);               //1,如果默认数据库能够使用，就使用默认数据库！
                    }
                }

                System.Threading.Thread.Sleep(1);

                //// 2021-08-14 Enable by default Hex values for eth_feeHistory, this supports the standard, if connecting to Geth < 1.10.7 use EthFeeHistory.UseBlockCountAsNumber = true.
                //Nethereum.RPC.Eth.Transactions. EthFeeHistory.UseBlockCountAsNumber = true;

                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
        }

        #endregion

        public static bool IsRunning = true;

        private static SplashScreen ThisSplashScreen = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            IsRunning = true;

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

      
            if (ThisSplashScreen != null)
            {
                System.Threading.Thread.Sleep(1);
                ThisSplashScreen.Close(new TimeSpan(0, 0, 10));       //10 秒  感觉没啥用
            }

            Share.WpfToastNotifications.Register();

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //e.Handled = true;   //是否标记为已经处理？

            try
            {
                log.Error("App_DispatcherUnhandledException", e.Exception);
                e.Handled = ShowThreadExceptionDialog("Error", e.Exception);
            }
            catch (Exception exc)
            {
                try
                {
                    log.Error("ThreadException Exception", exc);
                }
                finally
                {
                    e.Handled = false;
                    //this.Shutdown(99);
                }
            }
        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);
            log.Info("OnSessionEnding：" + Common.Security.Tools.CreateXML<SessionEndingCancelEventArgs>(e));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var DataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
            log.Info("DataDirectory Is " + DataDirectory);


            Share.WpfToastNotifications.UnRegister();

            // 停止 检查 Web 3 的链接
            Share.BLL.Web3Url.DoCheckWeb3Connection(false);

            if (OffWalletParam.IsFirstRun)
            {
                OffWalletParam.IsFirstRun = false;
            }

            IsRunning = false;

            base.OnExit(e);
            try
            {
                log.Info("app exit, version is " + OffWalletParam.Version);
            }
            catch (Exception ex)
            {
                log.Error("Application OnExit", ex);
            }
        }

        private static bool ShowThreadExceptionDialog(string title, Exception e)
        {
            string errorMsg = "An application error occurred. " + Environment.NewLine
                            + e.Message + "\n\nStack Trace:\n" + e.StackTrace
                            + Environment.NewLine + "Click 'Yes' Continue, Click 'No' Exit.";
            return MessageBox.Show(errorMsg, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                log.Error("CurrentDomain_UnhandledException", ex);
            }
            catch (Exception exc)
            {
                try
                {
                    log.Error("UnhandledException Exception", exc);
                }
                finally
                {
                    Application.Current.Shutdown(99);
                }
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            //log.Info("OnActivated");
        }


        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            //log.Info("OnDeactivated");
        }


        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
            log.Info("OnLoadCompleted");
        }



    }
}
