namespace Systems;

using Entitas;

public sealed class CreateEntitySystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    public CreateEntitySystem(Contexts contexts)
    {
        this._contexts = contexts;
    }
    
    public void Initialize()
    {
        _contexts.game.CreateEntity();
    }
}

public sealed class Class1 : Feature
{
    public Class1(Contexts contexts)
    {
        Add(new CreateEntitySystem(contexts));
    }
}
