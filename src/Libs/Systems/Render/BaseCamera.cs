using System.Numerics;
using Components.Render.Animation;
using Components.Render.Static;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Systems.Render;

public abstract class BaseCamera
{
    private readonly SpriteBatch _spriteBatch;
    protected readonly Viewport Viewport;

    protected BaseCamera(SpriteBatch spriteBatch, Viewport viewport)
    {
        _spriteBatch = spriteBatch;
        Viewport = viewport;
    }

    protected void RenderCharacterAnimator(Entity entity, Vector2 position, Stash<CharacterAnimatorComponent> characterAnimatorStash)
    {
        if (!characterAnimatorStash.Has(entity))
        {
            return;
        }

        ref var animator = ref characterAnimatorStash.Get(entity);
        animator.Animation.Draw(_spriteBatch, position);
    }

    protected void RenderSprite(Entity entity, Vector2 position, Stash<SpriteComponent> spriteStash)
    {
        if (!spriteStash.Has(entity))
        {
            return;
        }

        ref var spriteComponent = ref spriteStash.Get(entity);
        spriteComponent.Sprite.Draw(_spriteBatch, position);
    }
}
