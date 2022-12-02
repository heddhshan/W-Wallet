using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Share.DAL
{
    internal class Token
    {
        #region 对应的数据库表 Token
        public const string TableName = @"Token";
        #endregion

        #region  表 Token 的Insert操作
        public static void Insert(string conStr, Model.Token model)
        {
            string sql = @"insert into Token (ChainId,Name,Address,Decimals,Symbol,ImagePath,IsPricingToken,PricingTokenAddress,PricingTokenPrice,PricingTokenPriceUpdateTime,PricingIsFixed) values (@ChainId,@Name,@Address,@Decimals,@Symbol,@ImagePath,@IsPricingToken,@PricingTokenAddress,@PricingTokenPrice,@PricingTokenPriceUpdateTime,@PricingIsFixed)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = model.ChainId;
            cm.Parameters.Add("@Name", SqlDbType.NVarChar, 64).Value = model.Name;
            cm.Parameters.Add("@Address", SqlDbType.NVarChar, 43).Value = model.Address;
            cm.Parameters.Add("@Decimals", SqlDbType.Int, 4).Value = model.Decimals;
            cm.Parameters.Add("@Symbol", SqlDbType.NVarChar, 64).Value = model.Symbol;
            cm.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 2048).Value = model.ImagePath;
            cm.Parameters.Add("@IsPricingToken", SqlDbType.Bit, 1).Value = model.IsPricingToken;
            cm.Parameters.Add("@PricingTokenAddress", SqlDbType.NVarChar, 43).Value = model.PricingTokenAddress;
            cm.Parameters.Add("@PricingTokenPrice", SqlDbType.Float, 8).Value = model.PricingTokenPrice;
            cm.Parameters.Add("@PricingTokenPriceUpdateTime", SqlDbType.DateTime, 8).Value = model.PricingTokenPriceUpdateTime;
            cm.Parameters.Add("@PricingIsFixed", SqlDbType.Bit, 1).Value = model.PricingIsFixed;

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

        #region  表 Token 的Delete操作
        public static int Delete(string conStr, System.Int32 ChainId, System.String Address)
        {
            string sql = @"Delete From Token Where ChainId = @ChainId and Address = @Address";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;
            cm.Parameters.Add("@Address", SqlDbType.NVarChar, 43).Value = Address;
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

        #region  表 Token 的 Update 操作
        public static int Update(string conStr, Model.Token model)
        {
            string sql = @"Update Token Set Name = @Name, Decimals = @Decimals, Symbol = @Symbol, ImagePath = @ImagePath, IsPricingToken = @IsPricingToken, PricingTokenAddress = @PricingTokenAddress, PricingTokenPrice = @PricingTokenPrice, PricingTokenPriceUpdateTime = @PricingTokenPriceUpdateTime, PricingIsFixed = @PricingIsFixed Where ChainId = @ChainId And Address = @Address ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = model.ChainId;
            cm.Parameters.Add("@Name", SqlDbType.NVarChar, 64).Value = model.Name;
            cm.Parameters.Add("@Address", SqlDbType.NVarChar, 43).Value = model.Address;
            cm.Parameters.Add("@Decimals", SqlDbType.Int, 4).Value = model.Decimals;
            cm.Parameters.Add("@Symbol", SqlDbType.NVarChar, 64).Value = model.Symbol;
            cm.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 2048).Value = model.ImagePath;
            cm.Parameters.Add("@IsPricingToken", SqlDbType.Bit, 1).Value = model.IsPricingToken;
            cm.Parameters.Add("@PricingTokenAddress", SqlDbType.NVarChar, 43).Value = model.PricingTokenAddress;
            cm.Parameters.Add("@PricingTokenPrice", SqlDbType.Float, 8).Value = model.PricingTokenPrice;
            cm.Parameters.Add("@PricingTokenPriceUpdateTime", SqlDbType.DateTime, 8).Value = model.PricingTokenPriceUpdateTime;
            cm.Parameters.Add("@PricingIsFixed", SqlDbType.Bit, 1).Value = model.PricingIsFixed;

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

        #region  表 Token 的GetModel操作
        public static Model.Token GetModel(string conStr, System.Int32 ChainId, System.String Address)
        {
            string sql = @"select * from Token Where ChainId = @ChainId and Address = @Address ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;
            cm.Parameters.Add("@Address", SqlDbType.NVarChar, 43).Value = Address;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.Token.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 Token 的 Exist 操作 判断
        public static bool Exist(string conStr, System.Int32 ChainId, System.String Address)
        {
            string sql = @"Select Count(*) From Token Where ChainId = @ChainId and Address = @Address ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;
            cm.Parameters.Add("@Address", SqlDbType.NVarChar, 43).Value = Address;

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