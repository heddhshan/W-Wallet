
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Wallet.Model
{

    #region Add Time 2022/5/31 17:07:07

    [Serializable]
    public class AddressBalance
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From AddressBalance";
        #endregion

        #region Public Properties

        private System.String _UserAddress = System.String.Empty;
        public System.String UserAddress
        {
            get { return _UserAddress; }
            set { _UserAddress = value; }
        }

        private System.String _TokenAddress = System.String.Empty;
        public System.String TokenAddress
        {
            get { return _TokenAddress; }
            set { _TokenAddress = value; }
        }

        private System.Decimal _Balance;
        public System.Decimal Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }

        private System.Int64 _UpdateBlockNumber;
        public System.Int64 UpdateBlockNumber
        {
            get { return _UpdateBlockNumber; }
            set { _UpdateBlockNumber = value; }
        }

        private System.String _Balance_Text = System.String.Empty;
        public System.String Balance_Text
        {
            get { return _Balance_Text; }
            set { _Balance_Text = value; }
        }

        private System.DateTime _UpdateDateTime = System.DateTime.Now;
        public System.DateTime UpdateDateTime
        {
            get { return _UpdateDateTime; }
            set { _UpdateDateTime = value; }
        }

        #endregion

        #region Public construct

        public AddressBalance()
        {
        }


        public AddressBalance(System.String AUserAddress, System.String ATokenAddress)
        {
            _UserAddress = AUserAddress;
            _TokenAddress = ATokenAddress;
        }

        #endregion

        #region Public DataRow2Object

        public static AddressBalance DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            AddressBalance Obj = new AddressBalance();
            Obj.UserAddress = dr["UserAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["UserAddress"]);
            Obj.TokenAddress = dr["TokenAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["TokenAddress"]);
            Obj.Balance = dr["Balance"] == DBNull.Value ? 0 : (System.Decimal)(dr["Balance"]);
            Obj.UpdateBlockNumber = dr["UpdateBlockNumber"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["UpdateBlockNumber"]);
            Obj.Balance_Text = dr["Balance_Text"] == DBNull.Value ? string.Empty : (System.String)(dr["Balance_Text"]);
            Obj.UpdateDateTime = dr["UpdateDateTime"] == DBNull.Value ? System.DateTime.Now : (System.DateTime)(dr["UpdateDateTime"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.UserAddress != null && this.UserAddress.Length > FL_UserAddress && FL_UserAddress > 0) throw new Exception("UserAddress要求长度小于等于" + FL_UserAddress.ToString() + "。");
            if (this.TokenAddress != null && this.TokenAddress.Length > FL_TokenAddress && FL_TokenAddress > 0) throw new Exception("TokenAddress要求长度小于等于" + FL_TokenAddress.ToString() + "。");
            if (this.Balance_Text != null && this.Balance_Text.Length > FL_Balance_Text && FL_Balance_Text > 0) throw new Exception("Balance_Text要求长度小于等于" + FL_Balance_Text.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.UserAddress) || string.IsNullOrEmpty(this.UserAddress.Trim())) throw new Exception("UserAddress要求不空。");
            if (string.IsNullOrEmpty(this.TokenAddress) || string.IsNullOrEmpty(this.TokenAddress.Trim())) throw new Exception("TokenAddress要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public AddressBalance Copy()
        {
            AddressBalance obj = new AddressBalance();
            obj.UserAddress = this.UserAddress;
            obj.TokenAddress = this.TokenAddress;
            obj.Balance = this.Balance;
            obj.UpdateBlockNumber = this.UpdateBlockNumber;
            obj.Balance_Text = this.Balance_Text;
            obj.UpdateDateTime = this.UpdateDateTime;
            return obj;
        }



        public static List<AddressBalance> DataTable2List(System.Data.DataTable dt)
        {
            List<AddressBalance> result = new List<AddressBalance>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(AddressBalance.DataRow2Object(dr));
            }
            return result;
        }



        private static AddressBalance _NullObject = null;

        public static AddressBalance NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new AddressBalance();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 UserAddress 的长度——64
        /// </summary>
        public const int FL_UserAddress = 64;


        /// <summary>
        /// 字段 TokenAddress 的长度——64
        /// </summary>
        public const int FL_TokenAddress = 64;


        /// <summary>
        /// 字段 Balance 的长度——17
        /// </summary>
        public const int FL_Balance = 17;


        /// <summary>
        /// 字段 UpdateBlockNumber 的长度——8
        /// </summary>
        public const int FL_UpdateBlockNumber = 8;


        /// <summary>
        /// 字段 Balance_Text 的长度——80
        /// </summary>
        public const int FL_Balance_Text = 80;


        /// <summary>
        /// 字段 UpdateDateTime 的长度——8
        /// </summary>
        public const int FL_UpdateDateTime = 8;


        #endregion
    }


    #endregion


}