//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 10:58:46
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Share.DAL
{


    #region Add Time 2022/6/11 21:03:51

    internal class AppInfo_OnPublishAppDownload
    {
        #region 对应的数据库表 AppInfo_OnPublishAppDownload
        public const string TableName = @"AppInfo_OnPublishAppDownload";
        #endregion

        #region  表 AppInfo_OnPublishAppDownload 的Insert操作
        public static void Insert(string conStr, Model.AppInfo_OnPublishAppDownload model)
        {
            string sql = @"insert into AppInfo_OnPublishAppDownload (ContractAddress,BlockNumber,TransactionHash,_eventId,_AppId,_PlatformId,_Version,_HttpLink,_BTLink,_eMuleLink,_IpfsLink,_OtherLink) values (@ContractAddress,@BlockNumber,@TransactionHash,@_eventId,@_AppId,@_PlatformId,@_Version,@_HttpLink,@_BTLink,@_eMuleLink,@_IpfsLink,@_OtherLink)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@BlockNumber", SqlDbType.BigInt, 8).Value = model.BlockNumber;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@_eventId", SqlDbType.BigInt, 8).Value = model._eventId;
            cm.Parameters.Add("@_AppId", SqlDbType.Int, 4).Value = model._AppId;
            cm.Parameters.Add("@_PlatformId", SqlDbType.Int, 4).Value = model._PlatformId;
            cm.Parameters.Add("@_Version", SqlDbType.Int, 4).Value = model._Version;
            cm.Parameters.Add("@_HttpLink", SqlDbType.NVarChar, 1024).Value = model._HttpLink;
            cm.Parameters.Add("@_BTLink", SqlDbType.NVarChar, 1024).Value = model._BTLink;
            cm.Parameters.Add("@_eMuleLink", SqlDbType.NVarChar, 1024).Value = model._eMuleLink;
            cm.Parameters.Add("@_IpfsLink", SqlDbType.NVarChar, 1024).Value = model._IpfsLink;
            cm.Parameters.Add("@_OtherLink", SqlDbType.NVarChar, 1024).Value = model._OtherLink;

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
        #endregion

        #region  表 AppInfo_OnPublishAppDownload 的Delete操作
        public static int Delete(string conStr, System.String ContractAddress, System.Int64 _eventId)
        {
            string sql = @"Delete From AppInfo_OnPublishAppDownload Where ContractAddress = @ContractAddress and _eventId = @_eventId";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
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

        #region  表 AppInfo_OnPublishAppDownload 的 Update 操作
        public static int Update(string conStr, Model.AppInfo_OnPublishAppDownload model)
        {
            string sql = @"Update AppInfo_OnPublishAppDownload Set BlockNumber = @BlockNumber, TransactionHash = @TransactionHash, _AppId = @_AppId, _PlatformId = @_PlatformId, _Version = @_Version, _HttpLink = @_HttpLink, _BTLink = @_BTLink, _eMuleLink = @_eMuleLink, _IpfsLink = @_IpfsLink, _OtherLink = @_OtherLink Where ContractAddress = @ContractAddress And _eventId = @_eventId ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@BlockNumber", SqlDbType.BigInt, 8).Value = model.BlockNumber;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@_eventId", SqlDbType.BigInt, 8).Value = model._eventId;
            cm.Parameters.Add("@_AppId", SqlDbType.Int, 4).Value = model._AppId;
            cm.Parameters.Add("@_PlatformId", SqlDbType.Int, 4).Value = model._PlatformId;
            cm.Parameters.Add("@_Version", SqlDbType.Int, 4).Value = model._Version;
            cm.Parameters.Add("@_HttpLink", SqlDbType.NVarChar, 1024).Value = model._HttpLink;
            cm.Parameters.Add("@_BTLink", SqlDbType.NVarChar, 1024).Value = model._BTLink;
            cm.Parameters.Add("@_eMuleLink", SqlDbType.NVarChar, 1024).Value = model._eMuleLink;
            cm.Parameters.Add("@_IpfsLink", SqlDbType.NVarChar, 1024).Value = model._IpfsLink;
            cm.Parameters.Add("@_OtherLink", SqlDbType.NVarChar, 1024).Value = model._OtherLink;

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

        #region  表 AppInfo_OnPublishAppDownload 的GetModel操作
        public static Model.AppInfo_OnPublishAppDownload GetModel(string conStr, System.String ContractAddress, System.Int64 _eventId)
        {
            string sql = @"select * from AppInfo_OnPublishAppDownload Where ContractAddress = @ContractAddress and _eventId = @_eventId ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
            cm.Parameters.Add("@_eventId", SqlDbType.BigInt, 8).Value = _eventId;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.AppInfo_OnPublishAppDownload.DataRow2Object(ds.Tables[0].Rows[0]);
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


        #endregion

        #region  表 AppInfo_OnPublishAppDownload 的 Exist 操作 判断
        public static bool Exist(string conStr, System.String ContractAddress, System.Int64 _eventId)
        {
            string sql = @"Select Count(*) From AppInfo_OnPublishAppDownload Where ContractAddress = @ContractAddress and _eventId = @_eventId ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
            cm.Parameters.Add("@_eventId", SqlDbType.BigInt, 8).Value = _eventId;

            cn.Open();
            try
            {
                return Convert.ToInt32(cm.ExecuteScalar()) > 0;
            }
            finally
            {
                cn.Close();
            }
        }

        #endregion

    }

    #endregion



}