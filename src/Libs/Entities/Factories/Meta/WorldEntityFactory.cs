using Components.Data;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Entities.Factories.Meta;

public class WorldEntityFactory : EntityFactory
{
    private readonly WorldComponent _worldComponent;

    public WorldEntityFactory(WorldComponent worldComponent)
    {
        _worldComponent = worldComponent;
    }

    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(_worldComponent);
    }

    protected override void AddRender(Entity e)
    {
    }
}
