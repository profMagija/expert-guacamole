using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Windows.Forms;

namespace DiSh
{
    internal class VirtualDesktop
    {
        /// <summary>
        /// Windows that are visible on the desktop
        /// </summary>
        public HashSet<IntPtr> Windows { get; } = new HashSet<IntPtr>();

        /// <summary>
        /// Windows that are hidden on the desktop
        /// </summary>
        public HashSet<IntPtr> HiddenWindows { get; } = new HashSet<IntPtr>();

        public void Add(IntPtr hwnd)
        {
            if (Contains(hwnd))
                return;

            Win32.GetWindowThreadProcessId(hwnd, out var pid);
            var proc = Process.GetProcessById((int) pid);
            if (proc.ProcessName == "explorer")
            {
                return;
            }

            if (Win32.ShowWindow(hwnd, 0))
            {
                // it was visible
                Win32.ShowWindow(hwnd, 5);
                Windows.Add(hwnd);
            }
            else
            {
                HiddenWindows.Add(hwnd);
            }
        }

        public bool Contains(IntPtr hwnd)
        {
            return Windows.Contains(hwnd) || HiddenWindows.Contains(hwnd);
        }

        public void Remove(IntPtr hwnd)
        {
            if (Windows.Contains(hwnd))
                Windows.Remove(hwnd);
            if (HiddenWindows.Contains(hwnd))
                Windows.Remove(hwnd);
        }
    }

    internal class VirtualDesktopManager
    {
        private readonly Dictionary<IntPtr, List<VirtualDesktop>> _desktops = new Dictionary<IntPtr, List<VirtualDesktop>>();
        private readonly Dictionary<IntPtr, int> _curDesktop = new Dictionary<IntPtr, int>();
        private GCHandle _winEventHook, _shellHook;

        public void Init()
        {
            Win32.EnumWindows((hwnd,  param) =>
            {
                var (desktops, curDesktop) = GetDesktopsForWindow(hwnd);

                desktops[curDesktop].Add(hwnd);

                return true;
            }, 0);

            _winEventHook = GCHandle.Alloc((Win32.WinEventDelegate) WinMove);
            Win32.SetWinEventHook(Win32.EVENT_OBJECT_LOCATIONCHANGE,
                Win32.EVENT_OBJECT_LOCATIONCHANGE,
                IntPtr.Zero,
                (Win32.WinEventDelegate) _winEventHook.Target,
                0, 0,
                Win32.WINEVENT_OUTOFCONTEXT);

            _shellHook = GCHandle.Alloc((Win32.HookProc) ShellHook);

            Win32.SetWindowsHookEx(Win32.HookType.WH_SHELL, (Win32.HookProc) _shellHook.Target, Win32.GetModuleHandle(null), 0);

            Application.ApplicationExit += ApplicationOnApplicationExit;
        }

        private void ApplicationOnApplicationExit(object sender, EventArgs e)
        {
            foreach (var desktop in _desktops)
            {
                foreach (var virtualDesktop in desktop.Value)
                {
                    foreach (var hwnd in virtualDesktop.Windows)
                    {
                        Win32.ShowWindow(hwnd, 5);
                    }
                }
            }
        }

        private IntPtr ShellHook(int code, IntPtr wparam, IntPtr lparam)
        {
            if (code < 0)
                return Win32.CallNextHookEx(IntPtr.Zero, code, wparam, lparam);

            switch (code)
            {
                case 1: // HSHELL_WINDOWCREATED 
                    WindowCreated(wparam);
                    break;
                case 2: // HSHELL_WINDOWDESTROYED	
                    WindowDestroyed(wparam);
                    break;
                case 13: // HSHELL_WINDOWREPLACED
                    WindowDestroyed(wparam);
                    WindowCreated(lparam);
                    break;
            }

            return Win32.CallNextHookEx(IntPtr.Zero, code, wparam, lparam);
        }

        private  void WinMove(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            var (desktops, curDesktop) = GetDesktopsForWindow(hwnd);

            if (desktops == null)
                return;

            if (desktops[curDesktop].Contains(hwnd))
            {
                // still on current monitor
                return;
            }

            foreach (var l in _desktops.Values)
            {
                foreach (var ll in l)
                {
                    ll.Remove(hwnd);
                }
            }

            desktops[curDesktop].Add(hwnd);

        }

        public void MoveDesktop(int movement)
        {
            var curmon = Win32.MonitorFromWindow(Win32.GetForegroundWindow(), Win32.MONITOR_DEFAULTTONEAREST);

            var (desktops, curDesktop) = GetDesktops(curmon);
            var numDesktops = desktops.Count;

            var nextDesktop = curDesktop + movement;
            if (nextDesktop < 0) nextDesktop = 0;
            if (nextDesktop >= numDesktops) nextDesktop = numDesktops - 1;

            if (nextDesktop == curDesktop)
                return;

            foreach (var toHide in desktops[curDesktop].Windows)
            {
                Win32.ShowWindow(toHide, 0); // SW_HIDE
            }

            foreach(var toShow in desktops[nextDesktop].Windows)
            {
                Win32.ShowWindow(toShow, 5); // SW_SHOW
            }

            _curDesktop[curmon] = nextDesktop;
        }

        private (List<VirtualDesktop>, int) GetDesktopsForWindow(IntPtr hwnd)
        {
            return GetDesktops(Win32.MonitorFromWindow(hwnd, Win32.MONITOR_DEFAULTTONEAREST));
        }

        private (List<VirtualDesktop>, int) GetDesktops(IntPtr monitor)
        {
            if (monitor == IntPtr.Zero)
                return (null, 0);
            if (!_desktops.ContainsKey(monitor))
            {
                _desktops[monitor] = new List<VirtualDesktop> {new VirtualDesktop(), new VirtualDesktop()};
                _curDesktop[monitor] = 0;
            }

            return (_desktops[monitor], _curDesktop[monitor]);
        }

        private void WindowCreated(IntPtr hwnd)
        {
            var (desktops, curDesktop) = GetDesktopsForWindow(hwnd);

            desktops[curDesktop].Add(hwnd);
        }

        private void WindowDestroyed(IntPtr hwnd)
        {
            var (desktops, _) = GetDesktopsForWindow(hwnd);

            foreach (var windows in desktops)
            {
                if (windows.Contains(hwnd))
                    windows.Remove(hwnd);
            }
        }

        public void ShowAll()
        {
            foreach (var toShow in _desktops.SelectMany(x => x.Value).SelectMany(x => x.Windows))
            {
                Win32.ShowWindow(toShow, 5); // SW_SHOW
            }
        }
    }
}
