﻿using System;
using System.Windows.Forms;
using MetroFramework.Forms;
using Halo_2_Launcher.Controllers;
using MetroFramework;
using System.Globalization;

namespace Halo_2_Launcher.Forms
{
    public partial class MainForm : MetroForm
    {
        //this.Style = (MetroFramework.MetroColorStyle)Enum.Parse(typeof(MetroFramework.MetroColorStyle), Globals.Settings.Color);
        delegate void ResetLogonFeildsCallback();
        private static MainForm _MainForm = null;
        private Settings _settings;
        private bool SettingsOpen = false;
        private bool RegisterMode = false;
        public MainForm()
        {
            this.ShadowType = MetroFormShadowType.None;
            InitializeComponent();
            //CultureInfo culture = CultureInfo.CurrentCulture;
            //string locale = culture.Name;
            //if (locale.Contains("-"))
            //    locale = locale.Split('-')[0];
            //if(locale.ToLower() == "es")
            //{
                
            //}
            H2Launcher.LauncherSettings.LoadSettings();
            H2Launcher.XliveSettings.LoadSettings();
            this.usernameTextBox.Text = H2Launcher.LauncherSettings.RememberUsername;
            if (H2Launcher.XliveSettings.loginToken != "")
            {
                if (H2Launcher.LauncherSettings.RememberUsername != "")
                    this.passwordTextBox.Text = "PASSWORDHOLDER";
                else
                    H2Launcher.XliveSettings.loginToken = "";
            }
            this.metroLabel3.Visible = false;
            this.emailTextBox1.Visible = false;
            _MainForm = this;
            _settings = new Settings();
            _settings.FormClosed += _settings_Closed;
        }
        //private void switchLanguage(ControlCollection Controls)
        //{
        //    foreach(Control C in Controls)
        //    {
        //        C.Text = C.Tag;
        //        if(C.Controls.Count > 0)

        //    }
        //}
        public void playButton_Click(object sender, EventArgs e)
        {
            if (!this.SettingsOpen)
            {
                var loginResult = H2Launcher.WebControl.Login(usernameTextBox.Text, passwordTextBox.Text, H2Launcher.XliveSettings.loginToken);
                if (loginResult.LoginResultEnum != LoginResultEnum.Successfull)
                {
                    this.usernameTextBox.Text = "";
                    this.passwordTextBox.Text = "";
                }

                switch(loginResult.LoginResultEnum)
                {
                    case LoginResultEnum.Successfull:
                        {
                            H2Launcher.LauncherSettings.RememberUsername = usernameTextBox.Text;
                            H2Launcher.StartHalo(usernameTextBox.Text, loginResult.LoginToken, this);
                            break;
                        }
                    case LoginResultEnum.InvalidLoginToken:
                        {
                            MetroMessageBox.Show(this, "The login token was no longer valid.\r\nPlease re-enter your login information and try again.", Fun.PauseIdiomGenerator, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            H2Launcher.XliveSettings.loginToken = "";
                            break;
                        }
                    case LoginResultEnum.InvalidUsernameOrPassword:
                        {
                            MetroMessageBox.Show(this, "The username or password entered is either incorrect or invalid.\r\nPlease try again.", Fun.PauseIdiomGenerator, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            break;
                        }
                    case LoginResultEnum.Banned:
                        {
                            if (MetroMessageBox.Show(this, "You have been banned, please visit the forum to appeal your ban.\r\nWould you like us to open the forums for you?.", Fun.PauseIdiomGenerator, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error) == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(@"http://www.halo2vista.com/forums/");
                            }
                            break;
                        }
                    case LoginResultEnum.GenericFailure:
                        {
                            //wat do?
                            break;
                        }
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "You cannot do that!" + Environment.NewLine + Environment.NewLine + "Close the settings menu before you launch the game.", "Wait a moment!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                this._settings.BringToFront();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        #region Settings
        private void settingsButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this._settings.Show();
            this._settings.SetDesktopLocation(this._settings.Location.X, this._settings.Location.Y);
            this.SettingsOpen = true;
        }
        private void _settings_Closed(object sender, EventArgs e)
        {
            this.SettingsOpen = false;
            this._settings = new Settings();
            _settings.FormClosed += _settings_Closed;
            this.Show();
        }
        #endregion 

        private void registerButton_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(@"http://www.cartographer.h2pc.org/");
            if (this.RegisterMode == false)
            {
                this.usernameTextBox.Focus();
                this.metroLabel3.Visible = true;
                this.emailTextBox1.Visible = true;
                this.registerButton.Width = 204;
                this.registerButton.Location = new System.Drawing.Point(23, 172);
                this.emailTextBox1.Text = "";
                this.usernameTextBox.Text = "";
                this.passwordTextBox.Text = "";
                this.settingsButton.Visible = false;
                this.RegisterMode = true;
                this.cancelButton.Visible = true;
                this.Invalidate();
            }
            else if (this.RegisterMode == true)
            {
                this.metroLabel3.Visible = false;
                this.emailTextBox1.Visible = false;
                this.registerButton.Width = 83;
                this.registerButton.Location = new System.Drawing.Point(144, 172);
                this.RegisterMode = false;
                this.cancelButton.Visible = false;
                this.settingsButton.Visible = true;
                this.Invalidate();
                this.usernameTextBox.Focus();
                if (H2Launcher.WebControl.Register(this.usernameTextBox.Text, this.passwordTextBox.Text, this.emailTextBox1.Text))
                {
                    MetroMessageBox.Show(this, "Account created", Fun.GoIdioms, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MetroMessageBox.Show(this, "Account created failed\r\nOpening the web registration page instead.", Fun.GoIdioms, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Diagnostics.Process.Start(@"http://www.cartographer.h2pc.org/");
                }
                //H2Launcher.WebControl.Register(this, this.usernameTextBox.Text, this.passwordTextBox.Text);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {
            this.Text = Fun.GoIdioms;
            this.Invalidate();
        }

        private void metroLabel2_Click(object sender, EventArgs e)
        {
            this.Text = Fun.PauseIdiomGenerator;
            this.Invalidate();
        }

        private void usernameTextBox_Click(object sender, EventArgs e)
        {
            if (!this.RegisterMode)
            {
                if (H2Launcher.XliveSettings.loginToken != "")
                    H2Launcher.XliveSettings.loginToken = "";
                if (this.passwordTextBox.Text != "")
                    this.passwordTextBox.Text = "";
            }
        }
        private void passwordTextBox_Click(object sender, EventArgs e)
        {
            if (!this.RegisterMode)
            {
                if (H2Launcher.XliveSettings.loginToken != "")
                    H2Launcher.XliveSettings.loginToken = "";
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (this.RegisterMode == true)
            {
                this.metroLabel3.Visible = false;
                this.emailTextBox1.Visible = false;
                this.registerButton.Width = 83;
                this.registerButton.Location = new System.Drawing.Point(144, 172);
                this.RegisterMode = false;
                this.cancelButton.Visible = false;
                this.settingsButton.Visible = true;
                this.Invalidate();
                this.usernameTextBox.Focus();
            }
        }
    }
}
