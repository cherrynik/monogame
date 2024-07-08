using Components.Data;
using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Vector2 = System.Numerics.Vector2;
using World = Scellecs.Morpeh.World;

namespace Systems.Render;

public class RenderTilesSystem(World world, SpriteBatch spriteBatch, LDtkFile ldtkFile) : IRenderSystem
{
    public World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        // TODO: Refactor
        Filter filter = World.Filter
            .With<CameraComponent>()
            .Build();

        if (filter.IsEmpty()) return;

        var camera = filter
            .First()
            .GetComponent<CameraComponent>();

        // FIXME: Duplicated at WorldInitializer
        var world = ldtkFile.LoadWorld(ldtkFile.Worlds.First().Iid);
        var level = world.LoadLevel(0);

        RenderLevel(level, camera);
        // foreach (Entity e in entities)
        // {
        //     ref var transform = ref e.GetComponent<TransformComponent>();
        //     var at = camera.WorldToScreen(transform.Position);
        // }
    }

    private void RenderLevel(LDtkLevel level, CameraComponent camera)
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

            if (layer._TilesetRelPath is null)
            {
                continue;
            }

            switch (layer._Type)
            {
                case LayerType.Entities:
                    continue;
                case LayerType.AutoLayer:
                case LayerType.IntGrid:
                case LayerType.Tiles:
                    break;
            }

            // TODO: Cache
            Texture2D texture = GetTexture(level, layer._TilesetRelPath);
            // int width = layer._CWid * layer._GridSize;
            // int height = layer._CHei * layer._GridSize;
            // RenderTarget2D renderTarget = new(spriteBatch.GraphicsDevice, width, height, false, SurfaceFormat.Color,
            // DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

            // spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);

            switch (layer._Type)
            {
                case LayerType.Tiles:
                    // foreach (TileInstance tile in layer.GridTiles.Where(_ => layer._TilesetDefUid.HasValue))
                    // {
                    //     Vector2 tilePos = new(tile.Px.X + layer._PxTotalOffsetX, tile.Px.Y + layer._PxTotalOffsetY);
                    //     var position = camera.WorldToScreen(tilePos);
                    //     Rectangle rect = new(tile.Src.X, tile.Src.Y, layer._GridSize, layer._GridSize);
                    //     SpriteEffects mirror = (SpriteEffects)tile.F;
                    //     spriteBatch.Draw(texture, position, rect, new Color(1f, 1f, 1f, layer._Opacity), 0,
                    //         new Vector2(0, 0), 1f, mirror, 0);
                    // }

                    break;

                case LayerType.AutoLayer:
                case LayerType.IntGrid:
                    if (layer.AutoLayerTiles.Length > 0)
                    {
                        foreach (TileInstance tile in layer.AutoLayerTiles.Where(_ => layer._TilesetDefUid.HasValue))
                        {
                            Vector2 tilePos = new(tile.Px.X + layer._PxTotalOffsetX,
                                tile.Px.Y + layer._PxTotalOffsetY);
                            var camPos = camera.WorldToScreen(tilePos);
                            // casting to int for pixel perfect matching
                            var position = new Microsoft.Xna.Framework.Vector2((int)System.Math.Round(camPos.X),
                                (int)System.Math.Round(camPos.Y));
                            Rectangle rect = new(tile.Src.X, tile.Src.Y, layer._GridSize, layer._GridSize);
                            SpriteEffects mirror = (SpriteEffects)tile.F;
                            spriteBatch.Draw(texture, position, rect, new Color(Color.White, layer._Opacity), 0,
                                Vector2.Zero, 1f, mirror, 0);
                        }
                    }

                    break;

                case LayerType.Entities:
                    break;
            }
        }
    }

    private Texture2D GetTexture(LDtkLevel level, string path)
    {
        string directory = Path.GetDirectoryName(level.WorldFilePath)!;
        string fullPath = Path.Combine(directory, path);
        return Texture2D.FromFile(spriteBatch.GraphicsDevice, fullPath);
    }

    public void Dispose()
    {
    }
}
