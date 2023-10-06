using Entitas.Extended;

namespace Features;

public sealed class CameraFeature : Entitas.Extended.Feature
{
    public CameraFeature(IDrawSystem cameraFollowingSystem)
    {
        Add(cameraFollowingSystem);
    }
}
