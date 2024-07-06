using Entities.Items.Trees;
using LightInject;
using Scellecs.Morpeh;

namespace Entities.Items.Trees;

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
