using System.Numerics;
using Components.Render;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Implementations.Camera;

public abstract class BaseCamera(SpriteBatch spriteBatch, Viewport viewport)
{
    protected readonly Viewport _viewport = viewport;

    protected void RenderCharacterAnimator(Entity e, Vector2 at)
    {
        if (!e.Has<CharacterAnimatorComponent>()) return;

        ref var animator = ref e.GetComponent<CharacterAnimatorComponent>();
        animator.Animation.Draw(spriteBatch, at);
    }

    protected void RenderSprite(Entity e, Vector2 at)
    {
        if (!e.Has<SpriteComponent>())
        {
            return;
        }

        ref var spriteComponent = ref e.GetComponent<SpriteComponent>();
        spriteComponent.Sprite.Draw(spriteBatch, at);
    }
}
