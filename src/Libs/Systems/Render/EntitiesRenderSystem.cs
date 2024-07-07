using Components.Data;
using Components.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Scellecs.Morpeh.Extended.Extensions;

namespace Systems.Render;

public class EntitiesRenderSystem(World world, SpriteBatch spriteBatch) : IRenderSystem
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

        var worldMetaFilter = World.Filter.With<WorldMetaComponent>().Build().First();

        ref var worldMeta = ref worldMetaFilter.GetComponent<WorldMetaComponent>();
        worldMeta.SortedEntities = transformFilter.AsEnumerableSlow()
            .OrderBy(x => x.GetComponent<TransformComponent>().Position.Y);

        // TODO: Refactor
        foreach (Entity e in worldMeta.SortedEntities)
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
                sprite.Sprite.Draw(spriteBatch,
                    new Vector2((int)System.Math.Round(position.X), (int)System.Math.Round(position.Y)));
            }
        }
    }

    public void Dispose()
    {
    }
}
