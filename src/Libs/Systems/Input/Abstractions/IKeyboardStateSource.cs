using Microsoft.Xna.Framework.Input;

namespace Systems.Input.Abstractions;

public interface IKeyboardStateSource
{
    KeyboardState GetState();
}
