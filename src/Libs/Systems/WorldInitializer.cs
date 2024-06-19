using Entities.Factories;
using LDtk;
using Scellecs.Morpeh;

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
        // FIXME: Duplicated at TilesRenderingSystem
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
                abstractEntityFactory.CreateEntity(entity, world);
                // var e = rockEntityFactory.CreateEntity(World);
                // ref var transform = ref e.GetComponent<TransformComponent>();

                // transform.Pivot = MathUtils.LdtkPivotToSector(new Vector2(entity._Pivot.X, entity._Pivot.Y));
                // var pivotOffset = MathUtils.SectorToVector(transform.Pivot);
            }
        }
    }

    public void Dispose()
    {
    }
}
