using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace BlockChain.Share
{
    /// <summary>
    /// WindowLanguage.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLanguage : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowLanguage()
        {
            InitializeComponent();
        }


        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {   
            ListViewLanguage.ItemsSource = LanguageHelper.GetLanguageList().DefaultView;
            ShowLabelCurLan();

            TextBoxKey.Text = MicrosoftTextTranslation.key;
            TextBoxLocation.Text = MicrosoftTextTranslation.location;
            CheckBoxIsTranslateOnLine.IsChecked = LanguageHelper.IsTranslateOnLine == true;
        }



        private void LanguageIniOnClick(object sender, RoutedEventArgs e)
        {
            LanguageHelper.IniLangageData();
            ListViewLanguage.ItemsSource = LanguageHelper.GetLanguageList().DefaultView;
            ShowLabelCurLan();
        }

        private void ShowLabelCurLan()
        {
            var sm = LanguageHelper.GetSelectedModel();
            if (sm != null)
            {
                LabelCurLan.Content = sm.CultureInfoName + " - " + sm.NativeName + " - " + sm.DisplayName;
            }
            else
            {
                LabelCurLan.Content = LanguageHelper.GetAppLanguageCode();
            }
        }

        private async void InOnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                try
                {
                    string BackUpPath = Share.ShareParam.BackUpDir; //System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BackUp");

                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.InitialDirectory = BackUpPath;
                    if (ofd.ShowDialog() == true)
                    {
                        var b = sender as Button;
                        var CultureInfoName = b.Tag.ToString();

                        int ok_num = 0;
                        int fail_num = 0;

                        this.Cursor = Cursors.Wait;
                        ((IMainWindow)Application.Current.MainWindow).ShowProcessing();
                        try
                        {
                            //using (TransactionScope scope = new TransactionScope())       //不要事务， 失败的记录一下日志就好  //用了事务可能更快！
                            //{
                            System.Data.DataSet ds = new System.Data.DataSet();
                            ds.ReadXml(ofd.FileName);

                            var dt = ds.Tables[0];

                            if (dt != null)
                            {
                                var l1 = Model.T_TranslationText.DataTable2List(dt);
                                var t = Task.Run(() =>
                                {
                                    foreach (var m in l1)
                                    {
                                        try
                                        {
                                            if (m.LanCode == CultureInfoName)
                                            {
                                                if (DAL.T_TranslationText.Exist(ShareParam.DbConStr, m.OriginalHash, m.LanCode))
                                                {
                                                    DAL.T_TranslationText.Update(Share.ShareParam.DbConStr, m);
                                                }
                                                else
                                                {
                                                    DAL.T_TranslationText.Insert(Share.ShareParam.DbConStr, m);
                                                }
                                                ok_num++;
                                            }
                                            else
                                            {
                                                log.Error("language code is inconsistent, Selected Is " + CultureInfoName + "; But Inserting Is " + m.LanCode + ". OriginalHash is " + m.OriginalHash + ".");
                                                fail_num++;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error("DAL.T_TranslationText.Insert Error", ex);
                                            fail_num++;
                                        }
                                    }
                                });
                                await t;
                            }
                            //}
                        }
                        finally
                        {
                            ((IMainWindow)Application.Current.MainWindow).ShowProcesed();
                            Cursor = Cursors.Arrow;
                        }

                        MessageBox.Show("Success Number Is " + ok_num.ToString() + "; Fail Number Is " + fail_num.ToString());
                    }
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(this, ex.Message);
                }
            }

        }

        private void OutOnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                try
                {
                    var b = sender as Button;
                    var CultureInfoName = b.Tag.ToString();

                    string FileName = System.IO.Path.Combine(Share.ShareParam.BackUpDir, CultureInfoName + "_TranslationText(" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ").xml");

                    log.Info("TranslationText Export File: " + FileName);  //test

                    var ds = LanguageHelper.GetLanAllTranslationText(CultureInfoName);
                    ds.WriteXml(FileName, System.Data.XmlWriteMode.WriteSchema);        //必须写入架构，否则读取会出错！

                    System.Diagnostics.Process.Start("Explorer", "/select," + FileName);
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(this, ex.Message);
                }
            }
        }

        private void DelOnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                try
                {
                    var b = sender as Button;
                    var CultureInfoName = b.Tag.ToString();

                    Share.LanguageHelper.DeleteAllTranslateText(CultureInfoName);
                    MessageBox.Show(this, "Translate Text Is Deleted!");
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(this, ex.Message);
                }
            }

        }


        private void LanguageOnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                var rb = sender as RadioButton;
                var cname = rb.Tag.ToString();

                LanguageHelper.UpdateSelectedLanguage(cname);
                ListViewLanguage.ItemsSource = LanguageHelper.GetLanguageList().DefaultView;

                ShowLabelCurLan();
            }
        }
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cntext = @"Web3会加速世界经济一体化发展。";
                var entext = MicrosoftTextTranslation.TranslatFromZh(cntext, "en").Result;

                if (entext.StartsWith("M?"))
                {
                    MessageBox.Show(this, "Failed 1!", "Message");
                }
                else
                {
                    string s = "Zh Text: " + cntext + Environment.NewLine
                        + "En Text(Translation): " + entext + Environment.NewLine
                        + "Translation Is Ok!";

                    MessageBox.Show(this, s, "Message");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed 2!" + ex.Message, "Message");
            }
        }

        private void SaveKey_Click(object sender, RoutedEventArgs e)
        {
            MicrosoftTextTranslation.key = TextBoxKey.Text.Trim();
            MicrosoftTextTranslation.location = TextBoxLocation.Text.Trim();
            LanguageHelper.IsTranslateOnLine  = CheckBoxIsTranslateOnLine.IsChecked  == true;

            MessageBox.Show("OK");
        }

        private void GoTo_Click(object sender, RoutedEventArgs e)
        {
            string url = @"https://portal.azure.com/";
            System.Diagnostics.Process.Start("explorer.exe", url);
        }


        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

     
    }
}
