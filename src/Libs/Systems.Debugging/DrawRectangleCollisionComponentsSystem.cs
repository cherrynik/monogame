using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Systems.Debugging;

public class DrawRectangleCollisionComponentsSystem : IDrawSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;
    private readonly ILogger _logger;

    public DrawRectangleCollisionComponentsSystem(Contexts contexts, IGroup<GameEntity> group, ILogger logger)
    {
        _contexts = contexts;
        _group = group;
        _logger = logger;
    }

    // TODO: Move the logic in the release features
    // TODO: Make sloped collision
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        GameEntity[] entities = _group.GetEntities().OrderBy(x => x.transform.Position.Y).ToArray();

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // temp start
        Color[] colors = { Color.Black, Color.White };
        int k = 0;
        // temp end

        for (int i = 0; i < entities.Length; ++i)
        {
            GameEntity first = entities[i];

            for (int j = i + 1; j < entities.Length; ++j)
            {
                GameEntity second = entities[j];
                if (AreIntersecting(first, second))
                {
                    first.transform.Velocity = Vector2.Zero;
                    second.transform.Velocity = Vector2.Zero;
                }
            }

            // temp start
            var texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            texture.SetData(new[] { colors[k] });
            ++k;
            if (k is 2)
                k = 0;
            // temp end

            spriteBatch.Draw(texture, first.transform.Position, sourceRectangle: first.rectangleCollision.Size,
                Color.White);
        }

        spriteBatch.End();
    }

    private bool AreIntersecting(GameEntity first, GameEntity second)
    {
        Func<GameEntity, Rectangle> buildRectangle = new(x =>
            new((int)(x.transform.Position.X + x.transform.Velocity.X),
                (int)(x.transform.Position.Y + x.transform.Velocity.Y),
                x.rectangleCollision.Size.Width,
                x.rectangleCollision.Size.Height));

        Rectangle firstRectangle = buildRectangle(first);
        Rectangle secondRectangle = buildRectangle(second);

        return Rectangle.Intersect(firstRectangle, secondRectangle).IsEmpty is false;
    }
}
