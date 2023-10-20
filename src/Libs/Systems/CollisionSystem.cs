using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Serilog;

namespace Systems;

// Input & Collision systems both have to be fixed execute systems,
// otherwise it'll lead to the desynchronized behaviour.
public class CollisionSystem : IFixedExecuteSystem
{
    private readonly IGroup<GameEntity> _group;
    private readonly ILogger _logger;

    public CollisionSystem(IGroup<GameEntity> group, ILogger logger)
    {
        _group = group;
        _logger = logger;
    }

    // TODO: Optimize efficiency
    public void FixedExecute(GameTime gameTime)
    {
        GameEntity[] entities = _group.GetEntities();

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
        }
    }

    private static bool AreIntersecting(GameEntity first, GameEntity second)
    {
        Rectangle firstRectangle = BuildRectangle(first);
        Rectangle secondRectangle = BuildRectangle(second);

        Rectangle intersect = Rectangle.Intersect(firstRectangle, secondRectangle);

        return !intersect.IsEmpty;

        Rectangle BuildRectangle(GameEntity x) => new((int)(x.transform.Position.X + x.transform.Velocity.X),
            (int)(x.transform.Position.Y + x.transform.Velocity.Y), x.rectangleCollision.Size.Width,
            x.rectangleCollision.Size.Height);
    }
}
