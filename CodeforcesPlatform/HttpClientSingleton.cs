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
            httpClient = new HttpClient(handler);
        }

        /// <summary>
        /// HttpClient 实现 Get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DoGet(string url)
        {
            try
            {
                return httpClient.GetStringAsync(url).Result;
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
        public static string DoGet(string url, IDictionary<string, string> args)
        {
            if (args != null)
            {
                string argStr = "?";
                foreach (var item in args)
                {
                    argStr += item.Key + "=" + item.Value + "&";
                }
                argStr = argStr.TrimEnd('&');
                url += argStr;
            }
            return DoGet(url);
        }

        public static string DoGet(string url, IDictionary<string, string> args, IDictionary<string, string> headerDic)
        {
            if (headerDic != null)
            {
                httpClient.DefaultRequestHeaders.Clear();
                foreach (var item in headerDic)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            return DoGet(url, args);
        }
    }
}
