using BlockChain.Share;
using Microsoft.Win32;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;

namespace BlockChain.OfflineWallet
{
    /// <summary>
    /// WindowOfflineSig.xaml 的交互逻辑
    /// </summary>
    public partial class WindowOfflineSig : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowOfflineSig()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridFuncions.ItemsSource = BLL.AbiFunction.getAllFunction();
        }


        /// <summary>
        /// 对 inputdata 签名，是灵活性最大的签名方式！
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfflineSigTx_Click0(object sender, RoutedEventArgs e)
        {
            try
            {
                string inputdata = TextBoxInputData0.Text.Trim();

                string from = this.TextBoxFrom0.Text;
                var chainId = int.Parse(TextBoxChainId0.Text);
                string to = this.TextBoxTo0.Text;

                BigInteger etheramount = BigInteger.Parse(this.TextBoxAmountToSend0.Text);
                BigInteger maxPriorityFeePerGas = BigInteger.Parse(this.TextBoxMaxPriorityFeePerGas0.Text);
                BigInteger maxFeePerGas = BigInteger.Parse(this.TextBoxMaxFeePerGas0.Text);
                BigInteger gas = BigInteger.Parse(this.TextBoxGas0.Text);
                BigInteger nonce = BigInteger.Parse(this.TextBoxNonce0.Text);

                string Password;
                var hdmodel = BLL.HDWallet.GetHdModel(from);        //如果是助记词，需要使用助记词的密码
                if (hdmodel == null)
                {
                    if (!WindowPassword.GetPassword(this, from, "Get Password", out Password)) return;
                }
                else
                {
                    if (!WindowPassword.GetPassword(this, hdmodel.MneAlias, "Get Password", out Password)) return;
                }

                var account = new BLL.Address().GetAccount(from, Password);
                if (account == null) { MessageBox.Show(this, @"No Account"); return; }

                ////方法1 //参见： https://playground.nethereum.com/csharp/id/1053        使用 EtherTransferTransactionInputBuilder 是错的，这个处理eth转账         
                //var web3 = new Web3(account);
                //web3.TransactionManager.UseLegacyAsDefault = false;     //使用 eip1559  这种写法有问题的！
                //var transactionManager = web3.TransactionManager;
                //var transactionInput = EtherTransferTransactionInputBuilder.CreateTransactionInput(from, to, (decimal)etheramount / (decimal)Math.Pow(10, 18), maxPriorityFeePerGas, maxFeePerGas, gas, nonce);
                //var rawTransaction = await transactionManager.SignTransactionAsync(transactionInput);
                //TextBoxOfflineSigTx.Text = rawTransaction;

                //方法2 参见： https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Signer/Transaction1559.cs#L13
                var transaction = new Nethereum.Signer.Transaction1559(chainId, nonce, maxPriorityFeePerGas, maxFeePerGas, gas, to, etheramount, inputdata, null);
                transaction.Sign(new EthECKey(account.PrivateKey));
                TextBoxOfflineSigTx0.Text = transaction.GetRLPEncoded().ToHex(true);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }



        private void Select_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridFuncions.SelectedItem != null)
                {
                    if (DataGridFuncions.SelectedItem is Model.AbiFunction)
                    {
                        var m = DataGridFuncions.SelectedItem as Model.AbiFunction;
                        DataGridParamers.ItemsSource = BLL.AbiFunction.getAbiParamer(m.FunId);
                        LabelCurrentFunction.Content = m.FunctionFullName;
                        LabelCurrentFunction.Tag = m.FunId;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }


        private void TestParamerInput_Click(object sender, RoutedEventArgs e)
        {
            TextBoxInputData.Text = GetInputData();
        }

        private string GetInputData()
        {
            string result = string.Empty;
            try
            {
                //string fun = LabelCurrentFunction.Content.ToString();
                string funFull = LabelCurrentFunction.Content.ToString();
                string funHash = DataSig.InputDataHelper.getFunctionHash(funFull);
                Guid funid = (Guid)LabelCurrentFunction.Tag;
                var model = DAL.AbiFunction.GetModel(ShareParam.DbConStr, funid);
                if (model.IsEthTransfer)
                {
                    result = "0x";
                    return result;
                }

                if (null != DataGridParamers.ItemsSource)
                {
                    var list = (List<Model.View_AbiParamer>)DataGridParamers.ItemsSource;
                    List<Nethereum.ABI.Model.Parameter> pl = new List<Nethereum.ABI.Model.Parameter>();
                    List<object> vl = new List<object>();

                    foreach (var param in list)
                    {
                        //1，参数
                        Nethereum.ABI.Model.Parameter p = new Nethereum.ABI.Model.Parameter(param.ParamerType, param.ParamerName, param.ParamerOrder);
                        pl.Add(p);

                        //2: 值，     只处理简单类型！
                        Nethereum.ABI.ABIType abitype = Nethereum.ABI.ABIType.CreateABIType(param.ParamerType);       //如果报异常，类型就无法处理，
                        if (abitype is Nethereum.ABI.IntType)
                        {
                            vl.Add(BigInteger.Parse(param.TestValue));
                        }
                        else if (abitype is Nethereum.ABI.BoolType)
                        {
                            vl.Add(bool.Parse(param.TestValue));
                        }
                        else if (abitype is Nethereum.ABI.StringType || abitype is Nethereum.ABI.AddressType || abitype is Nethereum.ABI.BytesType || abitype is Nethereum.ABI.Bytes32Type)
                        {
                            vl.Add(param.TestValue);
                        }
                        //else if (abitype is Nethereum.ABI.ArrayType || abitype is Nethereum.ABI.DynamicArrayType || abitype is Nethereum.ABI.StaticArrayType || abitype is Nethereum.ABI.TupleType)
                        //{
                        //    throw new Exception("Array or Tuple cannot be processed!");
                        //}
                        else
                        {
                            throw new Exception(abitype.Name + " cannot be processed!");
                        }
                    }

                    result = DataSig.InputDataHelper.getInputData(funFull, pl, vl.ToArray());
                }
                else
                {
                    //泡不到这里！
                    result = DataSig.InputDataHelper.getInputData(funFull, null, null);
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }

            return result;
        }

        private  void OfflineSigTx_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string funFull = LabelCurrentFunction.Content.ToString();
                //string funHash = DataSig.InputDataHelper.getFunctionHash(funFull);
                //var model =      DAL.AbiFunction.GetModel(ShareParam.DbConStr, funHash);

                string inputdata = TextBoxInputData.Text.Trim();
                if (this.CheckBoxModifyInputData.IsChecked == true)
                {
                    //如果修改了，就取修改后的值
                }
                else
                {
                    inputdata = GetInputData();
                    TextBoxInputData.Text = inputdata;
                }

                string from = this.TextBoxFrom.Text;
                var chainId = int.Parse(TextBoxChainId.Text);
                string to = this.TextBoxTo.Text;
                //decimal etheramount = decimal.Parse(this.TextBoxAmountToSend.Text);
                BigInteger etheramount = BigInteger.Parse(this.TextBoxAmountToSend.Text);
                BigInteger maxPriorityFeePerGas = BigInteger.Parse(this.TextBoxMaxPriorityFeePerGas.Text);
                BigInteger maxFeePerGas = BigInteger.Parse(this.TextBoxMaxFeePerGas.Text);
                BigInteger gas = BigInteger.Parse(this.TextBoxGas.Text);
                BigInteger nonce = BigInteger.Parse(this.TextBoxNonce.Text);

                //string Password;
                //if (!WindowPassword.GetPassword(this, from, "Get Password", out Password)) return;
                string Password;
                var hdmodel = BLL.HDWallet.GetHdModel(from);        //如果是助记词，需要使用助记词的密码
                if (hdmodel == null)
                {
                    if (!WindowPassword.GetPassword(this, from, "Get Password", out Password)) return;
                }
                else
                {
                    if (!WindowPassword.GetPassword(this, hdmodel.MneAlias, "Get Password", out Password)) return;
                }

                var account = new BLL.Address().GetAccount(from, Password);
                if (account == null) { MessageBox.Show(this, @"No Account"); return; }

                //if (model.IsEthTransfer) { }

                ////方法1 //参见： https://playground.nethereum.com/csharp/id/1053        使用 EtherTransferTransactionInputBuilder 是错的，这个处理eth转账         
                //var web3 = new Web3(account);
                //web3.TransactionManager.UseLegacyAsDefault = false;     //使用 eip1559  这种写法有问题的！
                //var transactionManager = web3.TransactionManager;
                //var transactionInput = EtherTransferTransactionInputBuilder.CreateTransactionInput(from, to, (decimal)etheramount / (decimal)Math.Pow(10, 18), maxPriorityFeePerGas, maxFeePerGas, gas, nonce);
                //var rawTransaction = await transactionManager.SignTransactionAsync(transactionInput);
                //TextBoxOfflineSigTx.Text = rawTransaction;

                //方法2 参见： https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Signer/Transaction1559.cs#L13
                var transaction = new Nethereum.Signer.Transaction1559(chainId, nonce, maxPriorityFeePerGas, maxFeePerGas, gas, to, etheramount, inputdata, null);
                transaction.Sign(new EthECKey(account.PrivateKey));
                TextBoxOfflineSigTx.Text = transaction.GetRLPEncoded().ToHex(true);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void ModifyInputData_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxInputData.IsReadOnly = this.CheckBoxModifyInputData.IsChecked == true;
        }

        private void ModifyInputData_UnChecked(object sender, RoutedEventArgs e)
        {
            TextBoxInputData.IsReadOnly = this.CheckBoxModifyInputData.IsChecked == true;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string BackUpPath = Share.ShareParam.BackUpDir;

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = BackUpPath;
                ofd.Filter = "Waiting for Signature File|*.wfs|All File|*.*";

                if (ofd.ShowDialog() == true)
                {
                    var lines = System.IO.File.ReadAllLines(ofd.FileName);

                    List<DataSig.Model.UiBaseOffSigObj> ls = new List<DataSig.Model.UiBaseOffSigObj>();
                    int i = 0;

                    foreach (var line in lines)
                    {
                        //ChainId,From,To,Nonce,GasLimit,MaxFeePerGas,MaxPriorityFeePerGas,EthWeiAmount,InputData
                        DataSig.Model.UiBaseOffSigObj m = new DataSig.Model.UiBaseOffSigObj();
                        var arr = line.Split(',');
                        m.ChainId = BigInteger.Parse(arr[0]);
                        m.From = arr[1];
                        m.To = arr[2];
                        m.Nonce = BigInteger.Parse(arr[3]);
                        m.GasLimit = BigInteger.Parse(arr[4]);
                        m.MaxFeePerGas = BigInteger.Parse(arr[5]);
                        m.MaxPriorityFeePerGas = BigInteger.Parse(arr[6]);
                        m.EthWeiAmount = BigInteger.Parse(arr[7]);
                        m.InputData = arr[8];

                        i++;
                        m.IndexId = i;
                        m.IsSelected = true;

                        ls.Add(m);
                    }

                    DataGridCommonTxSig.ItemsSource = ls;
                }
                else
                {
                    ;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void SigSelected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridCommonTxSig.ItemsSource != null)
                {
                    List<string> result = new List<string>();
                    List<DataSig.Model.UiBaseOffSigObj> ls = (List<DataSig.Model.UiBaseOffSigObj>)DataGridCommonTxSig.ItemsSource;
                    foreach (var m in ls)
                    {
                        if (!m.IsSelected) { continue; }

                        //string password;
                        //if (!Common.PasswordManager.GetPassword(m.From, out password))          //密码如果保存了，就不要再提示了！  //todo: 这里有问题，一般都是HD钱包！！！
                        //{
                        //    if (!Share.WindowPassword.GetPassword(this, m.From, "Get Password", out password))
                        //    {
                        //        m.SigData = "No Get Password";
                        //        result.Add("IndexId=" + m.IndexId.ToString() + "No Get Password");
                        //        continue;
                        //    }
                        //}

                        string Password;
                        var hdmodel = BLL.HDWallet.GetHdModel(m.From);        //如果是助记词，需要使用助记词的密码
                        if (hdmodel == null)
                        {
                            if (!Common.PasswordManager.GetPassword(m.From, out Password))          //密码如果保存了，就不要再提示了！  //todo: 这里有问题，一般都是HD钱包！！！
                            {
                                if (!WindowPassword.GetPassword(this, m.From, "Get Password", out Password))
                                {
                                    m.SigData = "No Get Password";
                                    result.Add("IndexId=" + m.IndexId.ToString() + "No Get Password");
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if (!Common.PasswordManager.GetPassword(hdmodel.MneAlias, out Password))          //密码如果保存了，就不要再提示了！  //todo: 这里有问题，一般都是HD钱包！！！
                            {
                                if (!WindowPassword.GetPassword(this, hdmodel.MneAlias, "Get Password", out Password))
                                {
                                    m.SigData = "No Get Password";
                                    result.Add("IndexId=" + m.IndexId.ToString() + "No Get Password");
                                    continue;
                                }
                            }
                        }


                        var account = new BLL.Address().GetAccount(m.From, Password);
                        if (account == null)
                        {
                            m.SigData = "No Get Account";
                            result.Add("IndexId=" + m.IndexId.ToString() + ": No Get Account");
                            continue;
                        }

                        var transaction = new Nethereum.Signer.Transaction1559(m.ChainId, m.Nonce, m.MaxPriorityFeePerGas, m.MaxFeePerGas, m.GasLimit, m.To, m.EthWeiAmount, m.InputData, null);
                        transaction.Sign(new EthECKey(account.PrivateKey));
                        string sigdata = transaction.GetRLPEncoded().ToHex(true);

                        m.SigData = sigdata;        //界面上也要显示
                        result.Add(sigdata);
                    }

                    if (result.Count > 0)
                    {
                        string BackUpPath = Share.ShareParam.BackUpDir;
                        string filename = System.IO.Path.Combine(BackUpPath, "SigData_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".sig");
                        System.IO.File.AppendAllLines(filename, result);
                        System.Diagnostics.Process.Start("Explorer", "/select," + filename);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
            }
        }

        private void SigOne_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridCommonTxSig.SelectedItem != null)
                {
                    var m = (DataSig.Model.UiBaseOffSigObj)DataGridCommonTxSig.SelectedItem;

                    string password;

                    var hdmodel = BLL.HDWallet.GetHdModel(m.From);        //如果是助记词，需要使用助记词的密码
                    if (hdmodel == null)
                    {
                        if (!Share.WindowPassword.GetPassword(this, m.From, "Get Password", out password))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!Share.WindowPassword.GetPassword(this, hdmodel.MneAlias, "Get Password", out password))
                        {
                            return;
                        }
                    }

                    var account = new BLL.Address().GetAccount(m.From, password);
                    if (account == null)
                    {
                        return;
                    }

                    var transaction = new Nethereum.Signer.Transaction1559(m.ChainId, m.Nonce, m.MaxPriorityFeePerGas, m.MaxFeePerGas, m.GasLimit, m.To, m.EthWeiAmount, m.InputData, null);
                    transaction.Sign(new EthECKey(account.PrivateKey));
                    string sigdata = transaction.GetRLPEncoded().ToHex(true);

                    m.SigData = sigdata;

                    MessageBox.Show("OK");
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
            }
        }

        private void ERC20TokenLoad_Click(object sender, RoutedEventArgs e)
        {
            //DataGridERC20TokenTransferSig
            try
            {
                string BackUpPath = Share.ShareParam.BackUpDir;

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = BackUpPath;
                ofd.Filter = "Waiting for Signature File|*.wfs|All File|*.*";

                if (ofd.ShowDialog() == true)
                {
                    var lines = System.IO.File.ReadAllLines(ofd.FileName);

                    List<DataSig.Model.UiErc20TransferOffSigObj> ls = new List<DataSig.Model.UiErc20TransferOffSigObj>();
                    int i = 0;

                    foreach (var line in lines)
                    {
                        //ChainId,From,To,Nonce,GasLimit,MaxFeePerGas,MaxPriorityFeePerGas,EthWeiAmount,InputData
                        DataSig.Model.UiErc20TransferOffSigObj m = new DataSig.Model.UiErc20TransferOffSigObj();
                        var arr = line.Split(',');
                        m.ChainId = BigInteger.Parse(arr[0]);
                        m.From = arr[1];
                        m.To = arr[2];
                        m.Nonce = BigInteger.Parse(arr[3]);
                        m.GasLimit = BigInteger.Parse(arr[4]);
                        m.MaxFeePerGas = BigInteger.Parse(arr[5]);
                        m.MaxPriorityFeePerGas = BigInteger.Parse(arr[6]);
                        m.EthWeiAmount = BigInteger.Parse(arr[7]);
                        m.InputData = arr[8];

                        //and one 
                        var fun = new Nethereum.StandardTokenEIP20.ContractDefinition.TransferFunction();
                        var param = fun.DecodeInput(m.InputData);
                        m.TokenTo = param.To;
                        m.TokenAmount = param.Value;

                        i++;
                        m.IndexId = i;
                        m.IsSelected = true;

                        ls.Add(m);
                    }

                    DataGridERC20TokenTransferSig.ItemsSource = ls;
                }
                else
                {
                    ;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void ERC20TokenSigSelected_Click(object sender, RoutedEventArgs e)
        {
            //DataGridERC20TokenTransferSig
            try
            {
                if (DataGridERC20TokenTransferSig.ItemsSource != null)
                {
                    List<string> result = new List<string>();
                    List<DataSig.Model.UiErc20TransferOffSigObj> ls = (List<DataSig.Model.UiErc20TransferOffSigObj>)DataGridERC20TokenTransferSig.ItemsSource;
                    foreach (var m in ls)
                    {
                        if (!m.IsSelected) { continue; }

                        //string password;
                        //if (!Common.PasswordManager.GetPassword(m.From, out password))          //密码如果保存了，就不要再提示了！  //todo: 这里有问题，一般都是HD钱包！！！
                        //{
                        //    if (!Share.WindowPassword.GetPassword(this, m.From, "Get Password", out password))
                        //    {
                        //        m.SigData = "No Get Password";
                        //        result.Add("IndexId=" + m.IndexId.ToString() + "No Get Password");
                        //        continue;
                        //    }
                        //}
                        string Password;
                        var hdmodel = BLL.HDWallet.GetHdModel(m.From);        //如果是助记词，需要使用助记词的密码
                        if (hdmodel == null)
                        {
                            if (!Common.PasswordManager.GetPassword(m.From, out Password))          //密码如果保存了，就不要再提示了！  //todo: 这里有问题，一般都是HD钱包！！！
                            {
                                if (!WindowPassword.GetPassword(this, m.From, "Get Password", out Password))
                                {
                                    m.SigData = "No Get Password";
                                    result.Add("IndexId=" + m.IndexId.ToString() + "No Get Password");
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if (!Common.PasswordManager.GetPassword(hdmodel.MneAlias, out Password))          //密码如果保存了，就不要再提示了！  //todo: 这里有问题，一般都是HD钱包！！！
                            {
                                if (!WindowPassword.GetPassword(this, hdmodel.MneAlias, "Get Password", out Password))
                                {
                                    m.SigData = "No Get Password";
                                    result.Add("IndexId=" + m.IndexId.ToString() + "No Get Password");
                                    continue;
                                }
                            }
                        }

                        var account = new BLL.Address().GetAccount(m.From, Password);
                        if (account == null)
                        {
                            m.SigData = "No Get Account";
                            result.Add("IndexId=" + m.IndexId.ToString() + ": No Get Account");
                            continue;
                        }

                        var transaction = new Nethereum.Signer.Transaction1559(m.ChainId, m.Nonce, m.MaxPriorityFeePerGas, m.MaxFeePerGas, m.GasLimit, m.To, m.EthWeiAmount, m.InputData, null);
                        transaction.Sign(new EthECKey(account.PrivateKey));
                        string sigdata = transaction.GetRLPEncoded().ToHex(true);

                        m.SigData = sigdata;        //界面上也要显示
                        result.Add(sigdata);
                    }

                    if (result.Count > 0)
                    {
                        string BackUpPath = Share.ShareParam.BackUpDir;
                        string filename = System.IO.Path.Combine(BackUpPath, "SigData_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".sig");
                        System.IO.File.AppendAllLines(filename, result);
                        System.Diagnostics.Process.Start("Explorer", "/select," + filename);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
            }

        }

        private void ERC20TokenSigOne_Click(object sender, RoutedEventArgs e)
        {
            //DataGridERC20TokenTransferSig
            try
            {
                if (DataGridERC20TokenTransferSig.SelectedItem != null)
                {
                    var m = (DataSig.Model.UiErc20TransferOffSigObj)DataGridERC20TokenTransferSig.SelectedItem;

                    string password;

                    //if (!Share.WindowPassword.GetPassword(this, m.From, "Get Password", out password))
                    //{
                    //    return;
                    //}

                    var hdmodel = BLL.HDWallet.GetHdModel(m.From);        //如果是助记词，需要使用助记词的密码
                    if (hdmodel == null)
                    {
                        if (!Share.WindowPassword.GetPassword(this, m.From, "Get Password", out password))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!Share.WindowPassword.GetPassword(this, hdmodel.MneAlias, "Get Password", out password))
                        {
                            return;
                        }
                    }

                    var account = new BLL.Address().GetAccount(m.From, password);
                    if (account == null)
                    {
                        return;
                    }

                    var transaction = new Nethereum.Signer.Transaction1559(m.ChainId, m.Nonce, m.MaxPriorityFeePerGas, m.MaxFeePerGas, m.GasLimit, m.To, m.EthWeiAmount, m.InputData, null);
                    transaction.Sign(new EthECKey(account.PrivateKey));
                    string sigdata = transaction.GetRLPEncoded().ToHex(true);

                    m.SigData = sigdata;

                    MessageBox.Show("OK");
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
            }


        }


        private void EthLoad_Click(object sender, RoutedEventArgs e)
        {
            //DataGridEthTransferSig
            try
            {
                string BackUpPath = Share.ShareParam.BackUpDir;

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = BackUpPath;
                ofd.Filter = "Waiting for Signature File|*.wfs|All File|*.*";


                if (ofd.ShowDialog() == true)
                {
                    var lines = System.IO.File.ReadAllLines(ofd.FileName);

                    List<DataSig.Model.UiEthTransferSigObj> ls = new List<DataSig.Model.UiEthTransferSigObj>();
                    int i = 0;

                    foreach (var line in lines)
                    {
                        //ChainId,From,To,Nonce,GasLimit,MaxFeePerGas,MaxPriorityFeePerGas,EthWeiAmount,InputData
                        DataSig.Model.UiEthTransferSigObj m = new DataSig.Model.UiEthTransferSigObj();
                        var arr = line.Split(',');
                        m.ChainId = BigInteger.Parse(arr[0]);
                        m.From = arr[1];
                        m.To = arr[2];
                        m.Nonce = BigInteger.Parse(arr[3]);
                        m.GasLimit = BigInteger.Parse(arr[4]);
                        m.MaxFeePerGas = BigInteger.Parse(arr[5]);
                        m.MaxPriorityFeePerGas = BigInteger.Parse(arr[6]);
                        m.EthWeiAmount = BigInteger.Parse(arr[7]);
                        m.InputData = arr[8];

                        i++;
                        m.IndexId = i;
                        m.IsSelected = true;

                        ls.Add(m);
                    }

                    DataGridEthTransferSig.ItemsSource = ls;
                }
                else
                {
                    ;
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void EthSigSelected_Click(object sender, RoutedEventArgs e)
        {
            //DataGridEthTransferSig

            try
            {
                if (DataGridEthTransferSig.ItemsSource != null)
                {
                    List<string> result = new List<string>();
                    List<DataSig.Model.UiEthTransferSigObj> ls = (List<DataSig.Model.UiEthTransferSigObj>)DataGridEthTransferSig.ItemsSource;
                    foreach (var m in ls)
                    {
                        if (!m.IsSelected) { continue; }

                        //string password;
                        //if (!Common.PasswordManager.GetPassword(m.From, out password))          //密码如果保存了，就不要再提示了！  //todo: 这里有问题，一般都是HD钱包！！！
                        //{
                        //    if (!Share.WindowPassword.GetPassword(this, m.From, "Get Password", out password))
                        //    {
                        //        m.SigData = "No Get Password";
                        //        result.Add("IndexId=" + m.IndexId.ToString() + "No Get Password");
                        //        continue;
                        //    }
                        //}
                        string Password;
                        var hdmodel = BLL.HDWallet.GetHdModel(m.From);        //如果是助记词，需要使用助记词的密码
                        if (hdmodel == null)
                        {
                            if (!Common.PasswordManager.GetPassword(m.From, out Password))          //密码如果保存了，就不要再提示了！  //todo: 这里有问题，一般都是HD钱包！！！
                            {
                                if (!WindowPassword.GetPassword(this, m.From, "Get Password", out Password))
                                {
                                    m.SigData = "No Get Password";
                                    result.Add("IndexId=" + m.IndexId.ToString() + "No Get Password");
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if (!Common.PasswordManager.GetPassword(hdmodel.MneAlias, out Password))          //密码如果保存了，就不要再提示了！  //todo: 这里有问题，一般都是HD钱包！！！
                            {
                                if (!WindowPassword.GetPassword(this, hdmodel.MneAlias, "Get Password", out Password))
                                {
                                    m.SigData = "No Get Password";
                                    result.Add("IndexId=" + m.IndexId.ToString() + "No Get Password");
                                    continue;
                                }
                            }
                        }

                        var account = new BLL.Address().GetAccount(m.From, Password);//, m.ChainId
                        if (account == null)
                        {
                            m.SigData = "No Get Account";
                            result.Add("IndexId=" + m.IndexId.ToString() + ": No Get Account");
                            continue;
                        }

                        var transaction = new Nethereum.Signer.Transaction1559(m.ChainId, m.Nonce, m.MaxPriorityFeePerGas, m.MaxFeePerGas, m.GasLimit, m.To, m.EthWeiAmount, m.InputData, null);
                        transaction.Sign(new EthECKey(account.PrivateKey));
                        string sigdata = transaction.GetRLPEncoded().ToHex(true);

                        m.SigData = sigdata;        //界面上也要显示
                        result.Add(sigdata);
                    }

                    if (result.Count > 0)
                    {
                        string BackUpPath = Share.ShareParam.BackUpDir;
                        string filename = System.IO.Path.Combine(BackUpPath, "SigData_" + System.DateTime.Now.ToString("yyyyMMddmmHHss") + ".sig");
                        System.IO.File.AppendAllLines(filename, result);
                        System.Diagnostics.Process.Start("Explorer", "/select," + filename);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
            }

        }


        private void EthSigOne_Click(object sender, RoutedEventArgs e)
        {
            //DataGridEthTransferSig

            try
            {
                if (DataGridEthTransferSig.SelectedItem != null)
                {
                    var m = (DataSig.Model.UiEthTransferSigObj)DataGridEthTransferSig.SelectedItem;

                    string password;

                    //if (!Share.WindowPassword.GetPassword(this, m.From, "Get Password", out password))
                    //{
                    //    return;
                    //}

                    var hdmodel = BLL.HDWallet.GetHdModel(m.From);        //如果是助记词，需要使用助记词的密码
                    if (hdmodel == null)
                    {
                        if (!Share.WindowPassword.GetPassword(this, m.From, "Get Password", out password))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!Share.WindowPassword.GetPassword(this, hdmodel.MneAlias, "Get Password", out password))
                        {
                            return;
                        }
                    }


                    var account = new BLL.Address().GetAccount(m.From, password);
                    if (account == null)
                    {
                        return;
                    }

                    var transaction = new Nethereum.Signer.Transaction1559(m.ChainId, m.Nonce, m.MaxPriorityFeePerGas, m.MaxFeePerGas, m.GasLimit, m.To, m.EthWeiAmount, m.InputData, null);
                    transaction.Sign(new EthECKey(account.PrivateKey));
                    string sigdata = transaction.GetRLPEncoded().ToHex(true);

                    m.SigData = sigdata;

                    MessageBox.Show("OK");
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex); MessageBox.Show(ex.Message);
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
