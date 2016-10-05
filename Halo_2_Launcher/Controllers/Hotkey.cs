﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halo_2_Launcher.Objects;
namespace Halo_2_Launcher.Controllers
{
   

    public class HotkeyController
    {
        private HotkeyObject xDelayHotkey;
        public HotkeyController()
        {
            
        }
        public void Initialize()
        {
            if (H2Launcher.LauncherSettings.xDelayHotkey != "")
                this.xDelayHotkey = new HotkeyObject(H2Launcher.LauncherSettings.xDelayHotkey);
        }
        public async void ExecuteHotKeys()
        {
            await Task.Run(() =>
            {
                switch (H2Launcher.H2Game.GameState)
                {
                    case H2GameState.lobby:
                        {
                            if (this.xDelayHotkey != null)
                                if(this.xDelayHotkey.Pressed())
                                for (int i = 0; i < 10; i++)
                                    H2Launcher.Memory.WriteByte(0, true, 0x0050A3B8, 0);
                            break;
                        }
                }
            });
        }
    }
}
