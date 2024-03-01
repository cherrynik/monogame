using Systems;
using UI.Blocks;

namespace UI.Factories;

public class UIFactory(
    Func<Counter> counterFactory,
    Func<GameVersion> gameVersionFactory,
    CollisionSystem collisionSystem)
{
    public UIFactory AddTriggerCounter()
    {
        var counter = counterFactory();

        counter.UpdateText();

        collisionSystem.RaiseTriggerIntersect += (_, _) =>
        {
            ++counter.Count;
            counter.UpdateText();
        };

        return this;
    }


    public UIFactory AddGameVersion()
    {
        gameVersionFactory();
        return this;
    }
}
