using Entitas;

namespace Systems;

public class CreatePlayerEntitySystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    public CreatePlayerEntitySystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        GameEntity? e = _contexts.game.CreateEntity();

        e.isPlayer = true;
        e.isMovable = true;

        // FIXME: Trouble loading.
        // SpriteSheet spriteSheet =
        //     AnimatedCharactersFactory.LoadSpriteSheet(_graphicsDevice, "Content/SpriteSheets/Player.aseprite");
        // AnimatedCharactersFactory animatedCharactersFactory = new();
        //
        // StateMachine<PlayerState, PlayerTrigger> stateMachine = new(PlayerState.Idle);
        //
        // AnimatedMovementComponent animatedMovementComponent = new(
        //     stateMachine,
        //     animatedCharactersFactory.CreateAnimations(spriteSheet, "Standing"),
        //     animatedCharactersFactory.CreateAnimations(spriteSheet, "Walking"));
        // e.AddComponent(0, animatedMovementComponent);
        //
        // e.AddTransform(new Vector2(), new Vector2());
        // e.AddRectangleCollision(new Rectangle(0, 0, 16, 16));
    }
}
