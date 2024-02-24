using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Systems.Render;

namespace Features;

public class RenderFeature : Feature
{
    public RenderFeature(World world,
        RenderCharacterMovementAnimationSystem renderCharacterMovementAnimationSystem) : base(world)
    {
        Add(renderCharacterMovementAnimationSystem);
    }
}
