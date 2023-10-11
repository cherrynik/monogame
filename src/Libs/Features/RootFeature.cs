using Features.Debugging;

namespace Features;

public sealed class RootFeature : Entitas.Extended.Feature
{
    public RootFeature(WorldInitializeFeature worldInitializeFeature,
        InputFeature inputFeature,
        MovementFeature movementFeature,
        CameraFeature cameraFeature)
    {
        Add(worldInitializeFeature);
        Add(inputFeature);
        Add(movementFeature);
        Add(cameraFeature);
    }

    public RootFeature(DebugRootFeature debugRootFeature,
        WorldInitializeFeature worldInitializeFeature,
        InputFeature inputFeature,
        MovementFeature movementFeature,
        CameraFeature cameraFeature) : this(worldInitializeFeature, inputFeature, movementFeature, cameraFeature)
    {
        Add(debugRootFeature);
    }
}
