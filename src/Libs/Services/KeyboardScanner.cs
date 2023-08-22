using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Services;

public interface IInputScanner
{
    Vector2 GetDirection();
}

public class KeyboardScanner : IInputScanner
{
    private static int GetAxisDirection(Keys negative, Keys positive)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        int a = keyboardState.IsKeyDown(positive) ? 1 : 0;
        int b = keyboardState.IsKeyDown(negative) ? 1 : 0;

        return a - b;
    }

    public Vector2 GetDirection() =>
        new(GetAxisDirection(Keys.Left, Keys.Right),
            GetAxisDirection(Keys.Down, Keys.Up));
}
