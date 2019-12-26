using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PASOIBwin
{
    public partial class Form1 : Form
    {
        string selectedDirectory;
        string rawDirectory;
        string directoryPath;
        bool isEncrypted = false;
        bool isInitialized = false;
        byte[] aesKey = new byte[] { 201, 193, 179, 215, 1, 255, 234, 83, 217, 75, 198, 92, 199,
            88, 42, 244, 20, 166, 0, 39, 224, 106, 140, 225, 104, 245, 247, 17, 150, 187, 203, 252 };

        public Form1()
        {
            InitializeComponent();
            // Если мешает, закомменть, мне удобней
            folderBrowserDialog1.SelectedPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\testDirectory\";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Штатное завершение:
            //при закрытии зашифровать и удалить резервную копию
            //Экстренное завершение:
            //удалить защищаемую папку и оставить резервную копию
            if (isInitialized)
            {
                if (isEncrypted)
                {
                    if (Directory.Exists(rawDirectory))
                        Directory.Delete(rawDirectory, true);
                }
                else
                {
                    var reason = e.CloseReason;
                    if (Directory.Exists(selectedDirectory))
                    {
                        if (reason == CloseReason.TaskManagerClosing)
                            Directory.Delete(selectedDirectory);
                        else
                        {
                            EncryptContent();
                            if (Directory.Exists(rawDirectory))
                                Directory.Delete(rawDirectory, true);
                        }
                    }
                    else
                    {
                        //sanya nagovnokodil
                    }

                }
            }
            var reason1 = e.CloseReason;
            MessageBox.Show(reason1.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {

            folderBrowserDialog1.ShowDialog();
            directoryPath = folderBrowserDialog1.SelectedPath;
            if (!string.IsNullOrEmpty(directoryPath))
            {
                selectedDirectory = directoryPath;
                rawDirectory = selectedDirectory.Remove(0, selectedDirectory.LastIndexOf(@"\") + 1);
                label1.Visible = true;
                label1.Text = "Текущий защищаемый путь: " + directoryPath;
                button2.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EncryptContentInitial();
            button1.Visible = false;
            button2.Visible = false;
            label1.Text = "Система сконфигурирвана. Данные защищены";
            button4.Visible = true;
            isInitialized = true;
            isEncrypted = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EncryptContent();
            label2.Text = "Данные защищены";
            button4.Visible = true;
            button3.Visible = false;
            button5.Visible = false;
            isEncrypted = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DecryptContent();
            isEncrypted = false;
            label1.Visible = false;
            label2.Visible = true;
            label2.Text = "Можно юзать данные";
            button3.Visible = true;
            button5.Visible = true;
            button4.Visible = false;
        }

        string GetRawPath(string path)
        {
            return path.Remove(0, path.LastIndexOf(rawDirectory));
        }
        void EncryptContentBase(string path, Action<string> mode, bool needBackUp)
        {
            if (needBackUp)
                Directory.CreateDirectory(GetRawPath(path));



            var directories = Directory.EnumerateDirectories(path);
            foreach (var dir in directories)
                EncryptContentBase(dir, mode, needBackUp);
            var files = Directory.EnumerateFiles(path);
            foreach (var file in files)
                mode(file);
        }
        void EncryptContentInitial()
        {
            EncryptContentBase(selectedDirectory, f => EncryptFileBase(f, File.ReadAllBytes(f), d => ToAes256(d)), false);
        }
        void EncryptContent()
        {
            EncryptContentInitial();
        }
        void DecryptContent()
        {
            EncryptContentBase(selectedDirectory, f => DecryptFile(f), true);
        }
        void EncryptFileBase(string fileName, byte[] data, Func<byte[], byte[]> transformation)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                byte[] transformedData = transformation(data);
                stream.Write(transformedData, 0, transformedData.Length);
                stream.Close();
            }
        }
        void EncryptFile(string fileName)
        {
            EncryptFileBase(fileName, File.ReadAllBytes(fileName), d => ToAes256(d));
        }
        void DecryptFile(string fileName)
        {
            byte[] data = File.ReadAllBytes(fileName);
            using (FileStream stream = new FileStream(GetRawPath(fileName), FileMode.Create, FileAccess.ReadWrite))
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            EncryptFileBase(fileName, data, d => FromAes256(d));
        }

        public byte[] ToAes256(byte[] src)
        {
            //Объявляем объект класса AES
            Aes aes = Aes.Create();
            //Генерируем соль
            aes.GenerateIV();
            //Присваиваем ключ. aeskey - переменная (массив байт), сгенерированная методом GenerateKey() класса AES
            //aes.GenerateKey();
            aes.Key = aesKey;
            byte[] encrypted;
            ICryptoTransform crypt = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(cs))
                        writer.Write(src);
                }
                //Записываем в переменную encrypted зашиврованный поток байтов
                encrypted = ms.ToArray();
            }
            //Возвращаем поток байт + крепим соль
            return encrypted.Concat(aes.IV).ToArray();
        }
        public byte[] FromAes256(byte[] shifr)
        {
            byte[] bytesIv = new byte[16];
            byte[] mess = new byte[shifr.Length - 16];
            //Списываем соль
            for (int i = shifr.Length - 16, j = 0; i < shifr.Length; i++, j++)
                bytesIv[j] = shifr[i];
            //Списываем оставшуюся часть сообщения
            for (int i = 0; i < shifr.Length - 16; i++)
                mess[i] = shifr[i];
            //Объект класса Aes
            Aes aes = Aes.Create();
            //Задаем тот же ключ, что и для шифрования
            aes.Key = aesKey;
            //Задаем соль
            aes.IV = bytesIv;
            byte[] result;
            ICryptoTransform crypt = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream(mess))
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Read))
                {
                    using (BinaryReader sr = new BinaryReader(cs))
                        result = sr.ReadBytes(mess.Length);
                }
            }
            return result;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            isEncrypted = false;

            button1.Visible = true;

            selectedDirectory = null;

            label1.Visible = false;
            label2.Visible = false;
            button3.Visible = false;
            button5.Visible = false;
            button4.Visible = false;
        }
    }
}



/*using System;
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
    public partial class Form1 : Form
    {
        public Form1()
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
        }


    }
}
*/