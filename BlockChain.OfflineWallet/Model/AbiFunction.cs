
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/9/23 21:33:12
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.OfflineWallet.Model
{
[Serializable]
public class AbiFunction
{
    #region 生成该数据实体的SQL语句
    public const string SQL = @"Select Top(1) * From AbiFunction";
    #endregion

    #region Public Properties

    private System.Guid _FunId;
    public System.Guid FunId
    {
        get {return _FunId;}
        set {_FunId = value;}
    }

    private System.String _FunctionFullName = System.String.Empty;
    public System.String FunctionFullName
    {
        get {return _FunctionFullName;}
        set {_FunctionFullName = value;}
    }

    private System.String _Remark = System.String.Empty;
    public System.String Remark
    {
        get {return _Remark;}
        set {_Remark = value;}
    }

    private System.String _FunctionFullNameHash = System.String.Empty;
    public System.String FunctionFullNameHash
    {
        get {return _FunctionFullNameHash;}
        set {_FunctionFullNameHash = value;}
    }

    private System.String _FunctionFullNameHash4 = System.String.Empty;
    public System.String FunctionFullNameHash4
    {
        get {return _FunctionFullNameHash4;}
        set {_FunctionFullNameHash4 = value;}
    }

    private System.Boolean _IsSysDefine;
    public System.Boolean IsSysDefine
    {
        get {return _IsSysDefine;}
        set {_IsSysDefine = value;}
    }

    private System.Boolean _IsTestOk;
    public System.Boolean IsTestOk
    {
        get {return _IsTestOk;}
        set {_IsTestOk = value;}
    }

    private System.Boolean _IsEthTransfer;
    public System.Boolean IsEthTransfer
    {
        get {return _IsEthTransfer;}
        set {_IsEthTransfer = value;}
    }

    #endregion

    #region Public construct

    public AbiFunction()
    {
    }

    
    public AbiFunction(System.Guid AFunId)
    {
        _FunId = AFunId;
    }

    #endregion

    #region Public DataRow2Object

    public static AbiFunction DataRow2Object(System.Data.DataRow dr)
    {
        if (dr == null)
        {
            return null;
        }
        AbiFunction Obj = new AbiFunction();
        Obj.FunId = dr["FunId"] == DBNull.Value ? System.Guid.Empty : (System.Guid)(dr["FunId"]);
        Obj.FunctionFullName = dr["FunctionFullName"] == DBNull.Value ? string.Empty : (System.String)(dr["FunctionFullName"]);
        Obj.Remark = dr["Remark"] == DBNull.Value ? string.Empty : (System.String)(dr["Remark"]);
        Obj.FunctionFullNameHash = dr["FunctionFullNameHash"] == DBNull.Value ? string.Empty : (System.String)(dr["FunctionFullNameHash"]);
        Obj.FunctionFullNameHash4 = dr["FunctionFullNameHash4"] == DBNull.Value ? string.Empty : (System.String)(dr["FunctionFullNameHash4"]);
        Obj.IsSysDefine = dr["IsSysDefine"] == DBNull.Value ? false : (System.Boolean)(dr["IsSysDefine"]);
        Obj.IsTestOk = dr["IsTestOk"] == DBNull.Value ? false : (System.Boolean)(dr["IsTestOk"]);
        Obj.IsEthTransfer = dr["IsEthTransfer"] == DBNull.Value ? false : (System.Boolean)(dr["IsEthTransfer"]);
        return Obj;
    }

    #endregion


        public void ValidateDataLen()
        {
if (this.FunctionFullName != null && this.FunctionFullName.Length > FL_FunctionFullName && FL_FunctionFullName > 0) throw new Exception("FunctionFullName要求长度小于等于" + FL_FunctionFullName.ToString() + "。");
if (this.Remark != null && this.Remark.Length > FL_Remark && FL_Remark > 0) throw new Exception("Remark要求长度小于等于" + FL_Remark.ToString() + "。");
if (this.FunctionFullNameHash != null && this.FunctionFullNameHash.Length > FL_FunctionFullNameHash && FL_FunctionFullNameHash > 0) throw new Exception("FunctionFullNameHash要求长度小于等于" + FL_FunctionFullNameHash.ToString() + "。");
if (this.FunctionFullNameHash4 != null && this.FunctionFullNameHash4.Length > FL_FunctionFullNameHash4 && FL_FunctionFullNameHash4 > 0) throw new Exception("FunctionFullNameHash4要求长度小于等于" + FL_FunctionFullNameHash4.ToString() + "。");
}


        public void ValidateNotEmpty()
        {
if (string.IsNullOrEmpty(this.FunctionFullName) || string.IsNullOrEmpty(this.FunctionFullName.Trim())) throw new Exception("FunctionFullName要求不空。");
if (string.IsNullOrEmpty(this.Remark) || string.IsNullOrEmpty(this.Remark.Trim())) throw new Exception("Remark要求不空。");
if (string.IsNullOrEmpty(this.FunctionFullNameHash) || string.IsNullOrEmpty(this.FunctionFullNameHash.Trim())) throw new Exception("FunctionFullNameHash要求不空。");
if (string.IsNullOrEmpty(this.FunctionFullNameHash4) || string.IsNullOrEmpty(this.FunctionFullNameHash4.Trim())) throw new Exception("FunctionFullNameHash4要求不空。");
}



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


public AbiFunction Copy()
{
    AbiFunction obj = new AbiFunction();
    obj.FunId = this.FunId;
    obj.FunctionFullName = this.FunctionFullName;
    obj.Remark = this.Remark;
    obj.FunctionFullNameHash = this.FunctionFullNameHash;
    obj.FunctionFullNameHash4 = this.FunctionFullNameHash4;
    obj.IsSysDefine = this.IsSysDefine;
    obj.IsTestOk = this.IsTestOk;
    obj.IsEthTransfer = this.IsEthTransfer;
    return obj;
}



        public static List<AbiFunction> DataTable2List(System.Data.DataTable dt)
        {
            List<AbiFunction> result = new List<AbiFunction>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(AbiFunction.DataRow2Object(dr));
            }
            return result;
        }



        private static AbiFunction _NullObject = null;

        public static  AbiFunction NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new AbiFunction();
                }
                return _NullObject;
            }
        }


        #region 字段长度

        /// <summary>
        /// 字段 FunId 的长度——16
        /// </summary>
        public const int FL_FunId = 16;


        /// <summary>
        /// 字段 FunctionFullName 的长度——256
        /// </summary>
        public const int FL_FunctionFullName = 256;


        /// <summary>
        /// 字段 Remark 的长度——64
        /// </summary>
        public const int FL_Remark = 64;


        /// <summary>
        /// 字段 FunctionFullNameHash 的长度——128
        /// </summary>
        public const int FL_FunctionFullNameHash = 128;


        /// <summary>
        /// 字段 FunctionFullNameHash4 的长度——16
        /// </summary>
        public const int FL_FunctionFullNameHash4 = 16;


        /// <summary>
        /// 字段 IsSysDefine 的长度——1
        /// </summary>
        public const int FL_IsSysDefine = 1;


        /// <summary>
        /// 字段 IsTestOk 的长度——1
        /// </summary>
        public const int FL_IsTestOk = 1;


        /// <summary>
        /// 字段 IsEthTransfer 的长度——1
        /// </summary>
        public const int FL_IsEthTransfer = 1;


        #endregion
}



}