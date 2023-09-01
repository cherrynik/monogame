using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services;
using Stateless;

namespace Entities;

public class Player
{
    private readonly IMovement _movement;
    private readonly IInputScanner _inputScanner;
    private readonly PlayerView _playerView;

    private Vector2 _position;

    public Player(IMovement movement, IInputScanner inputScanner, PlayerView playerView)
    {
        _movement = movement;
        _inputScanner = inputScanner;
        _playerView = playerView;
    }

    public void Update(GameTime gameTime)
    {
        Vector2 direction = _inputScanner.GetDirection();

        _playerView.Update(gameTime, direction);

        if (direction.Equals(Vector2.Zero))
        {
            return;
        }

        _position = _movement.Move(_position, direction);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _playerView.Draw(spriteBatch, _position);
    }
}
