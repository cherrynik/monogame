using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Services;

namespace FrontEnd;

public interface IView
{
    void Draw(SpriteBatch spriteBatch, Vector2 at);
}

public class PlayerView : IView
{
    private readonly Texture2D _spriteSheet;
    private readonly IInputScanner _inputScanner;
    private readonly Dictionary<RadDir, string> _directions;

    public PlayerView(Texture2D spriteSheet, IInputScanner inputScanner, Dictionary<RadDir, string> directions)
    {
        _spriteSheet = spriteSheet;
        _inputScanner = inputScanner;
        _directions = directions;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 at)
    {
        // TODO: sprite animation
        Vector2 direction = _inputScanner.GetDirection();
        RadDir radDir = MathUtils.Rad8DirYFlipped(direction);
        string value = _directions[radDir];
        if (!direction.Equals(Vector2.Zero))
        {
            Console.WriteLine((value, direction));
        }

        spriteBatch.Begin();
        // TODO: static data, configs in json, so we could load meta-data and any kind of info automatically
        spriteBatch.Draw(_spriteSheet, at, new Rectangle(18, 20, 16, 22), Color.White);
        spriteBatch.End();
    }
}
