using Halo_2_Launcher.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halo_2_Launcher.Controllers
{
    public class Settings
    {
        private int _ResolutionWidth = RegistryControl.GetScreenResX();
        private int _ResolutionHeight = RegistryControl.GetScreenResY();
        private H2DisplayMode _DisplayMode = RegistryControl.GetDisplayMode();
        private bool _Sound = true;
        private bool _H2VSync = true;
        private int _StartingMonitor = 0;
        private bool _Intro = !Directory.Exists(Paths.InstallPath + "\\movie.bak");
        private bool _H2XFOV = false;
        private int _fieldOfView = 78;
        private bool _RememberAccount = false;
        private string _RememberToken = "";
        private string _RememberUsername = "";
        private string _LauncherStyle = "Orange";
        public int ResolutionWidth
        {
            get { return this._ResolutionWidth; }
            set { this._ResolutionWidth = value; }
        }
        public int ResolutionHeight
        {
            get { return this._ResolutionHeight; }
            set { this._ResolutionHeight = value; }
        }
        public H2DisplayMode DisplayMode
        {
            get { return this._DisplayMode; }
            set { this._DisplayMode = value; }
        }
        public bool Sound
        {
            get { return this._Sound; }
            set { this._Sound = value; }
        }
        public bool H2VSync
        {
            get { return this._H2VSync; }
            set { this._H2VSync = value; }
        }
        public int StartingMonitor
        {
            get { return this._StartingMonitor; }
            set { this._StartingMonitor = value; }
        }
        public bool Intro
        {
            get { return this._Intro; }
            set { this._Intro = value; }
        }
        public int FieldOfView
        {
            get { return this._fieldOfView; }
            set { this._fieldOfView = value; }
        }
        public bool RememberAccount
        {
            get { return this._RememberAccount; }
            set { this._RememberAccount = value; }
        }
        public string RememberToken
        {
            get { return this._RememberToken; }
            set { this._RememberToken = value; }
        }
        public string RememberUsername
        { 
            get { return this._RememberUsername; }
            set { this._RememberUsername = value; }
        }
        public string LauncherStyle
        {
            get { return this._LauncherStyle; }
            set { this._LauncherStyle = value; }
        }

        private int GetPrimaryMonitor()
        {
            for (int i = 0; i < Screen.AllScreens.Length; i++)
                if (Screen.AllScreens[i].Primary)
                    return i;
            return 0; //return 0 if nothing is set as default
        }
        public void LoadSettings()
        {
            if (!File.Exists(Paths.Files + "Settings.ini")) SaveSettings();
            else
            {
                StreamReader SR = new StreamReader(Paths.Files + "Settings.ini");
                string[] Lines = SR.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                SR.Close();
                SR.Dispose();
                foreach (string Line in Lines)
                {
                    string[] Setting = Line.Split(':');
                    switch (Setting[0])
                    {
                        case "ResolutionHeight":
                            {
                                this.ResolutionHeight = int.Parse(Setting[1]);
                                break;
                            }
                        case "ResolutionWidth":
                            {
                                this.ResolutionWidth = int.Parse(Setting[1]);
                                break;
                            }
                        case "DisplayMode":
                            {
                                this.DisplayMode = (H2DisplayMode)Enum.Parse(typeof(H2DisplayMode), Setting[1]);
                                break;
                            }
                        case "Sound":
                            {
                                this.Sound = bool.Parse(Setting[1]);
                                break;
                            }
                        case "H2VSync":
                            {
                                this.H2VSync = bool.Parse(Setting[1]);
                                break;
                            }
                        case "StartingMonitor":
                            {
                                this.StartingMonitor = int.Parse(Setting[1]);
                                break;
                            }
                        case "Intro":
                            {
                                this.Intro = bool.Parse(Setting[1]);
                                break;
                            }
                        case "FieldOfView":
                            {
                                this.FieldOfView = int.Parse(Setting[1]);
                                break;
                            }
                        case "RememberAccount":
                            {
                                this.RememberAccount = bool.Parse(Setting[1]);
                                break;
                            }
                        case "RememberToken":
                            {
                                this.RememberToken = DecryptStringAES(Setting[1]);
                                break;
                            }
                        case "RememberUsername":
                            {
                                this.RememberUsername = Setting[1];
                                break;
                            }
                        case "LauncherStyle":
                            {
                                this._LauncherStyle = Setting[1];
                                break;
                            }
                    }
                }
            }
        }
        public void SaveSettings()
        {
            StreamWriter SW = new StreamWriter(Paths.Files + "Settings.ini");
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("ResolutionHeight:" + this.ResolutionHeight.ToString());
            SB.AppendLine("ResolutionWidth:" + this.ResolutionWidth.ToString());
            SB.AppendLine("DisplayMode:" + DisplayMode.ToString());
            SB.AppendLine("Sound:" + Sound.ToString());
            SB.AppendLine("H2VSync:" + H2VSync.ToString());
            SB.AppendLine("StartingMonitor:" + StartingMonitor.ToString());
            SB.AppendLine("Intro:" + Intro.ToString());
            SB.AppendLine("FieldOfView:" + FieldOfView.ToString());
            SB.AppendLine("RememberAccount:" + RememberAccount.ToString());
            SB.AppendLine("RememberToken:" + EncryptStringAES(RememberToken));
            SB.AppendLine("RememberUsername:" + RememberUsername);
            SB.AppendLine("LauncherStyle:" + LauncherStyle);
            SW.Write(SB.ToString());
            SW.Flush();
            SW.Close();
            SW.Dispose();
            
        }
        private byte[] _salt = Encoding.ASCII.GetBytes("GIVEITTOMEDICK");
        private string EncryptStringAES(string plainText)
        {
            string Secret = "";
            ManagementClass Class = new ManagementClass("win32_processor");
            ManagementObjectCollection Collec = Class.GetInstances();
            foreach (ManagementObject Obj in Collec)
            {
                if (Secret == "")
                {
                    Secret = Obj.Properties["processorID"].Value.ToString().Substring(0, 8);
                    break;
                }
            }
            if (!string.IsNullOrEmpty(plainText) & !string.IsNullOrEmpty(Secret))
            {
                string outStr = null;
                RijndaelManaged aesAlg = null;
                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Secret, _salt);
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) swEncrypt.Write(plainText);
                        outStr = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
                finally
                {
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
                return outStr;
            }
            return "";
        }
        private string DecryptStringAES(string cipherText)
        {
            string Secret = "";
            ManagementClass Class = new ManagementClass("win32_processor");
            ManagementObjectCollection Collec = Class.GetInstances();
            foreach (ManagementObject Obj in Collec)
            {
                if (Secret == "")
                {
                    Secret = Obj.Properties["processorID"].Value.ToString().Substring(0, 8);
                    break;
                }
            }
            if (!string.IsNullOrEmpty(cipherText) & !string.IsNullOrEmpty(Secret))
            {
                RijndaelManaged aesAlg = null;
                string plaintext = null;
                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Secret, _salt);
                    byte[] bytes = Convert.FromBase64String(cipherText);
                    using (MemoryStream msDecrypt = new MemoryStream(bytes))
                    {
                        aesAlg = new RijndaelManaged();
                        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                        aesAlg.IV = ReadByteArray(msDecrypt);
                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
                finally
                {
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
                return plaintext;
            }
            return "";
        }
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
                throw new SystemException("Stream did not contain properly formatted byte array");

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
                throw new SystemException("Did not read byte array properly");

            return buffer;
        }
    }
}
