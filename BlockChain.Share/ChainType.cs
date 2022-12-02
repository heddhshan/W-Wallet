using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Share
{

    public class ChainType
    {

        /// <summary>
        /// 目前只支持以太坊系列！！！
        /// </summary>
        /// <param name="_cte"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetBaseTokenName(ChainTypeEnum _cte)
        {
            if (_cte == ChainTypeEnum.Ethereum)
            {
                return "ETH";
            }
            else if (_cte == ChainTypeEnum.Ethereum_Ropsten)
            {
                return "ETH";
            }
            else if (_cte == ChainTypeEnum.Ethereum_Rinkeby)
            {
                return "ETH";
            }

            else if (_cte == ChainTypeEnum.BNB)
            {
                return "BNB";
            }

            else if (_cte == ChainTypeEnum.Tron)
            {
                return "TRX";
            }

            //add

            return "ETH - " + _cte.ToString();

            //else
            //{
            //    throw new Exception("Error: Share.NodesTypeEnum? ");
            //}

        }
    }


    //以太坊测试网络 Ropsten、Kovan、Rinkeby

    /// <summary>
    /// 节点类型，也就是链类型，采用 ChainId, 参见 https://chainlist.org/zh 。
    /// 以后可以扩展L2。
    /// </summary>
    public enum ChainTypeEnum
    {
        /// <summary>
        /// 以太坊
        /// </summary>
        Ethereum = 1,

        Ethereum_Ropsten = 3,

        Ethereum_Rinkeby = 4,

        /// <summary>
        /// 币安智能链
        /// </summary>
        BNB = 56,

        /// <summary>
        /// 波场 没有 ChainId， 分叉太早的缘故
        /// </summary>
        Tron = 999,


        /////// <summary>
        /////// 未知的链
        /////// </summary>
        ////UnkownChain,

        ///// <summary>
        ///// 以太坊链 主链
        ///// </summary>
        //EthereumMainChain,


        ///// <summary>
        ///// 以太坊链 测试链 Rinkeby
        ///// </summary>
        //EthereumRinkebyChain,

        /////// <summary>
        /////// 以太坊链 测试链 Ropsten
        /////// </summary>
        ////EthereumRopstenChain,


        /////// <summary>
        /////// 币安链
        /////// </summary>
        ////BinanceChain,

        /////// <summary>
        /////// 波场链
        /////// </summary>
        ////TronChain



        

    }


    
}
