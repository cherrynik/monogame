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
    private Filter _transformFilter = default!;
    public World World { get; set; }

    public PivotRenderSystem(World world, SpriteBatch spriteBatch, Texture2D pixel)
    {
        _spriteBatch = spriteBatch;
        _pixel = pixel;
        World = world;
    }

    public void OnAwake()
    {
        _transformFilter = World.Filter.With<TransformComponent>().Build();
    }

    public void OnUpdate(float deltaTime)
    {
        var transformStash = World.GetStash<TransformComponent>();

        foreach (Entity e in _transformFilter)
        {
            ref var transform = ref transformStash.Get(e);

            _spriteBatch.Draw(texture: _pixel, position: transform.Position, color: Color.Gold);
        }
    }

    public void Dispose()
    {
    }
}
