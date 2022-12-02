using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RLP;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Signer;
using Nethereum.Util;
using Org.BouncyCastle.Math;
using System;
using System.Security.Policy;


namespace BlockChain.Common
{
    public class Web3Helper
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        //public static string LegacyTransactionGetAddressFrom(string _RlpMsg)
        //{
        //    return Nethereum.Signer.TransactionVerificationAndRecovery.GetSenderAddress(_RlpMsg);
        
        //Nethereum.Signer.LegacyTransaction thistx = new LegacyTransaction(_RlpMsg.HexToByteArray());
        //    var accountSenderRecovered2 = EthECKey.RecoverFromSignature(thistx.Signature, thistx.RawHash).GetPublicAddress();       //这种写法也是对的
        //    return accountSenderRecovered2;
        //}

        //public static string Eip1559TransactionGetAddressFrom(string _RlpMsg)
        //{
        //  return  Nethereum.Signer.TransactionVerificationAndRecovery.GetSenderAddress(_RlpMsg);

        //    //Nethereum.Signer.Transaction1559 thistx = Nethereum.Signer.Transaction1559Encoder.Current.Decode(_RlpMsg.HexToByteArray());
        //    //var accountSenderRecovered2 = EthECKey.RecoverFromSignature(thistx.Signature, thistx.RawHash).GetPublicAddress();     //这里是错的
        //    //return accountSenderRecovered2;
        //}

        public static string TransactionGetAddressFrom(string _RlpMsg)
        {
            return Nethereum.Signer.TransactionVerificationAndRecovery.GetSenderAddress(_RlpMsg);
        }


        public static string GetTxHashFrom(string _RlpMsg)
        {
            Nethereum.Signer.EthereumMessageSigner m = new Nethereum.Signer.EthereumMessageSigner();
            var h1 = m.Hash(_RlpMsg.HexToByteArray());
            var txid = h1.ToHex(true);
            return txid;
        }



        //把签名数据分解了！
        public static Transaction1559 GetTransaction1559(string _rlp)
        {
            return Nethereum.Signer.Transaction1559Encoder.Current.Decode(_rlp.HexToByteArray());

            //try
            //{
            //    FunctionData f = new FunctionData();

            //    Nethereum.Signer.Transaction1559 tx2 = Nethereum.Signer.Transaction1559Encoder.Current.Decode(_rlp.HexToByteArray());
            //    f.Nonce = tx2.Nonce;
            //    f.Gas = tx2.GasLimit;
            //    f.AmountToSend = (System.Numerics.BigInteger)tx2.Amount;
            //    f.InputData = tx2.Data;
            //    f.MaxFeePerGas = tx2.MaxFeePerGas;
            //    f.MaxPriorityFeePerGas = tx2.MaxPriorityFeePerGas;
                
            //    return f;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("", ex);

            //    return null;
            //}
        }

        //public static string GetLegacyTxHashFrom(string _RlpMsg)
        //{
        //    Nethereum.Signer.EthereumMessageSigner m = new Nethereum.Signer.EthereumMessageSigner();
        //    var h1 = m.Hash(_RlpMsg.HexToByteArray());
        //    var txid = h1.ToHex(true);
        //    return txid;
        //}

        //public static string GetEip1559TxHashFrom(string _RlpMsg)
        //{
        //    Nethereum.Signer.EthereumMessageSigner m = new Nethereum.Signer.EthereumMessageSigner();
        //    var h1 = m.Hash(_RlpMsg.HexToByteArray());
        //    var txid = h1.ToHex(true);
        //    return txid;
        //}

        ///// <summary>
        ///// TODO: 通过签名恢复地址，用于日志，以及地址拥有权（私钥拥有）验证。很有用！
        ///// </summary>
        ///// <param name="signature"></param>
        ///// <returns></returns>
        //public static string EcRecoverAddress(string signature, string hash)
        //{
        //    try
        //    {
        //        if (!signature.StartsWith("0x"))
        //        {
        //            signature = "0x" + signature;
        //        }

        //        //var signature =
        //        //        "0x0976a177078198a261faf206287b8bb93ebb233347ab09a57c8691733f5772f67f398084b30fc6379ffee2cc72d510fd0f8a7ac2ee0162b95dc5d61146b40ffa1c";
        //        //  var text = "test";
        //        var hasher = new Sha3Keccack();
        //        //var hash = hasher.CalculateHash(text);
        //        var signer = new EthereumMessageSigner();
        //        var account = signer.EcRecover(hash.HexToByteArray(), signature);
        //        //Assert.Equal("0x12890d2cce102216644c59dae5baed380d84830c", account.EnsureHexPrefix().ToLower());
        //        return account;
        //    }
        //    catch (Exception ex)
        //    {
        //        return string.Empty;
        //    }
        //}


        ///// <summary>
        ///// 得到最新区块数
        ///// </summary>
        ///// <param name="web3Url"></param>
        ///// <returns></returns>
        //public static System.Numerics.BigInteger GetLastBlockNuber(string web3Url)
        //{
        //    try
        //    {
        //        var web3 = new Nethereum.Web3.Web3();
        //        var Block = web3.Eth.Blocks.GetBlockNumber.SendRequestAsync(web3Url).Result;
        //        //BlockTask.Wait();
        //        return Block;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("GetLastBlockNuber", ex);
        //        return 0;
        //    }
        //}



        public async static System.Threading.Tasks.Task<System.Numerics.BigInteger> GetLastBlockNuber(string web3Url)
        {
            //try { } catch (Exception ex) { }
            var web3 = new Nethereum.Web3.Web3(web3Url);
            var block = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            return block.Value;
        }

        public static System.Numerics.BigInteger GetNowBlockNuber(string web3Url)
        {
            //try { } catch (Exception ex) { }
            var web3 = new Nethereum.Web3.Web3(web3Url);
            var block = web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().Result;
            return block.Value;
        }

        //public async static System.Threading.Tasks.Task<System.Numerics.BigInteger> GetLastGasPrice(string web3Url)
        //{
        //    var web3 = new Nethereum.Web3.Web3(web3Url);
        //    var GasPrice = await web3.Eth.GasPrice.SendRequestAsync();
        //    return GasPrice.Value;
        //}

        public static System.Numerics.BigInteger GetNowGasPrice(string web3Url)
        {
            var web3 = new Nethereum.Web3.Web3(web3Url);
            var GasPrice = web3.Eth.GasPrice.SendRequestAsync().Result;
            return GasPrice.Value;
        }

        public static System.Numerics.BigInteger GetChainId(string web3Url)
        {
            var web3 = new Nethereum.Web3.Web3(web3Url);
            var id = web3.Eth.ChainId.SendRequestAsync().Result;
            return id.Value;
        }

        //public async static System.Threading.Tasks.Task<double> GetLastEther(string web3Url, string address)
        //{
        //    var web3 = new Nethereum.Web3.Web3(web3Url);
        //    var amount = (double)((await web3.Client.Eth.GetBalance.SendRequestAsync(address)).Value);
        //    amount = amount / Math.Pow(10, 18);
        //    return amount;
        //}

        public static double GetNowEther(string web3Url, string address)
        {
            var web3 = new Nethereum.Web3.Web3(web3Url);
            var amount = (double)((web3.Eth.GetBalance.SendRequestAsync(address)).Result.Value);
            amount = amount / Math.Pow(10, 18);
            return amount;
        }

        //public async static System.Threading.Tasks.Task<double> GetLastTokenAmount(string web3Url, string erc20Token, string address)
        //{
        //    var web3 = new Nethereum.Web3.Web3(web3Url);
        //    Nethereum.StandardTokenEIP20.StandardTokenService s = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, erc20Token);
        //    var amount = (double)await s.BalanceOfQueryAsync(address);
        //    var d = (double)await s.DecimalsQueryAsync();
        //    amount = amount / Math.Pow(10, d);
        //    return amount;
        //}

        public static double GetNowTokenAmount(string web3Url, string erc20Token, string address)
        {
            var web3 = new Nethereum.Web3.Web3(web3Url);
            Nethereum.StandardTokenEIP20.StandardTokenService s = new Nethereum.StandardTokenEIP20.StandardTokenService(web3, erc20Token);
            var amount = (double)s.BalanceOfQueryAsync(address).Result;
            var d = (double)s.DecimalsQueryAsync().Result;
            amount = amount / Math.Pow(10, d);
            return amount;
        }


        public static async Task<System.Numerics.BigInteger> GetGasLimit(string web3url, string from, string to, string inputdata)
        {
            var client = new Nethereum.JsonRpc.Client.RpcClient(new Uri(web3url));
            var ethEstimateGas = new EthEstimateGas(client);
            var transactionInput = new CallInput();
            transactionInput.Data = inputdata;
            transactionInput.To = to;
            transactionInput.From = from;

            var result = await ethEstimateGas.SendRequestAsync(transactionInput);

            return result.Value;
        }


        public static bool TestWeb3Url(string url)
        {
            try
            {
                var net = new Nethereum.JsonRpc.Client.RpcClient(new Uri(url));
                var web3 = new Nethereum.Web3.Web3(net);

                log.Info("Web3 Url: " + url);
                log.Info("ChainID: " + web3.Eth.ChainId.SendRequestAsync().Result.Value.ToString());

                var blocknum = web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().Result;
                if (blocknum.Value >= 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("TestNet", ex);
            }
            return false;
        }




    }


    //public class FunctionData: FunctionMessage { 

    //    public string InputData { get; set; }
    
    //}
}
