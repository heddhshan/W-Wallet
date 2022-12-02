
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 11:02:11
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{


    #region Add Time 2022/6/11 21:04:52

    [Serializable]
    public class AppInfo_OnPublishAppVersion
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From AppInfo_OnPublishAppVersion";
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

        private System.String __Sha256Value = System.String.Empty;
        public System.String _Sha256Value
        {
            get { return __Sha256Value; }
            set { __Sha256Value = value; }
        }

        private System.String __AppName = System.String.Empty;
        public System.String _AppName
        {
            get { return __AppName; }
            set { __AppName = value; }
        }

        private System.String __UpdateInfo = System.String.Empty;
        public System.String _UpdateInfo
        {
            get { return __UpdateInfo; }
            set { __UpdateInfo = value; }
        }

        private System.String __IconUri = System.String.Empty;
        public System.String _IconUri
        {
            get { return __IconUri; }
            set { __IconUri = value; }
        }

        #endregion

        #region Public construct

        public AppInfo_OnPublishAppVersion()
        {
        }


        public AppInfo_OnPublishAppVersion(System.String AContractAddress, System.Int64 A_eventId)
        {
            _ContractAddress = AContractAddress;
            __eventId = A_eventId;
        }

        #endregion

        #region Public DataRow2Object

        public static AppInfo_OnPublishAppVersion DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            AppInfo_OnPublishAppVersion Obj = new AppInfo_OnPublishAppVersion();
            Obj.ContractAddress = dr["ContractAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["ContractAddress"]);
            Obj.BlockNumber = dr["BlockNumber"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["BlockNumber"]);
            Obj.TransactionHash = dr["TransactionHash"] == DBNull.Value ? string.Empty : (System.String)(dr["TransactionHash"]);
            Obj._eventId = dr["_eventId"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["_eventId"]);
            Obj._AppId = dr["_AppId"] == DBNull.Value ? 0 : (System.Int32)(dr["_AppId"]);
            Obj._PlatformId = dr["_PlatformId"] == DBNull.Value ? 0 : (System.Int32)(dr["_PlatformId"]);
            Obj._Version = dr["_Version"] == DBNull.Value ? 0 : (System.Int32)(dr["_Version"]);
            Obj._Sha256Value = dr["_Sha256Value"] == DBNull.Value ? string.Empty : (System.String)(dr["_Sha256Value"]);
            Obj._AppName = dr["_AppName"] == DBNull.Value ? string.Empty : (System.String)(dr["_AppName"]);
            Obj._UpdateInfo = dr["_UpdateInfo"] == DBNull.Value ? string.Empty : (System.String)(dr["_UpdateInfo"]);
            Obj._IconUri = dr["_IconUri"] == DBNull.Value ? string.Empty : (System.String)(dr["_IconUri"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.ContractAddress != null && this.ContractAddress.Length > FL_ContractAddress && FL_ContractAddress > 0) throw new Exception("ContractAddress要求长度小于等于" + FL_ContractAddress.ToString() + "。");
            if (this.TransactionHash != null && this.TransactionHash.Length > FL_TransactionHash && FL_TransactionHash > 0) throw new Exception("TransactionHash要求长度小于等于" + FL_TransactionHash.ToString() + "。");
            if (this._Sha256Value != null && this._Sha256Value.Length > FL__Sha256Value && FL__Sha256Value > 0) throw new Exception("_Sha256Value要求长度小于等于" + FL__Sha256Value.ToString() + "。");
            if (this._AppName != null && this._AppName.Length > FL__AppName && FL__AppName > 0) throw new Exception("_AppName要求长度小于等于" + FL__AppName.ToString() + "。");
            if (this._UpdateInfo != null && this._UpdateInfo.Length > FL__UpdateInfo && FL__UpdateInfo > 0) throw new Exception("_UpdateInfo要求长度小于等于" + FL__UpdateInfo.ToString() + "。");
            if (this._IconUri != null && this._IconUri.Length > FL__IconUri && FL__IconUri > 0) throw new Exception("_IconUri要求长度小于等于" + FL__IconUri.ToString() + "。");
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


        public AppInfo_OnPublishAppVersion Copy()
        {
            AppInfo_OnPublishAppVersion obj = new AppInfo_OnPublishAppVersion();
            obj.ContractAddress = this.ContractAddress;
            obj.BlockNumber = this.BlockNumber;
            obj.TransactionHash = this.TransactionHash;
            obj._eventId = this._eventId;
            obj._AppId = this._AppId;
            obj._PlatformId = this._PlatformId;
            obj._Version = this._Version;
            obj._Sha256Value = this._Sha256Value;
            obj._AppName = this._AppName;
            obj._UpdateInfo = this._UpdateInfo;
            obj._IconUri = this._IconUri;
            return obj;
        }



        public static List<AppInfo_OnPublishAppVersion> DataTable2List(System.Data.DataTable dt)
        {
            List<AppInfo_OnPublishAppVersion> result = new List<AppInfo_OnPublishAppVersion>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(AppInfo_OnPublishAppVersion.DataRow2Object(dr));
            }
            return result;
        }



        private static AppInfo_OnPublishAppVersion _NullObject = null;

        public static AppInfo_OnPublishAppVersion NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new AppInfo_OnPublishAppVersion();
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
        /// 字段 _Sha256Value 的长度——128
        /// </summary>
        public const int FL__Sha256Value = 128;


        /// <summary>
        /// 字段 _AppName 的长度——128
        /// </summary>
        public const int FL__AppName = 128;


        /// <summary>
        /// 字段 _UpdateInfo 的长度——1024
        /// </summary>
        public const int FL__UpdateInfo = 1024;


        /// <summary>
        /// 字段 _IconUri 的长度——1024
        /// </summary>
        public const int FL__IconUri = 1024;


        #endregion
    }


    #endregion




}