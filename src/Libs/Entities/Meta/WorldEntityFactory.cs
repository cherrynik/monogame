using Components.Data;
using Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Meta;

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
