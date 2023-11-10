using Components.Data;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Entities;

public class WorldEntity
{
    private readonly WorldComponent _worldComponent;

    public WorldEntity(WorldComponent worldComponent)
    {
        _worldComponent = worldComponent;
    }

    public Entity Create(World @in)
    {
        Entity e = @in.CreateEntity();

        e.AddComponent(_worldComponent);

        return e;
    }
}
