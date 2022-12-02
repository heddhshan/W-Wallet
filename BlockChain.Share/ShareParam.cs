using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;

namespace BlockChain.Share
{

    /// <summary>
    /// 系统参数 20220623 全部使用eth系统！
    /// </summary>
    public static class ShareParam
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly System.Numerics.BigInteger ether = 1_000_000_000_000_000_000;     // = MATH.POW(10,18) 18个0      


        /// <summary>
        /// 因为微软的math.pow 是 float ，有误差的，所以直接使用 BigInteger 计算！
        /// decimal	±1.0 x 10-28 至 ±7.9228 x 1028	28-29 位	16 个字节	System.Decimal 可能会溢出，所以当前dotnet实际上没有合适的数据类型对应solidity！！！
        /// solidity 数据和 dotnet 之间的数据映射，需要单独有一整套！ 那个dotnet版本的geth（Nethermind）是有的，但其他客户端程序没有，包括nethereum！
        /// 同时，微软应该提供 BigDecimal 这样的数据类型
        /// 在使用float这个不准确数据后，会有很多后遗症，例如转账4eth，实际上可能数据变成的4.000000123eth。目前整个系统都存在此类问题，暂时不处理(20220628)。
        /// </summary>
        /// <param name="Decimals"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static System.Numerics.BigInteger getPowerValue(int Decimals)
        {
            switch (Decimals)
            {
                case 0:
                    return 1;
                case 1:
                    return 10;
                case 2:
                    return 100;
                case 3:
                    return 1000;
                case 4:
                    return 10_000;
                case 5:
                    return 100_000;
                case 6:
                    return 1000_000;
                case 7:
                    return 10_000_000;
                case 8:
                    return 100_000_000;
                case 9:
                    return 1_000_000_000;
                case 10:
                    return 10_000_000_000;
                case 11:
                    return 100_000_000_000;
                case 12:
                    return 1_000_000_000_000;
                case 13:
                    return 10_000_000_000_000;
                case 14:
                    return 100_000_000_000_000;
                case 15:
                    return 1_000_000_000_000_000;
                case 16:
                    return 10_000_000_000_000_000;
                case 17:
                    return 100_000_000_000_000_000;
                case 18:
                    return 1_000_000_000_000_000_000;

                default:
                    throw new Exception("no supported value, Decimals = " + Decimals.ToString());
            }
        }


        // 起始区块号 不同的链，有不同的值。 这会大大提高效率！
        //public const int FromBlock = 10000000;      // 0;      //通过接口读取，就是APPInfo合约部署的区块号减一！
        public static int FromBlock
        {
            get
            {
                return BLL.AppInfo.getFromBlock();
            }
        }

        ////币安最多只能查询五千个区块！！！        Nethereum.JsonRpc.Client.RpcResponseException: exceed maximum block range: 5000
        public const int MaxBlock = 500000;         //币安，5000; 以太坊，可以不要; 采用比最大值小点的数据 

        ////public const int NowBlockNum = 16;      //币安，距离现在的区块间隔，币安至少16个区块；以太坊是6个区块
        public const int NowBlockNum = 0;           //todo: 以太坊，距离现在的区块间隔，币安至少16个区块；以太坊是6个区块 测试时候以太坊改为0，否则事件数据很慢才能获取


        /// <summary>
        /// 判断地址是否为空， nethereum 提供的 IsAnEmptyAddress 不准确 ，要自己写一个 IsEmptyAddress
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsAnEmptyAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return true;

            return string.IsNullOrWhiteSpace(address.ToUpperInvariant())
                    || address == Share.ShareParam.EmptyAddress
                    || address.ToUpperInvariant() == Share.ShareParam.EmptyAddress.ToUpperInvariant()
                    || address.IsAnEmptyAddress()
                    || address == "0x0" || address.ToLower() == "0x0"
                //|| address.HexToBigInteger(false) == 0                                                // nethereum 4.5 版本， 有可能抛出异常，会失败！
                //|| address.HexToBigInteger(true) == 0                                                 // nethereum 4.5 版本， 有可能抛出异常，会失败！
                ;
        }


        public static void SetDataDirectory()
        {
            // 尝试使用这样的语句, 使用相对路径！！！  无法使用 ！暂时留在这里！
            // Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\OfflineWalletDb.mdf;Integrated Security=True
            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string DataDirectory = System.IO.Path.Combine(BasePath, "DataBase");
            AppDomain.CurrentDomain.SetData("DataDirectory", DataDirectory);
        }


        public static string DbConStr
        {
            get
            {
                return Properties.Settings.Default.UserDbConnectionString;
            }
        }

        public static void UpdateDbConStr(string _dbstr)
        {
            Properties.Settings.Default.UserDbConnectionString = _dbstr;
            Properties.Settings.Default.Save();
        }


        /// <summary>
        /// 捐赠地址
        /// </summary>
        public const string DonationAddress = @"0xC7A9d8C6C987784967375aE97a35D30AB617eB48";                //只有钱包可以接受捐赠， C7A9d8C6C987784967375aE97a35D30AB617eB48@hotmail.com

        public const string EmptyAddress = @"0x0000000000000000000000000000000000000000";
        public const string AddressEth = EmptyAddress;


        #region Uniswap V2 V3 Contract 不能更新

        //uniswap router: https://etherscan.io/address/0x7a250d5630b4cf539739df2c5dacb4c659f2488d#code		一样
        //weth: https://etherscan.io/address/0xc02aaa39b223fe8d0a0e5c4f27ead9083c756cc2#code				不一样的！
        //factory： https://etherscan.io/address/0x5c69bee701ef814a2b6a3edd4b1652cb9cc5aa6f					一样

        public const string AddressUniV2Router02 = "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D";            //UniswapV2Router02
        public const string AddressUniV2Factory = "0x5C69bEe701ef814a2B6a3EDD4B1652CB9cc5aA6f";

        public const string AddressUniV3SwapRouter = "0xE592427A0AEce92De3Edee1F18E0157C05861564";          //
        public const string AddressUniV3SwapRouter02 = "0x68b3465833fb72A70ecDF485E0e4C7bD8665Fc45";        //实现了很多接口：ISwapRouter02 is IV2SwapRouter, IV3SwapRouter, IApproveAndCall, IMulticallExtended, ISelfPermit
        public const string AddressUniV3Factory = "0x1F98431c8aD98523631AE4a59f267346ea31F984";
        public const string AddressUniV3Quoter = "0xb27308f9F90D607463bb33eA1BeBb41C27CE5AB6";              //提供了计算 quoteExactInputSingle


        #endregion


        /// <summary>
        /// 合约 AppInfo 地址，各个网络可能不一样！
        /// </summary>
        public static string AddressAppInfo
        {
            get
            {
                var chainId = GetChainId();
                if (chainId == 1)
                {
                    return "0xfb33901c46587da50a2fe5c2671cd4836a2aef31";        //https://etherscan.io/address/0xfb33901c46587da50a2fe5c2671cd4836a2aef31
                }
                else if (chainId == 5)
                {
                    return "0x1658cff3785fff5b7cdcb4279a26310f8b72fed6";        //https://goerli.etherscan.io/address/0x1658cff3785fff5b7cdcb4279a26310f8b72fed6
                }
                else
                {
                    throw new Exception("Not Deployed");
                }
            }
        }


        /// <summary>
        /// 系统中通用的稳定币地址，通常使用 usdt， 也可以使用 usdc， 用于 token 计价！
        /// </summary>
        public static string AddressPricingToken
        {
            get
            {
                var m = BLL.Token.GetPricingToken();
                if (m == null)
                {
                    //如果数据库没有，就使用 ETH ！
                    return AddressEth;
                }
                else
                {
                    return m.Address;                                           //数据库记录的计价币，用户自己设定
                }
            }
        }


        /// <summary>
        /// Uniswap V2 使用的 WETH
        /// </summary>
        public static string WethOfUniswapV2
        {
            get {
                if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }

                var web3 = Share.ShareParam.GetWeb3(Share.ShareParam.Web3Url);
                Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service router = new Nethereum.Uniswap.Contracts.UniswapV2Router02.UniswapV2Router02Service(web3, ShareParam.AddressUniV2Router02);
                var AddressWETH =  router.WETHQueryAsync().Result;       
                return AddressWETH;
            }
        }

        /// <summary>
        /// Uniswap V3 使用的 WETH9
        /// </summary>
        public static string Weth9OfUniswapV3
        {
            get
            {
                if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }

                var web3 = Share.ShareParam.GetWeb3(Share.ShareParam.Web3Url);
                Nethereum.Uniswap.IPeripheryImmutableStateIPeripheryImmutableState.IPeripheryImmutableStateService router = new Nethereum.Uniswap.IPeripheryImmutableStateIPeripheryImmutableState.IPeripheryImmutableStateService(web3, ShareParam.AddressUniV3SwapRouter);
                var AddressWETH =  router.Weth9QueryAsync().Result;
                return AddressWETH;
            }
        }

        /// <summary>
        /// Web3NodesListUrl
        /// </summary>
        /// <returns></returns>
        public static string GetNodesListUrl()
        {
            string url = Share.BLL.AppInfo.GetKeyString("Web3NodesListUrl");
            if (string.IsNullOrEmpty(url))
            {
                url = "https://ethereumnodes.com/";
            }
            return url;
        }


        /// <summary>
        ///  Erc20TokenListUrl
        /// </summary>
        /// <returns></returns>
        public static string GetErc20TokenListUrl()
        {
            string url = Share.BLL.AppInfo.GetKeyString("Erc20TokenListUrl");
            if (string.IsNullOrEmpty(url))
            {
                url = "https://etherscan.io/tokens";
            }
            return url;
        }


        ///// <summary>
        ///// ExeOffTxUrl 第三方执行离线事务的 url 
        ///// </summary>
        ///// <returns></returns>
        //public static string GetExeOffTxUrl()
        //{
        //    string url = Share.BLL.AppInfo.GetKeyString("ExeOffTxUrl");
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        url = "https://app.mycrypto.com/broadcast-transaction";
        //    }
        //    return url;
        //}

        public static string GetT3sOfficeUri()
        {
            string url = Share.BLL.AppInfo.GetKeyString("T3sOfficeUri");
            if (string.IsNullOrEmpty(url))
            {
                url = "https://www.google.com";
            }
            return url;
        }

        ///// <summary>
        ///// 得到节点列表的 网址， 暂时写死！
        ///// </summary>
        ///// <returns></returns>
        //public static string GetNodesListUrl()
        //{
        //    //https://ethereumnodes.com/
        //    //https://bscscan.com/apis#rpc
        //    //https://developers.tron.network/docs/official-public-node

        //    if (CurrentNodesType == NodesTypeEnum.EthereumMainChain)
        //    {
        //        return "https://ethereumnodes.com/";
        //    }
        //    if (CurrentNodesType == NodesTypeEnum.EthereumRinkebyChain)
        //    {
        //        return "https://ethereumnodes.com/";
        //    }

        //    //if (CurrentNodesType == Common.NodesTypeEnum.TronChain)
        //    //{
        //    //    return "https://developers.tron.network/docs/official-public-node";
        //    //}

        //    return "https://www.google.com/";
        //}



        /// <summary>
        /// 基础币（以太币，币安币，波场币）  目前只是在以太坊和币安上测试；波场上没做任何测试，也还需要做一些修改（例如地址格式不一样，solidity版本也很低，等）。
        /// </summary>
        public static string BaseToken
        {
            get
            {
                //if (CurrentNodesType == Common.NodesTypeEnum.EthereumChain)
                //{
                //    return "ETH";
                //}
                //else if (CurrentNodesType == Common.NodesTypeEnum.BinanceChain)
                //{
                //    return "BNB";
                //}
                //else if (CurrentNodesType == Common.NodesTypeEnum.TronChain)
                //{
                //    return "TRON";
                //}

                return "ETH";
            }
        }

        public static string Web3Url
        {
            get
            {
                var m = Share.BLL.Web3Url.GetCurWeb3UrlModel();
                if (m != null)
                {
                    return m.Url;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static ChainTypeEnum CurrentChainType
        {
            get
            {
                int local_chainid = Properties.Settings.Default.ChainId;                //todo: 这个chainid保存好像没有用！！！？？？

                int net_chainid = (int)GetChainId();
                if (local_chainid != net_chainid && net_chainid != 0)
                {
                    log.Error("local_chainid=" + local_chainid.ToString() + "; net_chainid=" + net_chainid);

                    Properties.Settings.Default.ChainId = net_chainid;
                    Properties.Settings.Default.Save();
                    local_chainid = net_chainid;
                }

                return (ChainTypeEnum)local_chainid;
            }
        }


        /// <summary>
        /// Get ChainId, 使用了缓存。 系统中很多地方都没有使用这个 method ， 可以全部改正为使用此 method 。
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetChainId()
        {
            string key = "{40AFDE5E-C00B-4ADA-990D-3531C14E907D}";
            object result = Common.Cache.GetData(key);
            if (null == result)
            {
                result = Common.Web3Helper.GetChainId(ShareParam.Web3Url);
                Common.Cache.AddBySlidingTime(key, result);
            }
            return (System.Numerics.BigInteger)result;
        }


        /// <summary>
        /// 以太坊网站，例如etherscan， cn.etherscan 等。
        /// </summary>
        public static string EtherSacn
        {
            get
            {
                //BigInteger cid = 0;
                //try
                //{                   
                //    cid = GetChainId();
                //}
                //catch (Exception ex) { 
                //    log.Error("", ex); 
                //}

                //if (cid == 1)
                //{
                //    return "https://etherscan.io/";
                //}
                //else if (cid == 5)
                //{
                //    return "https://goerli.etherscan.io/";
                //}
                //else
                //{
                //    return Properties.Settings.Default.EtherSacn;
                //}

                return Properties.Settings.Default.EtherSacn;           //尊重用户的选择，不要写死主网和测试网的 EtherSacn ！
            }
            set
            {
                Properties.Settings.Default.EtherSacn = value;
                Properties.Settings.Default.Save();
            }
        }



        //https://etherscan.io/token/0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48

        public static string GetTokenUrl(string tokenAddress)
        {
            //https://etherscan.io/token/0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48
            string url = EtherSacn + "token/" + tokenAddress;
            return url;
        }


        /// <summary>
        /// 得到某个地址某种token的交易列表
        /// </summary>
        /// <param name="tokenAddress"></param>
        /// <param name="userAddress"></param>
        /// <returns></returns>
        public static string GetTokenUserUrl(string tokenAddress, string userAddress )
        {
            //https://goerli.etherscan.io/token/0x684e47e8f4c41abd96d65be15f1828985a613d4b?a=0x6670657c60352feb36EDBd6961E2D072b82Ba55d
            string url = EtherSacn + "token/" + tokenAddress + "?a=" + userAddress;
            return url;
        }


        public static string GetTxUrl(string txId)
        {
            //https://etherscan.io/tx/0xbdbb5ad8ab8c22648355164ca5609de92153b1068f42544d1f4f4941d64c7311
            string url = EtherSacn + "tx/" + txId;
            return url;
        }

        public static string GetAddressUrl(string address)
        {
            //https://etherscan.io/address/0x781145b67f36da5305c3a93686704f1a37ea1985
            string url = EtherSacn + "address/" + address;
            return url;
        }

        public static string GetBlockUrl(long block)
        {
            //https://etherscan.io/block/9655348
            string url = EtherSacn + "block/" + block.ToString();
            return url;
        }

       

        /// <summary>
        /// 得到用户数据目录
        /// </summary>
        /// <returns></returns>
        private static string GetUserDataDir()
        {
            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var restult = System.IO.Path.Combine(directoryPath, "BlockChain");

            if (Directory.Exists(restult))
            {
                //do nothing
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(restult);             //20210630 windows 更新后，这里会出错
                }
                catch (Exception ex)
                {
                    var err = LanguageHelper.GetTranslationText("创建目录失败:") + "'" + restult + "'"
                        + Environment.NewLine
                        + LanguageHelper.GetTranslationText("可能是权限不够造成的，请进入‘windows 安全中心’->'病毒和威胁防护'->‘保护历史记录’中'已阻止受保护的文件夹'执行‘操作’‘允许在此设备上’。");
                    //MessageBox.Show(err);
                    log.Error(err + Environment.NewLine, ex);
                    //throw ex;
                    return string.Empty;
                }
            }

            return restult;
        }

        public static string UserDataDir
        {
            get
            {
                var result = GetUserDataDir();
                if (string.IsNullOrEmpty(result))
                {
                    string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
                    result = Path.Combine(BasePath, "UserData");
                }
                return result;
            }
        }

        /// <summary>
        /// keysroe 目录
        /// </summary>
        public static string KeystoreDir
        {
            get
            {
                //return GetUserDataDir();
                var result = GetUserDataDir();
                if (string.IsNullOrEmpty(result))
                {
                    string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
                    result = Path.Combine(BasePath, "Keystore");
                }
                return result;
            }
        }


        /// <summary>
        /// 备份目录，目前主要是备份账号信息
        /// </summary>
        public static string BackUpDir
        {
            get
            {
                //return GetUserDataDir();
                var result = GetUserDataDir();
                if (string.IsNullOrEmpty(result))
                {
                    string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
                    result = Path.Combine(BasePath, "BackUp");
                }
                return result;
            }
        }


        //public static string ImageDir
        //{
        //    get
        //    {
        //        string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
        //        var restult = System.IO.Path.Combine(BasePath, "Image");
        //        return restult;
        //    }
        //}

        ///// <summary>
        /////  一次性授权的最大值
        ///// </summary>
        //public static readonly System.Numerics.BigInteger Erc20ApproveMaxValue = System.Numerics.BigInteger.Pow(10, 50);


        /// <summary>
        /// token 的总的发型量，也是  一次性授权的最大值
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task< System.Numerics.BigInteger > GetTokenTotalSupply(string token)
        {
            try {
            var web3 = GetWeb3();
            Nethereum.StandardTokenEIP20.StandardTokenService s = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, token);
            var result = await s.TotalSupplyQueryAsync();
            return result; 
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return System.Numerics.BigInteger.Pow(10, 50);
            }
        }


        public static async Task<System.Numerics.BigInteger> getNowBlockTimestamp()
        {
            return await BLL.AppInfo.getNowBlockTimestamp();
        }


        /// <summary>
        /// 判断是否是一次性授权
        /// </summary>
        public static bool IsErc20ApproveMax
        {
            get
            {
                return Properties.Settings.Default.IsErc20ApproveMax;
            }
            set
            {
                Properties.Settings.Default.IsErc20ApproveMax = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// 得到默认 GasPrice  旧格式事务才需要！
        /// </summary>
        public static long GetDefaultGasPriceAsync()
        {
            if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }

            string key = "{18E508D9-5D98-4C98-961B-BC3C5E1FB5C3}";
            var result = Common.Cache.GetData(key);
            if (result == null)
            {
                double dr = (double)Common.Web3Helper.GetNowGasPrice(ShareParam.Web3Url);
                result = (long)dr;
                Common.Cache.AddByAbsoluteTime(key, result, 120);
            }
            return (long)result;
        }



        /// <summary>
        /// 把数字转换成 bytes32
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte[] GetBytes32(uint number)
        {
            var result = Common.SolidityHelper.GetSolidityUint256(number);
            return result;
        }

        /// <summary>
        /// 把现在的时间转换成 bytes32 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetNowBytes32()
        {
            var dt = Common.DateTimeHelper.ConvertDateTime2Int(System.DateTime.Now);
            var result = GetBytes32((uint)dt);
            return result;
        }

        public static string GenSignature(string hash, string privatekey)
        {
            var b = hash.HexToByteArray();
            if (b.Length != 32)
            {
                throw new Exception("hash length is 32!");
            }

            return GenSig(b, privatekey);
        }

        /// <summary>
        /// 生成签名,
        /// </summary>
        /// <param name="hash">32位长数据</param>
        /// <param name="privatekey">私钥</param>
        /// <returns></returns>
        public static string GenSig(byte[] b, string privatekey)
        {
            try
            {
                var signer = new MessageSigner();           //没有前缀
                string signature = signer.Sign(b, privatekey);
                return signature;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return string.Empty;
            }
        }


        /// <summary>
        /// 验证签名，从签名中得到地址（公钥）
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="signature"></param>
        /// <returns></returns>

        public static string GetAddress(string hash, string signature)
        {
            try
            {
                var signer = new MessageSigner();
                var address = signer.EcRecover(hash.HexToByteArray(), signature);
                address = address.EnsureHexPrefix();
                return address;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return string.Empty;
            }
        }


        /// <summary>
        /// 取得当前源码的哪一行  https://blog.csdn.net/weizhiai12/article/details/7062854
        /// </summary>
        /// <returns></returns>
        public static int GetLineNum()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileLineNumber();
        }

        public static Nethereum.RPC.Fee1559Suggestions.Fee1559 GetEip1559Fee()
        {
            if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }

            var web3 = ShareParam.GetWeb3();
            var feeEstimate = web3.FeeSuggestion.GetSimpleFeeSuggestionStrategy();
            return feeEstimate.SuggestFeeAsync().Result;
        }

        public static BigInteger GetNonce(string address)
        {
            if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }

            if (!address.IsEthereumChecksumAddress())
            { return -1; }

            var web3 = Share.ShareParam.GetWeb3();
            var result = web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(address).Result;
            return result.Value;
        }

        private static bool? _IsOfflineWallet = null;

        /// <summary>
        /// 是否市离线钱包调用，智能设置一次。只有离线钱包调用需要显式设置。
        /// </summary>
        public static bool? IsOfflineWallet
        { 
            get { return _IsOfflineWallet; }
            set
            {
                if (_IsOfflineWallet == null)
                {
                    if (value == null)
                    {
                        throw new Exception("Require value != null");
                    }
                    else
                    {
                        _IsOfflineWallet = value;
                    }
                }
                else
                {
                    throw new Exception("Require Only Setting Once");
                }
            }        
        }

        public static Nethereum.Web3.Web3 GetWeb3()
        {
            return GetWeb3(ShareParam.Web3Url);
        }

        public static Nethereum.Web3.Web3 GetWeb3(string _web3url)
        {
            if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }
            var web3 = new Nethereum.Web3.Web3(_web3url);
            return web3;
        }


        public static Nethereum.Web3.Web3 GetWeb3(Nethereum.RPC.Accounts.IAccount _account)
        {
            return GetWeb3(_account, ShareParam.Web3Url);
        }

        public static Nethereum.Web3.Web3 GetWeb3(Nethereum.RPC.Accounts.IAccount _account, string _web3url)
        {
            if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }
            var chainid = ShareParam.GetChainId();
            var web3 = new Nethereum.Web3.Web3(_account, ShareParam.Web3Url);

            //    //2022-10-12 整个系统都支持 eip1559 ，包括离线钱包，所以不支持 eip1559  市不能的
            ////https://besu.hyperledger.org/en/stable/Concepts/NetworkID-And-ChainID/
            //if (chainid == 1 || chainid == 3 || chainid == 4)
            //{
            //    //do nothing   这些链支持eip1559  以太坊主链，ropsten，rinkeby
            //}
            //else
            //{
            //    // 目前(20210815) layer 2 还不支持 eip 1559 !         //这里需要不停增加，做成一个参数 ，自己去调吧 但是目前还没有界面！
            //    //web3.Eth.TransactionManager.UseLegacyAsDefault = true;
            //    web3.Eth.TransactionManager.UseLegacyAsDefault = Properties.Settings.Default.UseLegacyAsDefault;
            //}

            return web3;
        }

        public static System.Numerics.BigInteger GetGasLimit(string from, string to, string inputdata)
        {
            if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }
            return Common.Web3Helper.GetGasLimit(Web3Url, from, to, inputdata).Result;        
        }


        /// <summary>
        /// 得到实时的balance，如果返回 -1 ，表示查询失败！
        /// </summary>
        /// <param name="address"></param>
        /// <param name="token"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public static double GetRealBalance(string address, string token, bool isCache = false)
        {
            if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }
            try
            {

                string key = address + "{10A8287E-3C8D-49BA-B8B9-80C8E45D9D64}" + token;
                if (isCache)
                {
                    var result = Common.Cache.GetData(key);
                    if (result != null)
                    {
                        return (double)result;
                    }
                }

                double amount = 0;
                Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();

                if (token == "0x0" || Share.ShareParam.IsAnEmptyAddress(token))
                {
                    amount = (double)(web3.Eth.GetBalance.SendRequestAsync(address).Result).Value;
                    amount = amount / Math.Pow(10, 18.0);
                }
                else
                {
                    Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, token);
                    amount = (double)service.BalanceOfQueryAsync(address).Result;
                    var Decimals = (double)service.DecimalsQueryAsync().Result;

                    amount = amount / Math.Pow(10, Decimals);
                }

                if (isCache)
                {
                    Common.Cache.AddByAbsoluteTime(key, amount, 5);     //缓存五秒钟
                }
                return amount;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return -1;
            }
        }

        public static async Task< BigInteger> GetRealBalanceValue(string address, string token)
        {
            if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }
            Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();

            if (token == "0x0" || Share.ShareParam.IsAnEmptyAddress(token))
            {
               return (await web3.Eth.GetBalance.SendRequestAsync(address)).Value;
            }
            else
            {
                Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, token);
                return await service.BalanceOfQueryAsync(address);
            }

        }



        /// <summary>
        /// 取消事务执行 现在的问题就是，nethereum 执行事务的时候，没有返回 nonce gas，等信息，只返回了一个 tx，这就造成了很大麻烦！
        /// </summary>
        /// <param name="account"></param>
        /// <param name="lastTxNonce"></param>
        /// <param name="lastTxGasPrice"></param>
        public static async Task<bool> CancelTransaction(string TxId, Nethereum.Web3.Accounts.Account account, BigInteger lastTxNonce, BigInteger bigLastTxGasPrice)
        {
            if (IsOfflineWallet == true) { throw new Exception("Require Online Wallet"); }

            try
            {
                //逻辑： 1， 自己给自己发送 0 eth； 2， gasPrice 高20%以上，且 GasPrice 高于当前值。

                var nonce = GetNonce(account.Address);
                if (lastTxNonce < nonce)
                {
                    log.Error("lastTxNonce <= nonce");
                    return false;
                }

                var ThisGasePrice = bigLastTxGasPrice * 120 / 100;                              //比上次 gasprice 高 20%
                var gasprice = Share.ShareParam.GetDefaultGasPriceAsync() * 120 / 100;          //比当前 gasprice 高 20%  / Math.Pow(10, 9)) Math.Pow(0.1,18)
                if (ThisGasePrice < gasprice)
                {
                    ThisGasePrice = gasprice;
                }

                //decimal decimal_gasprice = (decimal)((double)ThisGasePrice / Math.Pow(10, 9));              

                var web3 = Share.ShareParam.GetWeb3(account);
                web3.TransactionManager.UseLegacyAsDefault = true;

                ///// 老方法，使用  web3.Eth.GetEtherTransferService().TransferEtherAsync 
                //var tx = await web3.Eth.GetEtherTransferService().TransferEtherAsync(account.Address, (decimal)Math.Pow(0.1, 18), decimal_gasprice, 21000, lastTxNonce);

                // 新方法， 使用 var txnHash2 = await transactionManager.SendTransactionAsync(txnInput2); 参见： http://playground.nethereum.com/csharp/id/1061
                var InputNew = new TransactionInput()
                {
                    From = account.Address,
                    To = account.Address,
                    Data = "",
                    Gas = new Nethereum.Hex.HexTypes.HexBigInteger(21000),
                    Nonce = new Nethereum.Hex.HexTypes.HexBigInteger(lastTxNonce),
                    Value = new Nethereum.Hex.HexTypes.HexBigInteger(0),
                };
                var tx = await web3.TransactionManager.SendTransactionAsync(InputNew);

                Share.BLL.TransactionReceipt.LogTx(account.Address, tx, "CancelTransaction", "To:" + account.Address + ", Eth:0; Old TxId Is " + TxId);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Cancel Transaction Failed, Old Transaction Hash: " + TxId, ex);
                return false;
            }
        }



        #region 免责声明，隐私声明，各个APP可以参考，或者公用

        /// <summary>
        /// privacy adn safe 隐私与安全
        /// </summary>
        public const string PrivacyAndSafeText = @"
1，本程序不收集、不传送密码、助记词、私钥、您的使用习惯数据等您的的私人信息。
2，系统日志不记录密码、助记词、私钥等您的私人信息，本程序不收集和传送系统日志；但系统日志可以作为程序bug修改的依据之一，您可以自愿提交系统日志给开发团队。
3，本程序主要采用Nethereum项目(https://github.com/Nethereum/Nethereum)执行web3事务，全部采用离线签名发起交易，不向web3接口传输私钥和密码等机密信息。
4，本程序是去中心化系统：使用web3接口和以太坊网络连接，其web3接口可以使用网上免费的（或付费的），专业用户也可以在您本机运行geth.exe等以太坊客户端提供web3接口。
5，本程序不主动收集或备份用户信息，对于某些重要的信息，例如助记词，KeyStore文件，私钥等，一旦遗失将导致资产永久丢失。钱包程序提供了多种备份方式，请用户自己妥善保管。
";


        /// <summary>
        /// Disclaimer 免责声明
        /// </summary>
        public const string DisclaimerText = @"
1，开发团队尽力保证整个技术体系的安全，但也可能存在疏漏之处。
2，请合法使用本程序，请遵守本国或本地区的相关法律。
3，请勿超过自己的经济和心里承受能力参与各种交易。
无论以上的哪种情况，以及使用本程序造成的各种已知的或未知的风险，团队会尽量配合处理，但不承担责任。
";


        #endregion


        /// <summary>
        /// 通用的打开url 的方式
        /// System.Diagnostics.Process.Start("explorer.exe", url); 这种模式可能失败！
        /// </summary>
        /// <param name="url"></param>
        public static void OperUrl(string url)
        {
            //https://www.cnblogs.com/tianma3798/p/14957783.html
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;            //不使用shell启动
            p.StartInfo.RedirectStandardInput = true        ;//喊cmd接受标准输入
            p.StartInfo.RedirectStandardOutput = false;     //不想听cmd讲话所以不要他输出
            p.StartInfo.RedirectStandardError = true;       //重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;              //不显示窗口
            p.Start();

            //向cmd窗口发送输入信息 后面的&exit告诉cmd运行好之后就退出
            p.StandardInput.WriteLine("start " + url + "&exit");
            //p.StandardInput.WriteLine("start " + url);
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
        }




    }



}
