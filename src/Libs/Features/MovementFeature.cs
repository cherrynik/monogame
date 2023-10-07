using Systems;

namespace Features;

public sealed class MovementFeature : Entitas.Extended.Feature
{
    public MovementFeature(MovementSystem movementSystem,
        AnimatedMovementSystem animatedMovementSystem)
    {
        Add(movementSystem);
        Add(animatedMovementSystem);
    }
}
