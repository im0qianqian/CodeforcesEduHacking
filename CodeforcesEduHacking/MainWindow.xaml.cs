using System;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CodeforcesPlatform;
using Newtonsoft.Json.Linq;

namespace CodeforcesEduHacking
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private CodeforcesAPI codeforcesApi = null;
        private JObject contestList = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task LoadContestList()
        {
            contestList = await codeforcesApi.GetContestListAsync();
            if (contestList["status"].ToString() == "OK")
            {
                var list = contestList["result"];
                foreach (var item in list)
                {
                    string now = item["name"].ToString().Trim();
                    if (now.StartsWith("Educational Codeforces Round") || now.EndsWith("(Div. 3)"))
                        contestListComboBox.Items.Add(string.Format("{0,4} {1}", item["id"], item["name"]));
                }
                contestListComboBox.SelectedIndex = list.Count() > 0 ? 0 : -1;
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
#if DEBUG
                currentVersionLabel.Content = "当前版本号：1.0.0.0";
#else
                currentVersionLabel.Content = "当前版本号：" + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
#endif

                codeforcesApi = new CodeforcesAPI();
                await LoadContestList();

                ///
                //for (int i = 444; i < 470; i++)
                //{
                //    contestListComboBox.Items.Add(" " + i.ToString() + " hahahahah");
                //}
                ///

                titleLabel.Content = "请选择一个 Edu Round or Div. 3";
                hackCountButton.IsEnabled = true;
                hackItButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: MainWindow.Grid_Loaded");
            }
        }

        private int GetContestId()
        {
            try
            {
                string contestId = contestListComboBox.Text.Substring(0, 4).Trim();
                return int.Parse(contestId);
            }
            catch (Exception)
            {
                MessageBox.Show("获取 ContestId 失败！ error: MainWindow.GetContestId");
            }
            return 0;
        }

        private async void hackCountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("点击确定榜单开始加载，稍后呈现结果，请勿多次点击……",
                                "Remind",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                var standings = await codeforcesApi.GetContestStandingsAsync(GetContestId(), true);
                if (standings["status"].ToString() == "OK")
                {
                    string message = "";
                    var rows = standings["result"]["rows"];
                    var res = new List<KeyValuePair<KeyValuePair<string, double>, KeyValuePair<int, int>>>();
                    foreach (var item in rows)
                    {
                        int successfulHackCount = int.Parse(item["successfulHackCount"].ToString());
                        int unsuccessfulHackCount = int.Parse(item["unsuccessfulHackCount"].ToString());
                        string handle = item["party"]["members"][0]["handle"].ToString();
                        res.Add(new KeyValuePair<KeyValuePair<string, double>, KeyValuePair<int, int>>(new KeyValuePair<string, double>(handle, successfulHackCount - unsuccessfulHackCount / 2.0), new KeyValuePair<int, int>(successfulHackCount, unsuccessfulHackCount)));
                    }
                    res.Sort((x, y) => y.Key.Value.CompareTo(x.Key.Value));
                    for (int i = 0; i < Math.Min(10, res.Count); i++)
                        message += string.Format("rk{0,-3}. {1,-30}\t({2}):\t{3,5}\t{4,5}\n", i + 1, res[i].Key.Key, res[i].Key.Value, res[i].Value.Key, res[i].Value.Value);
                    MessageBox.Show(message, contestListComboBox.Text);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hack Standings 获取失败！");
            }
        }

        private void hackItButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SelectedWindow(GetContestId().ToString()).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: MainWindow.hackItButton_Click");
            }
        }
    }
}
