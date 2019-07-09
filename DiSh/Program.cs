using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

            var taskbar = Win32.FindWindowA("Shell_TrayWnd", "");
            Win32.ShowWindow(taskbar, 0);

            var nWidth = Win32.GetSystemMetrics(SystemMetric.SM_CXSCREEN);
            int nHeight = Win32.GetSystemMetrics(SystemMetric.SM_CYSCREEN);

            var rcWorkArea = new RECT(0, 0, nWidth, nHeight);

            Win32.SystemParametersInfo(0x002f, 0, ref rcWorkArea, 0);

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
