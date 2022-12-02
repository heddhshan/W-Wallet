using BlockChain.Share;
using Microsoft.Win32;
using Nethereum.KeyStore;
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
    /// WindowLoadKeyStore.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLoadKeyStore : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowLoadKeyStore()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);

            // <Label Name="LabelInfo" Grid.Row="0" Grid.Column="0" Content="导入的KeyStore文件将被复制到‘’。" Grid.ColumnSpan="3" HorizontalAlignment="Center"></Label>
            LabelInfo.Text = LanguageHelper.GetTranslationText(@"导入的KeyStore文件将被复制到目录:‘") + Share.ShareParam.KeystoreDir + "'。";
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void OnSelectKeyStore(object sender, RoutedEventArgs e)
        {
            //导入keystore文件，流程：选择文件，输入密码，复制到keyStore目录，写入数据库
            //导入keystore文件，流程：选择文件，输入密码，复制到keyStore目录，写入数据库
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = LanguageHelper.GetTranslationText("请选择KeyStore文件");
            dialog.Filter = "All Filse(*.*)|*.*";
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            string file = dialog.FileName;
            this.TextBoxFile.Text = file;
        }


        private void OnLoadKeyStore(object sender, RoutedEventArgs e)
        {
            string password = PasswordBox1.Password;
            var filePath = this.TextBoxFile.Text;

            try
            {
                var file = System.IO.File.OpenText(filePath);
                var json = file.ReadToEnd();
                file.Close();
                var account = Nethereum.Web3.Accounts.Account.LoadFromKeyStore(json, password);

                var fn = System.IO.Path.GetFileName(filePath);
                fn = System.IO.Path.Combine(Share.ShareParam.KeystoreDir, fn);

                //复制到keyStore目录，写入数据库
                if (fn == filePath || fn.ToLower() == filePath.ToLower())
                {
                    //如果文件来源和保存位置相同， do nothing 
                }
                else {
                    if (System.IO.File.Exists(fn))
                    {
                        fn = fn + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    System.IO.File.Copy(filePath, fn);
                }

                Share.BLL.KeyStore.SaveKeyStoreToDb(TextBoxAlias.Text.Trim(), account.Address, json, filePath);// SaveKeyStore(TextBoxAlias.Text.Trim(), password);

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show (this, LanguageHelper.GetTranslationText("此错误一般是由密码错误、文件无法读取、或者地址重复造成的。")
                    + Environment.NewLine + ex.Message, "Error", MessageBoxButton.OK);
            }

        }

        public static bool LoadKeyStoreFile(Window _owner)
        {
            WindowLoadKeyStore w = new WindowLoadKeyStore();
            w.Owner = _owner;
            return w.ShowDialog() == true;
        }

    }
}
