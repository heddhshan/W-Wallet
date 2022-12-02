//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/9/24 16:30:15
//
using System;
using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Wallet.DAL
{
internal class HD_Mnemonic
{
#region 对应的数据库表 HD_Mnemonic
public const string TableName = @"HD_Mnemonic";
#endregion

#region  表 HD_Mnemonic 的Insert操作
public static void Insert(string conStr, Model.HD_Mnemonic model)
{
    string sql = @"insert into HD_Mnemonic (MneId,MneAlias,MneEncrypted,EncryptedTimes,MneHash,WordCount,MnePath,Salt,UserPasswordHash,UserPasswordTip,MneFirstSalt,MneSource,IsBackUp,CreateTime,HasPrivatekey) values (@MneId,@MneAlias,@MneEncrypted,@EncryptedTimes,@MneHash,@WordCount,@MnePath,@Salt,@UserPasswordHash,@UserPasswordTip,@MneFirstSalt,@MneSource,@IsBackUp,@CreateTime,@HasPrivatekey)";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier, 16).Value = model.MneId;
    cm.Parameters.Add("@MneAlias", SqlDbType.NVarChar, 64).Value = model.MneAlias;
    cm.Parameters.Add("@MneEncrypted", SqlDbType.NVarChar, 1024).Value = model.MneEncrypted;
    cm.Parameters.Add("@EncryptedTimes", SqlDbType.Int, 4).Value = model.EncryptedTimes;
    cm.Parameters.Add("@MneHash", SqlDbType.NVarChar, 64).Value = model.MneHash;
    cm.Parameters.Add("@WordCount", SqlDbType.Int, 4).Value = model.WordCount;
    cm.Parameters.Add("@MnePath", SqlDbType.NVarChar, 32).Value = model.MnePath;
    cm.Parameters.Add("@Salt", SqlDbType.NVarChar, 64).Value = model.Salt;
    cm.Parameters.Add("@UserPasswordHash", SqlDbType.NVarChar, 64).Value = model.UserPasswordHash;
    cm.Parameters.Add("@UserPasswordTip", SqlDbType.NVarChar, 64).Value = model.UserPasswordTip;
    cm.Parameters.Add("@MneFirstSalt", SqlDbType.NVarChar, 128).Value = model.MneFirstSalt;
    cm.Parameters.Add("@MneSource", SqlDbType.Int, 4).Value = model.MneSource;
    cm.Parameters.Add("@IsBackUp", SqlDbType.Bit, 1).Value = model.IsBackUp;
    cm.Parameters.Add("@CreateTime", SqlDbType.DateTime, 8).Value = model.CreateTime;
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

#region  表 HD_Mnemonic 的Delete操作
public static int Delete(string conStr, System.Guid MneId)
{
    string sql = @"Delete From HD_Mnemonic Where MneId = @MneId";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier, 16).Value = MneId;
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

#region  表 HD_Mnemonic 的 Update 操作
public static int Update(string conStr, Model.HD_Mnemonic model)
{
    string sql = @"Update HD_Mnemonic Set MneAlias = @MneAlias, MneEncrypted = @MneEncrypted, EncryptedTimes = @EncryptedTimes, MneHash = @MneHash, WordCount = @WordCount, MnePath = @MnePath, Salt = @Salt, UserPasswordHash = @UserPasswordHash, UserPasswordTip = @UserPasswordTip, MneFirstSalt = @MneFirstSalt, MneSource = @MneSource, IsBackUp = @IsBackUp, CreateTime = @CreateTime, HasPrivatekey = @HasPrivatekey Where MneId = @MneId ";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;

    cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier, 16).Value = model.MneId;
    cm.Parameters.Add("@MneAlias", SqlDbType.NVarChar, 64).Value = model.MneAlias;
    cm.Parameters.Add("@MneEncrypted", SqlDbType.NVarChar, 1024).Value = model.MneEncrypted;
    cm.Parameters.Add("@EncryptedTimes", SqlDbType.Int, 4).Value = model.EncryptedTimes;
    cm.Parameters.Add("@MneHash", SqlDbType.NVarChar, 64).Value = model.MneHash;
    cm.Parameters.Add("@WordCount", SqlDbType.Int, 4).Value = model.WordCount;
    cm.Parameters.Add("@MnePath", SqlDbType.NVarChar, 32).Value = model.MnePath;
    cm.Parameters.Add("@Salt", SqlDbType.NVarChar, 64).Value = model.Salt;
    cm.Parameters.Add("@UserPasswordHash", SqlDbType.NVarChar, 64).Value = model.UserPasswordHash;
    cm.Parameters.Add("@UserPasswordTip", SqlDbType.NVarChar, 64).Value = model.UserPasswordTip;
    cm.Parameters.Add("@MneFirstSalt", SqlDbType.NVarChar, 128).Value = model.MneFirstSalt;
    cm.Parameters.Add("@MneSource", SqlDbType.Int, 4).Value = model.MneSource;
    cm.Parameters.Add("@IsBackUp", SqlDbType.Bit, 1).Value = model.IsBackUp;
    cm.Parameters.Add("@CreateTime", SqlDbType.DateTime, 8).Value = model.CreateTime;
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

#region  表 HD_Mnemonic 的GetModel操作
public static Model.HD_Mnemonic GetModel(string conStr, System.Guid MneId)
{
    string sql = @"select * from HD_Mnemonic Where MneId = @MneId ";

    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;
    cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier, 16).Value = MneId;

    SqlDataAdapter da = new SqlDataAdapter();
    da.SelectCommand = cm;
    System.Data.DataSet ds = new System.Data.DataSet();
    da.Fill(ds);
    if (ds.Tables[0].Rows.Count == 1)
    {
        return Model.HD_Mnemonic.DataRow2Object(ds.Tables[0].Rows[0]);
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

#region  表 HD_Mnemonic 的 Exist 操作 判断
public static bool Exist(string conStr, System.Guid MneId)
{
    string sql = @"Select Count(*) From HD_Mnemonic Where MneId = @MneId ";
    SqlConnection cn = new SqlConnection(conStr);
    SqlCommand cm = new SqlCommand();
    cm.Connection = cn;
    cm.CommandType = System.Data.CommandType.Text;
    cm.CommandText = sql;
    cm.Parameters.Add("@MneId", SqlDbType.UniqueIdentifier, 16).Value = MneId;

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