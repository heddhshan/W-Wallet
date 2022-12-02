using BlockChain.Wallet.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Wallet.Model
{

    #region Add Time 2022/9/26 15:18:04

    [Serializable]
    public class ViewAddressRealBalance : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region 生成该数据实体的SQL语句
        public const string SQL = @"SELECT 1 AS IsSelected, 0 AS AddressIndex, AddressAlias, Address, 0.0 AS EthAmount, 0.0 AS TokenAmount, 0 AS EthValue, 0 AS TokenValue, 'USDT' AS TokenSymbol, 'abc' AS TokenAddress, 0 AS BlockNumber, 0.0 AS TokenAmountTransfer FROM HD_Address";
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

        //private System.String _TokenSymbol = System.String.Empty;
        //public System.String TokenSymbol
        //{
        //    get { return _TokenSymbol; }
        //    set { _TokenSymbol = value; }
        //}


        private System.String _Address = System.String.Empty;
        public System.String Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        private System.Numerics.BigInteger _Nonce;
        public System.Numerics.BigInteger Nonce
        {
            get { return _Nonce; }
            set
            {
                _Nonce = value;
                OnPropertyChanged("Nonce");
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

        private System.Decimal _TokenAmount;
        public System.Decimal TokenAmount
        {
            get { return _TokenAmount; }
            set
            {
                _TokenAmount = value;
                OnPropertyChanged("TokenAmount");
            }
        }

        private System.Numerics.BigInteger _EthValue;
        public System.Numerics.BigInteger EthValue
        {
            get { return _EthValue; }
            set
            {
                _EthValue = value;
                OnPropertyChanged("EthValue");
            }
        }

        private System.Numerics.BigInteger _TokenValue;
        public System.Numerics.BigInteger TokenValue
        {
            get { return _TokenValue; }
            set
            {
                _TokenValue = value;
                OnPropertyChanged("TokenValue");
            }
        }

        private System.String _TokenSymbol = System.String.Empty;
        public System.String TokenSymbol
        {
            get { return _TokenSymbol; }
            set { _TokenSymbol = value;
                OnPropertyChanged("TokenSymbol");
            }
        }

        private System.String _TokenAddress = System.String.Empty;
        public System.String TokenAddress
        {
            get { return _TokenAddress; }
            set { _TokenAddress = value;
                OnPropertyChanged("TokenAddress");
            }
        }

        private System.Numerics.BigInteger _BlockNumber;
        public System.Numerics.BigInteger BlockNumber
        {
            get { return _BlockNumber; }
            set
            {
                _BlockNumber = value;
                OnPropertyChanged("BlockNumber");
            }
        }

        private System.Decimal _TokenAmountTransfer;
        public System.Decimal TokenAmountTransfer
        {
            get { return _TokenAmountTransfer; }
            set
            {
                _TokenAmountTransfer = value;
                OnPropertyChanged("TokenAmountTransfer");
            }
        }

        #endregion

        #region Public construct

        public ViewAddressRealBalance()
        {
        }



        #endregion



    }


    #endregion


}
