using Components;
using Components.World;

namespace Features.Factories;

public class Player
{
    private readonly GameEntity _entity;

    public Player(Contexts contexts,
        MovementAnimationComponent movementAnimationComponent,
        TransformComponent transformComponent,
        CameraComponent cameraComponent)
    {
        _entity = contexts.game.CreateEntity();

        _entity.isPlayer = true;
        _entity.isMovable = true;
        _entity.AddMovementAnimation(movementAnimationComponent);
        _entity.AddTransform(transformComponent);
        _entity.AddCamera(cameraComponent);
        // e.AddRectangleCollision(new Rectangle(0, 0, 16, 16));
    }
}
