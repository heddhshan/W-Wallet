using BlockChain.Share;
using NBitcoin;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    /// WindowUniswapV2.xaml 的交互逻辑
    /// 创建交易，流动性管理，这些功能不需要的，做个交易就可以了。V2是做了这些，暂时隐藏UI，相关的执行代码并没有删除。
    /// </summary>
    public partial class WindowUniswap : Window
    {

        private const string UniswapRemark = @"
Uniswap 是当前最流行的去中心化交易所。
本客户端直接调用Uniswap的合约，基本参照Uniswap的官方App的调用方式，但有些地方简化了或者表现方式有细微差别：
1，在购买时候，显示当前价格和预计成交价格，其中预计成交价格中包含手续费（一般是0.3%），允许价差基于预计成交价基础上计算。
2，只支持通过一个交易对（Pair或Pool）来交易，不支持通过多个交易对（Pair或Pool）路由执行交易，适合流动性较大的交易对。
3，ERC20Token的授权（approve）需要单独操作，没有使用multicall合并。
4，只提供了交易功能。
如果需要使用Uniswap的高级功能，建议去Uniswap官网(https://app.uniswap.org)操作。
此处提供Uniswap客户端主要是方便某些用户的部分币币兑换操作。
";

        public static void ShowWindowUniswapV2()
        {
            WindowUniswap f = new WindowUniswap();
            f.Owner = App.Current.MainWindow;
            f.ShowDialog();
        }

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowUniswap()
        {
            InitializeComponent();

            TextBlockRemark.Text = LanguageHelper.GetTranslationText(UniswapRemark);

            log.Info(this.GetType().ToString() + ", Width=" + this.Width.ToString() + "; Height=" + this.Height.ToString());
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            var dv = new BLL.Address().GetAllTxAddress();
            ComboBoxAddress.SelectedValuePath = "Address";              //user address
            ComboBoxAddress.ItemsSource = dv;

            var TokenAView = Share.BLL.Token.GetAllSelectedToken().DefaultView;

            //兑换页面
            ComboBoxFromToken.SelectedValuePath = "Address";            //token address
            ComboBoxToToken.SelectedValuePath = "Address";
            ComboBoxFromToken.ItemsSource = TokenAView;
            ComboBoxToToken.ItemsSource = TokenAView;

            // 流动性页面 增加
            ComboBoxTokenA.SelectedValuePath = "Address";            //token address 
            ComboBoxTokenA.ItemsSource = TokenAView;
            ComboBoxTokenB.SelectedValuePath = "Address";            //token address 
            ComboBoxTokenB.ItemsSource = TokenAView;


            // 流动性页面 减少
            ComboBoxTokenAr.SelectedValuePath = "Address";            //token address 
            ComboBoxTokenAr.ItemsSource = TokenAView;
            ComboBoxTokenBr.SelectedValuePath = "Address";            //token address 
            ComboBoxTokenBr.ItemsSource = TokenAView;


            // 创建交易对页面
            ComboBoxTokenAc.SelectedValuePath = "Address";            //token address 
            ComboBoxTokenAc.ItemsSource = TokenAView;
            ComboBoxTokenBc.SelectedValuePath = "Address";            //token address 
            ComboBoxTokenBc.ItemsSource = TokenAView;


            #region Uniswap V3

            ComboBoxAddressV3.SelectedValuePath = "Address";              //user address
            ComboBoxAddressV3.ItemsSource = dv;

            ComboBoxFromTokenV3.SelectedValuePath = "Address";            //token address
            ComboBoxToTokenV3.SelectedValuePath = "Address";
            ComboBoxFromTokenV3.ItemsSource = TokenAView;
            ComboBoxToTokenV3.ItemsSource = TokenAView;

            #endregion
        }

        #region UniswapV2


        private int OldTabIndex = 0;            // TabControl 的 SelectionChanged 事件会（在 ComboBox SelectionChanged 中）触发 很讨人厌恶

        private async void TabControlUniswapV2OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControlUniswapV2.SelectedIndex == OldTabIndex)
            {
                return;
            }

            OldTabIndex = TabControlUniswapV2.SelectedIndex;

            await UpdateTabControlUniswapV2();
        }

        private async Task<bool> UpdateTabControlUniswapV2()
        {
            if (TabControlUniswapV2.SelectedIndex == 0)
            {
                return await UpdateV2Tab1();
            }

            else if (TabControlUniswapV2.SelectedIndex == 1)
            {
                if (ExpanderAdd.IsExpanded)
                {
                    return await UpdateV2Tab2Add();
                }

                if (ExpanderRemove.IsExpanded)
                {
                    return await UpdateV2Tab2Remove();
                }
            }

            return false;
        }


        //刷新所有：1，from token 余额；2，totoken，兑换金额；3，totoken,max金额；

        private async void ComboBoxAddressOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TabControlUniswapV2OnSelectionChanged(null, null);
            await UpdateTabControlUniswapV2();

            //TabControlUniswapV2OnSelectionChanged

            //if (ComboBoxAddress.SelectedValue != null)
            //{
            //    var UserAddress = ComboBoxAddress.SelectedValue.ToString();
            //    if (ComboBoxFromToken.SelectedValue != null)
            //    {
            //        var FromTokenAddress = ComboBoxFromToken.SelectedValue.ToString();
            //        //刷新from token 余额
            //        var TB = Task.Run(() => { return new BLL.Address().GetRealBalance(UserAddress, FromTokenAddress, false); });
            //        LabelFromTokenMax.Content = await TB;
            //    }
            //}
        }


        /// <summary>
        /// 刷新 Uniswap V2 的交易页面
        /// </summary>
        private async Task<bool> UpdateV2Tab1()
        {
            //都是重复代码
            if (TabControlUniswapV2.SelectedIndex != 0) { return false; }

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                if (ComboBoxAddress.SelectedValue != null)                              //1， 有地址
                {
                    var UserAddress = ComboBoxAddress.SelectedValue.ToString();
                    if (ComboBoxFromToken.SelectedValue != null)                        //2， 有 From Token
                    {
                        var FromTokenAddress = ComboBoxFromToken.SelectedValue.ToString();
                        //刷新from token 余额
                        var TB = Task.Run(() => { return new BLL.Address().GetRealBalance(UserAddress, FromTokenAddress, false); });
                        LabelFromTokenMax.Content = await TB;

                        if (ComboBoxToToken.SelectedValue != null && !string.IsNullOrEmpty(TextBoxFromToken.Text))         //3, 有 ToToken 和 交易金额
                        {
                            var FromTokenValue = decimal.Parse(TextBoxFromToken.Text);
                            var ToTokenAddress = ComboBoxToToken.SelectedValue.ToString();
                            //刷新 To 的两个值
                            RefreshToAmount(FromTokenAddress, FromTokenValue, ToTokenAddress);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
                return false;
            }
            finally
            {
                Cursor = Cursors.Arrow;
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
            }
        }

        private async void ComboBoxFromTokenOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFromToken.SelectedValue != null)
            {
                //TextBoxFromToken.Text = "";
                var FromTokenAddress = ComboBoxFromToken.SelectedValue.ToString();

                if (ComboBoxAddress.SelectedValue != null)
                {
                    var UserAddress = ComboBoxAddress.SelectedValue.ToString();
                    //刷新from token 余额
                    var TB = Task.Run(() => {
                        return new BLL.Address().GetRealBalance(UserAddress, FromTokenAddress, false);
                    });
                    LabelFromTokenMax.Content = await TB;
                }

                if (TextBoxFromToken.Text != string.Empty && ComboBoxToToken.SelectedValue != null)
                {
                    var FromTokenValue = decimal.Parse(TextBoxFromToken.Text);
                    var ToTokenAddress = ComboBoxToToken.SelectedValue.ToString();
                    //刷新 To 的两个值
                    RefreshToAmount(FromTokenAddress, FromTokenValue, ToTokenAddress);
                }
            }
        }

        private void TextBoxFromTokenOnLostFocus(object sender, RoutedEventArgs e)
        {
            if (ComboBoxFromToken.SelectedValue != null)
            {
                var FromTokenAddress = ComboBoxFromToken.SelectedValue.ToString();

                if (TextBoxFromToken.Text != string.Empty && ComboBoxToToken.SelectedValue != null)
                {
                    var FromTokenValue = decimal.Parse(TextBoxFromToken.Text);
                    var ToTokenAddress = ComboBoxToToken.SelectedValue.ToString();
                    //刷新 To 的两个值
                    RefreshToAmount(FromTokenAddress, FromTokenValue, ToTokenAddress);
                }
            }
        }


        private void ComboBoxToTokenOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxToToken.SelectedValue != null)
            {
                //LabelToToken.Content = "";
                var ToTokenAddress = ComboBoxToToken.SelectedValue.ToString();

                //刷新所有：1，from token 余额；2，totoken，兑换金额；3，totoken,max金额；
                if (ComboBoxFromToken.SelectedValue != null && TextBoxFromToken.Text != string.Empty)
                {
                    var FromTokenValue = decimal.Parse(TextBoxFromToken.Text);
                    var FromTokenAddress = ComboBoxFromToken.SelectedValue.ToString();
                    //刷新 To 的两个值                     //刷新 To token 交易金额                     //刷新 To token Max
                    RefreshToAmount(FromTokenAddress, FromTokenValue, ToTokenAddress);
                }
            }
        }


        private async void RefreshToAmount(string fromToken, decimal fromAmount, string toToken)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //刷新 To 的两个值                     //刷新 To token 交易金额                     //刷新 To token Max
                var web3 = Share.ShareParam.GetWeb3(Share.ShareParam.Web3Url);
                Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service router = new Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service(web3, ShareParam.AddressUniV2Router02);
                var AddressWETH = await router.WETHQueryAsync();        //注意：不能使用 ShareParam.AddressWETH !!!
                if (Share.ShareParam.IsAnEmptyAddress(fromToken)) { fromToken = AddressWETH; }
                if (Share.ShareParam.IsAnEmptyAddress(toToken)) { toToken = AddressWETH; }

                if (toToken == fromToken)
                {
                    return;
                }
                //刷新流动性值
                var factoryAddress = router.FactoryQueryAsync().Result;
                Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService factory = new Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService(web3, factoryAddress);
                var pair = await factory.GetPairQueryAsync(fromToken, toToken);
                log.Info("Pair:" + pair);
                if (Share.ShareParam.IsAnEmptyAddress(pair))
                {
                    LabelToToken.Content = "*";
                    LabelPriceV2.Content = "*";
                    LabelUserPriceV2.Content = "*";
                    LabelUserPriceDifV2.Content = "";

                    MessageBox.Show(this, LanguageHelper.GetTranslationText(LanguageHelper.GetTranslationText(@"没有这个交易对！")));
                    return;
                }

                var balance1 = (decimal)(new BLL.Address().GetRealBalance(pair, toToken));
                var balance2 = (decimal)(new BLL.Address().GetRealBalance(pair, fromToken));

                #region 计算当前价格。
                var price = balance1 / balance2;
                LabelPriceV2.Content = "1 : " + price.ToString("N9");

                #endregion


                #region 计算滑动价格  //计算swap的结果值

                var path = new List<string> { fromToken, toToken };
                var amountToken = (System.Numerics.BigInteger)((double)fromAmount * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(fromToken)));
                var amounts = await router.GetAmountsOutQueryAsync(amountToken, path);                                                          //这里会出错！???
                var toAmount = (decimal)((double)amounts[1] / Math.Pow(10, Share.BLL.Token.GetTokenDecimals(toToken)));
                LabelToToken.Content = toAmount.ToString("N" + Share.BLL.Token.GetTokenDecimals(toToken).ToString() + ""); ;                    //得到的token
                var swapprice = toAmount / fromAmount;
                var dif = (swapprice - price) * 100 / price;
                LabelUserPriceV2.Content = "1 : " + swapprice.ToString("N9");                                 //交易价格
                LabelUserPriceDifV2.Content = dif.ToString("N6") + "%";

                #endregion

            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
            }
        }

        private async void ButtonSwapOnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var User = ComboBoxAddress.SelectedValue.ToString();
                if (User == null) return;

                string Password;
                if (!WindowPassword.GetPassword(this, User, "Get Password", out Password)) return;

                var Account = new BLL.Address().GetAccount(User, Password);
                if (Account == null) { MessageBox.Show(this, @"No Account"); return; }

                if (ComboBoxFromToken.SelectedValue == null) { ComboBoxFromToken.Focus(); return; }
                if (ComboBoxToToken.SelectedValue == null) { ComboBoxToToken.Focus(); return; }
                var FromTokenAddress = ComboBoxFromToken.SelectedValue.ToString();
                var ToTokenAddress = ComboBoxToToken.SelectedValue.ToString();

                //var ThisPath = new List<string> { FromTokenAddress, ToTokenAddress };
                var ThisAmountIn = (System.Numerics.BigInteger)(double.Parse(TextBoxFromToken.Text) * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(FromTokenAddress)));
                var ThisAmountOutMin = (double.Parse(LabelToToken.Content.ToString()) * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(ToTokenAddress)));
                ThisAmountOutMin = ThisAmountOutMin * (100.00 - double.Parse(TextBoxSlippage.Text)) / 100;
                var ThisDeadline = Common.DateTimeHelper.ConvertDateTime2Int(System.DateTime.Now) + (int)(double.Parse(TextBoxTxDeadline.Text) * 60);

                var web3 = Share.ShareParam.GetWeb3(Account, Share.ShareParam.Web3Url);
                Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service router = new Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service(web3, ShareParam.AddressUniV2Router02);

                //var web3 = Share.ShareParam.GetWeb3(Share.ShareParam.Web3Url);
                //Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service router = new Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service(web3, ShareParam.AddressMyUniswapV2RouterV2);
                var AddressWETH = await router.WETHQueryAsync();        //注意：不能使用 ShareParam.AddressWETH !!!
                if (Share.ShareParam.IsAnEmptyAddress(FromTokenAddress)) { FromTokenAddress = AddressWETH; }
                if (Share.ShareParam.IsAnEmptyAddress(ToTokenAddress)) { ToTokenAddress = AddressWETH; }
                var ThisPath = new List<string> { FromTokenAddress, ToTokenAddress };


                //分三种情况： eth =》 token ; token => token; token => eth
                string tx;
                //if (Share.ShareParam.IsAnEmptyAddress(FromTokenAddress))
                if (AddressWETH == FromTokenAddress)
                {
                    //function swapExactETHForTokens(uint amountOutMin, address[] calldata path, address to, uint deadline)      //external      //payable      //returns(uint[] memory amounts);
                    Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.SwapExactETHForTokensFunction param1 = new Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.SwapExactETHForTokensFunction()
                    {
                        //AmountIn = ThisAmountIn,
                        AmountOutMin = (System.Numerics.BigInteger)ThisAmountOutMin,
                        Path = ThisPath,
                        To = User,
                        Deadline = ThisDeadline,
                        AmountToSend = ThisAmountIn,
                    };
                    tx = await router.SwapExactETHForTokensRequestAsync(param1);
                    Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2_SwapExactETHForTokens.ToString(), "UniswapV2_SwapExactETHForTokens");
                }
                else
                {
                    //处理授权   App.Current.MainWindow     public async Task<bool> UiErc20TokenApprove(Nethereum.Web3.Accounts.Account _account, string _token, string _spender, double _amount)
                    //BigInteger BigAmount = (BigInteger)(double.Parse(TextBoxFromToken.Text) * Math.Pow(10, 1));
                    bool IsOkApprove1 = await ((IMainWindow)App.Current.MainWindow).GetHelper().UiErc20TokenApprove(this, Account, FromTokenAddress, ShareParam.AddressUniV2Router02, ThisAmountIn);
                    if (!IsOkApprove1)
                    {
                        MessageBox.Show(this, "Erc20 Token Approve Failed!");
                        return;
                    }

                    //if (Share.ShareParam.IsAnEmptyAddress(ToTokenAddress))
                    if (AddressWETH == ToTokenAddress)
                    {
                        //function swapExactTokensForETH(uint amountIn, uint amountOutMin, address[] calldata path, address to, uint deadline) external returns(uint[] memory amounts);
                        Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.SwapExactTokensForETHFunction param2 = new Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.SwapExactTokensForETHFunction()
                        {
                            AmountIn = ThisAmountIn,
                            AmountOutMin = (System.Numerics.BigInteger)ThisAmountOutMin,
                            Path = ThisPath,
                            To = User,
                            Deadline = ThisDeadline,
                        };
                        tx = await router.SwapExactTokensForETHRequestAsync(param2);
                        Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2_SwapExactTokensForETH.ToString(), "UniswapV2_SwapExactTokensForETH");
                    }
                    else
                    {
                        //function swapExactTokensForTokens( uint amountIn,    uint amountOutMin,    address[] calldata path,    address to,   uint deadline) external returns(uint[] memory amounts);
                        Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.SwapExactTokensForTokensFunction param3 = new Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.SwapExactTokensForTokensFunction()
                        {

                            AmountIn = ThisAmountIn,
                            AmountOutMin = (System.Numerics.BigInteger)ThisAmountOutMin,
                            Path = ThisPath,
                            To = User,
                            Deadline = ThisDeadline,
                        };
                        tx = await router.SwapExactTokensForTokensRequestAsync(param3);
                        Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2_SwapExactTokensForTokens.ToString(), "UniswapV2_SwapExactTokensForTokens");
                    }
                }

                string text = LanguageHelper.GetTranslationText(@"交易已经提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                log.Error("OnSwap", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void GoToUniswapWeb(object sender, RoutedEventArgs e)
        {
            var url = @"https://app.uniswap.org";
            System.Diagnostics.Process.Start("explorer.exe", url);
        }


        private async void ButtonAddLiquidityOnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                var User = ComboBoxAddress.SelectedValue.ToString();
                if (User == null) return;

                string Password;
                if (!WindowPassword.GetPassword(this, User, "Get Password", out Password)) return;

                var Account = new BLL.Address().GetAccount(User, Password);
                if (Account == null) { MessageBox.Show(this, @"No Account"); return; }

                if (ComboBoxTokenA.SelectedValue == null) { ComboBoxTokenA.Focus(); return; }
                if (ComboBoxTokenB.SelectedValue == null) { ComboBoxTokenB.Focus(); return; }
                var ATokenAddress = ComboBoxTokenA.SelectedValue.ToString();
                var BTokenAddress = ComboBoxTokenB.SelectedValue.ToString();

                var AAmount = (System.Numerics.BigInteger)(double.Parse(TextBoxTokenA.Text) * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(ATokenAddress)));
                var BAmount = (System.Numerics.BigInteger)(double.Parse(TextBoxTokenB.Text.ToString()) * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(BTokenAddress)));

                var dif = double.Parse(TextBoxSlippageLiq.Text);
                var BAmountMin = (System.Numerics.BigInteger)((double)BAmount * (100.00 - dif) / 100);
                var AAmountMin = (System.Numerics.BigInteger)((double)AAmount * (100.00 - dif) / 100);

                var BAmountMax = (System.Numerics.BigInteger)((double)BAmount * (100.00 + dif) / 100);
                var AAmountMax = (System.Numerics.BigInteger)((double)AAmount * (100.00 + dif) / 100);

                var ThisDeadline = Common.DateTimeHelper.ConvertDateTime2Int(System.DateTime.Now) + (int)(double.Parse(TextBoxTxDeadlineLiq.Text) * 60);

                var web3 = Share.ShareParam.GetWeb3(Account, Share.ShareParam.Web3Url);
                Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service router = new Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service(web3, ShareParam.AddressUniV2Router02);
                var AddressWETH = await router.WETHQueryAsync();        //注意：不能使用 ShareParam.AddressWETH !!!

                if (Share.ShareParam.IsAnEmptyAddress(ATokenAddress)) { ATokenAddress = AddressWETH; }
                if (Share.ShareParam.IsAnEmptyAddress(BTokenAddress)) { BTokenAddress = AddressWETH; }
                var ThisPath = new List<string> { ATokenAddress, BTokenAddress };

                if (AddressWETH != ATokenAddress)
                {
                    //处理授权   App.Current.MainWindow     public async Task<bool> UiErc20TokenApprove(Nethereum.Web3.Accounts.Account _account, string _token, string _spender, double _amount)
                    bool IsOkApprove1 = await ((IMainWindow)App.Current.MainWindow).GetHelper().UiErc20TokenApprove(this, Account, ATokenAddress, ShareParam.AddressUniV2Router02, AAmountMax);    //(double.Parse(TextBoxTokenA.Text))
                    if (!IsOkApprove1)
                    {
                        MessageBox.Show(this, "Erc20 Token Approve Failed!");
                        return;
                    }
                }

                if (AddressWETH != BTokenAddress)
                {
                    //处理授权   App.Current.MainWindow     public async Task<bool> UiErc20TokenApprove(Nethereum.Web3.Accounts.Account _account, string _token, string _spender, double _amount)
                    bool IsOkApprove1 = await ((IMainWindow)App.Current.MainWindow).GetHelper().UiErc20TokenApprove(this, Account, BTokenAddress, ShareParam.AddressUniV2Router02, BAmountMax);    //(double.Parse(TextBoxTokenB.Text))
                    if (!IsOkApprove1)
                    {
                        MessageBox.Show(this, "Erc20 Token Approve Failed!");
                        return;
                    }
                }


                //function addLiquidityETH(
                //    address token,
                //    uint amountTokenDesired,
                //    uint amountTokenMin,
                //    uint amountETHMin,
                //    address to,
                //    uint deadline
                //) external payable returns(uint amountToken, uint amountETH, uint liquidity);

                //分三种情况： eth =》 token ; token => token; token => eth
                string tx;
                //if (Share.ShareParam.IsAnEmptyAddress(FromTokenAddress))
                if (AddressWETH == ATokenAddress)
                {
                    Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.AddLiquidityETHFunction param1 = new Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.AddLiquidityETHFunction()
                    {
                        Token = BTokenAddress,
                        AmountTokenDesired = BAmountMax,        // BAmount,
                        AmountTokenMin = BAmountMin,
                        AmountETHMin = AAmount,
                        To = User,
                        Deadline = ThisDeadline,
                        AmountToSend = AAmount,
                    };
                    tx = await router.AddLiquidityETHRequestAsync(param1);
                    Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2_SwapExactETHForTokens.ToString(), "UniswapV2_AddLiquidityETH");
                }
                else if (AddressWETH == BTokenAddress)
                {
                    //处理第二个 token 是 ETH 的情况
                    Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.AddLiquidityETHFunction param2 = new Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.AddLiquidityETHFunction()
                    {
                        Token = ATokenAddress,
                        AmountTokenDesired = AAmountMax, //AAmount,
                        AmountTokenMin = AAmountMin,
                        AmountETHMin = BAmount,
                        To = User,
                        Deadline = ThisDeadline,
                        AmountToSend = BAmount,
                    };
                    tx = await router.AddLiquidityETHRequestAsync(param2);
                    Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2_SwapExactTokensForETH.ToString(), "UniswapV2_AddLiquidityETH");
                }
                else
                {
                    //function addLiquidity(
                    //    address tokenA,
                    //    address tokenB,
                    //    uint amountADesired,
                    //    uint amountBDesired,
                    //    uint amountAMin,
                    //    uint amountBMin,
                    //    address to,
                    //    uint deadline
                    //) external returns(uint amountA, uint amountB, uint liquidity);

                    Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.AddLiquidityFunction param3 = new Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.AddLiquidityFunction()
                    {
                        TokenA = ATokenAddress,
                        TokenB = ATokenAddress,
                        AmountADesired = AAmount,
                        AmountBDesired = BAmount,
                        AmountAMin = AAmountMin,
                        AmountBMin = BAmountMin,
                        To = User,
                        Deadline = ThisDeadline,
                    };
                    tx = await router.AddLiquidityRequestAsync(param3);
                    Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2_SwapExactTokensForTokens.ToString(), "UniswapV2_SwapExactTokensForTokens");
                }

                string text = LanguageHelper.GetTranslationText(@"交易已经提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                log.Error("AddLiquidity", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async Task<bool> UpdateV2Tab2Add()
        {
            //都是重复代码
            if (TabControlUniswapV2.SelectedIndex != 1) { return false; }
            if (!ExpanderAdd.IsExpanded) { return false; }

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                if (ComboBoxAddress.SelectedValue != null)                              //1， 有地址
                {
                    var UserAddress = ComboBoxAddress.SelectedValue.ToString();

                    if (ComboBoxTokenA.SelectedValue != null)
                    {
                        var TokenAAddress = ComboBoxTokenA.SelectedValue.ToString();

                        //if (ComboBoxAddress.SelectedValue != null)
                        //{
                        //刷新from token 余额
                        var TB = Task.Run(() => {
                            return new BLL.Address().GetRealBalance(UserAddress, TokenAAddress, false);
                        });
                        LabelTokenAMax.Content = await TB;
                        //}

                    }
                    if (ComboBoxTokenB.SelectedValue != null)
                    {
                        var TokenBAddress = ComboBoxTokenB.SelectedValue.ToString();

                        //if (ComboBoxAddress.SelectedValue != null)
                        //{
                        //刷新from token 余额
                        var TB = Task.Run(() => {
                            return new BLL.Address().GetRealBalance(UserAddress, TokenBAddress, false);
                        });
                        LabelTokenBMax.Content = await TB;
                        //}
                    }

                    //刷新价格
                    if (ComboBoxTokenA.SelectedValue != null && ComboBoxTokenB.SelectedValue != null)
                    {
                        var TokenBAddress = ComboBoxTokenB.SelectedValue.ToString();
                        var TokenAAddress = ComboBoxTokenA.SelectedValue.ToString();
                        RefreshLiqPrice(TokenAAddress, TokenBAddress);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
                return false;
            }
            finally
            {
                Cursor = Cursors.Arrow;
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
            }
        }


        private async void ComboBoxTokenAOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxTokenA.SelectedValue != null)
            {
                //TextBoxFromToken.Text = "";
                var TokenAAddress = ComboBoxTokenA.SelectedValue.ToString();

                if (ComboBoxAddress.SelectedValue != null)
                {
                    var UserAddress = ComboBoxAddress.SelectedValue.ToString();
                    //刷新from token 余额
                    var TB = Task.Run(() => {
                        return new BLL.Address().GetRealBalance(UserAddress, TokenAAddress, false);
                    });
                    LabelTokenAMax.Content = await TB;


                    //刷新价格
                    if (ComboBoxTokenB.SelectedValue != null)
                    {
                        var TokenBAddress = ComboBoxTokenB.SelectedValue.ToString();
                        RefreshLiqPrice(TokenAAddress, TokenBAddress);
                    }
                }
            }
        }

        private void TextBoxTokenAOnLostFocus(object sender, RoutedEventArgs e)
        {
            if (ComboBoxTokenA.SelectedValue != null)
            {
                var TokenAAddress = ComboBoxTokenA.SelectedValue.ToString();

                if (ComboBoxTokenB.Text != string.Empty && ComboBoxTokenB.SelectedValue != null)
                {
                    var TokenBAddress = ComboBoxTokenB.SelectedValue.ToString();
                    RefreshLiqPrice(TokenAAddress, TokenBAddress);
                }
            }
        }

        private async void ComboBoxTokenBOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxTokenB.SelectedValue != null)
            {
                //TextBoxFromToken.Text = "";
                var TokenBAddress = ComboBoxTokenB.SelectedValue.ToString();

                if (ComboBoxAddress.SelectedValue != null)
                {
                    var UserAddress = ComboBoxAddress.SelectedValue.ToString();
                    //刷新from token 余额
                    var TB = Task.Run(() => {
                        return new BLL.Address().GetRealBalance(UserAddress, TokenBAddress, false);
                    });
                    LabelTokenBMax.Content = await TB;

                    //刷新价格
                    if (ComboBoxTokenA.SelectedValue != null)
                    {
                        var TokenAAddress = ComboBoxTokenA.SelectedValue.ToString();
                        RefreshLiqPrice(TokenAAddress, TokenBAddress);
                    }
                }
            }
        }


        private async void RefreshLiqPrice(string AToken, string BToken)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //刷新 To 的两个值                     //刷新 To token 交易金额                     //刷新 To token Max
                var web3 = Share.ShareParam.GetWeb3(Share.ShareParam.Web3Url);
                Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service router = new Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service(web3, ShareParam.AddressUniV2Router02);
                var AddressWETH = await router.WETHQueryAsync();        //注意：不能使用 ShareParam.AddressWETH !!!
                if (Share.ShareParam.IsAnEmptyAddress(AToken)) { AToken = AddressWETH; }
                if (Share.ShareParam.IsAnEmptyAddress(BToken)) { BToken = AddressWETH; }

                if (BToken == AToken)
                {
                    return;
                }
                //刷新流动性值
                var factoryAddress = router.FactoryQueryAsync().Result;
                Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService factory = new Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService(web3, factoryAddress);
                var pair = await factory.GetPairQueryAsync(AToken, BToken);
                log.Info("Pair:" + pair);
                if (Share.ShareParam.IsAnEmptyAddress(pair))
                {
                    LabelTokenALiq.Content = "*";
                    LabelTokenBLiq.Content = "*";
                    LabelPriceLiq.Content = "*";

                    MessageBox.Show(this, LanguageHelper.GetTranslationText(@"没有这个交易对！"));
                    return;
                }

                //Nethereum.Uniswap.Contracts.UniswapV2Pair.UniswapV2PairService ThisPair = new Nethereum.Uniswap.Contracts.UniswapV2Pair.UniswapV2PairService(web3, pair);

                var balance1 = (decimal)(new BLL.Address().GetRealBalance(pair, AToken));
                LabelTokenALiq.Content = balance1;
                var balance2 = (decimal)(new BLL.Address().GetRealBalance(pair, BToken));
                LabelTokenBLiq.Content = balance2;


                //var price =(double)( (BigInteger) ( (double)(balance2 / balance1) * Math.Pow(10.0, 9)) ) / Math.Pow(10.0, 9);       //保留 9 位小数
                var AAmount = decimal.Parse(TextBoxTokenA.Text);
                var BAmount = AAmount * balance2 / balance1;
                TextBoxTokenB.Text = BAmount.ToString("N" + Share.BLL.Token.GetTokenDecimals(BToken).ToString() + "");            //录入后可以修改

                //计算swap的结果值
                //var path = new List<string> { AToken, BToken };
                //var amountTokenA = (System.Numerics.BigInteger)((double)balance1 * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(AToken)));
                //var amountTokenB = (System.Numerics.BigInteger)((double)balance2 * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(BToken)));
                //if (amountTokenB > 0)
                //{
                LabelPriceLiq.Content = "1 : " + (balance2 / balance1).ToString("N9");
                //}
                //else
                //{
                //    LabelPriceLiq.Content = "1 : ?";
                //}
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
            }
        }


        /////////////////////////////////////////////////////////////////////////////////


        private async Task<bool> UpdateV2Tab2Remove()
        {
            //都是重复代码
            if (TabControlUniswapV2.SelectedIndex != 1) { return false; }
            if (!ExpanderRemove.IsExpanded) { return false; }

            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //要做的：1， 得到总的流动性值和用户的流动性值，2，得到 TokenA 和 TokenB 的总量！ 通过这些值计算出来用户可以领取的 TokenA TokenB 数量

                if (ComboBoxAddress.SelectedValue != null)                              //1， 有地址
                {
                    var UserAddress = ComboBoxAddress.SelectedValue.ToString();

                    if (ComboBoxTokenAr.SelectedValue != null && ComboBoxTokenBr.SelectedValue != null)
                    {
                        var AToken = ComboBoxTokenAr.SelectedValue.ToString();
                        var BToken = ComboBoxTokenBr.SelectedValue.ToString();

                        var web3 = Share.ShareParam.GetWeb3(Share.ShareParam.Web3Url);
                        Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service router = new Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service(web3, ShareParam.AddressUniV2Router02);

                        var AddressWETH = await router.WETHQueryAsync();        //注意：不能使用 ShareParam.AddressWETH !!!
                        if (Share.ShareParam.IsAnEmptyAddress(AToken)) { AToken = AddressWETH; }
                        if (Share.ShareParam.IsAnEmptyAddress(BToken)) { BToken = AddressWETH; }

                        var factoryAddress = await router.FactoryQueryAsync();
                        Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService factory = new Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService(web3, factoryAddress);
                        var pair = await factory.GetPairQueryAsync(AToken, BToken);
                        log.Info("Pair:" + pair);
                        if (Share.ShareParam.IsAnEmptyAddress(pair))
                        {
                            //TextBoxTokenAr.Text = "";
                            //TextBoxTokenBr.Text = "";
                            //SliderLiqR.Value = 0;
                            ClearRemoveLiqV2();
                            MessageBox.Show(this, LanguageHelper.GetTranslationText(@"没有这个交易对！"));
                            return false;
                        }
                        Nethereum.Uniswap.Contracts.UniswapV2Pair.UniswapV2PairService ThisPair = new Nethereum.Uniswap.Contracts.UniswapV2Pair.UniswapV2PairService(web3, pair);

                        var AllLiq = await ThisPair.TotalSupplyQueryAsync();
                        var UserLiq = await ThisPair.BalanceOfQueryAsync(UserAddress);
                        //SliderLiqR.Tag = UserLiq;                                       //1, 用户流动性

                        Nethereum.StandardTokenEIP20.StandardTokenService Erc20TokenA = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, AToken);
                        Nethereum.StandardTokenEIP20.StandardTokenService Erc20TokenB = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, BToken);

                        var AllAAmount = (double)await Erc20TokenA.BalanceOfQueryAsync(pair) / Math.Pow(10, Share.BLL.Token.GetTokenDecimals(AToken));
                        var AllBAmount = (double)await Erc20TokenB.BalanceOfQueryAsync(pair) / Math.Pow(10, Share.BLL.Token.GetTokenDecimals(BToken)); ;

                        //记录用户的流动性 Token 数量
                        TextBoxTokenAr.Tag = AllAAmount * (double)UserLiq / (double)AllLiq;         //2, 用户Token A 最大可取数量
                        TextBoxTokenBr.Tag = AllBAmount * (double)UserLiq / (double)AllLiq;         //3, 用户Token B 最大可取数量

                        //显示最大值
                        SliderLiqR.Tag = UserLiq;                   // 这个值是准确的 BigInter 类型
                        SliderLiqR.Maximum = (double)UserLiq;       // 这个值可能不准确，是 double 类型
                        SliderLiqR.Value = (double)UserLiq;
                        TextBoxTokenAr.Text = ((double)(TextBoxTokenAr.Tag)).ToString("N" + Share.BLL.Token.GetTokenDecimals(AToken).ToString());
                        TextBoxTokenBr.Text = ((double)(TextBoxTokenBr.Tag)).ToString("N" + Share.BLL.Token.GetTokenDecimals(AToken).ToString());

                        return true;
                    }
                }

                ClearRemoveLiqV2();

                return false;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(this, ex.Message);
                return false;
            }
            finally
            {
                Cursor = Cursors.Arrow;
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
            }
        }

        private void ClearRemoveLiqV2()
        {
            TextBoxTokenAr.Tag = 0;         //2, 用户Token A 最大可取数量
            TextBoxTokenBr.Tag = 0;        //3, 用户Token B 最大可取数量
            TextBoxTokenAr.Text = "0";
            TextBoxTokenBr.Text = "0";

            SliderLiqR.Tag = 0;
            SliderLiqR.Maximum = 0;
            SliderLiqR.Value = 0;
        }

        private async void ComboBoxTokenArOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateV2Tab2Remove();
        }

        private async void ComboBoxTokenBrOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateV2Tab2Remove();
        }

        private async void ButtonRemoveLiquidityOnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                if (string.IsNullOrEmpty(TextBoxTokenAr.Text) || string.IsNullOrEmpty(TextBoxTokenBr.Text) || SliderLiqR.Value == 0.0)
                {
                    return;
                }

                double AAmount = double.Parse(TextBoxTokenAr.Text);
                double BAmount = double.Parse(TextBoxTokenBr.Text);

                BigInteger UserLiq = (BigInteger)(SliderLiqR.Value);
                //特殊处理最大值，因为使用 double （或 float） 后，数据会变得不准确！  .net 系统和 以太坊的数字系统并不完全一一对应。 如果不这么处理会导致剩余很少的流动性值无法取出来。
                var ul = SliderLiqR.Value;
                if (ul == SliderLiqR.Maximum)
                {
                    UserLiq = (BigInteger)SliderLiqR.Tag;
                }

                if (AAmount == 0 || BAmount == 0 || UserLiq == 0)
                {
                    return;
                }

                string TokenA = ComboBoxTokenAr.SelectedValue.ToString();
                string TokenB = ComboBoxTokenBr.SelectedValue.ToString();

                var dif = double.Parse(TextBoxSlippageLiqR.Text);
                BigInteger AAmountMin = (BigInteger)(AAmount * (100 - dif) / 100 * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(TokenA)));
                BigInteger BAmountMin = (BigInteger)((BAmount * (100 - dif) / 100) * Math.Pow(10, Share.BLL.Token.GetTokenDecimals(TokenB)));

                var ThisDeadline = Common.DateTimeHelper.ConvertDateTime2Int(System.DateTime.Now) + (int)(double.Parse(TextBoxTxDeadlineLiqR.Text) * 60);

                var User = ComboBoxAddress.SelectedValue.ToString();
                if (User == null) return;

                string Password;
                if (!WindowPassword.GetPassword(this, User, "Get Password", out Password)) return;

                var Account = new BLL.Address().GetAccount(User, Password);
                if (Account == null) { MessageBox.Show(this, @"No Account"); return; }

                var web3 = Share.ShareParam.GetWeb3(Account, Share.ShareParam.Web3Url);
                Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service router = new Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service(web3, ShareParam.AddressUniV2Router02);
                var AddressWETH = await router.WETHQueryAsync();        //注意：不能使用 ShareParam.AddressWETH !!!

                if (Share.ShareParam.IsAnEmptyAddress(TokenA)) { TokenA = AddressWETH; }
                if (Share.ShareParam.IsAnEmptyAddress(TokenB)) { TokenB = AddressWETH; }

                var factoryAddress = router.FactoryQueryAsync().Result;
                Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService factory = new Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService(web3, factoryAddress);
                var pair = await factory.GetPairQueryAsync(TokenA, TokenB);
                if (Share.ShareParam.IsAnEmptyAddress(pair))
                {
                    MessageBox.Show(this, LanguageHelper.GetTranslationText(@"没有这个交易对！"));
                    return;
                }

                //处理授权   因为 流动性值 要 pair.burn，而调用 burn 是通过 router 调用！
                BigInteger UserAllLiq = (BigInteger)SliderLiqR.Tag;
                bool IsOkApprove1 = await ((IMainWindow)App.Current.MainWindow).GetHelper().UiErc20TokenApprove(this, Account, pair, ShareParam.AddressUniV2Router02, UserAllLiq);
                if (!IsOkApprove1)
                {
                    MessageBox.Show(this, "Erc20 Token Approve Failed!");
                    return;
                }


                #region uniswap code

                //    // **** REMOVE LIQUIDITY ****
                //    function removeLiquidity(
                //        address tokenA,
                //        address tokenB,
                //        uint liquidity,
                //        uint amountAMin,
                //        uint amountBMin,
                //        address to,
                //        uint deadline
                //    ) public virtual override ensure(deadline) returns(uint amountA, uint amountB)
                //{
                //    address pair = UniswapV2Library.pairFor(factory, tokenA, tokenB);
                //    IUniswapV2Pair(pair).transferFrom(msg.sender, pair, liquidity); // send liquidity to pair
                //    (uint amount0, uint amount1) = IUniswapV2Pair(pair).burn(to);
                //    (address token0,) = UniswapV2Library.sortTokens(tokenA, tokenB);
                //    (amountA, amountB) = tokenA == token0 ? (amount0, amount1) : (amount1, amount0);
                //    require(amountA >= amountAMin, 'UniswapV2Router: INSUFFICIENT_A_AMOUNT');
                //    require(amountB >= amountBMin, 'UniswapV2Router: INSUFFICIENT_B_AMOUNT');
                //}
                //function removeLiquidityETH(
                //    address token,
                //    uint liquidity,
                //    uint amountTokenMin,
                //    uint amountETHMin,
                //    address to,
                //    uint deadline
                //) public virtual override ensure(deadline) returns(uint amountToken, uint amountETH)
                //{
                //    (amountToken, amountETH) = removeLiquidity(
                //        token,
                //        WETH,
                //        liquidity,
                //        amountTokenMin,
                //        amountETHMin,
                //        address(this),
                //        deadline
                //    );
                //    TransferHelper.safeTransfer(token, to, amountToken);
                //    IWETH(WETH).withdraw(amountETH);
                //    TransferHelper.safeTransferETH(to, amountETH);
                //}

                #endregion

                string tx;
                if (AddressWETH == TokenA)
                {
                    Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.RemoveLiquidityETHFunction param1 = new Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.RemoveLiquidityETHFunction()
                    {
                        Token = TokenB,
                        Liquidity = UserLiq,
                        AmountTokenMin = BAmountMin,
                        AmountETHMin = AAmountMin,
                        To = User,
                        Deadline = ThisDeadline,
                    };
                    tx = await router.RemoveLiquidityETHRequestAsync(param1);
                    Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2_SwapExactETHForTokens.ToString(), "UniswapV2_RemoveLiquidityETH");
                }
                else if (AddressWETH == TokenB)
                {
                    Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.RemoveLiquidityETHFunction param2 = new Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.RemoveLiquidityETHFunction()
                    {
                        Token = TokenA,
                        Liquidity = UserLiq,
                        AmountTokenMin = AAmountMin,
                        AmountETHMin = BAmountMin,
                        To = User,
                        Deadline = ThisDeadline,
                    };
                    tx = await router.RemoveLiquidityETHRequestAsync(param2);
                    Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2_SwapExactETHForTokens.ToString(), "UniswapV2_RemoveLiquidityETH");
                }
                else
                {
                    Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.RemoveLiquidityFunction param3 = new Nethereum.Uniswap.Contracts.UniswapV2Router02.ContractDefinition.RemoveLiquidityFunction()
                    {
                        TokenA = TokenA,
                        TokenB = TokenB,
                        Liquidity = UserLiq,
                        AmountAMin = AAmountMin,
                        AmountBMin = BAmountMin,
                        To = User,
                        Deadline = ThisDeadline,
                    };
                    tx = await router.RemoveLiquidityRequestAsync(param3);
                    Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2_SwapExactETHForTokens.ToString(), "UniswapV2_RemoveLiquidity");
                }

                string text = LanguageHelper.GetTranslationText(@"交易已经提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                log.Error("OnSwap", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void AddOnExpanded(object sender, RoutedEventArgs e)
        {
            ExpanderRemove.IsExpanded = !ExpanderAdd.IsExpanded;
        }

        private void RemoveOnExpanded(object sender, RoutedEventArgs e)
        {
            ExpanderAdd.IsExpanded = !ExpanderRemove.IsExpanded;
        }

        private void SliderLiqR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ComboBoxTokenAr.SelectedValue == null || ComboBoxTokenBr.SelectedValue == null)
            {
                return;
            }
            if (TextBoxTokenAr.Tag == null || TextBoxTokenBr.Tag == null)
            {
                return;
            }

            string TokenA = ComboBoxTokenAr.SelectedValue.ToString();
            string TokenB = ComboBoxTokenBr.SelectedValue.ToString();

            var UserLiq = SliderLiqR.Maximum;

            double UserTokenAMax = (double)TextBoxTokenAr.Tag;         //2, 用户Token A 最大可取数量
            double UserTokenBMax = (double)TextBoxTokenBr.Tag;         //3, 用户Token B 最大可取数量

            if (UserLiq == 0 || UserTokenAMax == 0 || UserTokenBMax == 0)
            { return; }

            var SelectValue = SliderLiqR.Value;

            var da = Share.BLL.Token.GetTokenDecimals(TokenA);
            var db = Share.BLL.Token.GetTokenDecimals(TokenB);

            var TokenAUser = (double)UserTokenAMax * SelectValue / UserLiq;
            //TokenAUser = TokenAUser * Math.Pow(10,da );
            var TokenBUser = (double)UserTokenBMax * SelectValue / UserLiq;
            //TokenBUser = TokenBUser * Math.Pow(10, db);

            TextBoxTokenAr.Text = TokenAUser.ToString("N" + da.ToString());
            TextBoxTokenBr.Text = TokenBUser.ToString("N" + db.ToString());

            LabelLiqR.Content = SliderLiqR.Value.ToString("N0");

            //TextBoxTokenAr.Text = TokenAUser.ToString("N");
            //TextBoxTokenBr.Text = TokenBUser.ToString("N");
        }


        /// <summary>
        /// 创建交易对
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonCreatePairOnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                if (ComboBoxTokenAc.SelectedValue == null || ComboBoxTokenBc.SelectedValue == null || ComboBoxAddress.SelectedValue == null)
                {
                    return;
                }

                var AToken = ComboBoxTokenAc.SelectedValue.ToString();
                var BToken = ComboBoxTokenBc.SelectedValue.ToString();

                var User = ComboBoxAddress.SelectedValue.ToString();
                string Password;
                if (!WindowPassword.GetPassword(this, User, "Get Password", out Password)) return;

                var Account = new BLL.Address().GetAccount(User, Password);
                if (Account == null) { MessageBox.Show(this, @"No Account"); return; }

                var web3 = Share.ShareParam.GetWeb3(Account, Share.ShareParam.Web3Url);
                Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service router = new Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service(web3, ShareParam.AddressUniV2Router02);

                var factoryAddress = router.FactoryQueryAsync().Result;
                Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService factory = new Nethereum.Uniswap.Contracts.UniswapV2Factory.UniswapV2FactoryService(web3, factoryAddress);

                var AddressWETH = await router.WETHQueryAsync();        //注意：不能使用 ShareParam.AddressWETH !!!
                if (Share.ShareParam.IsAnEmptyAddress(AToken)) { AToken = AddressWETH; }
                if (Share.ShareParam.IsAnEmptyAddress(BToken)) { BToken = AddressWETH; }

                var pair = await factory.GetPairQueryAsync(AToken, BToken);
                if (!Share.ShareParam.IsAnEmptyAddress(pair))
                {
                    MessageBox.Show(this, LanguageHelper.GetTranslationText(@"这个交易对已经存在，不能再创建！"));
                    return;
                }

                var tx = await factory.CreatePairRequestAsync(AToken, BToken);
                Share.BLL.TransactionReceipt.LogTx(User, tx, TxUserMethod.UniswapV2Factory_CreatePair.ToString(), "UniswapV2Factory_CreatePair");

                string text = LanguageHelper.GetTranslationText(@"交易已经提交到以太坊网络，点击‘确定’查看详情。");
                string url = Share.ShareParam.GetTxUrl(tx);
                if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                log.Error("OnSwap", ex);
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }

        }


        #endregion


        #region UniswapV3 相关  net 客户端和 Js 客户端不一样。 js客户端处理参见： https://learnblockchain.cn/article/2580

        private int OldTabIndexV3 = 0;            // TabControl 的 SelectionChanged 事件会（在 ComboBox SelectionChanged 中）触发 很讨人厌恶

        private void TabControlUniswapV3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControlUniswapV3.SelectedIndex == OldTabIndexV3)
            {
                return;
            }
            OldTabIndexV3 = TabControlUniswapV3.SelectedIndex;
        }

        private void ComboBoxAddressV3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //账号发生变化 
                ShowFromTokenBalanceV3();
                UpdateRecipient();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private void ShowFromTokenBalanceV3()
        {
            if (ComboBoxAddressV3.SelectedValue != null && ComboBoxFromTokenV3.SelectedValue != null)
            {
                string from = ComboBoxAddressV3.SelectedValue.ToString();
                string token = ComboBoxFromTokenV3.SelectedValue.ToString();

                LabelFromTokenMaxV3.Content = Share.ShareParam.GetRealBalance(from, token, false);
            }
        }

        private async void ComboBoxFromTokenV3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //from token  发生变化
                await DoUpdatePrice();

                ShowFromTokenBalanceV3();

                await DoRefreshUserPriceV3();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private async void ComboBoxToTokenV3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //to token  发生变化
                await DoUpdatePrice();
                //RefreshGetToTokenAmount();

                await DoRefreshUserPriceV3();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        ///  更新价格, 价格保存起来  LabelPriceV3.Tag = price;
        /// </summary>
        /// <param name="Fee10000"></param>
        /// <returns></returns>
        private async Task<bool> DoUpdatePrice(long Fee10000 = 0)
        {
            if (ComboBoxFromTokenV3.SelectedValue != null && ComboBoxToTokenV3.SelectedValue != null)
            {
                try
                {
                    string AToken = ComboBoxFromTokenV3.SelectedValue.ToString();
                    string BToken = ComboBoxToTokenV3.SelectedValue.ToString();

                    double price; string pool;

                    if (Fee10000 == 0)
                    {
                        // 第一种做法是取第一个池子
                        //var haspool = await Share.UniswapTokenPrice.getFeePoolV3(AToken, BToken);
                        //Fee10000 = GetFirstFee10000V3(haspool);
                        //SetUiFee10000V3(Fee10000);
                        ////   public static async Task<(double price, string pool,  long fee10000)> get1PricePoolFeeV3(string token0, string token1)
                        ////(price, pool, Fee10000) = await Share.UniswapTokenPrice.get1PricePoolFeeV3(AToken, BToken);//getPricePoolV3
                        //(price, pool) = await Share.UniswapTokenPrice.getPricePoolV3(AToken, BToken, Fee10000);//getPricePoolV3

                        //第二种做法是取流动性最大的池子
                        var Liq = await Share.UniswapTokenPrice.getFeePoolLiqV3(AToken, BToken);
                        Fee10000 = GetSelectedFee10000V3(Liq);
                        SetUiFee10000V3(Fee10000);
                        (price, pool) = await Share.UniswapTokenPrice.getPricePoolV3(AToken, BToken, Fee10000);     //getPricePoolV3
                    }
                    else
                    {
                        (price, pool) = await Share.UniswapTokenPrice.getPricePoolV3(AToken, BToken, Fee10000);
                    }

                    if (0 < price)
                    {
                        ButtonPoolV3.Content = pool;
                        LabelPriceV3.Content = "1 : " + price.ToString("N9");
                        LabelPriceV3.Tag = price;
                    }
                    else
                    {
                        ButtonPoolV3.Content = "No Pool Or No Liquidity";
                        LabelPriceV3.Content = "1 : ???";
                        LabelPriceV3.Tag = -1;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    return false;
                }
            }
            return false;
        }

        private void SetUiFee10000V3(long Fee10000)
        {
            FeeIsDoing = true;  //不要执行 FeeV3_Checked 事件
            try
            {
                // Share.UniswapTokenPrice.Fee10000V3[0]
                if (Fee10000 == 100)        { RadioButton100.IsChecked = true;      }
                else if (Fee10000 == 500)   { RadioButton500.IsChecked = true;      }
                else if (Fee10000 == 3000)  { RadioButton3000.IsChecked = true;     }
                else if (Fee10000 == 10000) { RadioButton10000.IsChecked = true;    }
            }
            finally
            {
                FeeIsDoing = false;
            }
        }


        //private long GetFirstFee10000V3(bool[] HasFees)
        //{
        //    long FirstFee = 0;
        //    if (HasFees[0]) { FirstFee = 100; }
        //    else if (HasFees[0]) { FirstFee = 500; }
        //    else if (HasFees[0]) { FirstFee = 3000; }
        //    else if (HasFees[0]) { FirstFee = 10000; }
        //    return FirstFee;
        //}

        private long GetSelectedFee10000V3(double[] liqs)
        {
            long Fee = Share.UniswapTokenPrice.Fee10000V3[0];
            for (int i = 1; i < liqs.Length; i++)
            {
                if (liqs[i - 1] < liqs[i]) {
                    Fee = Share.UniswapTokenPrice.Fee10000V3[i];
                }            
            }                     
            return Fee;
        }

        private long getUiFee10000V3()
        {
            FeeIsDoing = true;  //不要执行 FeeV3_Checked 事件
            try
            {
                if (RadioButton100.IsChecked == true) { return 100; }
                if (RadioButton500.IsChecked == true) { return 500; }
                if (RadioButton3000.IsChecked == true) { return 3000; }
                if (RadioButton10000.IsChecked == true) { return 10000; }
                return 0;
                //throw new Exception("No Fee V3");
            }
            finally
            {
                FeeIsDoing = false;
            }
        }

        private async void TextBoxFromTokenV3_LostFocus(object sender, RoutedEventArgs e)
        {
            //输入金额文本框，失去焦点
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {
                //RefreshGetToTokenAmount();
                await DoRefreshUserPriceV3();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        private bool FeeIsDoing = true;

        private async void FeeV3_Checked(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ((IMainWindow)App.Current.MainWindow).ShowProcessing();
            try
            {

                if (sender is not RadioButton) return;
            if (FeeIsDoing) return;
            FeeIsDoing = true;
            try
            {
                RadioButton rb = sender as RadioButton;
                long Fee10000 = long.Parse(rb.Tag.ToString());

                await DoUpdatePrice(Fee10000);

                await DoRefreshUserPriceV3();

                //await ShowPooV3lAddress(Fee10000);

            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally { FeeIsDoing = false; }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            finally
            {
                ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                Cursor = Cursors.Arrow;
            }
        }

        //private async Task ShowPooV3lAddress(long Fee10000)
        //{
        //    var token0 = ComboBoxFromTokenV3.SelectedValue.ToString();
        //    var token1 = ComboBoxToTokenV3.SelectedValue.ToString();

        //    (double price, string pool) = await Share.UniswapTokenPrice.getPricePoolV3(token0, token1, Fee10000);

        //    if (0 < price)
        //    {
        //        ButtonPoolV3.Content = pool;
        //    }
        //    else
        //    {
        //        ButtonPoolV3.Content = "No Pool";
        //    }
        //}


        ///// <summary>
        ///// 刷新 User ToToken 的 balance 金额
        ///// </summary>
        //private void RefreshGetToTokenAmount()
        //{
        //    try
        //    {
        //        if (ComboBoxToTokenV3.SelectedValue != null && LabelPriceV3.Tag != null && !string.IsNullOrWhiteSpace(TextBoxFromTokenV3.Text))
        //        {
        //            string totoken = ComboBoxToTokenV3.SelectedValue.ToString();
        //            //todo: 这里错了！！！ 要使用评估价格！
        //            LabelToTokenV3.Content = (decimal.Parse(TextBoxFromTokenV3.Text) * decimal.Parse(LabelPriceV3.Tag.ToString())).ToString("N" + Share.BLL.Token.GetTokenDecimals(totoken).ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("", ex);
        //        LabelToTokenV3.Content = "???";
        //    }
        //}

        private async void ButtonSwapV3_Click(object sender, RoutedEventArgs e)
        {
            //交易  没有使用 multicall ，可以使用 multicall (有个麻烦之处，withdraw 的时候，不知道数量，而 withdraw 需要数量这个参数！)
            if (ComboBoxAddressV3.SelectedValue != null && ComboBoxFromTokenV3.SelectedValue != null && ComboBoxToTokenV3.SelectedValue != null && !string.IsNullOrWhiteSpace(TextBoxFromTokenV3.Text))
            {
                this.Cursor = Cursors.Wait;
                ((IMainWindow)App.Current.MainWindow).ShowProcessing();
                try
                {
                    var BuyerAddress = ComboBoxAddressV3.SelectedValue.ToString();
                    var token0 = ComboBoxFromTokenV3.SelectedValue.ToString();
                    var token1 = ComboBoxToTokenV3.SelectedValue.ToString();


                    var amountin = decimal.Parse(TextBoxFromTokenV3.Text.Trim()) * (decimal)Share.ShareParam.getPowerValue(Share.BLL.Token.GetTokenDecimals(token0));

                    string Password;
                    if (!WindowPassword.GetPassword(this, BuyerAddress, "Get Password", out Password)) return;

                    var account = new BLL.Address().GetAccount(BuyerAddress, Password);
                    if (account == null) { MessageBox.Show(this, @"No Account"); return; }
                    var web3 = Share.ShareParam.GetWeb3(account, Share.ShareParam.Web3Url);

                    //处理 weth ！
                    Nethereum.Uniswap.IPeripheryImmutableStateIPeripheryImmutableState.IPeripheryImmutableStateService router = new Nethereum.Uniswap.IPeripheryImmutableStateIPeripheryImmutableState.IPeripheryImmutableStateService(web3, ShareParam.AddressUniV3SwapRouter);
                    var AddressWETH = await router.Weth9QueryAsync();
                    if (Share.ShareParam.IsAnEmptyAddress(token0)) { token0 = AddressWETH; }
                    if (Share.ShareParam.IsAnEmptyAddress(token1)) { token1 = AddressWETH; }

                    //1，如果是 eth ，需要处理把ETH转变成 weth ！
                    if (token0 == AddressWETH)
                    {
                        Share.Contract.WETH9.WETH9Service ws = new Share.Contract.WETH9.WETH9Service(web3, AddressWETH);
                        Share.Contract.WETH9.ContractDefinition.DepositFunction wp = new Share.Contract.WETH9.ContractDefinition.DepositFunction()
                        {
                            AmountToSend = (BigInteger)amountin,
                        };
                        //var wtx = await ws.DepositRequestAsync(wp);
                        var wtx = await ws.DepositRequestAndWaitForReceiptAsync(wp);
                        Share.BLL.TransactionReceipt.LogTx(BuyerAddress, wtx.TransactionHash, "WETH9_Deposit", "UniswapV3SwapRouter_WETH9_Deposit");
                    }

                    var dif = decimal.Parse(TextBoxSlippageV3.Text) / 100;
                    var amountOut = decimal.Parse(LabelToTokenV3.Content.ToString()) * (decimal)ShareParam.getPowerValue(Share.BLL.Token.GetTokenDecimals(token1));
                    var amountOutMin = amountOut * (1 - dif);

                    var deadline = Common.DateTimeHelper.ConvertDateTime2Int(System.DateTime.Now.AddMinutes(int.Parse(TextBoxTxDeadlineV3.Text)));

                    var recipient = TextBoxRecipient.Text;      //todo: 输入合法性判断
                    if (token1 == AddressWETH)
                    {
                        recipient = BuyerAddress;
                        TextBoxRecipient.Text = BuyerAddress;       //如果交换得到eth，因为需要取现，所以不支持 Recipient ！
                    }

                    //2，要先approve！
                    bool IsOkApprove1 = await ((IMainWindow)App.Current.MainWindow).GetHelper().UiErc20TokenApprove(this, account, token0, ShareParam.AddressUniV3SwapRouter, (BigInteger)amountin);
                    if (!IsOkApprove1)
                    {
                        MessageBox.Show(this, "Erc20 Token Approve Failed!");
                        return;
                    }

                    var Fee10000 = getUiFee10000V3();

                    //3，执行交易 ExactInputSingle（SwapRouter-0xE592427A0AEce92De3Edee1F18E0157C05861564）
                    ////todo: 可以使用 SwapRouter02 （0x68b3465833fb72A70ecDF485E0e4C7bD8665Fc45）改进 SwapRouter02 提供了更多 function 
                    Nethereum.Uniswap.ISwapRouter.ContractDefinition.ExactInputSingleParams paramIn = new Nethereum.Uniswap.ISwapRouter.ContractDefinition.ExactInputSingleParams()
                    {
                        TokenIn = token0,
                        TokenOut = token1,
                        Fee = (uint)Fee10000,// Share.UniswapTokenPrice.Fee10000V3[0],       //3000
                        Recipient = recipient,
                        Deadline = deadline,
                        AmountIn = (BigInteger)amountin,
                        AmountOutMinimum = (BigInteger)amountOutMin,
                        SqrtPriceLimitX96 = 0,
                    };
                    Nethereum.Uniswap.ISwapRouter.ContractDefinition.ExactInputSingleFunction param = new Nethereum.Uniswap.ISwapRouter.ContractDefinition.ExactInputSingleFunction()
                    {
                        Params = paramIn,
                    };


                    //方法一：
                    Nethereum.Uniswap.ISwapRouter.ISwapRouterService service = new Nethereum.Uniswap.ISwapRouter.ISwapRouterService(web3, Share.ShareParam.AddressUniV3SwapRouter);
                    //var tx = await service.ExactInputSingleRequestAsync(param);
                    var FullTx = await service.ExactInputSingleRequestAndWaitForReceiptAsync(param);
                    var tx = FullTx.TransactionHash;

                    //方法二：
                    //Nethereum.RPC.Eth.DTOs.TransactionInput ti = new Nethereum.RPC.Eth.DTOs.TransactionInput()
                    //{
                    //    From = BuyerAddress,
                    //    To = Share.ShareParam.AddressUniV3SwapRouter,
                    //    Data = param.GetCallData().ToHex(),
                    //    Gas = new Nethereum.Hex.HexTypes.HexBigInteger(200000),
                    //};
                    //var tx = await web3.TransactionManager.SendTransactionAsync(ti);

                    Share.BLL.TransactionReceipt.LogTx(BuyerAddress, tx, TxUserMethod.UniswapV3SwapRouter_ExactInputSingle.ToString(), "UniswapV3SwapRouter_ExactInputSingle");

                    //4，如果是 eth ，需要处理提现！
                    if (token1 == AddressWETH)
                    {
                        Share.Contract.WETH9.WETH9Service ws2 = new Share.Contract.WETH9.WETH9Service(web3, AddressWETH);
                        var balance = await ws2.BalanceOfQueryAsync(account.Address);
                        Share.Contract.WETH9.ContractDefinition.WithdrawFunction wp2 = new Share.Contract.WETH9.ContractDefinition.WithdrawFunction()
                        {
                            Wad = balance,
                        };
                        var wtx2 = await ws2.WithdrawRequestAsync(wp2);
                        Share.BLL.TransactionReceipt.LogTx(BuyerAddress, wtx2, "WETH9_Withdraw", "UniswapV3SwapRouter_WETH9_Withdraw");
                    }

                    //以上最多4个事务，至少需要4个区块，每个区块12秒，大概需要48秒以上才能执行完成！

                    string text = LanguageHelper.GetTranslationText(@"交易已经提交到以太坊网络，点击‘确定’查看详情。");
                    string url = Share.ShareParam.GetTxUrl(tx);
                    if (MessageBox.Show(this, text, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", url);
                    }

                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    ((IMainWindow)App.Current.MainWindow).ShowProcesed();
                    Cursor = Cursors.Arrow;
                }

            }


        }

        private async Task<bool> DoRefreshUserPriceV3()
        {
            if (ComboBoxAddressV3.SelectedValue != null && ComboBoxFromTokenV3.SelectedValue != null && ComboBoxToTokenV3.SelectedValue != null && !string.IsNullOrWhiteSpace(TextBoxFromTokenV3.Text))
            {
                try
                {
                    var Fee10000 = getUiFee10000V3();
                    if (Fee10000 == 0) {
                        await DoUpdatePrice();  //先更新 价格 得到 Fee
                        Fee10000 = getUiFee10000V3();
                        if (Fee10000 == 0)
                        {
                            return false;
                        }
                    }

                    LabelUserPriceV3.Content = "?";
                    LabelToTokenV3.Content = "?";
                    LabelUserPriceDifV3.Content = "";

                    var BuyerAddress = ComboBoxAddressV3.SelectedValue.ToString();

                    //var account = Share.WindowAccount.GetAccount(this, new BLL.Address(), BuyerAddress, Share.ShareParam.AddressUniV3SwapRouter);
                    //if (account == null)
                    //{
                    //    return false;
                    //}
                    ////调用合约执行！ exactInputSingle
                    //var web3 = Share.ShareParam.GetWeb3(account, Share.ShareParam.Web3Url);
                    var web3 = Share.ShareParam.GetWeb3(Share.ShareParam.Web3Url);

                    var token0 = ComboBoxFromTokenV3.SelectedValue.ToString();
                    var token1 = ComboBoxToTokenV3.SelectedValue.ToString();
                    Nethereum.Uniswap.IPeripheryImmutableStateIPeripheryImmutableState.IPeripheryImmutableStateService router = new Nethereum.Uniswap.IPeripheryImmutableStateIPeripheryImmutableState.IPeripheryImmutableStateService(web3, ShareParam.AddressUniV3SwapRouter);
                    var AddressWETH = await router.Weth9QueryAsync();
                    if (Share.ShareParam.IsAnEmptyAddress(token0)) { token0 = AddressWETH; }
                    if (Share.ShareParam.IsAnEmptyAddress(token1)) { token1 = AddressWETH; }
                    if (token0 == token1 || token0.IsTheSameAddress(token1))
                    {
                        LabelUserPriceV3.Content = "??";
                        return false;
                    }

                    var d0 = Share.BLL.Token.GetTokenDecimals(token0);
                    var amountin = decimal.Parse(TextBoxFromTokenV3.Text.Trim()) * (decimal)Share.ShareParam.getPowerValue(d0);

                    //erc20 token approve

                    Nethereum.Uniswap.IQuoter.IQuoterService quoter = new Nethereum.Uniswap.IQuoter.IQuoterService(web3, Share.ShareParam.AddressUniV3Quoter);
                    Nethereum.Uniswap.IQuoter.ContractDefinition.QuoteExactInputSingleFunction param = new Nethereum.Uniswap.IQuoter.ContractDefinition.QuoteExactInputSingleFunction()
                    {
                        //FromAddress = BuyerAddress,       //不能有这个
                        TokenIn = token0,
                        TokenOut = token1,
                        Fee = (uint)Fee10000,               //Share.UniswapTokenPrice.Fee10000V3[0],
                        AmountIn = (BigInteger)amountin,
                        SqrtPriceLimitX96 = 0,
                    };
                    var AmountOut = await quoter.QuoteExactInputSingleQueryAsync(param);

                    var d1 = Share.BLL.Token.GetTokenDecimals(token1);

                    var price = (double)AmountOut / (double)amountin * Math.Pow(10, (d0 - d1));
                    LabelUserPriceV3.Content = price.ToString("N9");

                    //var swapprice = await Share.UniswapTokenPrice.getPriceV3(token0, token1);
                    var swapprice =(double) LabelPriceV3.Tag; //await Share.UniswapTokenPrice.getPriceV3(token0, token1);

                    var dif = (price - swapprice) * 100 / price;
                    LabelUserPriceDifV3.Content = dif.ToString("N6") + "%";

                    //得到评估金额
                    LabelToTokenV3.Content = (decimal.Parse(TextBoxFromTokenV3.Text) * (decimal)price).ToString("N" + Share.BLL.Token.GetTokenDecimals(token1).ToString());

                    return true;
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    LabelUserPriceV3.Content = ex.Message;
                    return false;
                }
            }
            else
            {
                LabelUserPriceV3.Content = "?";
                return false;
            }
        }


        private void Recipient_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRecipient();
        }

        private void UpdateRecipient()
        {
            TextBoxRecipient.IsReadOnly = CheckBoxRecipient.IsChecked == true;
            if (null != ComboBoxAddressV3.SelectedValue)
            {
                if (CheckBoxRecipient.IsChecked == true)
                {
                    TextBoxRecipient.Text = ComboBoxAddressV3.SelectedValue.ToString();
                }
            }
        }

        private void Recipient_UnChecked(object sender, RoutedEventArgs e)
        {
            UpdateRecipient();
        }



        private void Address_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                var pool = b.Content.ToString();
                System.Diagnostics.Process.Start("explorer.exe", Share.ShareParam.GetAddressUrl(pool));
            }
        }


        #endregion


    }


}



