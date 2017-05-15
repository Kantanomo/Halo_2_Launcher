using Halo_2_Launcher.Controllers;
using Halo_2_Launcher.Objects;
using MetroFramework.Controls;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Halo_2_Launcher.Forms
{
    public partial class Update : MetroForm
    {
        delegate void AddToDetailsCallback(string text);
        delegate void UpdateProgressCallback(int Precentage);
        delegate void UpdaterFinishedCallback();
        private UpdateController _UpdateController;

		public Update()
		{
			InitializeComponent();

			HttpWebRequest request = WebRequest.Create(Paths.LauncherCheck) as HttpWebRequest;
			request.Method = "HEAD";
			WebException we = new WebException();
			HttpWebResponse response;
			try { response = request.GetResponse() as HttpWebResponse; }
			catch (WebException ex) { response = ex.Response as HttpWebResponse; }


			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				MessageBox.Show(this, "This launcher is now obsolete." + Environment.NewLine + "A webpage will open to download the new launcher." + Environment.NewLine + "Sorry about the inconvenience.", "New Launcher", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
				Process.Start(@"http://www.halo2vista.com/update");
				Task.Delay(1000);
				ProcessStartInfo Info = new ProcessStartInfo();
				Info.Arguments = "/C ping 127.0.0.1 -n 1 -w 100 > Nul & Del \"" + Application.ExecutablePath + "\"";
				Info.WindowStyle = ProcessWindowStyle.Hidden;
				Info.CreateNoWindow = true;
				Info.WorkingDirectory = Application.StartupPath;
				Info.FileName = "cmd.exe";
				Process.Start(Info);
				Process.GetCurrentProcess().Kill();
			}
			else
			{
				H2Launcher.LauncherSettings.LoadSettings();
				_UpdateController = new UpdateController(this);
				_UpdateController.CheckUpdates();
				DetailsRichTextBox.BackColor = UpdateProgressBar.BackColor;
			}
		}
        
        private void Update_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            
        }
        public void AddToDetails(string Message)
        {
            if (this.DetailsRichTextBox.InvokeRequired)
            {
                AddToDetailsCallback Update = new AddToDetailsCallback(AddToDetails);
                this.Invoke(Update, new object[] { Message });
            }
            else
            {
                string DateStamp = "[" + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() + "]\r\n";
                this.DetailsRichTextBox.Text += DateStamp + Message + "\r\n";
                this.DetailsRichTextBox.SelectionStart = this.DetailsRichTextBox.Text.Length;
                this.DetailsRichTextBox.ScrollToCaret();
            }
        }
        public void UpdateProgress(int Percentage)
        {
            if (this.UpdateProgressBar.InvokeRequired & this.ProgressLabel.InvokeRequired)
            {
                UpdateProgressCallback Update = new UpdateProgressCallback(UpdateProgress);
                this.Invoke(Update, new object[] { Percentage });
            }
            else
            {
                this.UpdateProgressBar.Value = Percentage;
                this.ProgressLabel.Text = this.ProgressLabel.Tag.ToString().Replace("{0}", Percentage.ToString());
            }
        }
        public void UpdaterFinished()
        {
            if (this.InvokeRequired)
            {
                UpdaterFinishedCallback Update = new UpdaterFinishedCallback(UpdaterFinished);
                this.Invoke(Update);
            }
            else
            {
                new MainForm().Show();
                this.Hide();
            }
        }
    }
}