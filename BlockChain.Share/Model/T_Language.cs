

using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class T_Language
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From T_Language";
        #endregion

        #region Public Properties

        private System.Int32 _LCID;
        public System.Int32 LCID
        {
            get { return _LCID; }
            set { _LCID = value; }
        }

        private System.String _CultureInfoName = System.String.Empty;
        public System.String CultureInfoName
        {
            get { return _CultureInfoName; }
            set { _CultureInfoName = value; }
        }

        private System.String _TwoLetterISOLanguageName = System.String.Empty;
        public System.String TwoLetterISOLanguageName
        {
            get { return _TwoLetterISOLanguageName; }
            set { _TwoLetterISOLanguageName = value; }
        }

        private System.String _ThreeLetterISOLanguageName = System.String.Empty;
        public System.String ThreeLetterISOLanguageName
        {
            get { return _ThreeLetterISOLanguageName; }
            set { _ThreeLetterISOLanguageName = value; }
        }

        private System.String _ThreeLetterWindowsLanguageName = System.String.Empty;
        public System.String ThreeLetterWindowsLanguageName
        {
            get { return _ThreeLetterWindowsLanguageName; }
            set { _ThreeLetterWindowsLanguageName = value; }
        }

        private System.String _NativeName = System.String.Empty;
        public System.String NativeName
        {
            get { return _NativeName; }
            set { _NativeName = value; }
        }

        private System.String _DisplayName = System.String.Empty;
        public System.String DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }

        private System.String _EnglishName = System.String.Empty;
        public System.String EnglishName
        {
            get { return _EnglishName; }
            set { _EnglishName = value; }
        }

        private System.Boolean _IsSelected;
        public System.Boolean IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; }
        }

        private System.Int32 _ItemsNumber;
        public System.Int32 ItemsNumber
        {
            get { return _ItemsNumber; }
            set { _ItemsNumber = value; }
        }

        #endregion

        #region Public construct

        public T_Language()
        {
        }


        public T_Language(System.String ACultureInfoName)
        {
            _CultureInfoName = ACultureInfoName;
        }

        #endregion

        #region Public DataRow2Object

        public static T_Language DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            T_Language Obj = new T_Language();
            Obj.LCID = dr["LCID"] == DBNull.Value ? 0 : (System.Int32)(dr["LCID"]);
            Obj.CultureInfoName = dr["CultureInfoName"] == DBNull.Value ? string.Empty : (System.String)(dr["CultureInfoName"]);
            Obj.TwoLetterISOLanguageName = dr["TwoLetterISOLanguageName"] == DBNull.Value ? string.Empty : (System.String)(dr["TwoLetterISOLanguageName"]);
            Obj.ThreeLetterISOLanguageName = dr["ThreeLetterISOLanguageName"] == DBNull.Value ? string.Empty : (System.String)(dr["ThreeLetterISOLanguageName"]);
            Obj.ThreeLetterWindowsLanguageName = dr["ThreeLetterWindowsLanguageName"] == DBNull.Value ? string.Empty : (System.String)(dr["ThreeLetterWindowsLanguageName"]);
            Obj.NativeName = dr["NativeName"] == DBNull.Value ? string.Empty : (System.String)(dr["NativeName"]);
            Obj.DisplayName = dr["DisplayName"] == DBNull.Value ? string.Empty : (System.String)(dr["DisplayName"]);
            Obj.EnglishName = dr["EnglishName"] == DBNull.Value ? string.Empty : (System.String)(dr["EnglishName"]);
            Obj.IsSelected = dr["IsSelected"] == DBNull.Value ? false : (System.Boolean)(dr["IsSelected"]);
            Obj.ItemsNumber = dr["ItemsNumber"] == DBNull.Value ? 0 : (System.Int32)(dr["ItemsNumber"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.CultureInfoName != null && this.CultureInfoName.Length > FL_CultureInfoName && FL_CultureInfoName > 0) throw new Exception("CultureInfoName要求长度小于等于" + FL_CultureInfoName.ToString() + "。");
            if (this.TwoLetterISOLanguageName != null && this.TwoLetterISOLanguageName.Length > FL_TwoLetterISOLanguageName && FL_TwoLetterISOLanguageName > 0) throw new Exception("TwoLetterISOLanguageName要求长度小于等于" + FL_TwoLetterISOLanguageName.ToString() + "。");
            if (this.ThreeLetterISOLanguageName != null && this.ThreeLetterISOLanguageName.Length > FL_ThreeLetterISOLanguageName && FL_ThreeLetterISOLanguageName > 0) throw new Exception("ThreeLetterISOLanguageName要求长度小于等于" + FL_ThreeLetterISOLanguageName.ToString() + "。");
            if (this.ThreeLetterWindowsLanguageName != null && this.ThreeLetterWindowsLanguageName.Length > FL_ThreeLetterWindowsLanguageName && FL_ThreeLetterWindowsLanguageName > 0) throw new Exception("ThreeLetterWindowsLanguageName要求长度小于等于" + FL_ThreeLetterWindowsLanguageName.ToString() + "。");
            if (this.NativeName != null && this.NativeName.Length > FL_NativeName && FL_NativeName > 0) throw new Exception("NativeName要求长度小于等于" + FL_NativeName.ToString() + "。");
            if (this.DisplayName != null && this.DisplayName.Length > FL_DisplayName && FL_DisplayName > 0) throw new Exception("DisplayName要求长度小于等于" + FL_DisplayName.ToString() + "。");
            if (this.EnglishName != null && this.EnglishName.Length > FL_EnglishName && FL_EnglishName > 0) throw new Exception("EnglishName要求长度小于等于" + FL_EnglishName.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.CultureInfoName) || string.IsNullOrEmpty(this.CultureInfoName.Trim())) throw new Exception("CultureInfoName要求不空。");
            if (string.IsNullOrEmpty(this.TwoLetterISOLanguageName) || string.IsNullOrEmpty(this.TwoLetterISOLanguageName.Trim())) throw new Exception("TwoLetterISOLanguageName要求不空。");
            if (string.IsNullOrEmpty(this.ThreeLetterISOLanguageName) || string.IsNullOrEmpty(this.ThreeLetterISOLanguageName.Trim())) throw new Exception("ThreeLetterISOLanguageName要求不空。");
            if (string.IsNullOrEmpty(this.ThreeLetterWindowsLanguageName) || string.IsNullOrEmpty(this.ThreeLetterWindowsLanguageName.Trim())) throw new Exception("ThreeLetterWindowsLanguageName要求不空。");
            if (string.IsNullOrEmpty(this.NativeName) || string.IsNullOrEmpty(this.NativeName.Trim())) throw new Exception("NativeName要求不空。");
            if (string.IsNullOrEmpty(this.DisplayName) || string.IsNullOrEmpty(this.DisplayName.Trim())) throw new Exception("DisplayName要求不空。");
            if (string.IsNullOrEmpty(this.EnglishName) || string.IsNullOrEmpty(this.EnglishName.Trim())) throw new Exception("EnglishName要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public T_Language Copy()
        {
            T_Language obj = new T_Language();
            obj.LCID = this.LCID;
            obj.CultureInfoName = this.CultureInfoName;
            obj.TwoLetterISOLanguageName = this.TwoLetterISOLanguageName;
            obj.ThreeLetterISOLanguageName = this.ThreeLetterISOLanguageName;
            obj.ThreeLetterWindowsLanguageName = this.ThreeLetterWindowsLanguageName;
            obj.NativeName = this.NativeName;
            obj.DisplayName = this.DisplayName;
            obj.EnglishName = this.EnglishName;
            obj.IsSelected = this.IsSelected;
            obj.ItemsNumber = this.ItemsNumber;
            return obj;
        }



        public static List<T_Language> DataTable2List(System.Data.DataTable dt)
        {
            List<T_Language> result = new List<T_Language>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(T_Language.DataRow2Object(dr));
            }
            return result;
        }



        private static T_Language _NullObject = null;

        public static T_Language NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new T_Language();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 LCID 的长度——4
        /// </summary>
        public const int FL_LCID = 4;


        /// <summary>
        /// 字段 CultureInfoName 的长度——32
        /// </summary>
        public const int FL_CultureInfoName = 32;


        /// <summary>
        /// 字段 TwoLetterISOLanguageName 的长度——8
        /// </summary>
        public const int FL_TwoLetterISOLanguageName = 8;


        /// <summary>
        /// 字段 ThreeLetterISOLanguageName 的长度——8
        /// </summary>
        public const int FL_ThreeLetterISOLanguageName = 8;


        /// <summary>
        /// 字段 ThreeLetterWindowsLanguageName 的长度——8
        /// </summary>
        public const int FL_ThreeLetterWindowsLanguageName = 8;


        /// <summary>
        /// 字段 NativeName 的长度——64
        /// </summary>
        public const int FL_NativeName = 64;


        /// <summary>
        /// 字段 DisplayName 的长度——64
        /// </summary>
        public const int FL_DisplayName = 64;


        /// <summary>
        /// 字段 EnglishName 的长度——64
        /// </summary>
        public const int FL_EnglishName = 64;


        /// <summary>
        /// 字段 IsSelected 的长度——1
        /// </summary>
        public const int FL_IsSelected = 1;


        /// <summary>
        /// 字段 ItemsNumber 的长度——4
        /// </summary>
        public const int FL_ItemsNumber = 4;


        #endregion
    }



}