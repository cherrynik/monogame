using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Systems.Render;

public sealed class StaticCamera : BaseCamera, ICamera
{
    private readonly World _world;

    public StaticCamera(World world, SpriteBatch spriteBatch, Viewport viewport) : base(spriteBatch, viewport)
    {
        _world = world;
    }

    public void Render(Entity entity)
    {
        var transformStash = _world.GetStash<TransformComponent>();
        if (!transformStash.Has(entity))
        {
            return;
        }

        ref var transform = ref transformStash.Get(entity);
        var characterAnimatorStash = _world.GetStash<CharacterAnimatorComponent>();
        var spriteStash = _world.GetStash<SpriteComponent>();

        RenderCharacterAnimator(entity, transform.Position, characterAnimatorStash);
        RenderSprite(entity, transform.Position, spriteStash);
    }
}
