using Entitas;
using Microsoft.Xna.Framework;
using Services.Input;
using IExecuteSystem = Entitas.Extended.IExecuteSystem;

namespace Systems.Input;

[Input]
public class InputSystem : IExecuteSystem
{
    private readonly IInputScanner _inputScanner;
    private readonly IGroup<GameEntity> _group;

    public InputSystem(IInputScanner inputScanner, IGroup<GameEntity> group)
    {
        _inputScanner = inputScanner;
        _group = group;
    }

    public void Execute(GameTime gameTime)
    {
        Vector2 direction = _inputScanner.GetDirection();

        GameEntity[]? entities = _group.GetEntities();
        foreach (GameEntity e in entities)
        {
            e.transform.Velocity = direction;
        }
    }
}
