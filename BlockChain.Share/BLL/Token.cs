using BlockChain.Share;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;

namespace BlockChain.Share.BLL
{
    public class Token
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<bool> IniTokenData(int chainId)
        {
            string sql = @"delete Token";
            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cn.Open();
            try
            {
                cm.ExecuteNonQuery();
            }
            finally
            {
                cn.Close();
            }

            // eth
            await SaveTokenData(chainId, ShareParam.AddressEth, @"TokenIcon\Eth.png", true);

            if (chainId == 1)
            {
                //token 初始化，还需要 usdt usdc busd  dai  wbtc stEth, wstEth， bnb 这几个token， https://etherscan.io/tokens
                await SaveTokenData(chainId, "0xdAC17F958D2ee523a2206206994597C13D831ec7", @"TokenIcon\Usdt.png", true);              //usdt https://etherscan.io/token/0xdac17f958d2ee523a2206206994597c13d831ec7
                await SaveTokenData(chainId, "0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48", @"TokenIcon\USDC.png", true);              //usdc https://etherscan.io/token/0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48
                //await SaveTokenData(chainId, "0x4Fabb145d64652a948d72533023f6E7A623C7C53", @"TokenIcon\binanceusd_32.png", true);     //busd https://etherscan.io/token/0x4fabb145d64652a948d72533023f6e7a623c7c53
                await SaveTokenData(chainId, "0x6B175474E89094C44Da98b954EedeAC495271d0F", @"TokenIcon\dai_stablecoin.png", true);    //dai  https://etherscan.io/token/0x6b175474e89094c44da98b954eedeac495271d0f

                await SaveTokenData(chainId, "0x2260FAC5E5542a773Aa44fBCfeDf7C193bc2C599", @"TokenIcon\wbtc28.png", true);            //wbtc https://etherscan.io/token/0x2260fac5e5542a773aa44fbcfedf7c193bc2c599

                await SaveTokenData(chainId, "0xae7ab96520DE3A18E5e111B5EaAb095312D7fE84", @"TokenIcon\stETH28.png", true);           //stEth https://etherscan.io/token/0xae7ab96520de3a18e5e111b5eaab095312d7fe84
                await SaveTokenData(chainId, "0x7f39C581F595B53c5cb19bD0b3f8dA6c935E2Ca0", @"TokenIcon\wstETH.png", true);            //wstEth https://etherscan.io/token/0x7f39c581f595b53c5cb19bd0b3f8da6c935e2ca0

                //await SaveTokenData(chainId, "0xB8c77482e45F1F44dE1745F52C74426C631bDD52", @"TokenIcon\bnb_28_2.png", true);          //bnb  https://etherscan.io/token/0xB8c77482e45F1F44dE1745F52C74426C631bDD52
            }

            //默认是以 eth 计价，这样所有的网络都通用
            if (ShareParam.AddressPricingToken != ShareParam.AddressEth
                && DAL.Token.Exist(Share.ShareParam.DbConStr, chainId, ShareParam.AddressPricingToken))
            {
                await SaveTokenData(chainId, ShareParam.AddressPricingToken, @"TokenIcon\NoJPG.png", true);
            }

            return true;
        }

        private static System.Threading.Thread? UpdateTokenPriceThread = null;

        public static void StartUpdateTokenPrice()
        {
            if (UpdateTokenPriceThread == null)
            {
                UpdateTokenPriceThread = new System.Threading.Thread(LoopDoUpdateTokenStablecoinPrice);
                UpdateTokenPriceThread.Start();
            }
        }

        private static bool CheckIsRunning()
        {
            return Application.Current != null;         // && Application.Current.MainWindow != null;// && ((MainWindowHelper)(Application.Current.MainWindow)).IsRunning();
            //return Application.Current != null && Application.Current.MainWindow != null;// && ((MainWindowHelper)(Application.Current.MainWindow)).IsRunning();
            //Application.Current != null && Application.Current.MainWindow != null && Application.Current.Windows.Count > 0
        }

        private static void LoopDoUpdateTokenStablecoinPrice()
        {
            //只要程序没有退出，就需要执行。 只能用于 app， 没窗体的后台，例如 asp.net 要修改这段代码
            while (CheckIsRunning())
            {
                try
                {
                    var done = UpdateTokenStablecoinPrice(5).Result;
                    log.Info("UpdateTokenStablecoinPrice Number Is " + done.ToString());
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                }

                for (int i = 0; i < 60; i++)
                {
                    if (CheckIsRunning())
                    {
                        System.Threading.Thread.Sleep(5 * 1000);    //休息五秒， 总计休息5分钟！
                    }
                }
            }
        }


        /// <summary>
        /// 更新所有token相对稳定币的价格
        /// </summary>
        public async static Task<int> UpdateTokenStablecoinPrice(int sleepseconds = 0)
        {
            int done = 0;
            try
            {
                string sql = @"SELECT * FROM Token where ChainId = @ChainId";

                SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
                SqlCommand cm = new SqlCommand();
                cm.Connection = cn;
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = sql;

                var ChainId = (int)ShareParam.GetChainId();
                cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cm;
                System.Data.DataSet ds = new System.Data.DataSet();
                da.Fill(ds);

                var tl = Model.Token.DataTable2List(ds.Tables[0]);
                foreach (var m in tl)
                {
                    try
                    {
                        if (!m.PricingIsFixed)
                        {
                            m.PricingTokenAddress = Share.ShareParam.AddressPricingToken;
                            m.PricingTokenPrice = await Share.UniswapTokenPrice.getPrice(m.Address, Share.ShareParam.AddressPricingToken);
                            m.PricingTokenPriceUpdateTime = System.DateTime.Now;

                            DAL.Token.Update(Share.ShareParam.DbConStr, m);             //允许更新
                            done++;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("", ex);
                    }

                    System.Threading.Thread.Sleep(sleepseconds * 1000);             //缓解 cpu 压力，让程序不卡
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            return done;
        }

        public async static Task<bool> UpdateOneTokenStablecoinPrice(string token)
        {
            var ChainId = (int)ShareParam.GetChainId();
            var m = DAL.Token.GetModel(ShareParam.DbConStr, ChainId, token);

            try
            {
                if (!m.PricingIsFixed)
                {
                    m.PricingTokenAddress = Share.ShareParam.AddressPricingToken;
                    m.PricingTokenPrice = await Share.UniswapTokenPrice.getPrice(m.Address, Share.ShareParam.AddressPricingToken);
                    m.PricingTokenPriceUpdateTime = System.DateTime.Now;

                    DAL.Token.Update(Share.ShareParam.DbConStr, m);             //允许更新
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return false;
            }
        }


        //public static async Task<bool> SaveTokenData(Model.Token model)
        //{
        //    if (DAL.Token.Exist(Share.ShareParam.DbConStr, model.ChainId, model.Address))
        //    {
        //        DAL.Token.Update(Share.ShareParam.DbConStr, model);             //允许更新
        //    }
        //    else
        //    {
        //        DAL.Token.Insert(Share.ShareParam.DbConStr, model);
        //    }
        //    return true;
        //}


        public static async Task<bool> SaveTokenData(string TokenAddress, string ImagePath, bool FromUser = true, bool isFixedPrice = false, string priceToken = "", decimal price = 0)
        {
            var ChainId = (int)ShareParam.GetChainId();
            return await SaveTokenData(ChainId, TokenAddress, ImagePath, FromUser, isFixedPrice, priceToken, price);
        }

        public static async Task<bool> SaveTokenData(int ChainId, string TokenAddress, string ImagePath, bool FromUser = true, bool isFixedPrice = false, string priceToken = "", decimal price = 0)
        {
            log.Info(TokenAddress + " - " + ImagePath);

            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string filepath = System.IO.Path.Combine(BasePath, ImagePath);
            if (!System.IO.File.Exists(filepath))
            {
                log.Error("Not Exists Token(" + TokenAddress + ") Icon File: " + filepath);
            }

            Model.Token token = new Model.Token();
            token.Address = TokenAddress;
            token.ImagePath = ImagePath;
            //token.ImagePath = filepath;       //no ，要保存相对路径

            if (!TokenAddress.IsValidEthereumAddressHexFormat())
            {
                throw new Exception("Ethereum Address Hex Format Is Error! TokenAddress Is " + TokenAddress);
            }

            if (TokenAddress == "0x0" || ShareParam.IsAnEmptyAddress(TokenAddress))
            {
                token.Decimals = 18;
                //token.TotalSupply = 110000000;
                //token.IsSelected = IsSelected;
                token.ChainId = ChainId;
                token.Name = ShareParam.CurrentChainType.ToString();
                token.Symbol = ChainType.GetBaseTokenName(ShareParam.CurrentChainType);
            }
            else
            {
                try
                {
                    var web3 = Share.ShareParam.GetWeb3();
                    Nethereum.StandardTokenEIP20.StandardTokenService service = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, TokenAddress);
                    token.Decimals = service.DecimalsQueryAsync().Result;                                                    //没有这个方法，也不一定抛出异常的！！！
                    token.Name = service.NameQueryAsync().Result;
                    token.Symbol = service.SymbolQueryAsync().Result;

                    token.ChainId = ChainId;

                    if (string.IsNullOrEmpty(token.Name))   //简单判断，名字不能为空
                    {
                        token.Name = "-";
                    }
                    if (string.IsNullOrEmpty(token.Symbol)) //简单判断，简写不能为空
                    {
                        token.Symbol = "*";
                    }
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    return false;
                }
            }

            token.PricingIsFixed = isFixedPrice;

            if (isFixedPrice)
            {
                token.PricingTokenAddress = priceToken;
                token.PricingTokenPrice = (double)price;                //处理稳定币价格
            }
            else
            {
                token.PricingTokenAddress = Share.ShareParam.AddressPricingToken;
                token.PricingTokenPrice = await Share.UniswapTokenPrice.getPrice(TokenAddress, Share.ShareParam.AddressPricingToken);                //处理稳定币价格

            }
            token.IsPricingToken = Share.ShareParam.AddressPricingToken.ToLower() == TokenAddress.ToLower(); ;
            token.PricingTokenPriceUpdateTime = System.DateTime.Now;
            
            if (DAL.Token.Exist(Share.ShareParam.DbConStr, ChainId, TokenAddress))
            {
                DAL.Token.Update(Share.ShareParam.DbConStr, token);             //允许更新
            }
            else
            {
                DAL.Token.Insert(Share.ShareParam.DbConStr, token);
            }

            return true;
        }

        public static bool Exist(string tokenAddress)
        {
            return DAL.Token.Exist(ShareParam.DbConStr, (int)Share.ShareParam.GetChainId(), tokenAddress);
        }

        public static int GetTokenRealDecimals(string tokenAddress)
        {
            Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();
            Nethereum.StandardTokenEIP20.StandardTokenService token = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, tokenAddress);
            return token.DecimalsQueryAsync().Result;
        }

        public static int GetTokenDecimals(string tokenAddress)
        {
            string key = tokenAddress + "{1B895D70-E6AF-40A7-B4B5-B6CEA0315540}";
            var result = Common.Cache.GetData(key);
            if (result == null)
            {
                result = _GetTokenDecimals(tokenAddress);
                Common.Cache.AddBySlidingTime(key, result);
            }

            return (int)result;
        }

        private static int _GetTokenDecimals(string tokenAddress)
        {
            if (tokenAddress == "0x0" || ShareParam.IsAnEmptyAddress(tokenAddress))
            {
                return 18;
            }

            try
            {
                var m = GetModel(tokenAddress);
                if (m != null)
                {
                    return m.Decimals;
                }

                Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();
                Nethereum.StandardTokenEIP20.StandardTokenService token = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, tokenAddress);
                return token.DecimalsQueryAsync().Result;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                throw ex;
            }
        }


        public  static Model.Token GetModel(string tokenAddress)
        {
            if (tokenAddress == "0x0") tokenAddress = "0x0000000000000000000000000000000000000000";
            var ChainId = (int)ShareParam.GetChainId();

            string key = tokenAddress + "{8C0CD852-8DBC-48C9-90F9-3F167E4D3863}";
            var result = Common.Cache.GetData<Model.Token>(key);
            if (result == null)
            {
                result = DAL.Token.GetModel(Share.ShareParam.DbConStr, ChainId, tokenAddress);
                if (result == null)
                {
                     var CallResult = SaveNewToken(tokenAddress).Result;                                 //这个自动保存的做法，是否合理！！！  存在的问题是没用图标，这个也可以忍受！
                    log.Info("SaveNewToken " + tokenAddress + " , Result Is " + CallResult.ToString());   
                    result = DAL.Token.GetModel(Share.ShareParam.DbConStr, ChainId, tokenAddress);
                }

                Common.Cache.AddByAbsoluteTime<Model.Token>(key, result);
            }
            return result;
        }


        private static async Task<bool> SaveNewToken(string tokenAddress)
        {
            var st = SaveTokenData(tokenAddress, @"TokenIcon\NoJPG.png", false);            //todo: 默认图标可以根据 Symbol 自动生成一张图片！ 暂时不处理
            log.Info("Auto SaveNewToken, TokenAddress Is " + tokenAddress + ", SaveTokenData Result: " + st.ToString());
            return await st;
        }

        public static System.Data.DataTable GetAllSelectedToken()
        {
            //todo: ChainId 没有使用到！
            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;

            string sql = @"
SELECT '" + BasePath + @"\' + ImagePath AS ImagePath, Symbol, Address, Decimals, (CASE Address WHEN '0x0000000000000000000000000000000000000000' THEN 1 ELSE 0 END) AS IsEth
FROM   Token where ChainId = @ChainId
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            var ChainId = (int)ShareParam.GetChainId();
            cm.Parameters.Add("@ChainId", SqlDbType.Int).Value = ChainId;   //WHERE   (ChainId = @ChainId)

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }


        public static System.Data.DataTable GetAllSelectedTokenWithoutEth()
        {
            //todo: ChainId 没有使用到！
            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;

            string sql = @"
SELECT '" + BasePath + @"\' + ImagePath AS ImagePath, Symbol, Address, Decimals, 
          (CASE Address WHEN '0x0000000000000000000000000000000000000000' THEN 1 WHEN '0x0000000000000000000000000000000000000000' THEN 1 ELSE 0 END) AS IsEth
FROM   Token
WHERE (Address <> '0x0000000000000000000000000000000000000000') and  ChainId = @ChainId
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            var ChainId = (int)ShareParam.GetChainId();
            cm.Parameters.Add("@ChainId", SqlDbType.Int).Value = ChainId;   //WHERE   (ChainId = @ChainId)

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }

        public static System.Data.DataTable GetAllToken()
        {
            //todo: ChainId 没有使用到！

            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;

            //string sql = @"
            //SELECT   T1.ChainId, T1.Name, T1.Address, T1.Decimals, T1.Symbol, '" + BasePath + @"\' + T1.ImagePath AS ImagePath, 
            //                (CASE T1.Address WHEN '0x0000000000000000000000000000000000000000' THEN 1 ELSE 0 END) AS IsEth, 
            //                T1.IsPricingToken, T1.PricingTokenAddress, T1.PricingTokenPrice, T1.PricingTokenPriceUpdateTime, 
            //                T2.Symbol AS PricingTokenSymbol
            //FROM      Token AS T1 LEFT OUTER JOIN
            //                Token AS T2 ON T1.PricingTokenAddress = T2.Address AND T1.ChainId = T2.ChainId
            //";

            string sql = @"
SELECT   T1.PricingIsFixed, (CASE WHEN T1.PricingIsFixed = 1 THEN 0 ELSE 1 END) AS PricingIsFloat, T1.ChainId, T1.Name, 
                T1.Address, T1.Decimals, T1.Symbol, '" + BasePath + @"\' + T1.ImagePath AS ImagePath, 
                (CASE T1.Address WHEN '0x0000000000000000000000000000000000000000' THEN 1 ELSE 0 END) AS IsEth, 
                T1.IsPricingToken, T1.PricingTokenAddress, T1.PricingTokenPrice, T1.PricingTokenPriceUpdateTime, 
                T2.Symbol AS PricingTokenSymbol
FROM      Token AS T1 LEFT OUTER JOIN
                Token AS T2 ON T1.PricingTokenAddress = T2.Address AND T1.ChainId = T2.ChainId 
WHERE   (T1.ChainId = @ChainId) AND (T2.ChainId = @ChainId)
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            var ChainId = (int)ShareParam.GetChainId();
            cm.Parameters.Add("@ChainId", SqlDbType.Int).Value = ChainId;   //WHERE   (ChainId = @ChainId)

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }


        public static bool SetIsPricingToken(string selectedToken)
        {
            string sql = @"
update Token set IsPricingToken = 0;
update Token set IsPricingToken = 1 where Address = @Address;
";
            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@Address", SqlDbType.NVarChar).Value = selectedToken;

            int RecordAffected = -1;
            cn.Open();
            try
            {
                RecordAffected = cm.ExecuteNonQuery();
            }
            finally
            {
                cn.Close();
            }
            return RecordAffected > 0;
        }


        public static Model.Token GetPricingToken()
        {
            var chainid = (int)Share.ShareParam.GetChainId();

            string sql = @"
            SELECT   *
            FROM      Token
            WHERE   (IsPricingToken = 1) and   (ChainId = @ChainId)
            ";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = chainid;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.Token.DataRow2Object(ds.Tables[0].Rows[0]);
            }
            else //if (ds.Tables[0].Rows.Count > 1)
            {
                log.Error("PricingToken Count > 1");
                return Model.Token.DataRow2Object(ds.Tables[0].Rows[0]);
            }

            ////return null           //如果没有设定，则使用 eth ！
            //if (!DAL.Token.Exist(ShareParam.DbConStr, chainid, ShareParam.AddressEth))
            //{
            //    var st = SaveTokenData(ShareParam.AddressEth, @"TokenIcon\Eth.png", true).Result;
            //    log.Info("SaveTokenData" + st.ToString());
            //}
            //SetIsPricingToken(ShareParam.AddressEth);
            //return DAL.Token.GetModel(ShareParam.DbConStr, chainid, ShareParam.AddressEth);
        }

    }

}
