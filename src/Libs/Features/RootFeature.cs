using GameDesktop;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Features;

public class RootFeature : Feature
{
    public RootFeature(World world,
        WorldInitializer worldInitializer,
        MovementFeature movementFeature
    ) : base(world)
    {
        Add(worldInitializer);
        Add(movementFeature);
    }

    public RootFeature(World world,
        WorldInitializer worldInitializer,
        MovementFeature movementFeature,
        PreRenderFeature preRenderFeature,
        RenderFeature renderFeature
    ) : this(world, worldInitializer, movementFeature)
    {
        Add(preRenderFeature);
        Add(renderFeature);
    }

#if DEBUG
    public RootFeature(World world,
        WorldInitializer worldInitializer,
        MovementFeature movementFeature,
        PreRenderFeature preRenderFeature,
        RenderFeature renderFeature,
        DebugFeature debugFeature
    ) : this(world, worldInitializer, movementFeature, preRenderFeature, renderFeature)
    {
        Add(debugFeature);
    }
#endif
}
