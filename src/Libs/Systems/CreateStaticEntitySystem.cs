using Components;
using Components.World;
using Entitas;

namespace Systems;

public class CreateStaticEntitySystem : ISystem
{
    private readonly Contexts _contexts;

    public CreateStaticEntitySystem(Contexts contexts, TransformComponent transformComponent,
        SpriteComponent spriteComponent)
    {
        _contexts = contexts;

        CreateEntity(transformComponent, spriteComponent);
    }

    private void CreateEntity(TransformComponent transformComponent, SpriteComponent spriteComponent)
    {
        var e = _contexts.game.CreateEntity();

        e.AddTransform(transformComponent);
        e.AddSprite(spriteComponent);
    }
}
