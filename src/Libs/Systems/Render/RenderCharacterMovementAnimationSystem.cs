using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Systems.Render;

public class RenderCharacterMovementAnimationSystem : ISystem
{
    private readonly SpriteBatch _spriteBatch;
    public World World { get; set; }

    public RenderCharacterMovementAnimationSystem(World world, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        World = world;
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter.With<TransformComponent>().Build();
        
        var camera = World.Filter
            .With<CameraComponent>()
            .Build()
            .First()
            .GetComponent<CameraComponent>();

        IEnumerable<Entity> entities = SortEntitiesByYPosition(filter);

        foreach (Entity e in entities)
        {
            ref var transform = ref e.GetComponent<TransformComponent>();
            var at = transform.Position - camera.Position;

            if (e.Has<CharacterAnimatorComponent>())
            {
                ref var animator = ref e.GetComponent<CharacterAnimatorComponent>();

                animator.Animation.Draw(_spriteBatch, at);
            }

            if (e.Has<SpriteComponent>())
            {
                ref var sprite = ref e.GetComponent<SpriteComponent>();

                sprite.Sprite.Draw(_spriteBatch, at);
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
