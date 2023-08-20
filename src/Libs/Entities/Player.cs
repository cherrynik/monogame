using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Entities;

public class Player
{
    private readonly IMovement _movement;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private Vector2 _position;

    public Player(IMovement movement, Texture2D texture, SpriteBatch spriteBatch)
    {
        _movement = movement;
        _texture = texture;
        _spriteBatch = spriteBatch;
    }

    // TODO: Reimplement later to something useful with dependency injection
    // I also prefer having update in a single place
    public void Update()
    {
        // TODO: Use normalized movement here by math
        KeyboardState keyboardState = Keyboard.GetState();
        float horizontalDir = Convert.ToSingle(keyboardState.IsKeyDown(Keys.Right)) -
                              Convert.ToSingle(keyboardState.IsKeyDown(Keys.Left));

        float verticalDir = Convert.ToSingle(keyboardState.IsKeyDown(Keys.Down)) -
                            Convert.ToSingle(keyboardState.IsKeyDown(Keys.Up));

        if (horizontalDir == 0 && verticalDir == 0)
        {
            return;
        }

        _position = _movement.Move(_position, new Vector2 { X = horizontalDir, Y = verticalDir });

        Console.WriteLine(_position.ToString());
    }

    public void Draw()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture, _position, Color.White);
        _spriteBatch.End();
    }
}
