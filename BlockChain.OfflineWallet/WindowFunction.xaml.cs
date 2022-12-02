using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;

namespace BlockChain.OfflineWallet
{
    /// <summary>
    /// WindowFunction.xaml 的交互逻辑
    /// </summary>
    public partial class WindowFunction : Window
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WindowFunction()
        {
            InitializeComponent();

            DataGridFuncions.ItemsSource = BLL.AbiFunction.getAllFunction();
        }

        private void AddFunction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // FunctionFullName, FunctionFullNameHash, FunctionFullNameHash4, IsSysDefine, IsTestOk

                string f = this.TextBoxFunctionFullName.Text.Trim();
                Model.AbiFunction m = new Model.AbiFunction();
                m.FunId = Guid.NewGuid();
                m.FunctionFullName = f;
                m.FunctionFullNameHash = OfflineWallet.DataSig.InputDataHelper.getFunctionHash(f);
                m.FunctionFullNameHash4 = DataSig.InputDataHelper.getSha3SignatureFunction(f).ToHex(true);
                m.Remark = this.TextBoxFunctionRemark.Text.Trim();
                m.IsSysDefine = false;
                m.IsTestOk = false;

                DAL.AbiFunction.Insert(Share.ShareParam.DbConStr, m);

                DataGridFuncions.ItemsSource = BLL.AbiFunction.getAllFunction();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void AddParamer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LabelCurrentFunction.Content == null || string.IsNullOrEmpty(LabelCurrentFunction.Content.ToString()))
                {
                    return;
                }

                string f = LabelCurrentFunction.Content.ToString();
                Guid funid = (Guid)LabelCurrentFunction.Tag;

                Nethereum.ABI.ABIType abitype = Nethereum.ABI.ABIType.CreateABIType(TextBoxParamerType.Text);       //如果报异常，类型就无法处理，

                var FunctionFullNameHash = OfflineWallet.DataSig.InputDataHelper.getFunctionHash(f);
                var m = DAL.AbiFunction.GetModel(Share.ShareParam.DbConStr, funid);
                if (m == null || m.IsSysDefine) { 
                    MessageBox.Show("Sys Define Function can not be modified!");
                    return; 
                }

                Model.AbiParamer pm = new Model.AbiParamer();                
                pm.ParamerName = TextBoxParamerName.Text;
                pm.ParamerOrder = int.Parse(TextBoxParamerOrder.Text);
                pm.ParamerType = abitype.ToString();                    // TextBoxParamerType.Text;
                pm.FunId = funid;
                DAL.AbiParamer.Insert(Share.ShareParam.DbConStr, pm);

                DataGridParamers.ItemsSource = BLL.AbiFunction.getAbiParamer(m.FunId);
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

        private void DeleteFun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridFuncions.SelectedItem != null)
                {
                    if (DataGridFuncions.SelectedItem is Model.AbiFunction)
                    {
                        var m = DataGridFuncions.SelectedItem as Model.AbiFunction;                    

                        if (m.IsSysDefine)
                        {
                            MessageBox.Show("System Define, Can not delete。");
                            return;
                        }

                        DAL.AbiFunction.Delete(Share.ShareParam.DbConStr, m.FunId);

                        DataGridFuncions.ItemsSource = BLL.AbiFunction.getAllFunction();
                        DataGridParamers.ItemsSource = new List<Model.View_AbiParamer>();
                        LabelCurrentFunction.Content = Guid.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteParamer_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (DataGridParamers.SelectedItem != null)
                {
                    if (DataGridParamers.SelectedItem is Model.View_AbiParamer)
                    {
                        var m = DataGridParamers.SelectedItem as Model.View_AbiParamer;

                        if (m.IsSysDefine) {
                            MessageBox.Show("System Define, Can not delete。");
                            return;
                        }

                        DAL.AbiParamer.Delete(Share.ShareParam.DbConStr, m.FunId, m.ParamerOrder);

                        DataGridParamers.ItemsSource = BLL.AbiFunction.getAbiParamer(m.FunId);
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
            try
            {

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

                    TextBoxInputData.Text = DataSig.InputDataHelper.getInputData(LabelCurrentFunction.Content.ToString(), pl, vl.ToArray());
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void IniFun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLL.AbiFunction.IniSysAbiFunction();

                DataGridFuncions.ItemsSource = BLL.AbiFunction.getAllFunction();
                DataGridParamers.ItemsSource = new List<Model.View_AbiParamer>();
                LabelCurrentFunction.Content = Guid.Empty;

                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
