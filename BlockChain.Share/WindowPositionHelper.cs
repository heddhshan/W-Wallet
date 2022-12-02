using Microsoft.Win32;
using System;
using System.Windows;

namespace BlockChain.Share
{

    //public class WindowPositionHelper
    //{
    //    public static string RegPath = @"Software\BlockChain\";

    //    public static void SaveSize(Window win)
    //    {
    //        // Create or retrieve a reference to a key where the settings
    //        // will be stored.
    //        RegistryKey key;
    //        key = Registry.CurrentUser.CreateSubKey(RegPath + win.Name);

    //        key.SetValue("Bounds", win.RestoreBounds.ToString(System.Globalization.CultureInfo.InvariantCulture));
    //    }

    //    public static void SetSize(Window win)
    //    {
    //        RegistryKey key;
    //        key = Registry.CurrentUser.OpenSubKey(RegPath + win.Name);

    //        if (key != null)
    //        {
    //            Rect bounds = Rect.Parse(key.GetValue("Bounds").ToString());

    //            win.Top = bounds.Top;
    //            win.Left = bounds.Left;

    //            // Only restore the size for a manually sized
    //            // window.
    //            if (win.SizeToContent == SizeToContent.Manual)
    //            {
    //                win.Width = bounds.Width;
    //                win.Height = bounds.Height;
    //            }
    //        }
    //    }
    //}

    public class WindowPositionHelperConfig
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void SaveSize(Window win)
        {
            try
            {
                Properties.Settings.Default.WindowPosition = win.RestoreBounds.ToString();
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
        }

        public static void SetSize(Window win)
        {
            try
            {
                Rect rect = Rect.Parse(Properties.Settings.Default.WindowPosition);
                win.Top = rect.Top;
                win.Left = rect.Left;

                // Only restore the size for a manually sized
                // window.
                if (win.SizeToContent == SizeToContent.Manual)
                {
                    win.Width = rect.Width;
                    win.Height = rect.Height;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }

        }
    }

}
