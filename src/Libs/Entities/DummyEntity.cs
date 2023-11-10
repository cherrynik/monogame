namespace Entities;

using Components.Data;
using Components.Render.Static;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

public class DummyEntity
{
    private readonly TransformComponent _transform;
    private readonly SpriteComponent _sprite;
    private readonly RectangleCollisionComponent _rectangleCollision;

    public DummyEntity(TransformComponent transform,
        SpriteComponent sprite,
        RectangleCollisionComponent rectangleCollision)
    {
        _transform = transform;
        _sprite = sprite;
        _rectangleCollision = rectangleCollision;
    }

    public Entity Create(World @in)
    {
        Entity e = @in.CreateEntity();

        AddTags(e);
        AddData(e);
        AddRender(e);

        return e;
    }

    private void AddTags(Entity e)
    {
    }

    private void AddData(Entity e)
    {
        e.AddComponent(_transform);
        e.AddComponent(_rectangleCollision);
    }

    private void AddRender(Entity e)
    {
        e.AddComponent(_sprite);
    }
}
