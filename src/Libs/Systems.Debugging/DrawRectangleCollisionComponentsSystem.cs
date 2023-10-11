using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Systems.Debugging;

public class DrawRectangleCollisionComponentsSystem : IDrawSystem
{
    private readonly Contexts _contexts;
    private readonly ILogger _logger;

    public DrawRectangleCollisionComponentsSystem(Contexts contexts, IGroup<GameEntity> group, ILogger logger)
    {
        _contexts = contexts;
        _logger = logger;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _logger.ForContext<DrawRectangleCollisionComponentsSystem>().Verbose("Draw");
    }
}
