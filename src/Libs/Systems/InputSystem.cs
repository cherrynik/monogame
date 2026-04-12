using Components;
using Components.Data;
using Components.Tags;
using Scellecs.Morpeh;
using Systems.Input.Abstractions;

namespace Systems;

public class InputSystem : ISystem
{
    private readonly IInputScanner _inputScanner;
    private Filter _filter = default!;
    public World World { get; set; }

    public InputSystem(World world, IInputScanner inputScanner)
    {
        World = world;
        _inputScanner = inputScanner;
    }

    public void OnAwake()
    {
        _filter = World.Filter
            .With<InputMovableComponent>()
            .With<TransformComponent>()
            .Build();
    }

    public void OnUpdate(float deltaTime)
    {
        var transformStash = World.GetStash<TransformComponent>();

        foreach (Entity e in _filter)
        {
            ref TransformComponent transform = ref transformStash.Get(e);
            transform.Velocity = _inputScanner.GetDirection();
        }
    }

    public void Dispose()
    {
    }
}
