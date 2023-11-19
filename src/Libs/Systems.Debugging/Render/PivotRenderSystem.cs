using Components.Data;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Color = Microsoft.Xna.Framework.Color;

namespace Systems.Debugging.Render;

public class PivotRenderSystem : IRenderSystem
{
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _pixel;
    public World World { get; set; }

    public PivotRenderSystem(World world, SpriteBatch spriteBatch, Texture2D pixel)
    {
        _spriteBatch = spriteBatch;
        _pixel = pixel;
        World = world;
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter.With<TransformComponent>().Build();

        foreach (Entity e in filter)
        {
            ref var transform = ref e.GetComponent<TransformComponent>();

            _spriteBatch.Draw(texture: _pixel, position: transform.Position, color: Color.Gold);
        }
    }

    public void Dispose()
    {
    }
}
