using Entitas.Components;
using Entitas.Components.Data;

namespace Entitas.Entities;

public class Player
{
    public Player(Contexts contexts,
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
