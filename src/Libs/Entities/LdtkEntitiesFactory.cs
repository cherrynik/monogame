using Entities;
using Entities.Items.Rocks;
using Entities.Items.Trees;
using LDtk;
using LightInject;
using Scellecs.Morpeh;
using Serilog;

namespace Entities;

public class LdtkEntitiesFactory(IServiceFactory serviceFactory) : IAbstractEntityFactory
{
    private readonly ILogger _logger = Log.Logger.ForContext<LdtkEntitiesFactory>();

    private readonly Dictionary<string, IAbstractEntityFactory> _abstractFactories = new()
    {
        { "Rock", serviceFactory.GetInstance<AbstractRockFactory>() },
        { "Tree", serviceFactory.GetInstance<AbstractTreeFactory>() },
    };

    private readonly Dictionary<string, EntityFactory> _concreteFactories = new()
    {
        { "Player", serviceFactory.GetInstance<PlayerFactory>() },
    };

    private static readonly HashSet<string> UnsupportedIdentifiers = new();

    public Entity? CreateEntity(EntityInstance entity, World @in)
    {
        var entityByIdentifier = CreateEntity(entity._Identifier, @in);

        if (entityByIdentifier is not null) return entityByIdentifier;

        // In Ldtk, there are many entities tagged with 'rock', but what a rock that is, you know by the identifier.
        var entityByAnyTag = entity._Tags
            .Select(tag => _abstractFactories.TryGetValue(tag, out var @abstract)
                ? @abstract.CreateEntity(entity._Identifier, @in)
                : CreateEntity(tag, @in))
            .OfType<Entity>()
            .SingleOrDefault();

        if (entityByAnyTag is null && !UnsupportedIdentifiers.Contains(entity._Identifier))
        {
            _logger.Warning(
                $"⚠️ No Factory. Name: {entity._Identifier}, tags: {String.Join(value: entity._Tags, separator: ", ")}");
            UnsupportedIdentifiers.Add(entity._Identifier);
        }

        return entityByAnyTag;
    }

    public Entity? CreateEntity(string tag, World @in)
    {
        if (_abstractFactories.TryGetValue(tag, out var @abstract))
            return @abstract.CreateEntity(tag, @in);

        return _concreteFactories.TryGetValue(tag, out var factory) ? factory.CreateEntity(@in) : null;
    }
}
