
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 10:47:12
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class T_TranslationText_BAK
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From T_TranslationText_BAK";
        #endregion

        #region Public Properties

        private System.String _OriginalHash = System.String.Empty;
        public System.String OriginalHash
        {
            get { return _OriginalHash; }
            set { _OriginalHash = value; }
        }

        private System.String _LanCode = System.String.Empty;
        public System.String LanCode
        {
            get { return _LanCode; }
            set { _LanCode = value; }
        }

        private System.String _TranslationText = System.String.Empty;
        public System.String TranslationText
        {
            get { return _TranslationText; }
            set { _TranslationText = value; }
        }

        #endregion

        #region Public construct

        public T_TranslationText_BAK()
        {
        }


        public T_TranslationText_BAK(System.String AOriginalHash, System.String ALanCode)
        {
            _OriginalHash = AOriginalHash;
            _LanCode = ALanCode;
        }

        #endregion

        #region Public DataRow2Object

        public static T_TranslationText_BAK DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            T_TranslationText_BAK Obj = new T_TranslationText_BAK();
            Obj.OriginalHash = dr["OriginalHash"] == DBNull.Value ? string.Empty : (System.String)(dr["OriginalHash"]);
            Obj.LanCode = dr["LanCode"] == DBNull.Value ? string.Empty : (System.String)(dr["LanCode"]);
            Obj.TranslationText = dr["TranslationText"] == DBNull.Value ? string.Empty : (System.String)(dr["TranslationText"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.OriginalHash != null && this.OriginalHash.Length > FL_OriginalHash && FL_OriginalHash > 0) throw new Exception("OriginalHash要求长度小于等于" + FL_OriginalHash.ToString() + "。");
            if (this.LanCode != null && this.LanCode.Length > FL_LanCode && FL_LanCode > 0) throw new Exception("LanCode要求长度小于等于" + FL_LanCode.ToString() + "。");
            if (this.TranslationText != null && this.TranslationText.Length > FL_TranslationText && FL_TranslationText > 0) throw new Exception("TranslationText要求长度小于等于" + FL_TranslationText.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.OriginalHash) || string.IsNullOrEmpty(this.OriginalHash.Trim())) throw new Exception("OriginalHash要求不空。");
            if (string.IsNullOrEmpty(this.LanCode) || string.IsNullOrEmpty(this.LanCode.Trim())) throw new Exception("LanCode要求不空。");
            if (string.IsNullOrEmpty(this.TranslationText) || string.IsNullOrEmpty(this.TranslationText.Trim())) throw new Exception("TranslationText要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public T_TranslationText_BAK Copy()
        {
            T_TranslationText_BAK obj = new T_TranslationText_BAK();
            obj.OriginalHash = this.OriginalHash;
            obj.LanCode = this.LanCode;
            obj.TranslationText = this.TranslationText;
            return obj;
        }



        public static List<T_TranslationText_BAK> DataTable2List(System.Data.DataTable dt)
        {
            List<T_TranslationText_BAK> result = new List<T_TranslationText_BAK>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(T_TranslationText_BAK.DataRow2Object(dr));
            }
            return result;
        }



        private static T_TranslationText_BAK _NullObject = null;

        public static T_TranslationText_BAK NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new T_TranslationText_BAK();
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
        /// 字段 LanCode 的长度——8
        /// </summary>
        public const int FL_LanCode = 8;


        /// <summary>
        /// 字段 TranslationText 的长度——4000
        /// </summary>
        public const int FL_TranslationText = 4000;


        #endregion
    }



}