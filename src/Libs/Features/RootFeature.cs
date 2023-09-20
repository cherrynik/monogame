using Systems;

namespace Features;

public sealed class RootFeature : Entitas.Extended.Feature
{
    public RootFeature(InputSystem inputSystem,
        MovementSystem movementSystem,
        CreatePlayerEntitySystem createPlayerEntitySystem,
        AnimatedMovementSystem animatedMovementSystem
    )
    {
        Add(inputSystem);
        Add(movementSystem);
        Add(createPlayerEntitySystem);
        Add(animatedMovementSystem);
    }
}
