using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;

namespace CodeforcesPlatform
{
    public class CodeforcesAPI
    {
        private const string HOST_URL = "http://codeforces.com";
        private const string CONTEST_LIST_URL = HOST_URL + "/api/contest.list";
        private const string CONTEST_STANDINGS_URL = HOST_URL + "/api/contest.standings";
        private const string CONTEST_STATUS_URL = HOST_URL + "/api/contest.status";

        static CodeforcesAPI()
        {
        }

        public static string Test()
        {
            return HttpClientSingleton.DoGet("http://christmas.dreamwings.cn/");
        }
    }
}
