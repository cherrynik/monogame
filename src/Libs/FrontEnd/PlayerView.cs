using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Services;

namespace FrontEnd;

public interface IView
{
    void Draw(SpriteBatch spriteBatch);
}

public class PlayerView
{
    private readonly Texture2D _spriteSheet;
    private readonly IInputScanner _inputScanner;

    public PlayerView(Texture2D spriteSheet, IInputScanner inputScanner)
    {
        _spriteSheet = spriteSheet;
        _inputScanner = inputScanner;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 at)
    {
        // TODO: sprite animation
        // Vector2 direction = _inputScanner.GetDirection();

        spriteBatch.Begin();
        // TODO: static data, configs in json, so we could load meta-data and any kind of info automatically
        spriteBatch.Draw(_spriteSheet, at, new Rectangle(18, 20, 16, 22), Color.White);
        spriteBatch.End();
    }
}
