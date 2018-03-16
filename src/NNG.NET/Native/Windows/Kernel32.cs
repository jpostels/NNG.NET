using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NNG.Native.Windows
{
    internal static class Kernel32
    {
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);
    }
}
