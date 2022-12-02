//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 10:45:31
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Wallet.DAL
{
    public class WalletHelper_OnDeployContract_Local
    {
        #region 对应的数据库表 WalletHelper_OnDeployContract_Local
        public const string TableName = @"WalletHelper_OnDeployContract_Local";
        #endregion

        #region  表 WalletHelper_OnDeployContract_Local 的Insert操作
        public static void Insert(string conStr, Model.WalletHelper_OnDeployContract_Local model)
        {
            string sql = @"insert into WalletHelper_OnDeployContract_Local (ContractAddress,TransactionHash,ChainId,_bytecode,LocalRemark) values (@ContractAddress,@TransactionHash,@ChainId,@_bytecode,@LocalRemark)";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = model.ChainId;
            cm.Parameters.Add("@_bytecode", SqlDbType.NVarChar).Value = model._bytecode;
            cm.Parameters.Add("@LocalRemark", SqlDbType.NVarChar, 128).Value = model.LocalRemark;

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

        #region  表 WalletHelper_OnDeployContract_Local 的Delete操作
        public static int Delete(string conStr, System.String ContractAddress, System.String TransactionHash, System.Int32 ChainId)
        {
            string sql = @"Delete From WalletHelper_OnDeployContract_Local Where ContractAddress = @ContractAddress and TransactionHash = @TransactionHash and ChainId = @ChainId";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = TransactionHash;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;
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

        #region  表 WalletHelper_OnDeployContract_Local 的 Update 操作
        public static int Update(string conStr, Model.WalletHelper_OnDeployContract_Local model)
        {
            string sql = @"Update WalletHelper_OnDeployContract_Local Set _bytecode = @_bytecode, LocalRemark = @LocalRemark Where ContractAddress = @ContractAddress And TransactionHash = @TransactionHash And ChainId = @ChainId ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = model.ContractAddress;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = model.TransactionHash;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = model.ChainId;
            cm.Parameters.Add("@_bytecode", SqlDbType.NVarChar).Value = model._bytecode;
            cm.Parameters.Add("@LocalRemark", SqlDbType.NVarChar, 128).Value = model.LocalRemark;

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

        #region  表 WalletHelper_OnDeployContract_Local 的GetModel操作
        public static Model.WalletHelper_OnDeployContract_Local GetModel(string conStr, System.String ContractAddress, System.String TransactionHash, System.Int32 ChainId)
        {
            string sql = @"select * from WalletHelper_OnDeployContract_Local Where ContractAddress = @ContractAddress and TransactionHash = @TransactionHash and ChainId = @ChainId ";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = TransactionHash;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.WalletHelper_OnDeployContract_Local.DataRow2Object(ds.Tables[0].Rows[0]);
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

        #region  表 WalletHelper_OnDeployContract_Local 的 Exist 操作 判断
        public static bool Exist(string conStr, System.String ContractAddress, System.String TransactionHash, System.Int32 ChainId)
        {
            string sql = @"Select Count(*) From WalletHelper_OnDeployContract_Local Where ContractAddress = @ContractAddress and TransactionHash = @TransactionHash and ChainId = @ChainId ";
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ContractAddress", SqlDbType.NVarChar, 43).Value = ContractAddress;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar, 66).Value = TransactionHash;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = ChainId;

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