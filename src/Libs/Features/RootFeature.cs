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
}
