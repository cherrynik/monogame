using Components.Data;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Implementations.Camera;

public class StaticCamera(SpriteBatch spriteBatch, Viewport viewport) : BaseCamera(spriteBatch, viewport), ICamera
{
    public void Render(Entity e)
    {
        if (!e.Has<TransformComponent>())
        {
            return;
        }

        ref var transform = ref e.GetComponent<TransformComponent>();

        base.RenderCharacterAnimator(e, at: transform.Position);
        base.RenderSprite(e, at: transform.Position);
    }
}
