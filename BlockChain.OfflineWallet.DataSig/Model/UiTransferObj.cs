using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlockChain.OfflineWallet.DataSig.Model
{

    /// <summary>
    /// 转账数据对象，适用于生成显示转账数据，
    /// </summary>
    [Serializable]
    public class UiTransferObj : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int IndexId { get; set; }


        private bool _IsSelected = true;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private string _From;
        public string From
        {
            get
            {
                return _From;
            }
            set
            {
                _From = value;
                OnPropertyChanged("From");
            }
        }

        private BigInteger _FromNonce;
        public BigInteger FromNonce
        {
            get { return _FromNonce; }
            set
            {
                _FromNonce = value;
                OnPropertyChanged("FromNonce");
            }
        }


        private string _TokenAddress;
        public string TokenAddress
        {
            get { return _TokenAddress; }
            set
            {
                _TokenAddress = value;
                OnPropertyChanged("TokenAddress");
            }
        }


        private string _TokenSymbol;

        public string TokenSymbol
        {
            get
            {
                return _TokenSymbol;
            }
            set
            {
                _TokenSymbol = value;
                OnPropertyChanged("TokenSymbol");
            }
        }

        private string _TokenTo;
        public string TokenTo
        {
            get { return _TokenTo; }
            set
            {
                _TokenTo = value;
                OnPropertyChanged("TokenTo");
            }
        }


        private decimal _TokenAmountTransfer;
        public decimal TokenAmountTransfer
        {
            get { return _TokenAmountTransfer; }
            set
            {
                _TokenAmountTransfer = value;
                OnPropertyChanged("TokenAmountTransfer");
            }
        }




    }


}
