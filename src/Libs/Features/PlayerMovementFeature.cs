using Systems;

namespace Features;

public class PlayerMovementFeature : Entitas.Extended.Feature
{
    public PlayerMovementFeature(InputSystem inputSystem,
        MovementSystem movementSystem,
        AnimatedMovementSystem animatedMovementSystem)
    {
        Add(inputSystem);
        Add(movementSystem);
        Add(animatedMovementSystem);
    }
}
