using System;
using System.Runtime.InteropServices;

namespace Services.Input;

public readonly struct ScancodeKeyboardState(IntPtr keyboardState, int keyCount)
{
    private const byte Pressed = 1;

    public bool IsPressed(int scancode)
    {
        if (keyboardState == IntPtr.Zero || scancode < 0 || scancode >= keyCount)
        {
            return false;
        }

        return Marshal.ReadByte(keyboardState, scancode) == Pressed;
    }
}
