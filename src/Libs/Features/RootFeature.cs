using Systems;

namespace Features;

public sealed class RootFeature : Entitas.Extended.Feature
{
    public RootFeature(InputSystem inputSystem,
        MovementSystem movementSystem,
        CreatePlayerEntitySystem createPlayerEntitySystem,
        AnimatedMovementSystem animatedMovementSystem,
        CameraFollowingSystem cameraFollowingSystem,
        CreateStaticEntitySystem createStaticEntitySystem
    )
    {
        Add(inputSystem);
        Add(movementSystem);
        Add(createPlayerEntitySystem);
        Add(animatedMovementSystem);
        Add(cameraFollowingSystem);
        Add(createStaticEntitySystem);
    }
}
