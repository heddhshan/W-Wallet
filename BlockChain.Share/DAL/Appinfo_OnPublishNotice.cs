using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Share.DAL
{
    internal class Appinfo_OnPublishNotice
    {
        #region 对应的数据库表 Appinfo_OnPublishNotice
        public const string TableName = @"Appinfo_OnPublishNotice";
        #endregion

        #region  表 Appinfo_OnPublishNotice 的Insert操作
        public static void Insert(string conStr, Model.Appinfo_OnPublishNotice model)
        {
            string sql = @"insert into Appinfo_OnPublishNotice (ContractAddress,BlockNumber,TransactionHash,_publisher,_appId,_subject,_body,BlockTime) values (@ContractAddress,@BlockNumber,@TransactionHash,@_publisher,@_appId,@_subject,@_body,@BlockTime)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@BlockNumber", SqlDbType.BigInt, 8).Value = model.BlockNumber;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@_publisher", SqlDbType.NVarChar, 43).Value = model._publisher;
            cm.Parameters.Add("@_appId", SqlDbType.BigInt, 8).Value = model._appId;
            cm.Parameters.Add("@_subject", SqlDbType.NVarChar, 1024).Value = model._subject;
            cm.Parameters.Add("@_body", SqlDbType.NVarChar).Value = model._body;
            cm.Parameters.Add("@BlockTime", SqlDbType.DateTime, 8).Value = model.BlockTime;

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

        #region  表 Appinfo_OnPublishNotice 的Delete操作
        public static int Delete(string conStr, System.String TransactionHash)
        {
            string sql = @"Delete From Appinfo_OnPublishNotice Where TransactionHash = @TransactionHash";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = TransactionHash;
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

        #region  表 Appinfo_OnPublishNotice 的 Update 操作
        public static int Update(string conStr, Model.Appinfo_OnPublishNotice model)
        {
            string sql = @"Update Appinfo_OnPublishNotice Set ContractAddress = @ContractAddress, BlockNumber = @BlockNumber, _publisher = @_publisher, _appId = @_appId, _subject = @_subject, _body = @_body, BlockTime = @BlockTime Where TransactionHash = @TransactionHash ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@BlockNumber", SqlDbType.BigInt, 8).Value = model.BlockNumber;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@_publisher", SqlDbType.NVarChar, 43).Value = model._publisher;
            cm.Parameters.Add("@_appId", SqlDbType.BigInt, 8).Value = model._appId;
            cm.Parameters.Add("@_subject", SqlDbType.NVarChar, 1024).Value = model._subject;
            cm.Parameters.Add("@_body", SqlDbType.NVarChar).Value = model._body;
            cm.Parameters.Add("@BlockTime", SqlDbType.DateTime, 8).Value = model.BlockTime;

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

        #region  表 Appinfo_OnPublishNotice 的GetModel操作
        public static Model.Appinfo_OnPublishNotice GetModel(string conStr, System.String TransactionHash)
        {
            string sql = @"select * from Appinfo_OnPublishNotice Where TransactionHash = @TransactionHash ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = TransactionHash;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.Appinfo_OnPublishNotice.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 Appinfo_OnPublishNotice 的 Exist 操作 判断
        public static bool Exist(string conStr, System.String TransactionHash)
        {
            string sql = @"Select Count(*) From Appinfo_OnPublishNotice Where TransactionHash = @TransactionHash ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = TransactionHash;

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


}