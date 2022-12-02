using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.DataSig.Model
{

    [Serializable]
    public class UiTransaction1559Obj  : INotifyPropertyChanged     //Transaction1559
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int IndexId { get; set; }


        private bool _IsSelected = true;

        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnPropertyChanged("IsSelected"); } }


        public BigInteger ChainId { get;  set; }

        public BigInteger? Nonce { get;  set; }

        public BigInteger? MaxPriorityFeePerGas { get;  set; }

        public BigInteger? MaxFeePerGas { get;  set; }

        public BigInteger? GasLimit { get;  set; }

        public string From { get;  set; }
        public string To { get; set; }

        public BigInteger? Amount { get;  set; }

        public string Data { get;  set; }

        //public List<AccessListItem> AccessList { get; private set; }

        public  TransactionType TransactionType => TransactionType.EIP1559;


        public string TransactionHash { get; set; }

    }
}
