using Systems.Debugging;

namespace Features.Debugging;

public sealed class DebugRootFeature : Entitas.Extended.Feature
{
    public DebugRootFeature(DrawRectangleCollisionComponentsSystem drawRectangleCollisionComponentsSystem)
    {
        Add(drawRectangleCollisionComponentsSystem);
    }
}
