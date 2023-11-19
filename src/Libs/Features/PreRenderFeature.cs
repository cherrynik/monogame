using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Systems.Render;

namespace Features;

public class PreRenderFeature : Feature
{
    public PreRenderFeature(World world,
        CharacterMovementAnimationSystem characterMovementAnimationSystem,
        CameraFollowingSystem cameraFollowingSystem) : base(world)
    {
        Add(characterMovementAnimationSystem);
        Add(cameraFollowingSystem);
    }
}
