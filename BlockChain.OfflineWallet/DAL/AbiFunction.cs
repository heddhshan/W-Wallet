//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/9/23 21:33:06
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.OfflineWallet.DAL
{
internal class AbiFunction
{
#region 对应的数据库表 AbiFunction
public const string TableName = @"AbiFunction";
#endregion

#region  表 AbiFunction 的Insert操作
public static void Insert(string conStr, Model.AbiFunction model)
{
    string sql = @"insert into AbiFunction (FunId,FunctionFullName,Remark,FunctionFullNameHash,FunctionFullNameHash4,IsSysDefine,IsTestOk,IsEthTransfer) values (@FunId,@FunctionFullName,@Remark,@FunctionFullNameHash,@FunctionFullNameHash4,@IsSysDefine,@IsTestOk,@IsEthTransfer)";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = model.FunId;
    cm.Parameters.Add("@FunctionFullName", SqlDbType.NVarChar, 256).Value = model.FunctionFullName;
    cm.Parameters.Add("@Remark", SqlDbType.NVarChar, 64).Value = model.Remark;
    cm.Parameters.Add("@FunctionFullNameHash", SqlDbType.NVarChar, 128).Value = model.FunctionFullNameHash;
    cm.Parameters.Add("@FunctionFullNameHash4", SqlDbType.NVarChar, 16).Value = model.FunctionFullNameHash4;
    cm.Parameters.Add("@IsSysDefine", SqlDbType.Bit, 1).Value = model.IsSysDefine;
    cm.Parameters.Add("@IsTestOk", SqlDbType.Bit, 1).Value = model.IsTestOk;
    cm.Parameters.Add("@IsEthTransfer", SqlDbType.Bit, 1).Value = model.IsEthTransfer;

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

#region  表 AbiFunction 的Delete操作
public static int Delete(string conStr, System.Guid FunId)
{
    string sql = @"Delete From AbiFunction Where FunId = @FunId";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = FunId;
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

#region  表 AbiFunction 的 Update 操作
public static int Update(string conStr, Model.AbiFunction model)
{
    string sql = @"Update AbiFunction Set FunctionFullName = @FunctionFullName, Remark = @Remark, FunctionFullNameHash = @FunctionFullNameHash, FunctionFullNameHash4 = @FunctionFullNameHash4, IsSysDefine = @IsSysDefine, IsTestOk = @IsTestOk, IsEthTransfer = @IsEthTransfer Where FunId = @FunId ";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = model.FunId;
    cm.Parameters.Add("@FunctionFullName", SqlDbType.NVarChar, 256).Value = model.FunctionFullName;
    cm.Parameters.Add("@Remark", SqlDbType.NVarChar, 64).Value = model.Remark;
    cm.Parameters.Add("@FunctionFullNameHash", SqlDbType.NVarChar, 128).Value = model.FunctionFullNameHash;
    cm.Parameters.Add("@FunctionFullNameHash4", SqlDbType.NVarChar, 16).Value = model.FunctionFullNameHash4;
    cm.Parameters.Add("@IsSysDefine", SqlDbType.Bit, 1).Value = model.IsSysDefine;
    cm.Parameters.Add("@IsTestOk", SqlDbType.Bit, 1).Value = model.IsTestOk;
    cm.Parameters.Add("@IsEthTransfer", SqlDbType.Bit, 1).Value = model.IsEthTransfer;

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

#region  表 AbiFunction 的GetModel操作
public static Model.AbiFunction GetModel(string conStr, System.Guid FunId)
{
    string sql = @"select * from AbiFunction Where FunId = @FunId ";

    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;
    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = FunId;

    SqlDataAdapter da = new SqlDataAdapter();
    da.SelectCommand = cm;
    System.Data.DataSet ds = new System.Data.DataSet();
    da.Fill(ds);
    if (ds.Tables[0].Rows.Count == 1)
    {
        return Model.AbiFunction.DataRow2Object(ds.Tables[0].Rows[0]);
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

#region  表 AbiFunction 的 Exist 操作 判断
public static bool Exist(string conStr, System.Guid FunId)
{
    string sql = @"Select Count(*) From AbiFunction Where FunId = @FunId ";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;
    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = FunId;

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