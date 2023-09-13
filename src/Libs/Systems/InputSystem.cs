using Entitas;
using Microsoft.Xna.Framework;
using Services;

namespace Systems;

public class InputSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IInputScanner _inputScanner;
    private readonly IGroup<GameEntity> _group;

    public InputSystem(Contexts contexts, IInputScanner inputScanner)
    {
        _contexts = contexts;
        _inputScanner = inputScanner;

        _group = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Movable, GameMatcher.Transform));
    }

    public void Execute()
    {
        Vector2 direction = _inputScanner.GetDirection();

        GameEntity[]? entities = _group.GetEntities();
        foreach (GameEntity e in entities)
        {
            e.transform.Velocity = direction;
            Console.WriteLine((e.transform.Position));
        }
    }
}
