using FrontEnd;
using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Services;

namespace Entities;

public class Player
{
    private readonly IMovement _movement;
    private readonly Texture2D _spriteSheet;
    private readonly IInputScanner _inputScanner;
    private readonly PlayerView _playerView;
    private Vector2 _position;

    public Player(IMovement movement, IInputScanner inputScanner, PlayerView playerView)
    {
        _movement = movement;
        _inputScanner = inputScanner;
        _playerView = playerView;
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
        // spriteBatch.Begin();
        // spriteBatch.Draw(_spriteSheet, _position, Color.White);
        // spriteBatch.End();
        _playerView.Draw(spriteBatch, _position);
    }
}
