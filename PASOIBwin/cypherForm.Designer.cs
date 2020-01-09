namespace PASOIBwin
{
    partial class CypherForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CypherForm));
            this.label_FirstInit = new System.Windows.Forms.Label();
            this.button_ExitSession = new System.Windows.Forms.Button();
            this.label_DataProtected = new System.Windows.Forms.Label();
            this.button_UnlockData = new System.Windows.Forms.Button();
            this.button_ChangeFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_ProtectData = new System.Windows.Forms.Button();
            this.button_DecryptData = new System.Windows.Forms.Button();
            this.listBox_ProtectedDirectories = new System.Windows.Forms.ListBox();
            this.button_UnlockChosenDirectory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_FirstInit
            // 
            this.label_FirstInit.AutoSize = true;
            this.label_FirstInit.Location = new System.Drawing.Point(276, 42);
            this.label_FirstInit.Name = "label_FirstInit";
            this.label_FirstInit.Size = new System.Drawing.Size(143, 13);
            this.label_FirstInit.TabIndex = 1;
            this.label_FirstInit.Text = "Первичная инициализация";
            // 
            // button_ExitSession
            // 
            this.button_ExitSession.Location = new System.Drawing.Point(266, 372);
            this.button_ExitSession.Name = "button_ExitSession";
            this.button_ExitSession.Size = new System.Drawing.Size(197, 45);
            this.button_ExitSession.TabIndex = 3;
            this.button_ExitSession.Text = "Выйти из сессии";
            this.button_ExitSession.UseVisualStyleBackColor = true;
            this.button_ExitSession.Visible = false;
            this.button_ExitSession.Click += new System.EventHandler(this.button_ExitSession_Click);
            // 
            // label_DataProtected
            // 
            this.label_DataProtected.AutoSize = true;
            this.label_DataProtected.Location = new System.Drawing.Point(312, 66);
            this.label_DataProtected.Name = "label_DataProtected";
            this.label_DataProtected.Size = new System.Drawing.Size(107, 13);
            this.label_DataProtected.TabIndex = 4;
            this.label_DataProtected.Text = "Данные защищены";
            this.label_DataProtected.Visible = false;
            // 
            // button_UnlockData
            // 
            this.button_UnlockData.Location = new System.Drawing.Point(294, 361);
            this.button_UnlockData.Name = "button_UnlockData";
            this.button_UnlockData.Size = new System.Drawing.Size(144, 24);
            this.button_UnlockData.TabIndex = 5;
            this.button_UnlockData.Text = "Получить доступ";
            this.button_UnlockData.UseVisualStyleBackColor = true;
            this.button_UnlockData.Visible = false;
            this.button_UnlockData.Click += new System.EventHandler(this.button_UnlockData_Click);
            // 
            // button_ChangeFolder
            // 
            this.button_ChangeFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.25F);
            this.button_ChangeFolder.Location = new System.Drawing.Point(480, 132);
            this.button_ChangeFolder.Name = "button_ChangeFolder";
            this.button_ChangeFolder.Size = new System.Drawing.Size(54, 51);
            this.button_ChangeFolder.TabIndex = 6;
            this.button_ChangeFolder.Text = "+";
            this.button_ChangeFolder.UseVisualStyleBackColor = true;
            this.button_ChangeFolder.Click += new System.EventHandler(this.button_ChangeFolder_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "//";
            // 
            // button_ProtectData
            // 
            this.button_ProtectData.Location = new System.Drawing.Point(620, 132);
            this.button_ProtectData.Name = "button_ProtectData";
            this.button_ProtectData.Size = new System.Drawing.Size(168, 51);
            this.button_ProtectData.TabIndex = 7;
            this.button_ProtectData.Text = "Защитить новые данные (Надо кардинально переделать)";
            this.button_ProtectData.UseVisualStyleBackColor = true;
            this.button_ProtectData.Visible = false;
            this.button_ProtectData.Click += new System.EventHandler(this.button_ProtectData_Click);
            // 
            // button_DecryptData
            // 
            this.button_DecryptData.Location = new System.Drawing.Point(575, 349);
            this.button_DecryptData.Name = "button_DecryptData";
            this.button_DecryptData.Size = new System.Drawing.Size(213, 56);
            this.button_DecryptData.TabIndex = 8;
            this.button_DecryptData.Text = "Разблокировать защищаемую папку и выбрать другую";
            this.button_DecryptData.UseVisualStyleBackColor = true;
            this.button_DecryptData.Visible = false;
            this.button_DecryptData.Click += new System.EventHandler(this.Button_DecryptData_Click);
            // 
            // listBox_ProtectedDirectories
            // 
            this.listBox_ProtectedDirectories.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.listBox_ProtectedDirectories.FormattingEnabled = true;
            this.listBox_ProtectedDirectories.HorizontalScrollbar = true;
            this.listBox_ProtectedDirectories.ItemHeight = 17;
            this.listBox_ProtectedDirectories.Location = new System.Drawing.Point(84, 132);
            this.listBox_ProtectedDirectories.Name = "listBox_ProtectedDirectories";
            this.listBox_ProtectedDirectories.Size = new System.Drawing.Size(379, 157);
            this.listBox_ProtectedDirectories.TabIndex = 9;
            // 
            // button_UnlockChosenDirectory
            // 
            this.button_UnlockChosenDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.button_UnlockChosenDirectory.Location = new System.Drawing.Point(84, 298);
            this.button_UnlockChosenDirectory.Name = "button_UnlockChosenDirectory";
            this.button_UnlockChosenDirectory.Size = new System.Drawing.Size(379, 57);
            this.button_UnlockChosenDirectory.TabIndex = 10;
            this.button_UnlockChosenDirectory.Text = "Временно получить доступ к выбранной директории (НЕ РАБОТАЕТ)";
            this.button_UnlockChosenDirectory.UseVisualStyleBackColor = true;
            this.button_UnlockChosenDirectory.Click += new System.EventHandler(this.Button_UnlockChosenDirectory_Click);
            // 
            // CypherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PASOIBwin.Properties.Resources.tom;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_UnlockChosenDirectory);
            this.Controls.Add(this.listBox_ProtectedDirectories);
            this.Controls.Add(this.button_DecryptData);
            this.Controls.Add(this.button_ProtectData);
            this.Controls.Add(this.button_ChangeFolder);
            this.Controls.Add(this.button_UnlockData);
            this.Controls.Add(this.label_DataProtected);
            this.Controls.Add(this.button_ExitSession);
            this.Controls.Add(this.label_FirstInit);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CypherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CypherForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_FirstInit;
        private System.Windows.Forms.Button button_ExitSession;
        private System.Windows.Forms.Label label_DataProtected;
        private System.Windows.Forms.Button button_UnlockData;
        private System.Windows.Forms.Button button_ChangeFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_ProtectData;
        private System.Windows.Forms.Button button_DecryptData;
        private System.Windows.Forms.ListBox listBox_ProtectedDirectories;
        private System.Windows.Forms.Button button_UnlockChosenDirectory;
    }
}

