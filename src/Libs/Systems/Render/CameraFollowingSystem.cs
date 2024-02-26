using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Systems.Render;

public class CameraFollowingSystem(World world) : ILateSystem
{
    // private readonly ICamera _camera;
    public World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        // TODO: Components in the range of visibility (world grid system?)
        // Filter filter = World.Filter.With<TransformComponent>().Build();
        // IEnumerable<Entity> entities = SortEntitiesByYPosition(filter);
        //
        // foreach (Entity e in entities)
        // {
        //     _camera.Render(e);
        // }

        Entity e = World.Filter.With<CameraComponent>().Build().First();

        ref var transform = ref e.GetComponent<TransformComponent>();
        ref var camera = ref e.GetComponent<CameraComponent>();

        camera.Position = GetCenteredPosition(camera.Viewport, off: transform.Position);
    }

    private static Vector2 GetCenteredPosition(Viewport viewport, Vector2 off) => new(
        off.X - viewport.Width / 2,
        off.Y - viewport.Height / 2);

    private static IEnumerable<Entity> SortEntitiesByYPosition(Filter filter)
    {
        List<Entity> entities = new List<Entity>();

        foreach (Entity e in filter)
        {
            entities.Add(e);
        }

        return entities.OrderBy(x =>
        {
            ref var transform = ref x.GetComponent<TransformComponent>();
            return transform.Position.Y;
        });
    }

    public void Dispose()
    {
    }
}

public interface ICamera
{
    void Render(Entity e);
}

public class StaticCamera : BaseCamera, ICamera
{
    public StaticCamera(SpriteBatch spriteBatch, Viewport viewport) : base(spriteBatch, viewport)
    {
    }

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

public class FollowingCamera : BaseCamera, ICamera
{
    private Vector2 _position;

    public FollowingCamera(SpriteBatch spriteBatch, Viewport viewport, Vector2 position) : base(spriteBatch, viewport)
    {
        _position = position;
    }

    public void Render(Entity e)
    {
        if (!e.Has<TransformComponent>())
        {
            return;
        }

        ref var transform = ref e.GetComponent<TransformComponent>();
        Vector2 position = transform.Position;

        if (e.Has<CameraComponent>())
        {
            // _position = GetCenteredPosition(off: position);
        }

        Vector2 relativePosition = position - _position;

        base.RenderCharacterAnimator(e, at: relativePosition);
        base.RenderSprite(e, at: relativePosition);
    }
}

public abstract class BaseCamera
{
    private readonly SpriteBatch _spriteBatch;
    protected readonly Viewport _viewport;

    protected BaseCamera(SpriteBatch spriteBatch, Viewport viewport)
    {
        _spriteBatch = spriteBatch;
        _viewport = viewport;
    }

    protected void RenderCharacterAnimator(Entity e, Vector2 at)
    {
        if (!e.Has<CharacterAnimatorComponent>())
        {
            return;
        }

        ref var animator = ref e.GetComponent<CharacterAnimatorComponent>();
        animator.Animation.Draw(_spriteBatch, at);
    }

    protected void RenderSprite(Entity e, Vector2 at)
    {
        if (!e.Has<SpriteComponent>())
        {
            return;
        }

        ref var spriteComponent = ref e.GetComponent<SpriteComponent>();
        spriteComponent.Sprite.Draw(_spriteBatch, at);
    }
}
