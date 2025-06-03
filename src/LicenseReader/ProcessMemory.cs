using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;
using System.Media;
using System.Text;
using System.Threading;
namespace ReadWriteMemory
{
    internal class ProcessMemory
    {
        // Fields
        protected int BaseAddress;
        protected Process[] MyProcess;
        protected ProcessModule myProcessModule;
        private const uint PAGE_EXECUTE = 16;
        private const uint PAGE_EXECUTE_READ = 32;
        private const uint PAGE_EXECUTE_READWRITE = 64;
        private const uint PAGE_EXECUTE_WRITECOPY = 128;
        private const uint PAGE_GUARD = 256;
        private const uint PAGE_NOACCESS = 1;
        private const uint PAGE_NOCACHE = 512;
        private const uint PAGE_READONLY = 2;
        private const uint PAGE_READWRITE = 4;
        private const uint PAGE_WRITECOPY = 8;
        private const uint PROCESS_ALL_ACCESS = 2035711;
        protected IntPtr processHandle;
        protected string ProcessName;

        // Methods
        public ProcessMemory(string pProcessName)
        {
            this.ProcessName = pProcessName;
        }

        public bool CheckProcess()
        {
            return (Process.GetProcessesByName(this.ProcessName).Length > 0);
        }

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(int hObject);
        public string CutString(string mystring)
        {
            char[] chArray = mystring.ToCharArray();
            string str = "";
            for (int i = 0; i < mystring.Length; i++)
            {
                if ((chArray[i] == ' ') && (chArray[i + 1] == ' '))
                {
                    return str;
                }
                if (chArray[i] == '\0')
                {
                    return str;
                }
                str = str + chArray[i].ToString();
            }
            return mystring.TrimEnd(new char[] { '0' });
        }
        
      

            public IntPtr DllImageAddress(string dllname)
        {

           
            ProcessModuleCollection modules = this.MyProcess[0].Modules;

            foreach (ProcessModule procmodule in modules)
            {
             
                if (dllname == procmodule.ModuleName)
                {
                    return procmodule.BaseAddress;
                }
            }
            return (IntPtr.Zero);

        }
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern int FindWindowByCaption(int ZeroOnly, string lpWindowName);
        public int ImageAddress()
        {
            this.BaseAddress = 0;
            this.myProcessModule = this.MyProcess[0].MainModule;
            this.BaseAddress = (int)this.myProcessModule.BaseAddress;
            return this.BaseAddress;


        }

        public int ImageAddress(int pOffset)
        {
            this.BaseAddress = 0;
            this.myProcessModule = this.MyProcess[0].MainModule;
            this.BaseAddress = (int)this.myProcessModule.BaseAddress;
            return (pOffset + this.BaseAddress);
        }
        public string MyProcessName()
        {
            return this.ProcessName;
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        public byte[] ReadMem(IntPtr pOffset, int pSize)
        {
            byte[] buffer = new byte[pSize];
            ReadProcessMemory(this.processHandle, pOffset, buffer, pSize, 0);
            return buffer;
        }


        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesRead);
      
        public bool StartProcess()
        {
            if (this.ProcessName != "")
            {
                this.MyProcess = Process.GetProcessesByName(this.ProcessName);
                if (this.MyProcess.Length == 0)
                {
                    MessageBox.Show(this.ProcessName + " is not running or has not been found. Please check and try again", "Process Not Found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                this.processHandle = OpenProcess(2035711, false, this.MyProcess[0].Id);
                if (this.processHandle == IntPtr.Zero)
                {
                    MessageBox.Show(this.ProcessName + " is not running or has not been found. Please check and try again", "Process Not Found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                return true;
            }
            MessageBox.Show("Define process name first!");
            return false;
        }

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(int hProcess, int lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);
        public void WriteByte(IntPtr pOffset, byte pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes((short)pBytes));
        }

        public void WriteByte(string Module, int pOffset, byte pBytes)
        {
            this.WriteMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes((short)pBytes));
        }
        public bool WriteMem(IntPtr pOffset, byte[] pBytes)
        {
            return WriteProcessMemory(this.processHandle, pOffset, pBytes, pBytes.Length, 0);
        }

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);

        // Nested Types
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 2035711,
            CreateThread = 2,
            DupHandle = 64,
            QueryInformation = 1024,
            SetInformation = 512,
            Synchronize = 1048576,
            Terminate = 1,
            VMOperation = 8,
            VMRead = 16,
            VMWrite = 32
        }
    }
}