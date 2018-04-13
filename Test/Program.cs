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
                StreamReader sr = new StreamReader("main.cpp");
                string code = sr.ReadToEnd();

                var a = new CompilingEnvironment.GNUCompiler();
                string end = a.Execute(code, "44");
                Console.WriteLine(end);
                Console.WriteLine(a.ExcuteTotalTime.TotalSeconds);

                end = a.Execute(code, "55");
                Console.WriteLine(end);
                Console.WriteLine(a.ExcuteTotalTime.TotalSeconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
