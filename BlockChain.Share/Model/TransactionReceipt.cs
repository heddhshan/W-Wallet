
//
//  本文件由代码工具自动生成。如非必要请不要修改。2021-02-01 10:46:38
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class TransactionReceipt
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From TransactionReceipt";
        #endregion

        #region Public Properties

        private System.Int32 _Id;
        public System.Int32 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private System.String _TransactionHash = System.String.Empty;
        public System.String TransactionHash
        {
            get { return _TransactionHash; }
            set { _TransactionHash = value; }
        }

        private System.DateTime _CreateTime = System.DateTime.Now;
        public System.DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

        private System.String _UserMethod = System.String.Empty;
        public System.String UserMethod
        {
            get { return _UserMethod; }
            set { _UserMethod = value; }
        }

        private System.String _UserFrom = System.String.Empty;
        public System.String UserFrom
        {
            get { return _UserFrom; }
            set { _UserFrom = value; }
        }

        private System.String _UserRemark = System.String.Empty;
        public System.String UserRemark
        {
            get { return _UserRemark; }
            set { _UserRemark = value; }
        }

        private System.Int64 _TransactionIndex;
        public System.Int64 TransactionIndex
        {
            get { return _TransactionIndex; }
            set { _TransactionIndex = value; }
        }

        private System.Boolean _GotReceipt;
        public System.Boolean GotReceipt
        {
            get { return _GotReceipt; }
            set { _GotReceipt = value; }
        }

        private System.String _BlockHash = System.String.Empty;
        public System.String BlockHash
        {
            get { return _BlockHash; }
            set { _BlockHash = value; }
        }

        private System.Int64 _BlockNumber;
        public System.Int64 BlockNumber
        {
            get { return _BlockNumber; }
            set { _BlockNumber = value; }
        }

        private System.Int64 _CumulativeGasUsed;
        public System.Int64 CumulativeGasUsed
        {
            get { return _CumulativeGasUsed; }
            set { _CumulativeGasUsed = value; }
        }

        private System.Int64 _GasUsed;
        public System.Int64 GasUsed
        {
            get { return _GasUsed; }
            set { _GasUsed = value; }
        }

        private System.Double _GasPrice;
        public System.Double GasPrice
        {
            get { return _GasPrice; }
            set { _GasPrice = value; }
        }

        private System.String _ContractAddress = System.String.Empty;
        public System.String ContractAddress
        {
            get { return _ContractAddress; }
            set { _ContractAddress = value; }
        }

        private System.Int64 _Status;
        public System.Int64 Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private System.String _Logs = System.String.Empty;
        public System.String Logs
        {
            get { return _Logs; }
            set { _Logs = value; }
        }

        private System.Boolean _HasErrors;
        public System.Boolean HasErrors
        {
            get { return _HasErrors; }
            set { _HasErrors = value; }
        }

        private System.DateTime _ResultTime = System.DateTime.Now;
        public System.DateTime ResultTime
        {
            get { return _ResultTime; }
            set { _ResultTime = value; }
        }

        private System.Boolean _Canceled;
        public System.Boolean Canceled
        {
            get { return _Canceled; }
            set { _Canceled = value; }
        }

        #endregion

        #region Public construct

        public TransactionReceipt()
        {
        }


        public TransactionReceipt(System.Int32 AId)
        {
            _Id = AId;
        }

        #endregion

        #region Public DataRow2Object

        public static TransactionReceipt DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            TransactionReceipt Obj = new TransactionReceipt();
            Obj.Id = dr["Id"] == DBNull.Value ? 0 : (System.Int32)(dr["Id"]);
            Obj.TransactionHash = dr["TransactionHash"] == DBNull.Value ? string.Empty : (System.String)(dr["TransactionHash"]);
            Obj.CreateTime = dr["CreateTime"] == DBNull.Value ? System.DateTime.Now : (System.DateTime)(dr["CreateTime"]);
            Obj.UserMethod = dr["UserMethod"] == DBNull.Value ? string.Empty : (System.String)(dr["UserMethod"]);
            Obj.UserFrom = dr["UserFrom"] == DBNull.Value ? string.Empty : (System.String)(dr["UserFrom"]);
            Obj.UserRemark = dr["UserRemark"] == DBNull.Value ? string.Empty : (System.String)(dr["UserRemark"]);
            Obj.TransactionIndex = dr["TransactionIndex"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["TransactionIndex"]);
            Obj.GotReceipt = dr["GotReceipt"] == DBNull.Value ? false : (System.Boolean)(dr["GotReceipt"]);
            Obj.BlockHash = dr["BlockHash"] == DBNull.Value ? string.Empty : (System.String)(dr["BlockHash"]);
            Obj.BlockNumber = dr["BlockNumber"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["BlockNumber"]);
            Obj.CumulativeGasUsed = dr["CumulativeGasUsed"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["CumulativeGasUsed"]);
            Obj.GasUsed = dr["GasUsed"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["GasUsed"]);
            Obj.GasPrice = dr["GasPrice"] == DBNull.Value ? 0 : (System.Double)(dr["GasPrice"]);
            Obj.ContractAddress = dr["ContractAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["ContractAddress"]);
            Obj.Status = dr["Status"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["Status"]);
            Obj.Logs = dr["Logs"] == DBNull.Value ? string.Empty : (System.String)(dr["Logs"]);
            Obj.HasErrors = dr["HasErrors"] == DBNull.Value ? false : (System.Boolean)(dr["HasErrors"]);
            Obj.ResultTime = dr["ResultTime"] == DBNull.Value ? System.DateTime.Now : (System.DateTime)(dr["ResultTime"]);
            Obj.Canceled = dr["Canceled"] == DBNull.Value ? false : (System.Boolean)(dr["Canceled"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.TransactionHash != null && this.TransactionHash.Length > FL_TransactionHash && FL_TransactionHash > 0) throw new Exception("TransactionHash要求长度小于等于" + FL_TransactionHash.ToString() + "。");
            if (this.UserMethod != null && this.UserMethod.Length > FL_UserMethod && FL_UserMethod > 0) throw new Exception("UserMethod要求长度小于等于" + FL_UserMethod.ToString() + "。");
            if (this.UserFrom != null && this.UserFrom.Length > FL_UserFrom && FL_UserFrom > 0) throw new Exception("UserFrom要求长度小于等于" + FL_UserFrom.ToString() + "。");
            if (this.UserRemark != null && this.UserRemark.Length > FL_UserRemark && FL_UserRemark > 0) throw new Exception("UserRemark要求长度小于等于" + FL_UserRemark.ToString() + "。");
            if (this.BlockHash != null && this.BlockHash.Length > FL_BlockHash && FL_BlockHash > 0) throw new Exception("BlockHash要求长度小于等于" + FL_BlockHash.ToString() + "。");
            if (this.ContractAddress != null && this.ContractAddress.Length > FL_ContractAddress && FL_ContractAddress > 0) throw new Exception("ContractAddress要求长度小于等于" + FL_ContractAddress.ToString() + "。");
            if (this.Logs != null && this.Logs.Length > FL_Logs && FL_Logs > 0) throw new Exception("Logs要求长度小于等于" + FL_Logs.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.TransactionHash) || string.IsNullOrEmpty(this.TransactionHash.Trim())) throw new Exception("TransactionHash要求不空。");
            if (string.IsNullOrEmpty(this.UserMethod) || string.IsNullOrEmpty(this.UserMethod.Trim())) throw new Exception("UserMethod要求不空。");
            if (string.IsNullOrEmpty(this.UserFrom) || string.IsNullOrEmpty(this.UserFrom.Trim())) throw new Exception("UserFrom要求不空。");
            if (string.IsNullOrEmpty(this.UserRemark) || string.IsNullOrEmpty(this.UserRemark.Trim())) throw new Exception("UserRemark要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public TransactionReceipt Copy()
        {
            TransactionReceipt obj = new TransactionReceipt();
            obj.Id = this.Id;
            obj.TransactionHash = this.TransactionHash;
            obj.CreateTime = this.CreateTime;
            obj.UserMethod = this.UserMethod;
            obj.UserFrom = this.UserFrom;
            obj.UserRemark = this.UserRemark;
            obj.TransactionIndex = this.TransactionIndex;
            obj.GotReceipt = this.GotReceipt;
            obj.BlockHash = this.BlockHash;
            obj.BlockNumber = this.BlockNumber;
            obj.CumulativeGasUsed = this.CumulativeGasUsed;
            obj.GasUsed = this.GasUsed;
            obj.GasPrice = this.GasPrice;
            obj.ContractAddress = this.ContractAddress;
            obj.Status = this.Status;
            obj.Logs = this.Logs;
            obj.HasErrors = this.HasErrors;
            obj.ResultTime = this.ResultTime;
            obj.Canceled = this.Canceled;
            return obj;
        }



        public static List<TransactionReceipt> DataTable2List(System.Data.DataTable dt)
        {
            List<TransactionReceipt> result = new List<TransactionReceipt>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(TransactionReceipt.DataRow2Object(dr));
            }
            return result;
        }



        private static TransactionReceipt _NullObject = null;

        public static TransactionReceipt NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new TransactionReceipt();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 Id 的长度——4
        /// </summary>
        public const int FL_Id = 4;


        /// <summary>
        /// 字段 TransactionHash 的长度——66
        /// </summary>
        public const int FL_TransactionHash = 66;


        /// <summary>
        /// 字段 CreateTime 的长度——8
        /// </summary>
        public const int FL_CreateTime = 8;


        /// <summary>
        /// 字段 UserMethod 的长度——128
        /// </summary>
        public const int FL_UserMethod = 128;


        /// <summary>
        /// 字段 UserFrom 的长度——43
        /// </summary>
        public const int FL_UserFrom = 43;


        /// <summary>
        /// 字段 UserRemark 的长度——1024
        /// </summary>
        public const int FL_UserRemark = 1024;


        /// <summary>
        /// 字段 TransactionIndex 的长度——8
        /// </summary>
        public const int FL_TransactionIndex = 8;


        /// <summary>
        /// 字段 GotReceipt 的长度——1
        /// </summary>
        public const int FL_GotReceipt = 1;


        /// <summary>
        /// 字段 BlockHash 的长度——66
        /// </summary>
        public const int FL_BlockHash = 66;


        /// <summary>
        /// 字段 BlockNumber 的长度——8
        /// </summary>
        public const int FL_BlockNumber = 8;


        /// <summary>
        /// 字段 CumulativeGasUsed 的长度——8
        /// </summary>
        public const int FL_CumulativeGasUsed = 8;


        /// <summary>
        /// 字段 GasUsed 的长度——8
        /// </summary>
        public const int FL_GasUsed = 8;


        /// <summary>
        /// 字段 GasPrice 的长度——8
        /// </summary>
        public const int FL_GasPrice = 8;


        /// <summary>
        /// 字段 ContractAddress 的长度——43
        /// </summary>
        public const int FL_ContractAddress = 43;


        /// <summary>
        /// 字段 Status 的长度——8
        /// </summary>
        public const int FL_Status = 8;


        /// <summary>
        /// 字段 Logs 的长度——0
        /// </summary>
        public const int FL_Logs = 0;


        /// <summary>
        /// 字段 HasErrors 的长度——1
        /// </summary>
        public const int FL_HasErrors = 1;


        /// <summary>
        /// 字段 ResultTime 的长度——8
        /// </summary>
        public const int FL_ResultTime = 8;


        /// <summary>
        /// 字段 Canceled 的长度——1
        /// </summary>
        public const int FL_Canceled = 1;


        #endregion
    }



}