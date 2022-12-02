using BlockChain.OfflineWallet.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.Model
{
    #region Add Time 2022/9/14 11:11:03

    [Serializable]
    public class ViewHdAddress : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region 生成该数据实体的SQL语句
        public const string SQL = @"SELECT 1 AS IsSelected, AddressIndex, AddressAlias, Address, MneSecondSalt FROM HD_Address ";
        #endregion

        #region Public Properties

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
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

        private System.String _MneSecondSalt = System.String.Empty;
        public System.String MneSecondSalt
        {
            get { return _MneSecondSalt; }
            set { _MneSecondSalt = value; }
        }

        #endregion

        #region Public construct

        public ViewHdAddress()
        {
        }


        public ViewHdAddress(bool AIsSelected, System.String AAddress)
        {
            _IsSelected = AIsSelected;
            _Address = AAddress;
        }


        #endregion

        #region Public DataRow2Object

        public static ViewHdAddress DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            ViewHdAddress Obj = new ViewHdAddress();
            var Selected = dr["IsSelected"] == DBNull.Value ? 0 : (System.Int32)(dr["IsSelected"]);
            Obj.IsSelected = Selected == 1;
            Obj.AddressIndex = dr["AddressIndex"] == DBNull.Value ? 0 : (System.Int32)(dr["AddressIndex"]);
            Obj.AddressAlias = dr["AddressAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["AddressAlias"]);
            Obj.Address = dr["Address"] == DBNull.Value ? string.Empty : (System.String)(dr["Address"]);
            Obj.MneSecondSalt = dr["MneSecondSalt"] == DBNull.Value ? string.Empty : (System.String)(dr["MneSecondSalt"]);
            return Obj;
        }

        #endregion


        public static List<ViewHdAddress> DataTable2List(System.Data.DataTable dt)
        {
            List<ViewHdAddress> result = new List<ViewHdAddress>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(ViewHdAddress.DataRow2Object(dr));
            }
            return result;
        }





    }


    #endregion


}
