using Components.Data;
using Components.Render.Static;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Entities.Factories.Characters;

public class DummyEntityFactory : EntityFactory
{
    private readonly TransformComponent _transform;
    private readonly SpriteComponent _sprite;
    private readonly RectangleCollisionComponent _rectangleCollision;

    public DummyEntityFactory(TransformComponent transform,
        SpriteComponent sprite,
        RectangleCollisionComponent rectangleCollision)
    {
        _transform = transform;
        _sprite = sprite;
        _rectangleCollision = rectangleCollision;
    }

    protected override void AddTags(Entity e)
    {
    }

    protected override void AddData(Entity e)
    {
        e.AddComponent(_transform);
        e.AddComponent(_rectangleCollision);
    }

    protected override void AddRender(Entity e)
    {
        e.AddComponent(_sprite);
    }
}
