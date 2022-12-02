

using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class Appinfo_OnPublishNotice
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From Appinfo_OnPublishNotice";
        #endregion

        #region Public Properties

        private System.String _ContractAddress = System.String.Empty;
        public System.String ContractAddress
        {
            get { return _ContractAddress; }
            set { _ContractAddress = value; }
        }

        private System.Int64 _BlockNumber;
        public System.Int64 BlockNumber
        {
            get { return _BlockNumber; }
            set { _BlockNumber = value; }
        }

        private System.String _TransactionHash = System.String.Empty;
        public System.String TransactionHash
        {
            get { return _TransactionHash; }
            set { _TransactionHash = value; }
        }

        private System.String __publisher = System.String.Empty;
        public System.String _publisher
        {
            get { return __publisher; }
            set { __publisher = value; }
        }

        private System.Int64 __appId;
        public System.Int64 _appId
        {
            get { return __appId; }
            set { __appId = value; }
        }

        private System.String __subject = System.String.Empty;
        public System.String _subject
        {
            get { return __subject; }
            set { __subject = value; }
        }

        private System.String __body = System.String.Empty;
        public System.String _body
        {
            get { return __body; }
            set { __body = value; }
        }

        private System.DateTime _BlockTime = System.DateTime.Now;
        public System.DateTime BlockTime
        {
            get { return _BlockTime; }
            set { _BlockTime = value; }
        }

        #endregion

        #region Public construct

        public Appinfo_OnPublishNotice()
        {
        }


        public Appinfo_OnPublishNotice(System.String ATransactionHash)
        {
            _TransactionHash = ATransactionHash;
        }

        #endregion

        #region Public DataRow2Object

        public static Appinfo_OnPublishNotice DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            Appinfo_OnPublishNotice Obj = new Appinfo_OnPublishNotice();
            Obj.ContractAddress = dr["ContractAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["ContractAddress"]);
            Obj.BlockNumber = dr["BlockNumber"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["BlockNumber"]);
            Obj.TransactionHash = dr["TransactionHash"] == DBNull.Value ? string.Empty : (System.String)(dr["TransactionHash"]);
            Obj._publisher = dr["_publisher"] == DBNull.Value ? string.Empty : (System.String)(dr["_publisher"]);
            Obj._appId = dr["_appId"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["_appId"]);
            Obj._subject = dr["_subject"] == DBNull.Value ? string.Empty : (System.String)(dr["_subject"]);
            Obj._body = dr["_body"] == DBNull.Value ? string.Empty : (System.String)(dr["_body"]);
            Obj.BlockTime = dr["BlockTime"] == DBNull.Value ? System.DateTime.Now : (System.DateTime)(dr["BlockTime"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.ContractAddress != null && this.ContractAddress.Length > FL_ContractAddress && FL_ContractAddress > 0) throw new Exception("ContractAddress要求长度小于等于" + FL_ContractAddress.ToString() + "。");
            if (this.TransactionHash != null && this.TransactionHash.Length > FL_TransactionHash && FL_TransactionHash > 0) throw new Exception("TransactionHash要求长度小于等于" + FL_TransactionHash.ToString() + "。");
            if (this._publisher != null && this._publisher.Length > FL__publisher && FL__publisher > 0) throw new Exception("_publisher要求长度小于等于" + FL__publisher.ToString() + "。");
            if (this._subject != null && this._subject.Length > FL__subject && FL__subject > 0) throw new Exception("_subject要求长度小于等于" + FL__subject.ToString() + "。");
            if (this._body != null && this._body.Length > FL__body && FL__body > 0) throw new Exception("_body要求长度小于等于" + FL__body.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.ContractAddress) || string.IsNullOrEmpty(this.ContractAddress.Trim())) throw new Exception("ContractAddress要求不空。");
            if (string.IsNullOrEmpty(this.TransactionHash) || string.IsNullOrEmpty(this.TransactionHash.Trim())) throw new Exception("TransactionHash要求不空。");
            if (string.IsNullOrEmpty(this._publisher) || string.IsNullOrEmpty(this._publisher.Trim())) throw new Exception("_publisher要求不空。");
            if (string.IsNullOrEmpty(this._subject) || string.IsNullOrEmpty(this._subject.Trim())) throw new Exception("_subject要求不空。");
            if (string.IsNullOrEmpty(this._body) || string.IsNullOrEmpty(this._body.Trim())) throw new Exception("_body要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public Appinfo_OnPublishNotice Copy()
        {
            Appinfo_OnPublishNotice obj = new Appinfo_OnPublishNotice();
            obj.ContractAddress = this.ContractAddress;
            obj.BlockNumber = this.BlockNumber;
            obj.TransactionHash = this.TransactionHash;
            obj._publisher = this._publisher;
            obj._appId = this._appId;
            obj._subject = this._subject;
            obj._body = this._body;
            obj.BlockTime = this.BlockTime;
            return obj;
        }



        public static List<Appinfo_OnPublishNotice> DataTable2List(System.Data.DataTable dt)
        {
            List<Appinfo_OnPublishNotice> result = new List<Appinfo_OnPublishNotice>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(Appinfo_OnPublishNotice.DataRow2Object(dr));
            }
            return result;
        }



        private static Appinfo_OnPublishNotice _NullObject = null;

        public static Appinfo_OnPublishNotice NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new Appinfo_OnPublishNotice();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 ContractAddress 的长度——43
        /// </summary>
        public const int FL_ContractAddress = 43;


        /// <summary>
        /// 字段 BlockNumber 的长度——8
        /// </summary>
        public const int FL_BlockNumber = 8;


        /// <summary>
        /// 字段 TransactionHash 的长度——66
        /// </summary>
        public const int FL_TransactionHash = 66;


        /// <summary>
        /// 字段 _publisher 的长度——43
        /// </summary>
        public const int FL__publisher = 43;


        /// <summary>
        /// 字段 _appId 的长度——8
        /// </summary>
        public const int FL__appId = 8;


        /// <summary>
        /// 字段 _subject 的长度——1024
        /// </summary>
        public const int FL__subject = 1024;


        /// <summary>
        /// 字段 _body 的长度——0
        /// </summary>
        public const int FL__body = 0;


        /// <summary>
        /// 字段 BlockTime 的长度——8
        /// </summary>
        public const int FL_BlockTime = 8;


        #endregion
    }



}