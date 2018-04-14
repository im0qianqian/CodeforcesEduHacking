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

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //this.label1.Content = CodeforcesAPI.Test();
            var api = new CodeforcesAPI();
            var data = api.GetContestList()["result"];
            foreach (var item in data)
            {
                comboBox1.Items.Add(string.Format("{0,4} {1}", item["id"], item["name"]));
            }
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.label.Content = comboBox1.Text;
        }
    }
}
