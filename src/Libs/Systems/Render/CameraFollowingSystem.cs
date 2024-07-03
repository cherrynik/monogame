using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Vector2 = System.Numerics.Vector2;

namespace Systems.Render;

// issue: https://gamedev.stackexchange.com/questions/46963/how-to-avoid-texture-bleeding-in-a-texture-atlas
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

        Filter filter = World.Filter.With<CameraComponent>().Build();

        if (filter.IsEmpty())
            return;

        Entity e = filter.First();

        ref var transform = ref e.GetComponent<TransformComponent>();
        ref var camera = ref e.GetComponent<CameraComponent>();

        camera.Position = Vector2.Lerp(camera.Position, GetCenteredPosition(camera.Viewport, off: transform.Position),
            .25f);
        // camera.Position = Vector2.Lerp(camera.Position, transform.Position, .2f);
    }

    private static Vector2 GetCenteredPosition(Viewport viewport, Vector2 off)
    {
        var cameraX = off.X - (float)viewport.Width / 2;
        var cameraY = off.Y - (float)viewport.Height / 2;

        cameraX = MathHelper.Clamp(cameraX, 0, 900 - viewport.Width);
        cameraY = MathHelper.Clamp(cameraY, 0, 600 - viewport.Height);

        return new Vector2(cameraX, cameraY);
    }

    public void Dispose()
    {
    }
}

public interface ICamera
{
    void Render(Entity e);
}

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

public class FollowingCamera(SpriteBatch spriteBatch, Viewport viewport, Vector2 position)
    : BaseCamera(spriteBatch, viewport), ICamera
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

        Vector2 relativePosition = position1 - position;

        base.RenderCharacterAnimator(e, at: relativePosition);
        base.RenderSprite(e, at: relativePosition);
    }
}

public abstract class BaseCamera(SpriteBatch spriteBatch, Viewport viewport)
{
    protected readonly Viewport _viewport = viewport;

    protected void RenderCharacterAnimator(Entity e, Vector2 at)
    {
        if (!e.Has<CharacterAnimatorComponent>())
        {
            return;
        }

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
