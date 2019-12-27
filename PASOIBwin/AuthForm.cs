﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SecurityAPI;
using System.Security.Cryptography;


namespace PASOIBwin
{
    public partial class AuthForm : Form
    {

        public string sqlPath;
        SecurityAPI.DataBase datab;
        DataTable dataT;
        public AuthForm()
        {
            InitializeComponent();
            this.sqlPath = "‪security.sqlite";
            datab = new SecurityAPI.DataBase(sqlPath);
            

        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            //string path = "";
            //DataBase a = new DataBase(path);
            labelUsbCheck.Text = "✘";
            labelUsbCheck.ForeColor = Color.Red;
        }

        

        private void Button1_Click(object sender, EventArgs e)
        {
            bool authTrue = false;
            labelUsbCheck.Text = "✓";
            labelUsbCheck.ForeColor = Color.Green;
            MessageBox.Show("Галочка!", "Зелёная!");


            dataT = datab.ReadData("password", "user", "[login]='"+textBoxLogin.Text+"'");

            foreach (DataRow row in dataT.Rows)
            {
                foreach (DataColumn column in dataT.Columns)
                {
                    string pas = row[column].ToString();
                    if (pas == textBoxPassword.Text)
                    {
                        authTrue = true;
                    }
                }
            }
            if (authTrue)
            {
                this.Hide();
                Form1 workForm = new Form1();
                workForm.ShowDialog();
                this.Show();
            }
            else {
                //ЗАНОСИМ В ЖУРНАЛ
                MessageBox.Show("Неверный логин или пароль", "Ошибка!");
            }

        }

        private void AuthForm_FirstShown(object sender, EventArgs e)
        {
            // УДАЛИТЬ ПОТОМ
            textBoxLogin.Text = "admin";
            textBoxPassword.Text = "21232f297a57a5a743894a0e4a801fc3";
        }

        private void AuthForm_Shown(object sender, EventArgs a)
        {
            labelUsbCheck.Text = "✘";
            labelUsbCheck.ForeColor = Color.Red;
        }




    }
}