using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Share
{

    /// <summary>
    /// 账号和地址相关的公共接口，用于 WindowAccount 等！  钱包的地址和其他APP的地址不一样，钱包有助剂词！
    /// </summary>
    public interface IAddress
    {
        Nethereum.Web3.Accounts.Account GetAccount(string address, string password);

       double GetRealBalance(string address, string token, bool isCache = false);

        List<TxAddress> GetAllTxAddress();

    }



    [Serializable]
    public class TxAddress
    {   

        private System.String _AddressAlias = System.String.Empty;
        public System.String AddressAlias
        {
            get { return _AddressAlias; }
            set { _AddressAlias = value; }
        }

        private System.String _Address = System.String.Empty;
        public System.String Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        private System.String _SourceName = System.String.Empty;
        public System.String SourceName
        {
            get { return _SourceName; }
            set { _SourceName = value; }
        }

        #region Public DataRow2Object

        public static TxAddress DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            TxAddress Obj = new TxAddress();
            Obj.AddressAlias = dr["AddressAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["AddressAlias"]);
            Obj.Address = dr["Address"] == DBNull.Value ? string.Empty : (System.String)(dr["Address"]);
            Obj.SourceName = dr["SourceName"] == DBNull.Value ? string.Empty : (System.String)(dr["SourceName"]);
            return Obj;
        }

        #endregion



        public static List<TxAddress> DataTable2List(System.Data.DataTable dt)
        {
            List<TxAddress> result = new List<TxAddress>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(TxAddress.DataRow2Object(dr));
            }
            return result;
        }


    }



}
