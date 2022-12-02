
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 11:02:11
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Wallet.Model
{
    [Serializable]
    public class Contact
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From Contact";
        #endregion

        #region Public Properties

        private System.String _ContactName = System.String.Empty;
        public System.String ContactName
        {
            get { return _ContactName; }
            set { _ContactName = value; }
        }

        private System.String _ContactAddress = System.String.Empty;
        public System.String ContactAddress
        {
            get { return _ContactAddress; }
            set { _ContactAddress = value; }
        }

        private System.String _ContactRemark = System.String.Empty;
        public System.String ContactRemark
        {
            get { return _ContactRemark; }
            set { _ContactRemark = value; }
        }

        private System.DateTime _CreateTime = System.DateTime.Now;
        public System.DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

        #endregion

        #region Public construct

        public Contact()
        {
        }


        public Contact(System.String AContactAddress)
        {
            _ContactAddress = AContactAddress;
        }

        #endregion

        #region Public DataRow2Object

        public static Contact DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            Contact Obj = new Contact();
            Obj.ContactName = dr["ContactName"] == DBNull.Value ? string.Empty : (System.String)(dr["ContactName"]);
            Obj.ContactAddress = dr["ContactAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["ContactAddress"]);
            Obj.ContactRemark = dr["ContactRemark"] == DBNull.Value ? string.Empty : (System.String)(dr["ContactRemark"]);
            Obj.CreateTime = dr["CreateTime"] == DBNull.Value ? System.DateTime.Now : (System.DateTime)(dr["CreateTime"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.ContactName != null && this.ContactName.Length > FL_ContactName && FL_ContactName > 0) throw new Exception("ContactName要求长度小于等于" + FL_ContactName.ToString() + "。");
            if (this.ContactAddress != null && this.ContactAddress.Length > FL_ContactAddress && FL_ContactAddress > 0) throw new Exception("ContactAddress要求长度小于等于" + FL_ContactAddress.ToString() + "。");
            if (this.ContactRemark != null && this.ContactRemark.Length > FL_ContactRemark && FL_ContactRemark > 0) throw new Exception("ContactRemark要求长度小于等于" + FL_ContactRemark.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.ContactName) || string.IsNullOrEmpty(this.ContactName.Trim())) throw new Exception("ContactName要求不空。");
            if (string.IsNullOrEmpty(this.ContactAddress) || string.IsNullOrEmpty(this.ContactAddress.Trim())) throw new Exception("ContactAddress要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public Contact Copy()
        {
            Contact obj = new Contact();
            obj.ContactName = this.ContactName;
            obj.ContactAddress = this.ContactAddress;
            obj.ContactRemark = this.ContactRemark;
            obj.CreateTime = this.CreateTime;
            return obj;
        }



        public static List<Contact> DataTable2List(System.Data.DataTable dt)
        {
            List<Contact> result = new List<Contact>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(Contact.DataRow2Object(dr));
            }
            return result;
        }



        private static Contact _NullObject = null;

        public static Contact NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new Contact();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 ContactName 的长度——64
        /// </summary>
        public const int FL_ContactName = 64;


        /// <summary>
        /// 字段 ContactAddress 的长度——43
        /// </summary>
        public const int FL_ContactAddress = 43;


        /// <summary>
        /// 字段 ContactRemark 的长度——2048
        /// </summary>
        public const int FL_ContactRemark = 2048;


        /// <summary>
        /// 字段 CreateTime 的长度——8
        /// </summary>
        public const int FL_CreateTime = 8;


        #endregion
    }



}