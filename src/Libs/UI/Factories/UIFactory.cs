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

        // collisionSystem.Stay += (_, _, isTriggerEvent) =>
        // {
        //     if (!isTriggerEvent) return;
        //
        //     ++counter.Count;
        //     counter.UpdateText();
        // };

        collisionSystem.Entered += (_, _, _) => counter.SayEntered();
        collisionSystem.Exited += (_, _) => counter.SayExited();

        return this;
    }


    public UIFactory AddGameVersion()
    {
        gameVersionFactory();
        return this;
    }
}
