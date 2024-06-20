using Entities.Factories.Items.Rocks;
using LDtk;
using LightInject;
using Scellecs.Morpeh;

namespace Entities.Factories.Items.Trees;

public class AbstractTreeFactory(IServiceFactory serviceFactory) : IAbstractEntityFactory
{
    private readonly Dictionary<string, ConcreteEntityFactory> _factories = new()
    {
        { "Tree", serviceFactory.GetInstance<TreeFactory>() },
    };

    public Entity? CreateEntity(EntityInstance entity, World @in) =>
        _factories.TryGetValue(entity._Identifier, out var factory)
            ? factory.CreateEntity(@in)
            : null;
}
