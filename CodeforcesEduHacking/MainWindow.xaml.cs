using System;
using System.Collections.Generic;
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
                    if (item["name"].ToString().StartsWith("Educational Codeforces Round"))
                        contestListComboBox.Items.Add(string.Format("{0,4} {1}", item["id"], item["name"]));
                }
                contestListComboBox.SelectedIndex = list.Count() > 0 ? 0 : -1;
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //codeforcesApi = new CodeforcesAPI();
                //await LoadContestList();

                ///
                for (int i = 0; i < 10; i++)
                {
                    contestListComboBox.Items.Add(i.ToString());
                }
                ///

                titleLabel.Content = "请选择你所要查询的 Edu Round";
                submitButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(contestListComboBox.Text);
            SelectedWindow a = new SelectedWindow();
            a.Show();
        }
    }
}
