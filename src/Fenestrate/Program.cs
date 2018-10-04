using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Fenestrate
{
    /// <summary>
    /// Entry point for the program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main()
        {
            var screens = Screen.AllScreens.ToDictionary(s => s.DeviceName);
            var visibleWindows = LocateVisibleWindows(screens);

            foreach (var window in visibleWindows.Where(w => w.NeedsReposition))
            {
                Console.WriteLine("Repositioning '{0}'", window.Title);
                window.MoveWindowToMonitorBounds(screens);
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        private static IEnumerable<ApplicationWindow> LocateVisibleWindows(Dictionary<string, Screen> screens)
        {
            var windows = new List<ApplicationWindow>();
            WindowVisitor.VisitVisibleWindows(hWnd =>
            {
                var window = new ApplicationWindow(hWnd);
                window.RefreshProperties(screens);
                windows.Add(window);
            });

            return windows.Where(w => w.Visible);
        }
    }
}
