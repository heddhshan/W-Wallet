
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/9/9 19:03:56
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class UserTokenApprove
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From UserTokenApprove";
        #endregion

        #region Public Properties

        private System.Int32 _ChainId;
        public System.Int32 ChainId
        {
            get { return _ChainId; }
            set { _ChainId = value; }
        }

        private System.String _TransactionHash = System.String.Empty;
        public System.String TransactionHash
        {
            get { return _TransactionHash; }
            set { _TransactionHash = value; }
        }

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

        private System.String _SpenderAddress = System.String.Empty;
        public System.String SpenderAddress
        {
            get { return _SpenderAddress; }
            set { _SpenderAddress = value; }
        }

        private System.Int32 _TokenDecimals;
        public System.Int32 TokenDecimals
        {
            get { return _TokenDecimals; }
            set { _TokenDecimals = value; }
        }

        private System.String _TokenSymbol = System.String.Empty;
        public System.String TokenSymbol
        {
            get { return _TokenSymbol; }
            set { _TokenSymbol = value; }
        }

        private System.Double _CurrentAmount;
        public System.Double CurrentAmount
        {
            get { return _CurrentAmount; }
            set { _CurrentAmount = value; }
        }

        private System.Boolean _IsDivToken;
        public System.Boolean IsDivToken
        {
            get { return _IsDivToken; }
            set { _IsDivToken = value; }
        }

        private System.Boolean _DivTokenIsWithdrawable;
        public System.Boolean DivTokenIsWithdrawable
        {
            get { return _DivTokenIsWithdrawable; }
            set { _DivTokenIsWithdrawable = value; }
        }

        #endregion

        #region Public construct

        public UserTokenApprove()
        {
        }


        public UserTokenApprove(System.Int32 AChainId, System.String AUserAddress, System.String ATokenAddress, System.String ASpenderAddress)
        {
            _ChainId = AChainId;
            _UserAddress = AUserAddress;
            _TokenAddress = ATokenAddress;
            _SpenderAddress = ASpenderAddress;
        }

        #endregion

        #region Public DataRow2Object

        public static UserTokenApprove DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            UserTokenApprove Obj = new UserTokenApprove();
            Obj.ChainId = dr["ChainId"] == DBNull.Value ? 0 : (System.Int32)(dr["ChainId"]);
            Obj.TransactionHash = dr["TransactionHash"] == DBNull.Value ? string.Empty : (System.String)(dr["TransactionHash"]);
            Obj.UserAddress = dr["UserAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["UserAddress"]);
            Obj.TokenAddress = dr["TokenAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["TokenAddress"]);
            Obj.SpenderAddress = dr["SpenderAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["SpenderAddress"]);
            Obj.TokenDecimals = dr["TokenDecimals"] == DBNull.Value ? 0 : (System.Int32)(dr["TokenDecimals"]);
            Obj.TokenSymbol = dr["TokenSymbol"] == DBNull.Value ? string.Empty : (System.String)(dr["TokenSymbol"]);
            Obj.CurrentAmount = dr["CurrentAmount"] == DBNull.Value ? 0 : (System.Double)(dr["CurrentAmount"]);
            Obj.IsDivToken = dr["IsDivToken"] == DBNull.Value ? false : (System.Boolean)(dr["IsDivToken"]);
            Obj.DivTokenIsWithdrawable = dr["DivTokenIsWithdrawable"] == DBNull.Value ? false : (System.Boolean)(dr["DivTokenIsWithdrawable"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.TransactionHash != null && this.TransactionHash.Length > FL_TransactionHash && FL_TransactionHash > 0) throw new Exception("TransactionHash要求长度小于等于" + FL_TransactionHash.ToString() + "。");
            if (this.UserAddress != null && this.UserAddress.Length > FL_UserAddress && FL_UserAddress > 0) throw new Exception("UserAddress要求长度小于等于" + FL_UserAddress.ToString() + "。");
            if (this.TokenAddress != null && this.TokenAddress.Length > FL_TokenAddress && FL_TokenAddress > 0) throw new Exception("TokenAddress要求长度小于等于" + FL_TokenAddress.ToString() + "。");
            if (this.SpenderAddress != null && this.SpenderAddress.Length > FL_SpenderAddress && FL_SpenderAddress > 0) throw new Exception("SpenderAddress要求长度小于等于" + FL_SpenderAddress.ToString() + "。");
            if (this.TokenSymbol != null && this.TokenSymbol.Length > FL_TokenSymbol && FL_TokenSymbol > 0) throw new Exception("TokenSymbol要求长度小于等于" + FL_TokenSymbol.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.TransactionHash) || string.IsNullOrEmpty(this.TransactionHash.Trim())) throw new Exception("TransactionHash要求不空。");
            if (string.IsNullOrEmpty(this.UserAddress) || string.IsNullOrEmpty(this.UserAddress.Trim())) throw new Exception("UserAddress要求不空。");
            if (string.IsNullOrEmpty(this.TokenAddress) || string.IsNullOrEmpty(this.TokenAddress.Trim())) throw new Exception("TokenAddress要求不空。");
            if (string.IsNullOrEmpty(this.SpenderAddress) || string.IsNullOrEmpty(this.SpenderAddress.Trim())) throw new Exception("SpenderAddress要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public UserTokenApprove Copy()
        {
            UserTokenApprove obj = new UserTokenApprove();
            obj.ChainId = this.ChainId;
            obj.TransactionHash = this.TransactionHash;
            obj.UserAddress = this.UserAddress;
            obj.TokenAddress = this.TokenAddress;
            obj.SpenderAddress = this.SpenderAddress;
            obj.TokenDecimals = this.TokenDecimals;
            obj.TokenSymbol = this.TokenSymbol;
            obj.CurrentAmount = this.CurrentAmount;
            obj.IsDivToken = this.IsDivToken;
            obj.DivTokenIsWithdrawable = this.DivTokenIsWithdrawable;
            return obj;
        }



        public static List<UserTokenApprove> DataTable2List(System.Data.DataTable dt)
        {
            List<UserTokenApprove> result = new List<UserTokenApprove>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(UserTokenApprove.DataRow2Object(dr));
            }
            return result;
        }



        private static UserTokenApprove _NullObject = null;

        public static UserTokenApprove NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new UserTokenApprove();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 ChainId 的长度——4
        /// </summary>
        public const int FL_ChainId = 4;


        /// <summary>
        /// 字段 TransactionHash 的长度——66
        /// </summary>
        public const int FL_TransactionHash = 66;


        /// <summary>
        /// 字段 UserAddress 的长度——43
        /// </summary>
        public const int FL_UserAddress = 43;


        /// <summary>
        /// 字段 TokenAddress 的长度——43
        /// </summary>
        public const int FL_TokenAddress = 43;


        /// <summary>
        /// 字段 SpenderAddress 的长度——43
        /// </summary>
        public const int FL_SpenderAddress = 43;


        /// <summary>
        /// 字段 TokenDecimals 的长度——4
        /// </summary>
        public const int FL_TokenDecimals = 4;


        /// <summary>
        /// 字段 TokenSymbol 的长度——64
        /// </summary>
        public const int FL_TokenSymbol = 64;


        /// <summary>
        /// 字段 CurrentAmount 的长度——8
        /// </summary>
        public const int FL_CurrentAmount = 8;


        /// <summary>
        /// 字段 IsDivToken 的长度——1
        /// </summary>
        public const int FL_IsDivToken = 1;


        /// <summary>
        /// 字段 DivTokenIsWithdrawable 的长度——1
        /// </summary>
        public const int FL_DivTokenIsWithdrawable = 1;


        #endregion
    }



}