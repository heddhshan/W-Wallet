//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/9/24 16:29:12
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.OfflineWallet.DAL
{
internal class HD_Address
{
#region 对应的数据库表 HD_Address
public const string TableName = @"HD_Address";
#endregion

#region  表 HD_Address 的Insert操作
public static void Insert(string conStr, Model.HD_Address model)
{
    string sql = @"insert into HD_Address (MneId,MneSecondSalt,AddressIndex,AddressAlias,Address,IsTxAddress,HasPrivatekey) values (@MneId,@MneSecondSalt,@AddressIndex,@AddressAlias,@Address,@IsTxAddress,@HasPrivatekey)";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier, 16).Value = model.MneId;
    cm.Parameters.Add("@MneSecondSalt", SqlDbType.NVarChar, 64).Value = model.MneSecondSalt;
    cm.Parameters.Add("@AddressIndex", SqlDbType.Int, 4).Value = model.AddressIndex;
    cm.Parameters.Add("@AddressAlias", SqlDbType.NVarChar, 64).Value = model.AddressAlias;
    cm.Parameters.Add("@Address", SqlDbType.NVarChar, 43).Value = model.Address;
    cm.Parameters.Add("@IsTxAddress", SqlDbType.Bit, 1).Value = model.IsTxAddress;
    cm.Parameters.Add("@HasPrivatekey", SqlDbType.Bit, 1).Value = model.HasPrivatekey;

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

#region  表 HD_Address 的Delete操作
public static int Delete(string conStr, System.String Address)
{
    string sql = @"Delete From HD_Address Where Address = @Address";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

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

#region  表 HD_Address 的 Update 操作
public static int Update(string conStr, Model.HD_Address model)
{
    string sql = @"Update HD_Address Set MneId = @MneId, MneSecondSalt = @MneSecondSalt, AddressIndex = @AddressIndex, AddressAlias = @AddressAlias, IsTxAddress = @IsTxAddress, HasPrivatekey = @HasPrivatekey Where Address = @Address ";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier, 16).Value = model.MneId;
    cm.Parameters.Add("@MneSecondSalt", SqlDbType.NVarChar, 64).Value = model.MneSecondSalt;
    cm.Parameters.Add("@AddressIndex", SqlDbType.Int, 4).Value = model.AddressIndex;
    cm.Parameters.Add("@AddressAlias", SqlDbType.NVarChar, 64).Value = model.AddressAlias;
    cm.Parameters.Add("@Address", SqlDbType.NVarChar, 43).Value = model.Address;
    cm.Parameters.Add("@IsTxAddress", SqlDbType.Bit, 1).Value = model.IsTxAddress;
    cm.Parameters.Add("@HasPrivatekey", SqlDbType.Bit, 1).Value = model.HasPrivatekey;

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

#region  表 HD_Address 的GetModel操作
public static Model.HD_Address GetModel(string conStr, System.String Address)
{
    string sql = @"select * from HD_Address Where Address = @Address ";

    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;
    cm.Parameters.Add("@Address", SqlDbType.NVarChar, 43).Value = Address;

    SqlDataAdapter da = new SqlDataAdapter();
    da.SelectCommand = cm;
    System.Data.DataSet ds = new System.Data.DataSet();
    da.Fill(ds);
    if (ds.Tables[0].Rows.Count == 1)
    {
        return Model.HD_Address.DataRow2Object(ds.Tables[0].Rows[0]);
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

#region  表 HD_Address 的 Exist 操作 判断
public static bool Exist(string conStr, System.String Address)
{
    string sql = @"Select Count(*) From HD_Address Where Address = @Address ";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;
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