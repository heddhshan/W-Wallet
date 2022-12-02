//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 10:45:30
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Share.DAL
{
    internal class T_OriginalText
    {
        #region 对应的数据库表 T_OriginalText
        public const string TableName = @"T_OriginalText";
        #endregion

        #region  表 T_OriginalText 的Insert操作
        public static void Insert(string conStr, Model.T_OriginalText model)
        {
            string sql = @"insert into T_OriginalText (OriginalHash,OriginalText) values (@OriginalHash,@OriginalText)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@OriginalHash", SqlDbType.NVarChar, 64).Value = model.OriginalHash;
            cm.Parameters.Add("@OriginalText", SqlDbType.NVarChar, 4000).Value = model.OriginalText;

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

        #region  表 T_OriginalText 的Delete操作
        public static int Delete(string conStr, System.String OriginalHash)
        {
            string sql = @"Delete From T_OriginalText Where OriginalHash = @OriginalHash";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@OriginalHash", SqlDbType.NVarChar, 64).Value = OriginalHash;
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

        #region  表 T_OriginalText 的 Update 操作
        public static int Update(string conStr, Model.T_OriginalText model)
        {
            string sql = @"Update T_OriginalText Set OriginalText = @OriginalText Where OriginalHash = @OriginalHash ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@OriginalHash", SqlDbType.NVarChar, 64).Value = model.OriginalHash;
            cm.Parameters.Add("@OriginalText", SqlDbType.NVarChar, 4000).Value = model.OriginalText;

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

        #region  表 T_OriginalText 的GetModel操作
        public static Model.T_OriginalText GetModel(string conStr, System.String OriginalHash)
        {
            string sql = @"select * from T_OriginalText Where OriginalHash = @OriginalHash ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@OriginalHash", SqlDbType.NVarChar, 64).Value = OriginalHash;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.T_OriginalText.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 T_OriginalText 的 Exist 操作 判断
        public static bool Exist(string conStr, System.String OriginalHash)
        {
            string sql = @"Select Count(*) From T_OriginalText Where OriginalHash = @OriginalHash ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@OriginalHash", SqlDbType.NVarChar, 64).Value = OriginalHash;

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