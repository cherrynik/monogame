using Systems;

namespace Features;

public sealed class MovementFeature : Entitas.Extended.Feature
{
    public MovementFeature(
        CollisionSystem collisionSystem,
        MovementSystem movementSystem,
        AnimatedMovementSystem animatedMovementSystem)
    {
        Add(collisionSystem);
        Add(movementSystem);
        Add(animatedMovementSystem);
    }
}
