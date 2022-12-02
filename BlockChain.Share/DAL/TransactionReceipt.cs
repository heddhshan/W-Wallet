//
//  本文件由代码工具自动生成。如非必要请不要修改。2021-02-01 10:46:50
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Share.DAL
{
    internal class TransactionReceipt
    {
        #region 对应的数据库表 TransactionReceipt
        public const string TableName = @"TransactionReceipt";
        #endregion

        #region  表 TransactionReceipt 的Insert操作
        public static void Insert(string conStr, Model.TransactionReceipt model, out int NewID)
        {
            NewID = 0;
            string sql = @"insert into TransactionReceipt (TransactionHash,CreateTime,UserMethod,UserFrom,UserRemark,TransactionIndex,GotReceipt,BlockHash,BlockNumber,CumulativeGasUsed,GasUsed,GasPrice,ContractAddress,Status,Logs,HasErrors,ResultTime,Canceled) values (@TransactionHash,@CreateTime,@UserMethod,@UserFrom,@UserRemark,@TransactionIndex,@GotReceipt,@BlockHash,@BlockNumber,@CumulativeGasUsed,@GasUsed,@GasPrice,@ContractAddress,@Status,@Logs,@HasErrors,@ResultTime,@Canceled); SELECT SCOPE_IDENTITY() AS NewID;";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@CreateTime", SqlDbType.DateTime, 8).Value = model.CreateTime;
            cm.Parameters.Add("@UserMethod", SqlDbType.NVarChar, 128).Value = model.UserMethod;
            cm.Parameters.Add("@UserFrom", SqlDbType.NVarChar, 43).Value = model.UserFrom;
            cm.Parameters.Add("@UserRemark", SqlDbType.NVarChar, 1024).Value = model.UserRemark;
            cm.Parameters.Add("@TransactionIndex", SqlDbType.BigInt, 8).Value = model.TransactionIndex;
            cm.Parameters.Add("@GotReceipt", SqlDbType.Bit, 1).Value = model.GotReceipt;
            cm.Parameters.Add("@BlockHash", SqlDbType.NVarChar, 66).Value = model.BlockHash;
            cm.Parameters.Add("@BlockNumber", SqlDbType.BigInt, 8).Value = model.BlockNumber;
            cm.Parameters.Add("@CumulativeGasUsed", SqlDbType.BigInt, 8).Value = model.CumulativeGasUsed;
            cm.Parameters.Add("@GasUsed", SqlDbType.BigInt, 8).Value = model.GasUsed;
            cm.Parameters.Add("@GasPrice", SqlDbType.Float, 8).Value = model.GasPrice;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@Status", SqlDbType.BigInt, 8).Value = model.Status;
            cm.Parameters.Add("@Logs", SqlDbType.NVarChar).Value = model.Logs;
            cm.Parameters.Add("@HasErrors", SqlDbType.Bit, 1).Value = model.HasErrors;
            cm.Parameters.Add("@ResultTime", SqlDbType.DateTime, 8).Value = model.ResultTime;
            cm.Parameters.Add("@Canceled", SqlDbType.Bit, 1).Value = model.Canceled;

            cn.Open();
            try
            {
                NewID = Convert.ToInt32(cm.ExecuteScalar());
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion

        #region  表 TransactionReceipt 的Delete操作
        public static int Delete(string conStr, System.Int32 Id)
        {
            string sql = @"Delete From TransactionReceipt Where Id = @Id";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = Id;
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

        #region  表 TransactionReceipt 的 Update 操作
        public static int Update(string conStr, Model.TransactionReceipt model)
        {
            string sql = @"Update TransactionReceipt Set TransactionHash = @TransactionHash, CreateTime = @CreateTime, UserMethod = @UserMethod, UserFrom = @UserFrom, UserRemark = @UserRemark, TransactionIndex = @TransactionIndex, GotReceipt = @GotReceipt, BlockHash = @BlockHash, BlockNumber = @BlockNumber, CumulativeGasUsed = @CumulativeGasUsed, GasUsed = @GasUsed, GasPrice = @GasPrice, ContractAddress = @ContractAddress, Status = @Status, Logs = @Logs, HasErrors = @HasErrors, ResultTime = @ResultTime, Canceled = @Canceled Where Id = @Id ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = model.Id;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@CreateTime", SqlDbType.DateTime, 8).Value = model.CreateTime;
            cm.Parameters.Add("@UserMethod", SqlDbType.NVarChar, 128).Value = model.UserMethod;
            cm.Parameters.Add("@UserFrom", SqlDbType.NVarChar, 43).Value = model.UserFrom;
            cm.Parameters.Add("@UserRemark", SqlDbType.NVarChar, 1024).Value = model.UserRemark;
            cm.Parameters.Add("@TransactionIndex", SqlDbType.BigInt, 8).Value = model.TransactionIndex;
            cm.Parameters.Add("@GotReceipt", SqlDbType.Bit, 1).Value = model.GotReceipt;
            cm.Parameters.Add("@BlockHash", SqlDbType.NVarChar, 66).Value = model.BlockHash;
            cm.Parameters.Add("@BlockNumber", SqlDbType.BigInt, 8).Value = model.BlockNumber;
            cm.Parameters.Add("@CumulativeGasUsed", SqlDbType.BigInt, 8).Value = model.CumulativeGasUsed;
            cm.Parameters.Add("@GasUsed", SqlDbType.BigInt, 8).Value = model.GasUsed;
            cm.Parameters.Add("@GasPrice", SqlDbType.Float, 8).Value = model.GasPrice;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@Status", SqlDbType.BigInt, 8).Value = model.Status;
            cm.Parameters.Add("@Logs", SqlDbType.NVarChar).Value = model.Logs;
            cm.Parameters.Add("@HasErrors", SqlDbType.Bit, 1).Value = model.HasErrors;
            cm.Parameters.Add("@ResultTime", SqlDbType.DateTime, 8).Value = model.ResultTime;
            cm.Parameters.Add("@Canceled", SqlDbType.Bit, 1).Value = model.Canceled;

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

        #region  表 TransactionReceipt 的GetModel操作
        public static Model.TransactionReceipt GetModel(string conStr, System.Int32 Id)
        {
            string sql = @"select * from TransactionReceipt Where Id = @Id ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = Id;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.TransactionReceipt.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 TransactionReceipt 的 Exist 操作 判断
        public static bool Exist(string conStr, System.Int32 Id)
        {
            string sql = @"Select Count(*) From TransactionReceipt Where Id = @Id ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@Id", SqlDbType.Int, 4).Value = Id;

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