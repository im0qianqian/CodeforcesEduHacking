using System;
using System.Collections.Generic;
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
    /// TestInputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestInputWindow : Window
    {
        public string InputData { get; set; }
        public string OutputData { get; set; }
        public TestInputWindow(string inputData = "", string outputData = "", string title = "")
        {
            InitializeComponent();

            this.titleLabel.Content = string.Format("Please input the test data for \"{0}\"!", title);
            this.InputData = this.inputTextBox.Text = inputData;
            this.OutputData = this.outputTextBox.Text = outputData;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InputData = inputTextBox.Text;
                OutputData = outputTextBox.Text;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " error: TestInputWindow.submitButton_Click");
            }
        }
    }
}
