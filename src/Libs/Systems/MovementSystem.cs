using Entitas;
using Microsoft.Xna.Framework;
using Services;
using IExecuteSystem = Entitas.Extended.IExecuteSystem;

namespace Systems;

public class MovementSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _group;
    private readonly ISafeMovement _movement;

    public MovementSystem(IGroup<GameEntity> group, ISafeMovement movement)
    {
        _group = group;
        _movement = movement;
    }

    public void Execute(GameTime gameTime)
    {
        GameEntity[]? entities = _group.GetEntities();
        foreach (GameEntity e in entities)
        {
            e.transform.Position = _movement.SafeMove(e.transform.Position, e.transform.Velocity);
        }
    }
}
