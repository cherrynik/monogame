using Components.Data;
using Components.Render.Static;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Entities.Factories.Items;

public class RockEntityFactory(
    ItemComponent itemComponent,
    TransformComponent transformComponent) : EntityFactory
{
    private readonly SpriteComponent _spriteComponent;

    public RockEntityFactory(ItemComponent itemComponent, TransformComponent transformComponent,
        SpriteComponent spriteComponent) : this(itemComponent, transformComponent)
    {
        _spriteComponent = spriteComponent;
    }

    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(itemComponent);
        e.AddComponent(transformComponent);
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(_spriteComponent);
    }
}
