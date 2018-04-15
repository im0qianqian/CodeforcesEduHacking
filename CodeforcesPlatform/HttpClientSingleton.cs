using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodeforcesPlatform
{
    public class HttpClientSingleton
    {
        private static readonly HttpClient httpClient = null;

        static HttpClientSingleton()
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            httpClient = new HttpClient(handler);
            SetRequestHeaders(new Dictionary<string, string>() {
                { "User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36" }
            });
        }

        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <param name="headerDic"></param>
        public static void SetRequestHeaders(IDictionary<string, string> headerDic)
        {
            if (headerDic != null)
            {
                httpClient.DefaultRequestHeaders.Clear();
                foreach (var item in headerDic)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
        }
        /// <summary>
        /// HttpClient 实现 Get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> DoGetAsync(string url)
        {
            try
            {
                return await httpClient.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// HttpClient 实现 Get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="args">参数字典</param>
        /// <returns></returns>
        public static async Task<string> DoGetAsync(string url, IDictionary<string, string> args)
        {
            if (args != null && args.Count > 0)
            {
                string argStr = "?";
                foreach (var item in args)
                {
                    argStr += item.Key + "=" + item.Value + "&";
                }
                argStr = argStr.TrimEnd('&');
                url += argStr;
            }
            return await DoGetAsync(url);
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}
