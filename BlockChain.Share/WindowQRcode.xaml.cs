
using BlockChain.Share;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using ZXing;
using ZXing.Common;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowQRcode.xaml 的交互逻辑
    /// </summary>
    public partial class WindowQRcode : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowQRcode()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private BitMatrix bitMatrix;

        public string QRcode
        {
            get
            {
                return LabelInfo.Text;
            }
            set
            {
                LabelInfo.Text = value;
            }
        }

        //https://www.cnblogs.com/wywnet/p/3559917.html
        public void ShowImage()
        {
            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
            hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

            bitMatrix = new MultiFormatWriter().encode(QRcode, BarcodeFormat.QR_CODE, 200, 200);
            ImageQRcode.Source = toImage(bitMatrix);
        }

        private BitmapImage toImage(BitMatrix matrix)
        {
            try
            {
                int width = matrix.Width;
                int height = matrix.Height;
                Bitmap bmp = new Bitmap(width, height);
                for (int x = 0; x < height; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        if (bitMatrix[x, y])
                        {
                            bmp.SetPixel(x, y, System.Drawing.Color.Black);
                        }
                        else
                        {
                            bmp.SetPixel(x, y, System.Drawing.Color.White);
                        }
                    }
                }
                return ConvertBitmapToBitmapImage(bmp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static BitmapImage ConvertBitmapToBitmapImage(Bitmap wbm)
        {
            BitmapImage bimg = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                wbm.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                bimg.BeginInit();
                stream.Seek(0, SeekOrigin.Begin);
                bimg.StreamSource = stream;
                bimg.CacheOption = BitmapCacheOption.OnLoad;
                bimg.EndInit();
            }
            return bimg;
        }

        //https://blog.csdn.net/weixin_43100896/article/details/86666813
        private void OnSave(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image Files ( *.png, *.bmp, *.jpg)|*.bmp;*.png;*.jpg | All Files | *.*";//图片格式
            sfd.FileName = QRcode + ".png";
            sfd.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录 
            if (sfd.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)this.ImageQRcode.Source));
                using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                    encoder.Save(stream);
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public static void ShowQRcode(Window _owner, string qrcode)
        {
            WindowQRcode w = new WindowQRcode();
            w.Owner = _owner;
            w.QRcode = qrcode;
            w.ShowImage();
            w.ShowDialog();
        }

    }
}
