using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Media;

namespace Halo_2_Launcher.Controllers
{
    public class ProcessMemory
    {
        #region Flags
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        #endregion
        #region APIs



        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        //public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, Uint nSize, out int lpNumberOfBytesWritten);
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(int hObject);
        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(int hProcess, int lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        // Find window by Caption only. Note you must pass 0 as the first parameter.
        public static extern int FindWindowByCaption(int ZeroOnly, string lpWindowName);
        //public static extern short GetKeyState(VirtualKeyStates nVirtKey);
        #endregion
        #region Const

        const uint PAGE_NOACCESS = 1;
        const uint PAGE_READONLY = 2;
        const uint PAGE_READWRITE = 4;
        const uint PAGE_WRITECOPY = 8;
        const uint PAGE_EXECUTE = 16;
        const uint PAGE_EXECUTE_READ = 32;
        const uint PAGE_EXECUTE_READWRITE = 64;
        const uint PAGE_EXECUTE_WRITECOPY = 128;
        const uint PAGE_GUARD = 256;
        const uint PAGE_NOCACHE = 512;
        const uint PROCESS_ALL_ACCESS = 0x1F0FFF;

        #endregion
        protected string ProcessName;
        protected Process[] MyProcess;
        protected int[] processHandle;
        protected int[] processID;
        protected static int[] BaseAddress;
        protected static int SvrCt;
        public bool Check;
        List<int> procstore;
        public ProcessMemory(string pProcessName)
        {
            ProcessName = pProcessName;
            if (this.StartProcess())
            {
                if (this.ProcessCount != 0)
                    Check = true;
            }
            else
            {
                MessageBox.Show("Halo 2 Vista was not detected please close your game and try again.", "Error:117_2Ca-.212", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Close();
                Check = false;
            }
        }
        public string MyProcessName()
        {
            return ProcessName;
        }
        public string GetDirectory()
        {
            return MyProcess[0].MainModule.FileName;
        }
        public bool StartProcess()
        {
            if (ProcessName != "")
            {
                Process[] iprocess = Process.GetProcessesByName(ProcessName);
                if (iprocess.Length == 0)
                {
                    SvrCt = 0;
                    return (false);
                }
                procstore = new List<int>();
                for (int i = 0; i < iprocess.Length; i++)
                    procstore.Add(iprocess[i].Id);
                procstore.Sort();
                MyProcess = new Process[iprocess.Length];
                processID = new int[iprocess.Length];
                for (int i = 0; i < iprocess.Length; i++)
                {
                    MyProcess[i] = Process.GetProcessById(procstore[i]);
                    processID[i] = procstore[i];
                }
                iprocess = null;
                SvrCt = MyProcess.Length;
                BaseAddress = new int[SvrCt];
                processHandle = new int[SvrCt];
                for (int i = 0; i < SvrCt; i++)
                {
                    try
                    {
                        processHandle[i] = OpenProcess(PROCESS_ALL_ACCESS, false, MyProcess[i].Id);
                        BaseAddress[i] = (int)MyProcess[i].MainModule.BaseAddress;
                        if (processHandle[i] == 0)
                            return (false);
                    }
                    catch
                    {
                        return (false);
                    }
                }
                return (true);
            }
            else
            {
                return (false);
            }
        } //Checks For Running Process
        public bool CheckProcess()
        {
            Process[] p = Process.GetProcessesByName(ProcessName);
            if (p.Length > 0)
                return true;
            return false;
        }
        public int BaseAddy(int handle)
        {
            return BaseAddress[handle];
        }
        public int ProcessCount { get { return SvrCt; } }
        public int ProcessId(int ID)
        {
            return procstore[ID];
        }

        public string CutString(string mystring)
        {
            try
            {
                char[] mychar = mystring.ToCharArray();
                string returnstring = "";
                for (int i = 0; i < mystring.Length; i++)
                    if (mychar[i] == '\0') return returnstring;
                    else if (mychar.Length == i) return returnstring;
                    else returnstring += mychar[i].ToString();
                return returnstring;
            }
            catch
            {
                return mystring.TrimEnd('0');
            }
        }
        public int DllImageAddress(int handle, string dllname)
        {
            ProcessModuleCollection myProcessModuleCollection = Process.GetProcessById(processID[handle]).Modules;
            for (int i = 0; i < myProcessModuleCollection.Count; i++)
                if (myProcessModuleCollection[i].FileName.EndsWith(dllname))
                    return (int)myProcessModuleCollection[i].BaseAddress;
            return -1;
        }
        public int ImageAddress(int handle, int pOffset)
        {
            try
            {
                return BaseAddress[handle] + pOffset;
            }
            catch
            {
                return 0;
            }
        } //Calculates Image Address + Custom Offset
        #region readMem
        public byte[] ReadMem(int handle, int pOffset, int pSize)
        {
            byte[] buffer = new byte[pSize];
            ReadProcessMemory(processHandle[handle], pOffset, buffer, pSize, 0);
            return buffer;
        } //Read Memory With Custom Offset and Size
        public byte[] ReadMem(int handle, bool AddToImageAddress, int pOffset, int pSize)
        {
            byte[] buffer = new byte[pSize];
            int Address = AddToImageAddress ? ImageAddress(handle, pOffset) : pOffset;
            ReadProcessMemory(processHandle[handle], Address, buffer, pSize, 0);
            return buffer;
        } //Read Memory With Custom Offset And Size And Adds Offset To Image Address If True
        public byte ReadByte(int handle, int pOffset)
        {
            byte[] buffer = new byte[1];
            ReadProcessMemory(processHandle[handle], pOffset, buffer, 1, 0);
            return buffer[0];
        } //Read Memory With 1 Byte From Offset -- For Quick Reading
        public byte ReadByte(int handle, bool AddToImageAddress, int pOffset)
        {
            byte[] buffer = new byte[1];
            int Address = AddToImageAddress ? ImageAddress(handle, pOffset) : pOffset;
            ReadProcessMemory(processHandle[handle], Address, buffer, 1, 0);
            return buffer[0];
        } //Read Memory With 1 Byte From Offset -- For Quick Reading
        public byte ReadByte(int handle, string Module, int pOffset)
        {
            byte[] buffer = new byte[1];
            ReadProcessMemory(processHandle[handle], DllImageAddress(handle, Module) + pOffset, buffer, 1, 0);
            return buffer[0];
        } //Read Memory With 1 Byte From Offset -- For Quick Reading
        public int ReadInt(int handle, int pOffset)
        {
            return BitConverter.ToInt32(ReadMem(handle, pOffset, 4), 0);
        } //Read Memory Integer Offset -- For Quick Reading
        public int ReadInt(int handle, bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToInt32(ReadMem(handle, AddToImageAddress, pOffset, 4), 0);
        } //Read Memory Integer Offset -- For Quick Reading
        public int ReadInt(int handle, string Module, int pOffset)
        {
            return BitConverter.ToInt32(ReadMem(handle, DllImageAddress(handle, Module) + pOffset, 4), 0);
        } //Read Memory Integer Offset -- For Quick Reading
        public long ReadLong(int handle, int pOffset)
        {
            return BitConverter.ToInt64(ReadMem(handle, pOffset, 8), 0);
        } //Read Memory Integer Offset -- For Quick Reading
        public long ReadLong(int handle, bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToInt64(ReadMem(handle, AddToImageAddress, pOffset, 8), 0);
        } //Read Memory Integer Offset -- For Quick Reading
        public long ReadLong(int handle, string Module, int pOffset)
        {
            return BitConverter.ToInt64(ReadMem(handle, DllImageAddress(handle, Module) + pOffset, 8), 0);
        } //Read Memory Integer Offset -- For Quick Reading
        public short ReadShort(int handle, int pOffset)
        {
            return BitConverter.ToInt16(ReadMem(handle, pOffset, 2), 0);
        } //Read Memory Integer Offset -- For Quick Reading
        public short ReadShort(int handle, bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToInt16(ReadMem(handle, AddToImageAddress, pOffset, 2), 0);
        } //Read Memory Integer Offset -- For Quick Reading
        public short ReadShort(int handle, string Module, int pOffset)
        {
            return BitConverter.ToInt16(ReadMem(handle, DllImageAddress(handle, Module) + pOffset, 2), 0);
        } //Read Memory Integer Offset -- For Quick Reading
        public float ReadFloat(int handle, int pOffset)
        {
            return BitConverter.ToSingle(ReadMem(handle, pOffset, 4), 0);
        } //Read Memory Floating Point Offset -- For Quick Reading
        public float ReadFloat(int handle, bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToSingle(ReadMem(handle, AddToImageAddress, pOffset, 4), 0);
        } //Read Memory Floating Point Offset -- For Quick Reading
        public float ReadFloat(int handle, string Module, int pOffset)
        {
            return BitConverter.ToSingle(ReadMem(handle, DllImageAddress(handle, Module) + pOffset, 4), 0);
        } //Read Memory Floating Point Offset -- For Quick Reading
        public string ReadStringAscii(int handle, int pOffset, int pSize)
        {
            return CutString(Encoding.ASCII.GetString(ReadMem(handle, pOffset, pSize)));
        } //Read Memory Floating Point Offset -- For Quick Reading
        public string ReadStringAscii(int handle, bool AddToImageAddress, int pOffset, int pSize)
        {
            return CutString(Encoding.ASCII.GetString(ReadMem(handle, AddToImageAddress, pOffset, pSize)));
        } //Read Memory Floating Point Offset -- For Quick Reading
        public string ReadStringAscii(int handle, string Module, int pOffset, int pSize)
        {
            return CutString(Encoding.ASCII.GetString(ReadMem(handle, DllImageAddress(handle, Module) + pOffset, pSize)));
        } //Read Memory Floating Point Offset -- For Quick Reading
        public string ReadStringUnicode(int handle, int pOffset, int pSize)
        {
            return CutString(Encoding.Unicode.GetString(ReadMem(handle, pOffset, pSize)));
        } //Read Memory Floating Point Offset -- For Quick Reading
        public string ReadStringUnicode(int handle, bool AddToImageAddress, int pOffset, int pSize)
        {
            return CutString(Encoding.Unicode.GetString(ReadMem(handle, AddToImageAddress, pOffset, pSize)));
        } //Read Memory Floating Point Offset -- For Quick Reading
        public string ReadStringUnicode(int handle, string Module, int pOffset, int pSize)
        {
            return CutString(Encoding.Unicode.GetString(ReadMem(handle, DllImageAddress(handle, Module) + pOffset, pSize)));
        } //Read Memory Floating Point Offset -- For Quick Reading
        #endregion
        #region WriteMem
        public void WriteMem(int handle, int pOffset, byte[] pBytes)
        {
            WriteProcessMemory(processHandle[handle], pOffset, pBytes, pBytes.Length, 0);
        } //Write Memory With Custom Offset and Size
        public void WriteMem(int handle, bool AddToImageAddress, int pOffset, byte[] pBytes)
        {
            int Address = AddToImageAddress ? ImageAddress(handle, pOffset) : pOffset;
            WriteProcessMemory(processHandle[handle], Address, pBytes, pBytes.Length, 0);
        }//Read Memory With Custom Offset And Size And Adds Offset To Image Address If True
        public void WriteByte(int handle, int pOffset, byte pBytes)
        {
            WriteMem(handle, pOffset, new byte[] { pBytes });
        } //Write Memory Byte Offset -- For Quick Reading
        public void WriteByte(int handle, bool AddToImageAddress, int pOffset, byte pBytes)
        {
            WriteMem(handle, AddToImageAddress, pOffset, new byte[] { pBytes });
        } //Write Memory Byte Offset -- For Quick Reading
        public void WriteByte(int handle, string Module, int pOffset, byte pBytes)
        {
            WriteMem(handle, DllImageAddress(handle, Module) + pOffset, new byte[] { pBytes });
        } //Write Memory Byte Offset -- For Quick Reading
        public void WriteInt(int handle, int pOffset, int pBytes)
        {
            WriteMem(handle, pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Integer Offset -- For Quick Reading
        public void WriteInt(int handle, bool AddToImageAddress, int pOffset, int pBytes)
        {
            WriteMem(handle, AddToImageAddress, pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Integer Offset -- For Quick Reading
        public void WriteInt(int handle, string Module, int pOffset, int pBytes)
        {
            WriteMem(handle, DllImageAddress(handle, Module) + pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Integer Offset -- For Quick Reading
        public void WriteUInt(int handle, int pOffset, uint pBytes)
        {
            WriteMem(handle, pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Integer Offset -- For Quick Reading
        public void WriteUInt(int handle, bool AddToImageAddress, int pOffset, uint pBytes)
        {
            WriteMem(handle, AddToImageAddress, pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Integer Offset -- For Quick Reading
        public void WriteUInt(int handle, string Module, int pOffset, uint pBytes)
        {
            WriteMem(handle, DllImageAddress(handle, Module) + pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Integer Offset -- For Quick Reading
        public void WriteFloat(int handle, int pOffset, float pBytes)
        {
            WriteMem(handle, pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Floating Point Offset -- For Quick Reading
        public void WriteFloat(int handle, bool AddToImageAddress, int pOffset, float pBytes)
        {
            WriteMem(handle, AddToImageAddress, pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Floating Point Offset -- For Quick Reading
        public void WriteFloat(int handle, string Module, int pOffset, float pBytes)
        {
            WriteMem(handle, DllImageAddress(handle, Module) + pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Floating Point Offset -- For Quick Reading
        public void WriteShort(int handle, int pOffset, short pBytes)
        {
            WriteMem(handle, pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Integer Offset -- For Quick Reading
        public void WriteShort(int handle, bool AddToImageAddress, int pOffset, short pBytes)
        {
            WriteMem(handle, AddToImageAddress, pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Integer Offset -- For Quick Reading
        public void WriteShort(int handle, string Module, int pOffset, short pBytes)
        {
            WriteMem(handle, DllImageAddress(handle, Module) + pOffset, BitConverter.GetBytes(pBytes));
        } //Write Memory Integer Offset -- For Quick Reading
        public void WriteStringUnicode(int handle, int pOffset, string pBytes)
        {
            WriteMem(handle, pOffset, Encoding.Unicode.GetBytes(pBytes + "\0"));
        } //Write Memory Floating Point Offset -- For Quick Reading
        public void WriteStringUnicode(int handle, bool AddToImageAddress, int pOffset, string pBytes)
        {
            WriteMem(handle, AddToImageAddress, pOffset, Encoding.Unicode.GetBytes(pBytes + "\0"));
        } //Write Memory Floating Point Offset -- For Quick Reading
        public void WriteStringUnicode(int handle, string Module, int pOffset, string pBytes)
        {
            WriteMem(handle, DllImageAddress(handle, Module) + pOffset, Encoding.Unicode.GetBytes(pBytes + "\0"));
        } //Write Memory Floating Point Offset -- For Quick Reading
        public void WriteStringAscii(int handle, int pOffset, string pBytes)
        {
            WriteMem(handle, pOffset, Encoding.ASCII.GetBytes(pBytes + "\0"));
        } //Write Memory Floating Point Offset -- For Quick Reading
        public void WriteStringAscii(int handle, bool AddToImageAddress, int pOffset, string pBytes)
        {
            WriteMem(handle, AddToImageAddress, pOffset, Encoding.ASCII.GetBytes(pBytes + "\0"));
        } //Write Memory Floating Point Offset -- For Quick Reading
        public void WriteStringAscii(int handle, string Module, int pOffset, string pBytes)
        {
            WriteMem(handle, DllImageAddress(handle, Module) + pOffset, Encoding.ASCII.GetBytes(pBytes + "\0"));
        } //Write Memory Floating Point Offset -- For Quick Reading
        #endregion
        #region GetPointers
        //mainmodule pointers
        public int Pointer(int handle, bool AddToImageAddress, int pOffset)
        {
            return ReadInt(handle, BaseAddy(handle) + pOffset);
        }
        public int Pointer(int handle, bool AddToImageAddress, int pOffset, int pOffset2)
        {
            return ReadInt(handle, BaseAddy(handle) + pOffset) + pOffset2;
        }
        public int Pointer(int handle, bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3)
        {
            return ReadInt(handle, ReadInt(handle, BaseAddy(handle) + pOffset) + pOffset2) + pOffset3;
        }
        public int Pointer(int handle, bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3, int pOffset4)
        {
            return ReadInt(handle, ReadInt(handle, ReadInt(handle, BaseAddy(handle) + pOffset) + pOffset2) + pOffset3) + pOffset4;
        }
        public int Pointer(int handle, bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5)
        {
            return ReadInt(handle, ReadInt(handle, ReadInt(handle, ReadInt(handle, BaseAddy(handle) + pOffset) + pOffset2) + pOffset3) + pOffset4) + pOffset5;
        }
        public int Pointer(int handle, bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5, int pOffset6)
        {
            return ReadInt(handle, ReadInt(handle, ReadInt(handle, ReadInt(handle, ReadInt(handle, BaseAddy(handle) + pOffset) + pOffset2) + pOffset3) + pOffset4) + pOffset5) + pOffset6;
        }
        //dllmodule pointers
        public int Pointer(int handle, string Module, int pOffset)
        {
            return ReadInt(handle, DllImageAddress(handle, Module) + pOffset);
        }
        public int Pointer(int handle, string Module, int pOffset, int pOffset2)
        {
            return ReadInt(handle, DllImageAddress(handle, Module) + pOffset) + pOffset2;
        }
        public int Pointer(int handle, string Module, int pOffset, int pOffset2, int pOffset3)
        {
            return ReadInt(handle, ReadInt(handle, DllImageAddress(handle, Module) + pOffset) + pOffset2) + pOffset3;
        }
        public int Pointer(int handle, string Module, int pOffset, int pOffset2, int pOffset3, int pOffset4)
        {
            return ReadInt(handle, ReadInt(handle, ReadInt(handle, DllImageAddress(handle, Module) + pOffset) + pOffset2) + pOffset3) + pOffset4;
        }
        public int Pointer(int handle, string Module, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5)
        {
            return ReadInt(handle, ReadInt(handle, ReadInt(handle, ReadInt(handle, DllImageAddress(handle, Module) + pOffset) + pOffset2) + pOffset3) + pOffset4) + pOffset5;
        }
        public int Pointer(int handle, string Module, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5, int pOffset6)
        {
            return ReadInt(handle, ReadInt(handle, ReadInt(handle, ReadInt(handle, ReadInt(handle, DllImageAddress(handle, Module) + pOffset) + pOffset2) + pOffset3) + pOffset4) + pOffset5) + pOffset6;
        }
        #endregion
    }
}
