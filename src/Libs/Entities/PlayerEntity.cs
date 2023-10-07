using Components;
using Components.World;

namespace Entities;

public class PlayerEntity
{
    public PlayerEntity(Contexts contexts,
        MovementAnimationComponent movementAnimationComponent,
        TransformComponent transformComponent,
        CameraComponent cameraComponent)
    {
        GameEntity entity = contexts.game.CreateEntity();

        entity.isPlayer = true;
        entity.isMovable = true;
        entity.AddMovementAnimation(movementAnimationComponent);
        entity.AddTransform(transformComponent);
        entity.AddCamera(cameraComponent);
        // e.AddRectangleCollision(new Rectangle(0, 0, 16, 16));
    }
}
