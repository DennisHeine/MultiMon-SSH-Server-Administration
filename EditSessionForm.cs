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
    public partial class EditSessionForm : Form
    {
        public EditSessionForm()
        {
            InitializeComponent();
        }
        public String name = "";
        public String ip = "";
        public int port = 80;
        public String username = "";
        public String password="";

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                name = textBox1.Text;
                ip = textBox2.Text;
                port = int.Parse(textBox3.Text);
                username = textBox4.Text;
                password = textBox5.Text;
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
