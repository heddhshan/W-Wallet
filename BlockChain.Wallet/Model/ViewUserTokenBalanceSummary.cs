using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Wallet.Model
{
    #region Add Time 2022/9/4 15:34:43

    [Serializable]
    public class ViewUserTokenBalanceSummary
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @" SELECT ImagePath, Symbol, TokenAddress, COUNT(*) AS AddressNum, SUM(Balance) AS AllBalance, 0.0 StablecoinAmount, '*' StablecoinSymbol FROM (SELECT Token.ImagePath AS ImagePath, T_1.AddressAlias, AddressBalance.UserAddress, Token.Symbol, CONVERT(float, CASE WHEN AddressBalance.Balance = 0 THEN '0' ELSE CAST(AddressBalance.Balance AS varchar(64)) END) AS Balance, Token.Address AS TokenAddress FROM AddressBalance INNER JOIN Token ON Token.Address = AddressBalance.TokenAddress INNER JOIN (SELECT AddressAlias, Address FROM KeyStore_Address UNION SELECT AddressAlias, Address FROM HD_Address) AS T_1 ON T_1.Address = AddressBalance.UserAddress WHERE (AddressBalance.Balance > 0)) AS T GROUP BY ImagePath, Symbol, TokenAddress";
        #endregion

        #region Public Properties

        private System.String _ImagePath = System.String.Empty;
        public System.String ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

        private System.String _Symbol = System.String.Empty;
        public System.String Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        private System.String _TokenAddress = System.String.Empty;
        public System.String TokenAddress
        {
            get { return _TokenAddress; }
            set { _TokenAddress = value; }
        }

        private System.Int32 _AddressNum;
        public System.Int32 AddressNum
        {
            get { return _AddressNum; }
            set { _AddressNum = value; }
        }

        private System.Double _AllBalance;
        public System.Double AllBalance
        {
            get { return _AllBalance; }
            set { _AllBalance = value; }
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

        #endregion

        #region Public construct

        public ViewUserTokenBalanceSummary()
        {
        }


        public ViewUserTokenBalanceSummary(System.String AImagePath, System.Int32 AAddressNum, System.Double AAllBalance, System.Double AStablecoinAmount, System.String AStablecoinSymbol)
        {
            _ImagePath = AImagePath;
            _AddressNum = AAddressNum;
            _AllBalance = AAllBalance;
            _StablecoinAmount = AStablecoinAmount;
            _StablecoinSymbol = AStablecoinSymbol;
        }

        #endregion

        #region Public DataRow2Object

        public static ViewUserTokenBalanceSummary DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            ViewUserTokenBalanceSummary Obj = new ViewUserTokenBalanceSummary();
            Obj.ImagePath = dr["ImagePath"] == DBNull.Value ? string.Empty : (System.String)(dr["ImagePath"]);
            Obj.Symbol = dr["Symbol"] == DBNull.Value ? string.Empty : (System.String)(dr["Symbol"]);
            Obj.TokenAddress = dr["TokenAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["TokenAddress"]);
            Obj.AddressNum = dr["AddressNum"] == DBNull.Value ? 0 : (System.Int32)(dr["AddressNum"]);
            Obj.AllBalance = dr["AllBalance"] == DBNull.Value ? 0 : (System.Double)(dr["AllBalance"]);
            Obj.StablecoinAmount = dr["StablecoinAmount"] == DBNull.Value ? 0 :(double) (System.Decimal)(dr["StablecoinAmount"]);
            Obj.StablecoinSymbol = dr["StablecoinSymbol"] == DBNull.Value ? string.Empty : (System.String)(dr["StablecoinSymbol"]);
            return Obj;
        }

        #endregion




        public ViewUserTokenBalanceSummary Copy()
        {
            ViewUserTokenBalanceSummary obj = new ViewUserTokenBalanceSummary();
            obj.ImagePath = this.ImagePath;
            obj.Symbol = this.Symbol;
            obj.TokenAddress = this.TokenAddress;
            obj.AddressNum = this.AddressNum;
            obj.AllBalance = this.AllBalance;
            obj.StablecoinAmount = this.StablecoinAmount;
            obj.StablecoinSymbol = this.StablecoinSymbol;
            return obj;
        }



        public static List<ViewUserTokenBalanceSummary> DataTable2List(System.Data.DataTable dt)
        {
            List<ViewUserTokenBalanceSummary> result = new List<ViewUserTokenBalanceSummary>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(ViewUserTokenBalanceSummary.DataRow2Object(dr));
            }
            return result;
        }




    }


    #endregion


}
