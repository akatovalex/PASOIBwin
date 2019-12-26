namespace PASOIBwin
{
    partial class AuthForm
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
            this.buttonAuth = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.MaskedTextBox();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelUsbCheck = new System.Windows.Forms.Label();
            this.labelUsbText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonAuth
            // 
            this.buttonAuth.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.buttonAuth.Location = new System.Drawing.Point(253, 284);
            this.buttonAuth.Name = "buttonAuth";
            this.buttonAuth.Size = new System.Drawing.Size(214, 113);
            this.buttonAuth.TabIndex = 0;
            this.buttonAuth.Text = "Аутентификация";
            this.buttonAuth.UseVisualStyleBackColor = true;
            this.buttonAuth.Click += new System.EventHandler(this.Button1_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.textBoxPassword.Location = new System.Drawing.Point(281, 196);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(157, 29);
            this.textBoxPassword.TabIndex = 1;
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.textBoxLogin.Location = new System.Drawing.Point(281, 129);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(157, 29);
            this.textBoxLogin.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label1.Location = new System.Drawing.Point(278, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label2.Location = new System.Drawing.Point(278, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Пароль";
            // 
            // labelUsbCheck
            // 
            this.labelUsbCheck.AutoSize = true;
            this.labelUsbCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F);
            this.labelUsbCheck.ForeColor = System.Drawing.Color.LimeGreen;
            this.labelUsbCheck.Location = new System.Drawing.Point(280, 234);
            this.labelUsbCheck.Name = "labelUsbCheck";
            this.labelUsbCheck.Size = new System.Drawing.Size(28, 29);
            this.labelUsbCheck.TabIndex = 5;
            this.labelUsbCheck.Text = "✓";
            // 
            // labelUsbText
            // 
            this.labelUsbText.AutoSize = true;
            this.labelUsbText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.labelUsbText.Location = new System.Drawing.Point(307, 239);
            this.labelUsbText.Name = "labelUsbText";
            this.labelUsbText.Size = new System.Drawing.Size(96, 24);
            this.labelUsbText.TabIndex = 7;
            this.labelUsbText.Text = "USB-ключ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelUsbText);
            this.Controls.Add(this.labelUsbCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxLogin);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.buttonAuth);
            this.Name = "Form1";
            this.Text = "Аутентификация";
            this.Load += new System.EventHandler(this.AuthForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAuth;
        private System.Windows.Forms.MaskedTextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelUsbCheck;
        private System.Windows.Forms.Label labelUsbText;
    }
}