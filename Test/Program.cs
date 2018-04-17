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
            //try
            //{
            //    //CodeforcesAPI a = new CodeforcesAPI();
            //    ////var b = a.GetContestStandings(962, true, 1, 2, "im0qianqian", -1);
            //    //var b = a.GetContestList();
            //    //foreach (var item in b["result"])
            //    //{
            //    //    Console.WriteLine(item["id"] + " " + item["name"]);
            //    //}
            //    //Console.WriteLine(b);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}


            try
            {
                CodeforcesAPI a = new CodeforcesAPI();
                string language, code;
                code = a.GetCodeBySubmissionId(962, 37395478, out language);
                Console.WriteLine(language);
                Console.WriteLine(code);
                //var b = new CompilingEnvironment.GNUCompiler();
                //string end = b.Execute(code, "4\n-5 R\n0 P\n3 P\n7 B");
                //Console.WriteLine(end);
                //Console.WriteLine(b.ExcuteTotalTime.TotalSeconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            //try
            //{
            //    var a = new Dictionary<string, string>();
            //    a.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            //    HttpClientSingleton.SetRequestHeaders(a);
            //    string url = "http://christmas.dreamwings.cn/";
            //    string data = HttpClientSingleton.DoGet(url);
            //    Console.WriteLine(data);

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}


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
