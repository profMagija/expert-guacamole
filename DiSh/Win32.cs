using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DiSh
{
    internal static class Win32
    {
        public const int MONITOR_DEFAULTTONULL = 0;
        public const int MONITOR_DEFAULTTOPRIMARY = 1;
        public const int MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(WndEnumProc lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(
            IntPtr hWndParent,
            WndEnumProc lpEnumFunc,
            int lParam
        );

        public delegate bool WndEnumProc(IntPtr hwnd, int lParam);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attr, ref int attrValue, int attrSize);

        public enum DWMWINDOWATTRIBUTE : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        public const uint WINEVENT_OUTOFCONTEXT = 0x0000; // Events are ASYNC
        public const uint WINEVENT_SKIPOWNTHREAD = 0x0001; // Don't call back for events on installer's thread
        public const uint WINEVENT_SKIPOWNPROCESS = 0x0002; // Don't call back for events on installer's process
        public const uint WINEVENT_INCONTEXT = 0x0004; // Events are SYNC, this causes your dll to be injected into every process
        public const uint EVENT_MIN = 0x00000001;
        public const uint EVENT_MAX = 0x7FFFFFFF;
        public const uint EVENT_SYSTEM_SOUND = 0x0001;
        public const uint EVENT_SYSTEM_ALERT = 0x0002;
        public const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
        public const uint EVENT_SYSTEM_MENUSTART = 0x0004;
        public const uint EVENT_SYSTEM_MENUEND = 0x0005;
        public const uint EVENT_SYSTEM_MENUPOPUPSTART = 0x0006;
        public const uint EVENT_SYSTEM_MENUPOPUPEND = 0x0007;
        public const uint EVENT_SYSTEM_CAPTURESTART = 0x0008;
        public const uint EVENT_SYSTEM_CAPTUREEND = 0x0009;
        public const uint EVENT_SYSTEM_MOVESIZESTART = 0x000A;
        public const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        public const uint EVENT_SYSTEM_CONTEXTHELPSTART = 0x000C;
        public const uint EVENT_SYSTEM_CONTEXTHELPEND = 0x000D;
        public const uint EVENT_SYSTEM_DRAGDROPSTART = 0x000E;
        public const uint EVENT_SYSTEM_DRAGDROPEND = 0x000F;
        public const uint EVENT_SYSTEM_DIALOGSTART = 0x0010;
        public const uint EVENT_SYSTEM_DIALOGEND = 0x0011;
        public const uint EVENT_SYSTEM_SCROLLINGSTART = 0x0012;
        public const uint EVENT_SYSTEM_SCROLLINGEND = 0x0013;
        public const uint EVENT_SYSTEM_SWITCHSTART = 0x0014;
        public const uint EVENT_SYSTEM_SWITCHEND = 0x0015;
        public const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;
        public const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;
        public const uint EVENT_SYSTEM_DESKTOPSWITCH = 0x0020;
        public const uint EVENT_SYSTEM_END = 0x00FF;
        public const uint EVENT_OEM_DEFINED_START = 0x0101;
        public const uint EVENT_OEM_DEFINED_END = 0x01FF;
        public const uint EVENT_UIA_EVENTID_START = 0x4E00;
        public const uint EVENT_UIA_EVENTID_END = 0x4EFF;
        public const uint EVENT_UIA_PROPID_START = 0x7500;
        public const uint EVENT_UIA_PROPID_END = 0x75FF;
        public const uint EVENT_CONSOLE_CARET = 0x4001;
        public const uint EVENT_CONSOLE_UPDATE_REGION = 0x4002;
        public const uint EVENT_CONSOLE_UPDATE_SIMPLE = 0x4003;
        public const uint EVENT_CONSOLE_UPDATE_SCROLL = 0x4004;
        public const uint EVENT_CONSOLE_LAYOUT = 0x4005;
        public const uint EVENT_CONSOLE_START_APPLICATION = 0x4006;
        public const uint EVENT_CONSOLE_END_APPLICATION = 0x4007;
        public const uint EVENT_CONSOLE_END = 0x40FF;
        public const uint EVENT_OBJECT_CREATE = 0x8000; // hwnd ID idChild is created item
        public const uint EVENT_OBJECT_DESTROY = 0x8001; // hwnd ID idChild is destroyed item
        public const uint EVENT_OBJECT_SHOW = 0x8002; // hwnd ID idChild is shown item
        public const uint EVENT_OBJECT_HIDE = 0x8003; // hwnd ID idChild is hidden item
        public const uint EVENT_OBJECT_REORDER = 0x8004; // hwnd ID idChild is parent of zordering children
        public const uint EVENT_OBJECT_FOCUS = 0x8005; // hwnd ID idChild is focused item
        public const uint EVENT_OBJECT_SELECTION = 0x8006; // hwnd ID idChild is selected item (if only one), or idChild is OBJID_WINDOW if complex
        public const uint EVENT_OBJECT_SELECTIONADD = 0x8007; // hwnd ID idChild is item added
        public const uint EVENT_OBJECT_SELECTIONREMOVE = 0x8008; // hwnd ID idChild is item removed
        public const uint EVENT_OBJECT_SELECTIONWITHIN = 0x8009; // hwnd ID idChild is parent of changed selected items
        public const uint EVENT_OBJECT_STATECHANGE = 0x800A; // hwnd ID idChild is item w/ state change
        public const uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B; // hwnd ID idChild is moved/sized item
        public const uint EVENT_OBJECT_NAMECHANGE = 0x800C; // hwnd ID idChild is item w/ name change
        public const uint EVENT_OBJECT_DESCRIPTIONCHANGE = 0x800D; // hwnd ID idChild is item w/ desc change
        public const uint EVENT_OBJECT_VALUECHANGE = 0x800E; // hwnd ID idChild is item w/ value change
        public const uint EVENT_OBJECT_PARENTCHANGE = 0x800F; // hwnd ID idChild is item w/ new parent
        public const uint EVENT_OBJECT_HELPCHANGE = 0x8010; // hwnd ID idChild is item w/ help change
        public const uint EVENT_OBJECT_DEFACTIONCHANGE = 0x8011; // hwnd ID idChild is item w/ def action change
        public const uint EVENT_OBJECT_ACCELERATORCHANGE = 0x8012; // hwnd ID idChild is item w/ keybd accel change
        public const uint EVENT_OBJECT_TEXTSELECTIONCHANGED = 0x8014; // hwnd ID idChild is item w? test selection change
        public const uint EVENT_OBJECT_CONTENTSCROLLED = 0x8015;
        public const uint EVENT_SYSTEM_ARRANGMENTPREVIEW = 0x8016;
        public const uint EVENT_OBJECT_END = 0x80FF;
        public const uint EVENT_AIA_START = 0xA000;
        public const uint EVENT_AIA_END = 0xAFFF;

        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);
        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        /// <summary>
        ///     Passes the hook information to the next hook procedure in the current hook chain. A hook procedure can call this
        ///     function either before or after processing the hook information.
        ///     <para>
        ///     See [ https://msdn.microsoft.com/en-us/library/windows/desktop/ms644974%28v=vs.85%29.aspx ] for more
        ///     information.
        ///     </para>
        /// </summary>
        /// <param name="hhk">C++ ( hhk [in, optional]. Type: HHOOK )<br />This parameter is ignored. </param>
        /// <param name="nCode">
        ///     C++ ( nCode [in]. Type: int )<br />The hook code passed to the current hook procedure. The next
        ///     hook procedure uses this code to determine how to process the hook information.
        /// </param>
        /// <param name="wParam">
        ///     C++ ( wParam [in]. Type: WPARAM )<br />The wParam value passed to the current hook procedure. The
        ///     meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <param name="lParam">
        ///     C++ ( lParam [in]. Type: LPARAM )<br />The lParam value passed to the current hook procedure. The
        ///     meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <returns>
        ///     C++ ( Type: LRESULT )<br />This value is returned by the next hook procedure in the chain. The current hook
        ///     procedure must also return this value. The meaning of the return value depends on the hook type. For more
        ///     information, see the descriptions of the individual hook procedures.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///     Hook procedures are installed in chains for particular hook types. <see cref="CallNextHookEx" /> calls the
        ///     next hook in the chain.
        ///     </para>
        ///     <para>
        ///     Calling CallNextHookEx is optional, but it is highly recommended; otherwise, other applications that have
        ///     installed hooks will not receive hook notifications and may behave incorrectly as a result. You should call
        ///     <see cref="CallNextHookEx" /> unless you absolutely need to prevent the notification from being seen by other
        ///     applications.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int cmdShow);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref RECT pvParam, uint fWinIni);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr FindWindowA(string cname, string wname);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT rect);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric smIndex);

    }

    public enum SystemMetric
    {
        SM_CXSCREEN = 0,  // 0x00
        SM_CYSCREEN = 1,  // 0x01
        SM_CXVSCROLL = 2,  // 0x02
        SM_CYHSCROLL = 3,  // 0x03
        SM_CYCAPTION = 4,  // 0x04
        SM_CXBORDER = 5,  // 0x05
        SM_CYBORDER = 6,  // 0x06
        SM_CXDLGFRAME = 7,  // 0x07
        SM_CXFIXEDFRAME = 7,  // 0x07
        SM_CYDLGFRAME = 8,  // 0x08
        SM_CYFIXEDFRAME = 8,  // 0x08
        SM_CYVTHUMB = 9,  // 0x09
        SM_CXHTHUMB = 10, // 0x0A
        SM_CXICON = 11, // 0x0B
        SM_CYICON = 12, // 0x0C
        SM_CXCURSOR = 13, // 0x0D
        SM_CYCURSOR = 14, // 0x0E
        SM_CYMENU = 15, // 0x0F
        SM_CXFULLSCREEN = 16, // 0x10
        SM_CYFULLSCREEN = 17, // 0x11
        SM_CYKANJIWINDOW = 18, // 0x12
        SM_MOUSEPRESENT = 19, // 0x13
        SM_CYVSCROLL = 20, // 0x14
        SM_CXHSCROLL = 21, // 0x15
        SM_DEBUG = 22, // 0x16
        SM_SWAPBUTTON = 23, // 0x17
        SM_CXMIN = 28, // 0x1C
        SM_CYMIN = 29, // 0x1D
        SM_CXSIZE = 30, // 0x1E
        SM_CYSIZE = 31, // 0x1F
        SM_CXSIZEFRAME = 32, // 0x20
        SM_CXFRAME = 32, // 0x20
        SM_CYSIZEFRAME = 33, // 0x21
        SM_CYFRAME = 33, // 0x21
        SM_CXMINTRACK = 34, // 0x22
        SM_CYMINTRACK = 35, // 0x23
        SM_CXDOUBLECLK = 36, // 0x24
        SM_CYDOUBLECLK = 37, // 0x25
        SM_CXICONSPACING = 38, // 0x26
        SM_CYICONSPACING = 39, // 0x27
        SM_MENUDROPALIGNMENT = 40, // 0x28
        SM_PENWINDOWS = 41, // 0x29
        SM_DBCSENABLED = 42, // 0x2A
        SM_CMOUSEBUTTONS = 43, // 0x2B
        SM_SECURE = 44, // 0x2C
        SM_CXEDGE = 45, // 0x2D
        SM_CYEDGE = 46, // 0x2E
        SM_CXMINSPACING = 47, // 0x2F
        SM_CYMINSPACING = 48, // 0x30
        SM_CXSMICON = 49, // 0x31
        SM_CYSMICON = 50, // 0x32
        SM_CYSMCAPTION = 51, // 0x33
        SM_CXSMSIZE = 52, // 0x34
        SM_CYSMSIZE = 53, // 0x35
        SM_CXMENUSIZE = 54, // 0x36
        SM_CYMENUSIZE = 55, // 0x37
        SM_ARRANGE = 56, // 0x38
        SM_CXMINIMIZED = 57, // 0x39
        SM_CYMINIMIZED = 58, // 0x3A
        SM_CXMAXTRACK = 59, // 0x3B
        SM_CYMAXTRACK = 60, // 0x3C
        SM_CXMAXIMIZED = 61, // 0x3D
        SM_CYMAXIMIZED = 62, // 0x3E
        SM_NETWORK = 63, // 0x3F
        SM_CLEANBOOT = 67, // 0x43
        SM_CXDRAG = 68, // 0x44
        SM_CYDRAG = 69, // 0x45
        SM_SHOWSOUNDS = 70, // 0x46
        SM_CXMENUCHECK = 71, // 0x47
        SM_CYMENUCHECK = 72, // 0x48
        SM_SLOWMACHINE = 73, // 0x49
        SM_MIDEASTENABLED = 74, // 0x4A
        SM_MOUSEWHEELPRESENT = 75, // 0x4B
        SM_XVIRTUALSCREEN = 76, // 0x4C
        SM_YVIRTUALSCREEN = 77, // 0x4D
        SM_CXVIRTUALSCREEN = 78, // 0x4E
        SM_CYVIRTUALSCREEN = 79, // 0x4F
        SM_CMONITORS = 80, // 0x50
        SM_SAMEDISPLAYFORMAT = 81, // 0x51
        SM_IMMENABLED = 82, // 0x52
        SM_CXFOCUSBORDER = 83, // 0x53
        SM_CYFOCUSBORDER = 84, // 0x54
        SM_TABLETPC = 86, // 0x56
        SM_MEDIACENTER = 87, // 0x57
        SM_STARTER = 88, // 0x58
        SM_SERVERR2 = 89, // 0x59
        SM_MOUSEHORIZONTALWHEELPRESENT = 91, // 0x5B
        SM_CXPADDEDBORDER = 92, // 0x5C
        SM_DIGITIZER = 94, // 0x5E
        SM_MAXIMUMTOUCHES = 95, // 0x5F

        SM_REMOTESESSION = 0x1000, // 0x1000
        SM_SHUTTINGDOWN = 0x2000, // 0x2000
        SM_REMOTECONTROL = 0x2001, // 0x2001


        SM_CONVERTIBLESLATEMODE = 0x2003,
        SM_SYSTEMDOCKED = 0x2004,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }

        public static implicit operator System.Drawing.Rectangle(RECT r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        public static implicit operator RECT(System.Drawing.Rectangle r)
        {
            return new RECT(r);
        }

        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is RECT)
                return Equals((RECT)obj);
            else if (obj is System.Drawing.Rectangle)
                return Equals(new RECT((System.Drawing.Rectangle)obj));
            return false;
        }

        public override int GetHashCode()
        {
            return ((System.Drawing.Rectangle)this).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }
}