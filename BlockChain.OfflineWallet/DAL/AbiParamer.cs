//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/9/23 21:33:07
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.OfflineWallet.DAL
{
internal class AbiParamer
{
#region 对应的数据库表 AbiParamer
public const string TableName = @"AbiParamer";
#endregion

#region  表 AbiParamer 的Insert操作
public static void Insert(string conStr, Model.AbiParamer model)
{
    string sql = @"insert into AbiParamer (FunId,ParamerType,ParamerName,ParamerOrder) values (@FunId,@ParamerType,@ParamerName,@ParamerOrder)";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = model.FunId;
    cm.Parameters.Add("@ParamerType", SqlDbType.NVarChar, 128).Value = model.ParamerType;
    cm.Parameters.Add("@ParamerName", SqlDbType.NVarChar, 128).Value = model.ParamerName;
    cm.Parameters.Add("@ParamerOrder", SqlDbType.Int, 4).Value = model.ParamerOrder;

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

#region  表 AbiParamer 的Delete操作
public static int Delete(string conStr, System.Guid FunId,System.Int32 ParamerOrder)
{
    string sql = @"Delete From AbiParamer Where FunId = @FunId and ParamerOrder = @ParamerOrder";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = FunId;
    cm.Parameters.Add("@ParamerOrder", SqlDbType.Int, 4).Value = ParamerOrder;
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

#region  表 AbiParamer 的 Update 操作
public static int Update(string conStr, Model.AbiParamer model)
{
    string sql = @"Update AbiParamer Set ParamerType = @ParamerType, ParamerName = @ParamerName Where FunId = @FunId And ParamerOrder = @ParamerOrder ";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = model.FunId;
    cm.Parameters.Add("@ParamerType", SqlDbType.NVarChar, 128).Value = model.ParamerType;
    cm.Parameters.Add("@ParamerName", SqlDbType.NVarChar, 128).Value = model.ParamerName;
    cm.Parameters.Add("@ParamerOrder", SqlDbType.Int, 4).Value = model.ParamerOrder;

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

#region  表 AbiParamer 的GetModel操作
public static Model.AbiParamer GetModel(string conStr, System.Guid FunId, System.Int32 ParamerOrder)
{
    string sql = @"select * from AbiParamer Where FunId = @FunId and ParamerOrder = @ParamerOrder ";

    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;
    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = FunId;
    cm.Parameters.Add("@ParamerOrder", SqlDbType.Int, 4).Value = ParamerOrder;

    SqlDataAdapter da = new SqlDataAdapter();
    da.SelectCommand = cm;
    System.Data.DataSet ds = new System.Data.DataSet();
    da.Fill(ds);
    if (ds.Tables[0].Rows.Count == 1)
    {
        return Model.AbiParamer.DataRow2Object(ds.Tables[0].Rows[0]);
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

#region  表 AbiParamer 的 Exist 操作 判断
public static bool Exist(string conStr, System.Guid FunId, System.Int32 ParamerOrder)
{
    string sql = @"Select Count(*) From AbiParamer Where FunId = @FunId and ParamerOrder = @ParamerOrder ";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;
    cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier, 16).Value = FunId;
    cm.Parameters.Add("@ParamerOrder", SqlDbType.Int, 4).Value = ParamerOrder;

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