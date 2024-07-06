using LDtk;
using Scellecs.Morpeh;

namespace Entities;

public abstract class EntityFactory : IEntityFactory
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
