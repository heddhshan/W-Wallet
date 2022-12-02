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

using BlockChain.Share;


namespace BlockChain.OfflineWallet
{
    /// <summary>
    /// WindowMnemonic.xaml 的交互逻辑
    /// </summary>
    public partial class WindowNewMnemonic : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowNewMnemonic()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private BlockChain.OfflineWallet.Model.HD_Mnemonic model;

        private int Step = 0;
        private int LastPageIndex = 0;

        //public BlockChain.OfflineWallet.Model.HD_Mnemonic HdModel;

        private void OnCreateHd(object sender, RoutedEventArgs e)
        {
            try
            {
                string alias = TextBoxWalletName.Text.Trim();
                string password = PasswordBoxInput1.Password;
                string tip = TextBoxPasswordTip.Text.Trim();

                model = BLL.HDWallet.NewHDWallet(alias, password, tip);

                Step = 1;
                TabControlMain.SelectedIndex = Step;
            }
            catch (Exception ex)
            {
                MessageBox.Show (this, LanguageHelper.GetTranslationText("异常，多半是别名不唯一造成的。") + Environment.NewLine + ex.Message);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Step != TabControlMain.SelectedIndex)
                {
                    TabControlMain.SelectedIndex = Step;        //不允许手动翻页
                    return;
                }

                if (LastPageIndex == Step - 1)
                {
                    LastPageIndex = Step;                       //保证每个页面只进入一次

                    if (TabControlMain.SelectedIndex == 1)
                    {
                        string password = InputPasswrod();         //如果第二页，必须先得到密码

                        string EncPassword = Common.PasswordHelper.GetHDRealPassword(model.Salt, password);
                        TextBoxMne.Text = Common.Security.SymmetricalEncrypt.Decrypt(EncPassword, model.MneEncrypted);
                    }
                    else if (TabControlMain.SelectedIndex == 2)
                    {
                        //BLL.HDWallet.UpdateIsBackUp(model.MneHash);
                        var mne = TextBoxMne.Text;
                        var words = mne.Split(' ');

                        //排序
                        List<string> l = new List<string>(words);
                        l.Sort();

                        //foreach (var w in words) {
                        foreach (var w in l)
                        {
                            Button b = new Button();
                            b.FontSize = 14;
                            b.Margin = new Thickness(2, 2, 2, 2);
                            b.Content = w;
                            b.Click += OnMove;
                            WrapPanelShow.Children.Add(b);
                        }
                    }

                    else if (TabControlMain.SelectedIndex == 3)
                    {
                        BLL.HDWallet.UpdateIsBackUp(model.MneHash);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                this.Close();
            }
        }

        private string InputPasswrod()
        {
            //var password = WindowPassword.GetPassword("要求加密助记词的密码");
            string password;
            if (!WindowPassword.GetPassword(this, LanguageHelper.GetTranslationText("助记词") + "[" + System.DateTime.Now.ToString(Common.DateTimeHelper.DefaultDateTimeFormat) + "]", LanguageHelper.GetTranslationText("要求加密助记词的密码"), out password))
            {
                return string.Empty;
            }

            string ph = BLL.HDWallet.GetPasswordHash(password, model.Salt);
            if (ph == model.UserPasswordHash)
            {
                return password;
                //ok
            }
            else
            {
                if (MessageBox.Show (this, LanguageHelper.GetTranslationText("输入密码错误，点击‘是’重新输入，点击‘否’退出创建钱包。"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    return InputPasswrod();
                }
                else
                {
                    this.Close();
                    return string.Empty;
                }
            }
        }

        private void OnNextStep(object sender, RoutedEventArgs e)
        {
            Step = 2;
            TabControlMain.SelectedIndex = Step;
        }


        private void OnMove(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                var b = (Button)sender;
                if (b.Parent == WrapPanelShow)
                {
                    WrapPanelShow.Children.Remove(b);
                    WrapPanelOk.Children.Add(b);
                }
                else if (b.Parent == WrapPanelOk)
                {
                    WrapPanelOk.Children.Remove(b);
                    WrapPanelShow.Children.Add(b);
                }
            }

        }

        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            string mne = string.Empty;
            //判断输入是否和助记词一样，
            foreach (var b in WrapPanelOk.Children)
            {
                if (b is Button)
                {
                    Button but = (Button)b;
                    mne = mne + but.Content + " ";
                }
            }
            mne = mne.Trim();

            //计算hash
            string mh = BLL.HDWallet.GetMneHash(mne, model.Salt);

            if (mh == model.MneHash)
            {
                //ok
                Step = 3;
                TabControlMain.SelectedIndex = Step;
            }
            else
            {
                MessageBox.Show (this, LanguageHelper.GetTranslationText("输入助记词顺序错误。"));
            }

        }

        private void OnClsoe(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            //this.Close();
        }

        public static bool NewMne(Window _owner)
        {
            WindowNewMnemonic w = new WindowNewMnemonic();
            w.Owner = _owner;
            bool result = w.ShowDialog() == true;

            if (w.model != null && !result) 
            {
                //删除数据
                BLL.HDWallet.DeleteHdWallet(w.model.MneId);
            }

            return result;
        }

        private void OnInputPassword(object sender, RoutedEventArgs e)
        {
            //测试密码强度
            var v = (int)Common.PasswordHelper.PasswordStrength(PasswordBoxInput1.Password);
            ProgressBar1.Value = v;
        }
    }
}
