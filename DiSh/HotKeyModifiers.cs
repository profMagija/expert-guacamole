using System;

namespace DiSh
{
    /// <summary>
    /// Simpler way to expose key modifiers
    /// </summary>
    [Flags]
    public enum HotKeyModifiers
    {
        None = 0,
        Alt = 1,        // MOD_ALT
        Control = 2,    // MOD_CONTROL
        Shift = 4,      // MOD_SHIFT
        WindowsKey = 8,     // MOD_WIN
    }
}