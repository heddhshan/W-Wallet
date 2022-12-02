
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 10:47:12
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class T_OriginalText_BAK
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From T_OriginalText_BAK";
        #endregion

        #region Public Properties

        private System.String _OriginalHash = System.String.Empty;
        public System.String OriginalHash
        {
            get { return _OriginalHash; }
            set { _OriginalHash = value; }
        }

        private System.String _OriginalText = System.String.Empty;
        public System.String OriginalText
        {
            get { return _OriginalText; }
            set { _OriginalText = value; }
        }

        #endregion

        #region Public construct

        public T_OriginalText_BAK()
        {
        }


        public T_OriginalText_BAK(System.String AOriginalHash)
        {
            _OriginalHash = AOriginalHash;
        }

        #endregion

        #region Public DataRow2Object

        public static T_OriginalText_BAK DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            T_OriginalText_BAK Obj = new T_OriginalText_BAK();
            Obj.OriginalHash = dr["OriginalHash"] == DBNull.Value ? string.Empty : (System.String)(dr["OriginalHash"]);
            Obj.OriginalText = dr["OriginalText"] == DBNull.Value ? string.Empty : (System.String)(dr["OriginalText"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.OriginalHash != null && this.OriginalHash.Length > FL_OriginalHash && FL_OriginalHash > 0) throw new Exception("OriginalHash要求长度小于等于" + FL_OriginalHash.ToString() + "。");
            if (this.OriginalText != null && this.OriginalText.Length > FL_OriginalText && FL_OriginalText > 0) throw new Exception("OriginalText要求长度小于等于" + FL_OriginalText.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.OriginalHash) || string.IsNullOrEmpty(this.OriginalHash.Trim())) throw new Exception("OriginalHash要求不空。");
            if (string.IsNullOrEmpty(this.OriginalText) || string.IsNullOrEmpty(this.OriginalText.Trim())) throw new Exception("OriginalText要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public T_OriginalText_BAK Copy()
        {
            T_OriginalText_BAK obj = new T_OriginalText_BAK();
            obj.OriginalHash = this.OriginalHash;
            obj.OriginalText = this.OriginalText;
            return obj;
        }



        public static List<T_OriginalText_BAK> DataTable2List(System.Data.DataTable dt)
        {
            List<T_OriginalText_BAK> result = new List<T_OriginalText_BAK>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(T_OriginalText_BAK.DataRow2Object(dr));
            }
            return result;
        }



        private static T_OriginalText_BAK _NullObject = null;

        public static T_OriginalText_BAK NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new T_OriginalText_BAK();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 OriginalHash 的长度——64
        /// </summary>
        public const int FL_OriginalHash = 64;


        /// <summary>
        /// 字段 OriginalText 的长度——4000
        /// </summary>
        public const int FL_OriginalText = 4000;


        #endregion
    }



}