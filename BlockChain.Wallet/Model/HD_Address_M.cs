
//
//  本文件由代码工具自动生成 有修改
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.Wallet.Model
{
    [Serializable]
    public class HD_Address
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 生成该数据实体的SQL语句
        public const string SQL = @"Select Top(1) * From HD_Address";
        #endregion

        #region Public Properties

        private System.Guid _MneId;
        public System.Guid MneId
        {
            get { return _MneId; }
            set { _MneId = value; }
        }

        private System.String _MneSecondSalt = System.String.Empty;
        public System.String MneSecondSalt
        {
            get { return _MneSecondSalt; }
            set { _MneSecondSalt = value; }
        }

        private System.Int32 _AddressIndex;
        public System.Int32 AddressIndex
        {
            get { return _AddressIndex; }
            set { _AddressIndex = value; }
        }

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

        public HD_Address()
        {
        }


        public HD_Address(System.String AAddress)
        {
            _Address = AAddress;
        }

        #endregion

        #region Public DataRow2Object

        public static HD_Address DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            HD_Address Obj = new HD_Address();

            try
            {

                Obj.MneId = dr["MneId"] == DBNull.Value ? System.Guid.Empty : (System.Guid)(dr["MneId"]);
                Obj.MneSecondSalt = dr["MneSecondSalt"] == DBNull.Value ? string.Empty : (System.String)(dr["MneSecondSalt"]);
                Obj.AddressIndex = dr["AddressIndex"] == DBNull.Value ? 0 : (System.Int32)(dr["AddressIndex"]);
                Obj.AddressAlias = dr["AddressAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["AddressAlias"]);
                Obj.Address = dr["Address"] == DBNull.Value ? string.Empty : (System.String)(dr["Address"]);
                Obj.IsTxAddress = dr["IsTxAddress"] == DBNull.Value ? false : (System.Boolean)(dr["IsTxAddress"]);
                Obj.HasPrivatekey = dr["HasPrivatekey"] == DBNull.Value ? false : (System.Boolean)(dr["HasPrivatekey"]);
                return Obj;
            }
            catch (Exception ex) //为了兼容以前版本（无格式版本！）
            { 
                log.Error("", ex);

                try
                {
                    Obj.MneId = System.Guid.Parse(dr["MneId"].ToString());
                    Obj.MneSecondSalt = dr["MneSecondSalt"].ToString();
                    Obj.AddressIndex = System.Int32.Parse(dr["AddressIndex"].ToString());
                    Obj.AddressAlias = dr["AddressAlias"].ToString();
                    Obj.Address = dr["Address"].ToString();
                    Obj.IsTxAddress = System.Boolean.Parse(dr["IsTxAddress"].ToString());
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
            if (this.MneSecondSalt != null && this.MneSecondSalt.Length > FL_MneSecondSalt && FL_MneSecondSalt > 0) throw new Exception("MneSecondSalt要求长度小于等于" + FL_MneSecondSalt.ToString() + "。");
            if (this.AddressAlias != null && this.AddressAlias.Length > FL_AddressAlias && FL_AddressAlias > 0) throw new Exception("AddressAlias要求长度小于等于" + FL_AddressAlias.ToString() + "。");
            if (this.Address != null && this.Address.Length > FL_Address && FL_Address > 0) throw new Exception("Address要求长度小于等于" + FL_Address.ToString() + "。");
        }


        public void ValidateNotEmpty()
        {
            if (string.IsNullOrEmpty(this.Address) || string.IsNullOrEmpty(this.Address.Trim())) throw new Exception("Address要求不空。");
        }



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


        public HD_Address Copy()
        {
            HD_Address obj = new HD_Address();
            obj.MneId = this.MneId;
            obj.MneSecondSalt = this.MneSecondSalt;
            obj.AddressIndex = this.AddressIndex;
            obj.AddressAlias = this.AddressAlias;
            obj.Address = this.Address;
            obj.IsTxAddress = this.IsTxAddress;
            obj.HasPrivatekey = this.HasPrivatekey;
            return obj;
        }



        public static List<HD_Address> DataTable2List(System.Data.DataTable dt)
        {
            List<HD_Address> result = new List<HD_Address>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(HD_Address.DataRow2Object(dr));
            }
            return result;
        }



        private static HD_Address _NullObject = null;

        public static HD_Address NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new HD_Address();
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
        /// 字段 MneSecondSalt 的长度——64
        /// </summary>
        public const int FL_MneSecondSalt = 64;


        /// <summary>
        /// 字段 AddressIndex 的长度——4
        /// </summary>
        public const int FL_AddressIndex = 4;


        /// <summary>
        /// 字段 AddressAlias 的长度——64
        /// </summary>
        public const int FL_AddressAlias = 64;


        /// <summary>
        /// 字段 Address 的长度——43
        /// </summary>
        public const int FL_Address = 43;


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