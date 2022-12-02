using Nethereum.Util;
using System.Numerics;

namespace BlockChain.Common
{

    /// <summary>
    /// 计算合约地址
    /// http://playground.nethereum.com/csharp/id/1048
    /// </summary>
    public static class CalculateContractAddressesHelper
    {

        public static string getCalculateContractAddress1(string userAddress, int userNonce)
        {
            var contractAddress = ContractUtils.CalculateContractAddress(userAddress, new BigInteger(userNonce));
            return contractAddress;
        }


        public static string getCalculateContractAddress2(string addressCreate2, string salt, string byteCode)
        {
            var contractAddress = ContractUtils.CalculateCreate2Address(addressCreate2, salt, byteCode);
            return contractAddress;
        }


    }
}
