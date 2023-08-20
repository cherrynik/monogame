using System.Numerics;
using Microsoft.Xna.Framework.Input;

namespace Services;

public class InputMovement
{
    public InputMovement() { }

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
