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
using System.IO;


namespace PASOIBwin
{
    public partial class AuthForm : Form
    {

        public string sqlPath;
        SecurityAPI.DataBase datab;
        DataTable dataT;
        Timer USBTimer;

        static bool UsbCheck() {
            var info = DriveInfo.GetDrives();
            foreach (var device in info) {
                if (device.DriveType == DriveType.Removable && device.IsReady)
                    return true;
            }
            return false;
        }

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

            USBTimer = new Timer();
            USBTimer.Interval = (1 * 1000); // 1 sec
            USBTimer.Tick += new EventHandler(USBTimer_Tick);
        }

        private void USBTimer_Tick(object sender, EventArgs e)
        {
            if (UsbCheck()) {
                labelUsbCheck.Text = "✓";
                labelUsbCheck.ForeColor = Color.Green;
            }
            else
            {
                labelUsbCheck.Text = "✘";
                labelUsbCheck.ForeColor = Color.Red;
            }

            //MessageBox.Show("The form will now be closed.", "Time Elapsed");
            //this.Close();

        }


        private void Button1_Click(object sender, EventArgs e)
        {
            bool authTrue = false;

            //MessageBox.Show("Галочка!", "Зелёная!");


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
                if (UsbCheck()) {
                    datab.ExecuteCommand("INSERT INTO journal (code,login,description,time) VALUES ('1', '" + textBoxLogin.Text + "','Succesful authentication','" + (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff") + "')");
                    this.Hide();
                    USBTimer.Stop();    //Если поставить это до this.Hide, то таймер вырубается и сразу врубается обратно из-за AuthForm_Shown
                    CypherForm cypherForm = new CypherForm();
                    cypherForm.ShowDialog();
                    datab.ExecuteCommand("INSERT INTO journal (code,login,description,time) VALUES ('0', '" + textBoxLogin.Text + "','Logoff','" + (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff") + "')");
                    this.Show();
                }
                else { 
                    MessageBox.Show("Вставь флешку!", "Ошибка!"); //TODO журнал
                    datab.ExecuteCommand("INSERT INTO journal (code,login,description,time) VALUES ('-2', '" + textBoxLogin.Text + "','No USB token found','" + (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff") + "')");
                }
            }
            else {
                //спасибо, что ты есть
                datab.ExecuteCommand("INSERT INTO journal (code,login,description,time) VALUES ('-1', '"+textBoxLogin.Text+ "','Wrong username or password','" + (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff") + "')");

                //назвал параметры - страдай и гугли негуглИмое
                //datab.InsertData("journal [(code,login,description,time)]", "'-1','" + textBoxLogin.Text + "','Wrong username or password','0'");   

                //обязательно прописать ВСЕ параметры, автоинкремент не работает
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
            USBTimer.Start();
            //labelUsbCheck.Text = "✘";
            //labelUsbCheck.ForeColor = Color.Red;
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