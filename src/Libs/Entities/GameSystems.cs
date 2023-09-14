using Entitas.Extended;
using Services;
using Services.Input;
using Services.Movement;
using Systems;
using Systems.Sprites;

namespace Entities;

public sealed class GameSystems : Entitas.Extended.Feature
{
    private readonly Contexts _contexts;
    private readonly Components.Sprites.MovementAnimatedSprites _movementAnimatedSprites;

    public GameSystems(Contexts contexts, Components.Sprites.MovementAnimatedSprites movementAnimatedSprites)
    {
        _contexts = contexts;
        _movementAnimatedSprites = movementAnimatedSprites;

        RegisterSystems();
    }

    private void RegisterSystems()
    {
        RegisterEntitySystem();
        RegisterMovementSystem();
    }

    private void RegisterEntitySystem()
    {
        Add(new CreateEntity(_contexts, _movementAnimatedSprites));
    }

    private void RegisterMovementSystem()
    {
        var movableGroup =
            _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player,
                GameMatcher.Movable,
                GameMatcher.Transform));

        // Input
        Add(new Input(new KeyboardScanner(), movableGroup));

        // Update
        Add(new Movement(movableGroup, new SimpleMovement()));

        // Render
        var animatedMovementGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Movable,
            GameMatcher.Transform, GameMatcher.MovementAnimatedSprites));
        Add(new MovementAnimatedSprites(animatedMovementGroup));
    }
}
