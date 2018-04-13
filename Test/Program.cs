using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Process process = new Process();
            try
            {
                process.StartInfo.UseShellExecute = false;   // Shell的使用 
                process.StartInfo.CreateNoWindow = true;   //是否在新窗口中启动该进程的值
                //process.StartInfo.RedirectStandardInput = true;  // 重定向输入流   
                //process.StartInfo.RedirectStandardOutput = true;  //重定向输出流   
                //process.StartInfo.RedirectStandardError = true;  //重定向错误流 

                //process.StartInfo.FileName = "g++.exe";
                //process.StartInfo.Arguments = "test.cpp -pipe -o test.exe";
                //process.Start();
                //string output = process.StandardOutput.ReadToEnd();
                //string error = process.StandardError.ReadToEnd();
                //Console.WriteLine(output);
                //Console.WriteLine(error);
                //process.Close();

                //process.StartInfo.FileName = "test.exe";
                //process.Start();
                //process.StandardInput.WriteLine("\n33\n1");
                //var output = process.StandardOutput.ReadToEnd();
                //var error = process.StandardError.ReadToEnd();
                //Console.WriteLine(output);
                //Console.WriteLine(error);
                //Console.WriteLine(process.Id);
                //Console.WriteLine((process.ExitTime - process.StartTime).TotalSeconds);

                var aa = Process.Start("notepad.exe");
                //aa.Close();
                aa.Kill();
                aa.WaitForExit();
                Console.WriteLine("The End!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
