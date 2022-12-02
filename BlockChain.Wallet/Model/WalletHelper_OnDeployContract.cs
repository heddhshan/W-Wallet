
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/3/16 10:47:12
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Wallet.Model
{
    [Serializable]
    public class WalletHelper_OnDeployContract
    {
        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From WalletHelper_OnDeployContract";
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

        private System.Int32 _ChainId;
        public System.Int32 ChainId
        {
            get { return _ChainId; }
            set { _ChainId = value; }
        }

        private System.String __contract = System.String.Empty;
        public System.String _contract
        {
            get { return __contract; }
            set { __contract = value; }
        }

        private System.String __user = System.String.Empty;
        public System.String _user
        {
            get { return __user; }
            set { __user = value; }
        }

        private System.Decimal __amount;
        public System.Decimal _amount
        {
            get { return __amount; }
            set { __amount = value; }
        }

        private System.String __salt = System.String.Empty;
        public System.String _salt
        {
            get { return __salt; }
            set { __salt = value; }
        }

        private System.String __bytecodeHash = System.String.Empty;
        public System.String _bytecodeHash
        {
            get { return __bytecodeHash; }
            set { __bytecodeHash = value; }
        }

        private System.String __amount_Text = System.String.Empty;
        public System.String _amount_Text
        {
            get { return __amount_Text; }
            set { __amount_Text = value; }
        }

        #endregion

        #region Public construct

        public WalletHelper_OnDeployContract()
        {
        }


        public WalletHelper_OnDeployContract(System.Int32 AChainId, System.String A_contract)
        {
            _ChainId = AChainId;
            __contract = A_contract;
        }

        #endregion

        #region Public DataRow2Object

        public static WalletHelper_OnDeployContract DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            WalletHelper_OnDeployContract Obj = new WalletHelper_OnDeployContract();
            Obj.ContractAddress = dr["ContractAddress"] == DBNull.Value ? string.Empty : (System.String)(dr["ContractAddress"]);
            Obj.BlockNumber = dr["BlockNumber"] == DBNull.Value ? Convert.ToInt64(0) : (System.Int64)(dr["BlockNumber"]);
            Obj.TransactionHash = dr["TransactionHash"] == DBNull.Value ? string.Empty : (System.String)(dr["TransactionHash"]);
            Obj.ChainId = dr["ChainId"] == DBNull.Value ? 0 : (System.Int32)(dr["ChainId"]);
            Obj._contract = dr["_contract"] == DBNull.Value ? string.Empty : (System.String)(dr["_contract"]);
            Obj._user = dr["_user"] == DBNull.Value ? string.Empty : (System.String)(dr["_user"]);
            Obj._amount = dr["_amount"] == DBNull.Value ? 0 : (System.Decimal)(dr["_amount"]);
            Obj._salt = dr["_salt"] == DBNull.Value ? string.Empty : (System.String)(dr["_salt"]);
            Obj._bytecodeHash = dr["_bytecodeHash"] == DBNull.Value ? string.Empty : (System.String)(dr["_bytecodeHash"]);
            Obj._amount_Text = dr["_amount_Text"] == DBNull.Value ? string.Empty : (System.String)(dr["_amount_Text"]);
            return Obj;
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.ContractAddress != null && this.ContractAddress.Length > FL_ContractAddress && FL_ContractAddress > 0) throw new Exception("ContractAddress要求长度小于等于" + FL_ContractAddress.ToString() + "。");
            if (this.TransactionHash != null && this.TransactionHash.Length > FL_TransactionHash && FL_TransactionHash > 0) throw new Exception("TransactionHash要求长度小于等于" + FL_TransactionHash.ToString() + "。");
            if (this._contract != null && this._contract.Length > FL__contract && FL__contract > 0) throw new Exception("_contract要求长度小于等于" + FL__contract.ToString() + "。");
            if (this._user != null && this._user.Length > FL__user && FL__user > 0) throw new Exception("_user要求长度小于等于" + FL__user.ToString() + "。");
            if (this._salt != null && this._salt.Length > FL__salt && FL__salt > 0) throw new Exception("_salt要求长度小于等于" + FL__salt.ToString() + "。");
            if (this._bytecodeHash != null && this._bytecodeHash.Length > FL__bytecodeHash && FL__bytecodeHash > 0) throw new Exception("_bytecodeHash要求长度小于等于" + FL__bytecodeHash.ToString() + "。");
            if (this._amount_Text != null && this._amount_Text.Length > FL__amount_Text && FL__amount_Text > 0) throw new Exception("_amount_Text要求长度小于等于" + FL__amount_Text.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.ContractAddress) || string.IsNullOrEmpty(this.ContractAddress.Trim())) throw new Exception("ContractAddress要求不空。");
            if (string.IsNullOrEmpty(this.TransactionHash) || string.IsNullOrEmpty(this.TransactionHash.Trim())) throw new Exception("TransactionHash要求不空。");
            if (string.IsNullOrEmpty(this._contract) || string.IsNullOrEmpty(this._contract.Trim())) throw new Exception("_contract要求不空。");
            if (string.IsNullOrEmpty(this._user) || string.IsNullOrEmpty(this._user.Trim())) throw new Exception("_user要求不空。");
            if (string.IsNullOrEmpty(this._salt) || string.IsNullOrEmpty(this._salt.Trim())) throw new Exception("_salt要求不空。");
            if (string.IsNullOrEmpty(this._bytecodeHash) || string.IsNullOrEmpty(this._bytecodeHash.Trim())) throw new Exception("_bytecodeHash要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public WalletHelper_OnDeployContract Copy()
        {
            WalletHelper_OnDeployContract obj = new WalletHelper_OnDeployContract();
            obj.ContractAddress = this.ContractAddress;
            obj.BlockNumber = this.BlockNumber;
            obj.TransactionHash = this.TransactionHash;
            obj.ChainId = this.ChainId;
            obj._contract = this._contract;
            obj._user = this._user;
            obj._amount = this._amount;
            obj._salt = this._salt;
            obj._bytecodeHash = this._bytecodeHash;
            obj._amount_Text = this._amount_Text;
            return obj;
        }



        public static List<WalletHelper_OnDeployContract> DataTable2List(System.Data.DataTable dt)
        {
            List<WalletHelper_OnDeployContract> result = new List<WalletHelper_OnDeployContract>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(WalletHelper_OnDeployContract.DataRow2Object(dr));
            }
            return result;
        }



        private static WalletHelper_OnDeployContract _NullObject = null;

        public static WalletHelper_OnDeployContract NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new WalletHelper_OnDeployContract();
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
        /// 字段 ChainId 的长度——4
        /// </summary>
        public const int FL_ChainId = 4;


        /// <summary>
        /// 字段 _contract 的长度——43
        /// </summary>
        public const int FL__contract = 43;


        /// <summary>
        /// 字段 _user 的长度——43
        /// </summary>
        public const int FL__user = 43;


        /// <summary>
        /// 字段 _amount 的长度——17
        /// </summary>
        public const int FL__amount = 17;


        /// <summary>
        /// 字段 _salt 的长度——66
        /// </summary>
        public const int FL__salt = 66;


        /// <summary>
        /// 字段 _bytecodeHash 的长度——66
        /// </summary>
        public const int FL__bytecodeHash = 66;


        /// <summary>
        /// 字段 _amount_Text 的长度——80
        /// </summary>
        public const int FL__amount_Text = 80;


        #endregion
    }



}