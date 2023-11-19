using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Systems;

namespace Features;

public class MovementFeature : Feature
{
    public MovementFeature(World world,
        InputSystem inputSystem,
        MovementSystem movementSystem) : base(world)
    {
        Add(inputSystem);
        Add(movementSystem);
    }
}
