using System;
using System.Linq;

namespace Fenestrate.Win32
{
    /// <summary>
    /// Flags used to retrieve a monitor handle from a window.
    /// </summary>
    [Flags]
    internal enum MonitorFromWindowFlags
       : uint
    {
        /// <summary>
        /// Returns <see langword="null" />.
        /// </summary>
        DefaultToNull = 0,

        /// <summary>
        /// Returns a handle to the primary display monitor.
        /// </summary>
        DefaultToPrimary = 1,

        /// <summary>
        /// Returns a handle to the display monitor that is nearest to the window.
        /// </summary>
        DefaultToNearest = 2,
    }
}
