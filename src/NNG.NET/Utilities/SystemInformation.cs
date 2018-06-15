using System;

namespace NNGNET.Utilities
{
    internal static class SystemInformation
    {
        /// <summary>
        ///     Determines whether this platforms architecture is x64.
        /// </summary>
        /// <remarks>
        ///     This is necessary due to an error in .NET Framework v4.7 where 
        ///     <see cref="System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture"/> 
        ///     may return the wrong value i.e. x86 on x64 processes. <br />
        ///     see also: https://github.com/dotnet/corefx/issues/25267
        /// </remarks>
        /// <returns>
        ///     <c>true</c> if this platforms architecture is x64; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsX64() => IntPtr.Size == sizeof(ulong);
    }
}
