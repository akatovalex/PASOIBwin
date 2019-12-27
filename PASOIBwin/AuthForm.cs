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
                    if (pas == ComputeMD5Hash(textBoxPassword.Text))
                    {
                        authTrue = true;
                    }
                }
            }
            if (authTrue)
            {
                this.Hide();
                CypherForm cypherForm = new CypherForm();
                cypherForm.ShowDialog();
                this.Show();
            }
            else {
                //ЗАНОСИМ В ЖУРНАЛ
                //datab.InsertData("journal", "[code]='-1',[login]='" + textBoxLogin.Text + "',[description]='Wrong username or password',[time]='0'");
                //datab.InsertData("journal", "'0','-1','" + textBoxLogin.Text + "','Wrong username or password','0'");

                MessageBox.Show("Неверный логин или пароль", "Ошибка!");
            }

        }

        private void AuthForm_FirstShown(object sender, EventArgs e)
        {
            // УДАЛИТЬ ПОТОМ
            textBoxLogin.Text = "admin";
            textBoxPassword.Text = "admin";
        }

        private void AuthForm_Shown(object sender, EventArgs a)
        {
            labelUsbCheck.Text = "✘";
            labelUsbCheck.ForeColor = Color.Red;
        }

        static string ComputeSha1Hash(string rawData)
        {
            // Create a SHA1   
            using (SHA1 sha1Hash = SHA1.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        static string ComputeMD5Hash(string rawData)
        {
            // Create a MD5   
            using (MD5 md5Hash = MD5.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


    }
}