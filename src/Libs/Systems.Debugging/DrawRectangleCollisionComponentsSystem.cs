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

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        GameEntity[] entities = _group.GetEntities();


        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // temp start
        Color[] colors = { Color.Black, Color.White };
        int i = 0;
        // temp end

        foreach (var e in entities)
        {
            Vector2 at = e.transform.Position;
            Rectangle rectangleSize = e.rectangleCollision.Size;

            // temp start
            var texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            texture.SetData(new[] { colors[i] });
            ++i;
            if (i is 2)
                i = 0;
            // temp end

            spriteBatch.Draw(texture, at, sourceRectangle: rectangleSize, Color.White);
        }

        spriteBatch.End();
    }
}
