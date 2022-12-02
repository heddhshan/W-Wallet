
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 10:47:12
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class T_Refrence
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From T_Refrence";
        #endregion

        #region Public Properties

        private System.String _OriginalHash = System.String.Empty;
        public System.String OriginalHash
        {
            get { return _OriginalHash; }
            set { _OriginalHash = value; }
        }

        private System.String _RefrenceFormHash = System.String.Empty;
        public System.String RefrenceFormHash
        {
            get { return _RefrenceFormHash; }
            set { _RefrenceFormHash = value; }
        }

        private System.String _RefrenceForm = System.String.Empty;
        public System.String RefrenceForm
        {
            get { return _RefrenceForm; }
            set { _RefrenceForm = value; }
        }

        #endregion

        #region Public construct

        public T_Refrence()
        {
        }


        public T_Refrence(System.String AOriginalHash, System.String ARefrenceFormHash)
        {
            _OriginalHash = AOriginalHash;
            _RefrenceFormHash = ARefrenceFormHash;
        }

        #endregion

        #region Public DataRow2Object

        public static T_Refrence DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            T_Refrence Obj = new T_Refrence();
            Obj.OriginalHash = dr["OriginalHash"] == DBNull.Value ? string.Empty : (System.String)(dr["OriginalHash"]);
            Obj.RefrenceFormHash = dr["RefrenceFormHash"] == DBNull.Value ? string.Empty : (System.String)(dr["RefrenceFormHash"]);
            Obj.RefrenceForm = dr["RefrenceForm"] == DBNull.Value ? string.Empty : (System.String)(dr["RefrenceForm"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.OriginalHash != null && this.OriginalHash.Length > FL_OriginalHash && FL_OriginalHash > 0) throw new Exception("OriginalHash要求长度小于等于" + FL_OriginalHash.ToString() + "。");
            if (this.RefrenceFormHash != null && this.RefrenceFormHash.Length > FL_RefrenceFormHash && FL_RefrenceFormHash > 0) throw new Exception("RefrenceFormHash要求长度小于等于" + FL_RefrenceFormHash.ToString() + "。");
            if (this.RefrenceForm != null && this.RefrenceForm.Length > FL_RefrenceForm && FL_RefrenceForm > 0) throw new Exception("RefrenceForm要求长度小于等于" + FL_RefrenceForm.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.OriginalHash) || string.IsNullOrEmpty(this.OriginalHash.Trim())) throw new Exception("OriginalHash要求不空。");
            if (string.IsNullOrEmpty(this.RefrenceFormHash) || string.IsNullOrEmpty(this.RefrenceFormHash.Trim())) throw new Exception("RefrenceFormHash要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public T_Refrence Copy()
        {
            T_Refrence obj = new T_Refrence();
            obj.OriginalHash = this.OriginalHash;
            obj.RefrenceFormHash = this.RefrenceFormHash;
            obj.RefrenceForm = this.RefrenceForm;
            return obj;
        }



        public static List<T_Refrence> DataTable2List(System.Data.DataTable dt)
        {
            List<T_Refrence> result = new List<T_Refrence>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(T_Refrence.DataRow2Object(dr));
            }
            return result;
        }



        private static T_Refrence _NullObject = null;

        public static T_Refrence NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new T_Refrence();
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
        /// 字段 RefrenceFormHash 的长度——64
        /// </summary>
        public const int FL_RefrenceFormHash = 64;


        /// <summary>
        /// 字段 RefrenceForm 的长度——3096
        /// </summary>
        public const int FL_RefrenceForm = 3096;


        #endregion
    }



}