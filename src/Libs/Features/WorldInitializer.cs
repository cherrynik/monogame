using Entities.Factories.Characters;
using Entities.Factories.Meta;
using Scellecs.Morpeh;

namespace Features;

public class WorldInitializer : IInitializer
{
    public World World { get; set; }
    private readonly WorldEntityFactory _worldEntityFactory;
    private readonly PlayerEntityFactory _playerEntityFactory;
    private readonly DummyEntityFactory _dummyEntityFactory;

    public WorldInitializer(World world,
        WorldEntityFactory worldEntityFactory,
        PlayerEntityFactory playerEntityFactory,
        DummyEntityFactory dummyEntityFactory)
    {
        World = world;
        _worldEntityFactory = worldEntityFactory;
        _playerEntityFactory = playerEntityFactory;
        _dummyEntityFactory = dummyEntityFactory;
    }

    public void OnAwake()
    {
        _worldEntityFactory.CreateEntity(@in: World);
        _playerEntityFactory.CreateEntity(@in: World);
        _dummyEntityFactory.CreateEntity(@in: World);
    }

    public void Dispose()
    {
    }
}
