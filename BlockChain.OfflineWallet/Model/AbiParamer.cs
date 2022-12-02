
//
//  本文件由代码工具自动生成。如非必要请不要修改。2022/9/23 21:33:12
//
using System;
using System.Data;
using System.Collections.Generic;

namespace BlockChain.OfflineWallet.Model
{
[Serializable]
public class AbiParamer
{
    #region 生成该数据实体的SQL语句
    public const string SQL = @"Select Top(1) * From AbiParamer";
    #endregion

    #region Public Properties

    private System.Guid _FunId;
    public System.Guid FunId
    {
        get {return _FunId;}
        set {_FunId = value;}
    }

    private System.String _ParamerType = System.String.Empty;
    public System.String ParamerType
    {
        get {return _ParamerType;}
        set {_ParamerType = value;}
    }

    private System.String _ParamerName = System.String.Empty;
    public System.String ParamerName
    {
        get {return _ParamerName;}
        set {_ParamerName = value;}
    }

    private System.Int32 _ParamerOrder;
    public System.Int32 ParamerOrder
    {
        get {return _ParamerOrder;}
        set {_ParamerOrder = value;}
    }

    #endregion

    #region Public construct

    public AbiParamer()
    {
    }

    
    public AbiParamer(System.Guid AFunId,System.Int32 AParamerOrder)
    {
        _FunId = AFunId;
        _ParamerOrder = AParamerOrder;
    }

    #endregion

    #region Public DataRow2Object

    public static AbiParamer DataRow2Object(System.Data.DataRow dr)
    {
        if (dr == null)
        {
            return null;
        }
        AbiParamer Obj = new AbiParamer();
        Obj.FunId = dr["FunId"] == DBNull.Value ? System.Guid.Empty : (System.Guid)(dr["FunId"]);
        Obj.ParamerType = dr["ParamerType"] == DBNull.Value ? string.Empty : (System.String)(dr["ParamerType"]);
        Obj.ParamerName = dr["ParamerName"] == DBNull.Value ? string.Empty : (System.String)(dr["ParamerName"]);
        Obj.ParamerOrder = dr["ParamerOrder"] == DBNull.Value ? 0 : (System.Int32)(dr["ParamerOrder"]);
        return Obj;
    }

    #endregion


        public void ValidateDataLen()
        {
if (this.ParamerType != null && this.ParamerType.Length > FL_ParamerType && FL_ParamerType > 0) throw new Exception("ParamerType要求长度小于等于" + FL_ParamerType.ToString() + "。");
if (this.ParamerName != null && this.ParamerName.Length > FL_ParamerName && FL_ParamerName > 0) throw new Exception("ParamerName要求长度小于等于" + FL_ParamerName.ToString() + "。");
}


        public void ValidateNotEmpty()
        {
if (string.IsNullOrEmpty(this.ParamerType) || string.IsNullOrEmpty(this.ParamerType.Trim())) throw new Exception("ParamerType要求不空。");
}



        public void ValidateEmptyAndLen()
        {
            this.ValidateDataLen();
            this.ValidateNotEmpty();
        }


public AbiParamer Copy()
{
    AbiParamer obj = new AbiParamer();
    obj.FunId = this.FunId;
    obj.ParamerType = this.ParamerType;
    obj.ParamerName = this.ParamerName;
    obj.ParamerOrder = this.ParamerOrder;
    return obj;
}



        public static List<AbiParamer> DataTable2List(System.Data.DataTable dt)
        {
            List<AbiParamer> result = new List<AbiParamer>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(AbiParamer.DataRow2Object(dr));
            }
            return result;
        }



        private static AbiParamer _NullObject = null;

        public static  AbiParamer NullObject
        {
            get
            {
                if (_NullObject == null)
                {
                    _NullObject = new AbiParamer();
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
        /// 字段 ParamerType 的长度——128
        /// </summary>
        public const int FL_ParamerType = 128;


        /// <summary>
        /// 字段 ParamerName 的长度——128
        /// </summary>
        public const int FL_ParamerName = 128;


        /// <summary>
        /// 字段 ParamerOrder 的长度——4
        /// </summary>
        public const int FL_ParamerOrder = 4;


        #endregion
}



}