using BlockChain.Share;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BlockChain.Share.BLL
{
    public class ContEventBlockNum
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static long GetEventLastSynBlock(string contractAddress, string eventName)
        {
            string sql = @"
SELECT   LastBlockNumber
FROM      ContEventBlockNum
WHERE   (ContractAddress = @ContractAddress) AND (EventName = @EventName)
";
            SqlConnection cn = new SqlConnection(ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar).Value = contractAddress;
            cm.Parameters.Add("@EventName", SqlDbType.NVarChar).Value = eventName;

            cn.Open();
            try
            {
                long result = (long)Convert.ToDecimal(cm.ExecuteScalar()) + 1;
                if (result < ShareParam.FromBlock)
                {
                    result = ShareParam.FromBlock;
                }
                return result;
            }
            catch (Exception ex)
            {
                log.Error("contractAddress=" + contractAddress + ", eventName=" + eventName, ex);
                return 0;
            }
            finally
            {
                cn.Close();
            }
        }


        public static void UpdateEventLastSysBlock(string contractAddress, string eventName, long lastSynBlockNum, ulong ThisNowBlockNum)
        {
            //lastSynBlockNum 不能超过当前区块。
            if (lastSynBlockNum + ShareParam.NowBlockNum > (long)ThisNowBlockNum) 
            {
                lastSynBlockNum = (long)ThisNowBlockNum - ShareParam.NowBlockNum;
            }

            Model.ContEventBlockNum model = new Model.ContEventBlockNum();
            model.ContractAddress = contractAddress;
            model.EventName = eventName;
            //故意少3，因为有时候web3的http数据没那么准确，导致事件数据无法读取，这种情况极少，但一直存在。这样会缓和此问题。
            model.LastBlockNumber = lastSynBlockNum - 3;    
            if (!DAL.ContEventBlockNum.Exist( ShareParam.DbConStr, contractAddress, eventName))
            {
                DAL.ContEventBlockNum.Insert( ShareParam.DbConStr, model);
            }
            else
            {
                DAL.ContEventBlockNum.Update( ShareParam.DbConStr, model);
            }
        }


    }
}
