using Entitas.Entities;
using Services.Factories;

namespace Features;

public class WorldInitializeFeature : Entitas.Extended.Feature
{
    private readonly AbstractFactory<Player> _playerFactory;
    private readonly AbstractFactory<StaticEntity> _staticEntityFactory;

    public WorldInitializeFeature(AbstractFactory<Player> playerFactory,
        AbstractFactory<StaticEntity> staticEntityFactory)
    {
        _playerFactory = playerFactory;
        _staticEntityFactory = staticEntityFactory;
    }

    public override void Initialize()
    {
        _playerFactory.Create();
        _staticEntityFactory.Create();
    }
}
