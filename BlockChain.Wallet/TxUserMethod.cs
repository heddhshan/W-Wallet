using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Wallet
{
    /// <summary>
    /// 本机执行事务的类型名（方法名，主题，操作，等）,需要用到的时候再来添加（记录Tx日志）
    /// </summary>
    public enum TxUserMethod
    {       


        Transfer_EtherWithData,
       


        UniswapV2_SwapExactTokensForTokens,
        UniswapV2_SwapExactTokensForETH,
        UniswapV2_SwapExactETHForTokens,
        UniswapV2Factory_CreatePair,

        UniswapV3SwapRouter_ExactInputSingle,


        /// <summary>
        /// 执行离线签名
        /// </summary>
        OffiSig,

        /// <summary>
        /// 转账
        /// </summary>
        Transfer,

        /// <summary>
        /// Token Transfer
        /// </summary>
        Erc20TokenTransfer,

        /// <summary>
        /// 使用合约批量转账
        /// </summary>
        TransferByContract,

       

        /// <summary>
        /// 交易所买 ETH
        /// </summary>
        ExBuyEth,


        /// <summary>
        /// 取款
        /// </summary>
        Withdraw,

        /// <summary>
        /// 存款
        /// </summary>
        Deposit,


        /// <summary>
        /// 取款
        /// </summary>
        WithdrawToken,

        /// <summary>
        /// 存款
        /// </summary>
        DepositToken,


        /// <summary>
        /// 授权
        /// </summary>
        Erc20TokenApprove,


      
        DeployContract,

      



    };

}
