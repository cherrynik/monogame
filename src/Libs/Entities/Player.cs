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
    private readonly InputMovement _inputMovement;
    private Vector2 _position;

    public Player(IMovement movement, Texture2D texture, InputMovement inputMovement)
    {
        _movement = movement;
        _texture = texture;
        _inputMovement = inputMovement;
    }

    // TODO: Reimplement later to something useful with dependency injection
    // I also prefer having update in a single place
    public void Update()
    {
        // TODO: Use normalized movement here by math
        _position = _movement.Move(_position, _inputMovement.GetDirection());

        Console.WriteLine(_position.ToString());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_texture, _position, Color.White);
        spriteBatch.End();
    }
}
