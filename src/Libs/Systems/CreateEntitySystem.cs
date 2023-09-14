using Components.Sprites;
using Entitas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services;
using Services.Factories;
using Stateless;
using Vector2 = System.Numerics.Vector2;

namespace Systems;

public sealed class CreateEntitySystem : IInitializeSystem
{
    private readonly Contexts _contexts;
    private readonly GraphicsDevice _graphicsDevice;

    public CreateEntitySystem(Contexts contexts, GraphicsDevice graphicsDevice)
    {
        _contexts = contexts;
        _graphicsDevice = graphicsDevice;
    }

    public void Initialize()
    {
        CreatePlayer();

        Console.WriteLine("Initialized.");
    }

    private void CreatePlayer()
    {
        GameEntity? e = _contexts.game.CreateEntity();

        e.isPlayer = true;
        e.isMovable = true;

        SpriteSheet spriteSheet =
            AnimatedCharactersFactory.LoadSpriteSheet(_graphicsDevice, "Content/SpriteSheets/Player.aseprite");
        AnimatedCharactersFactory animatedCharactersFactory = new();

        StateMachine<PlayerState, PlayerTrigger> stateMachine = new(PlayerState.Idle);

        AnimatedMovementComponent animatedMovementComponent = new(
            stateMachine,
            animatedCharactersFactory.CreateAnimations(spriteSheet, "Standing"),
            animatedCharactersFactory.CreateAnimations(spriteSheet, "Walking"));
        e.AddComponent(0, animatedMovementComponent);

        e.AddTransform(new Vector2(), new Vector2());
        e.AddRectangleCollision(new Rectangle(0, 0, 16, 16));
    }
}
