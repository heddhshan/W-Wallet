
//
//  本文件由代码工具自动生成 但有修改！
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Wallet.Model
{
    [Serializable]
    public class HD_Mnemonic
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From HD_Mnemonic";
        #endregion

        #region Public Properties

        private System.Guid _MneId;
        public System.Guid MneId
        {
            get { return _MneId; }
            set { _MneId = value; }
        }

        private System.String _MneAlias = System.String.Empty;
        public System.String MneAlias
        {
            get { return _MneAlias; }
            set { _MneAlias = value; }
        }

        private System.String _MneEncrypted = System.String.Empty;
        public System.String MneEncrypted
        {
            get { return _MneEncrypted; }
            set { _MneEncrypted = value; }
        }

        private System.Int32 _EncryptedTimes;
        public System.Int32 EncryptedTimes
        {
            get { return _EncryptedTimes; }
            set { _EncryptedTimes = value; }
        }

        private System.String _MneHash = System.String.Empty;
        public System.String MneHash
        {
            get { return _MneHash; }
            set { _MneHash = value; }
        }

        private System.Int32 _WordCount;
        public System.Int32 WordCount
        {
            get { return _WordCount; }
            set { _WordCount = value; }
        }

        private System.String _MnePath = System.String.Empty;
        public System.String MnePath
        {
            get { return _MnePath; }
            set { _MnePath = value; }
        }

        private System.String _Salt = System.String.Empty;
        public System.String Salt
        {
            get { return _Salt; }
            set { _Salt = value; }
        }

        private System.String _UserPasswordHash = System.String.Empty;
        public System.String UserPasswordHash
        {
            get { return _UserPasswordHash; }
            set { _UserPasswordHash = value; }
        }

        private System.String _UserPasswordTip = System.String.Empty;
        public System.String UserPasswordTip
        {
            get { return _UserPasswordTip; }
            set { _UserPasswordTip = value; }
        }

        private System.String _MneFirstSalt = System.String.Empty;
        public System.String MneFirstSalt
        {
            get { return _MneFirstSalt; }
            set { _MneFirstSalt = value; }
        }

        private System.Int32 _MneSource;
        public System.Int32 MneSource
        {
            get { return _MneSource; }
            set { _MneSource = value; }
        }

        private System.Boolean _IsBackUp;
        public System.Boolean IsBackUp
        {
            get { return _IsBackUp; }
            set { _IsBackUp = value; }
        }

        private System.DateTime _CreateTime = System.DateTime.Now;
        public System.DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

        private System.Boolean _HasPrivatekey;
        public System.Boolean HasPrivatekey
        {
            get { return _HasPrivatekey; }
            set { _HasPrivatekey = value; }
        }

        #endregion

        #region Public construct

        public HD_Mnemonic()
        {
        }


        public HD_Mnemonic(System.Guid AMneId)
        {
            _MneId = AMneId;
        }

        #endregion

        #region Public DataRow2Object

        public static HD_Mnemonic DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            HD_Mnemonic Obj = new HD_Mnemonic();
            try
            {
                Obj.MneId = dr["MneId"] == DBNull.Value ? System.Guid.Empty : (System.Guid)(dr["MneId"]);
                Obj.MneAlias = dr["MneAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["MneAlias"]);
                Obj.MneEncrypted = dr["MneEncrypted"] == DBNull.Value ? string.Empty : (System.String)(dr["MneEncrypted"]);
                Obj.EncryptedTimes = dr["EncryptedTimes"] == DBNull.Value ? 0 : (System.Int32)(dr["EncryptedTimes"]);
                Obj.MneHash = dr["MneHash"] == DBNull.Value ? string.Empty : (System.String)(dr["MneHash"]);
                Obj.WordCount = dr["WordCount"] == DBNull.Value ? 0 : (System.Int32)(dr["WordCount"]);
                Obj.MnePath = dr["MnePath"] == DBNull.Value ? string.Empty : (System.String)(dr["MnePath"]);
                Obj.Salt = dr["Salt"] == DBNull.Value ? string.Empty : (System.String)(dr["Salt"]);
                Obj.UserPasswordHash = dr["UserPasswordHash"] == DBNull.Value ? string.Empty : (System.String)(dr["UserPasswordHash"]);
                Obj.UserPasswordTip = dr["UserPasswordTip"] == DBNull.Value ? string.Empty : (System.String)(dr["UserPasswordTip"]);
                Obj.MneFirstSalt = dr["MneFirstSalt"] == DBNull.Value ? string.Empty : (System.String)(dr["MneFirstSalt"]);
                Obj.MneSource = dr["MneSource"] == DBNull.Value ? 0 : (System.Int32)(dr["MneSource"]);
                Obj.IsBackUp = dr["IsBackUp"] == DBNull.Value ? false : (System.Boolean)(dr["IsBackUp"]);
                Obj.CreateTime = dr["CreateTime"] == DBNull.Value ? System.DateTime.Now : (System.DateTime)(dr["CreateTime"]);
                Obj.HasPrivatekey = dr["HasPrivatekey"] == DBNull.Value ? false : (System.Boolean)(dr["HasPrivatekey"]);
                return Obj;
            }
            catch (Exception ex)        //修改部分，为了兼容以前的格式！
            {
                log.Error("", ex);
                try
                {
                    Obj.MneId = System.Guid.Parse(dr["MneId"].ToString());
                    Obj.MneAlias = dr["MneAlias"].ToString();
                    Obj.MneEncrypted = dr["MneEncrypted"].ToString();
                    Obj.EncryptedTimes = System.Int32.Parse(dr["EncryptedTimes"].ToString());
                    Obj.MneHash = dr["MneHash"].ToString();
                    Obj.WordCount = System.Int32.Parse(dr["WordCount"].ToString());
                    Obj.MnePath = dr["MnePath"].ToString();
                    Obj.Salt = dr["Salt"].ToString();
                    Obj.UserPasswordHash = dr["UserPasswordHash"].ToString();
                    Obj.UserPasswordTip = dr["UserPasswordTip"].ToString();
                    Obj.MneFirstSalt = dr["MneFirstSalt"].ToString();
                    Obj.MneSource = System.Int32.Parse(dr["MneSource"].ToString());
                    Obj.IsBackUp = dr["IsBackUp"] != null && System.Boolean.Parse(dr["IsBackUp"].ToString());
                    Obj.CreateTime = System.DateTime.Parse(dr["CreateTime"].ToString());
                    Obj.HasPrivatekey = dr["HasPrivatekey"] != DBNull.Value && System.Boolean.Parse(dr["HasPrivatekey"].ToString());
                }
                catch (Exception ex1)
                {
                    log.Error("", ex1);
                }
                return Obj;
            }
        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.MneAlias != null && this.MneAlias.Length > FL_MneAlias && FL_MneAlias > 0) throw new Exception("MneAlias要求长度小于等于" + FL_MneAlias.ToString() + "。");
            if (this.MneEncrypted != null && this.MneEncrypted.Length > FL_MneEncrypted && FL_MneEncrypted > 0) throw new Exception("MneEncrypted要求长度小于等于" + FL_MneEncrypted.ToString() + "。");
            if (this.MneHash != null && this.MneHash.Length > FL_MneHash && FL_MneHash > 0) throw new Exception("MneHash要求长度小于等于" + FL_MneHash.ToString() + "。");
            if (this.MnePath != null && this.MnePath.Length > FL_MnePath && FL_MnePath > 0) throw new Exception("MnePath要求长度小于等于" + FL_MnePath.ToString() + "。");
            if (this.Salt != null && this.Salt.Length > FL_Salt && FL_Salt > 0) throw new Exception("Salt要求长度小于等于" + FL_Salt.ToString() + "。");
            if (this.UserPasswordHash != null && this.UserPasswordHash.Length > FL_UserPasswordHash && FL_UserPasswordHash > 0) throw new Exception("UserPasswordHash要求长度小于等于" + FL_UserPasswordHash.ToString() + "。");
            if (this.UserPasswordTip != null && this.UserPasswordTip.Length > FL_UserPasswordTip && FL_UserPasswordTip > 0) throw new Exception("UserPasswordTip要求长度小于等于" + FL_UserPasswordTip.ToString() + "。");
            if (this.MneFirstSalt != null && this.MneFirstSalt.Length > FL_MneFirstSalt && FL_MneFirstSalt > 0) throw new Exception("MneFirstSalt要求长度小于等于" + FL_MneFirstSalt.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.MneAlias) || string.IsNullOrEmpty(this.MneAlias.Trim())) throw new Exception("MneAlias要求不空。");
            if (string.IsNullOrEmpty(this.MneEncrypted) || string.IsNullOrEmpty(this.MneEncrypted.Trim())) throw new Exception("MneEncrypted要求不空。");
            if (string.IsNullOrEmpty(this.MneHash) || string.IsNullOrEmpty(this.MneHash.Trim())) throw new Exception("MneHash要求不空。");
            if (string.IsNullOrEmpty(this.MnePath) || string.IsNullOrEmpty(this.MnePath.Trim())) throw new Exception("MnePath要求不空。");
            if (string.IsNullOrEmpty(this.Salt) || string.IsNullOrEmpty(this.Salt.Trim())) throw new Exception("Salt要求不空。");
            if (string.IsNullOrEmpty(this.UserPasswordHash) || string.IsNullOrEmpty(this.UserPasswordHash.Trim())) throw new Exception("UserPasswordHash要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public HD_Mnemonic Copy()
        {
            HD_Mnemonic obj = new HD_Mnemonic();
            obj.MneId = this.MneId;
            obj.MneAlias = this.MneAlias;
            obj.MneEncrypted = this.MneEncrypted;
            obj.EncryptedTimes = this.EncryptedTimes;
            obj.MneHash = this.MneHash;
            obj.WordCount = this.WordCount;
            obj.MnePath = this.MnePath;
            obj.Salt = this.Salt;
            obj.UserPasswordHash = this.UserPasswordHash;
            obj.UserPasswordTip = this.UserPasswordTip;
            obj.MneFirstSalt = this.MneFirstSalt;
            obj.MneSource = this.MneSource;
            obj.IsBackUp = this.IsBackUp;
            obj.CreateTime = this.CreateTime;
            obj.HasPrivatekey = this.HasPrivatekey;
            return obj;
        }



        public static List<HD_Mnemonic> DataTable2List(System.Data.DataTable dt)
        {
            List<HD_Mnemonic> result = new List<HD_Mnemonic>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(HD_Mnemonic.DataRow2Object(dr));
            }
            return result;
        }



        private static HD_Mnemonic _NullObject = null;

        public static HD_Mnemonic NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new HD_Mnemonic();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 MneId 的长度——16
        /// </summary>
        public const int FL_MneId = 16;


        /// <summary>
        /// 字段 MneAlias 的长度——64
        /// </summary>
        public const int FL_MneAlias = 64;


        /// <summary>
        /// 字段 MneEncrypted 的长度——1024
        /// </summary>
        public const int FL_MneEncrypted = 1024;


        /// <summary>
        /// 字段 EncryptedTimes 的长度——4
        /// </summary>
        public const int FL_EncryptedTimes = 4;


        /// <summary>
        /// 字段 MneHash 的长度——64
        /// </summary>
        public const int FL_MneHash = 64;


        /// <summary>
        /// 字段 WordCount 的长度——4
        /// </summary>
        public const int FL_WordCount = 4;


        /// <summary>
        /// 字段 MnePath 的长度——32
        /// </summary>
        public const int FL_MnePath = 32;


        /// <summary>
        /// 字段 Salt 的长度——64
        /// </summary>
        public const int FL_Salt = 64;


        /// <summary>
        /// 字段 UserPasswordHash 的长度——64
        /// </summary>
        public const int FL_UserPasswordHash = 64;


        /// <summary>
        /// 字段 UserPasswordTip 的长度——64
        /// </summary>
        public const int FL_UserPasswordTip = 64;


        /// <summary>
        /// 字段 MneFirstSalt 的长度——128
        /// </summary>
        public const int FL_MneFirstSalt = 128;


        /// <summary>
        /// 字段 MneSource 的长度——4
        /// </summary>
        public const int FL_MneSource = 4;


        /// <summary>
        /// 字段 IsBackUp 的长度——1
        /// </summary>
        public const int FL_IsBackUp = 1;


        /// <summary>
        /// 字段 CreateTime 的长度——8
        /// </summary>
        public const int FL_CreateTime = 8;


        /// <summary>
        /// 字段 HasPrivatekey 的长度——1
        /// </summary>
        public const int FL_HasPrivatekey = 1;


        #endregion
    }



}