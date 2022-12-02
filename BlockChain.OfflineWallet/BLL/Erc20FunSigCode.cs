using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.BLL
{
    public class Erc20FunSigCode
    {
        public const string transfer = "0xa9059cbb";        //   transfer(address, uint256)： 0xa9059cbb

        public const string balanceOf = "0x70a08231";       //   balanceOf(address)：0x70a08231

        public const string decimals = "0x313ce567";        //   decimals()：0x313ce567

        public const string allowance = "0xdd62ed3e";       //   allowance(address, address)： 0xdd62ed3e

        public const string symbol = "0x95d89b41";          //   symbol()：0x95d89b41

        public const string totalSupply = "0x18160ddd";     //   totalSupply()：0x18160ddd

        public const string name = "0x06fdde03";            //   name()：0x06fdde03

        public const string approve = "0x095ea7b3";         //   approve(address, uint256)：0x095ea7b3

        public const string transferFrom = "0x23b872dd";    //   transferFrom(address, address, uint256)： 0x23b872dd

    }

}
