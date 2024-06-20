using Entities.Factories.Characters;
using Entities.Factories.Items.Rocks;
using LDtk;
using LightInject;
using Scellecs.Morpeh;

namespace Entities.Factories;

public class AbstractEntityFactory(IServiceFactory serviceFactory) : IAbstractEntityFactory
{
    private readonly Dictionary<string, IAbstractEntityFactory> _factories = new()
    {
        // { "Tree", null },
        { "Player", serviceFactory.GetInstance<PlayerFactory>() },
        { "Rock", serviceFactory.GetInstance<AbstractRockFactory>() },
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
