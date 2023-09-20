using Components;
using Components.World;
using Entitas;

namespace Systems;

public class CreatePlayerEntitySystem : ISystem
{
    private readonly Contexts _contexts;

    public CreatePlayerEntitySystem(Contexts contexts,
        MovementAnimationComponent movementAnimationComponent,
        TransformComponent transformComponent)
    {
        _contexts = contexts;
        CreateEntity(movementAnimationComponent, transformComponent);
    }

    private void CreateEntity(MovementAnimationComponent movementAnimationComponent,
        TransformComponent transformComponent)
    {
        GameEntity e = _contexts.game.CreateEntity();

        e.isPlayer = true;
        e.isMovable = true;
        e.AddMovementAnimation(movementAnimationComponent);
        e.AddTransform(transformComponent);

        // e.AddRectangleCollision(new Rectangle(0, 0, 16, 16));
    }
}
