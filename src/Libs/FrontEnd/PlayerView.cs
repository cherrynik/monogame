using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services;
using Stateless;

namespace FrontEnd;

public interface IView
{
    void Draw(SpriteBatch spriteBatch, Vector2 at);
}

public class PlayerView : IView
{
    private readonly Dictionary<RadDir, AnimatedSprite> _directions;
    private AnimatedSprite _animatedSprite;

    public PlayerView(IInputScanner inputScanner, Dictionary<RadDir, AnimatedSprite> directions)
    {
        _directions = directions;
    }

    public void Apply(Vector2 direction)
    {
        RadDir radDir = MathUtils.Rad8DirYFlipped(direction);
        _animatedSprite = _directions[radDir]; // TODO: Persist last state after stopped moving
    }

    public void Update(GameTime gameTime)
    {
        _animatedSprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 at)
    {
        // TODO: sprite animation
        // TODO: E2E testing for such a lil bit complex logic
        // TODO: static data, configs in json, so we could load metadata and any kind of info automatically

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _animatedSprite.Draw(spriteBatch, at);
        spriteBatch.End();
    }
}

