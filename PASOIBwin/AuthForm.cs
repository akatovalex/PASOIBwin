using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecurityAPI;

namespace PASOIBwin
{
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string path = "";
            //DataBase a = new DataBase(path);
            labelUsbCheck.Text = "✘";
            labelUsbCheck.ForeColor = Color.Red;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            labelUsbCheck.Text = "✓";
            labelUsbCheck.ForeColor = Color.Green;
            this.Hide();
            Form1 workForm = new Form1();
            workForm.ShowDialog();
            this.ShowDialog();
        }


    }
}