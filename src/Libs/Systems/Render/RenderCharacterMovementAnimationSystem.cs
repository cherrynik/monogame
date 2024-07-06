using Components.Data;
using Components.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Render;

public class RenderCharacterMovementAnimationSystem(World world, SpriteBatch spriteBatch) : IRenderSystem
{
    public World World { get; set; } = world;


    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter transformFilter = World.Filter.With<TransformComponent>().Build();

        if (transformFilter.IsEmpty()) return;

        Filter cameraFilter = World.Filter
            .With<CameraComponent>()
            .Build();

        if (cameraFilter.IsEmpty()) return;

        var camera = cameraFilter.First()
            .GetComponent<CameraComponent>();

        IEnumerable<Entity> entities = SortEntitiesByYPosition(transformFilter);

        foreach (Entity e in entities)
        {
            ref var transform = ref e.GetComponent<TransformComponent>();
            var at = camera.WorldToScreen(transform.Position);

            if (e.Has<CharacterAnimatorComponent>())
            {
                ref var animator = ref e.GetComponent<CharacterAnimatorComponent>();
                var pivotOffset = animator.GetOffPivot(transform);
                var localTransform = animator.LocalTransform;
                var position = at + localTransform.Position - pivotOffset;

                // casting to int for pixel perfect matching
                animator.Animation.Draw(spriteBatch,
                    new Vector2((int)System.Math.Round(position.X), (int)System.Math.Round(position.Y)));
            }

            if (e.Has<SpriteComponent>())
            {
                ref var sprite = ref e.GetComponent<SpriteComponent>();
                var pivotOffset = sprite.GetOffPivot(transform);
                var localTransform = sprite.LocalTransform;
                var position = at + localTransform.Position - pivotOffset;

                // casting to int for pixel perfect matching
                sprite.Sprite.Draw(spriteBatch, new Vector2((int)System.Math.Round(position.X), (int)System.Math.Round(position.Y)));
            }
        }
    }

    public void Dispose()
    {
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

            // if (x.Has<SpriteComponent>())
            // {
            //     ref var sprite = ref x.GetComponent<SpriteComponent>();
            //     var localTransform = sprite.LocalTransform;
            //     return transform.Position.Y + localTransform.Position.Y -
            //            transform.GetOffPivot(sprite.Sprite.Width, sprite.Sprite.Height).Y;
            // }
            //
            // if (x.Has<CharacterAnimatorComponent>())
            // {
            //     ref var animator = ref x.GetComponent<CharacterAnimatorComponent>();
            //     var localTransform = animator.LocalTransform;
            //
            //     return transform.Position.Y + localTransform.Position.Y -
            //            transform.GetOffPivot(animator.Animation.Width, animator.Animation.Height).Y;
            // }

            return transform.Position.Y;
        });
    }
}
