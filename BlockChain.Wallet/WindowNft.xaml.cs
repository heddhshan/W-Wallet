using BlockChain.Share;
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

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowNft.xaml 的交互逻辑
    /// </summary>
    public partial class WindowNft : Window
    {

        //nft 有很多可以做的，包括 去中心化交易所，nft 资产， nft的一些应用（优惠券，积分，演唱会票据，等，下一步做nft票据：包括电影票，足球门票，等等，需要单独做一个或一系列APP）
        //这个地方，现在简单处理一下转账就可以了，以后可以增加一个荷兰拍卖和挂单交易(其实是一个，都是拍卖，挂单交易只是金额不变而已)。
        //总之nft可以单独做一个nft的钱包，这里就是简单处理。

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public WindowNft()
        {
            InitializeComponent();

            var dv = new BLL.Address().GetAllTxAddress();   
            ComboBoxAddress1.SelectedValuePath = "Address";
            ComboBoxAddress1.ItemsSource = dv;
        }

        private async void Transfer_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                string from = this.ComboBoxAddress1.SelectedValue.ToString();
                string nft = this.TextBoxNftTokenAddress.Text;
                string to = this.TextBoxTo.Text;
                long tokenid = long.Parse(this.TextBoxTokenId.Text);

                string password;
                if (!WindowPassword.GetPassword(this, from, LanguageHelper.GetTranslationText("输入密码"), out password))
                {
                    return;
                }
                var account = new BLL.Address().GetAccount(from, password);

                if (null == account)
                {
                    MessageBox.Show(this, LanguageHelper.GetTranslationText("账号不存在，多半是密码错误导致。"));
                    return;
                }

                var web3 = Share.ShareParam.GetWeb3(account);
                var service = web3.Eth.ERC721.GetContractService(nft);
                var param = new Nethereum.Contracts.Standards.ERC721.ContractDefinition.SafeTransferFromFunction()
                {

                    From = from,
                    To = to,
                    TokenId = tokenid,
                };
                var tx = await service.SafeTransferFromRequestAsync(param);

                string text = LanguageHelper.GetTranslationText(@"交易已提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                log.Error("*", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                this.Cursor = Cursors.Arrow;
            }
        }

        private async void Query_Click(object sender, RoutedEventArgs e)
        {

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                this.TextBoxOwnerAddress.Text = "***";
                string nft = this.TextBoxNftTokenAddress2.Text;
                long tokenid = long.Parse(this.TextBoxTokenId2.Text);
                var web3 = Share.ShareParam.GetWeb3();
                var service = web3.Eth.ERC721.GetContractService(nft);
                var owner = await service.OwnerOfQueryAsync(tokenid);
                this.TextBoxOwnerAddress.Text = owner;
            }
            catch (Exception ex)
            {
                log.Error("*", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                this.Cursor = Cursors.Arrow;
            }
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
