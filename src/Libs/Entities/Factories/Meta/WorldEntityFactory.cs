using Components.Data;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Entities.Factories.Meta;

public class WorldEntityFactory(WorldMetaComponent worldMetaComponent) : EntityFactory
{
    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(worldMetaComponent);
    }

    protected override void AddRender(Entity e)
    {
    }
}
