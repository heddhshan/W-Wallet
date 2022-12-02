using BlockChain.Share;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlockChain.Share.BLL
{
    public class TransactionReceipt
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        class TransactionReceiptToast
        {
            private string From;
            private string TxHash;
            private string Method;
            private string Remark;

            public TransactionReceiptToast(string from, string txHash, string method, string remark)
            {
                From = from;
                TxHash = txHash;
                Method = method;
                Remark = remark;
            }

            System.Threading.Thread t = null;
            public void ExeReceipt()
            {
                if (t == null)
                {
                    t = new System.Threading.Thread(DoReceipt);
                    t.Start();
                }
            }

            private async void DoReceipt()
            {
                var t1 = System.DateTime.Now;
                try
                {
                    Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();
                    var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(TxHash);
                    while (null == receipt)
                    {
                        receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(TxHash);
                        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 1));
                    }

                    bool haserr = !(receipt.HasErrors() == null || receipt.HasErrors() == false);

                    if (!haserr)
                    {
                        WpfToastNotifications.Show(LanguageHelper.GetTranslationText("交易执行成功"),
                               "From: " + From + Environment.NewLine +
                               "TxHash: " + TxHash + Environment.NewLine +
                               "Method: " + LanguageHelper.GetTranslationText(Method) + Environment.NewLine +
                               Remark,
                               NotificationType.Information, 120, Share.ShareParam.GetTxUrl(TxHash));
                    }
                    else
                    {
                        WpfToastNotifications.Show(LanguageHelper.GetTranslationText("交易执行失败"),
                               "From: " + From + Environment.NewLine +
                               "TxHash: " + TxHash + Environment.NewLine +
                               "Method: " + LanguageHelper.GetTranslationText(Method) + Environment.NewLine +
                               Remark,
                               NotificationType.Warning, 120, Share.ShareParam.GetTxUrl(TxHash));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                }
                finally
                {
                    var t2 = System.DateTime.Now;
                    log.Info("var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash); TotalSeconds = " + (t2 - t1).TotalSeconds.ToString());
                    t = null;
                }
            }

        }


        //public static void LogTx(string from, string txHash, BLL.TxUserMethod method, string remark)
        //{
        //    LogTx(from, txHash, method.ToString(), remark);
        //}


        public static void LogTx(string from, string txHash, string method, string remark)
        {
            //所有的事务执行结果都通知一下
            TransactionReceiptToast toast = new TransactionReceiptToast(from, txHash, method, remark);
            toast.ExeReceipt();

            Model.TransactionReceipt model = new Model.TransactionReceipt();
            try
            {
                model.UserFrom = from;
                model.CreateTime = System.DateTime.Now;
                model.Canceled = false;
                model.TransactionHash = txHash;
                model.UserRemark = remark;
                model.UserMethod = method;
                model.GotReceipt = false;        //Pending 是否已经执行了           // -1;  //-1代表等待处理（padding）； 0代表错误，1代表成功
                model.ValidateEmptyAndLen();

                int newid;
                DAL.TransactionReceipt.Insert(Share.ShareParam.DbConStr, model, out newid);
            }
            catch (Exception ex)
            {
                // txHash 有可能重复，只有在变换网络的时候才可能重复，特别是把测试环境重新搭建后！！！
                var str = Common.Security.Tools.CreateXML<Model.TransactionReceipt>(model);
                log.Error("LogTx Error" + Environment.NewLine + str, ex);
            }
        }

        /// <summary>
        /// 得到正在处理的事务列表
        /// </summary>
        /// <returns></returns>
        public static List<Model.TransactionReceipt> GetDoingTx()
        {
            string sql = @"
SELECT  *
FROM      TransactionReceipt
WHERE   (GotReceipt = 0) AND (CreateTime >= @CreateTime)
ORDER BY Id DESC
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@CreateTime", SqlDbType.DateTime).Value = System.DateTime.Now.AddDays(-7);   //只显示七天前的

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            return Model.TransactionReceipt.DataTable2List(ds.Tables[0]);
        }

        /// <summary>
        /// 得到前面500条历史事务列表，包括错误的
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTop500HisTx()
        {
            string sql = @"
SELECT   TOP (500) TransactionHash, CreateTime, UserMethod, UserFrom, UserRemark, 
                CASE 
					WHEN Canceled = 1 THEN 'Cancel' 
					WHEN GotReceipt = 0 THEN 'Pending' 
					WHEN GotReceipt = 1 AND  HasErrors = 1 THEN 'Fail' 
					WHEN GotReceipt = 1 AND HasErrors = 0 THEN 'Success' 
				END AS TxStatus, 
                GasUsed, GasPrice, GasPrice / 1000000000.0 as GasPriceGwei
FROM      TransactionReceipt
ORDER BY Id DESC
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private static bool isRunning = false;


        public static List<Model.TransactionReceipt> GetToDoTx()
        {
            string sql = @"
SELECT  *
FROM      TransactionReceipt
WHERE   GotReceipt = 0
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            return Model.TransactionReceipt.DataTable2List(ds.Tables[0]);
        }

        public static bool SaveResult(Model.TransactionReceipt tx)
        {
            tx.ResultTime = System.DateTime.Now;
            return DAL.TransactionReceipt.Update(Share.ShareParam.DbConStr, tx) > 0;
        }

        public static void StartDoToDoTx()
        {
            if (!isRunning)
            {
                isRunning = true;
                System.Threading.Thread t = new System.Threading.Thread(DoToToTx);
                t.Start();
            }
        }


        public delegate void GetTransactionReceiptHandler(string txid, Nethereum.RPC.Eth.DTOs.TransactionReceipt receipt);

        public static event GetTransactionReceiptHandler OnGetTransactionReceipt;       //处理某些订阅事件，目前只处理了骰宝和时时彩的OnPlay事件。

        public async static void DoToToTx()
        {
            log.Info("后台线程: DoToToTx（检查事务执行状态） Begin");

            try
            {

                bool IsRunning = CheckIsRunning();
                while (IsRunning)
                {
                    for (int i = 0; i < 60 * 1000; i++)         //休息60秒，
                    {
                        IsRunning = CheckIsRunning();
                        if (IsRunning)
                        {
                            System.Threading.Thread.Sleep(1);
                        }
                        else
                        {
                            return;
                        }
                    }

                    try
                    {
                        var web3 = Share.ShareParam.GetWeb3();

                        var todolist = GetToDoTx();
                        foreach (var todo in todolist)
                        {
                            IsRunning = CheckIsRunning();
                            if (!IsRunning)
                            {
                                return;
                            }

                            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(todo.TransactionHash);
                            if (receipt == null)
                            {
                                continue;
                            }

                            try
                            {
                                OnGetTransactionReceipt?.Invoke(todo.TransactionHash, receipt); //发布收到执行成功事件，主要用于通知，等
                            }
                            catch (Exception ex)
                            {
                                log.Error("Event OnGetTransactionReceipt", ex);
                            }

                            todo.BlockHash = receipt.BlockHash;
                            todo.BlockNumber = (long)receipt.BlockNumber.Value;

                            if (!string.IsNullOrEmpty(receipt.ContractAddress))
                            {
                                todo.ContractAddress = receipt.ContractAddress;
                            }

                            bool haserr = !(receipt.HasErrors() == null || receipt.HasErrors() == false);
                            todo.CumulativeGasUsed = (long)receipt.CumulativeGasUsed.Value;
                            todo.GasUsed = (long)receipt.GasUsed.Value;
                            todo.HasErrors = haserr;
                            todo.ResultTime = System.DateTime.Now;

                            if (receipt.Status == null)
                            {
                                todo.Status = 0;
                            }
                            else
                            {
                                todo.Status = (long)receipt.Status.Value;
                            }

                            todo.TransactionIndex = (long)receipt.TransactionIndex.Value;
                            todo.Logs = receipt.Logs.ToString();
                            todo.GotReceipt = true;                     //标记是否获得Receipt！

                            var tx = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(todo.TransactionHash);
                            todo.GasPrice = (long)tx.GasPrice.Value;

                            todo.ValidateEmptyAndLen();
                            SaveResult(todo);

                            if (!haserr)
                            {
                                // 成功，发送Windows10通知 NO，这里不需要，
                            }
                            else
                            {
                                // 失败，发送Windows10通知 NO，这里不需要，
                                log.Error("TransactionHash " + todo.TransactionHash + " Failed!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("DoToToTx", ex);
                    }
                }

            }
            finally { 
                log.Info("后台线程: DoToToTx（检查事务执行状态） End"); 
            }
        }

        private static bool CheckIsRunning()
        {
            return Application.Current != null;         // && Application.Current.MainWindow != null;// && ((MainWindowHelper)(Application.Current.MainWindow)).IsRunning();
            //return Application.Current != null && Application.Current.MainWindow != null;// && ((MainWindowHelper)(Application.Current.MainWindow)).IsRunning();
        }
    }


}
