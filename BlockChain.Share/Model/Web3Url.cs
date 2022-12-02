
//
//  本文件由代码工具自动生成。如非必要请不要修改。2020-10-15 10:03:45
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class Web3Url
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From Web3Url";
        #endregion

        #region Public Properties

        private System.String _UrlAlias = System.String.Empty;
        public System.String UrlAlias
        {
            get { return _UrlAlias; }
            set { _UrlAlias = value; }
        }

        private System.String _UrlHash = System.String.Empty;
        public System.String UrlHash
        {
            get { return _UrlHash; }
            set { _UrlHash = value; }
        }

        private System.String _Url = System.String.Empty;
        public System.String Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        private System.Boolean _IsSelected;
        public System.Boolean IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; }
        }

        #endregion

        #region Public construct

        public Web3Url()
        {
        }


        public Web3Url(System.String AUrlHash)
        {
            _UrlHash = AUrlHash;
        }

        #endregion

        #region Public DataRow2Object

        public static Web3Url DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            Web3Url Obj = new Web3Url();
            Obj.UrlAlias = dr["UrlAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["UrlAlias"]);
            Obj.UrlHash = dr["UrlHash"] == DBNull.Value ? string.Empty : (System.String)(dr["UrlHash"]);
            Obj.Url = dr["Url"] == DBNull.Value ? string.Empty : (System.String)(dr["Url"]);
            Obj.IsSelected = dr["IsSelected"] == DBNull.Value ? false : (System.Boolean)(dr["IsSelected"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.UrlAlias != null && this.UrlAlias.Length > FL_UrlAlias && FL_UrlAlias > 0) throw new Exception("UrlAlias要求长度小于等于" + FL_UrlAlias.ToString() + "。");
            if (this.UrlHash != null && this.UrlHash.Length > FL_UrlHash && FL_UrlHash > 0) throw new Exception("UrlHash要求长度小于等于" + FL_UrlHash.ToString() + "。");
            if (this.Url != null && this.Url.Length > FL_Url && FL_Url > 0) throw new Exception("Url要求长度小于等于" + FL_Url.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.UrlAlias) || string.IsNullOrEmpty(this.UrlAlias.Trim())) throw new Exception("UrlAlias要求不空。");
            if (string.IsNullOrEmpty(this.UrlHash) || string.IsNullOrEmpty(this.UrlHash.Trim())) throw new Exception("UrlHash要求不空。");
            if (string.IsNullOrEmpty(this.Url) || string.IsNullOrEmpty(this.Url.Trim())) throw new Exception("Url要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public Web3Url Copy()
        {
            Web3Url obj = new Web3Url();
            obj.UrlAlias = this.UrlAlias;
            obj.UrlHash = this.UrlHash;
            obj.Url = this.Url;
            obj.IsSelected = this.IsSelected;
            return obj;
        }



        public static List<Web3Url> DataTable2List(System.Data.DataTable dt)
        {
            List<Web3Url> result = new List<Web3Url>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(Web3Url.DataRow2Object(dr));
            }
            return result;
        }



        private static Web3Url _NullObject = null;

        public static Web3Url NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new Web3Url();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 UrlAlias 的长度——64
        /// </summary>
        public const int FL_UrlAlias = 64;


        /// <summary>
        /// 字段 UrlHash 的长度——128
        /// </summary>
        public const int FL_UrlHash = 128;


        /// <summary>
        /// 字段 Url 的长度——2000
        /// </summary>
        public const int FL_Url = 2000;


        /// <summary>
        /// 字段 IsSelected 的长度——1
        /// </summary>
        public const int FL_IsSelected = 1;


        #endregion
    }



}