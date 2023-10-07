using Components;
using Components.World;

namespace Entities;

public class StaticEntity
{
    public StaticEntity(Contexts contexts,
        TransformComponent transformComponent,
        SpriteComponent spriteComponent)
    {
        GameEntity e = contexts.game.CreateEntity();

        e.AddTransform(transformComponent);
        e.AddSprite(spriteComponent);
    }
}
