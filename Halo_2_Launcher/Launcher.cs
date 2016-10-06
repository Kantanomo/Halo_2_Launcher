using System.Threading.Tasks;
using Halo_2_Launcher.Objects;
using Halo_2_Launcher.Controllers;
using System.Diagnostics;
using MetroFramework;
using System.Windows.Forms;

namespace Halo_2_Launcher
{
    public static class H2Launcher
    {

        private static Halo_2_Launcher.Controllers.Settings _LauncherSettings;
        private static Game _Game;
        private static PostLaunch _Post;
        private static ProcessMemory _Memory;
        private static WebHandler _WebControl;
        private static XliveSettings _XliveSettings;
        private static HotkeyController _HotKeyController;
        public static Halo_2_Launcher.Controllers.Settings LauncherSettings
        { get { if (H2Launcher._LauncherSettings == null) { H2Launcher._LauncherSettings = new Halo_2_Launcher.Controllers.Settings(); } return H2Launcher._LauncherSettings; } }
        public static XliveSettings XliveSettings
        { get { if (H2Launcher._XliveSettings == null) { H2Launcher._XliveSettings = new XliveSettings(); } return H2Launcher._XliveSettings; } }
        public static Game H2Game
        { get { if (_Game == null) { _Game = new Game(); } return _Game; } }
        public static PostLaunch Post
        { get { if (H2Launcher._Post == null) { H2Launcher._Post = new PostLaunch(); } return H2Launcher._Post; } }
        public static ProcessMemory Memory
        { get { if (H2Launcher._Memory == null) { H2Launcher._Memory = new ProcessMemory("halo2"); } return H2Launcher._Memory; } }
        public static WebHandler WebControl
        { get { if (H2Launcher._WebControl == null) { H2Launcher._WebControl = new WebHandler(); } return H2Launcher._WebControl; } }
        public static HotkeyController HotkeyController
        {
            get { if(H2Launcher._HotKeyController == null) { H2Launcher._HotKeyController = new HotkeyController(); } return H2Launcher._HotKeyController; }
            set { H2Launcher._HotKeyController = value; }
        }
        public static int MapPointer(int Offset)
        {
            //This function needs a home in the future.
            return Memory.Pointer(0, true, 0x47CD54, Offset);
        }
        public static async void StartHalo(string Gamertag, string LoginToken, Halo_2_Launcher.Forms.MainForm Form)
        {
            Form.Hide();
            HotkeyController = new HotkeyController();
            HotkeyController.Initialize();
            XliveSettings.ProfileName1 = Gamertag;
            XliveSettings.loginToken = LoginToken;
            XliveSettings.SaveSettings();
           
            LauncherSettings.SaveSettings();
            await Task.Delay(1);
            //File.WriteAllLines(Paths.InstallPath + "token.ini", new string[] { "token=" + LoginToken, "username=" + Gamertag });
            H2Game.RunGame();
            int RunningTicks = 0;
            /*
             * Game Running thread ticks every 1 second with a maximum of 15 ticks till reset.
             * 
             * */
            while (Process.GetProcessesByName("halo2").Length == 1)//DURING HALO RUNNING THREAD
            {
                if (RunningTicks == 15) //Check Ban Status every 15 ticks
                {
                    var banResult = WebControl.CheckBan(Gamertag, LoginToken);

                    if(banResult == CheckBanResult.Banned)
                    {
                        XliveSettings.loginToken = "";
                        Form.BringToFront();
                        if (MetroMessageBox.Show(Form, "You have been banned, please visit the forum to appeal your ban.\r\nWould you like us to open the forums for you?.", Fun.PauseIdiomGenerator, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(@"http://www.halo2vista.com/forums/");
                        }
                    }
                }

                #region GameStateChecks
                if (RunningTicks == 5) //GameState Check every 5 ticks
                {
                    switch (H2Game.GameState)
                    {
                        case H2GameState.ingame:
                            {
                                H2Game.SetCrossHairPosition();
                                break;
                            }
                    }
                }
                #endregion

                HotkeyController.ExecuteHotKeys();

                #region TickLogic
                if (RunningTicks == 15)
                    RunningTicks = 0;
                else
                    RunningTicks++;
                await Task.Delay(1000);
                #endregion
            }
            Form.Show();
        }
    }
}
