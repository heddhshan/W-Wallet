using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;



namespace BlockChain.Share.BLL
{
    public class AppInfo
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<System.Numerics.BigInteger> getNowBlockTimestamp()
        {
            try
            {
                var web3 = ShareParam.GetWeb3();
                Contract.AppInfo.AppInfoService service = new Contract.AppInfo.AppInfoService(web3, ShareParam.AddressAppInfo);
                var result = await service.GetCurrentBlockTimestampQueryAsync();
                return result;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return 0;
            }
        }


        public static int getFromBlock()
        {
            string key = "{B946C11D-0189-4350-82F7-DDCB7500136B}";
            var result = Common.Cache.GetData(key);
            if (result == null)
            {
                result = _getFromBlock();
                if (0 < (int)result)
                {
                    Common.Cache.AddBySlidingTime(key, result);
                }
            }

            return (int)result;
        }

        private static int _getFromBlock()
        {
            try
            {
                string contractAddress = ShareParam.AddressAppInfo;
                var web3 = ShareParam.GetWeb3();
                Contract.AppInfo.AppInfoService service = new Contract.AppInfo.AppInfoService(web3, contractAddress);
                var result = service.FromBlockQueryAsync().Result;

                return (int)result;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return 0;
            }
        }


        #region  表 AppInfo_OnPublishAppVersion 的Delete操作
        
        public static int DeleteVersion(System.String ContractAddress, System.Int64 appId, int platformId, System.Int64 _eventId)
        {
            string conStr = Share.ShareParam.DbConStr;
            string sql = @"
DELETE FROM AppInfo_OnPublishAppVersion
WHERE   (ContractAddress = @ContractAddress) AND (_AppId = @AppId) AND (_PlatformId = @PlatformId)  and _eventId < @_eventId";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
            cm.Parameters.Add("@AppId", SqlDbType.BigInt, 8).Value = appId;
            cm.Parameters.Add("@PlatformId", SqlDbType.BigInt, 8).Value = platformId;
            cm.Parameters.Add("@_eventId", SqlDbType.BigInt, 8).Value = _eventId;

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
            return RecordAffected;
        }
        
        #endregion


        #region Solidity Event  OnPublishAppVersion

        private static bool IsOnPublishAppVersion = false;

        public static async Task<bool> SynOnPublishAppVersion()
        {
            if (IsOnPublishAppVersion) return false;
            IsOnPublishAppVersion = true;
            var ThisNowBlockNum = (ulong)(await Common.Web3Helper.GetLastBlockNuber(ShareParam.Web3Url));
            ulong FromBlockNumber;
            ulong EndBlockNumber;
            try
            {
                string contractAddress = ShareParam.AddressAppInfo;
                FromBlockNumber = (ulong)BLL.ContEventBlockNum.GetEventLastSynBlock(contractAddress, "OnPublishAppVersion");
                EndBlockNumber = FromBlockNumber + ShareParam.MaxBlock;
                Nethereum.RPC.Eth.DTOs.BlockParameter fromBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(FromBlockNumber);
                Nethereum.RPC.Eth.DTOs.BlockParameter endBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(EndBlockNumber);

                var web3 = ShareParam.GetWeb3();
                var teh = web3.Eth.GetEvent<Contract.AppInfo.ContractDefinition.OnPublishAppVersionEventDTO>(contractAddress);
                var fat = teh.CreateFilterInput(fromBlock, endBlock);     //CreateFilterInput(fromBlock,   endBlock);
                var ate = await teh.GetAllChangesAsync(fat);
                //if (ate.Count == 0)
                //{
                //    return false;
                //}

                foreach (var ev in ate)
                {
                    var num = (ulong)ev.Log.BlockNumber.Value;
                    var EventId = (long)ev.Event.EventId;   //合约自定义的事件ID

                    var model = DAL.AppInfo_OnPublishAppVersion.GetModel(Share.ShareParam.DbConStr, contractAddress, EventId);
                    if (model == null)
                    {

                        // event OnSetT3sBaseToken18(address indexed _token, uint _t3sAmount, uint _eventId);
                        model = new Model.AppInfo_OnPublishAppVersion();
                        model.BlockNumber = (long)num;
                        model.TransactionHash = ev.Log.TransactionHash;
                        model.ContractAddress = contractAddress;
                        model._eventId = EventId;

                        model._AppId = (int)ev.Event.AppId;
                        model._PlatformId = (int)ev.Event.PlatformId;
                        model._Version = (int)ev.Event.Version;
                        model._Sha256Value = ev.Event.Sha256Value.ToHex(true);
                        model._AppName = ev.Event.AppName;
                        model._UpdateInfo = ev.Event.UpdateInfo;
                        model._IconUri = ev.Event.IconUri;

                        model.ValidateEmptyAndLen();

                        DeleteVersion(contractAddress, model._AppId, model._PlatformId, EventId);       // 删除以前的版本信息，不需要保存的！

                        DAL.AppInfo_OnPublishAppVersion.Insert(Share.ShareParam.DbConStr, model);
                    }
                    else
                    {
                        //log.Error(LanguageHelper.GetTranslationText("重复读取日志") + " - " + BlockChain.Share.ShareParam.GetLineNum().ToString());
                        var m = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                        log.Error(Share.LanguageHelper.GetTranslationText("重复读取日志") + " - " + m + " - " + Share.ShareParam.GetLineNum().ToString());
                    }
                }

                BLL.ContEventBlockNum.UpdateEventLastSysBlock(contractAddress, "OnPublishAppVersion", (long)EndBlockNumber, ThisNowBlockNum);
            }
            catch (Exception ex)
            {
                log.Error("*", ex);
                return false;
            }
            finally
            {
                IsOnPublishAppVersion = false;
            }
            if (EndBlockNumber + ShareParam.NowBlockNum >= ThisNowBlockNum)
            {
                return true;
            }
            else
            {
                //递归调用自己
                return await SynOnPublishAppVersion();
            }
        }

        #endregion


        #region Solidity Event  OnPublishAppDownload

        private static bool IsOnPublishAppDownload = false;

        public static async Task<bool> SynOnPublishAppDownload()
        {
            if (IsOnPublishAppDownload) return false;
            IsOnPublishAppDownload = true;
            var ThisNowBlockNum = (ulong)(await Common.Web3Helper.GetLastBlockNuber(ShareParam.Web3Url));
            ulong FromBlockNumber;
            ulong EndBlockNumber;
            try
            {
                string contractAddress = ShareParam.AddressAppInfo;
                FromBlockNumber = (ulong)BLL.ContEventBlockNum.GetEventLastSynBlock(contractAddress, "OnPublishAppDownload");
                EndBlockNumber = FromBlockNumber + ShareParam.MaxBlock;
                Nethereum.RPC.Eth.DTOs.BlockParameter fromBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(FromBlockNumber);
                Nethereum.RPC.Eth.DTOs.BlockParameter endBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(EndBlockNumber);

                var web3 = ShareParam.GetWeb3();
                var teh = web3.Eth.GetEvent<Contract.AppInfo.ContractDefinition.OnPublishAppDownloadEventDTO>(contractAddress);
                var fat = teh.CreateFilterInput(fromBlock, endBlock);     //CreateFilterInput(fromBlock,   endBlock);
                var ate = await teh.GetAllChangesAsync(fat);
                //if (ate.Count == 0)
                //{
                //    return false;
                //}

                foreach (var ev in ate)
                {
                    var num = (ulong)ev.Log.BlockNumber.Value;
                    var EventId = (long)ev.Event.EventId;   //合约自定义的事件ID

                    var model = DAL.AppInfo_OnPublishAppDownload.GetModel(Share.ShareParam.DbConStr, contractAddress, EventId);
                    if (model == null)
                    {
                        model = new Model.AppInfo_OnPublishAppDownload();
                        model.BlockNumber = (long)num;
                        model.TransactionHash = ev.Log.TransactionHash;
                        model.ContractAddress = contractAddress;
                        model._eventId = EventId;

                        model._AppId = (int)ev.Event.AppId;
                        model._PlatformId = (int)ev.Event.PlatformId;
                        model._Version = (int)ev.Event.Version;

                        model._HttpLink = ev.Event.HttpLink;
                        model._BTLink = ev.Event.BTLink;
                        model._eMuleLink = ev.Event.EMuleLink;
                        model._IpfsLink = ev.Event.IpfsLink;
                        model._OtherLink = ev.Event.OtherLink;

                        model.ValidateEmptyAndLen();
                        DAL.AppInfo_OnPublishAppDownload.Insert(Share.ShareParam.DbConStr, model);
                    }
                    else
                    {
                        //log.Error(LanguageHelper.GetTranslationText("重复读取日志") + " - " + ShareParam.GetLineNum().ToString());
                        var m = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                        log.Error(Share.LanguageHelper.GetTranslationText("重复读取日志") + " - " + m + " - " + Share.ShareParam.GetLineNum().ToString());
                    }
                }

                BLL.ContEventBlockNum.UpdateEventLastSysBlock(contractAddress, "OnPublishAppDownload", (long)EndBlockNumber, ThisNowBlockNum);
            }
            catch (Exception ex)
            {
                log.Error("*", ex);
                return false;
            }
            finally
            {
                IsOnPublishAppDownload = false;
            }
            if (EndBlockNumber + ShareParam.NowBlockNum >= ThisNowBlockNum)
            {
                return true;
            }
            else
            {
                //递归调用自己
                return await SynOnPublishAppDownload();
            }
        }

        #endregion

        public static async Task<bool> UpdateAppInfo()
        {
            await SynOnPublishAppVersion();
            await SynOnPublishAppDownload();
            return true;
        }

        public static int DeleteAllAppInfo()
        {
            string sql = @"
DELETE FROM AppInfo_OnPublishAppVersion;
DELETE FROM AppInfo_OnPublishAppDownload;
DELETE FROM ContEventBlockNum WHERE   (EventName = 'OnPublishAppDownload') OR (EventName = 'OnPublishAppVersion');
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cn.Open();
            try { return cm.ExecuteNonQuery(); }
            finally
            {
                cn.Close(); 
            }
        }

        /// <summary>
        /// 得到 APP 列表！
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCurAppInfo()
        {
            //            string sql = @"
            //SELECT Info._AppId, Info._PlatformId, Info._Version, Info._Sha256Value, Info._AppName, Info._UpdateInfo, Info._IconUri, Download._IpfsLink, Download._eMuleLink, Download._BTLink, Download._HttpLink, 
            //          Download._OtherLink
            //FROM   AppInfo_OnPublishAppVersion AS Info LEFT OUTER JOIN
            //          AppInfo_OnPublishAppDownload AS Download ON Info.ContractAddress = Download.ContractAddress AND Info._AppId = Download._AppId AND Info._PlatformId = Download._PlatformId AND 
            //          Info._Version = Download._Version
            //";

            string sql = @"
SELECT   Info._AppId, Info._PlatformId, Info._Version, Info._Sha256Value, Info._AppName, Info._UpdateInfo, Info._IconUri, 
                Download._IpfsLink, Download._eMuleLink, Download._BTLink, Download._HttpLink, Download._OtherLink
FROM      AppInfo_OnPublishAppVersion AS Info LEFT OUTER JOIN
                AppInfo_OnPublishAppDownload AS Download ON Info.ContractAddress = Download.ContractAddress AND 
                Info._AppId = Download._AppId AND Info._PlatformId = Download._PlatformId AND 
                Info._Version = Download._Version
WHERE   (Info._eventId =
                    (SELECT   MAX(_eventId) AS Expr1
                     FROM      AppInfo_OnPublishAppVersion
                     WHERE   (ContractAddress = @ContractAddress))) AND (Info.ContractAddress = @ContractAddress) AND 
                (Download._eventId =
                    (SELECT   MAX(_eventId) AS Expr1
                     FROM      AppInfo_OnPublishAppDownload
                     WHERE   (ContractAddress = @ContractAddress))) AND (Download.ContractAddress = @ContractAddress)
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            
            string contractAddress = ShareParam.AddressAppInfo;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = contractAddress;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }


        ////////////////////////////////////////////////////////////////////////////////

        private static string _GetKeyAddress(string key)
        {
            try
            {
                Nethereum.Web3.Web3 w3 = Share.ShareParam.GetWeb3();
                Contract.AppInfo.AppInfoService Service = new Contract.AppInfo.AppInfoService(w3, ShareParam.AddressAppInfo);
                //var result = await Service.GetKeyAddress1QueryAsync(key);
                var result = Service.GetKeyAddress1QueryAsync(key).Result;         //卡在这里不执行了！！！
                log.Info(key + " : " + result); //把地址记录入日志
                return result;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return null;
            }
        }


        private static string _GetKeyString(string key)
        {
            try
            {
                Nethereum.Web3.Web3 w3 = Share.ShareParam.GetWeb3();
                Contract.AppInfo.AppInfoService Service = new Contract.AppInfo.AppInfoService(w3, ShareParam.AddressAppInfo);
                var result = Service.GetKeyString1QueryAsync(key).Result;
                log.Info(key + " : " + result); //把地址记录入日志
                return result;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return null;
            }
        }


        public static string GetKeyString(string key)
        {
            string CacheKey = key + "{83A4DAB0-93D8-4D5A-9A6A-8C09B4DAB6AF}";
            string result = Common.Cache.GetData<string>(CacheKey);
            if (null == result)
            {
                result = _GetKeyString(key);
                Common.Cache.AddBySlidingTime<string>(CacheKey, result, 60 * 30);
            }
            return result;
        }


        /// <summary>
        /// GetKeyAddress, 使用了缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKeyAddress(string key)
        {
            string CacheKey = key + "{AE76C7B4-CD8F-42D3-97DF-16DEDF8B5F81}";
            string result = Common.Cache.GetData<string>(CacheKey);
            if (null == result)
            {
                result = _GetKeyAddress(key);
                Common.Cache.AddBySlidingTime<string>(CacheKey, result, 60 * 30);
            }
            return result;
        }

        ////function getUserKeyValue(address _user, uint256 _key) external override view returns(string memory)
        ////{
        ////    return userKeyValue[_user][_key];
        ////}
        //private static string _GetUserKeyValue(string userAddress, string key)
        //{
        //    try
        //    {
        //        Nethereum.Web3.Web3 w3 = Share.ShareParam.GetWeb3();
        //        Contract.AppInfo.AppInfoService Service = new Contract.AppInfo.AppInfoService(w3, ShareParam.AddressTools);
        //        var result = Service.GetUserKeyString1QueryAsync(userAddress, key).Result;
        //        return result;

        //        //Service.ContractHandler..DeployContractRequestAsync()
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("", ex);
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// GetUserKeyValue, 使用了缓存
        ///// </summary>
        ///// <param name="userAddress"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string GetUserKeyValue(string userAddress, string key)
        //{
        //    string CacheKey = key + "{F19B4393-9192-439C-B540-D2676DC972BE}" + userAddress;
        //    string result = Common.Cache.GetData<string>(CacheKey);
        //    if (null == result)
        //    {
        //        result = _GetUserKeyValue(userAddress, key);
        //        Common.Cache.AddBySlidingTime<string>(CacheKey, result, 60 * 30);
        //    }
        //    return result;
        //}

        //function IsContract(address account) public view returns(bool)
        //{
        //    return account.isContract();
        //}

        //public static bool? IsContractAddress(string address)
        //{
        //    try
        //    {
        //        Nethereum.Web3.Web3 w3 = Share.ShareParam.GetWeb3();
        //        Contract.AppInfo.AppInfoService Service = new Contract.AppInfo.AppInfoService(w3, ShareParam.AddressTools);
        //        var result = Service.IsContractQueryAsync(address).Result;
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("", ex);
        //        return null;
        //    }
        //}

        //public static System.Numerics.BigInteger GetBlcockNow()
        //{
        //    var web3 = Share.ShareParam.GetWeb3();      //读取属性不需要账号信息
        //    Contract.AppInfo.AppInfoService service = new Contract.AppInfo.AppInfoService(web3, ShareParam.AddressTools);
        //    var result = service.GetBlcockNowQueryAsync().Result;
        //    return result;
        //}



        /////////////////////////////////////////////////////////////////////////////////////////////////////



        //处理通知消息  //    event OnPublishNotice(address _user, uint _noticeId, string _subject, string _body); 

        private static bool IsOnPublishNotice = false;

        public async static Task<bool> SynOnPublishNotice()
        {
            if (IsOnPublishNotice) return false;
            IsOnPublishNotice = true;
            var ThisNowBlockNum = (ulong)(Common.Web3Helper.GetNowBlockNuber(ShareParam.Web3Url));
            ulong FromBlockNumber;  //1
            ulong EndBlockNumber;
            try
            {
                string contractAddress = ShareParam.AddressAppInfo;
                string eventName = "OnPublishNotice";
                //var num = (ulong)ContEventBlockNum.GetEventLastSynBlock(contractAddress, eventName);
                //Nethereum.RPC.Eth.DTOs.BlockParameter fromBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(num);

                FromBlockNumber = (ulong)ContEventBlockNum.GetEventLastSynBlock(contractAddress, eventName);
                EndBlockNumber = FromBlockNumber + ShareParam.MaxBlock;
                Nethereum.RPC.Eth.DTOs.BlockParameter fromBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(FromBlockNumber);
                Nethereum.RPC.Eth.DTOs.BlockParameter endBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(EndBlockNumber);

                var web3 = ShareParam.GetWeb3();                           //读取属性不需要账号信息
                //num = (ulong)(await Common.Web3Helper.GetLastBlockNuber(ShareParam.Web3Url));     //要及早更新，原来的写法存在漏读事件的可能！

                var teh = web3.Eth.GetEvent<Contract.AppInfo.ContractDefinition.OnPublishNoticeEventDTO>(contractAddress);
                var fat = teh.CreateFilterInput(fromBlock, endBlock);     //CreateFilterInput(fromBlock,   endBlock);
                var ate = await teh.GetAllChangesAsync(fat);
                //if (ate.Count == 0)
                //{
                //    return false;
                //}

                foreach (var ev in ate)
                {
                    var num = (ulong)ev.Log.BlockNumber.Value;
                    var model = DAL.Appinfo_OnPublishNotice.GetModel(Share.ShareParam.DbConStr, ev.Log.TransactionHash);
                    if (model == null)
                    {
                        model = new Model.Appinfo_OnPublishNotice();
                        model.BlockNumber = (long)num;
                        model.TransactionHash = ev.Log.TransactionHash;
                        model.ContractAddress = contractAddress;
                        model._publisher = ev.Event.Publisher;
                        model._appId = (long)ev.Event.AppId;
                        model._subject = ev.Event.Subject;
                        model._body = ev.Event.Body;

                        //var tx = await    web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(ev.Log.TransactionHash) ;
                        //var b = await web3.Eth.Blocks..CoinBase..Blocks.co.GetBlockNumber.SendRequestAsync();
                        //model.BlockTime = 

                        model.ValidateEmptyAndLen();
                        DAL.Appinfo_OnPublishNotice.Insert(Share.ShareParam.DbConStr, model);
                    }
                    else
                    {
                        //log.Error(LanguageHelper.GetTranslationText("重复读取日志") + " - " + ShareParam.GetLineNum().ToString());
                        var m = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                        log.Error(Share.LanguageHelper.GetTranslationText("重复读取日志") + " - " + m + " - " + Share.ShareParam.GetLineNum().ToString());
                    }
                }

                //num = (ulong)(await Common.Web3Helper.GetLastBlockNuber(ShareParam.Web3Url));
                ContEventBlockNum.UpdateEventLastSysBlock(contractAddress, eventName, (long)EndBlockNumber, ThisNowBlockNum);



            }
            catch (Exception ex)
            {
                log.Error("*", ex);
                return false;
            }
            finally
            {
                IsOnPublishNotice = false;
            }
            if (EndBlockNumber + ShareParam.NowBlockNum >= ThisNowBlockNum)
            {
                return true;
            }
            else
            {
                //递归调用自己
                return await SynOnPublishNotice();
            }
        }


        ///// <summary>
        ///// 系统公告，ID=100；
        ///// </summary>
        //public const long NoticeId_Sys = 100;

        //通知ID， 使用 AppID， 不单独定义！


        public static List<Model.Appinfo_OnPublishNotice> GetAllNotices(long _appId)
        {
            if (_appId == 0)
            {
                throw new Exception("_appId == 0");
            }

            string sql = @"
SELECT  *
FROM      Appinfo_OnPublishNotice
WHERE   (ContractAddress = @ContractAddress) AND (_appId = @AppId)
ORDER BY BlockNumber DESC
";

            SqlConnection cn = new SqlConnection(ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ShareParam.AddressAppInfo;
            cm.Parameters.Add("@AppId", SqlDbType.BigInt).Value = _appId;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return Model.Appinfo_OnPublishNotice.DataTable2List(ds.Tables[0]);
        }


    }


}
