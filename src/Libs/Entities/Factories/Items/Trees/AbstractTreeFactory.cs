using LightInject;
using Scellecs.Morpeh;

namespace Entities.Factories.Items.Trees;

public class AbstractTreeFactory(IServiceFactory serviceFactory) : IAbstractEntityFactory
{
    private readonly Dictionary<string, EntityFactory> _factories = new()
    {
        { "Tree", serviceFactory.GetInstance<TreeFactory>() },
    };

    public Entity? CreateEntity(string tag, World @in) =>
        _factories.TryGetValue(tag, out var factory)
            ? factory.CreateEntity(@in)
            : null;
}
