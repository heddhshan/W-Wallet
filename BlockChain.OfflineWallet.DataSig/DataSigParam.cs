using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.OfflineWallet.DataSig
{

    /// <summary>
    /// 参数定义，包括常数，系统变量，公共方法，等
    /// </summary>
    public static class DataSigParam
    {

        public const string Function_Eth_Transfer = "Eth_Transfer";

        public const string Function_Erc20_Transfer = "transfer(address,uint256)";    
        public const string Function_Erc20_Approve= "approve(address, uint256)";

        public const string Function_Nft_TransferFrom = "safeTransferFrom(address,address,uint256)";
        public const string Function_Nft_Approve = "approve(address,uint256)";
        


    }
}
