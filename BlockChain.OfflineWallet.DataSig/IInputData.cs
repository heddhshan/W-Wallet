using Nethereum.ABI;
using Nethereum.Hex.HexConvertors.Extensions;

namespace BlockChain.OfflineWallet.DataSig 
{ 

    /// <summary>
    /// 使用 dll （可以是动态的），得到 function 调用的 inputdata 值！ 此功能暂时没做到离线钱包里面。
    /// </summary>
    public interface IInputData
    {
        /// <summary>
        /// 得到参数列表
        /// </summary>
        /// <returns></returns>
        public List<Nethereum.ABI.Model.Parameter> getParameters();

        /// <summary>
        /// 从inputdata中解析出参数值
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public List<object> loadParameterValues(string inputData);


        /// <summary>
        /// 得到 InputData， 输入参数值
        /// </summary>
        /// <param name="functionInput"></param>
        /// <returns></returns>
        public string getInputData(params object[] functionInput);

        /// <summary>
        /// 得到function的签名，例如 transfer(address, uint256) 
        /// </summary>
        /// <returns></returns>
        public string getFunctionName();

    }




}