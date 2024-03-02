using Components.Data;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Color = Microsoft.Xna.Framework.Color;

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

            spriteBatch.Draw(texture: pixel, position: camera.WorldToScreen(transform.Position), color: Color.Gold);
        }
    }

    public void Dispose()
    {
    }
}
