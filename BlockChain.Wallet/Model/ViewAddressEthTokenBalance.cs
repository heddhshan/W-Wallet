
using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;

namespace BlockChain.Wallet.Model
{
    [Serializable]
    public class ViewAddressEthTokenBalance : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region 生成该数据实体的SQL语句
        public const string SQL = @"
SELECT 1 AS IsSelected, HD_Address.MneId, HD_Address.MneSecondSalt, HD_Address.AddressIndex, HD_Address.AddressAlias, HD_Address.Address, AddressBalance_1.Balance, 0.0 AS Amount,
              (SELECT Balance
             FROM   AddressBalance
             WHERE (TokenAddress = '0x0000000000000000000000000000000000000000') AND (UserAddress = HD_Address.Address)) AS EthAmount
FROM   HD_Address LEFT OUTER JOIN
          AddressBalance AS AddressBalance_1 ON HD_Address.Address = AddressBalance_1.UserAddress
";
        #endregion

        #region Public Properties

        private System.Boolean _IsSelected;
        public System.Boolean IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

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

        private System.Double _Balance;
        public System.Double Balance
        {
            get { return _Balance; }
            set
            {
                _Balance = value;
                OnPropertyChanged("Balance");
            }
        }

        private System.Decimal _Amount;
        public System.Decimal Amount
        {
            get { return _Amount; }
            set
            {
                _Amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private System.Decimal _EthAmount;
        public System.Decimal EthAmount
        {
            get { return _EthAmount; }
            set
            {
                _EthAmount = value;
                OnPropertyChanged("EthAmount");
            }
        }

        #endregion

        #region Public construct

        public ViewAddressEthTokenBalance()
        {
        }

        public ViewAddressEthTokenBalance(System.Boolean AIsSelected, System.String AAddress, System.Decimal AAmount, System.Decimal AEthAmount)
        {
            _IsSelected = AIsSelected;
            _Address = AAddress;
            _Amount = AAmount;
            _EthAmount = AEthAmount;
        }

        #endregion

        #region Public DataRow2Object

        public static ViewAddressEthTokenBalance DataRow2Object(System.Data.DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            ViewAddressEthTokenBalance Obj = new ViewAddressEthTokenBalance();
            Obj.IsSelected = dr["IsSelected"] == DBNull.Value ? false : ((System.Int32)dr["IsSelected"] == 1);
            Obj.MneId = dr["MneId"] == DBNull.Value ? Guid.Empty : (System.Guid)(dr["MneId"]);
            Obj.MneSecondSalt = dr["MneSecondSalt"] == DBNull.Value ? string.Empty : (System.String)(dr["MneSecondSalt"]);
            Obj.AddressIndex = dr["AddressIndex"] == DBNull.Value ? 0 : (System.Int32)(dr["AddressIndex"]);
            Obj.AddressAlias = dr["AddressAlias"] == DBNull.Value ? string.Empty : (System.String)(dr["AddressAlias"]);
            Obj.Address = dr["Address"] == DBNull.Value ? string.Empty : (System.String)(dr["Address"]);
            Obj.Balance = dr["Balance"] == DBNull.Value ? 0 : (System.Double)(dr["Balance"]);
            Obj.Amount = dr["Amount"] == DBNull.Value ? 0 : (System.Decimal)(dr["Amount"]);
            Obj.EthAmount = dr["EthAmount"] == DBNull.Value ? 0 : (System.Decimal)(dr["EthAmount"]);
            return Obj;
        }

        #endregion

        public static List<ViewAddressEthTokenBalance> DataTable2List(System.Data.DataTable dt)
        {
            List<ViewAddressEthTokenBalance> result = new List<ViewAddressEthTokenBalance>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                result.Add(ViewAddressEthTokenBalance.DataRow2Object(dr));
            }
            return result;
        }



    }



}