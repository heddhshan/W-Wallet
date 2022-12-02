
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Wallet.DAL
{
    #region Add Time 2022/5/31 17:07:43

    internal class AddressBalance
    {
        #region 对应的数据库表 AddressBalance
        public const string TableName = @"AddressBalance";
        #endregion

        #region  表 AddressBalance 的Insert操作
        public static void Insert(string conStr, Model.AddressBalance model)
        {
            string sql = @"insert into AddressBalance (UserAddress,TokenAddress,Balance,UpdateBlockNumber,Balance_Text,UpdateDateTime) values (@UserAddress,@TokenAddress,@Balance,@UpdateBlockNumber,@Balance_Text,@UpdateDateTime)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 64).Value = model.UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 64).Value = model.TokenAddress;
            cm.Parameters.Add("@Balance", SqlDbType.Decimal, 17).Value = model.Balance;
            cm.Parameters.Add("@UpdateBlockNumber", SqlDbType.BigInt, 8).Value = model.UpdateBlockNumber;
            cm.Parameters.Add("@Balance_Text", SqlDbType.NVarChar, 80).Value = model.Balance_Text;
            cm.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime, 8).Value = model.UpdateDateTime;

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

        #region  表 AddressBalance 的Delete操作
        public static int Delete(string conStr, System.String UserAddress, System.String TokenAddress)
        {
            string sql = @"Delete From AddressBalance Where UserAddress = @UserAddress and TokenAddress = @TokenAddress";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 64).Value = UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 64).Value = TokenAddress;
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

        #region  表 AddressBalance 的 Update 操作
        public static int Update(string conStr, Model.AddressBalance model)
        {
            string sql = @"Update AddressBalance Set Balance = @Balance, UpdateBlockNumber = @UpdateBlockNumber, Balance_Text = @Balance_Text, UpdateDateTime = @UpdateDateTime Where UserAddress = @UserAddress And TokenAddress = @TokenAddress ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 64).Value = model.UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 64).Value = model.TokenAddress;
            cm.Parameters.Add("@Balance", SqlDbType.Decimal, 17).Value = model.Balance;
            cm.Parameters.Add("@UpdateBlockNumber", SqlDbType.BigInt, 8).Value = model.UpdateBlockNumber;
            cm.Parameters.Add("@Balance_Text", SqlDbType.NVarChar, 80).Value = model.Balance_Text;
            cm.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime, 8).Value = model.UpdateDateTime;

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

        #region  表 AddressBalance 的GetModel操作
        public static Model.AddressBalance GetModel(string conStr, System.String UserAddress, System.String TokenAddress)
        {
            string sql = @"select * from AddressBalance Where UserAddress = @UserAddress and TokenAddress = @TokenAddress ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 64).Value = UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 64).Value = TokenAddress;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.AddressBalance.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 AddressBalance 的 Exist 操作 判断
        public static bool Exist(string conStr, System.String UserAddress, System.String TokenAddress)
        {
            string sql = @"Select Count(*) From AddressBalance Where UserAddress = @UserAddress and TokenAddress = @TokenAddress ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 64).Value = UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 64).Value = TokenAddress;

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