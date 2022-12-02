
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Share.DAL
{

    #region Add Time 2022/6/26 12:19:02

    internal class ContEventBlockNum
    {
        #region 对应的数据库表 ContEventBlockNum
        public const string TableName = @"ContEventBlockNum";
        #endregion

        #region  表 ContEventBlockNum 的Insert操作
        public static void Insert(string conStr, Model.ContEventBlockNum model)
        {
            string sql = @"insert into ContEventBlockNum (ContractAddress,EventName,LastBlockNumber) values (@ContractAddress,@EventName,@LastBlockNumber)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@EventName", SqlDbType.NVarChar, 256).Value = model.EventName;
            cm.Parameters.Add("@LastBlockNumber", SqlDbType.BigInt, 8).Value = model.LastBlockNumber;

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

        #region  表 ContEventBlockNum 的Delete操作
        public static int Delete(string conStr, System.String ContractAddress, System.String EventName)
        {
            string sql = @"Delete From ContEventBlockNum Where ContractAddress = @ContractAddress and EventName = @EventName";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
            cm.Parameters.Add("@EventName", SqlDbType.NVarChar, 256).Value = EventName;
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

        #region  表 ContEventBlockNum 的 Update 操作
        public static int Update(string conStr, Model.ContEventBlockNum model)
        {
            string sql = @"Update ContEventBlockNum Set LastBlockNumber = @LastBlockNumber Where ContractAddress = @ContractAddress And EventName = @EventName ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@EventName", SqlDbType.NVarChar, 256).Value = model.EventName;
            cm.Parameters.Add("@LastBlockNumber", SqlDbType.BigInt, 8).Value = model.LastBlockNumber;

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

        #region  表 ContEventBlockNum 的GetModel操作
        public static Model.ContEventBlockNum GetModel(string conStr, System.String ContractAddress, System.String EventName)
        {
            string sql = @"select * from ContEventBlockNum Where ContractAddress = @ContractAddress and EventName = @EventName ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
            cm.Parameters.Add("@EventName", SqlDbType.NVarChar, 256).Value = EventName;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.ContEventBlockNum.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 ContEventBlockNum 的 Exist 操作 判断
        public static bool Exist(string conStr, System.String ContractAddress, System.String EventName)
        {
            string sql = @"Select Count(*) From ContEventBlockNum Where ContractAddress = @ContractAddress and EventName = @EventName ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
            cm.Parameters.Add("@EventName", SqlDbType.NVarChar, 256).Value = EventName;

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