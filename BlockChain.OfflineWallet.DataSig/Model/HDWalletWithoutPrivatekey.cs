using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.DataSig.Model
{

    [Serializable]
    public class HDWalletWithoutPrivatekey
    {

        private System.Guid _MneId;
        public System.Guid MneId
        {
            get { return _MneId; }
            set { _MneId = value; }
        }

        private System.String _MneAlias = System.String.Empty;
        public System.String MneAlias
        {
            get { return _MneAlias; }
            set { _MneAlias = value; }
        }


        public List<HDAddressWithoutPrivatekey> AddressList = new List<HDAddressWithoutPrivatekey>();

    }



    [Serializable]
    public class HDAddressWithoutPrivatekey
    {

        private System.Guid _MneId;
        public System.Guid MneId
        {
            get { return _MneId; }
            set { _MneId = value; }
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

    }



}
