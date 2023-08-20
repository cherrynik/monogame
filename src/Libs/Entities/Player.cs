using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Services;

namespace Entities;

public class Player
{
    private readonly IMovement _movement;
    private readonly Texture2D _texture;
    private readonly IInputScanner _inputScanner;
    private Vector2 _position;

    public Player(IMovement movement, Texture2D texture, IInputScanner inputScanner)
    {
        _movement = movement;
        _texture = texture;
        _inputScanner = inputScanner;
    }

    // I prefer having update in a single place, but fo' now sum like diz
    public void Update()
    {
        Vector2 direction = _inputScanner.GetDirection();
        if (direction.Equals(Vector2.Zero))
        {
            return;
        }

        _position = _movement.Move(_position, direction);

        Console.WriteLine(_position.ToString());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_texture, _position, Color.White);
        spriteBatch.End();
    }
}
