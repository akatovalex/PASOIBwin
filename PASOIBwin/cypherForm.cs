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

namespace PASOIBwin {
    public partial class CypherForm : Form {
        string SelectedDirectory { get; set; }
        string RawDirectory { get { return SelectedDirectory?.Remove(0, SelectedDirectory.LastIndexOf(@"\") + 1); } }       //Мб не работает, мне выдало пустую строку
        string directoryPath;
        bool isEncrypted = false;
        bool isInitialized = false;
        byte[] aesKey = new byte[] { 201, 193, 179, 215, 1, 255, 234, 83, 217, 75, 198, 92, 199,
            88, 42, 244, 20, 166, 0, 39, 224, 106, 140, 225, 104, 245, 247, 17, 150, 187, 203, 252 };

        string keyHash;

        public string sqlPathDirectories;
        SecurityAPI.DataBase dbDirectories;
        DataTable dtDirectories;


        static string ValidateLastSlash(string path) {
            if (path.LastIndexOf(@"\") != path.Length - 1)
                return path + @"\";
            return path;
        }

        public CypherForm(bool isAdmin, byte[] encryptionKey) {
            InitializeComponent();
            //folderBrowserDialog1.SelectedPath = Directory.GetCurrentDirectory() + @"\testDirectory\";    // Если мешает, закомменть, мне удобней
            //listBox_ProtectedDirectories.Items.Add(Directory.GetCurrentDirectory() + @"\testDirectory\");


            sqlPathDirectories = "‪protectedfiles.sqlite";
            dbDirectories = new SecurityAPI.DataBase(sqlPathDirectories);

            keyHash = "df6670833b208a10561f74be3f79a279";    //Данный хэш надо считывать с флешки, рядом с ключом для дешифрования каталогов

            RefreshListBox();

            aesKey = encryptionKey;
            //if (!isAdmin)
            //GoToUserUI();
        }

        void GoToAdminUI() {

        }
        void GoToUserUI() {
            //from button_ChangeFolder_Click
            directoryPath = listBox_ProtectedDirectories.Items[0].ToString();
            if (!string.IsNullOrEmpty(directoryPath)) {
                //Чтобы случайно не зашифровать себе локальный диск
                if (directoryPath.Length > 4) {
                    SelectedDirectory = directoryPath;
                    label_FirstInit.Visible = true;
                    label_FirstInit.Text = "Текущий защищаемый путь: " + SelectedDirectory;
                    button_ProtectData.Visible = true;
                }
                else {
                    directoryPath = null;
                    SelectedDirectory = null;
                    button_ProtectData.Visible = false;

                    label_FirstInit.Visible = true;
                    label_FirstInit.Text = "Каталог не выбран";
                }
            }
            //from button_ProtectData_Click
            button_ChangeFolder.Visible = false;
            button_ProtectData.Visible = false;
            button_UnlockChosenDirectory.Visible = false;
            listBox_ProtectedDirectories.Visible = false;
            //from button_UnlockData_Click
            DecryptContent();
            isEncrypted = false;
            label_FirstInit.Visible = false;
            label_DataProtected.Visible = true;
            this.BackgroundImage = Properties.Resources.tomNewspaper;
            label_DataProtected.Text = "Можно юзать данные";
            button_ExitSession.Visible = true;
            button_DecryptData.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e) {
            label_FirstInit.Visible = true;
            label_FirstInit.Text = "Каталог не выбран";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            //Штатное завершение:
            //при закрытии зашифровать и удалить резервную копию
            //Экстренное завершение:
            //удалить защищаемую папку и оставить резервную копию
            if (isInitialized) {
                if (isEncrypted) {
                    if (Directory.Exists(RawDirectory))
                        Directory.Delete(RawDirectory, true);
                }
                else {
                    var reason = e.CloseReason;
                    if (Directory.Exists(SelectedDirectory)) {
                        if (reason == CloseReason.TaskManagerClosing)
                            Directory.Delete(SelectedDirectory);
                        else {
                            //EncryptContent();             //Пока что только мешает, т.к. дешифровать обратно невозможно
                            if (Directory.Exists(RawDirectory))
                                Directory.Delete(RawDirectory, true);
                        }
                    }
                    else {
                        //sanya nagovnokodil
                    }

                }
            }
            var reason1 = e.CloseReason;
            MessageBox.Show(reason1.ToString());
        }

        private void button_ChangeFolder_Click(object sender, EventArgs e) {

            folderBrowserDialog1.ShowDialog();
            directoryPath = folderBrowserDialog1.SelectedPath;
            if (!string.IsNullOrEmpty(directoryPath)) {
                //Чтобы случайно не зашифровать себе локальный диск
                if (directoryPath.Length > 4) {
                    SelectedDirectory = directoryPath;
                    label_FirstInit.Visible = true;
                    label_FirstInit.Text = "Текущий защищаемый путь: " + SelectedDirectory;
                    button_ProtectData.Visible = true;
                }
                else {
                    directoryPath = null;
                    SelectedDirectory = null;
                    button_ProtectData.Visible = false;

                    label_FirstInit.Visible = true;
                    label_FirstInit.Text = "Каталог не выбран";
                }
            }
        }

        private void button_ProtectData_Click(object sender, EventArgs e) {
            EncryptContentInitial();
            //button_ChangeFolder.Visible = false;
            //button_ProtectData.Visible = false;
            //button_UnlockChosenDirectory.Visible = false;
            //listBox_ProtectedDirectories.Visible = false;

            //this.BackgroundImage = Properties.Resources.jerry;
            //label_FirstInit.Text = "Система сконфигурирвана. Данные защищены";
            //button_UnlockData.Visible = true;
            //isInitialized = true;
            //isEncrypted = true;

            // СДЕЛАЙ ТАК, ЧТОБЫ ПРИ НАЖАТИИ ЭТОЙ КНОПКИ ListBox пополнялся новым элементом, а новый каталог шифровался (И ВСЁ, ОСТАЁМСЯ НА ЭТОМ ЭКРАНЕ)    
            // (можно сделать тупо по нажатию плюса, но не стоит, так всё себе перешифруешь случайно)
            // Код сверху надо переделать или удалить, но временно оставим

            button_ProtectData.Visible = false;
            dbDirectories.ExecuteCommand("INSERT INTO directories (path, encrypted, keyhash) VALUES ('" + SelectedDirectory + "', '" + 1 + "','" + keyHash + "')");

            RefreshListBox();

            SelectedDirectory = null;
            directoryPath = null;



        }

        private void button_ExitSession_Click(object sender, EventArgs e) {
            EncryptContent();
            label_DataProtected.Text = "Данные защищены";
            this.BackgroundImage = Properties.Resources.tom;
            //button_UnlockData.Visible = true;
            button_ExitSession.Visible = false;
            button_DecryptData.Visible = false;
            //isEncrypted = true;

            isEncrypted = false; //DEBUG (больше не нужна)

            button_ChangeFolder.Visible = true;
            button_UnlockChosenDirectory.Visible = true;
            listBox_ProtectedDirectories.Visible = true;
        }

        string GetRawPath(string path) {
            return path.Remove(0, path.LastIndexOf(RawDirectory));
        }
        void EncryptContentBase(string path, Action<string> mode, bool needBackUp) {
            if (needBackUp)
                Directory.CreateDirectory(GetRawPath(path));
            var directories = Directory.EnumerateDirectories(path);
            foreach (var dir in directories)
                EncryptContentBase(dir, mode, needBackUp);
            var files = Directory.EnumerateFiles(path);
            foreach (var file in files)
                mode(file);
        }
        void EncryptContentInitial() {
            EncryptContentBase(SelectedDirectory, f => EncryptFileBase(f, File.ReadAllBytes(f), d => Encrypting.ToAes256(d, aesKey)), false);
        }
        void EncryptContent() {
            EncryptContentInitial();
        }
        void DecryptContent() {
            EncryptContentBase(SelectedDirectory, f => DecryptFile(f), true);
        }
        void EncryptFileBase(string fileName, byte[] data, Func<byte[], byte[]> transformation) {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite)) {
                byte[] transformedData = transformation(data);
                stream.Write(transformedData, 0, transformedData.Length);
                stream.Close();
            }
        }
        void EncryptFile(string fileName) {
            EncryptFileBase(fileName, File.ReadAllBytes(fileName), d => Encrypting.ToAes256(d, aesKey));
        }
        void DecryptFile(string fileName) {
            byte[] data = File.ReadAllBytes(fileName);
            using (FileStream stream = new FileStream(GetRawPath(fileName), FileMode.Create, FileAccess.ReadWrite)) {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            EncryptFileBase(fileName, data, d => Encrypting.FromAes256(d, aesKey));
        }

        private void Button_DecryptData_Click(object sender, EventArgs e) {
            isEncrypted = false;

            button_ChangeFolder.Visible = true;
            button_UnlockChosenDirectory.Visible = true;
            listBox_ProtectedDirectories.Visible = true;

            //SelectedDirectory = null;
            //directoryPath = null;
            this.BackgroundImage = Properties.Resources.tom;

            label_FirstInit.Visible = true;
            label_FirstInit.Text = "Каталог не выбран";


            label_DataProtected.Visible = false;
            button_ExitSession.Visible = false;
            button_DecryptData.Visible = false;

            dbDirectories.ExecuteCommand("DELETE FROM directories WHERE path='" + (SelectedDirectory.Remove(SelectedDirectory.Length - 1)) + "'");

            RefreshListBox();

            SelectedDirectory = null;
            directoryPath = null;
        }

        private void Button_UnlockChosenDirectory_Click(object sender, EventArgs e) {
            // Пересылает на экран с кнопкой "Выйти из сессии"
            // Содержит кнопку "Разблокировать навсегда" (такой же экран, как сейчас уже есть в проге - Том с газетой)
            // Кнопка "разблокировать навсегда" удаляет каталог из listBox, возвращает на главный экран и не шифрует файлы; "Выйти из сессии" шифрует и возвращает на главный экран
            if (listBox_ProtectedDirectories.SelectedIndex < 0) {
                //Кнопка недоступна должна быть
                MessageBox.Show("Choose something already");
            }
            else {
                //Временно
                SelectedDirectory = listBox_ProtectedDirectories.SelectedItem.ToString();
                //MessageBox.Show("Выбран каталог для шифрования (в будущем для дешифрования - дальше врубается менюшка Тома с газетой):\n" + selectedDir);

                //label_FirstInit.Text = "Выбран каталог: " + selectedDirectory;
                //label_FirstInit.Visible = true;

                //rawDirectory = selectedDirectory.Remove(0, selectedDirectory.LastIndexOf(@"\") + 1);        // ну и костыли с удалением "\"


                //button_ProtectData.Visible = true;

                DecryptContent();
                isInitialized = true;
                isEncrypted = false;
                label_FirstInit.Visible = false;
                label_DataProtected.Visible = true;
                this.BackgroundImage = Properties.Resources.tomNewspaper;
                label_DataProtected.Text = "Можно юзать данные";
                button_ExitSession.Visible = true;
                button_DecryptData.Visible = true;


                listBox_ProtectedDirectories.Visible = false;
                button_ChangeFolder.Visible = false;
                button_UnlockChosenDirectory.Visible = false;
            }
        }

        private void RefreshListBox() {
            listBox_ProtectedDirectories.Items.Clear();
            dtDirectories = dbDirectories.ReadData("path", "directories", "[encrypted]='" + 1 + "' AND [keyhash]='" + keyHash + "'");

            foreach (DataRow row in dtDirectories.Rows) {
                foreach (DataColumn column in dtDirectories.Columns)
                    listBox_ProtectedDirectories.Items.Add(ValidateLastSlash(row[column].ToString()));
            }
        }
    }
}
