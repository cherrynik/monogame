using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
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

                animator.Animation.Draw(spriteBatch, at);
            }

            if (e.Has<SpriteComponent>())
            {
                ref var sprite = ref e.GetComponent<SpriteComponent>();

                sprite.Sprite.Draw(spriteBatch, at);
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
            return transform.Position.Y;
        });
    }
}
