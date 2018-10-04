using System;
using System.Collections.Generic;
using Fenestrate.Win32;

namespace Fenestrate
{
    /// <summary>
    /// Executes an action on visible windows.
    /// </summary>
    public static class WindowVisitor
    {
        /// <summary>
        /// Visits the visible windows with a provided action.
        /// </summary>
        /// <param name="handleVisitor">The window handle visitor.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="handleVisitor" /> is <see langword="null" />.
        /// </exception>
        public static void VisitVisibleWindows(Action<IntPtr> handleVisitor)
        {
            if (handleVisitor == null)
            {
                throw new ArgumentNullException(nameof(handleVisitor));
            }

            var windows = new List<IntPtr>();
            NativeMethods.EnumWindows(CreateVisibleWindowEnumerator(windows), 0);
            foreach (var window in windows)
            {
                handleVisitor(window);
            }
        }

        private static NativeMethods.EnumWindowsProc CreateVisibleWindowEnumerator(List<IntPtr> windows)
        {
            var shellWindow = NativeMethods.GetShellWindow();
            return (IntPtr hWnd, int lParam) =>
            {
                // Ignore the main shell window (the desktop) and
                // ignore any windows that aren't visible.
                if (hWnd == shellWindow || !NativeMethods.IsWindowVisible(hWnd))
                {
                    return true;
                }

                windows.Add(hWnd);
                return true;
            };
        }
    }
}
