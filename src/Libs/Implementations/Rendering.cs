using System.Numerics;
using Components.Data;
using Components.Render;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Implementations;

public class Rendering(SpriteBatch spriteBatch)
{
    public void RenderEntities(IEnumerable<Entity> entities, CameraComponent camera)
    {
        foreach (Entity e in entities)
        {
            ref var transform = ref e.GetComponent<TransformComponent>();
            var at = camera.WorldToScreen(transform.Position);

            RenderComponent<CharacterAnimatorComponent>(e, transform, at);
            RenderComponent<SpriteComponent>(e, transform, at);
        }
    }

    private void RenderComponent<T>(Entity e, TransformComponent parentTransform, Vector2 at)
        where T : struct, IRenderComponent
    {
        if (!e.Has<T>()) return;

        var renderComponent = e.GetComponent<T>();
        renderComponent.Draw(spriteBatch, GetWorldPosition(renderComponent, parentTransform, at));
    }

    private static Vector2 GetWorldPosition(IRenderComponent renderComponent, TransformComponent parentTransform,
        Vector2 at)
    {
        var pivotOffset = renderComponent.GetOffPivot(parentTransform);
        var localOffset = renderComponent.LocalTransform.Position;
        return at + localOffset - pivotOffset;
    }
}
