using Entitas;
using Services.Input;
using Services.Movement;
using Systems;
using Systems.Input;
using Systems.Sprites;

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
