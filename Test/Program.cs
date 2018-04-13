using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeforcesPlatform;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var a = new Dictionary<string, string>();
                a.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
                string url = "http://christmas.dreamwings.cn/";
                string data = HttpClientSingleton.DoGet(url, null,a);
                Console.WriteLine(data);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            //try
            //{
            //    StreamReader sr = new StreamReader("main.cpp");
            //    string code = sr.ReadToEnd();

            //    var a = new CompilingEnvironment.GNUCompiler();
            //    string end = a.Execute(code, "44");
            //    Console.WriteLine(end);
            //    Console.WriteLine(a.ExcuteTotalTime.TotalSeconds);

            //    end = a.Execute(code, "55");
            //    Console.WriteLine(end);
            //    Console.WriteLine(a.ExcuteTotalTime.TotalSeconds);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }
    }
}
