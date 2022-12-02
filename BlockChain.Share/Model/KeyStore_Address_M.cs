

using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Share.Model
{
    [Serializable]
    public class KeyStore_Address
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From KeyStore_Address";
        #endregion

        #region Public Properties

        private System.String _AddressAlias = System.String.Empty;
        public System.String AddressAlias
        {
            get { return _AddressAlias; }
            set { _AddressAlias = value; }
        }

        private System.String _Address = System.String.Empty;
        public System.String Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        private System.String _FilePath = System.String.Empty;
        public System.String FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        private System.String _KeyStoreText = System.String.Empty;
        public System.String KeyStoreText
        {
            get { return _KeyStoreText; }
            set { _KeyStoreText = value; }
        }

        private System.Boolean _IsTxAddress;
        public System.Boolean IsTxAddress
        {
            get { return _IsTxAddress; }
            set { _IsTxAddress = value; }
        }

        private System.Boolean _HasPrivatekey;
        public System.Boolean HasPrivatekey
        {
            get { return _HasPrivatekey; }
            set { _HasPrivatekey = value; }
        }

        #endregion

        #region Public construct

        public KeyStore_Address()
        {
        }


        public KeyStore_Address(System.String AAddress)
        {
            _Address = AAddress;
        }

        #endregion

        #region Public DataRow2Object

        public static KeyStore_Address DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            KeyStore_Address Obj = new KeyStore_Address();

            try
            {
                Obj.AddressAlias = dr["AddressAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["AddressAlias"]);
                Obj.Address = dr["Address"] == DBNull.Value ? string.Empty : (System.String)(dr["Address"]);
                Obj.FilePath = dr["FilePath"] == DBNull.Value ? string.Empty : (System.String)(dr["FilePath"]);
                Obj.KeyStoreText = dr["KeyStoreText"] == DBNull.Value ? string.Empty : (System.String)(dr["KeyStoreText"]);
                Obj.IsTxAddress = dr["IsTxAddress"] == DBNull.Value ? false : (System.Boolean)(dr["IsTxAddress"]);
                Obj.HasPrivatekey = dr["HasPrivatekey"] == DBNull.Value ? false : (System.Boolean)(dr["HasPrivatekey"]);
                return Obj;

            }
            catch (Exception ex)        //为了兼容以前版本（无格式版本！）
            {
                log.Error("", ex);

                try
                {
                    Obj.AddressAlias = dr["AddressAlias"].ToString();
                    Obj.Address = dr["Address"].ToString();
                    Obj.FilePath = dr["FilePath"].ToString();
                    Obj.KeyStoreText = dr["KeyStoreText"].ToString();
                    Obj.IsTxAddress = System.Boolean.Parse(dr["IsTxAddress"].ToString());
                    Obj.HasPrivatekey = true;   // dr["HasPrivatekey"] != DBNull.Value && System.Boolean.Parse(dr["HasPrivatekey"].ToString());
                }
                catch (Exception ex1) {
                    log.Error("", ex1); 
                }
                return Obj;
            }

        }

        #endregion


        public void ValidateDataLen()
        {
            if (this.AddressAlias != null && this.AddressAlias.Length > FL_AddressAlias && FL_AddressAlias > 0) throw new Exception("AddressAlias要求长度小于等于" + FL_AddressAlias.ToString() + "。");
            if (this.Address != null && this.Address.Length > FL_Address && FL_Address > 0) throw new Exception("Address要求长度小于等于" + FL_Address.ToString() + "。");
            if (this.FilePath != null && this.FilePath.Length > FL_FilePath && FL_FilePath > 0) throw new Exception("FilePath要求长度小于等于" + FL_FilePath.ToString() + "。");
            if (this.KeyStoreText != null && this.KeyStoreText.Length > FL_KeyStoreText && FL_KeyStoreText > 0) throw new Exception("KeyStoreText要求长度小于等于" + FL_KeyStoreText.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.Address) || string.IsNullOrEmpty(this.Address.Trim())) throw new Exception("Address要求不空。");
            if (string.IsNullOrEmpty(this.FilePath) || string.IsNullOrEmpty(this.FilePath.Trim())) throw new Exception("FilePath要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public KeyStore_Address Copy()
        {
            KeyStore_Address obj = new KeyStore_Address();
            obj.AddressAlias = this.AddressAlias;
            obj.Address = this.Address;
            obj.FilePath = this.FilePath;
            obj.KeyStoreText = this.KeyStoreText;
            obj.IsTxAddress = this.IsTxAddress;
            obj.HasPrivatekey = this.HasPrivatekey;
            return obj;
        }



        public static List<KeyStore_Address> DataTable2List(System.Data.DataTable dt)
        {
            List<KeyStore_Address> result = new List<KeyStore_Address>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(KeyStore_Address.DataRow2Object(dr));
            }
            return result;
        }



        private static KeyStore_Address _NullObject = null;

        public static KeyStore_Address NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new KeyStore_Address();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 AddressAlias 的长度——64
        /// </summary>
        public const int FL_AddressAlias = 64;


        /// <summary>
        /// 字段 Address 的长度——43
        /// </summary>
        public const int FL_Address = 43;


        /// <summary>
        /// 字段 FilePath 的长度——2048
        /// </summary>
        public const int FL_FilePath = 2048;


        /// <summary>
        /// 字段 KeyStoreText 的长度——2048
        /// </summary>
        public const int FL_KeyStoreText = 2048;


        /// <summary>
        /// 字段 IsTxAddress 的长度——1
        /// </summary>
        public const int FL_IsTxAddress = 1;


        /// <summary>
        /// 字段 HasPrivatekey 的长度——1
        /// </summary>
        public const int FL_HasPrivatekey = 1;


        #endregion
    }



}