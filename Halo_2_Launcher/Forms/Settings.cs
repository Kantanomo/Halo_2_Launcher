﻿using Halo_2_Launcher.Controllers;
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
using System.Xml;
using System.Xml.Linq;

namespace Halo_2_Launcher
{
    public partial class Settings : MetroForm
    {
        private bool _SettingsSaved = false;
        public bool LIVE = false;
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
            this.fieldOfView.Value = (int)H2Launcher.LauncherSettings.FieldOfView;
            this.fovLabel.Text = H2Launcher.LauncherSettings.FieldOfView.ToString();
            this.debugLogToggle.Checked = (H2Launcher.XliveSettings.DebugLog == 1) ? true : false;
            this.game_ports_textBox.Text = H2Launcher.XliveSettings.Ports.ToString();
            this.fpsToggle.Checked = (H2Launcher.XliveSettings.FPSCap == 1) ? true : false;
            this.fps_value_textbox.Text = H2Launcher.XliveSettings.FPSLimit.ToString();
            this.voiceChatToggle.Checked = (H2Launcher.XliveSettings.VoiceChat == 1) ? true : false;
            this.map_download_toggle.Checked = (H2Launcher.XliveSettings.MapDownload == 1) ? true : false;
            this.gameEnvironmentComboBox.SelectedItem = H2Launcher.LauncherSettings.GameEnvironment.ToString();
            this.metroTabControl1.SelectedIndex = 0;
            this.Invalidate();
            #endregion

            if (!this.fpsToggle.Checked)
                this.fps_value_textbox.Enabled = false;
            else
                this.fps_value_textbox.Enabled = true;
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
            if (gameEnvironmentComboBox.SelectedItem.ToString() == "Cartographer")
                MessageBox.Show("This will enable Project Cartographer and all its glory!");
            if (gameEnvironmentComboBox.SelectedItem.ToString() == "LIVE")
                MessageBox.Show("This will make Halo 2 run like normal, it enables Games for Windows LIVE.");
            if (gameEnvironmentComboBox.SelectedItem.ToString() == "Xliveless")
                MessageBox.Show("This will enable xliveless, which allows for campaign play without FPS drops.\r\nThis does disable Project Cartographer (which disables multipaler).");

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
            H2Launcher.LauncherSettings.FieldOfView = (float)fieldOfView.Value;
            H2Launcher.XliveSettings.FPSCap = (this.fpsToggle.Checked) ? 1 : 0;
            H2Launcher.LauncherSettings.GameEnvironment = (H2GameEnvironment)Enum.Parse(typeof(H2GameEnvironment), this.gameEnvironmentComboBox.SelectedItem.ToString());
            H2Launcher.XliveSettings.DebugLog = (this.debugLogToggle.Checked) ? 1 : 0;
            H2Launcher.XliveSettings.Ports = int.Parse(this.game_ports_textBox.Text);
            H2Launcher.XliveSettings.FPSCap = (this.fpsToggle.Checked) ? 1 : 0;
            H2Launcher.XliveSettings.FPSLimit = int.Parse(this.fps_value_textbox.Text);
            H2Launcher.XliveSettings.VoiceChat = (this.voiceChatToggle.Checked) ? 1 : 0;
            H2Launcher.XliveSettings.MapDownload = (this.map_download_toggle.Checked) ? 1 : 0;
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

            string xlive = Paths.InstallPath + "xlive.dll";
            string xliveless = Paths.InstallPath + "xliveless.dll";
            string xlive_project = Paths.InstallPath + "xlive_project.dll";
            string game_activation = Paths.InstallPath + "MF.dll";
            string game_deactivation = Paths.InstallPath + "MF_disable.dll";
            string localXML = Paths.Files + "LocalUpdate.xml";

            XmlDocument local = new XmlDocument();
            local.Load(localXML);
            XmlNode file = local.DocumentElement.FirstChild;
            XmlNodeList nameNodeList = local.GetElementsByTagName("name");
            XmlNodeList localNodeList = local.GetElementsByTagName("localpath");

            switch (H2Launcher.LauncherSettings.GameEnvironment)
            {
                case H2GameEnvironment.LIVE:
                    {
                        if (File.Exists(localXML))
                        {
                            foreach (XmlNode nameNode in nameNodeList)
                            {
                                if (nameNode.InnerXml == "XLive")
                                {
                                    foreach (XmlNode localNode in localNodeList)
                                    {
                                        if (localNode.InnerXml == "{InstallDir}\\xlive.dll")
                                            localNode.InnerText.Replace("{InstallDir}\\xlive.dll", "{InstallDir}\\xlive_project.dll");
                                        //MessageBox.Show(localNode.InnerXml);
                                    }
                                }
                                if(nameNode.InnerXml == "XLiveless")
                                {
                                    foreach (XmlNode localNode in localNodeList)
                                    {
                                        if (localNode.InnerXml == "{InstallDir}\\xlive.dll")
                                            localNode.InnerText.Replace("{InstallDir}\\xlive.dll", "{InstallDir}\\xliveless.dll");
                                        //MessageBox.Show(localNode.InnerXml);
                                    }
                                }
                                if (nameNode.InnerXml == "{InstallDir}\\MF.dll")
                                    nameNode.InnerText = "{InstallDir}\\MF_disable.dll";
                                local.Save(localXML);
                            }
                        }
                        if (File.Exists(game_activation))
                            File.Move(game_activation, game_deactivation);
                        if (File.Exists(xlive) && File.Exists(xliveless))
                            File.Move(xlive, xlive_project);
                        else if (File.Exists(xlive) && File.Exists(xlive_project))
                            File.Move(xlive, xliveless);
                        H2Launcher.LauncherSettings.GameEnvironment = H2GameEnvironment.LIVE;
                        break;
                    }
                case H2GameEnvironment.Cartographer:
                    {
                        if (File.Exists(game_deactivation))
                            File.Move(game_deactivation, game_activation);
                        if (File.Exists(xlive) && !File.Exists(xliveless))
                        {
                            File.Move(xlive, xliveless);
                            File.Move(xlive_project, xlive);
                        }
                        else if (!File.Exists(xlive) && File.Exists(xliveless))
                            File.Move(xlive_project, xlive);
                        H2Launcher.LauncherSettings.GameEnvironment = H2GameEnvironment.Cartographer;
                        break;
                    }
                case H2GameEnvironment.Xliveless:
                    {
                        if (File.Exists(game_activation))
                            File.Delete(game_activation);
                        if (!File.Exists(xlive) && !File.Exists(xliveless))
                            File.Move(xlive_project, xlive);
                        else if (File.Exists(xlive) && File.Exists(xliveless))
                        {
                            File.Move(xlive, xlive_project);
                            File.Move(xliveless, xlive);
                        }
                        H2Launcher.LauncherSettings.GameEnvironment = H2GameEnvironment.Xliveless;
                        break;
                    }
            }
            H2Launcher.LauncherSettings.SaveSettings();
            H2Launcher.XliveSettings.SaveSettings();
        }
        private void fpsToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.fpsToggle.Checked)
                this.fps_value_textbox.Enabled = false;
            else
                this.fps_value_textbox.Enabled = true;
        }
        private void fps_value_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }

        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }
    }
}