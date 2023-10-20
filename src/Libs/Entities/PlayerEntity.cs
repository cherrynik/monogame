using Components;
using Components.Data;

namespace Entities;

public class PlayerEntity
{
    public PlayerEntity(Contexts contexts,
        MovementAnimationComponent movementAnimationComponent,
        TransformComponent transformComponent,
        CameraComponent cameraComponent,
        RectangleCollisionComponent rectangleCollisionComponent)
    {
        GameEntity entity = contexts.game.CreateEntity();

        entity.isPlayer = true;
        entity.isMovable = true;
        entity.AddMovementAnimation(movementAnimationComponent);
        entity.AddTransform(transformComponent);
        entity.AddCamera(cameraComponent);
        entity.AddRectangleCollision(rectangleCollisionComponent);
    }
}
