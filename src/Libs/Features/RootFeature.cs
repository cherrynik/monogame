using Components;
using Components.World;
using Features.Factories;
using Systems;

namespace Features;

public sealed class RootFeature : Entitas.Extended.Feature
{
    public RootFeature(
        WorldInitializeFeature worldInitializeFeature,
        InputFeature inputFeature,
        MovementFeature movementFeature,
        CreatePlayerEntitySystem createPlayerEntitySystem,
        // CreateStaticEntitySystem createStaticEntitySystem
        // MovementSystem movementSystem,
        AnimatedMovementSystem animatedMovementSystem,
        CameraFollowingSystem cameraFollowingSystem
    )
    {
        // Add(createStaticEntitySystem);
        Add(worldInitializeFeature);
        Add(cameraFollowingSystem);
        Add(createPlayerEntitySystem);
        Add(inputFeature);
        Add(movementFeature);

        // Add(animatedMovementSystem);
    }
}
