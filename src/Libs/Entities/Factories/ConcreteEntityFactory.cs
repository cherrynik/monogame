using LDtk;
using Scellecs.Morpeh;

namespace Entities.Factories;

public abstract class ConcreteEntityFactory : IConcreteEntityFactory
{
    public Entity CreateEntity(World @in)
    {
        Entity e = @in.CreateEntity();

        AddTags(e);
        AddData(e);
        AddRender(e);

        return e;
    }

    public virtual Entity CreateEntity(EntityInstance entity, World @in) => this.CreateEntity(@in);

    protected abstract void AddTags(Entity e);

    protected abstract void AddData(Entity e);

    protected abstract void AddRender(Entity e);
}
