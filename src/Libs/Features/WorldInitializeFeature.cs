using Features.Factories;

namespace Features;

public class WorldInitializeFeature : Entitas.Extended.Feature
{
    public WorldInitializeFeature(//AbstractFactory<Player> playerFactory,
        AbstractFactory<StaticEntity> staticEntityFactory)
    {
        //playerFactory.Create();
        staticEntityFactory.Create();
        staticEntityFactory.Create();
        staticEntityFactory.Create();
    }
}
