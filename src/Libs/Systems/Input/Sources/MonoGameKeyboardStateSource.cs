using Microsoft.Xna.Framework.Input;
using Systems.Input.Abstractions;

namespace Systems.Input.Sources;

public sealed class MonoGameKeyboardStateSource : IKeyboardStateSource
{
    public KeyboardState GetState() => Keyboard.GetState();
}
