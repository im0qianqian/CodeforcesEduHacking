using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CodeforcesPlatform;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CodeforcesPlatform.CompilingEnvironment;

namespace CodeforcesEduHacking
{
    /// <summary>
    /// HackExcuteWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HackExcuteWindow : Window
    {
        private IDictionary<string, object> settings = null;
        private CodeforcesAPI codeforcesApi = null;
        private JObject contestStatus = null;
        private IDictionary<string, List<int>> submissionList = new Dictionary<string, List<int>>();
        private IDictionary<string, KeyValuePair<string[], string[]>> problems = null;
        private ArrayList lang = null;
        private int contestId = 0;
        private string contestStatusFilePath = null;
        private bool isStarted = true;
        // 查询区间左端点
        private int submissionIdRangeLeft = 0;
        // 查询区间右端点
        private int submissionIdRangeRight = int.MaxValue;


        private class Submission
        {
            public string Id { set; get; }
            public string Handle { set; get; }
            public string Url { set; get; }
            public string InputData { set; get; }
            public string OutputData { set; get; }
            public string ExpectedData { set; get; }
            public string Time { set; get; }

            public Submission(string id, string handle, string url, string inputData, string outputData, string expectedData, TimeSpan time)
            {
                this.Id = id;
                this.Handle = handle;
                this.Url = url;
                this.InputData = inputData;
                this.OutputData = outputData;
                this.ExpectedData = expectedData;
                this.Time = time.Milliseconds + "ms";
            }
        }

        public HackExcuteWindow(IDictionary<string, object> settings)
        {
            InitializeComponent();

            try
            {
                // 创建 api
                codeforcesApi = new CodeforcesAPI();

                this.settings = settings;
                contestId = int.Parse(settings["contestId"].ToString());
                problems = settings["problems"] as Dictionary<string, KeyValuePair<string[], string[]>>;
                lang = settings["lang"] as ArrayList;
                contestStatusFilePath = settings["contestStatusFilePath"] as string;

                // 设置查询区间
                var submissionIdRange = (KeyValuePair<int, int>)settings["submissionIdRange"];
                submissionIdRangeLeft = submissionIdRange.Key;
                submissionIdRangeRight = submissionIdRange.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: HackExcuteWindow.HackExcuteWindow",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }

        protected override void OnClosed(EventArgs e)
        {
            isStarted = false;
            base.OnClosed(e);
        }

        private void InitStatus()
        {
            try
            {
                if (contestStatus["status"] != null && contestStatus["status"].ToString().Trim() == "OK")
                {
                    foreach (var item in contestStatus["result"])
                    {
                        var idx = item["problem"]["index"].ToString().Trim();

                        if (item["verdict"] != null && item["verdict"].ToString().Trim() == "OK" && problems.ContainsKey(idx))
                        {
                            // id 为选手提交题目的 submissionId
                            int id = int.Parse(item["id"].ToString());
                            // 筛选出查询区间内的 id
                            if (id > submissionIdRangeRight || id < submissionIdRangeLeft) continue;
                            if (!submissionList.ContainsKey(idx))
                                submissionList.Add(idx, new List<int>());
                            submissionList[idx].Add(id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: HackExcuteWindow.initStatus",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 这里更改为从本地读取文件（直接用程序下载容易出错）
                var jsonData = new StreamReader(contestStatusFilePath).ReadToEnd();
                contestStatus = (JObject)JsonConvert.DeserializeObject(jsonData);

                InitStatus();

                titleLabel.Content = "准备开始";
                await JudgeSubmission();
                titleLabel.Content = "执行完毕";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: HackExcuteWindow.Grid_Loaded",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void FoundErrorInCode(string id, string handle, string inputData, string expectedData, string outputData, TimeSpan time)
        {
            // 在界面中显示该条记录
            const string SUBMISSION_URL = "http://codeforces.com/contest/{0}/submission/{1}";
            excuteListView.Items.Add(new Submission(id, handle, string.Format(SUBMISSION_URL, contestId, id), inputData, outputData, expectedData, time));
        }

        private async Task ExecuteCode(CompilingLanguage compiler, string problemId, string code, int submissionId, string handle)
        {
            try
            {
                // 取测试输入数据与测试输出数据的最小值
                int length = Math.Min(problems[problemId].Key.Length, problems[problemId].Value.Length);
                // Trim 需要去除的字符
                char[] trimChar = new char[] { '\n', ' ', '\r' };

                for (int i = 0; i < length; i++)
                {
                    string inputData = problems[problemId].Key[i].Trim(trimChar);
                    string expectedData = problems[problemId].Value[i].Trim(trimChar);
                    string outputData = (await compiler.ExecuteAsync(code, inputData)).Trim(trimChar);

                    if (expectedData != outputData)
                    {
                        FoundErrorInCode(submissionId.ToString(), handle, inputData, expectedData, outputData, compiler.ExcuteTotalTime);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: HackExcuteWindow.ExecuteCode",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task JudgeSubmission()
        {
            try
            {
                var gnuCompiler = new GNUCompiler();
                foreach (var item in submissionList)
                {
                    string problemId = item.Key;
                    foreach (var subId in item.Value)
                    {
                        try
                        {
                            titleLabel.Content = "正在下载：" + subId.ToString();
                            var code = await codeforcesApi.GetCodeBySubmissionIdAsync(contestId, subId);
                            if (lang.IndexOf(code["language"]) == -1) continue;
                            titleLabel.Content = "正在评测：" + subId.ToString();
                            switch (code["language"])
                            {
                                case "lang-cpp":
                                    await ExecuteCode(gnuCompiler, problemId, code["code"], subId, code["handle"]);
                                    break;
                                case "lang-java":
                                    break;
                                case "lang-py":
                                    break;
                                case "lang-cs":
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                        }
                        // 如果当前标记置为 false，则退出
                        if (!isStarted) return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: HackExcuteWindow.JudgeSubmission",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hyperlink link = sender as Hyperlink;
                Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: HackExcuteWindow.Hyperlink_Click",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 复制测试数据
                var s = excuteListView.SelectedItem as Submission;
                Clipboard.SetText(s.InputData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: HackExcuteWindow.MenuItem_Click",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // 删除该条记录
                excuteListView.Items.RemoveAt(excuteListView.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: HackExcuteWindow.MenuItem_Click_1",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
