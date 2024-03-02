using Components.Data;
using Components.Render.Static;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended.Extensions;

namespace Entities.Factories.Characters;

public class DummyEntityFactory(
    NameComponent name,
    TransformComponent transform,
    RectangleColliderComponent rectangleCollider)
    : EntityFactory
{
    private readonly SpriteComponent _sprite;

    public DummyEntityFactory(
        NameComponent name,
        TransformComponent transform,
        SpriteComponent sprite,
        RectangleColliderComponent rectangleCollider) : this(name, transform, rectangleCollider)
    {
        _sprite = sprite;
    }

    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(name);
        e.AddComponent(transform);
        e.AddComponent(rectangleCollider);
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(_sprite);
    }
}
