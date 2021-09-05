using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class AddMonitoringForm : Form
    {
        public String name = "default";
        public String ip = "127.0.0.1";
        public int port = 80;
        public String bannertext = "";
        public bool ignorebanner = false;

        public AddMonitoringForm()
        {
            InitializeComponent();
        }

        private void AddMonitoringForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //bool cont=true;
            foreach(String key in Globals.Settings.Monitors.Keys)
                if (((MonitoringData)Globals.Settings.Monitors[key]).name == textBox1.Text)
                {
                    MessageBox.Show("Name already existing.");
                    return;
                }


            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                name = textBox1.Text;
                ip = textBox2.Text;
                port = int.Parse(textBox3.Text);
                bannertext = textBox4.Text;
                ignorebanner = checkBox1.Checked;
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Plese fill in all fields.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
