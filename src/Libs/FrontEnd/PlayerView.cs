using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services;

namespace FrontEnd;

public interface IView
{
    void Draw(SpriteBatch spriteBatch, Vector2 at);
}

public class PlayerView : IView
{
    private readonly IInputScanner _inputScanner;
    private readonly Dictionary<RadDir, AnimatedSprite> _directions;

    public PlayerView(IInputScanner inputScanner, Dictionary<RadDir, AnimatedSprite> directions)
    {
        _inputScanner = inputScanner;
        _directions = directions;

        foreach (var direction in directions)
        {
            direction.Value.Play(); // Important to do when initialized
        }

        _directions[RadDir.Left].FlipHorizontally = true;
    }

    public void Update(GameTime gameTime)
    {
        // TODO: sprite animation
        // TODO: E2E testing for such a lil bit complex logic
        Vector2 direction = _inputScanner.GetDirection();
        RadDir radDir = MathUtils.Rad8DirYFlipped(direction);
        AnimatedSprite value = _directions[radDir]; // TODO: Persist last state after stopped moving
        
        value.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 at)
    {
        // TODO: sprite animation
        // TODO: E2E testing for such a lil bit complex logic
        Vector2 direction = _inputScanner.GetDirection();
        RadDir radDir = MathUtils.Rad8DirYFlipped(direction);
        AnimatedSprite value = _directions[radDir]; // TODO: Persist last state after stopped moving

        if (!direction.Equals(Vector2.Zero))
        {
            Console.WriteLine((value, direction, at));
        }

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        // TODO: static data, configs in json, so we could load metadata and any kind of info automatically
        value.Draw(spriteBatch, at);
        spriteBatch.End();
    }
}
