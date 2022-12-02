using System;
using System.Windows;

namespace BlockChain.Share
{
    /// <summary>
    /// WindowModifyAddressAlias.xaml 的交互逻辑
    /// </summary>
    public partial class WindowModifyAddressAlias : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowModifyAddressAlias()
        {
            InitializeComponent();
            this.Title = LanguageHelper.GetTranslationText(this.Title);
            TextBoxNewAlias.Focus();
            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void OnUpdateAlias(object sender, RoutedEventArgs e)
        {
            try
            {
                //  这里只是输入，不在这里修改！
                //  BLL.Address.UpdateAddressAlias(this.TextBoxAddress.Text, this.TextBoxNewAlias.Text.Trim());
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show (this, ex.Message);
            }
        }


        public static string GetUpdateAlias(string address, string alias, Window _owner)
        {
            WindowModifyAddressAlias w = new WindowModifyAddressAlias();
            w.Owner = _owner;
            w.TextBoxAddress.Text = address;
            w.TextBoxAlias.Text = alias;
            if (w.ShowDialog() == true)
            {
            return w.TextBoxNewAlias.Text.Trim() ;
            }
            else
            {
                return alias;
            }
        }

    }

}
