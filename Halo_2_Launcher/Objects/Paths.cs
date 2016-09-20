﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Halo_2_Launcher.Objects
{
    public static class Paths
    {
        public static string ExecutablePath
        {
            get
            {
                return InstallPath + "\\Halo2.exe";
            }
        }
        public static string InstallPath
        {
            get
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft Games\Halo 2\1.0\") == null)
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft Games\Halo 2\1.0\");
                    return (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft Games\Halo 2\1.0", "GameInstallDir", null);

                }
                else
                {
                    if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft Games\Halo 2\1.0\") == null)
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Microsoft Games\Halo 2\1.0\");
                    return (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft Games\Halo 2\1.0", "GameInstallDir", null);
                }
            }
            set
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft Games\Halo 2\1.0\") == null)
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft Games\Halo 2\1.0\");
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft Games\Halo 2\1.0", "GameInstallDir", value);
                }
                else
                {
                    if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft Games\Halo 2\1.0\") == null)
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Microsoft Games\Halo 2\1.0\");
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft Games\Halo 2\1.0", "GameInstallDir", value);
                }
            }
        }
        public static string AppData
        {
            get
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\H2PC\\H2Cartographer\\")) Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\H2PC\\H2Cartographer\\");
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\H2PC\\H2Cartographer\\Download")) Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\H2PC\\H2Cartographer\\Download");
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\H2PC\\H2Cartographer\\Files")) Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\H2PC\\H2Cartographer\\Files");
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\H2PC\\H2Cartographer\\Files\\Content")) Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\H2PC\\H2Cartographer\\Files\\Content");
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\H2PC\\H2Cartographer\\";
            }
        }
        public static string LauncherPath
        {
            get { return AppData; }
        }
        public static string Downloads
        {
            get
            {
                return AppData + "\\Download\\";
            }
        }
        public static string Files
        {
            get
            {
                return AppData + "\\Files\\";
            }
        }
        public static string Content
        {
            get
            {
                return AppData + "\\Files\\Content\\";
            }
        }
        public static string H2RegistryBase
        {
            get { return @"HKEY_CURRENT_USER\Software\Microsoft\Halo 2\"; }
        }
        public static string RemotePath
        {
            get { return @"http://halo2vista.com/H2Cartographer/"; }
        }
        public static string RemoteApiPath
        {
            get { return RemotePath + "H2Cartographer.php"; }
        }
        public static string RemoteUpdatePath
        {
            get { return RemotePath + "update/"; }
        }
        public static string RemoteUpdateXML
        {
            get { return RemotePath + "update.xml"; }
        }
    }
}
