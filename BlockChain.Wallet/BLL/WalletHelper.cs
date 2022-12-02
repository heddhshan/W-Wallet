using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BlockChain.Wallet.BLL
{
    public static class WalletHelper
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static bool IsSynOnDeployContract = false;

        public async static Task<bool> SynOnDeployContract()
        {
            if (IsSynOnDeployContract) return false;
            IsSynOnDeployContract = true;
            var ThisNowBlockNum = (ulong)(Common.Web3Helper.GetNowBlockNuber(Share.ShareParam.Web3Url));
            ulong FromBlockNumber;  //1
            ulong EndBlockNumber;
            try
            {
                string contractAddress = SystemParam.AddressWalletHelper;
                string eventName = "OnDeployContract";
                //var num = (ulong)ContEventBlockNum.GetEventLastSynBlock(contractAddress, eventName);
                //Nethereum.RPC.Eth.DTOs.BlockParameter fromBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(num);

                FromBlockNumber = (ulong)Share.BLL.ContEventBlockNum.GetEventLastSynBlock(contractAddress, eventName);
                EndBlockNumber = FromBlockNumber + Share.ShareParam.MaxBlock;
                Nethereum.RPC.Eth.DTOs.BlockParameter fromBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(FromBlockNumber);
                Nethereum.RPC.Eth.DTOs.BlockParameter endBlock = new Nethereum.RPC.Eth.DTOs.BlockParameter(EndBlockNumber);

                var web3 = Share.ShareParam.GetWeb3();                           //读取属性不需要账号信息
                //num = (ulong)(await Common.Web3Helper.GetLastBlockNuber(Share.ShareParam.Web3Url));     //要及早更新，原来的写法存在漏读事件的可能！

                var teh = web3.Eth.GetEvent<Contract.WalletHelper.ContractDefinition.OnDeployContractEventDTO>(contractAddress);
                var fat = teh.CreateFilterInput(fromBlock, endBlock);     //CreateFilterInput(fromBlock,   endBlock);
                var ate = await teh.GetAllChangesAsync(fat);
                //if (ate.Count == 0)
                //{
                //    return false;
                //}

                int ChainId = (int)Share.ShareParam.GetChainId();

                foreach (var ev in ate)
                {
                    var num = (ulong)ev.Log.BlockNumber.Value;
                    var model = DAL.WalletHelper_OnDeployContract.GetModel(Share.ShareParam.DbConStr, ChainId, ev.Event.Contract);
                    if (model == null)
                    {
                        model = new Model.WalletHelper_OnDeployContract();
                        model.BlockNumber = (long)num;
                        model.TransactionHash = ev.Log.TransactionHash;
                        model.ContractAddress = contractAddress;
                        model._user = ev.Event.User;
                        model.ChainId = ChainId;
                        model._amount = (decimal)((double)ev.Event.Amount / Math.Pow(10, 18));
                        model._contract = ev.Event.Contract;
                        model._salt = ev.Event.Salt.ToHex(true);
                        model._bytecodeHash = ev.Event.BytecodeHash.ToHex(true);

                        model.ValidateEmptyAndLen();
                        DAL.WalletHelper_OnDeployContract.Insert(Share.ShareParam.DbConStr, model);
                    }
                    else
                    {
                        //log.Error(LanguageHelper.GetTranslationText("重复读取日志") + " - " + Share.ShareParam.GetLineNum().ToString());
                        var m = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                        log.Error(Share.LanguageHelper.GetTranslationText("重复读取日志") + " - " + m + " - " + Share.ShareParam.GetLineNum().ToString());
                    }
                }

                Share.BLL.ContEventBlockNum.UpdateEventLastSysBlock(contractAddress, eventName, (long)EndBlockNumber, ThisNowBlockNum);

            }
            catch (Exception ex)
            {
                log.Error("*", ex);
                return false;
            }
            finally
            {
                IsSynOnDeployContract = false;
            }
            if (EndBlockNumber + Share.ShareParam.NowBlockNum >= ThisNowBlockNum)
            {
                return true;
            }
            else
            {
                //递归调用自己
                return await SynOnDeployContract();
            }
        }


        public static DataTable GetOnDeployContract(string user)
        {
            string sql = @"
SELECT  D.ContractAddress, D.BlockNumber, D.TransactionHash, D.ChainId, D._contract, D._user, D._amount, D._salt, 
                   D._bytecodeHash, L.LocalRemark, L._bytecode
FROM      WalletHelper_OnDeployContract AS D LEFT OUTER JOIN
                   WalletHelper_OnDeployContract_Local AS L ON D.ContractAddress = L.ContractAddress AND 
                   D.TransactionHash = L.TransactionHash AND D.ChainId = L.ChainId
WHERE  ( (D._user = @user) OR (@user IS NULL) OR (@user = ''))  AND (D.ContractAddress = @ContractAddress)
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = SystemParam.AddressWalletHelper;
            cm.Parameters.Add("@user", SqlDbType.NVarChar, 43).Value = user;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }

        public static bool? IsContractAddress(string address)
        {
            try
            {
                Nethereum.Web3.Web3 w3 = Share.ShareParam.GetWeb3();
                Contract.WalletHelper.WalletHelperService Service = new Contract.WalletHelper.WalletHelperService(w3, SystemParam.AddressWalletHelper);
                var result = Service.IsContractQueryAsync(address).Result;
                return result;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return null;
            }
        }


    }
}
