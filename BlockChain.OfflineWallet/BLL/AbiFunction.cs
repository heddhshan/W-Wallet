using BlockChain.OfflineWallet.DataSig;
using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NBitcoin.Scripting.PubKeyProvider;

namespace BlockChain.OfflineWallet.BLL
{

    public static class AbiFunction
    {

        /// <summary>
        /// 初始化系统的 function 定义
        /// </summary>
        public static int IniSysAbiFunction ()
        {
            //直接执行sql语句就可以了
            string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string sqlfile = System.IO.Path.Combine(BasePath, @"DataBase\SysAbiFun.txt");
            string sql = System.IO.File.ReadAllText(sqlfile);

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

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


        public static List<Model.AbiFunction> getAllFunction()
        {
            //FunctionFullName, FunctionFullNameHash, FunctionFullNameHash4, IsSysDefine, IsTestOk
            string sql = @"
SELECT  *
FROM   AbiFunction
";
            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return Model.AbiFunction.DataTable2List( ds.Tables[0]);
        }


        public static List<Model.View_AbiParamer> getAbiParamer(Guid funid)
        {           
            //string sql = @"
            //SELECT P.FunctionFullNameHash, P.ParamerType, P.ParamerName, P.ParamerOrder, F.IsSysDefine, F.FunctionFullName, '' AS TestValue
            //FROM   AbiParamer AS P INNER JOIN
            //          AbiFunction AS F ON F.FunctionFullNameHash = P.FunctionFullNameHash
            //where p.FunctionFullNameHash = @FunctionFullNameHash
            //";
            string sql = @"
SELECT P.FunId, P.ParamerType, P.ParamerName, P.ParamerOrder, F.IsSysDefine, F.FunctionFullName, '' AS TestValue
FROM   AbiParamer AS P INNER JOIN
          AbiFunction AS F ON F.FunId = P.FunId
WHERE (P.FunId = @FunId)
";
            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@FunId", SqlDbType.UniqueIdentifier).Value = funid;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return Model.View_AbiParamer.DataTable2List(ds.Tables[0]);
        }


    }


}
