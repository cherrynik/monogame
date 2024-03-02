using Entities.Factories.Characters;
using Entities.Factories.Items;
using Entities.Factories.Meta;
using Scellecs.Morpeh;

namespace Systems;

public class WorldInitializer(
    World world,
    WorldEntityFactory worldEntityFactory,
    PlayerEntityFactory playerEntityFactory,
    DummyEntityFactory dummyEntityFactory,
    RockEntityFactory rockEntityFactory) // TODO: Convert ctor's params into (...EntityFactory entityFactories) (?)
    : IInitializer
{
    public World World { get; set; } = world;

    public void OnAwake()
    {
        worldEntityFactory.CreateEntity(@in: World);
        playerEntityFactory.CreateEntity(@in: World);
        dummyEntityFactory.CreateEntity(@in: World);
        rockEntityFactory.CreateEntity(@in: World);
    }

    public void Dispose()
    {
    }
}
