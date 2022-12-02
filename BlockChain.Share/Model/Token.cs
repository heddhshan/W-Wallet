using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class Token
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From Token";
        #endregion

        #region Public Properties

        private System.Int32 _ChainId;
        public System.Int32 ChainId
        {
            get { return _ChainId; }
            set { _ChainId = value; }
        }

        private System.String _Name = System.String.Empty;
        public System.String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private System.String _Address = System.String.Empty;
        public System.String Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        private System.Int32 _Decimals;
        public System.Int32 Decimals
        {
            get { return _Decimals; }
            set { _Decimals = value; }
        }

        private System.String _Symbol = System.String.Empty;
        public System.String Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        private System.String _ImagePath = System.String.Empty;
        public System.String ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

        private System.Boolean _IsPricingToken;
        public System.Boolean IsPricingToken
        {
            get { return _IsPricingToken; }
            set { _IsPricingToken = value; }
        }

        private System.String _PricingTokenAddress = System.String.Empty;
        public System.String PricingTokenAddress
        {
            get { return _PricingTokenAddress; }
            set { _PricingTokenAddress = value; }
        }

        private System.Double _PricingTokenPrice;
        public System.Double PricingTokenPrice
        {
            get { return _PricingTokenPrice; }
            set { _PricingTokenPrice = value; }
        }

        private System.DateTime _PricingTokenPriceUpdateTime = System.DateTime.Now;
        public System.DateTime PricingTokenPriceUpdateTime
        {
            get { return _PricingTokenPriceUpdateTime; }
            set { _PricingTokenPriceUpdateTime = value; }
        }

        private System.Boolean _PricingIsFixed;
        public System.Boolean PricingIsFixed
        {
            get { return _PricingIsFixed; }
            set { _PricingIsFixed = value; }
        }

        #endregion

        #region Public construct

        public Token()
        {
        }


        public Token(System.Int32 AChainId, System.String AAddress)
        {
            _ChainId = AChainId;
            _Address = AAddress;
        }

        #endregion

        #region Public DataRow2Object

        public static Token DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            Token Obj = new Token();
            Obj.ChainId = dr["ChainId"] == DBNull.Value ? 0 : (System.Int32)(dr["ChainId"]);
            Obj.Name = dr["Name"] == DBNull.Value ? string.Empty : (System.String)(dr["Name"]);
            Obj.Address = dr["Address"] == DBNull.Value ? string.Empty : (System.String)(dr["Address"]);
            Obj.Decimals = dr["Decimals"] == DBNull.Value ? 0 : (System.Int32)(dr["Decimals"]);
            Obj.Symbol = dr["Symbol"] == DBNull.Value ? string.Empty : (System.String)(dr["Symbol"]);
            Obj.ImagePath = dr["ImagePath"] == DBNull.Value ? string.Empty : (System.String)(dr["ImagePath"]);
            Obj.IsPricingToken = dr["IsPricingToken"] == DBNull.Value ? false : (System.Boolean)(dr["IsPricingToken"]);
            Obj.PricingTokenAddress = dr["PricingTokenAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["PricingTokenAddress"]);
            Obj.PricingTokenPrice = dr["PricingTokenPrice"] == DBNull.Value ? 0 : (System.Double)(dr["PricingTokenPrice"]);
            Obj.PricingTokenPriceUpdateTime = dr["PricingTokenPriceUpdateTime"] == DBNull.Value ? System.DateTime.Now : (System.DateTime)(dr["PricingTokenPriceUpdateTime"]);
            Obj.PricingIsFixed = dr["PricingIsFixed"] == DBNull.Value ? false : (System.Boolean)(dr["PricingIsFixed"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.Name != null && this.Name.Length > FL_Name && FL_Name > 0) throw new Exception("Name要求长度小于等于" + FL_Name.ToString() + "。");
            if (this.Address != null && this.Address.Length > FL_Address && FL_Address > 0) throw new Exception("Address要求长度小于等于" + FL_Address.ToString() + "。");
            if (this.Symbol != null && this.Symbol.Length > FL_Symbol && FL_Symbol > 0) throw new Exception("Symbol要求长度小于等于" + FL_Symbol.ToString() + "。");
            if (this.ImagePath != null && this.ImagePath.Length > FL_ImagePath && FL_ImagePath > 0) throw new Exception("ImagePath要求长度小于等于" + FL_ImagePath.ToString() + "。");
            if (this.PricingTokenAddress != null && this.PricingTokenAddress.Length > FL_PricingTokenAddress && FL_PricingTokenAddress > 0) throw new Exception("PricingTokenAddress要求长度小于等于" + FL_PricingTokenAddress.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.Name) || string.IsNullOrEmpty(this.Name.Trim())) throw new Exception("Name要求不空。");
            if (string.IsNullOrEmpty(this.Address) || string.IsNullOrEmpty(this.Address.Trim())) throw new Exception("Address要求不空。");
            if (string.IsNullOrEmpty(this.Symbol) || string.IsNullOrEmpty(this.Symbol.Trim())) throw new Exception("Symbol要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public Token Copy()
        {
            Token obj = new Token();
            obj.ChainId = this.ChainId;
            obj.Name = this.Name;
            obj.Address = this.Address;
            obj.Decimals = this.Decimals;
            obj.Symbol = this.Symbol;
            obj.ImagePath = this.ImagePath;
            obj.IsPricingToken = this.IsPricingToken;
            obj.PricingTokenAddress = this.PricingTokenAddress;
            obj.PricingTokenPrice = this.PricingTokenPrice;
            obj.PricingTokenPriceUpdateTime = this.PricingTokenPriceUpdateTime;
            obj.PricingIsFixed = this.PricingIsFixed;
            return obj;
        }



        public static List<Token> DataTable2List(System.Data.DataTable dt)
        {
            List<Token> result = new List<Token>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(Token.DataRow2Object(dr));
            }
            return result;
        }



        private static Token _NullObject = null;

        public static Token NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new Token();
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
        /// 字段 Name 的长度——64
        /// </summary>
        public const int FL_Name = 64;


        /// <summary>
        /// 字段 Address 的长度——43
        /// </summary>
        public const int FL_Address = 43;


        /// <summary>
        /// 字段 Decimals 的长度——4
        /// </summary>
        public const int FL_Decimals = 4;


        /// <summary>
        /// 字段 Symbol 的长度——64
        /// </summary>
        public const int FL_Symbol = 64;


        /// <summary>
        /// 字段 ImagePath 的长度——2048
        /// </summary>
        public const int FL_ImagePath = 2048;


        /// <summary>
        /// 字段 IsPricingToken 的长度——1
        /// </summary>
        public const int FL_IsPricingToken = 1;


        /// <summary>
        /// 字段 PricingTokenAddress 的长度——43
        /// </summary>
        public const int FL_PricingTokenAddress = 43;


        /// <summary>
        /// 字段 PricingTokenPrice 的长度——8
        /// </summary>
        public const int FL_PricingTokenPrice = 8;


        /// <summary>
        /// 字段 PricingTokenPriceUpdateTime 的长度——8
        /// </summary>
        public const int FL_PricingTokenPriceUpdateTime = 8;


        /// <summary>
        /// 字段 PricingIsFixed 的长度——1
        /// </summary>
        public const int FL_PricingIsFixed = 1;


        #endregion
    }



}