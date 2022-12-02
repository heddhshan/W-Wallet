using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.Model
{
    #region Add Time 2022/9/23 21:35:53

    [Serializable]
    public class View_AbiParamer : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region 生成该数据实体的SQL语句
        public const string SQL = @"SELECT P.FunId, P.ParamerType, P.ParamerName, P.ParamerOrder, F.IsSysDefine, F.FunctionFullName, '' AS TestValue FROM AbiParamer AS P INNER JOIN AbiFunction AS F ON F.FunId = P.FunId";
        #endregion

        #region Public Properties

        private System.Guid _FunId;
        public System.Guid FunId
        {
            get { return _FunId; }
            set { _FunId = value; }
        }

        private System.String _ParamerType = System.String.Empty;
        public System.String ParamerType
        {
            get { return _ParamerType; }
            set { _ParamerType = value; }
        }

        private System.String _ParamerName = System.String.Empty;
        public System.String ParamerName
        {
            get { return _ParamerName; }
            set { _ParamerName = value; }
        }

        private System.Int32 _ParamerOrder;
        public System.Int32 ParamerOrder
        {
            get { return _ParamerOrder; }
            set { _ParamerOrder = value; }
        }

        private System.Boolean _IsSysDefine;
        public System.Boolean IsSysDefine
        {
            get { return _IsSysDefine; }
            set { _IsSysDefine = value; }
        }

        private System.String _FunctionFullName = System.String.Empty;
        public System.String FunctionFullName
        {
            get { return _FunctionFullName; }
            set { _FunctionFullName = value; }
        }

        private System.String _TestValue = System.String.Empty;
        public System.String TestValue
        {
            get { return _TestValue; }
            set
            {
                _TestValue = value;
                OnPropertyChanged("TestValue");     //只有这个值可以变换，需要读取}
            }
        }

        #endregion

        #region Public construct

        public View_AbiParamer()
        {
        }


        public View_AbiParamer(System.Guid AFunId, System.Int32 AParamerOrder, System.String ATestValue)
        {
            _FunId = AFunId;
            _ParamerOrder = AParamerOrder;
            _TestValue = ATestValue;
        }

        #endregion

        #region Public DataRow2Object

        public static View_AbiParamer DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            View_AbiParamer Obj = new View_AbiParamer();
            Obj.FunId = dr["FunId"] == DBNull.Value ? System.Guid.Empty : (System.Guid)(dr["FunId"]);
            Obj.ParamerType = dr["ParamerType"] == DBNull.Value ? string.Empty : (System.String)(dr["ParamerType"]);
            Obj.ParamerName = dr["ParamerName"] == DBNull.Value ? string.Empty : (System.String)(dr["ParamerName"]);
            Obj.ParamerOrder = dr["ParamerOrder"] == DBNull.Value ? 0 : (System.Int32)(dr["ParamerOrder"]);
            Obj.IsSysDefine = dr["IsSysDefine"] == DBNull.Value ? false : (System.Boolean)(dr["IsSysDefine"]);
            Obj.FunctionFullName = dr["FunctionFullName"] == DBNull.Value ? string.Empty : (System.String)(dr["FunctionFullName"]);
            Obj.TestValue = dr["TestValue"] == DBNull.Value ? string.Empty : (System.String)(dr["TestValue"]);
            return Obj;
        }

        #endregion


        public View_AbiParamer Copy()
        {
            View_AbiParamer obj = new View_AbiParamer();
            obj.FunId = this.FunId;
            obj.ParamerType = this.ParamerType;
            obj.ParamerName = this.ParamerName;
            obj.ParamerOrder = this.ParamerOrder;
            obj.IsSysDefine = this.IsSysDefine;
            obj.FunctionFullName = this.FunctionFullName;
            obj.TestValue = this.TestValue;
            return obj;
        }



        public static List<View_AbiParamer> DataTable2List(System.Data.DataTable dt)
        {
            List<View_AbiParamer> result = new List<View_AbiParamer>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(View_AbiParamer.DataRow2Object(dr));
            }
            return result;
        }



    }


    #endregion


}
