using System.Numerics;
using Microsoft.Xna.Framework.Input;

namespace Services;

public interface IInputScanner
{
    Vector2 GetDirection();
}

public class KeyboardScanner : IInputScanner
{
    public KeyboardScanner() { }

    public Vector2 GetDirection()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        float horizontalDir = Convert.ToSingle(keyboardState.IsKeyDown(Keys.Right)) -
                              Convert.ToSingle(keyboardState.IsKeyDown(Keys.Left));

        float verticalDir = Convert.ToSingle(keyboardState.IsKeyDown(Keys.Down)) -
                            Convert.ToSingle(keyboardState.IsKeyDown(Keys.Up));

        return new Vector2(horizontalDir, verticalDir);
    }
}
