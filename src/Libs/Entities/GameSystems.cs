using Entitas;
using Microsoft.Xna.Framework.Graphics;
using Services.Input;
using Services.Movement;
using Systems;
using Systems.Input;
using Systems.Sprites;

namespace Entities;

public sealed class GameSystems : Entitas.Extended.Feature
{
    private readonly Contexts _contexts;
    private readonly GraphicsDevice _graphicsDevice;

    public GameSystems(Contexts contexts, GraphicsDevice graphicsDevice)
    {
        _contexts = contexts;
        _graphicsDevice = graphicsDevice;

        RegisterSystems();
    }

    private void RegisterSystems()
    {
        RegisterEntitySystem();
        RegisterMovementSystem();
    }

    private void RegisterEntitySystem()
    {
        Add(new CreateEntitySystem(_contexts, _graphicsDevice));
    }

    private void RegisterMovementSystem()
    {
        var movableGroup = AllOf(GameMatcher.Player, GameMatcher.Movable, GameMatcher.Transform);

        // Input
        Add(new InputSystem(new KeyboardScanner(), movableGroup));

        // Update
        Add(new MovementSystem(movableGroup, new SimpleMovement()));

        // Render
        var animatedMovementGroup = AllOf(GameMatcher.Movable, GameMatcher.Transform, GameMatcher.AnimatedMovement);
        Add(new AnimatedMovementSystem(animatedMovementGroup));
    }

    private IGroup<GameEntity> AllOf(params IMatcher<GameEntity>[] matchers) =>
        _contexts.game.GetGroup(GameMatcher.AllOf(matchers));
}
