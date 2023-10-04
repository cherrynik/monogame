using Systems;

namespace Features;

public sealed class MovementFeature : Entitas.Extended.Feature
{
    public MovementFeature(InputSystem inputSystem,
        MovementSystem movementSystem,
        AnimatedMovementSystem animatedMovementSystem,
        CameraFollowingSystem cameraFollowingSystem
    )
    {
        Add(inputSystem);
        Add(movementSystem);
        Add(animatedMovementSystem);
        // Add(cameraFollowingSystem);
    }
}
