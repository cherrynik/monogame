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
    private readonly IInputScanner _inputScanner;
    private readonly Dictionary<RadDir, Texture2D> _directions;

    public PlayerView(IInputScanner inputScanner, Dictionary<RadDir, Texture2D> directions)
    {
        _inputScanner = inputScanner;
        _directions = directions;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 at)
    {
        // TODO: sprite animation
        // TODO: E2E testing for such a lil bit complex logic
        Vector2 direction = _inputScanner.GetDirection();
        RadDir radDir = MathUtils.Rad8DirYFlipped(direction);
        Texture2D value = _directions[radDir]; // TODO: Persist last state after stopped moving

        if (!direction.Equals(Vector2.Zero))
        {
            Console.WriteLine((value, direction, at));
        }

        spriteBatch.Begin();
        // TODO: static data, configs in json, so we could load metadata and any kind of info automatically
        spriteBatch.Draw(value, at, Color.White);
        spriteBatch.End();
    }
}
