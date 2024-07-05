using Entities.Factories.Characters;
using Entities.Factories.Items.Rocks;
using Entities.Factories.Items.Trees;
using LDtk;
using LightInject;
using Scellecs.Morpeh;

namespace Entities.Factories;

public class EntitiesFactory(IServiceFactory serviceFactory) : IAbstractEntityFactory
{
    private readonly Dictionary<string, IAbstractEntityFactory> _abstractFactories = new()
    {
        { "Rock", serviceFactory.GetInstance<AbstractRockFactory>() },
        { "Tree", serviceFactory.GetInstance<AbstractTreeFactory>() },
        // { "Default", null }
    };

    private readonly Dictionary<string, EntityFactory> _concreteFactories = new()
    {
        { "Player", serviceFactory.GetInstance<PlayerFactory>() },
        // { "Default", null }
    };

    public Entity? CreateEntity(EntityInstance entity, World @in) => CreateEntity(entity._Identifier, @in) ??
                                                                     entity._Tags
                                                                         .Select(tag =>
                                                                             CreateEntity(entity._Identifier, @in))
                                                                         .OfType<Entity>().FirstOrDefault();

    public Entity? CreateEntity(string tag, World @in)
    {
        if (_abstractFactories.TryGetValue(tag, out var @abstract)) return @abstract.CreateEntity(tag, @in);

        if (_concreteFactories.TryGetValue(tag, out var concrete)) return concrete.CreateEntity(@in);

        return null;
    }
}
