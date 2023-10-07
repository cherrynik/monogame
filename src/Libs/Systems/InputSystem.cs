using Entitas;
using Microsoft.Xna.Framework;
using Serilog;
using Services.Input;
using IExecuteSystem = Entitas.Extended.IExecuteSystem;

namespace Systems;

[Input]
public class InputSystem : IExecuteSystem
{
    private readonly IInputScanner _inputScanner;
    private readonly IGroup<GameEntity> _group;
    private readonly ILogger _logger;

    public InputSystem(IInputScanner inputScanner, IGroup<GameEntity> group, ILogger logger)
    {
        _inputScanner = inputScanner;
        _group = group;
        _logger = logger;
    }

    public void Execute(GameTime gameTime)
    {
        try
        {
            Vector2 direction = _inputScanner.GetDirection();

            if (direction.Equals(Vector2.Zero) is false)
            {
                _logger.ForContext<InputSystem>().Verbose(direction.ToString()!);
            }

            GameEntity[] entities = _group.GetEntities();
            foreach (GameEntity e in entities)
            {
                e.transform.Velocity = direction;
            }
        }
        catch (Exception e)
        {
            _logger.ForContext<InputSystem>().Fatal(e.ToString());

            throw new Exception(e.Message);
        }
    }
}
