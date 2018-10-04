using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fenestrate.Win32;

namespace Fenestrate
{
    /// <summary>
    /// Data class with information about a specific window and associated monitor.
    /// </summary>
    public class ApplicationWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationWindow"/> class.
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="windowHandle" /> is <see langword="null" />.
        /// </exception>
        public ApplicationWindow(IntPtr windowHandle)
        {
            if (windowHandle == null)
            {
                throw new ArgumentNullException(nameof(windowHandle));
            }

            this.WindowHandle = windowHandle;
        }

        /// <summary>
        /// Gets the bottom Y coordinate of the window.
        /// </summary>
        public int Bottom { get; private set; }

        /// <summary>
        /// Gets the left X coordinate of the window.
        /// </summary>
        public int Left { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ApplicationWindow"/> is maximized.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if maximized; otherwise, <see langword="false" />.
        /// </value>
        public bool Maximized { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this window needs to be repositioned.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the window is not maximized and falls outside
        /// the screen bounds; <see langword="false" /> if it's good.
        /// </value>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no <see cref="ApplicationWindow.Screen"/> has been set.
        /// </exception>
        public bool NeedsReposition
        {
            get
            {
                if (this.Screen == null)
                {
                    throw new InvalidOperationException("Refresh the properties before checking reposition. No screen has been set.");
                }

                // Don't reposition maximized windows - sometimes maximized windows
                // report being slightly over the working area of the monitor because
                // of the window border.
                return !this.Maximized &&
                    (this.Top < this.Screen.WorkingArea.Top ||
                    this.Left < this.Screen.WorkingArea.Left ||
                    this.Right > this.Screen.WorkingArea.Right ||
                    this.Bottom > this.Screen.WorkingArea.Bottom);
            }
        }

        /// <summary>
        /// Gets the right X coordinate of the window.
        /// </summary>
        public int Right { get; private set; }

        /// <summary>
        /// Gets the associated monitor the window is on.
        /// </summary>
        public Screen Screen { get; private set; }

        /// <summary>
        /// Gets the window title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the top Y coordinate of the window.
        /// </summary>
        public int Top { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ApplicationWindow"/> is visible.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if visible; otherwise, <see langword="false" />.
        /// </value>
        public bool Visible
        {
            get
            {
                return this.Title != null && this.Screen != null;
            }
        }

        /// <summary>
        /// Gets a pointer to the window handle.
        /// </summary>
        public IntPtr WindowHandle { get; private set; }

        /// <summary>
        /// Moves the window to within the monitor bounds of the nearest monitor.
        /// </summary>
        /// <param name="screens">The available screens/monitors.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="screens" /> is <see langword="null" />.
        /// </exception>
        public void MoveWindowToMonitorBounds(Dictionary<string, Screen> screens)
        {
            if (screens == null)
            {
                throw new ArgumentNullException(nameof(screens));
            }

            // TODO: You can't move admin windows like Task Manager without admin permissions. Right now we sidestep it with a max attempt check.
            var moves = 0;
            var moved = false;

            do
            {
                moves++;
                moved = false;
                var windowRectangle = default(RectStruct);
                if (!NativeMethods.GetWindowRect(this.WindowHandle, out windowRectangle))
                {
                    throw new InvalidOperationException($"Unable to query position of window: {this.Title}");
                }

                var originalWindowHeight = windowRectangle.Bottom - windowRectangle.Top;
                var originalWindowWidth = windowRectangle.Right - windowRectangle.Left;

                if (windowRectangle.Top < this.Screen.WorkingArea.Top)
                {
                    moved = true;
                    windowRectangle.Top = this.Screen.WorkingArea.Top;
                    windowRectangle.Bottom = windowRectangle.Top + originalWindowHeight;
                    if (windowRectangle.Bottom > this.Screen.WorkingArea.Bottom)
                    {
                        windowRectangle.Bottom = this.Screen.WorkingArea.Bottom;
                    }
                }
                else if (windowRectangle.Bottom > this.Screen.WorkingArea.Bottom)
                {
                    moved = true;
                    windowRectangle.Bottom = this.Screen.WorkingArea.Bottom;
                    windowRectangle.Top = windowRectangle.Bottom - originalWindowHeight;
                    if (windowRectangle.Top < this.Screen.WorkingArea.Top)
                    {
                        windowRectangle.Top = this.Screen.WorkingArea.Top;
                    }
                }
                else if (windowRectangle.Left < this.Screen.WorkingArea.Left)
                {
                    moved = true;
                    windowRectangle.Left = this.Screen.WorkingArea.Left;
                    windowRectangle.Right = windowRectangle.Left + originalWindowWidth;
                    if (windowRectangle.Right > this.Screen.WorkingArea.Right)
                    {
                        windowRectangle.Right = this.Screen.WorkingArea.Right;
                    }
                }
                else if (windowRectangle.Right > this.Screen.WorkingArea.Right)
                {
                    moved = true;
                    windowRectangle.Right = this.Screen.WorkingArea.Right;
                    windowRectangle.Left = windowRectangle.Right - originalWindowWidth;
                    if (windowRectangle.Left < this.Screen.WorkingArea.Left)
                    {
                        windowRectangle.Left = this.Screen.WorkingArea.Left;
                    }
                }

                if (moved)
                {
                    NativeMethods.MoveWindow(this.WindowHandle, windowRectangle.Left, windowRectangle.Top, windowRectangle.Right - windowRectangle.Left, windowRectangle.Bottom - windowRectangle.Top, true);
                }
            }
            while (moved && moves < 4);

            this.RefreshProperties(screens);
        }

        /// <summary>
        /// Refreshes the properties for the window and maps it to its monitor info.
        /// </summary>
        /// <param name="screens">The available screens/monitors.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="screens" /> is <see langword="null" />.
        /// </exception>
        public void RefreshProperties(Dictionary<string, Screen> screens)
        {
            if (screens == null)
            {
                throw new ArgumentNullException(nameof(screens));
            }

            this.Title = null;
            var length = NativeMethods.GetWindowTextLength(this.WindowHandle);
            if (length != 0)
            {
                var builder = new StringBuilder(length);
                if (NativeMethods.GetWindowText(this.WindowHandle, builder, length + 1) > 0)
                {
                    this.Title = builder.ToString();
                }
            }

            var monitorHandle = NativeMethods.MonitorFromWindow(this.WindowHandle, MonitorFromWindowFlags.DefaultToNearest);
            if (monitorHandle != null)
            {
                var monitorInfo = MonitorInfoEx.Default;
                if (NativeMethods.GetMonitorInfo(monitorHandle, ref monitorInfo))
                {
                    this.Screen = screens[monitorInfo.DeviceName];
                }
            }

            var windowRectangle = default(RectStruct);
            if (NativeMethods.GetWindowRect(this.WindowHandle, out windowRectangle))
            {
                this.Top = windowRectangle.Top;
                this.Bottom = windowRectangle.Bottom;
                this.Left = windowRectangle.Left;
                this.Right = windowRectangle.Right;
            }

            var windowPlacement = WindowPlacement.Default;
            if (NativeMethods.GetWindowPlacement(this.WindowHandle, ref windowPlacement))
            {
                this.Maximized = windowPlacement.ShowCmd == ShowWindowCommand.ShowMaximized;
            }
        }
    }
}
