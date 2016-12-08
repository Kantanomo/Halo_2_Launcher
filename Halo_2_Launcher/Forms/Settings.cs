using Halo_2_Launcher.Controllers;
using Halo_2_Launcher.Objects;
using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Halo_2_Launcher
{
    public partial class Settings : MetroForm
    {
        private bool _SettingsSaved = false;
        public Settings()
        {
            InitializeComponent();
            #region SetControls
            this.widthTextBox.Text = H2Launcher.LauncherSettings.ResolutionWidth.ToString();
            this.heightTextBox.Text = H2Launcher.LauncherSettings.ResolutionHeight.ToString();
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                startingMonitorComboBox.Items.Add((i + 1).ToString() + ((Screen.AllScreens[i].Primary) ? " Primary" : ""));
                if (i == H2Launcher.LauncherSettings.StartingMonitor) startingMonitorComboBox.SelectedIndex = i;
            }
            this.windowModeComboBox.SelectedItem = H2Launcher.LauncherSettings.DisplayMode.ToString();
            this.windowModeComboBox.SelectedIndexChanged += windowModeComboBox_SelectedIndexChanged;
            this.vsyncToggle.Checked = H2Launcher.LauncherSettings.H2VSync;
            this.soundToggle.Checked = H2Launcher.LauncherSettings.Sound;
            this.introToggle.Checked = H2Launcher.LauncherSettings.Intro;
            this.fieldOfView.Value = H2Launcher.XliveSettings.FieldOfView;
            this.fovLabel.Text = H2Launcher.XliveSettings.FieldOfView.ToString();
            this.debugLogToggle.Checked = (H2Launcher.XliveSettings.DebugLog == 1) ? true : false;
            this.fpsToggle.Checked = (H2Launcher.XliveSettings.FPSCap == 1) ? true : false;
            this.metroTabControl1.SelectedIndex = 0;
            if (H2Launcher.LauncherSettings.xDelayHotkey != "")
                this.xDelayCombo.SelectedIndex = this.xDelayCombo.FindStringExact(H2Launcher.LauncherSettings.xDelayHotkey.Trim());
            if (H2Launcher.LauncherSettings.noHUDHotkey != "")
                this.noHudCombo.SelectedIndex = this.noHudCombo.FindStringExact(H2Launcher.LauncherSettings.noHUDHotkey.Trim());
            this.Invalidate();
            #endregion
        }

        private void textBox_Numerical(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back))) e.Handled = true;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_SettingsSaved)
            {
                if (MessageBox.Show("Do you want to save all the changed settings?", Fun.PauseIdiomGenerator, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveSettings();
                }
            }
        }

        private void forceUpdateButton_Click(object sender, EventArgs e)
        {
            File.Delete(Paths.Files + "LocalUpdate.xml");
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C ping 127.0.0.1 -n 1 -w 5000 > Nul & start Halo_2_Launcher.exe";
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.WorkingDirectory = Application.StartupPath;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
            Process.GetCurrentProcess().Kill();
        }
        public void windowModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (windowModeComboBox.SelectedItem.ToString() == "Fullscreen")
            {
                MessageBox.Show("Custom resolutions will not work using the Fullscreen option, Please use a standard resolution when selecting this option.");
                //MetroMessageBox.Show(this, , Fun.PauseIdiomGenerator, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            _SettingsSaved = true;
            SaveSettings();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _SettingsSaved = true;
            this.Close();
        }
        private void fieldOfView_Scroll(object sender, ScrollEventArgs e)
        {
            switch (e.NewValue)
            {
                case 57:
                    {
                        fovLabel.Text = "Default";
                        fovLabel.Style = MetroColorStyle.Green;
                        break;
                    }
                case 65:
                    {
                        fovLabel.Text = "Xbox";
                        fovLabel.Style = MetroColorStyle.Green;
                        break;
                    }
                default:
                    {
                        fovLabel.Text = e.NewValue.ToString();
                        fovLabel.Style = MetroColorStyle.Black;
                        break;
                    }
            }
            fovLabel.Invalidate(); //You have to invalidate the Graphics of a Metro Control to update it.
        }
        private void SaveSettings()
        {
            H2Launcher.LauncherSettings.ResolutionHeight = int.Parse(this.heightTextBox.Text);
            H2Launcher.LauncherSettings.ResolutionWidth = int.Parse(this.widthTextBox.Text);
            H2Launcher.LauncherSettings.DisplayMode = (H2DisplayMode)Enum.Parse(typeof(H2DisplayMode), this.windowModeComboBox.SelectedItem.ToString());
            H2Launcher.LauncherSettings.StartingMonitor = startingMonitorComboBox.SelectedIndex;
            H2Launcher.LauncherSettings.H2VSync = vsyncToggle.Checked;
            H2Launcher.LauncherSettings.Sound = soundToggle.Checked;
            H2Launcher.LauncherSettings.Intro = introToggle.Checked;
            H2Launcher.XliveSettings.FieldOfView = fieldOfView.Value;
            H2Launcher.XliveSettings.CrosshairOffset = (float)Math.Round(0.284302f - (0.00209302f * fieldOfView.Value), 4, MidpointRounding.ToEven);
            H2Launcher.XliveSettings.DebugLog = (this.debugLogToggle.Checked) ? 1 : 0;
            H2Launcher.XliveSettings.FPSCap = (this.fpsToggle.Checked) ? 1 : 0;
            H2Launcher.LauncherSettings.xDelayHotkey = this.xDelayCombo.SelectedItem != null ? this.xDelayCombo.SelectedItem.ToString() : string.Empty;
            H2Launcher.LauncherSettings.noHUDHotkey = this.noHudCombo.SelectedItem != null ? this.noHudCombo.SelectedItem.ToString() : string.Empty;
            if (!introToggle.Checked)
            {
                if (!Directory.Exists(Paths.InstallPath + "\\movie.bak"))
                {
                    Directory.Move(Paths.InstallPath + "\\movie", Paths.InstallPath + "\\movie.bak");
                    Directory.CreateDirectory(Paths.InstallPath + "\\movie");
                    File.Create(Paths.InstallPath + "\\movie\\credits_60.wmv").Close();
                    File.Create(Paths.InstallPath + "\\movie\\intro_60.wmv").Close();
                    File.Create(Paths.InstallPath + "\\movie\\intro_low_60.wmv").Close();
                }
            }
            else
            {
                if (Directory.Exists(Paths.InstallPath + "\\movie.bak"))
                {
                    Directory.Delete(Paths.InstallPath + "\\movie", true);
                    Directory.Move(Paths.InstallPath + "\\movie.bak", Paths.InstallPath + "\\movie");
                }
            }
            H2Launcher.LauncherSettings.SaveSettings();
            H2Launcher.XliveSettings.SaveSettings();
        }
    }
}