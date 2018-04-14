using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeforcesPlatform;

namespace CodeforcesEduHacking
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //label1.Text = CodeforcesAPI.Test();
            var api = new CodeforcesAPI();
            var data = api.GetContestList()["result"];
            foreach (var item in data)
            {
                comboBox1.Items.Add(string.Format("{0,4} {1}", item["id"], item["name"]));
            }
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = comboBox1.Text;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
