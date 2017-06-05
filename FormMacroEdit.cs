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
    public partial class FormMacroEdit : Form
    {

        public String macro="";
        public String macrotext="";
        public bool macrodelete=false;
        public FormMacroEdit()
        {
            InitializeComponent();
        }

        private void FormMacroEdit_Load(object sender, EventArgs e)
        {
            textBox1.Text = macro;
            textBox2.Text = macrotext;
        }

        private void button1_Click(object sender, EventArgs e)
        {
             if (!(textBox1.Text == "" || textBox2.Text == ""))
            {
                macro = textBox1.Text;
                macrotext = textBox2.Text;
                macrodelete=checkBox1.Checked;
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
