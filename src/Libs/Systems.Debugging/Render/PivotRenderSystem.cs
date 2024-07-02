using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Debugging.Render;

public class PivotRenderSystem(Scellecs.Morpeh.World world, SpriteBatch spriteBatch, Texture2D pixel) : IRenderSystem
{
    public Scellecs.Morpeh.World World { get; set; } = world;

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

        foreach (Entity e in filter)
        {
            ref var transform = ref e.GetComponent<TransformComponent>();

            if (e.Has<SpriteComponent>())
            {
                ref var sprite = ref e.GetComponent<SpriteComponent>();
                var pivotOffset = transform.GetOffPivot(sprite.Sprite.Width, sprite.Sprite.Height);
                spriteBatch.Draw(texture: pixel,
                    position: camera.WorldToScreen(transform.Position) + sprite.LocalTransform.Position -
                              pivotOffset,
                    color: Color.White,
                    sourceRectangle: new Rectangle(0, 0, 16, 16));
            }

            if (e.Has<CharacterAnimatorComponent>())
            {
                ref var characterAnimator = ref e.GetComponent<CharacterAnimatorComponent>();
                spriteBatch.Draw(texture: pixel,
                    position: camera.WorldToScreen(transform.Position) + characterAnimator.LocalTransform.Position -
                              transform.GetOffPivot(characterAnimator.Animation.Width,
                                  characterAnimator.Animation.Height),
                    color: Color.White,
                    sourceRectangle: new Rectangle(0, 0, 16, 16));
            }

            spriteBatch.Draw(texture: pixel,
                position: camera.WorldToScreen(transform.Position),
                color: Color.Gold);
        }
    }

    public void Dispose()
    {
    }
}
