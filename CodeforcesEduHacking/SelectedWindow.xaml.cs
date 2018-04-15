using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CodeforcesEduHacking
{
    /// <summary>
    /// SelectedWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectedWindow : Window
    {
        private class CFProblem
        {
            public string Id { get; }
            public string Title { get; }
            public string InputData
            {
                get
                {
                    return string.Join("\n", inputData);
                }
            }
            private string[] inputData;
            public string OutputData
            {
                get
                {
                    return string.Join("\n", outputData);
                }
            }
            private string[] outputData;
            public bool Enable { get; set; }

            public object ProblemObject { get; }
            public CFProblem() { }
            public CFProblem(string id, string title, string[] inputData, string[] outputData, bool enable = false)
            {
                this.Id = id;
                this.Title = title;
                this.inputData = inputData;
                this.outputData = outputData;
                this.Enable = enable;
                this.ProblemObject = this;
            }
        }

        public SelectedWindow()
        {
            InitializeComponent();
        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string[] sk = { "111", "222" };
            for (int i = 0; i < 5; i++)
            {
                problemList.Items.Add(new CFProblem(((char)('A' + i)).ToString(), i.ToString(), sk, sk));
            }
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var problemItem = (CFProblem)checkBox.Tag;
            problemItem.Enable = checkBox.IsChecked.GetValueOrDefault();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in problemList.Items)
            {
                var s = (CFProblem)item;
                if (s.Enable)
                {
                    MessageBox.Show(s.Id);
                }
            }
        }

        private void threadSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.threadSizeLabel.Content = "线程数目：" + ((int)(e.NewValue)).ToString();
        }

        private void problemList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var txtBox = sender;
            //var model = txtBox.DataContext as CFProblem;//ListBoxModel是自定义的数据对象
            //CFProblem a = (CFProblem)sender;

            MessageBox.Show(sender.ToString());
        }
    }
}
