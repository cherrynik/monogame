using Entitas.Components;
using Entitas.Components.Data;
using Microsoft.Xna.Framework;

namespace Entitas.Entities;

public class StaticEntity
{
    public StaticEntity(Contexts contexts,
        TransformComponent transformComponent,
        SpriteComponent spriteComponent,
        RectangleCollisionComponent rectangleCollision)
    {
        GameEntity e = contexts.game.CreateEntity();

        e.AddTransform(transformComponent);
        e.AddSprite(spriteComponent);
        e.AddRectangleCollision(rectangleCollision);
    }
}
