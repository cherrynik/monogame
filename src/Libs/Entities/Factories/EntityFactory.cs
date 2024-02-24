using Scellecs.Morpeh;

namespace Entities.Factories;

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
