using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Systems.Render;

public sealed class FollowingCamera : BaseCamera, ICamera
{
    private Vector2 _position;
    private readonly World _world;

    public FollowingCamera(World world, SpriteBatch spriteBatch, Viewport viewport, Vector2 position) : base(spriteBatch, viewport)
    {
        _world = world;
        _position = position;
    }

    public void Render(Entity entity)
    {
        var transformStash = _world.GetStash<TransformComponent>();
        if (!transformStash.Has(entity))
        {
            return;
        }

        ref var transform = ref transformStash.Get(entity);
        Vector2 position = transform.Position;
        var cameraStash = _world.GetStash<CameraComponent>();

        if (cameraStash.Has(entity))
        {
        }

        Vector2 relativePosition = position - _position;
        var characterAnimatorStash = _world.GetStash<CharacterAnimatorComponent>();
        var spriteStash = _world.GetStash<SpriteComponent>();

        RenderCharacterAnimator(entity, relativePosition, characterAnimatorStash);
        RenderSprite(entity, relativePosition, spriteStash);
    }
}
