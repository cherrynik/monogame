using Entitas;
using Microsoft.Xna.Framework;
using Services;
using IExecuteSystem = Entitas.Extended.IExecuteSystem;

namespace Systems;

public class MovementSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _group;
    private readonly IMovement _movement;

    public MovementSystem(IGroup<GameEntity> group, IMovement movement)
    {
        _group = group;
        _movement = movement;
    }

    public void Execute(GameTime gameTime)
    {
        GameEntity[]? entities = _group.GetEntities();
        foreach (GameEntity e in entities)
        {
            if (e.transform.Velocity.Equals(Vector2.Zero))
            {
                continue;
            }

            e.transform.Position = _movement.Move(e.transform.Position, e.transform.Velocity);
        }
    }
}
