using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlockChain.Share
{
    public static class TranslationOnLine
    {
        //
    }


    public class MicrosoftTextTranslation
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class translation
        {
            [JsonInclude]
            public string text { get; set; }

            [JsonInclude]
            public string to { get; set; }

        }
        public class root
        {
            [JsonInclude]
            public List<translation> translations;
        }

        public static root getObject(string json)
        {
            //{"error":{"code":401000,"message":"The request is not authorized because credentials are missing or invalid."}}

            //ok
            json = json.Trim();
            if (json.StartsWith('['))
            {
                json = json.Substring(1, json.Length - 2);
            }
            var result = System.Text.Json.JsonSerializer.Deserialize<root>(json)!;
            return result;
        }


        // 微软的某些 demo， 是由问题的， 这里就要复制这个url 代码： https://docs.microsoft.com/zh-cn/azure/cognitive-services/translator/translator-text-apis?tabs=csharp

        #region 参数配置，来自配置页面，密钥和终结点

        ///// <summary>
        ///// 这个不能写到代码中，要删除
        ///// </summary>
        //private static readonly string key = "0ae3530fc6474a038493668f70c19beb";                        //todo: delete 0ae3530fc6474a038493668f70c19beb
        //private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";      //https://api.cognitive.microsofttranslator.com/
        //private static readonly string location = "eastasia";

        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";      //https://api.cognitive.microsofttranslator.com/

        public static string key
        {
            get { return Properties.Settings.Default.MsTranslationKey; }
            set
            {
                Properties.Settings.Default.MsTranslationKey = value;
                Properties.Settings.Default.Save();

            }
        }

        public static string location
        {
            get { return Properties.Settings.Default.MsTranslationLocation; }
            set
            {
                Properties.Settings.Default.MsTranslationLocation = value;
                Properties.Settings.Default.Save();
            }
        }



        #endregion

        /// <summary>
        /// 使用 http reset api,  把中文翻译成英文, 本系统只需要处理这种就可以了！
        /// </summary>
        /// <param name="textToTranslate"></param>
        /// <returns></returns>
        public static async Task<string> TranslatByZhToEn(string textToTranslate)
        {
            try
            {
                string route = "/translate?api-version=3.0&from=zh-Hans&to=en";
                object[] body = new object[] { new { Text = textToTranslate } };
                var requestBody = JsonConvert.SerializeObject(body);

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(endpoint + route);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                    request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                    string result = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(result);

                    return getObject(result).translations[0].text;      //[{"translations":[{"text":"Chinese","to":"en"}]}]
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return "M?" + textToTranslate;
            }
        }

        public static async Task<string> TranslatFromZh(string textToTranslate, string to)
        {
            try
            {
                string route = "/translate?api-version=3.0&from=zh-Hans&to=" + to;
                object[] body = new object[] { new { Text = textToTranslate } };
                var requestBody = JsonConvert.SerializeObject(body);

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(endpoint + route);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                    request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                    string result = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(result);
                    if (!string.IsNullOrEmpty(result)) {
                        try
                        {
                            return getObject(result).translations[0].text;      //[{"translations":[{"text":"Chinese","to":"en"}]}]
                        }
                        catch //(Exception thisex) 
                        {
                            log.Error(to + " Can Not Get TranslateText, "+ "Error Info: " + result + " ; Original Text Is " + textToTranslate);
                            return "M?1?" + textToTranslate;
                        }
                    }
                    else
                    {
                        log.Error(to + " Can Not Get TranslateText, Original Text Is " + textToTranslate);
                        return "M?2?" + textToTranslate;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return "M?3?" + textToTranslate;
            }
        }


        //public static async Task Test1()
        //{
        //    // Input and output languages are defined as parameters.
        //    string route = "/translate?api-version=3.0&from=en&to=fr&to=zu";
        //    string textToTranslate = "I would really like to drive your car around the block a few times!";
        //    object[] body = new object[] { new { Text = textToTranslate } };
        //    var requestBody = JsonConvert.SerializeObject(body);

        //    using (var client = new HttpClient())
        //    using (var request = new HttpRequestMessage())
        //    {
        //        // Build the request.
        //        request.Method = HttpMethod.Post;
        //        request.RequestUri = new Uri(endpoint + route);
        //        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        //        request.Headers.Add("Ocp-Apim-Subscription-Key", key);


        //        request.Headers.Add("Ocp-Apim-Subscription-Region", location);

        //        // Send the request and get response.
        //        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
        //        // Read response as a string.
        //        string result = await response.Content.ReadAsStringAsync();
        //        //Console.WriteLine(result);
        //    }
        //}


        //public static async Task Test2()
        //{

        //    // Input and output languages are defined as parameters.
        //    string route = "/translate?api-version=3.0&from=en&to=sw&to=it";
        //    string textToTranslate = "Hello, friend! What did you do today?";
        //    object[] body = new object[] { new { Text = textToTranslate } };
        //    var requestBody = JsonConvert.SerializeObject(body);

        //    using (var client = new HttpClient())
        //    using (var request = new HttpRequestMessage())
        //    {
        //        // Build the request.
        //        request.Method = HttpMethod.Post;
        //        request.RequestUri = new Uri(endpoint + route);
        //        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        //        request.Headers.Add("Ocp-Apim-Subscription-Key", key);
        //        request.Headers.Add("Ocp-Apim-Subscription-Region", location);

        //        // Send the request and get response.
        //        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
        //        // Read response as a string.
        //        string result = await response.Content.ReadAsStringAsync();
        //        //Console.WriteLine(result);
        //    }
        //}


        #region MicrosoftTranslationV2 WebService 这是老版本 不能用！！！

        ///// <summary>
        ///// 得到译文，使用 WebService ，版本是 V2 ！
        ///// </summary>
        ///// <param name="fromText"></param>
        ///// <param name="fromLanguage"></param>
        ///// <param name="toLanguage"></param>
        ///// <returns></returns>
        //public static string Translat(string fromText, string fromLanguage, string toLanguage)
        //{
        //    string result;
        //    MicrosoftTranslationV2.LanguageServiceClient client = new MicrosoftTranslationV2.LanguageServiceClient();
        //    //result = client.Translate(key, fromText, "en", "zh-CHS", "text/html", "general");
        //    result = client.Translate(key, fromText, fromLanguage, toLanguage, "text/html", "ethereum", "general");
        //    // richTextBox2.Text = result;
        //    return result;
        //}


        //public static string[] GetLanguages()
        //{
        //    //翻译的语言支持 https://docs.microsoft.com/zh-cn/azure/cognitive-services/translator/language-support
        //    MicrosoftTranslationV2.LanguageServiceClient client = new MicrosoftTranslationV2.LanguageServiceClient();

        //    string[] Languagescode = client.GetLanguagesForTranslate(key);                                      //得到语言
        //    string[] LanguagesName = client.GetLanguageNames(key, "zh-CHS", Languagescode, true);               //获取的语言转译成中文

        //    foreach (var l in LanguagesName)
        //    {
        //        Console.WriteLine(l);
        //    }

        //    return Languagescode;
        //}

        #endregion

    }

}
