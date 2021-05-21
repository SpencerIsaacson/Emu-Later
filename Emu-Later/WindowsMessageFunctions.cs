using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace EmuLater
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeMessage
    {
        public IntPtr Handle;
        public uint Message;
        public IntPtr WParameter;
        public IntPtr LParameter;
        public uint Time;
        public Point Location;
    }

    internal class WindowsMessageFunctions
    {
        [DllImport("user32.dll")]
        private static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

        public static bool ApplicationIsIdle
        {
            get
            {
                return PeekMessage(out _, IntPtr.Zero, 0, 0, 0) == 0;
            }
        }
    }
}
