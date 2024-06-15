using Components.Data;
using Ldtk;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Serilog;
using Serilog.Core;
using World = Scellecs.Morpeh.World;

namespace Systems.Render;

public class TilesRenderingSystem(World world, SpriteBatch spriteBatch, LdtkData ldtkData) : ILateSystem
{
    public World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        var camera = World.Filter
            .With<CameraComponent>()
            .Build()
            .First()
            .GetComponent<CameraComponent>();

        RenderLevel(ldtkData.Levels.First());
        // foreach (Entity e in entities)
        // {
        //     ref var transform = ref e.GetComponent<TransformComponent>();
        //     var at = camera.WorldToScreen(transform.Position);
        // }
    }

    private static void RenderLevel(Level level)
    {
        switch (level.LayerInstances)
        {
            case null when level.ExternalRelPath is not null:
                throw new Exception("Level has not been loaded.");
            case null:
                throw new Exception("Level has no layers.");
        }

        for (int i = level.LayerInstances.Length - 1; i >= 0; --i)
        {
            LayerInstance layer = level.LayerInstances[i];

            if (layer.TilesetRelPath is null)
            {
                continue;
            }

            switch (layer.Type)
            {
                case TypeEnum.Entities:
                    continue;
                case TypeEnum.AutoLayer:
                    break;
                case TypeEnum.IntGrid:
                    break;
                case TypeEnum.Tiles:
                    break;
            }
        }
    }

    public void Dispose()
    {
    }
}
