using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Factories;

// TODO: refactor to the right factory, so an entity is instantiated using IServiceContainer
public abstract class EntityFactory
{
    public Entity CreateEntity(World @in)
    {
        Entity e = @in.CreateEntity();
        
        AddTags(e);
        AddData(e);
        AddRender(e);

        return e;
    }

    protected abstract void AddTags(Entity e);
    protected abstract void AddData(Entity e);
    protected abstract void AddRender(Entity e);
}
