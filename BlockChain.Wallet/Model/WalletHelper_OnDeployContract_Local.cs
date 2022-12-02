
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 10:47:12
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Wallet.Model
{
    [Serializable]
    public class WalletHelper_OnDeployContract_Local
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From WalletHelper_OnDeployContract_Local";
        #endregion

        #region Public Properties

        private System.String _ContractAddress = System.String.Empty;
        public System.String ContractAddress
        {
            get { return _ContractAddress; }
            set { _ContractAddress = value; }
        }

        private System.String _TransactionHash = System.String.Empty;
        public System.String TransactionHash
        {
            get { return _TransactionHash; }
            set { _TransactionHash = value; }
        }

        private System.Int32 _ChainId;
        public System.Int32 ChainId
        {
            get { return _ChainId; }
            set { _ChainId = value; }
        }

        private System.String __bytecode = System.String.Empty;
        public System.String _bytecode
        {
            get { return __bytecode; }
            set { __bytecode = value; }
        }

        private System.String _LocalRemark = System.String.Empty;
        public System.String LocalRemark
        {
            get { return _LocalRemark; }
            set { _LocalRemark = value; }
        }

        #endregion

        #region Public construct

        public WalletHelper_OnDeployContract_Local()
        {
        }


        public WalletHelper_OnDeployContract_Local(System.String AContractAddress, System.String ATransactionHash, System.Int32 AChainId)
        {
            _ContractAddress = AContractAddress;
            _TransactionHash = ATransactionHash;
            _ChainId = AChainId;
        }

        #endregion

        #region Public DataRow2Object

        public static WalletHelper_OnDeployContract_Local DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            WalletHelper_OnDeployContract_Local Obj = new WalletHelper_OnDeployContract_Local();
            Obj.ContractAddress = dr["ContractAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["ContractAddress"]);
            Obj.TransactionHash = dr["TransactionHash"] == DBNull.Value ? string.Empty : (System.String)(dr["TransactionHash"]);
            Obj.ChainId = dr["ChainId"] == DBNull.Value ? 0 : (System.Int32)(dr["ChainId"]);
            Obj._bytecode = dr["_bytecode"] == DBNull.Value ? string.Empty : (System.String)(dr["_bytecode"]);
            Obj.LocalRemark = dr["LocalRemark"] == DBNull.Value ? string.Empty : (System.String)(dr["LocalRemark"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.ContractAddress != null && this.ContractAddress.Length > FL_ContractAddress && FL_ContractAddress > 0) throw new Exception("ContractAddress要求长度小于等于" + FL_ContractAddress.ToString() + "。");
            if (this.TransactionHash != null && this.TransactionHash.Length > FL_TransactionHash && FL_TransactionHash > 0) throw new Exception("TransactionHash要求长度小于等于" + FL_TransactionHash.ToString() + "。");
            if (this._bytecode != null && this._bytecode.Length > FL__bytecode && FL__bytecode > 0) throw new Exception("_bytecode要求长度小于等于" + FL__bytecode.ToString() + "。");
            if (this.LocalRemark != null && this.LocalRemark.Length > FL_LocalRemark && FL_LocalRemark > 0) throw new Exception("LocalRemark要求长度小于等于" + FL_LocalRemark.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.ContractAddress) || string.IsNullOrEmpty(this.ContractAddress.Trim())) throw new Exception("ContractAddress要求不空。");
            if (string.IsNullOrEmpty(this.TransactionHash) || string.IsNullOrEmpty(this.TransactionHash.Trim())) throw new Exception("TransactionHash要求不空。");
            if (string.IsNullOrEmpty(this._bytecode) || string.IsNullOrEmpty(this._bytecode.Trim())) throw new Exception("_bytecode要求不空。");
            if (string.IsNullOrEmpty(this.LocalRemark) || string.IsNullOrEmpty(this.LocalRemark.Trim())) throw new Exception("LocalRemark要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public WalletHelper_OnDeployContract_Local Copy()
        {
            WalletHelper_OnDeployContract_Local obj = new WalletHelper_OnDeployContract_Local();
            obj.ContractAddress = this.ContractAddress;
            obj.TransactionHash = this.TransactionHash;
            obj.ChainId = this.ChainId;
            obj._bytecode = this._bytecode;
            obj.LocalRemark = this.LocalRemark;
            return obj;
        }



        public static List<WalletHelper_OnDeployContract_Local> DataTable2List(System.Data.DataTable dt)
        {
            List<WalletHelper_OnDeployContract_Local> result = new List<WalletHelper_OnDeployContract_Local>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(WalletHelper_OnDeployContract_Local.DataRow2Object(dr));
            }
            return result;
        }



        private static WalletHelper_OnDeployContract_Local _NullObject = null;

        public static WalletHelper_OnDeployContract_Local NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new WalletHelper_OnDeployContract_Local();
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
        /// 字段 TransactionHash 的长度——66
        /// </summary>
        public const int FL_TransactionHash = 66;


        /// <summary>
        /// 字段 ChainId 的长度——4
        /// </summary>
        public const int FL_ChainId = 4;


        /// <summary>
        /// 字段 _bytecode 的长度——0
        /// </summary>
        public const int FL__bytecode = 0;


        /// <summary>
        /// 字段 LocalRemark 的长度——128
        /// </summary>
        public const int FL_LocalRemark = 128;


        #endregion
    }



}