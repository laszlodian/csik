using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace e77.MeasureBase.Beep
{
    public static class BeeperIO
    {
        [Serializable]
        public enum EBeepType { None, Speaker, System };



        private static bool Is64Bit = Environment.Is64BitProcess;

        [DllImport("inpout32.dll", EntryPoint = "DlPortWritePortUchar")]
        private static extern void DlPortWritePortUchar_32(short PortAddress, byte Data);
        [DllImport("inpout64.dll", EntryPoint = "DlPortWritePortUchar")]
        private static extern void DlPortWritePortUchar_64(short PortAddress, byte Data);

        [DllImport("inpout32.dll", EntryPoint = "DlPortReadPortUchar")]
        private static extern byte DlPortReadPortUchar_32(short PortAddress);
        [DllImport("inpout64.dll", EntryPoint = "DlPortReadPortUchar")]
        private static extern byte DlPortReadPortUchar_64(short PortAddress);

        [DllImport("inpout32.dll", EntryPoint = "IsInpOutDriverOpen")]
        private static extern UInt32 IsInpOutDriverOpen_32();
        [DllImport("inpout64.dll", EntryPoint = "IsInpOutDriverOpen")]
        private static extern UInt32 IsInpOutDriverOpen_64();

        static public bool IsDriverOK()
        {
            if (Is64Bit)
                return IsInpOutDriverOpen_64() != 0;
            else
                return IsInpOutDriverOpen_32() != 0;
        }

        static public void Out(int address, byte data)
        {

            if (Is64Bit)
                DlPortWritePortUchar_64((short)address, data);
            else
                DlPortWritePortUchar_32((short)address, data);

        }

        static public byte In(int address)
        {

            if (Is64Bit)
                return DlPortReadPortUchar_64((short)address);
            else
                return DlPortReadPortUchar_32((short)address);
        }
    }
}
