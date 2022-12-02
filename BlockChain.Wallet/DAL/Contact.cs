//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 10:58:46
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Wallet.DAL
{
    internal class Contact
    {
        #region 对应的数据库表 Contact
        public const string TableName = @"Contact";
        #endregion

        #region  表 Contact 的Insert操作
        public static void Insert(string conStr, Model.Contact model)
        {
            string sql = @"insert into Contact (ContactName,ContactAddress,ContactRemark,CreateTime) values (@ContactName,@ContactAddress,@ContactRemark,@CreateTime)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContactName", SqlDbType.NVarChar, 64).Value = model.ContactName;
            cm.Parameters.Add("@ContactAddress", SqlDbType.NVarChar, 43).Value = model.ContactAddress;
            cm.Parameters.Add("@ContactRemark", SqlDbType.NVarChar, 2048).Value = model.ContactRemark;
            cm.Parameters.Add("@CreateTime", SqlDbType.DateTime, 8).Value = model.CreateTime;

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

        #region  表 Contact 的Delete操作
        public static int Delete(string conStr, System.String ContactAddress)
        {
            string sql = @"Delete From Contact Where ContactAddress = @ContactAddress";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContactAddress", SqlDbType.NVarChar, 43).Value = ContactAddress;
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

        #region  表 Contact 的 Update 操作
        public static int Update(string conStr, Model.Contact model)
        {
            string sql = @"Update Contact Set ContactName = @ContactName, ContactRemark = @ContactRemark, CreateTime = @CreateTime Where ContactAddress = @ContactAddress ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContactName", SqlDbType.NVarChar, 64).Value = model.ContactName;
            cm.Parameters.Add("@ContactAddress", SqlDbType.NVarChar, 43).Value = model.ContactAddress;
            cm.Parameters.Add("@ContactRemark", SqlDbType.NVarChar, 2048).Value = model.ContactRemark;
            cm.Parameters.Add("@CreateTime", SqlDbType.DateTime, 8).Value = model.CreateTime;

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

        #region  表 Contact 的GetModel操作
        public static Model.Contact GetModel(string conStr, System.String ContactAddress)
        {
            string sql = @"select * from Contact Where ContactAddress = @ContactAddress ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ContactAddress", SqlDbType.NVarChar, 43).Value = ContactAddress;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.Contact.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 Contact 的 Exist 操作 判断
        public static bool Exist(string conStr, System.String ContactAddress)
        {
            string sql = @"Select Count(*) From Contact Where ContactAddress = @ContactAddress ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ContactAddress", SqlDbType.NVarChar, 43).Value = ContactAddress;

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