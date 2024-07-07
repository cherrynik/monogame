using System.Numerics;
using Components.Data;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Implementations.Camera;

public class LinearlyCameraFollowing(SpriteBatch spriteBatch, Viewport viewport) // , Vector2 position)
    : BaseCamera(spriteBatch, viewport), ICameraFollowing
{
    public void Render(Entity e)
    {
        if (!e.Has<TransformComponent>())
        {
            return;
        }

        ref var transform = ref e.GetComponent<TransformComponent>();
        Vector2 position1 = transform.Position;

        if (e.Has<CameraComponent>())
        {
            // _position = GetCenteredPosition(off: position);
        }

        // Vector2 relativePosition = position1 - position;
        //
        // base.RenderCharacterAnimator(e, at: relativePosition);
        // base.RenderSprite(e, at: relativePosition);
    }

    public Vector2 Move(Vector2 from, Vector2 to, float step) => Vector2.Lerp(from, to, step);
}
