using System;
using System.Numerics;
using Microsoft.Xna.Framework.Input;
using Systems.Input.Abstractions;
using Systems.Input.Model;
using Systems.Input.Sources;

namespace Systems.Input;

public sealed class KeyboardInput : IInputScanner
{
    private static readonly KeyAxisBinding HorizontalBinding = new([Keys.Left, Keys.A], [Keys.Right, Keys.D]);
    private static readonly KeyAxisBinding VerticalBinding = new([Keys.Up, Keys.W], [Keys.Down, Keys.S]);

    private readonly IKeyboardStateSource _keyboardStateSource;
    private readonly IPhysicalKeyboardSource _physicalKeyboardSource;

    public KeyboardInput() : this(new MonoGameKeyboardStateSource(), new SdlPhysicalKeyboardSource())
    {
    }

    public KeyboardInput(IKeyboardStateSource keyboardStateSource)
        : this(keyboardStateSource, DisabledPhysicalKeyboardSource.Instance)
    {
    }

    public KeyboardInput(IKeyboardStateSource keyboardStateSource, IPhysicalKeyboardSource physicalKeyboardSource)
    {
        _keyboardStateSource = keyboardStateSource ?? throw new ArgumentNullException(nameof(keyboardStateSource));
        _physicalKeyboardSource = physicalKeyboardSource ?? throw new ArgumentNullException(nameof(physicalKeyboardSource));
    }

    public Vector2 GetDirection()
    {
        if (_physicalKeyboardSource.TryGetDirection(out Vector2 directionFromPhysicalKeyboard) &&
            directionFromPhysicalKeyboard != Vector2.Zero)
        {
            return directionFromPhysicalKeyboard;
        }

        KeyboardState keyboardState = _keyboardStateSource.GetState();

        return new(
            ResolveAxisDirection(keyboardState, HorizontalBinding),
            ResolveAxisDirection(keyboardState, VerticalBinding));
    }

    private static int ResolveAxisDirection(KeyboardState keyboardState, KeyAxisBinding binding)
    {
        int positive = IsAnyKeyDown(keyboardState, binding.Positive) ? 1 : 0;
        int negative = IsAnyKeyDown(keyboardState, binding.Negative) ? 1 : 0;
        return positive - negative;
    }

    private static bool IsAnyKeyDown(KeyboardState keyboardState, Keys[] keys) =>
        keys.Any(keyboardState.IsKeyDown);

    private sealed class DisabledPhysicalKeyboardSource : IPhysicalKeyboardSource
    {
        public static readonly DisabledPhysicalKeyboardSource Instance = new();

        public bool TryGetDirection(out Vector2 direction)
        {
            direction = Vector2.Zero;
            return false;
        }
    }
}
