using System.Numerics;
using Components.Data;
using Entities.Factories;
using LDtk;
using Scellecs.Morpeh;
using Services.Math;

namespace Systems;

public class WorldInitializer(
    World world,
    AbstractEntityFactory abstractEntityFactory,
    LDtkFile ldtkFile)
    : IInitializer
{
    public World World { get; set; } = world;

    public void OnAwake()
    {
        // // FIXME: Duplicated at TilesRenderingSystem
        var ldtkWorld = ldtkFile.LoadWorld(ldtkFile.Worlds.First().Iid);
        var level = ldtkWorld.LoadLevel(0);

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

            switch (layer._Type)
            {
                case LayerType.AutoLayer:
                case LayerType.IntGrid:
                case LayerType.Tiles:
                    continue;
                case LayerType.Entities:
                    break;
            }

            foreach (var entity in layer.EntityInstances)
            {
                var e = abstractEntityFactory.CreateEntity(entity, world);

                if (e is null) continue;

                ref var transform = ref e.GetComponent<TransformComponent>();
                var mappedLDtkPivot = MapAndFlip(new Vector2(entity._Pivot.X, entity._Pivot.Y));
                transform.Pivot = MathUtils.VectorToSector(mappedLDtkPivot);
                // var pivotOffset = MathUtils.SectorToVector(transform.Pivot);

                transform.Position = new Vector2((float)entity._WorldX, (float)entity._WorldY);

                if (e.Has<CameraComponent>())
                {
                    ref var camera = ref e.GetComponent<CameraComponent>();
                    camera.Position = camera.GetCenteredPosition(camera.Viewport, transform.Position);
                }
            }
        }
    }

    private static Vector2 MapAndFlip(Vector2 v)
    {
        // Define the mapping and flipping logic
        float[] mapping = [-1, 0, 1];

        // Map x component to the corresponding index in mapping array
        int indexX = (int)(v.X * (mapping.Length - 1));
        float mappedX = mapping[indexX];

        // Map y component to the corresponding index in mapping array and flip y
        int indexY = (int)(v.Y * (mapping.Length - 1));
        float mappedY = -mapping[indexY];

        return new Vector2(mappedX, mappedY);
    }

    public void Dispose()
    {
    }
}
