using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Fenestrate.Win32
{
    /// <summary>
    /// Native methods for executing Win32 commands.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Size of a device name string.
        /// </summary>
        public const int CCHDEVICENAME = 32;

        /// <summary>
        /// Delegate for enumerating windows.
        /// </summary>
        /// <param name="hWnd">Window handle to process.</param>
        /// <param name="lParam">An application-defined value to be passed to the callback function.</param>
        /// <returns><see langword="true" /> if the callback function succeeds; <see langword="false" /> if not.</returns>
        public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        /// <summary>
        /// Enums the windows.
        /// </summary>
        /// <param name="enumFunc">A pointer to an application-defined callback function.</param>
        /// <param name="lParam">An application-defined value to be passed to the callback function.</param>
        /// <returns><see langword="true" /> if the enumeration succeeds; <see langword="false" /> if not.</returns>
        [DllImport("USER32.DLL")]
        public static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        /// <summary>
        /// Gets the monitor information.
        /// </summary>
        /// <param name="hMonitor">A handle to the display monitor of interest.</param>
        /// <param name="lpmi">A pointer to a <see cref="MonitorInfoEx"/> structure that receives information about the specified display monitor.</param>
        /// <returns>
        /// <see langword="true" /> if the query succeeds; <see langword="false" /> if not.
        /// </returns>
        [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        /// <summary>
        /// Retrieves a handle to the Shell's desktop window.
        /// </summary>
        /// <returns>
        /// The handle of the Shell's desktop window. If no Shell process is present, the return value is <see langword="null" />.
        /// </returns>
        [DllImport("USER32.DLL")]
        public static extern IntPtr GetShellWindow();

        /// <summary>
        /// Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window.
        /// </param>
        /// <param name="lpwndpl">
        /// A pointer to the WINDOWPLACEMENT structure that receives the show state and position information.
        /// <para>
        /// Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fails if lpwndpl-> length is not set correctly.
        /// </para>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// <para>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </para>
        /// </returns>
        [DllImport("USER32.DLL", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
        /// </summary>
        /// <param name="hwnd">A handle to the window.</param>
        /// <param name="lpRect">The lp rect.</param>
        /// <returns>
        /// <see langword="true" /> if the query succeeds; <see langword="false" /> if not.
        /// </returns>
        [DllImport("USER32.DLL", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RectStruct lpRect);

        /// <summary>
        /// Copies the text of the specified window's title bar (if it has one) into a buffer. If the specified window is a control, the text of the control is copied.
        /// </summary>
        /// <param name="hWnd">A handle to the window or control containing the text.</param>
        /// <param name="lpString">The buffer that will receive the text. If the string is as long or longer than the buffer, the string is truncated and terminated with a null character.</param>
        /// <param name="nMaxCount">The maximum number of characters to copy to the buffer, including the null character. If the text exceeds this limit, it is truncated.</param>
        /// <returns>
        /// If the function succeeds, the return value is the length, in characters, of the copied string, not including the terminating null character. If the window has no title bar or text,
        /// if the title bar is empty, or if the window or control handle is invalid, the return value is zero.
        /// </returns>
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// Retrieves the length, in characters, of the specified window's title bar text (if the window has a title bar). If the specified window is a control, the function retrieves the length of the text within the control.
        /// </summary>
        /// <param name="hWnd">A handle to the window or control.</param>
        /// <returns>
        /// If the function succeeds, the return value is the length, in characters, of the text. Under certain conditions, this value may actually be greater than the length of the text. If the window has no text, the return value is zero.
        /// </returns>
        [DllImport("USER32.DLL")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>
        /// Determines the visibility state of the specified window.
        /// </summary>
        /// <param name="hWnd">A handle to the window to be tested.</param>
        /// <returns>
        /// <see langword="true" /> if the window is visible; <see langword="false" /> if not.
        /// </returns>
        [DllImport("USER32.DLL")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// Retrieves a handle to the display monitor that has the largest area of intersection with the bounding rectangle of a specified window.
        /// </summary>
        /// <param name="hwnd">A handle to the window of interest.</param>
        /// <param name="dwFlags">Determines the function's return value if the window does not intersect any display monitor.</param>
        /// <returns>
        /// If the window intersects one or more display monitor rectangles, the return value is a handle to the display monitor
        /// that has the largest area of intersection with the window. If the window does not intersect a display monitor, the
        /// return value depends on the value of <paramref name="dwFlags" />.
        /// </returns>
        [DllImport("USER32.DLL")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorFromWindowFlags dwFlags);

        /// <summary>
        /// Changes the position and dimensions of the specified window.
        /// </summary>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="X">Specifies the new position of the left side of the window.</param>
        /// <param name="Y">Specifies the new position of the top of the window.</param>
        /// <param name="nWidth">Specifies the new width of the window.</param>
        /// <param name="nHeight">Specifies the new height of the window.</param>
        /// <param name="bRepaint">
        /// Specifies whether the window is to be repainted.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the move succeeds; <see langword="false" /> if not.
        /// </returns>
        [DllImport("USER32.DLL", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
    }
}
