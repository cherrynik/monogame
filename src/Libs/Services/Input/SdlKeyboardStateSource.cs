using System;
using System.Runtime.InteropServices;

namespace Services.Input;

public sealed class SdlKeyboardStateSource : IScancodeKeyboardStateSource
{
    [DllImport("SDL2", EntryPoint = "SDL_PumpEvents")]
    private static extern void SDL_PumpEvents();

    [DllImport("SDL2", EntryPoint = "SDL_GetKeyboardState")]
    private static extern IntPtr SDL_GetKeyboardState(out int numkeys);

    [DllImport("libSDL2-2.0.0.dylib", EntryPoint = "SDL_PumpEvents")]
    private static extern void SDL_PumpEvents_Mac();

    [DllImport("libSDL2-2.0.0.dylib", EntryPoint = "SDL_GetKeyboardState")]
    private static extern IntPtr SDL_GetKeyboardState_Mac(out int numkeys);

    public bool TryGetState(out ScancodeKeyboardState state)
    {
        if (TryReadState(SDL_PumpEvents, SDL_GetKeyboardState, out state))
        {
            return true;
        }

        if (TryReadState(SDL_PumpEvents_Mac, SDL_GetKeyboardState_Mac, out state))
        {
            return true;
        }

        state = default;
        return false;
    }

    private delegate void PumpEvents();
    private delegate IntPtr GetKeyboardState(out int numkeys);

    private static bool TryReadState(PumpEvents pumpEvents, GetKeyboardState getKeyboardState, out ScancodeKeyboardState state)
    {
        state = default;
        try
        {
            pumpEvents();
            IntPtr keyboardState = getKeyboardState(out int keyCount);
            if (keyboardState == IntPtr.Zero || keyCount <= 0)
            {
                return false;
            }

            state = new ScancodeKeyboardState(keyboardState, keyCount);
            return true;
        }
        catch (DllNotFoundException)
        {
            return false;
        }
        catch (EntryPointNotFoundException)
        {
            return false;
        }
    }
}
