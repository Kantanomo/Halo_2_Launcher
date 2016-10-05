using Halo_2_Launcher.Objects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Halo_2_Launcher.Controllers
{
    public static class RegistryControl
    {
        public static void SetScreenResX(int X)
        {
            SetVideoSetting("ScreenResX", X);
        }
        public static void SetScreenResY(int Y)
        {
            SetVideoSetting("ScreenResY", Y);
        }
        public static void SetDisplayMode(bool FullScreen)
        {
            SetVideoSetting("DisplayMode", (FullScreen) ? 0 : 1);
        }
        public static int GetScreenResX()
        {
            try
            {
                return (int)GetVideoSetting("ScreenResX");
            }
            catch (Exception)
            {
                return (int)Screen.PrimaryScreen.Bounds.Width;
            }
        }
        public static int GetScreenResY()
        {
            try
            {
                return (int)GetVideoSetting("ScreenResY");
            }
            catch (Exception)
            {
                return (int)Screen.PrimaryScreen.Bounds.Height;
            }
        }
        public static H2DisplayMode GetDisplayMode()
        {
            try
            {
                return (((int)GetVideoSetting("DisplayMode") == 1) ? H2DisplayMode.Windowed : H2DisplayMode.Fullscreen);
            }
            catch (Exception)
            {
                return H2DisplayMode.Windowed;
            }
        }
        private static void SetVideoSetting(string Setting, object Value)
        {
            Registry.SetValue(Paths.H2RegistryBase + "Video Settings", Setting, Value);
        }
        private static object GetVideoSetting(string Setting)
        {
            return Registry.GetValue(Paths.H2RegistryBase + "Video Settings", Setting, null);
        }
    }
}
