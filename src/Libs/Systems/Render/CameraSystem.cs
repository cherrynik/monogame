using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Systems.Render;

public interface ICamera
{
    void Render(Entity e);
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

    public FollowingCamera(SpriteBatch spriteBatch, Viewport viewport) : base(spriteBatch, viewport)
    {
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
            _position = GetCenteredPosition(off: position);
        }

        Vector2 relativePosition = position - _position;

        base.RenderCharacterAnimator(e, at: relativePosition);
        base.RenderSprite(e, at: relativePosition);
    }

    private Vector2 GetCenteredPosition(Vector2 off) => new(
        off.X - _viewport.Width / 2,
        off.Y - _viewport.Height / 2);
}

public class CameraSystem : ISystem
{
    private readonly ICamera _camera;
    public World World { get; set; }

    public CameraSystem(World world, ICamera camera)
    {
        World = world;
        _camera = camera;
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        // TODO: Components in the range of visibility (world grid system?)
        Filter filter = World.Filter.With<TransformComponent>().Build();

        IEnumerable<Entity> entities = SortEntitiesByYPosition(filter);

        foreach (Entity e in entities)
        {
            _camera.Render(e);
        }
    }

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

// using Entitas;
// using Entitas.Extended;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
//
// namespace Systems;
//
// public class CameraFollowingSystem : IDrawSystem
// {
//     private readonly Contexts _contexts;
//     private readonly IGroup<GameEntity> _group;
//
//     public CameraFollowingSystem(Contexts contexts, IGroup<GameEntity> group)
//     {
//         _contexts = contexts;
//         _group = group;
//     }
//
//     // todo: refactor, put the logic in impl
//     // todo: smooth diagonal movement
//     private Vector2 GetPosition(GameEntity target) =>
//         new(
//             (float)target.camera.Size.Width / 2 -
//             (float)target.movementAnimation.PlayingAnimation.Width / 2,
//             (float)target.camera.Size.Height / 2 -
//             (float)target.movementAnimation.PlayingAnimation.Height / 2);
//
//     public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
//     {
//         GameEntity[] entities = _group.GetEntities();
//
//         spriteBatch.Begin(samplerState: SamplerState.PointWrap);
//
//         var target = _contexts.game.cameraEntity;
//         foreach (GameEntity e in entities)
//         {
//             Vector2 otherAt = e.transform.Position - (target?.transform.Position ?? Vector2.Zero);
//             if (e.hasSprite)
//             {
//                 e.sprite.Sprite.Draw(spriteBatch, otherAt);
//                 // todo: drawing complex entities' sprite/animated components
//             }
//
//             target?.movementAnimation.PlayingAnimation.Draw(spriteBatch, GetPosition(target));
//         }
//
//         spriteBatch.End();
//     }
// }
