using System.Numerics;

namespace BlockChain.Lido
{
    public class LidoParam
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        //public const string AddressLido = "0xae7ab96520DE3A18E5e111B5EaAb095312D7fE84";         //mainnet goerli 0x1643E812aE58766192Cf7D2Cf9567dF2C37e9B7F
        public static string GetAddressLido(int chainId)
        {
            switch (chainId)
            {
                case 1: 
                    return "0xae7ab96520DE3A18E5e111B5EaAb095312D7fE84";
                case 5:
                    return "0x1643E812aE58766192Cf7D2Cf9567dF2C37e9B7F";
                default:
                    throw new Exception("No Suplly ChainID:" + chainId.ToString());
            }
        }


        //public const string Address_stETH = "0xae7ab96520DE3A18E5e111B5EaAb095312D7fE84";       //mainnet goerli 0x1643E812aE58766192Cf7D2Cf9567dF2C37e9B7F
        public static string GetAddress_stETH(int chainId) {
            return GetAddressLido(chainId);
        }

        //public const string Address_wstETH = "0x7f39C581F595B53c5cb19bD0b3f8dA6c935E2Ca0";      //mainnet goerli
        public static string GetAddress_wstETH(int chainId)
        {
            switch (chainId)
            {
                case 1:
                    return "0x7f39C581F595B53c5cb19bD0b3f8dA6c935E2Ca0";
                case 5:
                    return "0x6320cD32aA674d2898A68ec82e869385Fc5f7E2f";
                default:
                    throw new Exception("No Suplly ChainID:" + chainId.ToString());
            }
        }

        public const string AddressSubmitReferral = "0xC7A9d8C6C987784967375aE97a35D30AB617eB48";        //写死 

    }
}