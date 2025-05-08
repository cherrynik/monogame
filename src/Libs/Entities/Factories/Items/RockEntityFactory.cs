using Components.Data;
using Components.Render.Static;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Factories.Items;

public class RockEntityFactory(
    NameComponent nameComponent,
    ItemComponent itemComponent,
    TransformComponent transformComponent) : EntityFactory
{
    private readonly SpriteComponent _spriteComponent;

    public RockEntityFactory(NameComponent nameComponent, ItemComponent itemComponent,
        TransformComponent transformComponent,
        SpriteComponent spriteComponent) : this(nameComponent, itemComponent, transformComponent)
    {
        _spriteComponent = spriteComponent;
    }

    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(nameComponent);
        e.AddComponent(itemComponent);
        e.AddComponent(transformComponent);
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(_spriteComponent);
    }
}
