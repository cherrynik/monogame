using Components.Data;
using Components.Render.Animation;
using Components.Tags;
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
    Entity? CreateEntity(EntityInstance entity, World @in);
}

public class AbstractEntityFactory(IServiceFactory serviceFactory) : IAbstractEntityFactory
{
    private readonly Dictionary<string, IAbstractEntityFactory> _factories = new()
    {
        // { "Tree", null },
        { "Player", serviceFactory.GetInstance<PlayerFactory>() },
        { "Rock", serviceFactory.GetInstance<RockFactory>() },
        // { "Default", null }
    };

    public Entity? CreateEntity(EntityInstance entity, World @in)
    {
        {
            if (_factories.TryGetValue(entity._Identifier, out var factory))
            {
                return factory.CreateEntity(entity, @in);
            }
        }

        foreach (var tag in entity._Tags)
        {
            if (_factories.TryGetValue(tag, out var factory))
            {
                return factory.CreateEntity(entity, @in);
            }
        }

        return null;
        // throw new ArgumentException(
        // $"{entity._Identifier} ({entity.Iid}) has unknown tag(-s): {(string.Join(", ", entity._Tags))}");
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

public class RockFactory(IServiceFactory serviceFactory, World @in) : IAbstractEntityFactory
{
    private readonly Dictionary<string, Func<Entity>> _createRock = new()
    {
        { "Pebble", () => serviceFactory.GetInstance<PebbleFactory>().CreateEntity(@in) },
    };

    public Entity? CreateEntity(EntityInstance entity, World @in)
    {
        if (_createRock.TryGetValue(entity._Identifier, out var createRock))
        {
            return createRock();
        }

        // throw new ArgumentException($"{entity.Iid} has an unknown tag: {entity._Tags}");
        return null;
    }
}

public class PlayerFactory(IServiceFactory serviceFactory) : EntityFactory, IAbstractEntityFactory
{
    public Entity CreateEntity(EntityInstance entity, World @in) => this.CreateEntity(@in);

    protected override void AddTags(Entity e)
    {
        e.AddComponent(serviceFactory.GetInstance<CameraComponent>());
        e.AddComponent(serviceFactory.GetInstance<InputMovableComponent>());
        e.AddComponent(serviceFactory.GetInstance<MovableComponent>());
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(serviceFactory.GetInstance<string, NameComponent>("Player"));
        e.AddComponent(serviceFactory.GetInstance<TransformComponent>("PlayerEntity"));
        e.AddComponent(serviceFactory.GetInstance<RectangleColliderComponent>("PlayerEntity"));
        e.AddComponent(serviceFactory.GetInstance<InventoryComponent>());
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(serviceFactory.GetInstance<MovementAnimationsComponent>("PlayerEntity"));
        e.AddComponent(serviceFactory.GetInstance<CharacterAnimatorComponent>("PlayerEntity"));
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
