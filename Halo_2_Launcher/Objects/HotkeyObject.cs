using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Halo_2_Launcher.Objects
{

    public class HotkeyObject
    {
        [DllImport("user32.dll")]
        public static extern short GetKeyState(System.Windows.Forms.Keys nVirtKey);
        private System.Windows.Forms.Keys[] Keys;
        public bool Triggered = false;
        public HotkeyObject(string Keys)
        {
            string[] SplitKeys = Keys.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            this.Keys = new System.Windows.Forms.Keys[SplitKeys.Length];
            for(int i = 0; i < SplitKeys.Length; i++)
            {
                string aKey = SplitKeys[i];
                int yeah = 0;
                if(int.TryParse(aKey,out yeah))
                    aKey = "D" + aKey;
                System.Windows.Forms.Keys Key = (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), aKey.Replace("CTRL", "ControlKey"), true);
                this.Keys[i] = Key;
            }
        }
        public bool Pressed()
        {
            int PressedCount = 0;
            foreach (System.Windows.Forms.Keys Key in this.Keys)
            {
                int state = GetKeyState(Key);
                if (state == -127 || state == -128)
                    PressedCount++;
            }
            return (PressedCount == Keys.Length);
        }
    }
}
