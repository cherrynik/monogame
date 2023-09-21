using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Serilog;
using Services;
using IExecuteSystem = Entitas.Extended.IExecuteSystem;

namespace Systems;

// TODO: correct fixed execution in the game loop
public class MovementSystem : IFixedExecuteSystem
{
    private readonly IGroup<GameEntity> _group;
    private readonly IMovement _movement;
    private readonly ILogger _logger;

    public MovementSystem(IMovement movement, IGroup<GameEntity> group, ILogger logger)
    {
        _group = group;
        _movement = movement;
        _logger = logger;
    }

    public void FixedExecute(GameTime fixedGameTime)
    {
        try
        {
            GameEntity[] entities = _group.GetEntities();
            foreach (GameEntity e in entities)
            {
                if (e.transform.Velocity.Equals(Vector2.Zero))
                {
                    continue;
                }

                e.transform.Position = _movement.Move(e.transform.Position, e.transform.Velocity);
                _logger.ForContext<MovementSystem>().Verbose(e.transform.Position.ToString());
            }
        }
        catch (Exception e)
        {
            _logger.ForContext<MovementSystem>().Fatal(e.ToString());

            throw new Exception(e.Message);
        }
    }
}
