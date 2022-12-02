
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 11:02:11
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{

    #region Add Time 2022/6/11 21:03:12

    [Serializable]
    public class AppInfo_OnPublishAppDownload
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From AppInfo_OnPublishAppDownload";
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

        private System.Int64 __eventId;
        public System.Int64 _eventId
        {
            get { return __eventId; }
            set { __eventId = value; }
        }

        private System.Int32 __AppId;
        public System.Int32 _AppId
        {
            get { return __AppId; }
            set { __AppId = value; }
        }

        private System.Int32 __PlatformId;
        public System.Int32 _PlatformId
        {
            get { return __PlatformId; }
            set { __PlatformId = value; }
        }

        private System.Int32 __Version;
        public System.Int32 _Version
        {
            get { return __Version; }
            set { __Version = value; }
        }

        private System.String __HttpLink = System.String.Empty;
        public System.String _HttpLink
        {
            get { return __HttpLink; }
            set { __HttpLink = value; }
        }

        private System.String __BTLink = System.String.Empty;
        public System.String _BTLink
        {
            get { return __BTLink; }
            set { __BTLink = value; }
        }

        private System.String __eMuleLink = System.String.Empty;
        public System.String _eMuleLink
        {
            get { return __eMuleLink; }
            set { __eMuleLink = value; }
        }

        private System.String __IpfsLink = System.String.Empty;
        public System.String _IpfsLink
        {
            get { return __IpfsLink; }
            set { __IpfsLink = value; }
        }

        private System.String __OtherLink = System.String.Empty;
        public System.String _OtherLink
        {
            get { return __OtherLink; }
            set { __OtherLink = value; }
        }

        #endregion

        #region Public construct

        public AppInfo_OnPublishAppDownload()
        {
        }


        public AppInfo_OnPublishAppDownload(System.String AContractAddress, System.Int64 A_eventId)
        {
            _ContractAddress = AContractAddress;
            __eventId = A_eventId;
        }

        #endregion

        #region Public DataRow2Object

        public static AppInfo_OnPublishAppDownload DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            AppInfo_OnPublishAppDownload Obj = new AppInfo_OnPublishAppDownload();
            Obj.ContractAddress = dr["ContractAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["ContractAddress"]);
            Obj.BlockNumber = dr["BlockNumber"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["BlockNumber"]);
            Obj.TransactionHash = dr["TransactionHash"] == DBNull.Value ? string.Empty : (System.String)(dr["TransactionHash"]);
            Obj._eventId = dr["_eventId"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["_eventId"]);
            Obj._AppId = dr["_AppId"] == DBNull.Value ? 0 : (System.Int32)(dr["_AppId"]);
            Obj._PlatformId = dr["_PlatformId"] == DBNull.Value ? 0 : (System.Int32)(dr["_PlatformId"]);
            Obj._Version = dr["_Version"] == DBNull.Value ? 0 : (System.Int32)(dr["_Version"]);
            Obj._HttpLink = dr["_HttpLink"] == DBNull.Value ? string.Empty : (System.String)(dr["_HttpLink"]);
            Obj._BTLink = dr["_BTLink"] == DBNull.Value ? string.Empty : (System.String)(dr["_BTLink"]);
            Obj._eMuleLink = dr["_eMuleLink"] == DBNull.Value ? string.Empty : (System.String)(dr["_eMuleLink"]);
            Obj._IpfsLink = dr["_IpfsLink"] == DBNull.Value ? string.Empty : (System.String)(dr["_IpfsLink"]);
            Obj._OtherLink = dr["_OtherLink"] == DBNull.Value ? string.Empty : (System.String)(dr["_OtherLink"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.ContractAddress != null && this.ContractAddress.Length > FL_ContractAddress && FL_ContractAddress > 0) throw new Exception("ContractAddress要求长度小于等于" + FL_ContractAddress.ToString() + "。");
            if (this.TransactionHash != null && this.TransactionHash.Length > FL_TransactionHash && FL_TransactionHash > 0) throw new Exception("TransactionHash要求长度小于等于" + FL_TransactionHash.ToString() + "。");
            if (this._HttpLink != null && this._HttpLink.Length > FL__HttpLink && FL__HttpLink > 0) throw new Exception("_HttpLink要求长度小于等于" + FL__HttpLink.ToString() + "。");
            if (this._BTLink != null && this._BTLink.Length > FL__BTLink && FL__BTLink > 0) throw new Exception("_BTLink要求长度小于等于" + FL__BTLink.ToString() + "。");
            if (this._eMuleLink != null && this._eMuleLink.Length > FL__eMuleLink && FL__eMuleLink > 0) throw new Exception("_eMuleLink要求长度小于等于" + FL__eMuleLink.ToString() + "。");
            if (this._IpfsLink != null && this._IpfsLink.Length > FL__IpfsLink && FL__IpfsLink > 0) throw new Exception("_IpfsLink要求长度小于等于" + FL__IpfsLink.ToString() + "。");
            if (this._OtherLink != null && this._OtherLink.Length > FL__OtherLink && FL__OtherLink > 0) throw new Exception("_OtherLink要求长度小于等于" + FL__OtherLink.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.ContractAddress) || string.IsNullOrEmpty(this.ContractAddress.Trim())) throw new Exception("ContractAddress要求不空。");
            if (string.IsNullOrEmpty(this.TransactionHash) || string.IsNullOrEmpty(this.TransactionHash.Trim())) throw new Exception("TransactionHash要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public AppInfo_OnPublishAppDownload Copy()
        {
            AppInfo_OnPublishAppDownload obj = new AppInfo_OnPublishAppDownload();
            obj.ContractAddress = this.ContractAddress;
            obj.BlockNumber = this.BlockNumber;
            obj.TransactionHash = this.TransactionHash;
            obj._eventId = this._eventId;
            obj._AppId = this._AppId;
            obj._PlatformId = this._PlatformId;
            obj._Version = this._Version;
            obj._HttpLink = this._HttpLink;
            obj._BTLink = this._BTLink;
            obj._eMuleLink = this._eMuleLink;
            obj._IpfsLink = this._IpfsLink;
            obj._OtherLink = this._OtherLink;
            return obj;
        }



        public static List<AppInfo_OnPublishAppDownload> DataTable2List(System.Data.DataTable dt)
        {
            List<AppInfo_OnPublishAppDownload> result = new List<AppInfo_OnPublishAppDownload>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(AppInfo_OnPublishAppDownload.DataRow2Object(dr));
            }
            return result;
        }



        private static AppInfo_OnPublishAppDownload _NullObject = null;

        public static AppInfo_OnPublishAppDownload NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new AppInfo_OnPublishAppDownload();
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
        /// 字段 _eventId 的长度——8
        /// </summary>
        public const int FL__eventId = 8;


        /// <summary>
        /// 字段 _AppId 的长度——4
        /// </summary>
        public const int FL__AppId = 4;


        /// <summary>
        /// 字段 _PlatformId 的长度——4
        /// </summary>
        public const int FL__PlatformId = 4;


        /// <summary>
        /// 字段 _Version 的长度——4
        /// </summary>
        public const int FL__Version = 4;


        /// <summary>
        /// 字段 _HttpLink 的长度——1024
        /// </summary>
        public const int FL__HttpLink = 1024;


        /// <summary>
        /// 字段 _BTLink 的长度——1024
        /// </summary>
        public const int FL__BTLink = 1024;


        /// <summary>
        /// 字段 _eMuleLink 的长度——1024
        /// </summary>
        public const int FL__eMuleLink = 1024;


        /// <summary>
        /// 字段 _IpfsLink 的长度——1024
        /// </summary>
        public const int FL__IpfsLink = 1024;


        /// <summary>
        /// 字段 _OtherLink 的长度——1024
        /// </summary>
        public const int FL__OtherLink = 1024;


        #endregion
    }


    #endregion



}