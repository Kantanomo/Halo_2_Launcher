using Halo_2_Launcher.Controllers;
using MetroFramework.Controls;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            _UpdateController = new UpdateController(this);
            _UpdateController.CheckUpdates();
            DetailsRichTextBox.BackColor = UpdateProgressBar.BackColor;
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