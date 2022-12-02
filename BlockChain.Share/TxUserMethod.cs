using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Share
{

    /// <summary>
    /// 本机执行事务的类型名（方法名，主题，操作，等）,需要用到的时候再来添加（记录Tx日志）
    /// </summary>
    public enum TxUserMethod
    {
        

        ToolsPublishNotice,
             

        Approve,

     

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




    };

}
