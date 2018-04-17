using CodeforcesPlatform;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CodeforcesEduHacking
{
    /// <summary>
    /// SelectedWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectedWindow : Window
    {
        private const string SPLIT_STRING = "\r\n---\r\n";
        private string contestId;
        private CodeforcesAPI codeforcesApi = null;
        private JObject problemList = null;
        private class CFProblem : INotifyPropertyChanged
        {
            public string Count { get; set; }
            public string Id { get; }
            public string Title { get; }
            public string InputData
            {
                get
                {
                    if (inputData == null) return "";
                    return string.Join(SPLIT_STRING, inputData);
                }
                set
                {
                    if (value == null) return;
                    inputData = Regex.Split(value, SPLIT_STRING);
                    NotifyChange("InputData");
                }
            }
            private string[] inputData;
            public string OutputData
            {
                get
                {
                    if (outputData == null) return "";
                    return string.Join(SPLIT_STRING, outputData);
                }
                set
                {
                    if (value == null) return;
                    outputData = Regex.Split(value, SPLIT_STRING);
                    NotifyChange("OutputData");
                }
            }
            private string[] outputData;

            public string[] GetInputData()
            {
                return inputData;
            }

            public string[] GetOutputData()
            {
                return outputData;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyChange(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            public bool Enable { get; set; }
            public string ContestId { get; }
            public object ProblemObject { get; }
            public CFProblem() { }
            public CFProblem(string id, string contestId, string title, string[] inputData = null, string[] outputData = null, bool enable = false)
            {
                this.Id = id;
                this.ContestId = contestId;
                this.Title = title;
                this.inputData = inputData;
                this.outputData = outputData;
                this.Enable = enable;
                this.ProblemObject = this;
            }
        }

        public SelectedWindow(string contestId = "1")
        {
            InitializeComponent();

            this.contestId = contestId;
        }


        private async Task LoadProblemList()
        {
            problemList = await codeforcesApi.GetProblemSetProblemsAsync();
            if (problemList["status"].ToString() == "OK")
            {
                var list = problemList["result"]["problems"];
                foreach (var item in list.Reverse())
                {
                    if (item["contestId"].ToString() == contestId)
                    {
                        problemListView.Items.Add(new CFProblem(item["index"].ToString(), contestId, item["name"].ToString()));
                    }
                }
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            codeforcesApi = new CodeforcesAPI();

            await LoadProblemList();

            titleLabel.Content = "题 目 列 表";

            //string[] sk = { "111", "222" };
            //for (int i = 0; i < 5; i++)
            //{
            //    var newData = new CFProblem(((char)('A' + i)).ToString(), "111", i.ToString(), sk, sk);
            //    problemListView.Items.Add(newData);
            //}
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var problemItem = (CFProblem)checkBox.Tag;
            problemItem.Enable = checkBox.IsChecked.GetValueOrDefault();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Hack 配置
                var settings = new Dictionary<string, object>();

                // 线程数目
                settings.Add("threadSize", ((int)threadSizeSlider.Value).ToString());

                // 支持语言
                var langArray = new ArrayList();
                if (cppCheckBox.IsChecked.GetValueOrDefault()) langArray.Add("lang-cpp");
                if (javaCheckBox.IsChecked.GetValueOrDefault()) langArray.Add("lang-java");
                if (python2CheckBox.IsChecked.GetValueOrDefault() || python3CheckBox.IsChecked.GetValueOrDefault()) langArray.Add("lang-py");
                if (csharpCheckBox.IsChecked.GetValueOrDefault()) langArray.Add("lang-cs");
                settings.Add("lang", langArray);

                // 选择的题目及竞赛 ID
                var problems = new Dictionary<string, KeyValuePair<string[], string[]>>();
                string contestId = "";
                foreach (var item in problemListView.Items)
                {
                    var s = item as CFProblem;
                    if (s.Enable)
                        problems.Add(s.Id, new KeyValuePair<string[], string[]>(s.GetInputData(), s.GetOutputData()));
                    contestId = s.ContestId;
                }
                settings.Add("problems", problems);
                settings.Add("contestId", contestId);

                var hacking = new HackExcuteWindow(settings);
                hacking.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void threadSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.threadSizeLabel.Content = "线程数目：" + ((int)(e.NewValue)).ToString();
        }

        private void problemListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var listView = sender as ListView;
                var item = listView.SelectedItem as CFProblem;
                var inputForm = new TestInputWindow(item.InputData, item.OutputData, item.Title);
                inputForm.ShowDialog();
                item.InputData = inputForm.InputData;
                item.OutputData = inputForm.OutputData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
