using LDtk;
using LightInject;
using Scellecs.Morpeh;

namespace Entities.Items.Rocks;

public class AbstractRockFactory(IServiceFactory serviceFactory) : IAbstractEntityFactory
{
    private readonly Dictionary<string, EntityFactory> _factories = new()
    {
        { "Pebble", serviceFactory.GetInstance<PebbleFactory>() },
    };

    public Entity? CreateEntity(string tag, World @in) =>
        _factories.TryGetValue(tag, out var factory)
            ? factory.CreateEntity(@in)
            : null;
}
