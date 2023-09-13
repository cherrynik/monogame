using System.Numerics;
using Entitas;

namespace Systems;

public sealed class CreateEntitySystem : IInitializeSystem, IExecuteSystem
{
    private readonly Contexts _contexts;

    public CreateEntitySystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        GameEntity? e = _contexts.game.CreateEntity();

        e.isMovable = true;
        e.AddTransform(new Vector2(), new Vector2());

        Console.WriteLine("Initialized.");
    }

    public void Execute()
    {
        var entities = _contexts.game.GetEntities(Matcher<GameEntity>.AllOf(GameMatcher.Transform));
        foreach (var e in entities)
        {
            // e.transform.Position += new Vector2(1, 1);
            Console.WriteLine((e.transform.Position));
        }
    }
}
