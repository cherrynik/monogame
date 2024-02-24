using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Systems.Debugging;
using Systems.Debugging.Render;

namespace GameDesktop;

public class DebugFeature : Feature
{
    public DebugFeature(World world,
        EntitiesList entitiesList,
        FrameCounter frameCounter,
        RenderFramesPerSec renderFramesPerSec, PivotRenderSystem pivotRenderSystem) : base(world)
    {
        Add(entitiesList);
        Add(frameCounter);
        Add(renderFramesPerSec);
    }
}
