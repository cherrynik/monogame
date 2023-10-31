using Components;
using Components.Data;
using Components.Tags;
using Implementations;
using Scellecs.Morpeh;

namespace Systems;

public class InputSystem : ISystem
{
    private readonly IInputScanner _inputScanner;
    public World World { get; set; }

    public InputSystem(IInputScanner inputScanner)
    {
        _inputScanner = inputScanner;
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter
            .With<InputMovableComponent>()
            .With<TransformComponent>()
            .Build();

        foreach (Entity e in filter)
        {
            ref TransformComponent transform = ref e.GetComponent<TransformComponent>();
            transform.Velocity = _inputScanner.GetDirection();
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
