using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Wallet.Model
{
    #region Add Time 2022/9/4 15:06:23

    [Serializable]
    public class ViewUserTokenBalance
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"select * from ( SELECT ROW_NUMBER() OVER(Order by AddressBalance.Balance desc) AS RowId, 0.0001 StablecoinAmount, '***' StablecoinSymbol, Token.ImagePath AS ImagePath, T.AddressAlias, AddressBalance.UserAddress, Token.Symbol, AddressBalance.UpdateDateTime, AddressBalance.UpdateBlockNumber, CONVERT(float, CASE WHEN AddressBalance.Balance = 0 THEN '0' ELSE CAST(AddressBalance.Balance AS varchar(64)) END) AS Balance, Token.Address AS TokenAddress FROM AddressBalance INNER JOIN Token ON Token.Address = AddressBalance.TokenAddress INNER JOIN (SELECT AddressAlias, Address FROM KeyStore_Address UNION SELECT AddressAlias, Address FROM HD_Address) AS T ON T.Address = AddressBalance.UserAddress ) T";
        #endregion

        #region Public Properties

        private System.Int64 _RowId;
        public System.Int64 RowId
        {
            get { return _RowId; }
            set { _RowId = value; }
        }

        private System.Double _StablecoinAmount;
        public System.Double StablecoinAmount
        {
            get { return _StablecoinAmount; }
            set { _StablecoinAmount = value; }
        }

        private System.String _StablecoinSymbol = System.String.Empty;
        public System.String StablecoinSymbol
        {
            get { return _StablecoinSymbol; }
            set { _StablecoinSymbol = value; }
        }

        private System.String _ImagePath = System.String.Empty;
        public System.String ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

        private System.String _AddressAlias = System.String.Empty;
        public System.String AddressAlias
        {
            get { return _AddressAlias; }
            set { _AddressAlias = value; }
        }

        private System.String _UserAddress = System.String.Empty;
        public System.String UserAddress
        {
            get { return _UserAddress; }
            set { _UserAddress = value; }
        }

        private System.String _Symbol = System.String.Empty;
        public System.String Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        private System.DateTime _UpdateDateTime = System.DateTime.Now;
        public System.DateTime UpdateDateTime
        {
            get { return _UpdateDateTime; }
            set { _UpdateDateTime = value; }
        }

        private System.Int64 _UpdateBlockNumber;
        public System.Int64 UpdateBlockNumber
        {
            get { return _UpdateBlockNumber; }
            set { _UpdateBlockNumber = value; }
        }

        private System.Double _Balance;
        public System.Double Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }

        private System.String _TokenAddress = System.String.Empty;
        public System.String TokenAddress
        {
            get { return _TokenAddress; }
            set { _TokenAddress = value; }
        }

        #endregion

        #region Public construct

        public ViewUserTokenBalance()
        {
        }


        public ViewUserTokenBalance(System.Int64 ARowId, System.Double AStablecoinAmount, System.String AStablecoinSymbol, System.String AAddressAlias, System.Double ABalance)
        {
            _RowId = ARowId;
            _StablecoinAmount = AStablecoinAmount;
            _StablecoinSymbol = AStablecoinSymbol;
            _AddressAlias = AAddressAlias;
            _Balance = ABalance;
        }

        #endregion

        #region Public DataRow2Object

        public static ViewUserTokenBalance DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            ViewUserTokenBalance Obj = new ViewUserTokenBalance();
            Obj.RowId = dr["RowId"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["RowId"]);
            Obj.StablecoinAmount = dr["StablecoinAmount"] == DBNull.Value ? 0 : (double)(System.Decimal)(dr["StablecoinAmount"]);
            Obj.StablecoinSymbol = dr["StablecoinSymbol"] == DBNull.Value ? string.Empty : (System.String)(dr["StablecoinSymbol"]);
            Obj.ImagePath = dr["ImagePath"] == DBNull.Value ? string.Empty : (System.String)(dr["ImagePath"]);
            Obj.AddressAlias = dr["AddressAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["AddressAlias"]);
            Obj.UserAddress = dr["UserAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["UserAddress"]);
            Obj.Symbol = dr["Symbol"] == DBNull.Value ? string.Empty : (System.String)(dr["Symbol"]);
            Obj.UpdateDateTime = dr["UpdateDateTime"] == DBNull.Value ? System.DateTime.Now : (System.DateTime)(dr["UpdateDateTime"]);
            Obj.UpdateBlockNumber = dr["UpdateBlockNumber"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["UpdateBlockNumber"]);
            Obj.Balance = dr["Balance"] == DBNull.Value ? 0 : (System.Double)(dr["Balance"]);
            Obj.TokenAddress = dr["TokenAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["TokenAddress"]);
            return Obj;
        }

        #endregion



        public ViewUserTokenBalance Copy()
        {
            ViewUserTokenBalance obj = new ViewUserTokenBalance();
            obj.RowId = this.RowId;
            obj.StablecoinAmount = this.StablecoinAmount;
            obj.StablecoinSymbol = this.StablecoinSymbol;
            obj.ImagePath = this.ImagePath;
            obj.AddressAlias = this.AddressAlias;
            obj.UserAddress = this.UserAddress;
            obj.Symbol = this.Symbol;
            obj.UpdateDateTime = this.UpdateDateTime;
            obj.UpdateBlockNumber = this.UpdateBlockNumber;
            obj.Balance = this.Balance;
            obj.TokenAddress = this.TokenAddress;
            return obj;
        }



        public static List<ViewUserTokenBalance> DataTable2List(System.Data.DataTable dt)
        {
            List<ViewUserTokenBalance> result = new List<ViewUserTokenBalance>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(ViewUserTokenBalance.DataRow2Object(dr));
            }
            return result;
        }





    }


    #endregion



}
