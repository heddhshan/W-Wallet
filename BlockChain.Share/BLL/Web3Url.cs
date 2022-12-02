using System;
using System.Data;
using System.Data.SqlClient;

namespace BlockChain.Share.BLL
{
    public class Web3Url
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static bool _Web3IsConnected = false;
        public static bool Web3IsConnected
        {
            get
            {
                return _Web3IsConnected;
            }
        }

        public static void SetWeb3IsConnected(bool value)
        {
            lock (log)
            {
                _Web3IsConnected = value;
            }
        }

        private static bool IsRunning;

        private static void _SetIsRunning(bool _IsRunning)
        {
            IsRunning = _IsRunning;
        }
                

        private static System.Threading.Thread t = null;

        /// <summary>
        /// 检查Web3网络是否可以连接  //可以 不需要
        /// </summary>
        public static void DoCheckWeb3Connection(bool _IsRunning)
        {
            IsRunning = _IsRunning;
            if (_IsRunning && t == null)
            {
                t = new System.Threading.Thread(CheckWeb3Connection);
                t.Start();
            }
        }

        private static void CheckWeb3Connection()
        {
            log.Info("后台线程: CheckWeb3Connection(心跳测试Web3连接) Begin");

            try { 

            while (IsRunning)
            {
                var result = Common.Web3Helper.TestWeb3Url(ShareParam.Web3Url);
                SetWeb3IsConnected(result);
                for (int i = 0; i < 5 * 60; i++)    //约五分钟,心跳检测
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        if (IsRunning)
                        {
                            System.Threading.Thread.Sleep(1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            } 
            finally 
            { 
                log.Info("后台线程: CheckWeb3Connection(心跳测试Web3连接) End"); }
           
        }


        public static void UpdateIsSelected(string UrlHash)
        {
            string sql = @"
UPDATE  Web3Url SET IsSelected = 0;
UPDATE  Web3Url SET IsSelected = 1 WHERE   (UrlHash = @UrlHash);
";

            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@UrlHash", SqlDbType.NVarChar, 128).Value = UrlHash;

            cn.Open();
            try
            {
                cm.ExecuteNonQuery();
            }
            finally
            {
                cn.Close();
            }
        }



        public static Model.Web3Url GetCurWeb3UrlModel(bool _cache = true)
        {
            string key = "{3DDD8E6F-9B50-4C95-9082-5C06EE501B10}";
            if (_cache)
            {
                Model.Web3Url result = Common.Cache.GetData<Model.Web3Url>(key);
                if (result == null)
                {
                    result = _GetCurWeb3UrlModel();
                    Common.Cache.AddByAbsoluteTime(key, result);
                }
                return result;
            }
            else
            {
                var result = _GetCurWeb3UrlModel();
                Common.Cache.Remove(key);
                Common.Cache.AddByAbsoluteTime(key, result);
                return result;
            }

        }

        private static Model.Web3Url _GetCurWeb3UrlModel()
        {
            try
            {
                string conStr = Share.ShareParam.DbConStr;
                //, System.String UrlHash
                string sql = @"
SELECT  top(1) *
FROM      Web3Url
WHERE   (IsSelected = 1)
";

                SqlConnection cn = new SqlConnection(conStr);
                SqlCommand cm = new SqlCommand();
                cm.Connection = cn;
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cm;
                System.Data.DataSet ds = new System.Data.DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    return Model.Web3Url.DataRow2Object(ds.Tables[0].Rows[0]);
                }
                else if (ds.Tables[0].Rows.Count > 1)
                {
                    throw new Exception("数据错误，对应多条记录！");
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return null;
            }
        }

        public static System.Data.DataTable GetUrlDt()
        {
            string sql = @"
SELECT  *
FROM      Web3Url
";
            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }



        public static System.Data.DataTable GetApiUrlDt()
        {
            string sql = @"
SELECT  *
FROM      Web3Url
WHERE   (IsApi = 1)
";
            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }


    }
}
