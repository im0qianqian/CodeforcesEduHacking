using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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


        private class Submission
        {
            public string Id { set; get; }
            public string Handle { set; get; }
            public string Url { set; get; }
            public string InputData { set; get; }
            public string OutputData { set; get; }
            public string ExpectedData { set; get; }

            public Submission(string id, string handle, string url, string inputData, string outputData, string expectedData)
            {
                this.Id = id;
                this.Handle = handle;
                this.Url = url;
                this.InputData = inputData;
                this.OutputData = outputData;
                this.ExpectedData = expectedData;
            }
        }

        public HackExcuteWindow(IDictionary<string, object> settings)
        {
            InitializeComponent();

            try
            {
                this.settings = settings;
                contestId = int.Parse(settings["contestId"].ToString());
                problems = settings["problems"] as Dictionary<string, KeyValuePair<string[], string[]>>;
                lang = settings["lang"] as ArrayList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: CodeforcesEduHacking.HackExcuteWindow");
            }

        }

        private void InitStatus()
        {
            try
            {
                if (contestStatus["status"].ToString().Trim() == "OK")
                {
                    foreach (var item in contestStatus["result"])
                    {

                        var idx = item["problem"]["index"].ToString().Trim();
                        if (item["verdict"].ToString().Trim() == "OK" && problems.ContainsKey(idx))
                        {
                            if (!submissionList.ContainsKey(idx))
                                submissionList.Add(idx, new List<int>());
                            submissionList[idx].Add(int.Parse(item["id"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: CodeforcesEduHacking.initStatus");
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //FoundErrorInCode("37740511", "im0qianqian", "4", "5", "6");
                //FoundErrorInCode("37740512", "im0qianqian", "4", "5", "6");
                //FoundErrorInCode("37740513", "im0qianqian", "4", "5", "6");
                //FoundErrorInCode("37740514", "im0qianqian", "4", "5", "6");
                //submissionList.Add("A", new List<int> { 37740511 });

                codeforcesApi = new CodeforcesAPI();
                contestStatus = await codeforcesApi.GetContestStatusAsync(contestId);

                InitStatus();

                titleLabel.Content = "准备开始";
                JudgeSubmission();
                titleLabel.Content = "执行完毕";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: CodeforcesEduHacking.Grid_Loaded");
            }
        }

        private void FoundErrorInCode(string id, string handle, string inputData, string expectedData, string outputData)
        {
            const string SUBMISSION_URL = "http://codeforces.com/contest/{0}/submission/{1}";
            excuteListView.Items.Add(new Submission(id, handle, string.Format(SUBMISSION_URL, contestId, id), inputData, outputData, expectedData));
        }

        private async Task ExecuteCode(CompilingLanguage compiler, string problemId, string code, int submissionId, string handle)
        {
            try
            {
                int length = Math.Min(problems[problemId].Key.Length, problems[problemId].Value.Length);
                for (int i = 0; i < length; i++)
                {
                    string inputData = problems[problemId].Key[i].Trim();
                    string expectedData = problems[problemId].Value[i].Trim();
                    string outputData = (await compiler.ExecuteAsync(code, inputData)).Trim();

                    if (expectedData.TrimEnd('\n') != outputData.TrimEnd('\n'))
                    {
                        FoundErrorInCode(submissionId.ToString(), handle, inputData, expectedData, outputData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: CodeforcesEduHacking.ExecuteCode");
            }
        }

        private async void JudgeSubmission()
        {
            try
            {
                var gnuCompiler = new GNUCompiler();
                foreach (var item in submissionList)
                {
                    string problemId = item.Key;
                    foreach (var subId in item.Value)
                    {
                        titleLabel.Content = "正在评测：" + subId.ToString();
                        var code = await codeforcesApi.GetCodeBySubmissionIdAsync(contestId, subId);
                        if (lang.IndexOf(code["language"]) == -1) continue;
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: CodeforcesEduHacking.JudgeSubmission");
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
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
                MessageBox.Show(ex.Message + " error: CodeforcesEduHacking.MenuItem_Click");
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                excuteListView.Items.RemoveAt(excuteListView.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: CodeforcesEduHacking.MenuItem_Click_1");
            }
        }
    }
}
