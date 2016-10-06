using System;
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
        private HotkeyObject noHudHotkey;
        public HotkeyController()
        {
            if (H2Launcher.LauncherSettings.xDelayHotkey != "")
                this.xDelayHotkey = new HotkeyObject(H2Launcher.LauncherSettings.xDelayHotkey);
            if (H2Launcher.LauncherSettings.noHUDHotkey != "")
                this.noHudHotkey = new HotkeyObject(H2Launcher.LauncherSettings.noHUDHotkey);
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
                                if (this.xDelayHotkey.Pressed())
                                    for (int i = 0; i < 10; i++)
                                        H2Launcher.Memory.WriteByte(0, true, 0x0050A3B8, 0);
                            break;
                        }
                    case H2GameState.ingame:
                        {
                            if (this.noHudHotkey != null)
                                if (this.noHudHotkey.Pressed())
                                {
                                    H2Launcher.Memory.WriteByte(0, true, 0x2228F8, (byte)((this.noHudHotkey.Triggered) ? 0x84 : 0x85));
                                    H2Launcher.Memory.WriteByte(0, true, 0x222311, (byte)((this.noHudHotkey.Triggered) ? 0x84 : 0x85));
                                    H2Launcher.Memory.WriteUInt(0, false, H2Launcher.MapPointer(0xD8C5BC), ((this.noHudHotkey.Triggered) ? 0xF0E3367D : 0xFFFFFFFF));
                                    this.noHudHotkey.Triggered = !this.noHudHotkey.Triggered;
                                }
                            break;
                        }
                }
            });
        }
    }
}