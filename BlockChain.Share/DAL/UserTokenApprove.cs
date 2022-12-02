//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/9/9 19:03:55
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Share.DAL
{
    internal class UserTokenApprove
    {
        #region 对应的数据库表 UserTokenApprove
        public const string TableName = @"UserTokenApprove";
        #endregion

        #region  表 UserTokenApprove 的Insert操作
        public static void Insert(string conStr, Model.UserTokenApprove model)
        {
            string sql = @"insert into UserTokenApprove (ChainId,TransactionHash,UserAddress,TokenAddress,SpenderAddress,TokenDecimals,TokenSymbol,CurrentAmount,IsDivToken,DivTokenIsWithdrawable) values (@ChainId,@TransactionHash,@UserAddress,@TokenAddress,@SpenderAddress,@TokenDecimals,@TokenSymbol,@CurrentAmount,@IsDivToken,@DivTokenIsWithdrawable)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = model.ChainId;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 43).Value = model.UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 43).Value = model.TokenAddress;
            cm.Parameters.Add("@SpenderAddress", SqlDbType.NVarChar, 43).Value = model.SpenderAddress;
            cm.Parameters.Add("@TokenDecimals", SqlDbType.Int, 4).Value = model.TokenDecimals;
            cm.Parameters.Add("@TokenSymbol", SqlDbType.NVarChar, 64).Value = model.TokenSymbol;
            cm.Parameters.Add("@CurrentAmount", SqlDbType.Float, 8).Value = model.CurrentAmount;
            cm.Parameters.Add("@IsDivToken", SqlDbType.Bit, 1).Value = model.IsDivToken;
            cm.Parameters.Add("@DivTokenIsWithdrawable", SqlDbType.Bit, 1).Value = model.DivTokenIsWithdrawable;

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

        #region  表 UserTokenApprove 的Delete操作
        public static int Delete(string conStr, System.Int32 ChainId, System.String UserAddress, System.String TokenAddress, System.String SpenderAddress)
        {
            string sql = @"Delete From UserTokenApprove Where ChainId = @ChainId and UserAddress = @UserAddress and TokenAddress = @TokenAddress and SpenderAddress = @SpenderAddress";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;
            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 43).Value = UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 43).Value = TokenAddress;
            cm.Parameters.Add("@SpenderAddress", SqlDbType.NVarChar, 43).Value = SpenderAddress;
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

        #region  表 UserTokenApprove 的 Update 操作
        public static int Update(string conStr, Model.UserTokenApprove model)
        {
            string sql = @"Update UserTokenApprove Set TransactionHash = @TransactionHash, TokenDecimals = @TokenDecimals, TokenSymbol = @TokenSymbol, CurrentAmount = @CurrentAmount, IsDivToken = @IsDivToken, DivTokenIsWithdrawable = @DivTokenIsWithdrawable Where ChainId = @ChainId And UserAddress = @UserAddress And TokenAddress = @TokenAddress And SpenderAddress = @SpenderAddress ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = model.ChainId;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 43).Value = model.UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 43).Value = model.TokenAddress;
            cm.Parameters.Add("@SpenderAddress", SqlDbType.NVarChar, 43).Value = model.SpenderAddress;
            cm.Parameters.Add("@TokenDecimals", SqlDbType.Int, 4).Value = model.TokenDecimals;
            cm.Parameters.Add("@TokenSymbol", SqlDbType.NVarChar, 64).Value = model.TokenSymbol;
            cm.Parameters.Add("@CurrentAmount", SqlDbType.Float, 8).Value = model.CurrentAmount;
            cm.Parameters.Add("@IsDivToken", SqlDbType.Bit, 1).Value = model.IsDivToken;
            cm.Parameters.Add("@DivTokenIsWithdrawable", SqlDbType.Bit, 1).Value = model.DivTokenIsWithdrawable;

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

        #region  表 UserTokenApprove 的GetModel操作
        public static Model.UserTokenApprove GetModel(string conStr, System.Int32 ChainId, System.String UserAddress, System.String TokenAddress, System.String SpenderAddress)
        {
            string sql = @"select * from UserTokenApprove Where ChainId = @ChainId and UserAddress = @UserAddress and TokenAddress = @TokenAddress and SpenderAddress = @SpenderAddress ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;
            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 43).Value = UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 43).Value = TokenAddress;
            cm.Parameters.Add("@SpenderAddress", SqlDbType.NVarChar, 43).Value = SpenderAddress;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.UserTokenApprove.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 UserTokenApprove 的 Exist 操作 判断
        public static bool Exist(string conStr, System.Int32 ChainId, System.String UserAddress, System.String TokenAddress, System.String SpenderAddress)
        {
            string sql = @"Select Count(*) From UserTokenApprove Where ChainId = @ChainId and UserAddress = @UserAddress and TokenAddress = @TokenAddress and SpenderAddress = @SpenderAddress ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;
            cm.Parameters.Add("@UserAddress", SqlDbType.NVarChar, 43).Value = UserAddress;
            cm.Parameters.Add("@TokenAddress", SqlDbType.NVarChar, 43).Value = TokenAddress;
            cm.Parameters.Add("@SpenderAddress", SqlDbType.NVarChar, 43).Value = SpenderAddress;

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