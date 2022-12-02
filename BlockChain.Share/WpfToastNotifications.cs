using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.Foundation.Collections;


namespace BlockChain.Share
{
    public class WpfToastNotifications
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static int getDefaultExpiSeconds(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Error:
                    return 60 * 10;

                case NotificationType.Warning:
                    return 60 * 5;

                case NotificationType.Information:
                    return 60 * 1;

                case NotificationType.Success:
                    return 60 * 3;

                default:
                    return 60 * 1;
            }
        }


        public static Uri getAppLogo(NotificationType type)
        {         
            switch (type)
            {
                case NotificationType.Error:
                    var p = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"ToastImage\Blue.png");
                    var u = new Uri(p);
                    return u;

                case NotificationType.Warning:
                     p = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"ToastImage\Yellow.png");
                     u = new Uri(p);
                    return u;

                case NotificationType.Information:
                     p = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"ToastImage\Teal.png");
                     u = new Uri(p);
                    return u;

                case NotificationType.Success:
                     p = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"ToastImage\Red.png");
                     u = new Uri(p);
                    return u;

                default:
                     p = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"ToastImage\Orange.png");
                     u = new Uri(p);
                    return u;

            }
        }




        private static bool IsRegistered = false;

        public static void Register()
        {
            if (!IsRegistered)
            {
                IsRegistered = true;
                // Listen to notification activation
                ToastNotificationManagerCompat.OnActivated += toastArgs =>
                {
                    try
                    {
                        // Obtain the arguments from the notification
                        ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                        // Obtain any user input (text boxes, menu selections) from the notification
                        ValueSet userInput = toastArgs.UserInput;

                        // Need to dispatch to UI thread if performing UI operations
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            string url = toastArgs.Argument;
                            log.Info("Toast CallBack, ToastArgs.Argument: " + url);
                            if (!string.IsNullOrEmpty(url))
                            {
                                url = url.Trim();
                                if (!string.IsNullOrEmpty(url))
                                {
                                    System.Diagnostics.Process.Start("explorer.exe", url);
                                }
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        log.Error("Toast CallBack", ex);
                    }
                };
            }
            else
            {
                throw new Exception("only once");
            }
        }

        public static void UnRegister()
        {
            ToastNotificationManagerCompat.Uninstall();
        }

        private static object Show_Lock = new object();

        public static void Show(string title, string message, NotificationType type, int expiSeconds, string? url = null)
        {
            lock (Show_Lock)
            {
                try
                {
                    //根据不同的类型，显示不同的图标和背景；如果有url，则需要点击的时候打开 url 页面。

                    var t = new ToastContentBuilder();

                    //t.AddHeader(type.ToString(), type.ToString(), "");      //head 的回调，不用处理！  Header 是分类，不需要！

                    //根据不同 NotificationType type ，显示不同的提示图片 
                    var u = getAppLogo(type);
                    t.AddAppLogoOverride(u, ToastGenericAppLogoCrop.Circle);

                    //t.AddText(type.ToString() + " - "+ title);        //不需要， 图标区别了！
                    t.AddText(title);
                    t.AddText(message);

                    //如果有 url， 需要打开的 //Launch 的 callback， 需要处理！
                    if (url != null)
                    {
                        t.Content.Launch = url;
                    }

                    //t.Show();
                    t.Show(toast =>
                    {
                        toast.ExpirationTime = DateTime.Now.AddMinutes(expiSeconds);            //调试的时候这里可能出错，还不知道原因，一般是UI线程和后台线程调用，估计是多线程调用并发导致？？？！！！  非调试状态未发现问题！
                    });

                }
                catch (Exception ex)
                {
                    log.Error("Toast.Show", ex);
                }
            }
        }
    }


    public enum NotificationType
    {
        Information = 1,
        Warning = 2,
        Error = 3,
        Success = 4,
    }

}
