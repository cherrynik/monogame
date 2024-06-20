using LDtk;
using LightInject;
using Scellecs.Morpeh;

namespace Entities.Factories.Items.Rocks;

public class AbstractRockFactory(IServiceFactory serviceFactory) : IAbstractEntityFactory
{
    private readonly Dictionary<string, ConcreteEntityFactory> _factories = new()
    {
        { "Pebble", serviceFactory.GetInstance<PebbleFactory>() },
    };

    public Entity? CreateEntity(EntityInstance entity, World @in) =>
        _factories.TryGetValue(entity._Identifier, out var factory)
            ? factory.CreateEntity(@in)
            : null;
}

// public class RockEntityFactory(
//     NameComponent nameComponent,
//     ItemComponent itemComponent,
//     TransformComponent transformComponent) : EntityFactory
// {
//     private readonly SpriteComponent _spriteComponent;
//
//     public RockEntityFactory(NameComponent nameComponent, ItemComponent itemComponent,
//         TransformComponent transformComponent,
//         SpriteComponent spriteComponent) : this(nameComponent, itemComponent, transformComponent)
//     {
//         _spriteComponent = spriteComponent;
//     }
//
//     protected override void AddTags(Entity e)
//     {
//     }
//
//     protected override void AddData(Entity e)
//     {
//         e.AddComponent(nameComponent);
//         e.AddComponent(itemComponent);
//         e.AddComponent(transformComponent);
//     }
//
//     protected override void AddRender(Entity e)
//     {
//         e.AddComponent(_spriteComponent);
//     }
// }
