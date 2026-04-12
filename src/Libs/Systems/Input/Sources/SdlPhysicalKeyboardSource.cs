using System;
using System.Numerics;
using Services.Input;
using Systems.Input.Abstractions;
using Systems.Input.Model;

namespace Systems.Input.Sources;

public sealed class SdlPhysicalKeyboardSource : IPhysicalKeyboardSource
{
    private static readonly ScancodeAxisBinding HorizontalBinding = new(80, 79);
    private static readonly ScancodeAxisBinding VerticalBinding = new(82, 81);
    private static readonly ScancodeAxisBinding HorizontalWasdBinding = new(4, 7);
    private static readonly ScancodeAxisBinding VerticalWasdBinding = new(26, 22);

    private readonly IScancodeKeyboardStateSource _scancodeSource;

    public SdlPhysicalKeyboardSource() : this(new SdlKeyboardStateSource())
    {
    }

    public SdlPhysicalKeyboardSource(IScancodeKeyboardStateSource scancodeSource)
    {
        _scancodeSource = scancodeSource ?? throw new ArgumentNullException(nameof(scancodeSource));
    }

    public bool TryGetDirection(out Vector2 direction)
    {
        if (!_scancodeSource.TryGetState(out ScancodeKeyboardState state))
        {
            direction = Vector2.Zero;
            return false;
        }

        int x = ResolveAxis(state, HorizontalBinding, HorizontalWasdBinding);
        int y = ResolveAxis(state, VerticalBinding, VerticalWasdBinding);
        direction = new Vector2(x, y);
        return true;
    }

    private static int ResolveAxis(ScancodeKeyboardState state, ScancodeAxisBinding arrows, ScancodeAxisBinding wasd) =>
        ResolveAxisDirection(
            state.IsPressed(arrows.Negative) || state.IsPressed(wasd.Negative),
            state.IsPressed(arrows.Positive) || state.IsPressed(wasd.Positive));

    private static int ResolveAxisDirection(bool negativePressed, bool positivePressed)
    {
        int positive = positivePressed ? 1 : 0;
        int negative = negativePressed ? 1 : 0;
        return positive - negative;
    }
}
