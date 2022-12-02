
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Share.DAL
{
    internal class T_Language
    {
        #region 对应的数据库表 T_Language
        public const string TableName = @"T_Language";
        #endregion

        #region  表 T_Language 的Insert操作
        public static void Insert(string conStr, Model.T_Language model)
        {
            string sql = @"insert into T_Language (LCID,CultureInfoName,TwoLetterISOLanguageName,ThreeLetterISOLanguageName,ThreeLetterWindowsLanguageName,NativeName,DisplayName,EnglishName,IsSelected,ItemsNumber) values (@LCID,@CultureInfoName,@TwoLetterISOLanguageName,@ThreeLetterISOLanguageName,@ThreeLetterWindowsLanguageName,@NativeName,@DisplayName,@EnglishName,@IsSelected,@ItemsNumber)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@LCID", SqlDbType.Int, 4).Value = model.LCID;
            cm.Parameters.Add("@CultureInfoName", SqlDbType.NVarChar, 32).Value = model.CultureInfoName;
            cm.Parameters.Add("@TwoLetterISOLanguageName", SqlDbType.NVarChar, 8).Value = model.TwoLetterISOLanguageName;
            cm.Parameters.Add("@ThreeLetterISOLanguageName", SqlDbType.NVarChar, 8).Value = model.ThreeLetterISOLanguageName;
            cm.Parameters.Add("@ThreeLetterWindowsLanguageName", SqlDbType.NVarChar, 8).Value = model.ThreeLetterWindowsLanguageName;
            cm.Parameters.Add("@NativeName", SqlDbType.NVarChar, 64).Value = model.NativeName;
            cm.Parameters.Add("@DisplayName", SqlDbType.NVarChar, 64).Value = model.DisplayName;
            cm.Parameters.Add("@EnglishName", SqlDbType.NVarChar, 64).Value = model.EnglishName;
            cm.Parameters.Add("@IsSelected", SqlDbType.Bit, 1).Value = model.IsSelected;
            cm.Parameters.Add("@ItemsNumber", SqlDbType.Int, 4).Value = model.ItemsNumber;

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

        #region  表 T_Language 的Delete操作
        public static int Delete(string conStr, System.String CultureInfoName)
        {
            string sql = @"Delete From T_Language Where CultureInfoName = @CultureInfoName";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@CultureInfoName", SqlDbType.NVarChar, 32).Value = CultureInfoName;
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

        #region  表 T_Language 的 Update 操作
        public static int Update(string conStr, Model.T_Language model)
        {
            string sql = @"Update T_Language Set LCID = @LCID, TwoLetterISOLanguageName = @TwoLetterISOLanguageName, ThreeLetterISOLanguageName = @ThreeLetterISOLanguageName, ThreeLetterWindowsLanguageName = @ThreeLetterWindowsLanguageName, NativeName = @NativeName, DisplayName = @DisplayName, EnglishName = @EnglishName, IsSelected = @IsSelected, ItemsNumber = @ItemsNumber Where CultureInfoName = @CultureInfoName ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@LCID", SqlDbType.Int, 4).Value = model.LCID;
            cm.Parameters.Add("@CultureInfoName", SqlDbType.NVarChar, 32).Value = model.CultureInfoName;
            cm.Parameters.Add("@TwoLetterISOLanguageName", SqlDbType.NVarChar, 8).Value = model.TwoLetterISOLanguageName;
            cm.Parameters.Add("@ThreeLetterISOLanguageName", SqlDbType.NVarChar, 8).Value = model.ThreeLetterISOLanguageName;
            cm.Parameters.Add("@ThreeLetterWindowsLanguageName", SqlDbType.NVarChar, 8).Value = model.ThreeLetterWindowsLanguageName;
            cm.Parameters.Add("@NativeName", SqlDbType.NVarChar, 64).Value = model.NativeName;
            cm.Parameters.Add("@DisplayName", SqlDbType.NVarChar, 64).Value = model.DisplayName;
            cm.Parameters.Add("@EnglishName", SqlDbType.NVarChar, 64).Value = model.EnglishName;
            cm.Parameters.Add("@IsSelected", SqlDbType.Bit, 1).Value = model.IsSelected;
            cm.Parameters.Add("@ItemsNumber", SqlDbType.Int, 4).Value = model.ItemsNumber;

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

        #region  表 T_Language 的GetModel操作
        public static Model.T_Language GetModel(string conStr, System.String CultureInfoName)
        {
            string sql = @"select * from T_Language Where CultureInfoName = @CultureInfoName ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@CultureInfoName", SqlDbType.NVarChar, 32).Value = CultureInfoName;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.T_Language.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 T_Language 的 Exist 操作 判断
        public static bool Exist(string conStr, System.String CultureInfoName)
        {
            string sql = @"Select Count(*) From T_Language Where CultureInfoName = @CultureInfoName ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@CultureInfoName", SqlDbType.NVarChar, 32).Value = CultureInfoName;

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