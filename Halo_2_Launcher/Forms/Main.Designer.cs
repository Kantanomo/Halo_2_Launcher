namespace Halo_2_Launcher.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.playButton = new MetroFramework.Controls.MetroButton();
            this.passwordTextBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.usernameTextBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.settingsButton = new MetroFramework.Controls.MetroButton();
            this.registerButton = new MetroFramework.Controls.MetroButton();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.emailTextBox1 = new MetroFramework.Controls.MetroTextBox();
            this.cancelButton = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // playButton
            // 
            this.playButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.playButton.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.playButton.Location = new System.Drawing.Point(23, 172);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(115, 31);
            this.playButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.playButton.TabIndex = 5;
            this.playButton.Text = "Play";
            this.playButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.playButton.UseSelectable = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Lines = new string[0];
            this.passwordTextBox.Location = new System.Drawing.Point(124, 91);
            this.passwordTextBox.MaxLength = 255;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '•';
            this.passwordTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.passwordTextBox.SelectedText = "";
            this.passwordTextBox.Size = new System.Drawing.Size(184, 23);
            this.passwordTextBox.Style = MetroFramework.MetroColorStyle.Blue;
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.Theme = MetroFramework.MetroThemeStyle.Light;
            this.passwordTextBox.UseSelectable = true;
            this.passwordTextBox.Click += new System.EventHandler(this.passwordTextBox_Click);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel2.Location = new System.Drawing.Point(23, 89);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(91, 25);
            this.metroLabel2.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel2.TabIndex = 4;
            this.metroLabel2.Tag = "Contraseña:";
            this.metroLabel2.Text = "Password:";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel2.Click += new System.EventHandler(this.metroLabel2_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Lines = new string[0];
            this.usernameTextBox.Location = new System.Drawing.Point(124, 62);
            this.usernameTextBox.MaxLength = 15;
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.PasswordChar = '\0';
            this.usernameTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.usernameTextBox.SelectedText = "";
            this.usernameTextBox.Size = new System.Drawing.Size(184, 23);
            this.usernameTextBox.Style = MetroFramework.MetroColorStyle.Blue;
            this.usernameTextBox.TabIndex = 1;
            this.usernameTextBox.Theme = MetroFramework.MetroThemeStyle.Light;
            this.usernameTextBox.UseSelectable = true;
            this.usernameTextBox.Click += new System.EventHandler(this.usernameTextBox_Click);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel1.Location = new System.Drawing.Point(23, 60);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(95, 25);
            this.metroLabel1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Tag = "Usuario:";
            this.metroLabel1.Text = "Username:";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel1.Click += new System.EventHandler(this.metroLabel1_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.settingsButton.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.settingsButton.ForeColor = System.Drawing.Color.DimGray;
            this.settingsButton.Location = new System.Drawing.Point(233, 172);
            this.settingsButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(75, 31);
            this.settingsButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.settingsButton.TabIndex = 6;
            this.settingsButton.Text = "Settings";
            this.settingsButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.settingsButton.UseSelectable = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.registerButton.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.registerButton.Location = new System.Drawing.Point(144, 172);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(83, 31);
            this.registerButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.registerButton.TabIndex = 5;
            this.registerButton.Tag = "";
            this.registerButton.Text = "Register";
            this.registerButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.registerButton.UseSelectable = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel3.Location = new System.Drawing.Point(23, 118);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(58, 25);
            this.metroLabel3.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel3.TabIndex = 8;
            this.metroLabel3.Tag = "Correo:";
            this.metroLabel3.Text = "Email:";
            this.metroLabel3.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // emailTextBox1
            // 
            this.emailTextBox1.Lines = new string[0];
            this.emailTextBox1.Location = new System.Drawing.Point(124, 120);
            this.emailTextBox1.MaxLength = 0;
            this.emailTextBox1.Name = "emailTextBox1";
            this.emailTextBox1.PasswordChar = '\0';
            this.emailTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.emailTextBox1.SelectedText = "";
            this.emailTextBox1.Size = new System.Drawing.Size(184, 23);
            this.emailTextBox1.Style = MetroFramework.MetroColorStyle.Blue;
            this.emailTextBox1.TabIndex = 3;
            this.emailTextBox1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.emailTextBox1.UseSelectable = true;
            // 
            // cancelButton
            // 
            this.cancelButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.cancelButton.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.cancelButton.Location = new System.Drawing.Point(233, 172);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 31);
            this.cancelButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Tag = "";
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.cancelButton.UseSelectable = true;
            this.cancelButton.Visible = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(331, 221);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.emailTextBox1);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.cancelButton);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ShowIcon = false;
            this.Tag = new int[] {
        300,
        500};
            this.Text = "H2PC Launcher";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox usernameTextBox;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroButton playButton;
        private MetroFramework.Controls.MetroButton settingsButton;
        private MetroFramework.Controls.MetroButton registerButton;
        public MetroFramework.Controls.MetroTextBox passwordTextBox;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        public MetroFramework.Controls.MetroTextBox emailTextBox1;
        private MetroFramework.Controls.MetroButton cancelButton;
    }
}

