using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Wallet.BLL
{

    /// <summary>
    /// https://etherscan.io/apis 的调用封装
    /// </summary>
    public static class EtherSacnApi
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        //public static string ApiKey = "XWCEQSHTE5E14HE88YY38S3ZDJ7RX5SIBJ";

        public static async Task<AddressTxState> GetAddressTxs(string address, string url)
        {
            //// https://api.etherscan.io/api?module=account&action=txlist&address=0xddbd2b932c763ba5b1b7ae3b362eac3e8d40121a&startblock=0&endblock=99999999&sort=asc&apikey=YourApiKeyToken
            //string url_eth = @"https://api.etherscan.io/api?module=account&action=txlist&address=" + address + @"&startblock=0&endblock=99999999&sort=asc&apikey=" + ApiKey;
            //string url_bnb  = @"https://api.bscscan.com/api?module=account&action=txlist&address=" + address + @"&startblock=1&endblock=99999999&sort=asc&apikey=" + ApiKey;

            //string url = url_eth;
            //if (Share.ShareParam.CurrentNodesType == Common.NodesTypeEnum.BinanceChain)
            //{
            //    url = url_bnb;
            //}

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AddressTxState>(responseBody);
                return result;
            }
            catch (HttpRequestException e)
            {
                log.Error("", e);
                return null;
            }
        }

        // {"blockNumber":"11021077","timeStamp":"1602244168","hash":"0x62d98e9dd80160e04b02b7c7da6c6c97960a64b2774267f3ce9ae95c67e42055","nonce":"486","blockHash":"0x2e0801d5530e346a7337b102ea5c818f438ec712cc717fa445f499026c259db0","transactionIndex":"191","from":"0x61176031135efe59300f1d513d3da6fb8f8a3533","to":"0xcfbec053a18dccf2e24d66047c19e7108c9cb8ad","value":"3588943508889936","gas":"30000","gasPrice":"85272001636","isError":"0","txreceipt_status":"1","input":"0x","contractAddress":"","cumulativeGasUsed":"12301014","gasUsed":"21000","confirmations":"83996"        },

        [JsonObject("AddressTxState")]
        public class AddressTxState
        {
            //"status":"1","message":"OK","result":
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 status;//":"1",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string message;//":

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public AddressTxDetail[] result;
        }


        [JsonObject("AddressTxDetail")]
        public class AddressTxDetail
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 blockNumber;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 timeStamp;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string hash;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 nonce;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string blockHash;//":"0x2e0801d5530e346a7337b102ea5c818f438ec712cc717fa445f499026c259db0",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 transactionIndex;//":"191",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string from;//":"0x61176031135efe59300f1d513d3da6fb8f8a3533",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string to;//":"0xcfbec053a18dccf2e24d66047c19e7108c9cb8ad",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public System.Numerics.BigInteger value;//":"3588943508889936",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 gas;//":"30000",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 gasPrice;//":"85272001636",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 isError;//":"0",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 txreceipt_status;//":"1",

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string input;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string contractAddress;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 cumulativeGasUsed;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 gasUsed;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public UInt64 confirmations;

        }


    }

}
