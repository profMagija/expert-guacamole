using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiSh
{
    static class Program
    {
        private const int HotKeyPriorDesktop = 0;
        private const int HotKeyNextDesktop = 1;
        private const int HotKeyShowAllDesktop = 2;
        private const int HotKeyRun = 3;

        private static VirtualDesktopManager _vdm = new VirtualDesktopManager();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //foreach (var process in Process.GetProcessesByName("explorer"))
            //{
            //    process.Kill();
            //    process.WaitForExit();
            //}

            var hotkeys = new[]
            {
                new HotKeyRegister(IntPtr.Zero, HotKeyPriorDesktop, HotKeyModifiers.Control | HotKeyModifiers.WindowsKey, Keys.Left),
                new HotKeyRegister(IntPtr.Zero, HotKeyNextDesktop, HotKeyModifiers.Control | HotKeyModifiers.WindowsKey, Keys.Right)
            };

            foreach (var hotkey in hotkeys)
            {
                hotkey.HotKeyPressed += HotkeyOnHotKeyPressed;
            }

            _vdm.Init();

            //Process.Start("explorer.exe");

            Win32.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr monitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr data) =>
            {
                var mi = new MONITORINFO();
                mi.Init();

                Win32.GetMonitorInfoA(monitor, ref mi);

                if (!Win32.SystemParametersInfo(0x2f, 0, ref mi.Monitor, 0)) // 0x27 = SPI_SETWORKAREA
                {
                    var le = Marshal.GetLastWin32Error();
                    Console.WriteLine("COuld not set thing, {0}", le);
                }

                return true;
            }, IntPtr.Zero);



            Win32.EnumWindows((hwnd, param) =>
            {
                var sb = new StringBuilder(50);
                Win32.GetClassNameA(hwnd, sb, 50);
                if (sb.ToString() == "Shell_TrayWnd" || sb.ToString() == "Shell_SecondaryTrayWnd")
                {
                    Win32.SetWindowPos(hwnd, new IntPtr(1), 0, 0, 0, 0, new IntPtr(0x10 | 0x400 | 0x80)); // SWP_NOACTIVATE | SWP_NOSENDCHANGING | SWP_HIDEWINDOW
                }
                return true;
            }, 0);

            Application.Run();
        }

        private static void HotkeyOnHotKeyPressed(object sender, EventArgs e)
        {
            var s = (HotKeyRegister) sender;

            switch (s.ID)
            {
                case HotKeyPriorDesktop:
                    _vdm.MoveDesktop(-1);
                    break;
                case HotKeyNextDesktop:
                    _vdm.MoveDesktop(1);
                    break;
            }
        }
    }
}
