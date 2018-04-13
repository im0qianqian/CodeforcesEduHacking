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
    public class HttpContentTypes
    {
        public enum HttpContentTypeEnum
        {
            JSON,
            FORM
        }

        public static string GetContentType(HttpContentTypeEnum type)
        {
            string typeStr = "";
            switch (type)
            {
                case HttpContentTypeEnum.JSON:
                    typeStr = "application/json";
                    break;
                case HttpContentTypeEnum.FORM:
                    typeStr = "application/x-www-form-urlencoded";
                    break;
            }
            return typeStr;
        }

    }
    public class HttpClientSingleton
    {
        public static readonly HttpClient http = null;
        static HttpClientSingleton()
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            http = new HttpClient(handler);
        }

        /// <summary>
        /// post 提交json格式参数
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postJson">json字符串</param>
        /// <returns></returns>
        public static string DoPost(string url, string postJson)
        {
            HttpContent content = new StringContent(postJson);
            content.Headers.ContentType = new MediaTypeHeaderValue(HttpContentTypes.GetContentType(HttpContentTypes.HttpContentTypeEnum.JSON));
            return DoPost(url, content);
        }

        /// <summary>
        /// post 提交json格式参数
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="argDic">参数字典</param>
        /// <param name="headerDic">请求头字典</param>
        /// <returns></returns>
        public static string DoPost(string url, Dictionary<string, string> argDic, Dictionary<string, string> headerDic)
        {
            //argDic.ToSortUrlParamString();
            //var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(argDic);
            //HttpContent content = new StringContent(jsonStr);
            //content.Headers.ContentType = new MediaTypeHeaderValue(HttpContentTypes.GetContentType(HttpContentTypes.HttpContentTypeEnum.JSON));
            //if (headerDic != null)
            //{
            //    foreach (var item in headerDic)
            //    {
            //        content.Headers.Add(item.Key, item.Value);
            //    }
            //}
            //return DoPost(url, content);
            return "Hello World!";
        }

        /// <summary>
        /// HttpClient POST 提交
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="content">HttpContent</param>
        /// <returns></returns>
        public static string DoPost(string url, HttpContent content)
        {
            try
            {
                var response = http.PostAsync(url, content).Result;
                //确保HTTP成功状态值
                response.EnsureSuccessStatusCode();
                //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                var reJson = response.Content.ReadAsStringAsync().Result;
                return reJson;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// HttpClient实现Get请求
        /// </summary>
        public static string DoGet(string url)
        {
            try
            {
                var response = http.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return ex.Message + "," + ex.Source + "," + ex.StackTrace;
            }
        }

        /// <summary>
        /// HttpClient实现Get请求
        /// <param name="arg">参数字典</param>
        /// </summary>
        public static string DoGet(string url, IDictionary<string, string> arg)
        {
            string argStr = "?";
            foreach (var item in arg)
            {
                argStr += item.Key + "=" + item.Value + "&";
            }
            argStr = argStr.TrimEnd('&');
            url = url + argStr;
            return DoGet(url);
        }

        /// <summary>
        /// HttpClient Get 提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="arg"></param>
        /// <param name="headerDic"></param>
        /// <returns></returns>
        public static string DoGet(string url, IDictionary<string, string> arg, IDictionary<string, string> headerDic)
        {
            try
            {
                if (arg != null && arg.Count > 0)
                {
                    string argStr = "?";
                    foreach (var item in arg)
                    {
                        argStr += item.Key + "=" + item.Value + "&";
                    }
                    argStr = argStr.TrimEnd('&');
                    url = url + argStr;
                }

                if (headerDic != null)
                {
                    foreach (var item in headerDic)
                    {
                        http.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                //await异步等待回应
                var response = http.GetStringAsync(url).Result;
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}
