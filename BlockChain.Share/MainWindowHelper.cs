using BlockChain.Share.Model;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace BlockChain.Share
{
    interface IErc20Token
    {
        Task<bool> UiErc20TokenApprove(Window _owner, Nethereum.Web3.Accounts.Account _account, string _token, string _spender, System.Numerics.BigInteger _bigAmount);
    }

    /// <summary>
    /// Base MainWindow  3#T �� ������Ϸ ������Ļ��࣬��ߵĹ��ܰ�����ͳһ�ķ����С������ERC20����Ȩ������״̬������������Ϣ �� ״̬��Ϣ��
    /// </summary>
    public class MainWindowHelper : IErc20Token
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Window MainWindow = null;

        //public delegate void SystemLogTx(string from, string txHash, string method, string remark);
        //public static event SystemLogTx OnLogTx;

        public MainWindowHelper(Window mainwin)
        {
            MainWindow = mainwin;
            //MinHeight = "800" MinWidth = "1280"
            // todo : �ֱ���Ҫ��Ϊ1280 * 720����Ϊ14��ıʼǱ���Ĭ��1920*1080������������1.5��������ʵ�ʳ������ 1920/1.5=1280��1080/1.5=720 !!!  �����һ̨���ֵ�����һ��(ȥ���Գǿ���)��
            MainWindow.MinHeight = 800;
            MainWindow.MinWidth = 1280;
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }


        /// <summary>
        /// �õ���Ȩ���
        /// </summary>
        /// <param name="token"></param>
        /// <param name="owner"></param>
        /// <param name="spender"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetApprovedAmount(string token, string owner, string spender)
        {
            Nethereum.Web3.Web3 web3 = ShareParam.GetWeb3();
            Nethereum.StandardTokenEIP20.StandardTokenService Erc20Token = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, token);

            var query = new Nethereum.StandardTokenEIP20.ContractDefinition.AllowanceFunction()
            {
                Owner = owner,
                Spender = spender,
            };
            var result = Erc20Token.AllowanceQueryAsync(query).Result;
            return result;
        }

        // Approve ��Ȩ
        //public async Task<bool> UiErc20TokenApprove(Window _owner, Nethereum.Web3.Accounts.Account _account, string _token, string _spender, double _amount)
        public async Task<bool> UiErc20TokenApprove(Window _owner, Nethereum.Web3.Accounts.Account _account, string _token, string _spender, System.Numerics.BigInteger _bigAmount)
        {
            //var OldCursor = MainWindow.Cursor;
            //MainWindow.Cursor = Cursors.Wait;
            try
            {
                var TokenModel = BLL.Token.GetModel(_token);

                var ApprovedAmount = GetApprovedAmount(_token, _account.Address, _spender);
                if (_bigAmount <= ApprovedAmount)
                {
                    Share.BLL.UserTokenApprove.SaveApproveInfo(_account.Address, _token, _spender, TokenModel, (double)ApprovedAmount / Math.Pow(10, TokenModel.Decimals), "?");
                    return true; //��Ȩ��
                }

                ////��Ȩ��������ִ����Ȩ
                //if (ShareParam.IsErc20ApproveMax)
                //{
                //    _bigAmount = ShareParam.Erc20ApproveMaxValue;
                //}

                //��Ȩ���뵯�����壬��ȷ���Σ�
                var ApproveAmount = (double)_bigAmount / Math.Pow(10, TokenModel.Decimals);
                string Msg = LanguageHelper.GetTranslationText(@"����ִ�� ERC20 Token �� Approve ������") + Environment.NewLine
                            + LanguageHelper.GetTranslationText(@"������������޷�������һ��������") + Environment.NewLine
                            + LanguageHelper.GetTranslationText(@"ERC20Token Address:") + _token + Environment.NewLine
                            + LanguageHelper.GetTranslationText(@"����Ȩ��ַ(Spender Address)��") + _spender + Environment.NewLine
                            + LanguageHelper.GetTranslationText(@"����Ȩ�Ľ��(Approved Amount)��") + ApproveAmount.ToString() + " .";
                bool IsApproved = MessageBox.Show(_owner, Msg, "ERC20Token Approve", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                if (!IsApproved)
                {
                    return false;
                }

                //aprove  //ERC20 Tokenת��, ��������1�� token��Ȩ��2��Ȼ�����  //1�� token��Ȩ
                Nethereum.Web3.Web3 web3 = ShareParam.GetWeb3(_account);

                Nethereum.StandardTokenEIP20.StandardTokenService Erc20Token = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, _token);
                var input = new Nethereum.StandardTokenEIP20.ContractDefinition.ApproveFunction()
                {
                    Spender = _spender,
                    Value = _bigAmount,
                    //GasPrice = Common.Web3Helper.GetNowGasPrice(ShareParam.Web3Url) * 12 / 10,    //����20%����Ҫ������ٶȣ�������һ��������
                };
                
                ////д��1
                //var tx = await Erc20Token.ApproveRequestAsync(input);  

                var FullTx = await Erc20Token.ApproveRequestAndWaitForReceiptAsync(input);
                var tx = FullTx.TransactionHash;
                Share.BLL.TransactionReceipt.LogTx(_account.Address, tx, "Erc20Token.Approve", "To:" + _token + ",Spender:" + _spender + ",Value:" + _bigAmount.ToString());
                Share.BLL.UserTokenApprove.SaveApproveInfo(_account.Address, _token, _spender, TokenModel, ApproveAmount, tx);
                return true;

                ////д��1
                ////����������δ�������գ�ֻ����Ȩ�ɹ����ܿ�Ǯ����������ִ��ʱ�䣡
                //int MaxS = 0;                               //�ʱ�䣬������
                //while (true)
                //{
                //    var query = new Nethereum.StandardTokenEIP20.ContractDefinition.AllowanceFunction()
                //    {
                //        Owner = _account.Address,
                //        Spender = _spender,
                //    };
                //    var an = await Erc20Token.AllowanceQueryAsync(query);
                //    if (_bigAmount <= an)
                //    {
                //        return true;                            //OK
                //    }
                //    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));      //��Ϣ 10 �룬 ��̫�� 13 �봦��һ�� block ��
                //    MaxS++;
                //    if (12 <= MaxS)
                //    {
                //        var IsGo = (MessageBox.Show(_owner, LanguageHelper.GetTranslationText("��Ȩת�ˣ�Approve�����������ڻ�δִ�гɹ����Ƿ�����ȵȣ�")
                //                                            + Environment.NewLine
                //                                            + LanguageHelper.GetTranslationText("������ȴ�����һ����������ʧ�ܡ�"), "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes);
                //        if (!IsGo)
                //        {
                //            return false;
                //        }
                //        MaxS = 0;
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(_owner, ex.Message);
                return false;
            }
            //finally
            //{
            //    MainWindow.Cursor = OldCursor;
            //}
        }


    }


}
