using Components.Sprites;
using Components.World;
using Entitas;
using Microsoft.Xna.Framework;

namespace Systems;

public class CreatePlayerEntitySystem : IInitializeSystem
{
    private readonly Contexts _contexts;
    private readonly AnimatedMovementComponent _animatedMovementComponent;
    private int _componentsCount = 0;

    public CreatePlayerEntitySystem(Contexts contexts, AnimatedMovementComponent animatedMovementComponent)
    {
        _contexts = contexts;
        _animatedMovementComponent = animatedMovementComponent;
    }

    public void Initialize()
    {
        GameEntity e = _contexts.game.CreateEntity();

        e.AddComponent(_componentsCount++, _animatedMovementComponent);
        // e.AddComponent(_componentsCount++, new PlayerComponent());
        // e.AddComponent(_componentsCount++, new MovableComponent());
        // e.AddComponent(_componentsCount++, new TransformComponent());
        // e.AddComponent(_componentsCount++, new RectangleCollisionComponent());

        e.isPlayer = true;
        e.isMovable = true;

        e.AddTransform(new Vector2(), new Vector2());
        e.AddRectangleCollision(new Rectangle(0, 0, 16, 16));
    }
}
