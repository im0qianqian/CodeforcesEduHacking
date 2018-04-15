using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AngleSharp;
using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodeforcesPlatform
{
    public class CodeforcesAPI
    {
        private const string HOST_URL = "http://codeforces.com";
        private const string CONTEST_LIST_URL = HOST_URL + "/api/contest.list";
        private const string CONTEST_STANDINGS_URL = HOST_URL + "/api/contest.standings";
        private const string CONTEST_STATUS_URL = HOST_URL + "/api/contest.status";
        private const string PROBLEMSET_PROBLEMS_URL = HOST_URL + "/api/problemset.problems";
        private const string CONTEST_SUBMISSION_URL = HOST_URL + "/contest/{0}/submission/{1}";

        public CodeforcesAPI()
        {
        }

        /// <summary>
        /// 查询比赛列表
        /// </summary>
        /// <param name="gym">是否显示 GYM 的比赛</param>
        /// <returns>返回 JSON(JObject) 格式的比赛列表</returns>
        public async Task<JObject> GetContestListAsync(bool gym = false)
        {
            try
            {
                var getParams = new Dictionary<string, string>();
                if (gym != false)
                    getParams.Add("gym", "true");
                var jsonData = await HttpClientSingleton.DoGetAsync(CONTEST_LIST_URL, getParams);
                return (JObject)JsonConvert.DeserializeObject(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 返回指定比赛的提交（可以返回指定用户的提交）
        /// </summary>
        /// <param name="contestId">比赛 ID</param>
        /// <param name="handle">用户 ID</param>
        /// <param name="from">起始索引</param>
        /// <param name="count">返回条目</param>
        /// <returns>返回 JSON(JObject) 格式的提交列表，按提交 ID 递减顺序排序</returns>
        public async Task<JObject> GetContestStatusAsync(int contestId, string handle = null, int from = -1, int count = -1)
        {
            try
            {
                var getParams = new Dictionary<string, string>();
                getParams.Add("contestId", contestId.ToString());
                if (handle != null)
                    getParams.Add("handle", handle);
                if (from != -1)
                    getParams.Add("from", from.ToString());
                if (count != -1)
                    getParams.Add("count", count.ToString());
                var jsonData = await HttpClientSingleton.DoGetAsync(CONTEST_STATUS_URL, getParams);
                return (JObject)JsonConvert.DeserializeObject(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询 Codeforces 指定比赛的榜单
        /// </summary>
        /// <param name="contestId">比赛 ID</param>
        /// <param name="showUnofficial">是否显示非官方数据</param>
        /// <param name="from">起始索引</param>
        /// <param name="count">返回条目</param>
        /// <param name="handles">查询的用户 ID，多个的话中间用分号分隔</param>
        /// <param name="room">房间号</param>
        /// <returns>返回 JSON(JObject) 格式的比赛榜单</returns>
        public async Task<JObject> GetContestStandingsAsync(int contestId, bool showUnofficial = false, int from = -1, int count = -1, string handles = null, int room = -1)
        {
            try
            {
                var getParams = new Dictionary<string, string>();
                getParams.Add("contestId", contestId.ToString());
                if (showUnofficial)
                    getParams.Add("showUnofficial", "true");
                if (from != -1)
                    getParams.Add("from", from.ToString());
                if (count != -1)
                    getParams.Add("count", count.ToString());
                if (handles != null)
                    getParams.Add("handles", handles);
                if (room != -1)
                    getParams.Add("room", room.ToString());
                var jsonData = await HttpClientSingleton.DoGetAsync(CONTEST_STANDINGS_URL, getParams);
                return (JObject)JsonConvert.DeserializeObject(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据竞赛 ID 以及提交 ID 查询代码内容
        /// </summary>
        /// <param name="contestId">竞赛 ID</param>
        /// <param name="submissionId">提交 ID</param>
        /// <param name="language">返回所使用的编程语言</param>
        /// <returns>返回指定 ID 的提交代码</returns>
        public string GetCodeBySubmissionId(int contestId, int submissionId, out string language)
        {
            try
            {
                var url = string.Format(CONTEST_SUBMISSION_URL, contestId, submissionId);
                var html = HttpClientSingleton.DoGetAsync(url).Result;
                var parser = new HtmlParser();
                var document = parser.Parse(html);
                var preCode = document.QuerySelector("pre");
                language = preCode.ClassName.Split()[1];
                return document.QuerySelector("pre").TextContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回问题集中的所有问题，可以通过标签进行过滤。
        /// </summary>
        /// <param name="tags">标签</param>
        /// <returns>返回 JSON(JObject) 格式的问题列表</returns>
        public async Task<JObject> GetProblemSetProblemsAsync(string tags = null)
        {
            try
            {
                var getParams = new Dictionary<string, string>();
                if (tags != null)
                    getParams.Add("tags", tags);
                var jsonData = await HttpClientSingleton.DoGetAsync(PROBLEMSET_PROBLEMS_URL, getParams);
                return (JObject)JsonConvert.DeserializeObject(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
