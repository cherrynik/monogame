using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Systems;

namespace Features;

public class RootFeature : Feature
{
    public RootFeature(World world,
        SystemsEngine systemsEngine,
        WorldInitializer worldInitializer
    ) : base(world, systemsEngine)
    {
        Add(worldInitializer);
    }
}
