using Components.Data;
using LDtk;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Factories;

// TODO: refactor to the right factory, so an entity is instantiated using IServiceContainer
public interface IEntityFactory
{
    Entity CreateEntity(World @in);
}

public interface IAbstractEntityFactory
{
    Entity CreateEntity(EntityInstance entity, World @in);
}

public class AbstractEntityFactory(IServiceFactory serviceFactory) : IAbstractEntityFactory
{
    private readonly Dictionary<string, IAbstractEntityFactory> _factories = new()
    {
        // { "Tree", () => (IEntityFactory)serviceProvider.GetService(typeof(RockFactory)) },
        { "Rock", serviceFactory.GetInstance<RockFactory>() }, { "Default", null }
    };

    public Entity CreateEntity(EntityInstance entity, World @in)
    {
        foreach (var tag in entity._Tags)
        {
            if (_factories.TryGetValue(tag, out var factory))
            {
                return factory.CreateEntity(entity, @in);
            }
        }

        // throw new ArgumentException($"{entity.Iid} has an unknown tag: {(string.Join(", ", entity._Tags))}");
        // return _factories["Default"].CreateEntity(entity, @in);
        return null;
    }
}

public class PebbleFactory(IServiceFactory serviceProvider) : EntityFactory
{
    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(serviceProvider.GetInstance<string, NameComponent>("Pebble"));
        e.AddComponent(serviceProvider.GetInstance<TransformComponent>());
    }

    protected override void AddRender(Entity e)
    {
    }
}

public class RockFactory(IServiceFactory serviceProvider, World @in) : IAbstractEntityFactory
{
    private readonly Dictionary<string, Func<Entity>> _createRock = new()
    {
        { "Pebble", () => serviceProvider.GetInstance<PebbleFactory>().CreateEntity(@in) },
    };

    public Entity CreateEntity(EntityInstance entity, World @in)
    {
        if (_createRock.TryGetValue(entity._Identifier, out var createRock))
        {
            return createRock();
        }

        // throw new ArgumentException($"{entity.Iid} has an unknown tag: {entity._Tags}");
        return null;
    }
}

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
