using LDtk;
using Scellecs.Morpeh;

namespace Entities.Factories;

// TODO: refactor to the right factory, so an entity is instantiated using IServiceContainer
public abstract class EntityFactory
{
    private readonly Dictionary<string, Func<Entity>> _entityCreators = new()
    {
        // { "Player", () => PlayerEntityFactory.CreateEntity(World.Default) },
        // { "Rock", () => RockEntityFactory.CreateEntity(World.Default) },
    };

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
