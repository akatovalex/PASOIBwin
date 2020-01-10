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
        EncryptingDirectory SelectedDirectory { get; set; }
        string RawDirectory { get { return SelectedDirectory?.Path.Remove(0, SelectedDirectory.Path.LastIndexOf(@"\") + 1); } }
        bool isInitialized = false;
        byte[] aesKey = new byte[] { 201, 193, 179, 215, 1, 255, 234, 83, 217, 75, 198, 92, 199,
            88, 42, 244, 20, 166, 0, 39, 224, 106, 140, 225, 104, 245, 247, 17, 150, 187, 203, 252 };

        string keyHash;

        public string sqlPathDirectories;
        SecurityAPI.DataBase dbDirectories;
        DataTable dtDirectories;

        SecurityAPI.DataBase dbJournal;
        readonly string login;
        readonly bool isAdmin;

        static string ValidateLastSlash(string path) {
            if (path.LastIndexOf(@"\") != path.Length - 1)
                return path + @"\";
            return path;
        }
        bool IsEncrypted(DataTable table) {
            if ((bool)table.Rows[0]["encrypted"]) {
                MessageBox.Show("Данный путь уже зашифрован!");
                return true;
            }
            return false;
        }
        bool IsSelectedPathInDB(string path) {
            DataTable dt = dbDirectories.ReadData("encrypted", "directories", "[path]='" + path + "'");
            if (dt.Rows.Count == 0) {
                dt = dbDirectories.ReadData("encrypted", "directories", "[path]='" + path + "@/" + "'");
                if (dt.Rows.Count == 0)
                    return false;
                return IsEncrypted(dt);
            }
            return IsEncrypted(dt);
         }
        void RefreshListBox() {
            listBox_ProtectedDirectories.Items.Clear();
            dtDirectories = dbDirectories.ReadData("path", "directories", "[encrypted]='" + 1 + "' AND [keyhash]='" + keyHash + "'");

            foreach (DataRow row in dtDirectories.Rows) {
                foreach (DataColumn column in dtDirectories.Columns)
                    listBox_ProtectedDirectories.Items.Add(ValidateLastSlash(row[column].ToString()));
            }
        }
        void DeleteSelectedDirectoryFromDB() {
            dbDirectories.ExecuteCommand("DELETE FROM directories WHERE path='" + SelectedDirectory + "'");
            dbDirectories.ExecuteCommand("DELETE FROM directories WHERE path='" + SelectedDirectory + @"\" + "'");
        }

        public CypherForm(bool isAdmin, byte[] usbHash, byte[] encryptionKey, SecurityAPI.DataBase dbJ, string login) {
            InitializeComponent();
            //folderBrowserDialog1.SelectedPath = Directory.GetCurrentDirectory() + @"\testDirectory\";    // Если мешает, закомменть, мне удобней
            //listBox_ProtectedDirectories.Items.Add(Directory.GetCurrentDirectory() + @"\testDirectory\");


            sqlPathDirectories = "‪protectedfiles.sqlite";
            dbDirectories = new SecurityAPI.DataBase(sqlPathDirectories);

            //keyHash = "df6670833b208a10561f74be3f79a279";    //Данный хэш надо считывать с флешки, рядом с ключом для дешифрования каталогов

            keyHash = Encoding.UTF8.GetString(usbHash);
            RefreshListBox();

            aesKey = encryptionKey;
            //if (!isAdmin)
            //GoToUserUI();

            dbJournal = dbJ;
            this.login = login;
            this.isAdmin = isAdmin;
            SelectedDirectory = new EncryptingDirectory();

            //if (!isAdmin) {
            //    button_ChangeFolder.Enabled = false;
            //    button_ProtectData.Enabled = false;
            //    button_DecryptData.Enabled = false;
            //}


        }

        void DrawUI(string uiChange = "do nothing") {
            if (isAdmin) {
                GoToAdminUI(uiChange);
            }
            else {
                GoToUserUI(uiChange);
            }
        }

        void GoToAdminUI(string uiChange) {
            switch (uiChange) {
                case "FirstInit":
                    label_FirstInit.Visible = true;
                    label_FirstInit.Text = "Каталог не выбран";
                    break;
                case "MainMenu":
                    listBox_ProtectedDirectories.Visible = true;
                    button_UnlockChosenDirectory.Visible = true;

                    button_ExitSession.Visible = false;
                    label_DataProtected.Visible = false;

                    button_ChangeFolder.Visible = true;
                    button_DecryptData.Visible = false;
                    break;
                case "NewDirectoryChosen":
                    button_ProtectData.Visible = true;
                    break;
                case "NewDirectoryEncrypted":
                    button_ProtectData.Visible = false;
                    break;
                case "ChosenDirectoryUnlocked":
                    listBox_ProtectedDirectories.Visible = false;
                    button_UnlockChosenDirectory.Visible = false;

                    button_ExitSession.Visible = true;

                    button_ChangeFolder.Visible = false;
                    button_ProtectData.Visible = false;
                    button_DecryptData.Visible = true;
                    break;
                default:
                    break;
            }
        }
        void GoToUserUI(string uiChange) {
            switch (uiChange) {
                case "FirstInit":
                    label_FirstInit.Visible = true;
                    label_FirstInit.Text = "Каталог не выбран";

                    button_ChangeFolder.Visible = false;
                    button_DecryptData.Visible = false;
                    button_ProtectData.Visible = false;
                    break;
                case "MainMenu":
                    listBox_ProtectedDirectories.Visible = true;
                    button_UnlockChosenDirectory.Visible = true;

                    button_ExitSession.Visible = false;
                    label_DataProtected.Visible = false;
                    break;
                case "ChosenDirectoryUnlocked":
                    listBox_ProtectedDirectories.Visible = false;
                    button_UnlockChosenDirectory.Visible = false;

                    button_ExitSession.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            DrawUI("FirstInit");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            //Штатное завершение:
            //при закрытии зашифровать и удалить резервную копию
            //Экстренное завершение:
            //удалить защищаемую папку и оставить резервную копию
            if (isInitialized) {
                if (SelectedDirectory.IsEncrypted) {
                    if (Directory.Exists(RawDirectory))
                        Directory.Delete(RawDirectory, true);
                }
                else {
                    var reason = e.CloseReason;
                    if (Directory.Exists(SelectedDirectory.Path)) {
                        if (reason == CloseReason.TaskManagerClosing)
                            Directory.Delete(SelectedDirectory.Path);
                        else {
                            EncryptContent();
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
            string directoryPath = folderBrowserDialog1.SelectedPath;
            if (!string.IsNullOrEmpty(directoryPath) && !IsSelectedPathInDB(directoryPath)) {
                //Чтобы случайно не зашифровать себе локальный диск
                if (directoryPath.Length > 4) {
                    SelectedDirectory.Path = directoryPath;
                    label_FirstInit.Visible = true;
                    label_FirstInit.Text = "Текущий защищаемый путь: " + SelectedDirectory;
                    DrawUI("NewDirectoryChosen");
                }
                else {
                    directoryPath = null;
                    SelectedDirectory = null;
                    DrawUI("NewDirectoryEncrypted");

                    label_FirstInit.Visible = true;
                    label_FirstInit.Text = "Каталог не выбран";
                }
            }
        }

        private void button_ProtectData_Click(object sender, EventArgs e) {
            EncryptContentInitial();
            dbJournal.ExecuteCommand("INSERT INTO journal (code,login,description,time) VALUES ('4', '" + login + "','New encrypted directory: " + SelectedDirectory + " ', '" + (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff") + "')");
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

            DrawUI("NewDirectoryEncrypted");

            DeleteSelectedDirectoryFromDB();
            dbDirectories.ExecuteCommand("INSERT INTO directories (path, encrypted, keyhash) VALUES ('" + SelectedDirectory + "', '" + 1 + "','" + keyHash + "')");

            RefreshListBox();

            SelectedDirectory = null;
        }

        private void button_ExitSession_Click(object sender, EventArgs e) {
            EncryptContent();
            dbJournal.ExecuteCommand("INSERT INTO journal (code,login,description,time) VALUES ('2', '" + login + "','Encrypted: " + SelectedDirectory + " ', '" + (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff") + "')");
            label_DataProtected.Text = "Данные защищены";
            this.BackgroundImage = Properties.Resources.tom;

            SelectedDirectory.IsEncrypted = true;

            DrawUI("MainMenu");
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
            EncryptContentBase(SelectedDirectory.Path, f => EncryptFileBase(f, File.ReadAllBytes(f), d => Encrypting.ToAes256(d, aesKey)), false);
        }
        void EncryptContent() {
            EncryptContentInitial();
        }
        void DecryptContent() {
            EncryptContentBase(SelectedDirectory.Path, f => DecryptFile(f), true);
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
            SelectedDirectory.IsEncrypted = false;



            //SelectedDirectory = null;
            //directoryPath = null;
            this.BackgroundImage = Properties.Resources.tom;
            DrawUI("MainMenu");

            label_FirstInit.Visible = true;
            label_FirstInit.Text = "Каталог не выбран";

            DeleteSelectedDirectoryFromDB();
            dbJournal.ExecuteCommand("INSERT INTO journal (code,login,description,time) VALUES ('5', '" + login + "','Directory is decrypted and deleted from the DB: " + SelectedDirectory + " ', '" + (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff") + "')");

            RefreshListBox();

            SelectedDirectory = null;
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
                string path = listBox_ProtectedDirectories.SelectedItem.ToString();
                path = path.Remove(path.Length - 1);
                if (Directory.Exists(path)) {
                    SelectedDirectory.Path = path;
                    DecryptContent();
                    dbJournal.ExecuteCommand("INSERT INTO journal (code,login,description,time) VALUES ('3', '" + login + "','Decrypted " + SelectedDirectory + " ', '" + (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff") + "')");
                    isInitialized = true;
                    SelectedDirectory.IsEncrypted = false;
                    label_FirstInit.Visible = false;
                    label_DataProtected.Visible = true;
                    this.BackgroundImage = Properties.Resources.tomNewspaper;
                    label_DataProtected.Text = "Можно юзать данные";
                    DrawUI("ChosenDirectoryUnlocked");
                }
                else 
                    MessageBox.Show("Директории не существует " + SelectedDirectory, "Ошибка");
            }
        }
    }
}
