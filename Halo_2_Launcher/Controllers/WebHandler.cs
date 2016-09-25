﻿using Halo_2_Launcher.Objects;
using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Halo_2_Launcher.Controllers
{
    public class WebHandler
    {
        private string Api = "http://cartographer.online/api.php";
        public bool Login(Halo_2_Launcher.Forms.MainForm Form, string username, string password, string rememberToken = "")
        {
            var pairs = new List<KeyValuePair<string, string>>
              {
                new KeyValuePair<string, string>("launcher", "1")
              };

            if (rememberToken != "")
                pairs.Add(new KeyValuePair<string, string>("token", rememberToken));
            else
            {
                pairs.Add(new KeyValuePair<string, string>("user", username));
                pairs.Add(new KeyValuePair<string, string>("pass", password));
            }
            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(Api, content).Result;
                var contentString = response.Content.ReadAsStringAsync().Result;
                if (rememberToken != "" && rememberToken.Length == 32)
                {
                    if (contentString == "0")//Invalid Token
                    {
                        MetroMessageBox.Show(Form, "The login token was no longer valid.\r\nPlease re-enter your login information and try again.", Fun.PauseIdiomGenerator, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        H2Launcher.XliveSettings.loginToken = "";
                        return false;
                    }
                    if(contentString == "1")
                    {
                        H2Launcher.LauncherSettings.RememberUsername = username;
                        H2Launcher.StartHalo(username, rememberToken, Form);
                        return true;
                    }
                }
                else
                {
                    if(contentString == "0")//Invalid username or password
                    {
                        MetroMessageBox.Show(Form, "The username or password entered is either incorrect or invalid.\r\nPlease try again.", Fun.PauseIdiomGenerator, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return false;
                    }
                    else if(contentString.Length == 32) //login successful
                    {
                        H2Launcher.LauncherSettings.RememberUsername = username;
                        H2Launcher.StartHalo(username, contentString, Form);
                        return true;
                    }
                }
                return false;
            }
        }
        public bool Register(Halo_2_Launcher.Forms.MainForm Form, string user, string pass, string email)
        {
            var pairs = new List<KeyValuePair<string, string>>
              {
                new KeyValuePair<string, string>("launcher", "1"),
                new KeyValuePair<string, string>("user", user),
                new KeyValuePair<string, string>("pass", pass),
                new KeyValuePair<string, string>("email", email)
              };
            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(Api, content).Result;
                var contentString = response.Content.ReadAsStringAsync().Result;
                return (contentString == "0") ? true : false;
            }
        }
    }
}
