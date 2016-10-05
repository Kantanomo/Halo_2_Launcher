using Halo_2_Launcher.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halo_2_Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Process.GetProcessesByName("Halo_2_Launcher").Length > 1)
            {
                foreach (Process P in Process.GetProcessesByName("Halo_2_Launcher"))
                    if (P.Handle != Process.GetCurrentProcess().Handle)
                        P.Kill();
            }
            Application.Run(new Halo_2_Launcher.Forms.Update());
        }
    }
}
