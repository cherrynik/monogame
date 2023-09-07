using Entitas;
using Vector2 = System.Numerics.Vector2;

namespace Components;

public class TransformComponent : IComponent
{
    public Vector2 Position;
}

public sealed class CreateEntitySystem : IInitializeSystem, IExecuteSystem
{
    private readonly Contexts _contexts;

    public CreateEntitySystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        _contexts.game.CreateEntity();
        Console.WriteLine("Initialized.");
    }

    public void Execute()
    {
        Console.WriteLine("Executing...");
    }

}
